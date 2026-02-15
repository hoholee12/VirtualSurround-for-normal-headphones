Imports System
Imports System.Text.RegularExpressions
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces
Imports System.Web.Script.Serialization
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.Security.AccessControl
Imports System.Security.Principal

Public Class Form1
    ' Windows API declarations for registry permission management
    <DllImport("advapi32.dll", SetLastError:=True)>
    Private Shared Function OpenProcessToken(processHandle As IntPtr, desiredAccess As UInteger, ByRef tokenHandle As IntPtr) As Boolean
    End Function

    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Private Shared Function LookupPrivilegeValue(lpSystemName As String, lpName As String, ByRef lpLuid As LUID) As Boolean
    End Function

    <DllImport("advapi32.dll", SetLastError:=True)>
    Private Shared Function AdjustTokenPrivileges(tokenHandle As IntPtr, disableAllPrivileges As Boolean, ByRef newState As TOKEN_PRIVILEGES, bufferLength As UInteger, ByVal previousState As IntPtr, ByVal returnLength As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll")>
    Private Shared Function GetCurrentProcess() As IntPtr
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function RegOpenKeyEx(hKey As IntPtr, lpSubKey As String, ulOptions As UInteger, samDesired As Integer, ByRef phkResult As IntPtr) As Integer
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function RegSetValueEx(hKey As IntPtr, lpValueName As String, reserved As UInteger, dwType As UInteger, lpData As String, cbData As UInteger) As Integer
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function RegSetValueEx(hKey As IntPtr, lpValueName As String, reserved As UInteger, dwType As UInteger, lpData As Byte(), cbData As UInteger) As Integer
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function RegSetValueEx(hKey As IntPtr, lpValueName As String, reserved As UInteger, dwType As UInteger, lpData As String(), cbData As UInteger) As Integer
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function RegQueryValueEx(hKey As IntPtr, lpValueName As String, lpReserved As IntPtr, ByRef lpType As UInteger, lpData As Byte(), ByRef lpcbData As UInteger) As Integer
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function RegCreateKeyEx(hKey As IntPtr, lpSubKey As String, Reserved As UInteger, lpClass As String, dwOptions As UInteger, samDesired As UInteger, lpSecurityAttributes As IntPtr, ByRef phkResult As IntPtr, lpdwDisposition As IntPtr) As Integer
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function RegDeleteValue(hKey As IntPtr, lpValueName As String) As Integer
    End Function

    <DllImport("advapi32.dll", SetLastError:=True)>
    Private Shared Function RegCloseKey(hKey As IntPtr) As Integer
    End Function

    Private Shared ReadOnly HKEY_LOCAL_MACHINE As IntPtr = New IntPtr(&H80000002)
    Private Const KEY_WOW64_64KEY As Integer = &H100
    Private Const KEY_SET_VALUE As Integer = &H2
    Private Const KEY_QUERY_VALUE As Integer = &H1
    Private Const KEY_CREATE_SUB_KEY As Integer = &H4
    Private Const REG_SZ As UInteger = 1
    Private Const REG_MULTI_SZ As UInteger = 7
    Private Const ERROR_SUCCESS As Integer = 0

    <StructLayout(LayoutKind.Sequential)>
    Private Structure LUID
        Public LowPart As UInteger
        Public HighPart As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure LUID_AND_ATTRIBUTES
        Public Luid As LUID
        Public Attributes As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure TOKEN_PRIVILEGES
        Public PrivilegeCount As UInteger
        Public Privileges As LUID_AND_ATTRIBUTES
    End Structure

    Private Const TOKEN_ADJUST_PRIVILEGES As UInteger = &H20
    Private Const TOKEN_QUERY As UInteger = &H8
    Private Const SE_PRIVILEGE_ENABLED As UInteger = &H2
    Private Const SE_TAKE_OWNERSHIP_NAME As String = "SeTakeOwnershipPrivilege"

    Public Shared effector_num As Integer = 1
    Public Shared effector_on As Integer = 0
    Public Shared echo_texts = New String() {"REVERB 3", "REVERB 2", "REVERB 1", "ECHO 1", "ECHO 2", "ECHO 3", "ECHO 4"}
    Public Shared echo_ex_texts = New String() {"REVERB EX 3", "REVERB EX 2", "REVERB EX 1", "ECHO EX 1", "ECHO EX 2", "ECHO EX 3", "ECHO EX 4"}
    Public Shared compressor_texts = New String() {"COMPRESSOR 1", "  COMPRESSOR 1      written by manual...as a fan project for beatmania series...have fun!", "COMPRESSOR 1", "COMPRESSOR 1", "COMPRESSOR 2", "COMPRESSOR 3", "COMPRESSOR 4"}
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

    ' Device management: connector_names and device_guids are parallel lists
    ' connector_names: Display names like "Speakers (Realtek Audio)"
    ' device_guids: Corresponding device GUIDs for APO installation
    Public Shared connector_names As New List(Of String)
    Public Shared device_guids As New List(Of String)
    Public Shared current_connector_index As Integer = 0

    ' Settings stored by connector type (e.g., "Speakers", "Headphones")
    ' Multiple devices with same connector type share settings
    Public Shared connector_settings As New Dictionary(Of String, ConnectorSettings)

    ' Structure to hold connector-specific settings
    Public Structure ConnectorSettings
        Public EffectorOn As Integer
        Public EffectorNum As Integer
        Public EffectorSlider As Integer
        Public LowEQSlider As Integer
        Public HighEQSlider As Integer
        Public FilterSlider As Integer
        Public VolSlider As Integer
        Public ChannelSlider As Integer
        Public BgfxToggle As Integer
    End Structure

    ' Thread synchronization for effect threads
    Private Shared effectThreadAbort As Boolean = False
    Private Shared ReadOnly effectThreadLock As New Object()

    ' Preset management variables
    Private Shared customPresets As New Dictionary(Of String, PresetData)
    Private Shared ReadOnly configFilePath As String = System.IO.Path.Combine(Application.StartupPath, "config.json")

    ' Device refresh timer
    Private WithEvents deviceRefreshTimer As New Timer()
    Private Shared lastDeviceGuids As New List(Of String)

    ' Structure to hold preset data
    Public Structure PresetData
        Public EffectorOn As Integer
        Public EffectorNum As Integer
        Public VEFXValue As Integer
        Public VolumeValue As Integer
        Public FilterValue As Integer
        Public HighEQValue As Integer
        Public LowEQValue As Integer
        Public ChannelValue As Integer
        Public BgfxValue As Integer
    End Structure

    ' Structure to hold config data (for JSON serialization)
    Public Class ConfigData
        Public Property CustomPresets As Dictionary(Of String, PresetData)

        Public Sub New()
            CustomPresets = New Dictionary(Of String, PresetData)()
        End Sub
    End Class

    ' Helper method to write file with buffered I/O (reduces physical disk writes)
    ' Uses FileShare.Read to allow other programs to read while we write
    ' Closes file after write to ensure other programs see changes
    Private Shared Sub WriteAllLinesBuffered(fileName As String, contents As String())
        Using fs As New System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read, 4096, System.IO.FileOptions.None)
            Using writer As New System.IO.StreamWriter(fs, System.Text.Encoding.UTF8)
                For Each line As String In contents
                    writer.WriteLine(line)
                Next
            End Using
        End Using
    End Sub

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

    ' Helper function to extract connector name from display name "Connector (Device)"
    Public Function GetConnectorName(displayName As String) As String
        Dim parenIndex As Integer = displayName.IndexOf("("c)
        If parenIndex > 0 Then
            Return displayName.Substring(0, parenIndex).Trim()
        End If
        Return displayName
    End Function

    ' Get the GUID of the currently active default audio device
    Private Function GetDefaultAudioDeviceGuid() As String
        Try
            Dim enumerator As Object = New MMDeviceEnumerator()
            Dim iEnumerator As IMMDeviceEnumerator = DirectCast(enumerator, IMMDeviceEnumerator)
            
            ' Get default audio output device
            Dim defaultDevice As IMMDevice = Nothing
            Dim hr As Integer = iEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, defaultDevice)
            
            If hr = 0 AndAlso defaultDevice IsNot Nothing Then
                ' Get device ID
                Dim pDeviceId As IntPtr = IntPtr.Zero
                defaultDevice.GetId(pDeviceId)
                
                If pDeviceId <> IntPtr.Zero Then
                    Dim deviceId As String = Marshal.PtrToStringUni(pDeviceId)
                    Marshal.FreeCoTaskMem(pDeviceId)
                    
                    ' Extract GUID from device ID (format: {0.0.0.00000000}.{GUID})
                    ' The GUID we need is after the last '.'
                    If deviceId.Contains("}.{") Then
                        Dim startIndex As Integer = deviceId.LastIndexOf(".{") + 1
                        Dim guid As String = deviceId.Substring(startIndex)
                        Return guid
                    End If
                End If
            End If
        Catch ex As Exception
            ' If we can't get default device, return empty string
        End Try
        Return String.Empty
    End Function

    ' Update the form title bar with current connector name
    Private Sub UpdateTitleBar()
        If current_connector_index >= 0 AndAlso current_connector_index < connector_names.Count Then
            Me.Text = "VEFX Slider (Current: " & GetCurrentConnectorName() & ")"
        Else
            Me.Text = "VEFX Slider"
        End If
    End Sub

    ' Update the APO status indicator based on current device
    Private Sub UpdateAPOStatusIndicator()
        If current_connector_index >= 0 AndAlso current_connector_index < device_guids.Count Then
            Dim deviceGuid As String = device_guids(current_connector_index)
            If Not String.IsNullOrEmpty(deviceGuid) AndAlso CheckAPOInstalled(deviceGuid) Then
                apo_status_indicator.BackColor = Color.Lime
            Else
                apo_status_indicator.BackColor = Color.Red
            End If
        Else
            apo_status_indicator.BackColor = Color.Gray
        End If
    End Sub

    ' Helper: Get current connector name (without device description)
    Private Function GetCurrentConnectorName() As String
        Return GetConnectorName(connector_names(current_connector_index))
    End Function

    ' Helper: Get vefx filename for a connector name
    Private Function GetVefxFileNameForConnector(connectorName As String) As String
        Dim fileNameSafe As String = connectorName.ToLower().Replace(" ", "_")
        Return "C:\Program Files\EqualizerAPO\config\vefx_" & fileNameSafe & ".txt"
    End Function

    ' Helper: Load connector settings from vefx file (or return defaults)
    Private Function LoadConnectorSettingsFromFile(connectorName As String) As ConnectorSettings
        Dim settings As New ConnectorSettings With {
            .EffectorOn = 0,
            .EffectorNum = 1,
            .EffectorSlider = 3,
            .LowEQSlider = 3,
            .HighEQSlider = 3,
            .FilterSlider = 3,
            .VolSlider = 3,
            .ChannelSlider = 1,
            .BgfxToggle = 1
        }

        Try
            Dim vefxFilePath As String = GetVefxFileNameForConnector(connectorName)
            If System.IO.File.Exists(vefxFilePath) Then
                Dim tempFileLines = IO.File.ReadAllLines(vefxFilePath)
                If tempFileLines.Length > 0 AndAlso tempFileLines(0) <> "" Then
                    settings.EffectorOn = 1
                    settings.EffectorNum = Val(tempFileLines(0)(1))
                    settings.EffectorSlider = Val(tempFileLines(0)(3))
                    settings.LowEQSlider = Val(tempFileLines(0)(5))
                    settings.HighEQSlider = Val(tempFileLines(0)(7))
                    settings.FilterSlider = Val(tempFileLines(0)(9))
                    settings.VolSlider = Val(tempFileLines(0)(11))
                    settings.ChannelSlider = Val(tempFileLines(0)(13))
                    settings.BgfxToggle = Val(tempFileLines(0)(15))
                End If
            End If
        Catch ex As Exception
            ' If loading fails, return default settings
        End Try

        Return settings
    End Function

    ' Function to get connector-specific vefx filename for current device
    Public Function GetVefxFileName() As String
        Return GetVefxFileNameForConnector(GetCurrentConnectorName())
    End Function

    ' Load audio devices from system registry - each device shown separately
    ' Returns True if device list changed, False if unchanged
    Private Function LoadAudioDevices() As Boolean
        Dim previousDevices As New List(Of String)(device_guids)
        Dim previousNames As New List(Of String)(connector_names)

        connector_names.Clear()
        device_guids.Clear()

        Try
            ' Enumerate all playback devices from registry
            Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using key As RegistryKey = baseKey.OpenSubKey(RENDER_PATH)
                    If key IsNot Nothing Then
                        Dim subKeys() As String = key.GetSubKeyNames()

                        For Each deviceGuid In subKeys
                            ' Skip inactive/disconnected devices
                            If Not IsDeviceActive(deviceGuid) Then
                                Continue For
                            End If

                            ' Get connector name and device description
                            Dim connectorName As String = GetDeviceConnectorName(deviceGuid)
                            Dim deviceDesc As String = GetDeviceDescription(deviceGuid)

                            If Not String.IsNullOrEmpty(connectorName) Then
                                ' Format as "Connector (Device Description)"
                                Dim displayName As String = connectorName
                                If Not String.IsNullOrEmpty(deviceDesc) Then
                                    displayName &= " (" & deviceDesc & ")"
                                End If

                                connector_names.Add(displayName)
                                device_guids.Add(deviceGuid)
                            End If
                        Next
                    End If
                End Using
            End Using

            ' Sort devices: first by connector name, then by device name
            If connector_names.Count > 1 Then
                ' Create a list of tuples to maintain the relationship between names and GUIDs
                Dim deviceList As New List(Of Tuple(Of String, String))
                For i As Integer = 0 To connector_names.Count - 1
                    deviceList.Add(New Tuple(Of String, String)(connector_names(i), device_guids(i)))
                Next

                ' Sort by connector name first, then by device name
                deviceList.Sort(Function(x, y)
                                    Dim xConnector As String = GetConnectorName(x.Item1)
                                    Dim yConnector As String = GetConnectorName(y.Item1)

                                    ' Compare connector names first
                                    Dim connectorCompare As Integer = String.Compare(xConnector, yConnector, StringComparison.OrdinalIgnoreCase)
                                    If connectorCompare <> 0 Then
                                        Return connectorCompare
                                    End If

                                    ' If connector names are the same, compare full display names (includes device name)
                                    Return String.Compare(x.Item1, y.Item1, StringComparison.OrdinalIgnoreCase)
                                End Function)

                ' Update the lists with sorted values
                connector_names.Clear()
                device_guids.Clear()
                For Each item In deviceList
                    connector_names.Add(item.Item1)
                    device_guids.Add(item.Item2)
                Next
            End If

        Catch ex As Exception
            MsgBox("Error loading audio devices: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        ' Ensure we have at least one device
        If connector_names.Count = 0 Then
            connector_names.Add("Speakers (Default)")
            device_guids.Add("")
        End If

        ' Check if device list changed
        If previousDevices.Count <> device_guids.Count Then
            Return True
        End If

        For i As Integer = 0 To device_guids.Count - 1
            If i >= previousDevices.Count OrElse device_guids(i) <> previousDevices(i) OrElse connector_names(i) <> previousNames(i) Then
                Return True
            End If
        Next

        Return False ' No changes detected
    End Function


    Public Sub check_config()
        Try
            ' Create empty config.txt if it doesn't exist
            If Not System.IO.File.Exists(config_file_name) Then
                System.IO.File.WriteAllText(config_file_name, "")
            End If

            temp_file2 = System.IO.File.ReadAllLines(config_file_name)
            Dim needsUpdate As Boolean = False

            ' Build list of unique connector names (extract from display names)
            Dim uniqueConnectors As New HashSet(Of String)
            For Each displayName In connector_names
                Dim connectorName As String = GetConnectorName(displayName)
                uniqueConnectors.Add(connectorName)
            Next

            ' Check if each connector's include directive exists
            For Each connectorName In uniqueConnectors
                Dim deviceLine As String = "Device: " & connectorName
                Dim fileNameSafe As String = connectorName.ToLower().Replace(" ", "_")
                Dim includeLine As String = "Include: vefx_" & fileNameSafe & ".txt"
                Dim foundConnector As Boolean = False

                For i As Integer = 0 To temp_file2.Length - 2
                    If temp_file2(i).Trim() = deviceLine AndAlso temp_file2(i + 1).Trim() = includeLine Then
                        foundConnector = True
                        Exit For
                    End If
                Next

                If Not foundConnector Then
                    needsUpdate = True
                    Exit For
                End If
            Next

            ' If update needed, rebuild config file with unique connectors only
            If needsUpdate Then
                Dim newConfig As New List(Of String)

                ' Add any existing content that's not Device: or Include: lines
                For Each line In temp_file2
                    If Not line.Trim().StartsWith("Device:") AndAlso Not line.Trim().StartsWith("Include: vefx_") Then
                        newConfig.Add(line)
                    End If
                Next

                ' Add connector device blocks - one per unique connector only
                For Each connectorName In uniqueConnectors
                    Dim fileNameSafe As String = connectorName.ToLower().Replace(" ", "_")
                    newConfig.Add("Device: " & connectorName)
                    newConfig.Add("Include: vefx_" & fileNameSafe & ".txt")
                Next

                System.IO.File.WriteAllLines(config_file_name, newConfig.ToArray())
            End If
        Catch x As Exception
        End Try
    End Sub

    Public Sub firstrun()
        ' Build list of unique connector names (extract from display names)
        Dim uniqueConnectors As New HashSet(Of String)
        For Each displayName In connector_names
            uniqueConnectors.Add(GetConnectorName(displayName))
        Next

        ' Load settings for unique connectors only
        For Each connectorName In uniqueConnectors
            ' Create empty connector-specific vefx file if it doesn't exist
            Dim vefxFilePath As String = GetVefxFileNameForConnector(connectorName)
            If Not System.IO.File.Exists(vefxFilePath) Then
                System.IO.File.WriteAllText(vefxFilePath, "")
            End If

            ' Store settings for this connector (load from file)
            If Not connector_settings.ContainsKey(connectorName) Then
                connector_settings.Add(connectorName, LoadConnectorSettingsFromFile(connectorName))
            End If
        Next

        ' Load first connector settings to UI
        If connector_names.Count > 0 Then
            LoadCurrentConnectorSettings()
        End If
    End Sub

    Public Sub rerun()
        ' Safely abort any existing effect thread
        Try
            SyncLock effectThreadLock
                effectThreadAbort = True
            End SyncLock

            If temp_thread IsNot Nothing AndAlso temp_thread.IsAlive Then
                ' Wait for thread to finish gracefully with timeout
                Dim timeout As Integer = 0
                While wait_for_thread2 AndAlso timeout < 10
                    System.Threading.Thread.Sleep(10)
                    timeout += 1
                End While

                ' Force abort if still running
                If temp_thread.IsAlive Then
                    temp_thread.Abort()
                End If
            End If

            wait_for_thread2 = False
        Catch e As Exception
            ' Thread might already be aborted or null
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
            System.IO.File.Create(GetVefxFileName()).Dispose()
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

                    ' Only create bgfx thread for FLANGER (slider < 3), not for CHORUS
                    If slider < 3 Then
                        temp_thread = New System.Threading.Thread(AddressOf chorus_thread)
                    End If


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

                    ' Only create bgfx thread for GARGLE (slider >= 4), not for DISTORTION
                    If slider >= 4 Then
                        temp_thread = New System.Threading.Thread(AddressOf gargle_thread)
                    End If

                Case 6
                    temp_file = eq_only_file
            End Select
            If slider5 >= 3 Then
                temp_file(11) = "LoudnessCorrection: State 1 ReferenceLevel " & (slider5 - 6) * -1 - 12 - If(num > 1 And num <= 3, Math.Abs(slider - 3) * 2, 0) & " ReferenceOffset 10 Attenuation 1.0"
                temp_file(13) = "Preamp: " & slider5 - 6 & "dB"
            Else
                temp_file(11) = "LoudnessCorrection: State 1 ReferenceLevel " & (slider5 * 2 - 9) * -1 - 12 - If(num > 1 And num <= 3, Math.Abs(slider - 3) * 2, 0) & " ReferenceOffset 10 Attenuation 1.0"
                temp_file(13) = "Preamp: " & slider5 * 2 - 9 & "dB"
            End If
            If num = 1 Then
                temp_file(11) = "LoudnessCorrection: State 1 ReferenceLevel " & (slider5 - 6) * -1 - 15 & " ReferenceOffset 10 Attenuation 1.0"
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
            temp_file(0) = "#" & num & " " & slider & " " & slider2 & " " & slider3 & " " & slider4 & " " & slider5 & " " & slider6 & " " & bgfx_toggleb

            Try
                ' Signal existing thread to stop
                SyncLock effectThreadLock
                    effectThreadAbort = True
                End SyncLock

                ' Wait for previous thread to finish with timeout
                Dim timeout As Integer = 0
                While wait_for_thread2 AndAlso timeout < 50
                    System.Threading.Thread.Sleep(10)
                    timeout += 1
                End While

                System.IO.File.WriteAllLines(GetVefxFileName(), temp_file)

                ' Reset abort flag and start new thread
                SyncLock effectThreadLock
                    effectThreadAbort = False
                End SyncLock
                temp_thread.Start()
            Catch x As Exception
            End Try

        End If
    End Sub

    Private Sub gargle_thread()
        Dim count As Integer = 3
        Dim flag As Boolean = False

        wait_for_thread2 = True
        Dim shouldExit As Boolean = False

        While True
            ' Check abort flag with lock
            SyncLock effectThreadLock
                If effectThreadAbort Then
                    shouldExit = True
                End If
            End SyncLock

            If shouldExit Then Exit While

            If count = 27 Then
                count = 3
            End If
            count += 1
            If (count Mod (7 - effector_slider)) = 0 Then
                flag = Not flag
            End If
            Try
                SyncLock effectThreadLock
                    If effectThreadAbort Then Exit While
                    ' Update preamp value
                    If flag Then
                        temp_file(22) = "Preamp: -18dB"
                    Else
                        temp_file(22) = "Preamp: 0dB"
                    End If
                    WriteAllLinesBuffered(GetVefxFileName(), temp_file)
                End SyncLock
            Catch x As Exception
                ' Continue on error instead of exiting
                System.Threading.Thread.Sleep(33)
            End Try

            System.Threading.Thread.Sleep(33)
        End While
        wait_for_thread2 = False
    End Sub

    Private Sub chorus_thread()
        Dim count As Integer = 99
        Dim flag As Boolean = False
        wait_for_thread2 = True
        Dim shouldExit As Boolean = False

        While True
            ' Check abort flag with lock
            SyncLock effectThreadLock
                If effectThreadAbort Then
                    shouldExit = True
                End If
            End SyncLock

            If shouldExit Then Exit While

            If count <= 33 Then
                flag = True
            ElseIf count >= 99 Then
                flag = False
            End If
            If flag Then
                count += 1
            Else
                count -= 1
            End If
            Try
                SyncLock effectThreadLock
                    If effectThreadAbort Then Exit While
                    ' Update delay value
                    temp_file(27) = "Delay: 0." & count & "ms"
                    WriteAllLinesBuffered(GetVefxFileName(), temp_file)
                End SyncLock
            Catch x As Exception
                ' Continue on error instead of exiting
                System.Threading.Thread.Sleep(33)
            End Try

            System.Threading.Thread.Sleep(33)
        End While
        wait_for_thread2 = False
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Cleanup VU Meter
        CleanupVUMeter()

        ' Stop device refresh timer
        If deviceRefreshTimer IsNot Nothing Then
            deviceRefreshTimer.Stop()
            deviceRefreshTimer.Enabled = False
        End If

        ' Check if bgfx is active - if so, minimize to tray instead of closing
        If bgfx_toggleb = 1 AndAlso effector_on <> 0 AndAlso (effector_num = 4 OrElse effector_num = 5) Then
            ' Cancel the close event
            e.Cancel = True
            ' Hide the form instead
            Me.Hide()
            ' Show notification that app is still running
            MsgBox("VEFX Slider minimized to background." & vbCrLf & vbCrLf & "BGFX effects are still active." & vbCrLf & "To completely exit, turn off the effect first or use Task Manager.", MsgBoxStyle.Information, "Running in Background")
            ' Restart timer if form is being hidden
            If deviceRefreshTimer IsNot Nothing Then
                deviceRefreshTimer.Start()
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        ' Load audio devices from system
        LoadAudioDevices()

        ' Initialize device refresh timer (refresh every 5 seconds)
        deviceRefreshTimer.Interval = 5000 ' 5 seconds
        deviceRefreshTimer.Enabled = True
        deviceRefreshTimer.Start()

        ' Load configuration from config.json (includes last connector and presets)
        LoadConfig()

        ' Initialize connector selector
        connector_selector.Items.Clear()
        For Each connectorName In connector_names
            connector_selector.Items.Add(connectorName)
        Next
        If connector_names.Count > 0 Then
            ' Find and select the currently active default audio device
            Dim defaultGuid As String = GetDefaultAudioDeviceGuid()
            Dim foundIndex As Integer = -1
            
            ' Search for the default device in our device list
            If Not String.IsNullOrEmpty(defaultGuid) Then
                For i As Integer = 0 To device_guids.Count - 1
                    If device_guids(i).Equals(defaultGuid, StringComparison.OrdinalIgnoreCase) Then
                        foundIndex = i
                        Exit For
                    End If
                Next
            End If
            
            ' Use found default device, or fall back to first device if not found
            If foundIndex >= 0 Then
                connector_selector.SelectedIndex = foundIndex
                current_connector_index = foundIndex
            Else
                connector_selector.SelectedIndex = 0
                current_connector_index = 0
            End If
            
            UpdateTitleBar()
            UpdateAPOStatusIndicator()
        End If

        ' Initialize connector settings dictionary
        For Each connectorName In connector_names
            If Not connector_settings.ContainsKey(connectorName) Then
                connector_settings.Add(connectorName, New ConnectorSettings With {
                    .EffectorOn = 0,
                    .EffectorNum = 1,
                    .EffectorSlider = 3,
                    .LowEQSlider = 3,
                    .HighEQSlider = 3,
                    .FilterSlider = 3,
                    .VolSlider = 3,
                    .ChannelSlider = 1,
                    .BgfxToggle = 1
                })
            End If
        Next

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

        ' Load custom presets on startup
        LoadCustomPresets()

        ' Initialize VU Meter
        InitializeVUMeter()
    End Sub

    ' Save current UI values to connector settings
    Private Sub SaveCurrentConnectorSettings()
        Dim connectorName As String = GetCurrentConnectorName()
        If connector_settings.ContainsKey(connectorName) Then
            Dim settings As ConnectorSettings = connector_settings(connectorName)
            settings.EffectorOn = effector_on
            settings.EffectorNum = effector_num
            settings.EffectorSlider = effector_slider
            settings.LowEQSlider = loweq_slider
            settings.HighEQSlider = hieq_slider
            settings.FilterSlider = filter_slider
            settings.VolSlider = vol_slider
            settings.ChannelSlider = channel_slider
            settings.BgfxToggle = bgfx_toggleb
            connector_settings(connectorName) = settings
        End If
    End Sub

    ' Load connector settings to UI
    Private Sub LoadCurrentConnectorSettings()
        Dim connectorName As String = GetCurrentConnectorName()
        If connector_settings.ContainsKey(connectorName) Then
            Dim settings As ConnectorSettings = connector_settings(connectorName)
            effector_on = settings.EffectorOn
            effector_num = settings.EffectorNum
            effector_slider = settings.EffectorSlider
            loweq_slider = settings.LowEQSlider
            hieq_slider = settings.HighEQSlider
            filter_slider = settings.FilterSlider
            vol_slider = settings.VolSlider
            channel_slider = settings.ChannelSlider
            bgfx_toggleb = settings.BgfxToggle

            ' Update UI controls
            VEFX.Value = effector_slider
            LOW_EQ.Value = loweq_slider
            HIGH_EQ.Value = hieq_slider
            FILTER.Value = filter_slider
            VOLUME.Value = vol_slider
            CHANNEL.Value = channel_slider

            If bgfx_toggleb = 1 Then
                bgfx_toggle.Text = "BGFX on"
                bgfx_toggle.ForeColor = Color.OrangeRed
            Else
                bgfx_toggle.Text = "BGFX off"
                bgfx_toggle.ForeColor = Color.Black
            End If

            rerun()
        End If
    End Sub

    Private Sub connector_selector_SelectedIndexChanged(sender As Object, e As EventArgs) Handles connector_selector.SelectedIndexChanged
        ' Save current connector settings before switching
        SaveCurrentConnectorSettings()

        ' Update current connector index
        current_connector_index = connector_selector.SelectedIndex

        ' Update title bar with new connector
        UpdateTitleBar()
        UpdateAPOStatusIndicator()

        ' Save config with new connector selection
        SaveConfig()

        ' Load new connector settings
        LoadCurrentConnectorSettings()

        ' Update VU meter to monitor the newly selected device
        UpdateVUMeterDevice()
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
        UpdateVUMeterDevice()

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
            bgfx_toggle.ForeColor = Color.Black
            bgfx_toggleb = 0
        Else                    'bgfx off
            bgfx_toggle.Text = "BGFX on"
            bgfx_toggle.ForeColor = Color.OrangeRed
            bgfx_toggleb = 1
        End If

        rerun()
    End Sub

    Private Sub ResetAudiosrvToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetAudiosrvToolStripMenuItem.Click
        Dim result = MsgBox("Are you sure?", MsgBoxStyle.OkCancel, "Reset Audiosrv")
        If result <> MsgBoxResult.Cancel Then
            If RestartAudioService() Then
                MsgBox("Done. Try restarting audio applications.", MsgBoxStyle.OkOnly, "Reset Audiosrv")
            End If
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("Made by hoholee12@gmail.com" & vbCrLf & "This application requires EqualizerAPO", MsgBoxStyle.OkOnly)
    End Sub

    Private Sub TunnelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TunnelToolStripMenuItem.Click
        effector_on = 1
        effector_num = 2
        VEFX.Value = 6
        VOLUME.Value = 3
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
        VOLUME.Value = 3
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
        VOLUME.Value = 3
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
        ' Install APO to currently selected device
        If connector_names.Count = 0 OrElse current_connector_index < 0 OrElse current_connector_index >= connector_names.Count Then
            MsgBox("No audio device selected.", MsgBoxStyle.Exclamation, "APO Installation")
            Return
        End If

        Dim currentDeviceName As String = connector_names(current_connector_index)
        Dim deviceGuid As String = device_guids(current_connector_index)

        If String.IsNullOrEmpty(deviceGuid) Then
            MsgBox("Could not find device GUID for: " & currentDeviceName, MsgBoxStyle.Exclamation, "APO Installation")
            Return
        End If

        ' Check if APO is already installed
        If CheckAPOInstalled(deviceGuid) Then
            Dim result As MsgBoxResult = MsgBox("EqualizerAPO is already installed on device: " & currentDeviceName & vbCrLf & vbCrLf & "Do you want to uninstall it?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "APO Already Installed")
            If result = MsgBoxResult.Yes Then
                If UninstallAPOFromDevice(deviceGuid) Then
                    MsgBox("APO uninstalled successfully." & vbCrLf & vbCrLf & "The audio service will now restart to apply changes.", MsgBoxStyle.Information, "Uninstallation Complete")
                    RestartAudioService()
                    UpdateAPOStatusIndicator()
                    UpdateVUMeterDevice()
                Else
                    MsgBox("Failed to uninstall APO.", MsgBoxStyle.Exclamation, "Uninstallation Failed")
                End If
            End If
            Return
        End If

        ' Ask user to select install mode using custom dialog
        Dim installMode As Integer = ShowInstallModeDialog()
        If installMode = -1 Then
            ' User cancelled
            Return
        End If

        Dim modeNames As String() = {"LFX/GFX (Legacy)", "SFX/MFX (Win 8.1+)", "SFX/EFX (Win 8.1+)"}

        ' Confirmation before installation
        Dim confirmResult As MsgBoxResult = MsgBox("Install EqualizerAPO to device:" & vbCrLf & currentDeviceName & vbCrLf & vbCrLf & "Selected Mode: " & modeNames(installMode), MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Confirm APO Installation")
        If confirmResult <> MsgBoxResult.Yes Then
            Return
        End If

        If InstallAPOToDevice(deviceGuid, installMode) Then
            MsgBox("APO installed successfully." & vbCrLf & vbCrLf & "The audio service will now restart to apply changes.", MsgBoxStyle.Information, "Installation Complete")
            RestartAudioService()
            UpdateAPOStatusIndicator()
            UpdateVUMeterDevice()
        Else
            MsgBox("Failed to install APO.", MsgBoxStyle.Exclamation, "Installation Failed")
        End If
    End Sub

    Private Sub TheaterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TheaterToolStripMenuItem.Click
        effector_on = 1
        effector_num = 2
        VEFX.Value = 2
        VOLUME.Value = 3
        FILTER.Value = 5
        HIGH_EQ.Value = 4
        LOW_EQ.Value = 6
        effector_slider = VEFX.Value
        vol_slider = VOLUME.Value
        filter_slider = FILTER.Value
        loweq_slider = LOW_EQ.Value
        hieq_slider = HIGH_EQ.Value
        rerun()
    End Sub

    Private Sub DNRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DNRToolStripMenuItem.Click
        effector_on = 1
        effector_num = 1
        VEFX.Value = 4
        VOLUME.Value = 6
        FILTER.Value = 2
        HIGH_EQ.Value = 5
        LOW_EQ.Value = 3
        effector_slider = VEFX.Value
        vol_slider = VOLUME.Value
        filter_slider = FILTER.Value
        loweq_slider = LOW_EQ.Value
        hieq_slider = HIGH_EQ.Value
        rerun()
    End Sub

    Private Sub AddPresetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddPresetToolStripMenuItem.Click
        ' Prompt user for preset name
        Dim presetName As String = InputBox("Enter a name for this preset:", "Add Custom Preset", "")

        ' Validate input
        If String.IsNullOrWhiteSpace(presetName) Then
            Return ' User cancelled or entered empty name
        End If

        ' Check if preset already exists
        If customPresets.ContainsKey(presetName) Then
            Dim result = MsgBox("A preset with this name already exists. Overwrite?", MsgBoxStyle.YesNo, "Preset Exists")
            If result = MsgBoxResult.No Then
                Return
            End If
        End If

        ' Save current slider values
        ' Save current slider values
        Dim preset As New PresetData With {
            .EffectorOn = effector_on,
            .EffectorNum = effector_num,
            .VEFXValue = VEFX.Value,
            .VolumeValue = VOLUME.Value,
            .FilterValue = FILTER.Value,
            .HighEQValue = HIGH_EQ.Value,
            .LowEQValue = LOW_EQ.Value,
            .ChannelValue = CHANNEL.Value,
            .BgfxValue = bgfx_toggleb
        }
        ' Add or update preset
        If customPresets.ContainsKey(presetName) Then
            customPresets(presetName) = preset
        Else
            customPresets.Add(presetName, preset)
        End If

        ' Save to file
        SaveCustomPresets()

        ' Refresh the menu
        LoadCustomPresets()

        MsgBox("Preset '" & presetName & "' saved successfully!", MsgBoxStyle.OkOnly, "Preset Saved")
    End Sub

    ' Save configuration to config.json
    Private Sub SaveConfig()
        Try
            Dim config As New ConfigData With {
                .CustomPresets = customPresets
            }

            Dim serializer As New JavaScriptSerializer()
            Dim json As String = serializer.Serialize(config)
            System.IO.File.WriteAllText(configFilePath, json, System.Text.Encoding.UTF8)
        Catch ex As Exception
            MsgBox("Error saving config: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    ' Load configuration from config.json
    Private Sub LoadConfig()
        Try
            If System.IO.File.Exists(configFilePath) Then
                Dim json As String = System.IO.File.ReadAllText(configFilePath, System.Text.Encoding.UTF8)
                Dim serializer As New JavaScriptSerializer()
                Dim config As ConfigData = serializer.Deserialize(Of ConfigData)(json)

                ' Load custom presets
                If config.CustomPresets IsNot Nothing Then
                    customPresets = config.CustomPresets
                End If
            End If
        Catch ex As Exception
            MsgBox("Error loading config: " & ex.Message, MsgBoxStyle.Exclamation, "Error")
        End Try
    End Sub

    Private Sub SaveCustomPresets()
        ' Now uses config.json instead
        SaveConfig()
    End Sub
    Private Sub LoadCustomPresets()
        ' Clear existing custom preset menu items (items after the separator)
        Dim separatorIndex As Integer = -1
        For i As Integer = 0 To PresetsToolStripMenuItem.DropDownItems.Count - 1
            If TypeOf PresetsToolStripMenuItem.DropDownItems(i) Is ToolStripSeparator Then
                separatorIndex = i
                Exit For
            End If
        Next

        If separatorIndex >= 0 Then
            ' Remove all items after "Add Preset..."
            For i As Integer = PresetsToolStripMenuItem.DropDownItems.Count - 1 To separatorIndex + 2 Step -1
                PresetsToolStripMenuItem.DropDownItems.RemoveAt(i)
            Next
        End If

        ' Add menu items from loaded presets (already loaded from config.json in LoadConfig)
        Try
            For Each kvp In customPresets
                Dim menuItem As New ToolStripMenuItem(kvp.Key)
                AddHandler menuItem.Click, Sub(s, ev) ApplyCustomPreset(kvp.Key)
                PresetsToolStripMenuItem.DropDownItems.Add(menuItem)
            Next
        Catch ex As Exception
            MsgBox("Error loading preset menu items: " & ex.Message, MsgBoxStyle.Exclamation, "Error")
        End Try
    End Sub

    Private Sub ApplyCustomPreset(presetName As String)
        If customPresets.ContainsKey(presetName) Then
            Dim preset As PresetData = customPresets(presetName)
            effector_on = preset.EffectorOn
            effector_num = preset.EffectorNum
            VEFX.Value = preset.VEFXValue
            VOLUME.Value = preset.VolumeValue
            FILTER.Value = preset.FilterValue
            HIGH_EQ.Value = preset.HighEQValue
            LOW_EQ.Value = preset.LowEQValue
            CHANNEL.Value = preset.ChannelValue
            bgfx_toggleb = preset.BgfxValue
            effector_slider = VEFX.Value
            vol_slider = VOLUME.Value
            filter_slider = FILTER.Value
            loweq_slider = LOW_EQ.Value
            hieq_slider = HIGH_EQ.Value
            channel_slider = CHANNEL.Value

            ' Update bgfx button text and color
            If bgfx_toggleb = 1 Then
                bgfx_toggle.Text = "BGFX on"
                bgfx_toggle.ForeColor = Color.OrangeRed
            Else
                bgfx_toggle.Text = "BGFX off"
                bgfx_toggle.ForeColor = Color.Black
            End If

            rerun()
        End If
    End Sub

#Region "APO Device Management"
    ' Constants for registry paths
    Private Const AUDIO_DEVICES_PATH As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\MMDevices\Audio"
    Private Const RENDER_PATH As String = AUDIO_DEVICES_PATH & "\Render"
    Private Const CAPTURE_PATH As String = AUDIO_DEVICES_PATH & "\Capture"

    ' EqualizerAPO GUIDs
    Private Const EQUALIZERAPO_PRE_MIX_GUID As String = "{EACD2258-FCAC-4FF4-B36D-419E924A6D79}"
    Private Const EQUALIZERAPO_POST_MIX_GUID As String = "{EC1CC9CE-FAED-4822-828A-82A81A6F018F}"

    ' Helper function to take ownership and grant full access to registry key
    Private Sub TakeRegistryKeyOwnership(keyPath As String)
        Dim errors As String = ""

        ' Enable SE_TAKE_OWNERSHIP_NAME privilege
        Dim tokenHandle As IntPtr = IntPtr.Zero
        Try
            If Not OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES Or TOKEN_QUERY, tokenHandle) Then
                errors &= "Failed to open process token" & vbCrLf
                Return
            End If

            Dim luid As LUID
            If Not LookupPrivilegeValue(Nothing, SE_TAKE_OWNERSHIP_NAME, luid) Then
                errors &= "Failed to lookup privilege value" & vbCrLf
                Return
            End If

            Dim tp As TOKEN_PRIVILEGES
            tp.PrivilegeCount = 1
            tp.Privileges.Luid = luid
            tp.Privileges.Attributes = SE_PRIVILEGE_ENABLED

            If Not AdjustTokenPrivileges(tokenHandle, False, tp, CUInt(Marshal.SizeOf(tp)), IntPtr.Zero, IntPtr.Zero) Then
                errors &= "Failed to adjust token privileges" & vbCrLf
                Return
            End If
        Finally
            If tokenHandle <> IntPtr.Zero Then
                Marshal.FreeHGlobal(tokenHandle)
            End If
        End Try

        Try
            Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using key As RegistryKey = baseKey.OpenSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.TakeOwnership Or RegistryRights.ChangePermissions)
                    If key IsNot Nothing Then
                        Dim acl As RegistrySecurity = key.GetAccessControl()

                        ' Set owner to Administrators group
                        Dim adminsSid As New SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, Nothing)
                        acl.SetOwner(adminsSid)
                        key.SetAccessControl(acl)

                        ' Grant full control to Administrators
                        Dim rule As New RegistryAccessRule(adminsSid, RegistryRights.FullControl, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow)
                        acl.SetAccessRule(rule)
                        key.SetAccessControl(acl)
                    End If
                End Using
            End Using
        Catch ex As Exception
            errors &= "Device key: " & ex.Message & vbCrLf & ex.StackTrace
        End Try

        ' Also take ownership of FxProperties subkey if it exists
        Dim fxPath As String = keyPath & "\FxProperties"
        Try
            Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using key As RegistryKey = baseKey.OpenSubKey(fxPath, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.TakeOwnership Or RegistryRights.ChangePermissions)
                    If key IsNot Nothing Then
                        Dim acl As RegistrySecurity = key.GetAccessControl()
                        Dim adminsSid As New SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, Nothing)
                        acl.SetOwner(adminsSid)
                        key.SetAccessControl(acl)

                        Dim rule As New RegistryAccessRule(adminsSid, RegistryRights.FullControl, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow)
                        acl.SetAccessRule(rule)
                        key.SetAccessControl(acl)
                    End If
                End Using
            End Using
        Catch ex As Exception
            errors &= "FxProperties key: " & ex.Message & vbCrLf & ex.StackTrace
        End Try
    End Sub

    ' FX Property registry value names for APO installation
    Private Const LFX_GUID As String = "{d04e05a6-594b-4fb6-a80d-01af5eed7d1d},1"
    Private Const GFX_GUID As String = "{d04e05a6-594b-4fb6-a80d-01af5eed7d1d},2"
    Private Const SFX_GUID As String = "{d04e05a6-594b-4fb6-a80d-01af5eed7d1d},5"
    Private Const MFX_GUID As String = "{d04e05a6-594b-4fb6-a80d-01af5eed7d1d},6"
    Private Const EFX_GUID As String = "{d04e05a6-594b-4fb6-a80d-01af5eed7d1d},7"
    Private Const MULTI_SFX_GUID As String = "{d04e05a6-594b-4fb6-a80d-01af5eed7d1d},13"
    Private Const MULTI_MFX_GUID As String = "{d04e05a6-594b-4fb6-a80d-01af5eed7d1d},14"
    Private Const MULTI_EFX_GUID As String = "{d04e05a6-594b-4fb6-a80d-01af5eed7d1d},15"
    Private Const COMBINED_DEVICE_VALUE As String = "{b3f8fa53-0004-438e-9003-51a46e139bfc},41"
    Private Const PROCESSING_MODE As String = "{C18E2F7E-933D-4965-B7D1-1EEF228D2AF3}"
    Private Const CHILD_APO_PATH As String = "SOFTWARE\EqualizerAPO\Child APOs"
    Private Const FX_TITLE_VALUE As String = "{b725f130-47ef-101a-a5f1-02608c9eebac},10"
    Private Const PROTECTED_AUDIODG_PATH As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"
    Private Const APO_REGISTRATION_PATH As String = "SOFTWARE\Classes\AudioEngine\AudioProcessingObjects"

    Private Function CheckAndFixProtectedAudioDG() As Boolean
        Try
            Using key As RegistryKey = Registry.LocalMachine.OpenSubKey(PROTECTED_AUDIODG_PATH, True)
                If key Is Nothing Then Return False

                Dim value As Object = key.GetValue("DisableProtectedAudioDG")
                If value Is Nothing OrElse CInt(value) <> 1 Then
                    ' Need to set the value
                    key.SetValue("DisableProtectedAudioDG", 1, RegistryValueKind.DWord)
                    Return True ' Indicates we changed it (reboot may be needed)
                End If
            End Using
            Return False ' Already set correctly
        Catch ex As Exception
            MsgBox("Warning: Could not set DisableProtectedAudioDG." & vbCrLf & ex.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function

    Private Function GetAllDeviceGuidsByName(deviceName As String) As List(Of String)
        ' Search for device GUID by matching device name
        Dim debugInfo As String = "Searching for: '" & deviceName & "'" & vbCrLf
        debugInfo &= "Full registry path: HKEY_LOCAL_MACHINE\" & RENDER_PATH & vbCrLf & vbCrLf

        Try
            ' Try opening with 64-bit registry view
            Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using key As RegistryKey = baseKey.OpenSubKey(RENDER_PATH)
                    If key Is Nothing Then
                        debugInfo &= "ERROR: Could not open registry key (64-bit view)!" & vbCrLf
                        debugInfo &= "Trying 32-bit registry view..." & vbCrLf & vbCrLf

                        ' Try 32-bit view
                        Using baseKey32 As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                            Using key32 As RegistryKey = baseKey32.OpenSubKey(RENDER_PATH)
                                If key32 Is Nothing Then
                                    debugInfo &= "ERROR: Could not open registry key (32-bit view either)!" & vbCrLf
                                    Return Nothing
                                End If

                                Return SearchInKey(key32, deviceName, debugInfo, "32-bit")
                            End Using
                        End Using
                    Else
                        Return SearchInKey(key, deviceName, debugInfo, "64-bit")
                    End If
                End Using
            End Using
        Catch ex As Exception
            debugInfo &= vbCrLf & "EXCEPTION: " & ex.Message & vbCrLf & ex.StackTrace
            Return Nothing
        End Try
    End Function

    Private Function SearchInKey(key As RegistryKey, deviceName As String, ByRef debugInfo As String, viewName As String) As List(Of String)
        Dim matchingGuids As New List(Of String)
        Dim subKeys() As String = key.GetSubKeyNames()
        debugInfo &= "Registry view: " & viewName & vbCrLf
        debugInfo &= "Found " & subKeys.Length & " device(s):" & vbCrLf & vbCrLf

        For Each deviceGuid In subKeys
            Dim name As String = GetDeviceName(deviceGuid)
            debugInfo &= "  " & name & vbCrLf & "    GUID: " & deviceGuid & vbCrLf

            Dim isMatch As Boolean = False

            ' Try exact match
            If name.Equals(deviceName, StringComparison.OrdinalIgnoreCase) Then
                isMatch = True
            End If

            ' Try partial match (contains)
            If name.IndexOf(deviceName, StringComparison.OrdinalIgnoreCase) >= 0 Then
                isMatch = True
            End If

            ' Try reverse partial match (deviceName contains registry name)
            If deviceName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 Then
                isMatch = True
            End If

            If isMatch Then
                matchingGuids.Add(deviceGuid)
                debugInfo &= "    MATCHED" & vbCrLf
            End If
        Next

        Return matchingGuids
    End Function

    Private Function GetDeviceName(deviceGuid As String) As String
        Try
            ' Use 64-bit registry view explicitly
            Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using key As RegistryKey = baseKey.OpenSubKey(RENDER_PATH & "\" & deviceGuid & "\Properties")
                    If key IsNot Nothing Then
                        ' Try connection name first (this is the connector name like "스피커", "Speakers")
                        Dim name = key.GetValue("{a45c254e-df1c-4efd-8020-67d146a850e0},2")
                        If name IsNot Nothing AndAlso Not String.IsNullOrEmpty(name.ToString()) Then
                            Return name.ToString()
                        End If

                        ' Fallback to device description (driver name like "Realtek Audio")
                        name = key.GetValue("{b3f8fa53-0004-438e-9003-51a46e139bfc},6")
                        If name IsNot Nothing AndAlso Not String.IsNullOrEmpty(name.ToString()) Then
                            Return name.ToString()
                        End If
                    End If
                End Using
            End Using
        Catch
        End Try
        Return deviceGuid.Substring(0, Math.Min(8, deviceGuid.Length)) & "..."
    End Function

    Private Function GetDeviceConnectorName(deviceGuid As String) As String
        Try
            Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using key As RegistryKey = baseKey.OpenSubKey(RENDER_PATH & "\" & deviceGuid & "\Properties")
                    If key IsNot Nothing Then
                        ' Get connection name (connector like "Speakers", "Headphones")
                        Dim name = key.GetValue("{a45c254e-df1c-4efd-8020-67d146a850e0},2")
                        If name IsNot Nothing Then
                            Return name.ToString()
                        End If
                    End If
                End Using
            End Using
        Catch
        End Try
        Return ""
    End Function

    Private Function GetDeviceDescription(deviceGuid As String) As String
        Try
            Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using key As RegistryKey = baseKey.OpenSubKey(RENDER_PATH & "\" & deviceGuid & "\Properties")
                    If key IsNot Nothing Then
                        ' Get device description (driver/hardware name like "Realtek Audio", "USB Audio")
                        Dim name = key.GetValue("{b3f8fa53-0004-438e-9003-51a46e139bfc},6")
                        If name IsNot Nothing Then
                            Return name.ToString()
                        End If
                    End If
                End Using
            End Using
        Catch
        End Try
        Return ""
    End Function

    ' Check if device is active (connected and enabled)
    ' DeviceState values: 1=ACTIVE, 2=DISABLED, 4=NOTPRESENT, 8=UNPLUGGED
    Private Function IsDeviceActive(deviceGuid As String) As Boolean
        Try
            Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using key As RegistryKey = baseKey.OpenSubKey(RENDER_PATH & "\" & deviceGuid)
                    If key IsNot Nothing Then
                        Dim stateValue = key.GetValue("DeviceState")
                        If stateValue IsNot Nothing Then
                            Dim state As Integer = CInt(stateValue)
                            Return state = 1 ' Only return true if device state is ACTIVE (1)
                        End If
                    End If
                End Using
            End Using
        Catch
        End Try
        Return False ' If we can't determine state, assume inactive
    End Function

    Private Function CheckAPOInstalled(deviceGuid As String) As Boolean
        Try
            Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using key As RegistryKey = baseKey.OpenSubKey(RENDER_PATH & "\" & deviceGuid & "\FxProperties")
                    If key Is Nothing Then Return False

                    For Each valueName In {LFX_GUID, GFX_GUID, SFX_GUID, MFX_GUID, EFX_GUID}
                        Dim value = key.GetValue(valueName)
                        If value IsNot Nothing Then
                            Dim guid As String = value.ToString()
                            If guid.Equals(EQUALIZERAPO_PRE_MIX_GUID, StringComparison.OrdinalIgnoreCase) OrElse
                               guid.Equals(EQUALIZERAPO_POST_MIX_GUID, StringComparison.OrdinalIgnoreCase) Then
                                Return True
                            End If
                        End If
                    Next
                End Using
            End Using
        Catch
        End Try
        Return False
    End Function

    Private Function InstallAPOToDevice(deviceGuid As String, mode As Integer) As Boolean
        Dim hKey As IntPtr = IntPtr.Zero
        Dim hChildKey As IntPtr = IntPtr.Zero
        Dim fxPropertiesExisted As Boolean = False

        Try
            Dim devicePath As String = RENDER_PATH & "\" & deviceGuid
            Dim fxPath As String = devicePath & "\FxProperties"
            Dim childPath As String = CHILD_APO_PATH & "\" & deviceGuid

            ' Check if FxProperties already exists
            Dim hTestKey As IntPtr = IntPtr.Zero
            Dim testResult As Integer = RegOpenKeyEx(HKEY_LOCAL_MACHINE, fxPath, 0, KEY_QUERY_VALUE Or KEY_WOW64_64KEY, hTestKey)
            If testResult = ERROR_SUCCESS Then
                fxPropertiesExisted = True
                RegCloseKey(hTestKey)
            End If

            ' Create Child APO backup registry structure
            Dim result As Integer = RegOpenKeyEx(HKEY_LOCAL_MACHINE, CHILD_APO_PATH, 0, KEY_CREATE_SUB_KEY Or KEY_WOW64_64KEY, hChildKey)
            If result <> ERROR_SUCCESS Then
                ' CHILD_APO_PATH doesn't exist, create it
                result = RegCreateKeyEx(HKEY_LOCAL_MACHINE, CHILD_APO_PATH, 0, Nothing, 0, KEY_CREATE_SUB_KEY Or KEY_WOW64_64KEY, IntPtr.Zero, hChildKey, IntPtr.Zero)
                If result <> ERROR_SUCCESS Then
                    MsgBox("Cannot create Child APO registry path." & vbCrLf & "Error: " & result, MsgBoxStyle.Critical)
                    Return False
                End If
            End If

            Dim hDeviceKey As IntPtr = IntPtr.Zero
            result = RegCreateKeyEx(hChildKey, deviceGuid, 0, Nothing, 0, KEY_SET_VALUE Or KEY_WOW64_64KEY, IntPtr.Zero, hDeviceKey, IntPtr.Zero)
            RegCloseKey(hChildKey)
            hChildKey = hDeviceKey

            If result <> ERROR_SUCCESS Then
                MsgBox("Cannot create device backup key.", MsgBoxStyle.Critical)
                Return False
            End If

            ' Try to open FxProperties, create if needed
            result = RegOpenKeyEx(HKEY_LOCAL_MACHINE, fxPath, 0, KEY_SET_VALUE Or KEY_QUERY_VALUE Or KEY_WOW64_64KEY, hKey)
            If result <> ERROR_SUCCESS Then
                ' FxProperties doesn't exist - try to create it
                Dim hDevicePathKey As IntPtr = IntPtr.Zero
                result = RegOpenKeyEx(HKEY_LOCAL_MACHINE, devicePath, 0, KEY_CREATE_SUB_KEY Or KEY_WOW64_64KEY, hDevicePathKey)
                If result <> ERROR_SUCCESS Then
                    MsgBox("Cannot access device registry path. Try running as administrator.", MsgBoxStyle.Critical)
                    Return False
                End If

                Dim hNewFxKey As IntPtr = IntPtr.Zero
                result = RegCreateKeyEx(hDevicePathKey, "FxProperties", 0, Nothing, 0, KEY_SET_VALUE Or KEY_QUERY_VALUE Or KEY_WOW64_64KEY, IntPtr.Zero, hNewFxKey, IntPtr.Zero)
                RegCloseKey(hDevicePathKey)

                If result <> ERROR_SUCCESS Then
                    MsgBox("Cannot create FxProperties key. Insufficient permissions." & vbCrLf & "Error: " & result, MsgBoxStyle.Critical)
                    Return False
                End If

                hKey = hNewFxKey
                fxPropertiesExisted = False

                ' Set FxTitle for new FxProperties
                Dim fxTitle As String = "Equalizer APO"
                RegSetValueEx(hKey, FX_TITLE_VALUE, 0, REG_SZ, fxTitle, CUInt((fxTitle.Length + 1) * 2))
            End If

            ' Backup original APO GUIDs before modifying
            If fxPropertiesExisted Then
                For Each valueName In {LFX_GUID, GFX_GUID, SFX_GUID, MFX_GUID, EFX_GUID}
                    Dim buffer(512) As Byte
                    Dim bufferSize As UInteger = CUInt(buffer.Length)
                    Dim valueType As UInteger = 0
                    Dim readResult As Integer = RegQueryValueEx(hKey, valueName, IntPtr.Zero, valueType, buffer, bufferSize)

                    If readResult = ERROR_SUCCESS Then
                        ' Check if it's already our GUID - mark as !VALUE if so
                        Dim existingGuid As String = System.Text.Encoding.Unicode.GetString(buffer, 0, CInt(bufferSize - 2))
                        If existingGuid.Equals(EQUALIZERAPO_PRE_MIX_GUID, StringComparison.OrdinalIgnoreCase) OrElse
                           existingGuid.Equals(EQUALIZERAPO_POST_MIX_GUID, StringComparison.OrdinalIgnoreCase) Then
                            Dim noValue As String = "!VALUE"
                            RegSetValueEx(hChildKey, valueName, 0, REG_SZ, noValue, CUInt((noValue.Length + 1) * 2))
                        Else
                            ' Save original GUID
                            RegSetValueEx(hChildKey, valueName, 0, valueType, buffer, bufferSize)
                        End If
                    Else
                        ' No original value - mark as !VALUE
                        Dim noValue As String = "!VALUE"
                        RegSetValueEx(hChildKey, valueName, 0, REG_SZ, noValue, CUInt((noValue.Length + 1) * 2))
                    End If
                Next
            Else
                ' FxProperties didn't exist - mark all as !KEY
                For Each valueName In {LFX_GUID, GFX_GUID, SFX_GUID, MFX_GUID, EFX_GUID}
                    Dim noKey As String = "!KEY"
                    RegSetValueEx(hChildKey, valueName, 0, REG_SZ, noKey, CUInt((noKey.Length + 1) * 2))
                Next
            End If

            ' Write version number
            Dim versionStr As String = "2"
            RegSetValueEx(hChildKey, "Version", 0, REG_SZ, versionStr, CUInt((versionStr.Length + 1) * 2))

            ' Install based on mode - SELECTIVE deletion/setting
            Select Case mode
                Case 0 ' LFX/GFX mode
                    ' Set LFX and GFX
                    RegSetValueEx(hKey, LFX_GUID, 0, REG_SZ, EQUALIZERAPO_PRE_MIX_GUID, CUInt((EQUALIZERAPO_PRE_MIX_GUID.Length + 1) * 2))
                    RegSetValueEx(hKey, GFX_GUID, 0, REG_SZ, EQUALIZERAPO_POST_MIX_GUID, CUInt((EQUALIZERAPO_POST_MIX_GUID.Length + 1) * 2))
                    ' Delete incompatible SFX/MFX/EFX
                    RegDeleteValue(hKey, SFX_GUID)
                    RegDeleteValue(hKey, MFX_GUID)
                    RegDeleteValue(hKey, EFX_GUID)

                Case 1 ' SFX/MFX mode
                    ' Delete incompatible LFX/GFX
                    RegDeleteValue(hKey, LFX_GUID)
                    RegDeleteValue(hKey, GFX_GUID)
                    ' Set SFX and MFX with processing modes
                    RegSetValueEx(hKey, SFX_GUID, 0, REG_SZ, EQUALIZERAPO_PRE_MIX_GUID, CUInt((EQUALIZERAPO_PRE_MIX_GUID.Length + 1) * 2))
                    RegSetValueEx(hKey, MFX_GUID, 0, REG_SZ, EQUALIZERAPO_POST_MIX_GUID, CUInt((EQUALIZERAPO_POST_MIX_GUID.Length + 1) * 2))
                    ' Set processing modes with proper MULTI_SZ format (double null terminated)
                    Dim processingModeBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(PROCESSING_MODE & vbNullChar & vbNullChar)
                    RegSetValueEx(hKey, "{d3993a3f-99c2-4402-b5ec-a92a0367664b},5", 0, REG_MULTI_SZ, processingModeBytes, CUInt(processingModeBytes.Length))
                    RegSetValueEx(hKey, "{d3993a3f-99c2-4402-b5ec-a92a0367664b},6", 0, REG_MULTI_SZ, processingModeBytes, CUInt(processingModeBytes.Length))
                    ' Don't touch EFX - leave it for driver if it exists

                Case 2 ' SFX/EFX mode
                    ' Delete incompatible LFX/GFX
                    RegDeleteValue(hKey, LFX_GUID)
                    RegDeleteValue(hKey, GFX_GUID)
                    ' Set SFX and EFX with processing modes
                    RegSetValueEx(hKey, SFX_GUID, 0, REG_SZ, EQUALIZERAPO_PRE_MIX_GUID, CUInt((EQUALIZERAPO_PRE_MIX_GUID.Length + 1) * 2))
                    RegSetValueEx(hKey, EFX_GUID, 0, REG_SZ, EQUALIZERAPO_POST_MIX_GUID, CUInt((EQUALIZERAPO_POST_MIX_GUID.Length + 1) * 2))
                    ' Set processing modes with proper MULTI_SZ format (double null terminated)
                    Dim processingModeBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(PROCESSING_MODE & vbNullChar & vbNullChar)
                    RegSetValueEx(hKey, "{d3993a3f-99c2-4402-b5ec-a92a0367664b},5", 0, REG_MULTI_SZ, processingModeBytes, CUInt(processingModeBytes.Length))
                    RegSetValueEx(hKey, "{d3993a3f-99c2-4402-b5ec-a92a0367664b},7", 0, REG_MULTI_SZ, processingModeBytes, CUInt(processingModeBytes.Length))
                    ' Don't touch MFX - leave it for driver if it exists
            End Select

            ' Force enable audio enhancements (delete the disable flag)
            'RegDeleteValue(hKey, "{1da5d803-d492-4edd-8c23-e0c0ffee7f0e},5") -> bluetooth volume control does not like this. delete

            RegCloseKey(hKey)
            hKey = IntPtr.Zero
            RegCloseKey(hChildKey)
            hChildKey = IntPtr.Zero

            Return True

        Catch ex As Exception
            MsgBox("Install error: " & ex.GetType().Name & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.StackTrace, MsgBoxStyle.Exclamation)
            Return False
        Finally
            If hKey <> IntPtr.Zero Then
                RegCloseKey(hKey)
            End If
            If hChildKey <> IntPtr.Zero Then
                RegCloseKey(hChildKey)
            End If
        End Try
    End Function

    Private Function UninstallAPOFromDevice(deviceGuid As String) As Boolean
        Dim hKey As IntPtr = IntPtr.Zero
        Dim hChildKey As IntPtr = IntPtr.Zero
        Try
            Dim fxPath As String = RENDER_PATH & "\" & deviceGuid & "\FxProperties"
            Dim childPath As String = CHILD_APO_PATH & "\" & deviceGuid

            ' Check if we have backup data in Child APO registry
            Dim result As Integer = RegOpenKeyEx(HKEY_LOCAL_MACHINE, childPath, 0, KEY_QUERY_VALUE Or KEY_WOW64_64KEY, hChildKey)
            If result <> ERROR_SUCCESS Then
                ' No backup exists - just delete EqualizerAPO GUIDs
                result = RegOpenKeyEx(HKEY_LOCAL_MACHINE, fxPath, 0, KEY_SET_VALUE Or KEY_WOW64_64KEY, hKey)
                If result = ERROR_SUCCESS Then
                    For Each valueName In {LFX_GUID, GFX_GUID, SFX_GUID, MFX_GUID, EFX_GUID}
                        RegDeleteValue(hKey, valueName)
                    Next
                    RegCloseKey(hKey)
                End If
                Return True
            End If

            ' Read the first backed-up value to check if FxProperties existed originally
            Dim buffer(512) As Byte
            Dim bufferSize As UInteger = CUInt(buffer.Length)
            Dim valueType As UInteger = 0
            result = RegQueryValueEx(hChildKey, LFX_GUID, IntPtr.Zero, valueType, buffer, bufferSize)

            Dim firstValue As String = ""
            If result = ERROR_SUCCESS AndAlso bufferSize > 0 Then
                firstValue = System.Text.Encoding.Unicode.GetString(buffer, 0, CInt(bufferSize - 2)) ' -2 to exclude null terminator
            End If

            ' If first value is "!KEY", FxProperties didn't exist - we should delete the entire key
            ' But we can't easily delete keys with low-level API, so just delete all our values
            ' The system will clean up empty keys

            ' Open FxProperties for writing
            result = RegOpenKeyEx(HKEY_LOCAL_MACHINE, fxPath, 0, KEY_SET_VALUE Or KEY_WOW64_64KEY, hKey)
            If result <> ERROR_SUCCESS Then
                RegCloseKey(hChildKey)
                Return False
            End If

            ' Restore each backed-up value
            For Each valueName In {LFX_GUID, GFX_GUID, SFX_GUID, MFX_GUID, EFX_GUID}
                bufferSize = CUInt(buffer.Length)
                valueType = 0
                result = RegQueryValueEx(hChildKey, valueName, IntPtr.Zero, valueType, buffer, bufferSize)

                If result = ERROR_SUCCESS AndAlso bufferSize > 0 Then
                    Dim originalValue As String = System.Text.Encoding.Unicode.GetString(buffer, 0, CInt(bufferSize - 2))

                    If originalValue = "!KEY" OrElse originalValue = "!VALUE" OrElse originalValue = "" Then
                        ' No original value existed - delete it
                        RegDeleteValue(hKey, valueName)
                    Else
                        ' Restore original value
                        RegSetValueEx(hKey, valueName, 0, valueType, buffer, bufferSize)
                    End If
                Else
                    ' Couldn't read backup - just delete
                    RegDeleteValue(hKey, valueName)
                End If
            Next

            RegCloseKey(hKey)
            hKey = IntPtr.Zero
            RegCloseKey(hChildKey)
            hChildKey = IntPtr.Zero

            ' Delete the Child APO backup key
            ' We need to use .NET Registry classes for this as RegDeleteKey requires recursion
            Try
                Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                    Using parentKey As RegistryKey = baseKey.OpenSubKey(CHILD_APO_PATH, True)
                        If parentKey IsNot Nothing Then
                            parentKey.DeleteSubKey(deviceGuid, False)
                        End If
                    End Using
                End Using
            Catch
                ' Ignore errors deleting backup key
            End Try

            Return True
        Catch ex As Exception
            MsgBox("Uninstall error: " & ex.GetType().Name & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.StackTrace, MsgBoxStyle.Exclamation)
            Return False
        Finally
            If hKey <> IntPtr.Zero Then
                RegCloseKey(hKey)
            End If
            If hChildKey <> IntPtr.Zero Then
                RegCloseKey(hChildKey)
            End If
        End Try
    End Function

    Private Function RestartAudioService() As Boolean
        Try
            Dim psi As New ProcessStartInfo()
            psi.FileName = "net.exe"
            psi.Arguments = "stop Audiosrv"
            psi.Verb = "runas"
            psi.UseShellExecute = True
            psi.WindowStyle = ProcessWindowStyle.Hidden
            Process.Start(psi).WaitForExit()

            Threading.Thread.Sleep(1000)

            psi.Arguments = "start Audiosrv"
            Process.Start(psi).WaitForExit()

            ' Wait a bit longer for audio service to fully initialize
            Threading.Thread.Sleep(1000)
            
            ' Update VU meter device after audio service restart
            UpdateVUMeterDevice()

            Return True
        Catch ex As Exception
            MsgBox("Error restarting audio: " & ex.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function

    ' Timer event handler to refresh device list periodically
    Private Sub deviceRefreshTimer_Tick(sender As Object, e As EventArgs) Handles deviceRefreshTimer.Tick
        Try
            Dim devicesChanged As Boolean = LoadAudioDevices()

            ' Only update UI if devices actually changed
            If devicesChanged Then
                ' Store current selection info
                Dim currentGuid As String = ""
                Dim currentName As String = ""
                If current_connector_index >= 0 AndAlso current_connector_index < device_guids.Count Then
                    currentGuid = device_guids(current_connector_index)
                    currentName = connector_names(current_connector_index)
                End If

                ' Build list of currently existing connector names
                Dim currentConnectors As New HashSet(Of String)
                For Each displayName In connector_names
                    Dim connectorName As String = GetConnectorName(displayName)
                    currentConnectors.Add(connectorName)
                Next

                ' Remove settings for connectors that no longer exist
                Dim keysToRemove As New List(Of String)
                For Each key In connector_settings.Keys
                    If Not currentConnectors.Contains(key) Then
                        keysToRemove.Add(key)
                    End If
                Next
                For Each key In keysToRemove
                    connector_settings.Remove(key)
                Next

                ' Initialize settings for new connectors (load from vefx file if exists)
                For Each connectorName In currentConnectors
                    If Not connector_settings.ContainsKey(connectorName) Then
                        connector_settings.Add(connectorName, LoadConnectorSettingsFromFile(connectorName))
                    End If
                Next

                ' Update connector selector
                connector_selector.Items.Clear()
                For Each connectorName In connector_names
                    connector_selector.Items.Add(connectorName)
                Next

                ' Try to restore previous selection by GUID, then by name, or default to first
                Dim newIndex As Integer = -1
                If Not String.IsNullOrEmpty(currentGuid) Then
                    newIndex = device_guids.IndexOf(currentGuid)
                End If

                If newIndex = -1 AndAlso Not String.IsNullOrEmpty(currentName) Then
                    newIndex = connector_names.IndexOf(currentName)
                End If

                If newIndex = -1 AndAlso connector_names.Count > 0 Then
                    newIndex = 0
                End If

                If newIndex >= 0 Then
                    ' Check if we're switching to a different connector
                    Dim connectorChanged As Boolean = (newIndex <> current_connector_index)

                    ' Update index and UI
                    current_connector_index = newIndex

                    ' Temporarily disable event to prevent double-loading
                    RemoveHandler connector_selector.SelectedIndexChanged, AddressOf connector_selector_SelectedIndexChanged
                    connector_selector.SelectedIndex = newIndex
                    AddHandler connector_selector.SelectedIndexChanged, AddressOf connector_selector_SelectedIndexChanged

                    UpdateTitleBar()
                    UpdateAPOStatusIndicator()

                    ' If connector actually changed, load the new connector's settings
                    If connectorChanged Then
                        LoadCurrentConnectorSettings()
                        ' Update VU meter to monitor the new device
                        UpdateVUMeterDevice()
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Function ShowInstallModeDialog() As Integer
        ' Create a simple dialog with radio buttons for install mode selection
        Dim dialog As New Form()
        dialog.Text = "Select APO Installation Mode"
        dialog.Size = New Size(450, 220)
        dialog.FormBorderStyle = FormBorderStyle.FixedDialog
        dialog.StartPosition = FormStartPosition.CenterParent
        dialog.MaximizeBox = False
        dialog.MinimizeBox = False

        Dim label As New Label()
        label.Text = "Select the APO installation mode for this device:"
        label.Location = New Point(10, 10)
        label.Size = New Size(420, 20)
        dialog.Controls.Add(label)

        Dim radio0 As New RadioButton()
        radio0.Text = "LFX/GFX (Legacy, Windows 7)"
        radio0.Location = New Point(20, 40)
        radio0.Size = New Size(400, 24)
        radio0.Tag = 0
        dialog.Controls.Add(radio0)

        Dim radio1 As New RadioButton()
        radio1.Text = "SFX/MFX (Windows 8.1+, recommended for Bluetooth)"
        radio1.Location = New Point(20, 70)
        radio1.Size = New Size(400, 24)
        radio1.Tag = 1
        dialog.Controls.Add(radio1)

        Dim radio2 As New RadioButton()
        radio2.Text = "SFX/EFX (Windows 8.1+, recommended for most devices)"
        radio2.Location = New Point(20, 100)
        radio2.Size = New Size(400, 24)
        radio2.Tag = 2
        radio2.Checked = True ' Default selection
        dialog.Controls.Add(radio2)

        Dim btnOK As New Button()
        btnOK.Text = "OK"
        btnOK.Location = New Point(260, 140)
        btnOK.Size = New Size(80, 30)
        btnOK.DialogResult = DialogResult.OK
        dialog.Controls.Add(btnOK)
        dialog.AcceptButton = btnOK

        Dim btnCancel As New Button()
        btnCancel.Text = "Cancel"
        btnCancel.Location = New Point(350, 140)
        btnCancel.Size = New Size(80, 30)
        btnCancel.DialogResult = DialogResult.Cancel
        dialog.Controls.Add(btnCancel)
        dialog.CancelButton = btnCancel

        ' Show dialog and return selected mode
        If dialog.ShowDialog() = DialogResult.OK Then
            If radio0.Checked Then Return 0
            If radio1.Checked Then Return 1
            If radio2.Checked Then Return 2
        End If

        Return -1 ' Cancelled
    End Function

#End Region

#Region "VU Meter"

    ' COM Interfaces for Audio Meter Information
    <ComImport>
    <Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")>
    Private Class MMDeviceEnumerator
    End Class

    <Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IMMDeviceEnumerator
        Function EnumAudioEndpoints(dataFlow As EDataFlow, dwStateMask As Integer, <Out> ByRef ppDevices As IntPtr) As Integer
        Function GetDefaultAudioEndpoint(dataFlow As EDataFlow, role As ERole, <Out> <MarshalAs(UnmanagedType.Interface)> ByRef ppDevice As IMMDevice) As Integer
    End Interface

    <Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IMMDeviceCollection
        Function GetCount(ByRef pcDevices As UInteger) As Integer
        Function Item(nDevice As UInteger, <Out> <MarshalAs(UnmanagedType.Interface)> ByRef ppDevice As IMMDevice) As Integer
    End Interface

    <Guid("D666063F-1587-4E43-81F1-B948E807363F")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IMMDevice
        Function Activate(ByRef iid As Guid, dwClsCtx As Integer, pActivationParams As IntPtr, <Out> <MarshalAs(UnmanagedType.IUnknown)> ByRef ppInterface As Object) As Integer
        Function OpenPropertyStore(stgmAccess As Integer, <Out> ByRef ppProperties As IntPtr) As Integer
        Function GetId(<Out> ByRef ppstrId As IntPtr) As Integer
        Function GetState(ByRef pdwState As Integer) As Integer
    End Interface

    <Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IAudioMeterInformation
        Function GetPeakValue(ByRef pfPeak As Single) As Integer
        Function GetMeteringChannelCount(ByRef pnChannelCount As UInteger) As Integer
        Function GetChannelsPeakValues(u32ChannelCount As UInteger, <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=0)> afPeakValues As Single()) As Integer
        Function QueryHardwareSupport(ByRef pdwHardwareSupportMask As UInteger) As Integer
    End Interface

    Private Enum EDataFlow
        eRender
        eCapture
        eAll
    End Enum

    Private Enum ERole
        eConsole
        eMultimedia
        eCommunications
    End Enum

    ' VU Meter variables
    Private meterDevice As IMMDevice = Nothing
    Private meterInfo As IAudioMeterInformation = Nothing
    Private WithEvents vuMeterTimer As New Timer()
    Private vuMeterPanel As Panel
    Private vuMeterSquares As New List(Of Panel)()

    Private Sub InitializeVUMeter()
        Try
            ' Get position and size from vumeterpos control
            Dim meterLocation As Point = vumeterpos.Location
            
            ' Remove the placeholder control
            Me.Controls.Remove(vumeterpos)
            vumeterpos.Dispose()

            ' Create VU Meter UI components
            vuMeterPanel = New Panel()
            vuMeterPanel.Location = meterLocation
            vuMeterPanel.Size = New Size(200, 30)
            vuMeterPanel.BackColor = Color.Transparent
            vuMeterPanel.BorderStyle = BorderStyle.None
            Me.Controls.Add(vuMeterPanel)

            ' Create 20 square segments
            Dim numSquares As Integer = 20
            Dim squareSize As Integer = 8
            Dim spacing As Integer = 2
            
            vuMeterSquares.Clear()
            For i As Integer = 0 To numSquares - 1
                Dim square As New Panel()
                square.Location = New Point(i * (squareSize + spacing), 0)
                square.Size = New Size(squareSize, squareSize)
                square.BackColor = Color.DarkGray
                square.BorderStyle = BorderStyle.FixedSingle
                vuMeterPanel.Controls.Add(square)
                vuMeterSquares.Add(square)
            Next

            ' Initialize COM objects for audio meter
            Dim enumerator As Object = New MMDeviceEnumerator()
            Dim iEnumerator As IMMDeviceEnumerator = DirectCast(enumerator, IMMDeviceEnumerator)
            
            ' Get default audio output device
            Dim hr As Integer = iEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, meterDevice)
            If hr <> 0 Then
                Throw New Exception("Failed to get default audio endpoint. HRESULT: 0x" & hr.ToString("X8"))
            End If
            
            ' Activate IAudioMeterInformation
            Dim IID_IAudioMeterInformation As New Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064")
            Dim obj As Object = Nothing
            hr = meterDevice.Activate(IID_IAudioMeterInformation, 0, IntPtr.Zero, obj)
            If hr <> 0 Then
                Throw New Exception("Failed to activate IAudioMeterInformation. HRESULT: 0x" & hr.ToString("X8"))
            End If
            
            meterInfo = DirectCast(obj, IAudioMeterInformation)

            ' Setup timer to update VU meter (30 FPS)
            vuMeterTimer.Interval = 33
            AddHandler vuMeterTimer.Tick, AddressOf VUMeterTimer_Tick
            vuMeterTimer.Start()

        Catch ex As Exception
            MessageBox.Show("Failed to initialize VU Meter: " & ex.Message, "VU Meter Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub VUMeterTimer_Tick(sender As Object, e As EventArgs)
        Try
            If meterInfo IsNot Nothing AndAlso vuMeterSquares.Count > 0 Then
                ' Check if effector is off or APO not installed
                Dim deviceGuid As String = If(current_connector_index >= 0 AndAlso current_connector_index < device_guids.Count, device_guids(current_connector_index), "")
                Dim apoInstalled As Boolean = Not String.IsNullOrEmpty(deviceGuid) AndAlso CheckAPOInstalled(deviceGuid)
                
                If effector_on = 0 OrElse Not apoInstalled Then
                    ' Turn off all squares when effector is off or APO not installed
                    For Each square In vuMeterSquares
                        square.BackColor = Color.DarkGray
                    Next
                    Return
                End If
                
                Dim peakValue As Single = 0
                meterInfo.GetPeakValue(peakValue)
                
                ' Calculate how many squares should be lit
                Dim numSquares As Integer = vuMeterSquares.Count
                Dim litSquares As Integer = CInt(peakValue * numSquares)
                
                ' Update each square
                For i As Integer = 0 To numSquares - 1
                    If i < litSquares Then
                        ' Light up this square with appropriate color
                        Dim ratio As Single = CSng(i) / numSquares
                        If ratio >= 0.85 Then
                            vuMeterSquares(i).BackColor = Color.Red
                        ElseIf ratio >= 0.65 Then
                            vuMeterSquares(i).BackColor = Color.Yellow
                        Else
                            vuMeterSquares(i).BackColor = Color.Lime
                        End If
                    Else
                        ' Turn off this square
                        vuMeterSquares(i).BackColor = Color.DarkGray
                    End If
                Next
            End If
        Catch ex As Exception
            ' Silently ignore errors during meter updates
        End Try
    End Sub

    Private Sub CleanupVUMeter()
        Try
            If vuMeterTimer IsNot Nothing Then
                vuMeterTimer.Stop()
                RemoveHandler vuMeterTimer.Tick, AddressOf VUMeterTimer_Tick
            End If

            If meterInfo IsNot Nothing Then
                Marshal.ReleaseComObject(meterInfo)
                meterInfo = Nothing
            End If

            If meterDevice IsNot Nothing Then
                Marshal.ReleaseComObject(meterDevice)
                meterDevice = Nothing
            End If
        Catch ex As Exception
            ' Ignore cleanup errors
        End Try
    End Sub

    ' Update VU meter to monitor the currently selected device
    Private Sub UpdateVUMeterDevice()
        Try
            ' Release existing COM objects
            If meterInfo IsNot Nothing Then
                Try
                    Marshal.ReleaseComObject(meterInfo)
                Catch
                End Try
                meterInfo = Nothing
            End If

            If meterDevice IsNot Nothing Then
                Try
                    Marshal.ReleaseComObject(meterDevice)
                Catch
                End Try
                meterDevice = Nothing
            End If

            ' Get the currently selected device GUID
            If current_connector_index >= 0 AndAlso current_connector_index < device_guids.Count Then
                Dim deviceGuid As String = device_guids(current_connector_index)
                
                If Not String.IsNullOrEmpty(deviceGuid) Then
                    ' Get device by GUID
                    Dim enumerator As Object = New MMDeviceEnumerator()
                    Dim iEnumerator As IMMDeviceEnumerator = DirectCast(enumerator, IMMDeviceEnumerator)
                    
                    ' Get device collection
                    Dim pDevices As IntPtr = IntPtr.Zero
                    Dim hr As Integer = iEnumerator.EnumAudioEndpoints(EDataFlow.eRender, 1, pDevices) ' 1 = DEVICE_STATE_ACTIVE
                    
                    If hr = 0 AndAlso pDevices <> IntPtr.Zero Then
                        ' Get IMMDeviceCollection interface
                        Dim deviceCollection As IMMDeviceCollection = DirectCast(Marshal.GetObjectForIUnknown(pDevices), IMMDeviceCollection)
                        
                        Dim count As UInteger = 0
                        deviceCollection.GetCount(count)
                        
                        ' Search for device with matching GUID
                        For i As UInteger = 0 To count - 1
                            Dim device As IMMDevice = Nothing
                            deviceCollection.Item(i, device)
                            
                            If device IsNot Nothing Then
                                ' Get device ID
                                Dim pDeviceId As IntPtr = IntPtr.Zero
                                device.GetId(pDeviceId)
                                
                                If pDeviceId <> IntPtr.Zero Then
                                    Dim deviceId As String = Marshal.PtrToStringUni(pDeviceId)
                                    Marshal.FreeCoTaskMem(pDeviceId)
                                    
                                    ' Extract GUID from device ID (format: {0.0.0.00000000}.{GUID})
                                    If deviceId.Contains(deviceGuid) Then
                                        ' Found matching device
                                        meterDevice = device
                                        
                                        ' Activate IAudioMeterInformation
                                        Dim IID_IAudioMeterInformation As New Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064")
                                        Dim obj As Object = Nothing
                                        hr = meterDevice.Activate(IID_IAudioMeterInformation, 0, IntPtr.Zero, obj)
                                        If hr = 0 Then
                                            meterInfo = DirectCast(obj, IAudioMeterInformation)
                                        End If
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                        
                        Marshal.ReleaseComObject(deviceCollection)
                    End If
                    
                    Marshal.ReleaseComObject(iEnumerator)
                End If
            End If
            
            ' If we couldn't get the specific device, fall back to default
            If meterDevice Is Nothing OrElse meterInfo Is Nothing Then
                Dim enumerator As Object = New MMDeviceEnumerator()
                Dim iEnumerator As IMMDeviceEnumerator = DirectCast(enumerator, IMMDeviceEnumerator)
                
                Dim hr As Integer = iEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, meterDevice)
                If hr = 0 Then
                    Dim IID_IAudioMeterInformation As New Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064")
                    Dim obj As Object = Nothing
                    hr = meterDevice.Activate(IID_IAudioMeterInformation, 0, IntPtr.Zero, obj)
                    If hr = 0 Then
                        meterInfo = DirectCast(obj, IAudioMeterInformation)
                    End If
                End If
                
                Marshal.ReleaseComObject(iEnumerator)
            End If
            
        Catch ex As Exception
            ' Silently fail - VU meter will just not work
        End Try
    End Sub

#End Region

End Class