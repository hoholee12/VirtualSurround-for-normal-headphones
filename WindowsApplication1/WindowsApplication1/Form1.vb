Public Class Form1
    Public Shared effector_num As Integer = 1
    Public Shared effector_on As Integer = 0
    Public Shared echo_texts = New String() {"REVERB 3", "REVERB 2", "REVERB 1", "ECHO 1", "ECHO 2", "ECHO 3", "ECHO 4"}
    Public Shared echo_ex_texts = New String() {"REVERB EX 3", "REVERB EX 2", "REVERB EX 1", "ECHO EX 1", "ECHO EX 2", "ECHO EX 3", "ECHO EX 4"}
    Public Shared compressor_texts = New String() {"COMPRESSOR"}
    Public Shared chorus_texts = New String() {"FLANGER 3", "FLANGER 2", "FLANGER 1", "CHORUS 1", "CHORUS 2", "CHORUS 3", "CHORUS 4"}
    Public Shared distortion_texts = New String() {"GARGLE 3", "GARGLE 2", "GARGLE 1", "DISTORTION 1", "DISTORTION 2", "DISTORTION 3", "DISTORTION4"}
    Public Shared eq_only_texts = New String() {"EQ ONLY"}
    Public Shared effector_slider As Integer = 3
    Public Shared effector_lim As Boolean = False
    Public Shared loweq_slider As Integer = 3
    Public Shared hieq_slider As Integer = 3
    Public Shared filter_slider As Integer = 3
    Public Shared vol_slider As Integer = 3

    Public Shared temp_thread As System.Threading.Thread
    Public Shared temp_file() As String
    Public Shared eq_only_file(124) As String
    Public Shared chorus_file = New String() {
        "",
"#CHORUS",
"",
"Preamp: 0dB",
"",
"",
"",
"Device: all",
"Copy: L1=L R1=R",
"",
"",
"Device: 스피커; speaker",
"Copy: L1=L+RL R1=R+RR",
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
"Delay: 1ms",
"#",
"# LEVEL 1: 83/17",
"# LEVEL 2: 80/20",
"# LEVEL 3: 75/25",
"# LEVEL 4: 67/33",
"Copy: L1=0.67*L1+0.33*L99 R1=0.67*R1+0.33*R99",
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
"#GraphicEQ: 1 0; 160 0; 161 -57; 40000 -57",
"Delay: 320ms",
"Preamp: -9dB",
"Channel: L13 R13",
"#GraphicEQ: 1 0; 400 0; 401 -57; 40000 -57",
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
"Preamp: 6dB		#set -57 to kill REVERB		12dB maximum",
"Channel: L13 L14 L15 L16 R13 R14 R15 R16",
"Preamp: -57dB		#set -57 to kill ECHO		12dB maximum",
"Copy: L19=L11+L12+L13+L14+L15+L16 R19=R11+R12+R13+R14+R15+R16",
"",
"#bass speaker system",
"Channel: L2 R2",
"Preamp: 6dB		#set -57 to kill REVERB		12dB maximum",
"Channel: L3 L4 L5 L6 R3 R4 R5 R6",
"Preamp: -57dB		#set -57 to kill ECHO		12dB maximum",
"Copy: L9=L1+L2+L3+L4+L5+L6 R9=R1+R2+R3+R4+R5+R6",
"",
"#<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
"",
"",
"",
"Device: all",
"Copy: L=L19+L9 R=R19+R9",
"",
"",
"Device: 스피커; speaker",
"Copy: RL=L19+L9 RR=R19+R9",
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
"Device: all",
"Copy: L1=L R1=R",
"",
"",
"Device: 스피커; speaker",
"Copy: L1=L+RL R1=R+RR",
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
"Device: all",
"Copy: L=L19+L9 R=R19+R9",
"",
"",
"Device: 스피커; speaker",
"Copy: RL=L19+L9 RR=R19+R9",
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
"Device: all",
"Copy: L1=L R1=R",
"",
"",
"Device: 스피커; speaker",
"Copy: L1=L+RL R1=R+RR",
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
"#GraphicEQ: 1 0; 160 0; 161 -57; 40000 -57",
"Delay: 320ms",
"Preamp: -9dB",
"Channel: L13 R13",
"#GraphicEQ: 1 0; 400 0; 401 -57; 40000 -57",
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
"Device: all",
"Copy: L=L19+L9 R=R19+R9",
"",
"",
"Device: 스피커; speaker",
"Copy: RL=L19+L9 RR=R19+R9",
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
"Device: all",
"Copy: L1=L R1=R",
"",
"",
"Device: 스피커; speaker",
"Copy: L1=L+RL R1=R+RR",
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
"Device: all",
"Copy: L=L19+L9 R=R19+R9",
"",
"",
"Device: 스피커; speaker",
"Copy: RL=L19+L9 RR=R19+R9",
"",
""}

    Public Sub rerun()
        Try
            temp_thread.Abort()
        Catch e As Exception
        End Try
        If effector_on = 0 Then
            EFFECTOR_TEXT.Text = "EFFECTOR OFF"
        Else
            Select Case effector_num
                Case 2
                    If effector_lim = True And effector_slider >= 5 Then
                        effector_slider = 6
                    Else
                        effector_lim = False
                    End If
                Case Else
                    effector_lim = If(effector_slider > 5 Or If(effector_slider = 5 And effector_lim = True, True, False), True, False)
                    effector_slider = If(effector_slider > 5, 5, effector_slider)
            End Select
            Select Case effector_num
                Case 1
                    EFFECTOR_TEXT.Text = compressor_texts(0)
                Case 2
                    EFFECTOR_TEXT.Text = echo_texts(effector_slider)
                Case 3
                    EFFECTOR_TEXT.Text = echo_ex_texts(effector_slider)
                Case 4
                    EFFECTOR_TEXT.Text = chorus_texts(effector_slider)
                Case 5
                    EFFECTOR_TEXT.Text = eq_only_texts(0)
                Case 6
                    effector_num = 1
                    EFFECTOR_TEXT.Text = compressor_texts(0)
            End Select
        End If

        writetostuff(effector_num, effector_slider, loweq_slider, hieq_slider, filter_slider, vol_slider)
    End Sub

    Public Sub writetostuff(num As Integer, slider As Integer, slider2 As Integer, slider3 As Integer, slider4 As Integer, slider5 As Integer)
        Try
            System.IO.File.Create("config.txt").Dispose()
        Catch x As Exception
        End Try
        If effector_on <> 0 Then
            Select Case num
                Case 1
                    temp_file = compressor_file

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
                        temp_file(33) = "Copy: L1=0.66*L1+0.33*L99 R1=0.66*R1+0.33*R99"
                    Else
                        temp_file(1) = "#REVERB"
                        temp_file(22) = "Preamp: 0dB"

                        temp_file(27) = "Delay: 0.5ms"
                        temp_file(33) = "Copy: L1=0.83*L1+0.16*L99 R1=0.83*R1+0.16*R99"
                    End If
                Case 3
                    temp_file = echo_ex_file
                    temp_file(101) = "Preamp: " & If(slider >= 3, -6, 0) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(103) = "Preamp: " & If(slider >= 3, (slider * 4) - 12, 12 - (slider * 4)) - 12 & "dB		#set -57 to kill ECHO		12dB maximum"
                    temp_file(108) = "Preamp: " & If(slider >= 3, -6, 0) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(110) = "Preamp: " & If(slider >= 3, (slider * 4) - 12, 12 - (slider * 4)) - 12 & "dB		#set -57 to kill ECHO		12dB maximum"

                    If slider >= 3 Then
                        temp_file(1) = "#ECHO EX"
                        temp_file(22) = "Preamp: 0dB"
                    Else
                        temp_file(1) = "#REVERB EX"
                        temp_file(22) = "Preamp: -3dB"
                    End If

                    temp_file(64) = "Delay: " & Int(280 * slider / 6 + 40) & "ms"
                    temp_file(68) = "Delay: " & Int(280 * slider / 6 + 40) & "ms"

                    temp_file(73) = "Delay: " & Int(280 * slider / 6 + 40) & "ms"
                    temp_file(76) = "Delay: " & Int(280 * slider / 6 + 40) & "ms"

                    temp_file(81) = "Delay: " & Int(280 * slider / 6 + 40) & "ms"
                    temp_file(84) = "Delay: " & Int(280 * slider / 6 + 40) & "ms"

                    temp_file(89) = "Delay: " & Int(280 * slider / 6 + 40) & "ms"
                    temp_file(92) = "Delay: " & Int(280 * slider / 6 + 40) & "ms"

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
                        temp_file(58) = "Delay: 40ms"
                    Else
                        temp_file(33) = "Copy: L1=0." & Int(50 + 50 / 6 * slider) & "*L1+0." & Int(50 - 50 / 6 * slider) & "*L99 R1=0." & Int(50 + 50 / 6 * slider) & "*R1+0." & Int(50 - 50 / 6 * slider) & "*R99"
                        temp_file(22) = "Preamp: " & 3 - slider & "dB"
                        temp_file(58) = "Delay: 0ms"
                    End If

                    temp_file(27) = "Delay: 40ms"

                    temp_file(101) = "Preamp: " & If(slider >= 3, 0, -57) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(108) = "Preamp: " & If(slider >= 3, 0, -57) & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_thread = New System.Threading.Thread(AddressOf chorus_thread)
                    Try
                        temp_thread.Start()
                    Catch x As Exception
                    End Try

                Case 5
                    temp_file = eq_only_file
            End Select
            If slider5 >= 3 Then
                temp_file(3) = "Preamp: " & slider5 - 6 & "dB"
            Else
                temp_file(3) = "Preamp: " & slider5 * 2 - 9 & "dB"
            End If
            Select Case num
                Case 2
                    temp_file(5) = "GraphicEQ: 1 " & If(slider4 >= 3, slider4 * 2 - 6, slider4 * 6 - 18) - 6 & "; 160 " & If(slider2 >= 3, slider2 - 3, slider2 * 6 - 18) - 3 & "; 2500 " & If(slider3 >= 3, slider3 - 3, slider3 * 6 - 18) & "; 16000 " & If(slider4 >= 3, slider4 * 2 - 6, slider4 * 6 - 18)
                Case Else
                    temp_file(5) = "GraphicEQ: 1 " & If(slider4 >= 3, slider4 * 2 - 6, slider4 * 6 - 18) & "; 160 " & If(slider2 >= 3, slider2 - 3, slider2 * 6 - 18) & "; 2500 " & If(slider3 >= 3, slider3 - 3, slider3 * 6 - 18) & "; 16000 " & If(slider4 >= 3, slider4 * 2 - 6, slider4 * 6 - 18)
            End Select
            Try
                System.IO.File.WriteAllLines("config.txt", temp_file)
            Catch x As Exception
            End Try

        End If
    End Sub

    Private Sub chorus_thread()
        Dim count As Integer = 99
        Dim flag As Integer = 0
        While True
            If count <= 50 Then
                flag = 1
            ElseIf count >= 99 Then
                flag = 0
            End If
            If flag = 1 Then
                count += 1
            Else
                count -= 1
            End If
            Try
                If temp_file(1) = "#FLANGER" Then
                    temp_file(27) = "Delay: 0." & count & "ms"
                    System.IO.File.WriteAllLines("config.txt", temp_file)
                Else
                    Exit Sub
                End If
            Catch x As Exception
            End Try

            System.Threading.Thread.Sleep(40)
        End While
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        writetostuff(effector_num, effector_slider, loweq_slider, hieq_slider, filter_slider, vol_slider)
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged

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
End Class
