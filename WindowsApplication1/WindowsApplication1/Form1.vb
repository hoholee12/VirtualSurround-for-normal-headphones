Public Class Form1
    Public Shared effector_num As Integer = 1
    Public Shared effector_on As Integer = 0
    Public Shared echo_texts = New String() {"REVERB 3", "REVERB 2", "REVERB 1", "ECHO 1", "ECHO 2", "ECHO 3", "ECHO 4"}
    Public Shared echo_ex_texts = New String() {"REVERB EX 3", "REVERB EX 2", "REVERB EX 1", "ECHO EX 1", "ECHO EX 2", "ECHO EX 3", "ECHO EX 4"}
    Public Shared compressor_texts = New String() {"COMPRESSOR"}
    Public Shared chorus_texts = New String() {"FLANGER 3", "FLANGER 2", "FLANGER 1", "CHORUS 1", "CHORUS 2", "CHORUS 3", "CHORUS 4"}
    Public Shared distortion_texts = New String() {"GARGLE 3", "GARGLE 2", "GARGLE 1", "DISTORTION 1", "DISTORTION 2", "DISTORTION 3", "DISTORTION 4"}
    Public Shared effector_slider As Integer = 3
    Public Shared loweq_slider As Integer = 3
    Public Shared hieq_slider As Integer = 3
    Public Shared filter_slider As Integer = 3
    Public Shared vol_slider As Integer = 3

    Public Shared temp_file = New String() {}
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
"Preamp: 3dB",
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
"Preamp: 3dB",
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
"Preamp: 3dB",
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
        If effector_on = 0 Then
            EFFECTOR_TEXT.Text = "EFFECTOR OFF"
        Else
            Select Case effector_num
                Case 1
                    EFFECTOR_TEXT.Text = compressor_texts(0)
                Case 2
                    EFFECTOR_TEXT.Text = echo_texts(effector_slider)
                Case 3
                    EFFECTOR_TEXT.Text = echo_ex_texts(effector_slider)
                Case 4
                    effector_num = 1
                    EFFECTOR_TEXT.Text = compressor_texts(0)
            End Select
        End If

        writetostuff(effector_num, effector_slider, loweq_slider, hieq_slider, filter_slider, vol_slider)
    End Sub

    Public Sub writetostuff(num As Integer, slider As Integer, slider2 As Integer, slider3 As Integer, slider4 As Integer, slider5 As Integer)
        System.IO.File.Create("config.txt").Dispose()
        If effector_on <> 0 Then
            Select Case num
                Case 1
                    temp_file = compressor_file

                Case 2
                    temp_file = echo_file
                    temp_file(100) = "Preamp: " & (slider - 3) * -2 & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(102) = "Preamp: " & slider * 2 & "dB		#set -57 to kill ECHO		12dB maximum"
                    temp_file(107) = "Preamp: " & (slider - 3) * -2 & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(109) = "Preamp: " & slider * 2 & "dB		#set -57 to kill ECHO		12dB maximum"
                    If slider >= 3 Then
                        temp_file(1) = "#ECHO"
                        temp_file(22) = "Preamp: 3dB"
                    Else
                        temp_file(1) = "#REVERB"
                        temp_file(22) = "Preamp: 0dB"
                    End If
                Case 3
                    temp_file = echo_ex_file
                    temp_file(101) = "Preamp: " & (slider - 3) * -2 & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(103) = "Preamp: " & slider * 2 - 12 & "dB		#set -57 to kill ECHO		12dB maximum"
                    temp_file(108) = "Preamp: " & (slider - 3) * -2 & "dB		#set -57 to kill REVERB		12dB maximum"
                    temp_file(110) = "Preamp: " & slider * 2 - 12 & "dB		#set -57 to kill ECHO		12dB maximum"
                    If slider >= 3 Then
                        temp_file(1) = "#ECHO EX"
                        temp_file(22) = "Preamp: 3dB"
                    Else
                        temp_file(1) = "#REVERB EX"
                        temp_file(22) = "Preamp: 0dB"
                    End If
            End Select
            If slider5 >= 3 Then
                temp_file(3) = "Preamp: " & slider5 - 3 & "dB"
            Else
                temp_file(3) = "Preamp: " & slider5 * 10 - 30 & "dB"
            End If

            temp_file(5) = "GraphicEQ: 1 " & If(slider2 >= 3, slider2 - 3, slider2 * 10 - 30) & "; 160 " & If(slider4 >= 3, slider4 - 3, slider4 * 10 - 30) & "; 250 0; 2500 " & If(slider3 >= 3, slider3 - 3, slider3 * 10 - 30)

            System.IO.File.WriteAllLines("config.txt", temp_file)
        End If
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
