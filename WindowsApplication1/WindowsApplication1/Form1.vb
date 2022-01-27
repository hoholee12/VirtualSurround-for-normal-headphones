Imports System.Text.RegularExpressions

Public Class Form1
    Public Shared effector_num As Integer = 1
    Public Shared effector_on As Integer = 0
    Public Shared echo_texts = New String() {"REVERB 3", "REVERB 2", "REVERB 1", "ECHO 1", "ECHO 2", "ECHO 3", "ECHO 4"}
    Public Shared echo_ex_texts = New String() {"REVERB EX 3", "REVERB EX 2", "REVERB EX 1", "ECHO EX 1", "ECHO EX 2", "ECHO EX 3", "ECHO EX 4"}
    Public Shared compressor_texts = New String() {"COMPRESSOR 1", "     COMPRESSOR 1      written by manual...as a fan project for beatmania series...have fun!", "COMPRESSOR 1", "COMPRESSOR 1", "COMPRESSOR 2", "COMPRESSOR 3", "COMPRESSOR 4"}
    Public Shared chorus_texts = New String() {"FLANGER 3", "FLANGER 2", "FLANGER 1", "CHORUS 1", "CHORUS 2", "CHORUS 3", "CHORUS 4"}
    Public Shared gargle_texts = New String() {"DISTORTION 4", "DISTORTION 3", "DISTORTION 2", "DISTORTION 1", "GARGLE 1", "GARGLE 2", "GARGLE 3"}
    Public Shared eq_only_texts = New String() {"EQ ONLY"}
    Public Shared loweq_texts = New String() {"LOW EQ -3", "LOW EQ -2", "LOW EQ -1", "LOW EQ +0", "LOW EQ +1", "LOW EQ +2", "LOW EQ +3"}
    Public Shared hieq_texts = New String() {"HI EQ -3", "HI EQ -2", "HI EQ -1", "HI EQ +0", "HI EQ +1", "HI EQ +2", "HI EQ +3"}
    Public Shared filter_texts = New String() {"FILTER -3", "FILTER -2", "FILTER -1", "FILTER +0", "FILTER +1", "FILTER +2", "FILTER +3"}
    Public Shared vol_texts = New String() {"VOL -3", "VOL -2", "VOL -1", "VOL +0", "VOL +1", "VOL +2", "VOL +3"}
    Public Shared channel_texts = New String() {"MONO", "STEREO", "QUADRAPHONIC", "SURROUND", "5.1 SRND", "6.1 SRND", "7.1 SRND"}
    Public Shared effector_slider As Integer = 3
    Public Shared loweq_slider As Integer = 3
    Public Shared prev_loweq As Integer = loweq_slider
    Public Shared hieq_slider As Integer = 3
    Public Shared prev_hieq As Integer = hieq_slider
    Public Shared filter_slider As Integer = 3
    Public Shared prev_filter As Integer = filter_slider
    Public Shared vol_slider As Integer = 3
    Public Shared prev_vol As Integer = vol_slider
    Public Shared channel_slider As Integer = 1
    Public Shared prev_channel As Integer = channel_slider
    Public Shared check_flag As Boolean = False

    Public Shared wait_for_thread As Boolean = False
    Public Shared wait_for_thread2 As Boolean = False

    Public Shared bgfx_toggleb As Integer = 1

    Public Shared temp_thread As System.Threading.Thread
    Public Shared menu_thread As System.Threading.Thread
    Public Shared temp_file() As String
    Public Shared temp_file2() As String
    Public Shared eq_only_file(124) As String
    Public Shared gargle_file = New String() {
"",
"#GARGLE",
"",
"Preamp: 0dB",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"Device: all",
"#DELAY SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"#L1 R1: BASS SPEAKER SYSTEM",
"#L11 R11: UPPER SPEAKER SYSTEM",
"",
"Channel: L1 R1",
"Preamp: 0dB",
"",
"Copy: L99=L1 R99=R1",
"Channel: L99 R99",
"#GraphicEQ: 1 12; 160 12; 250 6; 2500 -6",
"Delay: 0ms",
"#",
"# LEVEL 1: 83/17",
"# LEVEL 2: 80/20",
"# LEVEL 3: 75/25",
"# LEVEL 4: 67/33",
"Copy: L1=0.58*L1+0.41*L99 R1=0.58*R1+0.41*R99",
"Copy: L11=L1 R11=R1",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#SURROUND SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"Channel: L1 R1",
"GraphicEQ: 1 0; 250 0; 251 -57; 40000 -57",
"Delay: 0ms",
"Preamp: -0dB",
"Channel: L11 R11",
"GraphicEQ: 1 -57; 250 -57; 251 0; 40000 0",
"Delay: 0ms",
"Preamp: -0dB",
"",
"#reverb source",
"Copy: L2=R1 R2=L1 L12=R11 R12=L11",
"Channel: L2 R2",
"Delay: 20ms",
"Preamp: -6dB",
"Channel: L12 R12",
"Delay: 20ms",
"Preamp: -6dB",
"",
"#reverb only delay",
"Channel: R12 R2",
"Delay: 0ms",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#MIXER<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#upper speaker system",
"Channel: L12 R12",
"Preamp: -57dB		#set -57 to kill REVERB		12dB maximum",
"Copy: L19=L11+L12 R19=R11+R12",
"",
"#bass speaker system",
"Channel: L2 R2",
"Preamp: -57dB		#set -57 to kill REVERB		12dB maximum",
"Copy: L9=L1+L2 R9=R1+R2",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
""}
    Public Shared chorus_file = New String() {
"",
"#CHORUS",
"",
"Preamp: 0dB",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"Device: all",
"#DELAY SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"#L1 R1: BASS SPEAKER SYSTEM",
"#L11 R11: UPPER SPEAKER SYSTEM",
"",
"Channel: L1 R1",
"Preamp: 0dB",
"",
"Copy: L99=L1 R99=R1",
"Channel: L99 R99",
"#GraphicEQ: 1 12; 160 12; 250 6; 2500 -6",
"Delay: 33ms",
"#",
"# LEVEL 1: 83/17",
"# LEVEL 2: 80/20",
"# LEVEL 3: 75/25",
"# LEVEL 4: 67/33",
"Copy: L1=0.58*L1+0.41*L99 R1=0.58*R1+0.41*R99",
"Copy: L11=L1 R11=R1",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#SURROUND SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"Channel: L1 R1",
"GraphicEQ: 1 0; 250 0; 251 -57; 40000 -57",
"Delay: 0ms",
"Preamp: -0dB",
"Channel: L11 R11",
"GraphicEQ: 1 -57; 250 -57; 251 0; 40000 0",
"Delay: 0ms",
"Preamp: -0dB",
"",
"#reverb source",
"Copy: L2=R1 R2=L1 L12=R11 R12=L11",
"Channel: L2 R2",
"Delay: 20ms",
"Preamp: -6dB",
"Channel: L12 R12",
"Delay: 20ms",
"Preamp: -6dB",
"",
"#reverb only delay",
"Channel: R12 R2",
"Delay: 33ms",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#MIXER<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#upper speaker system",
"Channel: L12 R12",
"Preamp: 0dB		#set -57 to kill REVERB		12dB maximum",
"Copy: L19=L11+L12 R19=R11+R12",
"",
"#bass speaker system",
"Channel: L2 R2",
"Preamp: 0dB		#set -57 to kill REVERB		12dB maximum",
"Copy: L9=L1+L2 R9=R1+R2",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
""}
    Public Shared echo_file = New String() {
    "",
"#ECHO",
"",
"Preamp: 0dB",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"Device: all",
"#DELAY SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"#L1 R1: BASS SPEAKER SYSTEM",
"#L11 R11: UPPER SPEAKER SYSTEM",
"",
"Channel: L1 R1",
"Preamp: 0dB",
"",
"Copy: L99=L1 R99=R1",
"Channel: L99 R99",
"GraphicEQ: 1 12; 160 12; 250 6; 2500 -6",
"Delay: 5ms",
"#",
"# LEVEL 1: 83/17",
"# LEVEL 2: 80/20",
"# LEVEL 3: 75/25",
"# LEVEL 4: 67/33",
"Copy: L1=0.75*L1+0.25*L99 R1=0.75*R1+0.25*R99",
"Copy: L11=L1 R11=R1",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#SURROUND SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"Channel: L1 R1",
"GraphicEQ: 1 0; 250 0; 251 -57; 40000 -57",
"Delay: 0ms",
"Preamp: -0dB",
"Channel: L11 R11",
"GraphicEQ: 1 -57; 250 -57; 251 0; 40000 0",
"Delay: 0ms",
"Preamp: -0dB",
"",
"#reverb source",
"Copy: L2=R1 R2=L1 L12=R11 R12=L11",
"Channel: L2 R2",
"Delay: 20ms",
"Preamp: -6dB",
"Channel: L12 R12",
"Delay: 20ms",
"Preamp: -6dB",
"",
"#reverb only delay",
"Channel: R12 R2",
"Delay: 20ms",
"",
"Copy: R3=R2 L3=L2 R13=R12 L13=L12",
"Channel: L3 R3",
"GraphicEQ: 1 0; 160 0; 161 -57; 40000 -57",
"Delay: 40ms",
"Preamp: -9dB",
"Channel: L13 R13",
"GraphicEQ: 1 0; 400 0; 401 -57; 40000 -57",
"Delay: 40ms",
"Preamp: -9dB",
"",
"Copy: L4=R3 R4=L3 L14=R13 R14=L13",
"Channel: L4 R4",
"Delay: 80ms",
"Preamp: -12dB",
"Channel: L14 R14",
"Delay: 80ms",
"Preamp: -12dB",
"",
"Copy: L5=R4 R5=L4 L15=R14 R15=L14",
"Channel: L5 R5",
"Delay: 160ms",
"Preamp: -15dB",
"Channel: L15 R15",
"Delay: 160ms",
"Preamp: -15dB",
"",
"Copy: L6=R5 R6=L5 L16=R15 R16=L15",
"Channel: L6 R6",
"Delay: 320ms",
"Preamp: -18dB",
"Channel: L16 R16",
"Delay: 320ms",
"Preamp: -18dB",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#MIXER<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#upper speaker system",
"Channel: L12 R12",
"Preamp: -6dB		#set -57 to kill REVERB		12dB maximum",
"Channel: L13 L14 L15 L16 R13 R14 R15 R16",
"Preamp: 12dB		#set -57 to kill ECHO		12dB maximum",
"Copy: L19=L11+L12+L13+L14+L15+L16 R19=R11+R12+R13+R14+R15+R16",
"",
"#bass speaker system",
"Channel: L2 R2",
"Preamp: -6dB		#set -57 to kill REVERB		12dB maximum",
"Channel: L3 L4 L5 L6 R3 R4 R5 R6",
"Preamp: 12dB		#set -57 to kill ECHO		12dB maximum",
"Copy: L9=L1+L2+L3+L4+L5+L6 R9=R1+R2+R3+R4+R5+R6",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
""}

    Public Shared echo_ex_file = New String() {
        "",
"#ECHO EX",
"",
"Preamp: 0dB",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"Device: all",
"#DELAY SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"#L1 R1: BASS SPEAKER SYSTEM",
"#L11 R11: UPPER SPEAKER SYSTEM",
"",
"Channel: L1 R1",
"Preamp: 0dB",
"",
"Copy: L99=L1 R99=R1",
"Channel: L99 R99",
"GraphicEQ: 1 12; 160 12; 250 6; 2500 -6",
"Delay: 0.0ms",
"#",
"# LEVEL 1: 83/17",
"# LEVEL 2: 80/20",
"# LEVEL 3: 75/25",
"# LEVEL 4: 67/33",
"Copy: L1=0.83*L1+0.17*L99 R1=0.83*R1+0.17*R99",
"Copy: L11=L1 R11=R1",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#SURROUND SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"Channel: L1 R1",
"GraphicEQ: 1 0; 250 0; 251 -57; 40000 -57",
"Delay: 0ms",
"Preamp: -0dB",
"Channel: L11 R11",
"GraphicEQ: 1 -57; 250 -57; 251 0; 40000 0",
"Delay: 0ms",
"Preamp: -0dB",
"",
"#reverb source",
"Copy: L2=R1 R2=L1 L12=R11 R12=L11",
"Channel: L2 R2",
"Delay: 20ms",
"Preamp: -6dB",
"Channel: L12 R12",
"Delay: 20ms",
"Preamp: -6dB",
"",
"#reverb only delay",
"Channel: R12 R2",
"Delay: 20ms",
"",
"#first reverb",
"Copy: R3=R2 L3=L2 R13=R12 L13=L12",
"Channel: L3 R3",
"GraphicEQ: 1 0; 160 0; 161 -57; 40000 -57",
"Delay: 320ms",
"Preamp: -9dB",
"Channel: L13 R13",
"GraphicEQ: 1 -57; 400 -57; 401 0; 40000 0",
"Delay: 320ms",
"Preamp: -9dB",
"",
"Copy: L4=R3 R4=L3 L14=R13 R14=L13",
"Channel: L4 R4",
"Delay: 320ms",
"Preamp: -12dB",
"Channel: L14 R14",
"Delay: 320ms",
"Preamp: -12dB",
"",
"Copy: L5=R4 R5=L4 L15=R14 R15=L14",
"Channel: L5 R5",
"Delay: 320ms",
"Preamp: -15dB",
"Channel: L15 R15",
"Delay: 320ms",
"Preamp: -15dB",
"",
"Copy: L6=R5 R6=L5 L16=R15 R16=L15",
"Channel: L6 R6",
"Delay: 320ms",
"Preamp: -18dB",
"Channel: L16 R16",
"Delay: 320ms",
"Preamp: -18dB",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#MIXER<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#upper speaker system",
"Channel: L12 R12",
"Preamp: -6dB		#set -57 to kill REVERB		12dB maximum",
"Channel: L13 L14 L15 L16 R13 R14 R15 R16",
"Preamp: 0dB		#set -57 to kill ECHO		12dB maximum",
"Copy: L19=L11+L12+L13+L14+L15+L16 R19=R11+R12+R13+R14+R15+R16",
"",
"#bass speaker system",
"Channel: L2 R2",
"Preamp: -6dB		#set -57 to kill REVERB		12dB maximum",
"Channel: L3 L4 L5 L6 R3 R4 R5 R6",
"Preamp: 0dB		#set -57 to kill ECHO		12dB maximum",
"Copy: L9=L1+L2+L3+L4+L5+L6 R9=R1+R2+R3+R4+R5+R6",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
""}

    Public Shared compressor_file = New String() {
"",
"#COMPRESSOR",
"",
"Preamp: 0dB",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"Device: all",
"#DELAY SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"#L1 R1: BASS SPEAKER SYSTEM",
"#L11 R11: UPPER SPEAKER SYSTEM",
"",
"Channel: L1 R1",
"Preamp: 0dB",
"",
"Copy: L99=L1 R99=R1",
"Channel: L99 R99",
"GraphicEQ: 1 12; 160 12; 250 6; 2500 -6",
"Delay: 0.5ms",
"#",
"# LEVEL 1: 83/17",
"# LEVEL 2: 80/20",
"# LEVEL 3: 75/25",
"# LEVEL 4: 67/33",
"Copy: L1=0.83*L1+0.17*L99 R1=0.83*R1+0.17*R99",
"Copy: L11=L1 R11=R1",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#SURROUND SYSTEM<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"Channel: L1 R1",
"GraphicEQ: 1 0; 250 0; 251 -57; 40000 -57",
"Delay: 0ms",
"Preamp: -0dB",
"Channel: L11 R11",
"GraphicEQ: 1 -57; 250 -57; 251 0; 40000 0",
"Delay: 0ms",
"Preamp: -0dB",
"",
"#reverb source",
"Copy: L2=R1 R2=L1 L12=R11 R12=L11",
"Channel: L2 R2",
"Delay: 20ms",
"Preamp: -6dB",
"Channel: L12 R12",
"Delay: 20ms",
"Preamp: -6dB",
"",
"#reverb only delay",
"Channel: R12 R2",
"Delay: 0ms",
"",
"Copy: R3=R2 L3=L2 R13=R12 L13=L12",
"Channel: L3 R3",
"GraphicEQ: 1 0; 160 0; 161 -57; 40000 -57",
"Delay: 40ms",
"Preamp: -9dB",
"Channel: L13 R13",
"GraphicEQ: 1 0; 400 0; 401 -57; 40000 -57",
"Delay: 40ms",
"Preamp: -9dB",
"",
"Copy: L4=R3 R4=L3 L14=R13 R14=L13",
"Channel: L4 R4",
"Delay: 80ms",
"Preamp: -12dB",
"Channel: L14 R14",
"Delay: 80ms",
"Preamp: -12dB",
"",
"Copy: L5=R4 R5=L4 L15=R14 R15=L14",
"Channel: L5 R5",
"Delay: 160ms",
"Preamp: -15dB",
"Channel: L15 R15",
"Delay: 160ms",
"Preamp: -15dB",
"",
"Copy: L6=R5 R6=L5 L16=R15 R16=L15",
"Channel: L6 R6",
"Delay: 320ms",
"Preamp: -18dB",
"Channel: L16 R16",
"Delay: 320ms",
"Preamp: -18dB",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#MIXER<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"#upper speaker system",
"Channel: L12 R12",
"Preamp: -6dB		#set -57 to kill REVERB		12dB maximum",
"Channel: L13 L14 L15 L16 R13 R14 R15 R16",
"Preamp: -57dB		#set -57 to kill ECHO		12dB maximum",
"Copy: L19=L11+L12+L13+L14+L15+L16 R19=R11+R12+R13+R14+R15+R16",
"",
"#bass speaker system",
"Channel: L2 R2",
"Preamp: -6dB		#set -57 to kill REVERB		12dB maximum",
"Channel: L3 L4 L5 L6 R3 R4 R5 R6",
"Preamp: -57dB		#set -57 to kill ECHO		12dB maximum",
"Copy: L9=L1+L2+L3+L4+L5+L6 R9=R1+R2+R3+R4+R5+R6",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"",
"",
"",
"",
"",
"",
"",
"",
"",
""}

    Dim config_file_name As String = "C:\Program Files\EqualizerAPO\config\config.txt"
    Dim vefx_file_name As String = "C:\Program Files\EqualizerAPO\config\vefx.txt"


    Public Sub check_config()
        Try
            Dim check_count As Integer = 0
            check_flag = False
            temp_file2 = System.IO.File.ReadAllLines(config_file_name)
            For Each s In temp_file2
                check_count += 1
                check_flag = String.Equals(Regex.Replace(s, "\s+", String.Empty), Regex.Replace("Include: vefx.txt", "\s+", String.Empty))
            Next
            If check_flag = False Then
                Array.Resize(temp_file2, temp_file2.Length + 2)
                temp_file2(check_count) = "Device: all"
                temp_file2(check_count + 1) = "Include: vefx.txt"
                System.IO.File.WriteAllLines(config_file_name, temp_file2)
            End If
        Catch x As Exception
        End Try
    End Sub

    Public Sub firstrun()
        temp_file = IO.File.ReadAllLines(vefx_file_name)
        Try
            If temp_file(0) <> "" Then
                effector_on = 1
                effector_num = Val(temp_file(0)(1))
                effector_slider = Val(temp_file(0)(3))
                loweq_slider = Val(temp_file(0)(5))
                hieq_slider = Val(temp_file(0)(7))
                filter_slider = Val(temp_file(0)(9))
                vol_slider = Val(temp_file(0)(11))
                channel_slider = Val(temp_file(0)(13))
                bgfx_toggleb = Val(temp_file(0)(15))
            End If

        Catch ex As Exception

        End Try
        
        VEFX.Value = effector_slider
        LOW_EQ.Value = loweq_slider
        HIGH_EQ.Value = hieq_slider
        FILTER.Value = filter_slider
        VOLUME.Value = vol_slider
        CHANNEL.Value = channel_slider
        If bgfx_toggleb = 1 Then
            bgfx_toggle.Text = "BGFX on"
        Else
            bgfx_toggle.Text = "BGFX off"
        End If

        rerun()
    End Sub

    Public Sub rerun()
        Try
            temp_thread.Abort()
            wait_for_thread2 = False
        Catch e As Exception
        End Try

        'write
        If bgfx_toggleb = 1 Then
            If effector_on <> 0 Then
                Select Case effector_num
                    Case 2, 5
                        effector_slider = VEFX.Value
                    Case Else
                        effector_slider = If(effector_slider > 5, 5, effector_slider)
                End Select
                If effector_num >= 7 Then
                    effector_num = 1
                End If
            End If
        Else        'no bgfx version
            If effector_on <> 0 Then
                Select Case effector_num
                    Case 4, 5
                        effector_num = 6
                End Select
                Select Case effector_num
                    Case 2, 5
                        effector_slider = VEFX.Value
                    Case Else
                        effector_slider = If(effector_slider > 5, 5, effector_slider)
                End Select
                If effector_num >= 7 Then
                    effector_num = 1
                End If
            End If
        End If

        writetostuff(effector_num, effector_slider, loweq_slider, hieq_slider, filter_slider, vol_slider, channel_slider)

        'display
        menu_thread = New System.Threading.Thread(AddressOf writetext)
        Try
            menu_thread.Start()
        Catch x As Exception

        End Try

    End Sub

    Public Sub writetext()
        If effector_on = 0 Then
            While wait_for_thread
                System.Threading.Thread.Sleep(33)
            End While
            EFFECTOR_TEXT.Text = "EFFECTOR OFF"
        Else
            wait_for_thread = True

            If prev_loweq <> loweq_slider Then
                EFFECTOR_TEXT.Text = loweq_texts(loweq_slider)
                System.Threading.Thread.Sleep(2000)
            End If
            prev_loweq = loweq_slider

            If prev_hieq <> hieq_slider Then
                EFFECTOR_TEXT.Text = hieq_texts(hieq_slider)
                System.Threading.Thread.Sleep(2000)
            End If
            prev_hieq = hieq_slider

            If prev_filter <> filter_slider Then
                EFFECTOR_TEXT.Text = filter_texts(filter_slider)
                System.Threading.Thread.Sleep(2000)
            End If
            prev_filter = filter_slider

            If prev_vol <> vol_slider Then
                EFFECTOR_TEXT.Text = vol_texts(vol_slider)
                System.Threading.Thread.Sleep(2000)
            End If
            prev_vol = vol_slider

            If prev_channel <> channel_slider Then
                EFFECTOR_TEXT.Text = channel_texts(channel_slider)
                System.Threading.Thread.Sleep(2000)
            End If
            prev_channel = channel_slider


            Select Case effector_num
                Case 1
                    EFFECTOR_TEXT.Text = compressor_texts(effector_slider)
                Case 2
                    EFFECTOR_TEXT.Text = echo_texts(effector_slider)
                Case 3
                    EFFECTOR_TEXT.Text = echo_ex_texts(effector_slider)
                Case 4
                    EFFECTOR_TEXT.Text = chorus_texts(effector_slider)
                Case 5
                    EFFECTOR_TEXT.Text = gargle_texts(effector_slider)
                Case 6
                    EFFECTOR_TEXT.Text = eq_only_texts(0)
            End Select
            wait_for_thread = False

        End If

    End Sub

    Public Sub writetostuff(num As Integer, slider As Integer, slider2 As Integer, slider3 As Integer, slider4 As Integer, slider5 As Integer, slider6 As Integer)
        temp_thread = Nothing

        check_config()
        Try
            System.IO.File.Create(vefx_file_name).Dispose()
        Catch x As Exception
        End Try
        If effector_on <> 0 Then
            Select Case num
                Case 1
                    temp_file = compressor_file
                    If slider >= 3 Then
                        temp_file(33) = "Copy: L1=0." & Int(50 + 50 / 6 * (7 - slider)) & "*L1+0." & Int(50 - 50 / 6 * (7 - slider)) & "*L99 R1=0." & Int(50 + 50 / 6 * (7 - slider)) & "*R1+0." & Int(50 - 50 / 6 * (7 - slider)) & "*R99"
                        temp_file(22) = "Preamp: " & slider - 3 & "dB"
                    Else
                        temp_file(33) = "Copy: L1=0.83*L1+0.16*L99 R1=0.83*R1+0.16*R99"
                        temp_file(22) = "Preamp: 0dB"
                    End If
                Case 2
                    temp_file = echo_file
                    temp_file(100) = "Preamp: " & If(slider >= 3, -6, 0) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(102) = "Preamp: " & If(slider >= 3, (slider * 4) - 12, 12 - (slider * 4)) & "dB		#set -57 to kill ECHO		12dB maximum"
                    temp_file(107) = "Preamp: " & If(slider >= 3, -6, 0) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(109) = "Preamp: " & If(slider >= 3, (slider * 4) - 12, 12 - (slider * 4)) & "dB		#set -57 to kill ECHO		12dB maximum"
                    If slider >= 3 Then
                        temp_file(1) = "#ECHO"
                        temp_file(22) = "Preamp: 3dB"

                        temp_file(27) = "Delay: 5ms"
                        If slider = 6 Then
                            temp_file(33) = "Copy: L1=0.66*L1+0.33*L99 R1=0.66*R1+0.33*R99"
                        Else
                            temp_file(33) = "Copy: L1=0.75*L1+0.25*L99 R1=0.75*R1+0.25*R99"
                        End If

                    Else
                        temp_file(1) = "#REVERB"
                        temp_file(22) = "Preamp: 0dB"

                        temp_file(27) = "Delay: 0.5ms"
                        temp_file(33) = "Copy: L1=0.83*L1+0.16*L99 R1=0.83*R1+0.16*R99"
                    End If
                Case 3
                    temp_file = echo_ex_file
                    temp_file(101) = "Preamp: " & If(slider >= 3, -6, 0) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(103) = "Preamp: " & If(slider >= 3, (slider * 6) - 18, 18 - (slider * 6)) - 15 & "dB		#set -57 to kill ECHO		12dB maximum"
                    temp_file(108) = "Preamp: " & If(slider >= 3, -6, 0) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(110) = "Preamp: " & If(slider >= 3, (slider * 6) - 18, 18 - (slider * 6)) - 15 & "dB		#set -57 to kill ECHO		12dB maximum"

                    If slider >= 3 Then
                        temp_file(1) = "#ECHO EX"
                        temp_file(22) = "Preamp: " & slider - 6 & "dB"
                    Else
                        temp_file(1) = "#REVERB EX"
                        temp_file(22) = "Preamp: " & 0 - slider & "dB"
                    End If

                    temp_file(64) = "Delay: " & Int(240 * slider / 6 + 80) & "ms"
                    temp_file(68) = "Delay: " & Int(240 * slider / 6 + 80) & "ms"

                    temp_file(73) = "Delay: " & Int(240 * slider / 6 + 80) & "ms"
                    temp_file(76) = "Delay: " & Int(240 * slider / 6 + 80) & "ms"

                    temp_file(81) = "Delay: " & Int(240 * slider / 6 + 80) & "ms"
                    temp_file(84) = "Delay: " & Int(240 * slider / 6 + 80) & "ms"

                    temp_file(89) = "Delay: " & Int(240 * slider / 6 + 80) & "ms"
                    temp_file(92) = "Delay: " & Int(240 * slider / 6 + 80) & "ms"

                Case 4
                    temp_file = chorus_file

                    If slider >= 3 Then
                        temp_file(1) = "#CHORUS"
                    Else
                        temp_file(1) = "#FLANGER"

                    End If

                    If slider >= 3 Then
                        temp_file(33) = "Copy: L1=0." & Int(50 + 50 / 6 * (6 - slider)) & "*L1+0." & Int(50 - 50 / 6 * (6 - slider)) & "*L99 R1=0." & Int(50 + 50 / 6 * (6 - slider)) & "*R1+0." & Int(50 - 50 / 6 * (6 - slider)) & "*R99"
                        temp_file(22) = "Preamp: " & slider - 3 & "dB"
                        temp_file(58) = "Delay: 33ms"
                    Else
                        temp_file(33) = "Copy: L1=0." & Int(50 + 50 / 6 * slider) & "*L1+0." & Int(50 - 50 / 6 * slider) & "*L99 R1=0." & Int(50 + 50 / 6 * slider) & "*R1+0." & Int(50 - 50 / 6 * slider) & "*R99"
                        temp_file(22) = "Preamp: " & 6 - (slider * 2) & "dB"
                        temp_file(58) = "Delay: 0ms"
                    End If

                    temp_file(27) = "Delay: 33ms"

                    temp_file(66) = "Preamp: " & If(slider >= 3, 0, -57) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(71) = "Preamp: " & If(slider >= 3, 0, -57) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_thread = New System.Threading.Thread(AddressOf chorus_thread)
                    

                Case 5
                    temp_file = gargle_file
                    If slider >= 4 Then
                        temp_file(1) = "#GARGLE"
                        temp_file(12) = ""
                    Else
                        temp_file(1) = "#DISTORTION"
                        temp_file(12) = "GraphicEQ: 1 " & 6 - (slider * 2) & "; 160 " & 6 - (slider * 2) & "; " & 2500 + (slider * 1833) & " " & 6 - (slider * 2) & "; 8000 -57"
                    End If

                    temp_file(22) = "Preamp: 0dB"

                    temp_thread = New System.Threading.Thread(AddressOf gargle_thread)
                    
                Case 6
                    temp_file = eq_only_file
            End Select
            If slider5 >= 3 Then
                temp_file(13) = "Preamp: " & slider5 - 6 & "dB"
            Else
                temp_file(13) = "Preamp: " & slider5 * 2 - 9 & "dB"
            End If
            Select Case num
                Case 1, 2
                    temp_file(14) = "GraphicEQ: 1 " & If(slider4 >= 3, slider4 * 2 - 6, slider4 * 6 - 18) & "; 160 " & If(slider2 >= 3, slider2 - 3, slider2 * 6 - 18) & "; 2500 " & If(slider3 >= 3, slider3 - 3, slider3 * 6 - 18) & "; 16000 " & If(slider4 >= 3, slider4 * 2 - 6, slider4 * 6 - 18)
                Case Else
                    temp_file(14) = "GraphicEQ: 1 " & If(slider4 >= 3, slider4 * 2 - 6, slider4 * 6 - 18) + 6 & "; 160 " & If(slider2 >= 3, slider2 - 3, slider2 * 6 - 18) + 3 & "; 2500 " & If(slider3 >= 3, slider3 - 3, slider3 * 6 - 18) & "; 16000 " & If(slider4 >= 3, slider4 * 2 - 6, slider4 * 6 - 18)
            End Select


            Select Case num

                Case 6
                Case Else

                    temp_file(7) = "Device: all"
                    temp_file(8) = ""
                    temp_file(9) = ""
                    temp_file(10) = ""
                    temp_file(115) = "Device: all"
                    temp_file(116) = ""
                    temp_file(117) = ""
                    Select Case slider6
                        Case 0
                            temp_file(8) = "Copy: L1=C R1=C" 'mono
                            temp_file(9) = "Channel: L1 R1"
                            temp_file(10) = "Preamp: 6dB"
                            temp_file(116) = "Copy: C=0.25*L9+0.25*L19+0.25*R9+0.25*R19"
                        Case 1
                            temp_file(8) = "Copy: L1=L R1=R" 'stereo
                            temp_file(9) = "Channel: L1 R1"
                            temp_file(116) = "Copy: L=L9+L19 R=R9+R19"
                        Case 2
                            temp_file(8) = "Copy: L1=0.5*L+0.5*RL R1=0.5*R+0.5*RR" 'quad
                            temp_file(9) = "Channel: L1 R1"
                            temp_file(10) = "Preamp: 6dB"
                            temp_file(116) = "Copy: L=L9+L19 R=R9+R19 RL=L9+L19 RR=R9+R19"
                        Case 3
                            temp_file(8) = "Copy: L1=0.33*L+0.33*C+0.33*RC R1=0.33*R+0.33*C+0.33*RC" 'surround
                            temp_file(9) = "Channel: L1 R1"
                            temp_file(10) = "Preamp: 6dB"
                            temp_file(116) = "Copy: L=L9+L19 R=R9+R19 C=L19+R19 RC=L19+R19"
                        Case 4
                            temp_file(8) = "Copy: L1=0.25*L+0.25*C+0.25*SUB+0.25*SL R1=0.25*R+0.25*C+0.25*SUB+0.25*SR" '5.1
                            temp_file(9) = "Channel: L1 R1"
                            temp_file(10) = "Preamp: 6dB"
                            temp_file(116) = "Copy: L=L9+L19 R=R9+R19 C=L19+R19 SUB=L9+R9 SL=L9+L19 SR=R9+R19"
                        Case 5
                            temp_file(8) = "Copy: L1=0.25*L+0.25*C+0.25*SUB+0.25*RL R1=0.25*R+0.25*C+0.25*SUB+0.25*RR" '6.1
                            temp_file(9) = "Channel: L1 R1"
                            temp_file(10) = "Preamp: 6dB"
                            temp_file(116) = "Copy: L=L9+L19 R=R9+R19 C=L19+R19 SUB=L9+R9 RL=L9+L19 RR=R9+R19"
                        Case 6
                            temp_file(8) = "Copy: L1=0.2*L+0.2*C+0.2*SUB+0.2*SL+0.2*RL R1=0.2*R+0.2*C+0.2*SUB+0.2*SR+0.2*RR" '7.1
                            temp_file(9) = "Channel: L1 R1"
                            temp_file(10) = "Preamp: 6dB"
                            temp_file(116) = "Copy: L=L9+L19 R=R9+R19 C=L19+R19 SUB=L9+R9 SL=L9+L19 SR=R9+R19 RL=L9+L19 RR=R9+R19"
                    End Select

            End Select

            'save previous settings on top of vefx file
            temp_file(0) = " " & num & " " & slider & " " & slider2 & " " & slider3 & " " & slider4 & " " & slider5 & " " & slider6 & " " & bgfx_toggleb

            Try
                System.IO.File.WriteAllLines(vefx_file_name, temp_file)
                While wait_for_thread2
                    System.Threading.Thread.Sleep(33)
                End While
                temp_thread.Start()
            Catch x As Exception
            End Try

        End If
    End Sub

    Private Sub gargle_thread()
        Dim count As Integer = 3
        Dim flag As Boolean = False

        wait_for_thread2 = True
        While True
            If count = 27 Then
                count = 3
            End If
            count += 1
            If (count Mod (7 - effector_slider)) = 0 Then
                If flag = True Then
                    flag = False
                Else
                    flag = True
                End If
            End If
            Try
                If temp_file(1) = "#GARGLE" Then
                    If flag = True Then
                        temp_file(22) = "Preamp: -18dB"
                    Else
                        temp_file(22) = "Preamp: 0dB"
                    End If
                    System.IO.File.WriteAllLines(vefx_file_name, temp_file)
                Else
                    wait_for_thread2 = False
                    Exit Sub
                End If
            Catch x As Exception
                wait_for_thread2 = False
                Exit Sub
            End Try

            System.Threading.Thread.Sleep(33)
        End While
    End Sub

    Private Sub chorus_thread()
        Dim count As Integer = 99
        Dim flag As Boolean = False
        wait_for_thread2 = True
        While True
            If count <= 33 Then
                flag = True
            ElseIf count >= 99 Then
                flag = False
            End If
            If flag = True Then
                count += 1
            Else
                count -= 1
            End If
            Try
                If temp_file(1) = "#FLANGER" Then
                    temp_file(27) = "Delay: 0." & count & "ms"
                    System.IO.File.WriteAllLines(vefx_file_name, temp_file)
                Else
                    wait_for_thread2 = False
                    Exit Sub
                End If
            Catch x As Exception
                wait_for_thread2 = False
                Exit Sub
            End Try

            System.Threading.Thread.Sleep(33)
        End While
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        check_config()

        Dim processNames() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("VEFX Slider")
        Dim thisId As Integer = System.Diagnostics.Process.GetCurrentProcess.Id
        Try
            For Each p As System.Diagnostics.Process In processNames
                If p.Id <> thisId Then
                    p.Kill()
                End If
            Next
        Catch x As Exception
        End Try
        firstrun()
    End Sub

    Private Sub EFFECTOR_TEXT_TextChanged(sender As Object, e As EventArgs) Handles EFFECTOR_TEXT.TextChanged

    End Sub

    Private Sub EFFECT_ON_OFF_Click(sender As Object, e As EventArgs) Handles EFFECT_ON_OFF.Click
        If effector_on = 0 Then
            effector_on = 1
        Else
            effector_on = 0
        End If
        rerun()

    End Sub

    Private Sub VEFX_CHANGE_Click(sender As Object, e As EventArgs) Handles VEFX_CHANGE.Click
        If effector_on <> 0 Then
            effector_num = effector_num + 1
        End If
        rerun()

    End Sub

    Private Sub VEFX_Scroll(sender As Object, e As EventArgs) Handles VEFX.Scroll
        effector_slider = VEFX.Value
        rerun()

    End Sub

    Private Sub VOLUME_Scroll(sender As Object, e As EventArgs) Handles VOLUME.Scroll
        vol_slider = VOLUME.Value
        rerun()

    End Sub

    Private Sub FILTER_Scroll(sender As Object, e As EventArgs) Handles FILTER.Scroll
        filter_slider = FILTER.Value
        rerun()

    End Sub

    Private Sub LOW_EQ_Scroll(sender As Object, e As EventArgs) Handles LOW_EQ.Scroll
        loweq_slider = LOW_EQ.Value
        rerun()

    End Sub

    Private Sub HIGH_EQ_Scroll(sender As Object, e As EventArgs) Handles HIGH_EQ.Scroll
        hieq_slider = HIGH_EQ.Value
        rerun()

    End Sub

    Private Sub CHANNEL_Scroll(sender As Object, e As EventArgs) Handles CHANNEL.Scroll
        channel_slider = CHANNEL.Value
        rerun()

    End Sub

    Private Sub bgfx_toggle_Click(sender As Object, e As EventArgs) Handles bgfx_toggle.Click
        If bgfx_toggleb = 1 Then    'bgfx on
            bgfx_toggle.Text = "BGFX off"
            bgfx_toggleb = 0
        Else                    'bgfx off
            bgfx_toggle.Text = "BGFX on"
            bgfx_toggleb = 1
        End If

        rerun()
    End Sub

    Private Sub ResetAudiosrvToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetAudiosrvToolStripMenuItem.Click
        Dim result = MsgBox("Are you sure?", MsgBoxStyle.OkCancel, "Reset Audiosrv")
        If result <> MsgBoxResult.Cancel Then
            Dim process As New Process()
            process.StartInfo.FileName = "net.exe"
            process.StartInfo.Verb = "runas"
            process.StartInfo.Arguments = "stop audiosrv"
            process.StartInfo.UseShellExecute = True
            process.Start()
            process.WaitForExit()
            process.StartInfo.Arguments = "start audiosrv"
            process.Start()
            process.WaitForExit()
            MsgBox("Done. Try restarting audio applications.", MsgBoxStyle.OkOnly, "Reset Audiosrv")
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("Made by hoholee12@naver.com" & vbCrLf & "This application requires EqualizerAPO", MsgBoxStyle.OkOnly)
    End Sub

    Private Sub TunnelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TunnelToolStripMenuItem.Click
        effector_on = 1
        effector_num = 2
        VEFX.Value = 6
        VOLUME.Value = 6
        FILTER.Value = 5
        HIGH_EQ.Value = 4
        LOW_EQ.Value = 3
        effector_slider = VEFX.Value
        vol_slider = VOLUME.Value
        filter_slider = FILTER.Value
        loweq_slider = LOW_EQ.Value
        hieq_slider = HIGH_EQ.Value
        rerun()
    End Sub

    Private Sub MildEchoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MildEchoToolStripMenuItem.Click
        effector_on = 1
        effector_num = 2
        VEFX.Value = 3
        VOLUME.Value = 6
        FILTER.Value = 5
        HIGH_EQ.Value = 4
        LOW_EQ.Value = 3
        effector_slider = VEFX.Value
        vol_slider = VOLUME.Value
        filter_slider = FILTER.Value
        loweq_slider = LOW_EQ.Value
        hieq_slider = HIGH_EQ.Value
        rerun()
    End Sub

    Private Sub DancefloorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DancefloorToolStripMenuItem.Click
        effector_on = 1
        effector_num = 2
        VEFX.Value = 2
        VOLUME.Value = 6
        FILTER.Value = 5
        HIGH_EQ.Value = 4
        LOW_EQ.Value = 3
        effector_slider = VEFX.Value
        vol_slider = VOLUME.Value
        filter_slider = FILTER.Value
        loweq_slider = LOW_EQ.Value
        hieq_slider = HIGH_EQ.Value
        rerun()
    End Sub

    Private Sub OpenEAPOConfigToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenEAPOConfigToolStripMenuItem.Click
        Dim result = MsgBox("BTW, there is no need to reboot your system. Just click cancel after setup, and use 'Reset Audiosrv'!", MsgBoxStyle.OkOnly, "Open EAPO Config")
        Dim process As New Process()
        process.StartInfo.FileName = "C:\Program Files\EqualizerAPO\Configurator.exe"
        process.StartInfo.Verb = "runas"
        process.StartInfo.UseShellExecute = True
        process.Start()
        process.WaitForExit()
    End Sub
End Class
