#!/usr/bin/env python3
"""
IR generator for EqualizerAPO - VEFX virtual-effects system
"""

import numpy as np
import wave
import argparse


# ---------------------------------------------------------------------------
# Biquad filter helpers
# ---------------------------------------------------------------------------

def _low_shelf(sr, f0, db):
    A = 10.0 ** (db / 40.0)
    w0 = 2.0 * np.pi * f0 / sr
    alpha = np.sin(w0) / 2.0 * np.sqrt(2.0)
    c, sA = np.cos(w0), np.sqrt(A)
    b = np.array([A*((A+1)-(A-1)*c+2*sA*alpha), 2*A*((A-1)-(A+1)*c), A*((A+1)-(A-1)*c-2*sA*alpha)])
    a = np.array([(A+1)+(A-1)*c+2*sA*alpha, -2*((A-1)+(A+1)*c), (A+1)+(A-1)*c-2*sA*alpha])
    return b / a[0], a / a[0]

def _high_shelf(sr, f0, db):
    A = 10.0 ** (db / 40.0)
    w0 = 2.0 * np.pi * f0 / sr
    alpha = np.sin(w0) / 2.0 * np.sqrt(2.0)
    c, sA = np.cos(w0), np.sqrt(A)
    b = np.array([A*((A+1)+(A-1)*c+2*sA*alpha), -2*A*((A-1)+(A+1)*c), A*((A+1)+(A-1)*c-2*sA*alpha)])
    a = np.array([(A+1)-(A-1)*c+2*sA*alpha, 2*((A-1)-(A+1)*c), (A+1)-(A-1)*c-2*sA*alpha])
    return b / a[0], a / a[0]

def _peaking(sr, f0, db, Q=1.0):
    A = 10.0 ** (db / 40.0)
    w0 = 2.0 * np.pi * f0 / sr
    alpha = np.sin(w0) / (2.0 * Q)
    b = np.array([1+alpha*A, -2*np.cos(w0), 1-alpha*A])
    a = np.array([1+alpha/A, -2*np.cos(w0), 1-alpha/A])
    return b / a[0], a / a[0]


# ---------------------------------------------------------------------------
# VEFX slider conversions
# ---------------------------------------------------------------------------

def _db_to_lin(db):
    return 0.0 if db <= -80.0 else 10.0 ** (db / 20.0)

def _filter_db(s):   # slider4 - bass/treble tilt
    return s * 2 - 6 if s >= 3 else s * 6 - 18

def _eq_db(s):       # slider2/3 - 160 Hz / 2500 Hz bands
    return s - 3 if s >= 3 else s * 6 - 18


# ---------------------------------------------------------------------------
# Signal chain blocks
# ---------------------------------------------------------------------------

def _graphiceq_points(effect_type, low_eq, high_eq, filter_slider, bass_shelf=0):
    """VB temp_file[14]: 4-point GraphicEQ -> (g_low, g_160, g_2500, g_hi) dB.
    bass_shelf: dB reduction at 1 Hz, linear rolloff in log-frequency to 0 dB at 16 kHz.
    """
    f, lo, hi = _filter_db(filter_slider), _eq_db(low_eq), _eq_db(high_eq)
    pts = [f, lo, hi, f] if effect_type in (1, 2) else [f + 6, lo + 3, hi, f]
    if bass_shelf:
        ctrl_hz = [1.0, 160.0, 2500.0, 16000.0]
        log_max = np.log10(ctrl_hz[-1])
        for i, hz in enumerate(ctrl_hz):
            pts[i] -= bass_shelf * (1.0 - np.log10(hz) / log_max)
    return tuple(pts)


def _apply_graphiceq(L, R, sr, g_low, g_160, g_2500, g_hi):
    """Exact EqualizerAPO GraphicEQ: piecewise-linear gain in log-frequency space.
    Uses a linear-phase FIR; centered convolution removes the group delay."""
    from scipy import signal as sp

    if max(abs(g_low), abs(g_160), abs(g_2500), abs(g_hi)) < 0.1:
        return L, R

    nyq = sr / 2.0
    ctrl_hz = np.array([1.0, 160.0, 2500.0, 16000.0])
    ctrl_db = np.array([g_low, g_160, g_2500, g_hi], dtype=np.float64)

    # Sample the log-linear curve at 512 log-spaced frequencies
    f   = np.logspace(np.log10(1.0), np.log10(nyq * 0.9999), 512)
    db  = np.interp(np.log10(f), np.log10(ctrl_hz), ctrl_db)
    amp = 10.0 ** (db / 20.0)

    freqs = np.concatenate([[0.0],   f / nyq, [1.0]])
    amps  = np.concatenate([[amp[0]], amp,    [amp[-1]]])

    n_taps = 2049
    fir    = sp.firwin2(n_taps, freqs, amps)

    half = (n_taps - 1) // 2
    n    = len(L)
    L    = sp.fftconvolve(L, fir)[half:half + n].astype(np.float64)
    R    = sp.fftconvolve(R, fir)[half:half + n].astype(np.float64)
    return L, R


def _blend_params(effect_type, s):
    """(blend_delay_ms, direct_weight, blend_weight, apply_bass_eq)."""
    if effect_type == 1:
        dw = (50 + 50/6*(7-s)) / 100.0 if s >= 3 else 0.83
        return 0.5, dw, 1.0-dw, True
    if effect_type == 2:
        if s >= 3: return 5.0, 0.66 if s == 6 else 0.75, 0.34 if s == 6 else 0.25, True
        return 0.5, 0.83, 0.17, True
    if effect_type == 3: return 0.0, 0.83, 0.17, True
    if effect_type == 4:
        dw = (50 + 50/6*(6-s))/100.0 if s >= 3 else (50 + 50/6*s)/100.0
        return 33.0, dw, 1.0-dw, False
    if effect_type == 5: return 0.0, 0.58, 0.41, False
    return 0.0, 1.0, 0.0, False   # type 6: EQ only


def _main_preamp_db(effect_type, s):
    """temp_file[22] - L1/R1 preamp before blend."""
    if effect_type == 1: return float(s-3) if s >= 3 else 0.0
    if effect_type == 2: return 3.0        if s >= 3 else 0.0
    if effect_type == 3: return float(s-6) if s >= 3 else float(-s)
    if effect_type == 4: return float(s-3) if s >= 3 else float(6-s*2)
    return 0.0


def _reverb_taps(effect_type, s):
    """
    Returns (left_taps, right_taps) as separate [(delay_ms, amplitude), ...] lists.

    The audio chain has a "reverb only delay" on R2/R12 that creates stereo ping-pong:
      type 1 (COMPRESSOR): +0 ms  -> L and R taps identical
      type 2 (ECHO)      : +20 ms -> L reverb at 20 ms, R reverb at 40 ms
      type 3 (ECHO EX)   : +20 ms -> same asymmetry
      type 4 (CHORUS)    : +33 ms -> L at 20 ms, R at 53 ms

    Echo copies alternate cross-channel (L(k)=R(k-1), R(k)=L(k-1) for k >= 2),
    so L and R bounce to opposite delay positions at each step beyond the first.
    """
    THRESH = -80.0

    # right_offset: the extra ms added to R2/R12 by the "reverb only delay" block
    right_offset = {1: 0.0, 2: 20.0, 3: 20.0, 4: 33.0, 5: 0.0}.get(effect_type, 0.0)

    if effect_type == 1:
        t = (20.0, _db_to_lin(-12.0))
        return [t], [t]

    if effect_type in (2, 3):
        mx_rev  = -6.0 if s >= 3 else 0.0
        if effect_type == 2:
            mx_echo = float(s*4-12)  if s >= 3 else float(12-s*4)
            step0, step_fn = 40.0, lambda x: x * 2.0
        else:
            mx_echo = float(s*6-33)  if s >= 3 else float(3-s*6)
            step0, step_fn = float(int(240.0*s/6.0 + 80.0)), lambda x: x

        rev_amp = _db_to_lin(-6 + mx_rev)
        L_taps = [(20.0,                  rev_amp)]
        R_taps = [(20.0 + right_offset,   rev_amp)]
        dL, dR, step, cum, k = 20.0, 20.0 + right_offset, step0, -6.0, 0
        while True:
            cum -= (9 + k*3)
            if cum + mx_echo <= THRESH: break
            amp = _db_to_lin(cum + mx_echo)
            if k == 0:                    # L3=L2, R3=R2: straight copy, no cross
                new_dL, new_dR = dL + step, dR + step
            else:                         # L(k+3)=R(k+2), R(k+3)=L(k+2): cross
                new_dL, new_dR = dR + step, dL + step
            L_taps.append((new_dL, amp))
            R_taps.append((new_dR, amp))
            dL, dR = new_dL, new_dR
            step = step_fn(step)
            k += 1
        return L_taps, R_taps

    if effect_type == 4 and s >= 3:   # CHORUS only; FLANGER has no reverb
        amp = _db_to_lin(-6.0)
        return [(20.0, amp)], [(20.0 + right_offset, amp)]

    return [], []


# ---------------------------------------------------------------------------
# Main IR generator
# ---------------------------------------------------------------------------

def generate_ir(
    effect_type=1, effect_depth=3, low_eq=3, high_eq=3,
    filter_slider=3, volume=3, channel=1,
    sample_rate=48000, duration=None, bass_shelf=12,
):
    """
    Generate a convolution IR from VEFX slider values.

    effect_type   1=COMPRESSOR/REVERB  2=ECHO/REVERB  3=ECHO EX/REVERB EX
                  4=CHORUS/FLANGER     5=GARGLE/DISTORTION  6=EQ ONLY
    effect_depth  0-6  ('slider')
    low_eq        0-6  160 Hz band     ('slider2')
    high_eq       0-6  2500 Hz band    ('slider3')
    filter_slider 0-6  bass/treble     ('slider4')
    volume/channel     not encoded in the IR (kept for API parity)

    Reverb/echo taps loop to natural silence (-80 dB).
    A smooth exponential tail fills in the density beyond discrete taps.
    """
    from scipy import signal as sp

    s = effect_depth

    if duration is None:
        if effect_type == 3:
            duration = 20/1000 + int(240*s/6+80)/1000*6 + 0.1
        elif effect_type == 2:
            duration = 3.0
        elif effect_type in (1, 4):
            duration = 0.5
        else:
            duration = 0.1

    n = int(duration * sample_rate)
    L = np.zeros(n, dtype=np.float64)
    R = np.zeros(n, dtype=np.float64)

    main_lin = 10.0 ** (_main_preamp_db(effect_type, s) / 20.0)
    delay_ms, dw, bw, use_bass_eq = _blend_params(effect_type, s)

    L[0] = dw * main_lin
    R[0] = dw * main_lin

    if bw > 0.01:
        bi = int(delay_ms * sample_rate / 1000.0)
        if 0 <= bi < n:
            bt = np.zeros(n)
            bt[bi] = bw * main_lin
            if use_bass_eq:
                # GraphicEQ 1 12; 160 12; 250 6; 2500 -6
                b, a = _low_shelf(sample_rate, 200.0, 12.0)
                bt = sp.lfilter(b, a, bt)
                b, a = _peaking(sample_rate, 2500.0, -6.0, Q=0.7)
                bt = sp.lfilter(b, a, bt)
            L += bt; R += bt

    L_taps, R_taps = _reverb_taps(effect_type, s)
    blend_offset = int(delay_ms * sample_rate / 1000.0)

    def _place_taps_with_blend(taps_L, taps_R, apply_lpf):
        """
        Each reverb/echo tap inherits the blend comb-filter because L2=R1 takes the
        already-blended L1. Split every tap into direct (dw) + blend (bw) components
        so the echo carries the same comb character as the direct signal.
        """
        direct_L = np.zeros(n, dtype=np.float64)
        direct_R = np.zeros(n, dtype=np.float64)
        blend_src_L = np.zeros(n, dtype=np.float64)
        blend_src_R = np.zeros(n, dtype=np.float64)

        for (ms, amp), (ms_r, amp_r) in zip(taps_L, taps_R):
            Li = int(ms   * sample_rate / 1000.0)
            Ri = int(ms_r * sample_rate / 1000.0)
            if Li < n: direct_L[Li] += amp * dw
            if Ri < n: direct_R[Ri] += amp_r * dw
            if bw > 0.01:
                Lb = Li + blend_offset
                Rb = Ri + blend_offset
                if Lb < n: blend_src_L[Lb] += amp * bw
                if Rb < n: blend_src_R[Rb] += amp_r * bw

        # Apply bass EQ to blend components (GraphicEQ 1 12; 160 12; 250 6; 2500 -6)
        if use_bass_eq and bw > 0.01:
            b, a = _low_shelf(sample_rate, 200.0, 12.0)
            blend_src_L = sp.lfilter(b, a, blend_src_L)
            blend_src_R = sp.lfilter(b, a, blend_src_R)
            b, a = _peaking(sample_rate, 2500.0, -6.0, Q=0.7)
            blend_src_L = sp.lfilter(b, a, blend_src_L)
            blend_src_R = sp.lfilter(b, a, blend_src_R)

        # Echo taps: separate bass and upper speaker path filters.
        # Bass path (L3 series): LPF at 160 Hz
        #   GraphicEQ: 1 0; 160 0; 161 -57; 40000 -57
        # Upper path, type 2 (ECHO):     BPF 250-400 Hz
        #   speaker HPF 250 Hz + GraphicEQ LPF 400 Hz  →  BPF 250-400 Hz
        # Upper path, type 3 (ECHO EX):  HPF 400 Hz
        #   speaker HPF 250 Hz + GraphicEQ HPF 400 Hz  →  net HPF 400 Hz
        # Echo has a gap at 160-250 Hz (type 2) or 160-400 Hz (type 3)
        if apply_lpf and effect_type in (2, 3):
            sos_bass = sp.butter(4, 160.0 / (sample_rate / 2.0), btype='low', output='sos')
            if effect_type == 2:
                sos_upper = sp.butter(2, [250.0, 400.0], btype='bandpass',
                                      fs=sample_rate, output='sos')
            else:
                sos_upper = sp.butter(4, 400.0 / (sample_rate / 2.0), btype='high', output='sos')

            def _band(a):
                return sp.sosfilt(sos_bass, a) + sp.sosfilt(sos_upper, a)

            direct_L    = _band(direct_L)
            direct_R    = _band(direct_R)
            blend_src_L = _band(blend_src_L)
            blend_src_R = _band(blend_src_R)

        return direct_L + blend_src_L, direct_R + blend_src_R

    # Reverb tap (first): full-range
    if L_taps and R_taps:
        rL, rR = _place_taps_with_blend(L_taps[:1], R_taps[:1], apply_lpf=False)
        L += rL; R += rR

    # Echo taps (1+): LPF applied
    if len(L_taps) > 1:
        eL, eR = _place_taps_with_blend(L_taps[1:], R_taps[1:], apply_lpf=True)
        L += eL; R += eR

    g_low, g_160, g_2500, g_hi = _graphiceq_points(effect_type, low_eq, high_eq, filter_slider, bass_shelf)
    L, R = _apply_graphiceq(L, R, sample_rate, g_low, g_160, g_2500, g_hi)

    # Distortion low-pass (type 5, depth < 4)
    if effect_type == 5 and s < 4:
        cutoff = min(2500.0 + s*1833.0, sample_rate*0.45)
        sos = sp.butter(4, cutoff/(sample_rate/2.0), btype='low', output='sos')
        L = sp.sosfilt(sos, L); R = sp.sosfilt(sos, R)

    peak = max(np.max(np.abs(L)), np.max(np.abs(R)))
    if peak > 0:
        L = (L * 0.9 / peak).astype(np.float32)
        R = (R * 0.9 / peak).astype(np.float32)
    else:
        L = L.astype(np.float32)
        R = R.astype(np.float32)

    return L, R, sample_rate


# ---------------------------------------------------------------------------
# WAV output + CLI
# ---------------------------------------------------------------------------

def save_wav(filename, left, right, sample_rate):
    stereo = np.empty(len(left) * 2, dtype=np.int16)
    stereo[0::2] = np.int16(left  * 32767)
    stereo[1::2] = np.int16(right * 32767)
    with wave.open(filename, 'w') as f:
        f.setnchannels(2); f.setsampwidth(2); f.setframerate(sample_rate)
        f.writeframes(stereo.tobytes())
    print('OK {}  {:.2f}s  {} Hz  stereo'.format(filename, len(left)/sample_rate, sample_rate))


if __name__ == '__main__':
    p = argparse.ArgumentParser(
        description='VEFX IR generator',
        formatter_class=argparse.RawTextHelpFormatter)
    p.add_argument('--effect-type',  '--et',  type=int, choices=range(1,7), default=1, metavar='1-6',
                   help='1=COMPRESSOR  2=ECHO  3=ECHO EX  4=CHORUS  5=GARGLE  6=EQ ONLY')
    p.add_argument('--effect-depth', '--ed',  type=int, choices=range(7),   default=3, metavar='0-6')
    p.add_argument('--low-eq',       '--le',  type=int, choices=range(7),   default=3, metavar='0-6')
    p.add_argument('--high-eq',      '--he',  type=int, choices=range(7),   default=3, metavar='0-6')
    p.add_argument('--filter',       '--fi',  type=int, choices=range(7),   default=3, metavar='0-6',
                   help='Bass/treble tilt slider')
    p.add_argument('--volume',       '--vol', type=int, choices=range(7),   default=3, metavar='0-6')
    p.add_argument('--channel',      '--chan',type=int, choices=range(7),   default=1, metavar='0-6')
    p.add_argument('--sample-rate',  '-s',    type=int, default=48000, choices=[44100, 48000, 96000])
    p.add_argument('--duration',     '-d',    type=float, default=None)
    p.add_argument('--output',       '-o',    type=str)
    p.add_argument('--vefx',         type=str, metavar='ET ED LE HE FI VOL CHAN BGFX',
                   help='Parse preset string "et ed le he fi vol chan bgfx" (e.g., "1 5 4 5 6 5 1 0")')
    p.add_argument('--bass-shelf',   type=float, default=12.0, metavar='dB',
                   help='Bass rolloff: dB reduction at 1 Hz, linear to 0 dB at 160 Hz (default 12)')
    args = p.parse_args()

    # Parse --vefx preset string if provided
    if args.vefx:
        parts = args.vefx.split()
        if len(parts) >= 7:
            args.effect_type = int(parts[0])
            args.effect_depth = int(parts[1])
            args.low_eq = int(parts[2])
            args.high_eq = int(parts[3])
            args.filter = int(parts[4])
            args.volume = int(parts[5])
            args.channel = int(parts[6])

    et, ed = args.effect_type, args.effect_depth
    out = args.output or 'ir_et{}_ed{}_lo{}_hi{}_fi{}.wav'.format(et, ed, args.low_eq, args.high_eq, args.filter)
    L, R, sr = generate_ir(
        effect_type=et, effect_depth=ed, low_eq=args.low_eq, high_eq=args.high_eq,
        filter_slider=args.filter, volume=args.volume, channel=args.channel,
        sample_rate=args.sample_rate, duration=args.duration, bass_shelf=args.bass_shelf,
    )
    save_wav(out, L, R, sr)
    print('  Convolution: {}'.format(out))
