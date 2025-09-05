VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Face/Palm/Fingerprint Device VB Demo V1.1"
   ClientHeight    =   11160
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   20715
   BeginProperty Font 
      Name            =   "Tahoma"
      Size            =   8.25
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   LinkTopic       =   "Form1"
   ScaleHeight     =   11160
   ScaleWidth      =   20715
   StartUpPosition =   2  '屏幕中心
   Begin VB.CheckBox chkUploadFP 
      Caption         =   "Upload FP"
      Height          =   255
      Left            =   360
      TabIndex        =   23
      Top             =   1920
      Width           =   1095
   End
   Begin VB.CheckBox chkUploadPalm 
      Caption         =   "Upload Palm Vein"
      Height          =   255
      Left            =   1560
      TabIndex        =   22
      Top             =   1920
      Width           =   1575
   End
   Begin VB.CommandButton cmdCloseAllAlarm 
      Caption         =   "Close All Alarm"
      Height          =   360
      Left            =   6120
      TabIndex        =   21
      Top             =   1080
      Width           =   2775
   End
   Begin VB.CommandButton cmdReloadRecord 
      Caption         =   "Reload Record"
      Height          =   360
      Left            =   6120
      TabIndex        =   20
      Top             =   2280
      Width           =   2775
   End
   Begin VB.ComboBox cmbLocalIP 
      Height          =   315
      Left            =   1200
      Style           =   2  'Dropdown List
      TabIndex        =   19
      Top             =   540
      Width           =   1695
   End
   Begin VB.TextBox TxtNetPort 
      Height          =   285
      Left            =   10560
      MaxLength       =   5
      TabIndex        =   17
      Text            =   "8101"
      Top             =   120
      Width           =   1695
   End
   Begin VB.TextBox txtConnectPassword 
      Height          =   285
      Left            =   4800
      MaxLength       =   8
      TabIndex        =   15
      Text            =   "FFFFFFFF"
      Top             =   120
      Width           =   1695
   End
   Begin VB.TextBox txtIP 
      Height          =   285
      Left            =   7680
      MaxLength       =   15
      TabIndex        =   13
      Text            =   "192.168.1.124"
      Top             =   120
      Width           =   1695
   End
   Begin VB.TextBox txtSN 
      Height          =   285
      Left            =   1200
      MaxLength       =   16
      TabIndex        =   11
      Text            =   "FC-8255H66244654"
      Top             =   120
      Width           =   1695
   End
   Begin VB.CommandButton CmdBeginWatch 
      Caption         =   "Start Watch Log"
      Height          =   360
      Left            =   3240
      TabIndex        =   9
      Top             =   2280
      Width           =   2775
   End
   Begin VB.CommandButton cmdDownloadRecord 
      Caption         =   "Download Record"
      Height          =   360
      Left            =   360
      TabIndex        =   8
      Top             =   2280
      Width           =   2775
   End
   Begin VB.CommandButton cmdDownloadAllUser 
      Caption         =   "Download User From Device"
      Height          =   360
      Left            =   9000
      TabIndex        =   7
      Top             =   1560
      Width           =   2775
   End
   Begin VB.CommandButton cmdClearLog 
      Caption         =   "Clear Log"
      Height          =   360
      Left            =   12480
      TabIndex        =   6
      Top             =   2280
      Width           =   1575
   End
   Begin VB.CommandButton cmdDeleteUser 
      Caption         =   "Delete User On Device"
      Height          =   360
      Left            =   6120
      TabIndex        =   5
      Top             =   1560
      Width           =   2775
   End
   Begin VB.CommandButton cmdClearUser 
      Caption         =   "Clear User On Device"
      Height          =   360
      Left            =   3240
      TabIndex        =   4
      Top             =   1560
      Width           =   2775
   End
   Begin VB.CommandButton cmdUploadUserToDevice 
      Caption         =   "Upload User To Device"
      Height          =   360
      Left            =   360
      TabIndex        =   3
      Top             =   1560
      Width           =   2775
   End
   Begin VB.TextBox txtLog 
      Height          =   8175
      Left            =   360
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   2
      Text            =   "Form1.frx":0000
      Top             =   2760
      Width           =   20295
   End
   Begin VB.CommandButton cmdSearchDevice 
      Caption         =   "SearchDevice"
      Height          =   360
      Left            =   3240
      TabIndex        =   1
      Top             =   1080
      Width           =   2775
   End
   Begin VB.CommandButton cmdOpenDoor 
      Caption         =   "OpenDoor"
      Height          =   360
      Left            =   360
      TabIndex        =   0
      Top             =   1080
      Width           =   2775
   End
   Begin VB.Label lblLocalIP 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Local IP:"
      Height          =   195
      Left            =   480
      TabIndex        =   18
      Top             =   600
      Width           =   780
   End
   Begin VB.Label LblNetPort 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Net Port:"
      Height          =   195
      Left            =   9840
      TabIndex        =   16
      Top             =   165
      Width           =   660
   End
   Begin VB.Label LblPassword 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Connect Password:"
      Height          =   195
      Left            =   3360
      TabIndex        =   14
      Top             =   165
      Width           =   1395
   End
   Begin VB.Label LblIP 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Device IP:"
      Height          =   195
      Left            =   6840
      TabIndex        =   12
      Top             =   165
      Width           =   780
   End
   Begin VB.Label LblSN 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Device SN:"
      Height          =   195
      Left            =   360
      TabIndex        =   10
      Top             =   165
      Width           =   780
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private WithEvents mIO As DriveMain
Attribute mIO.VB_VarHelpID = -1
Private mSearchNetWorkNum As Long
Private mCommandNames() As String 'Command Name List
Private mRecordEventCodes() As String


'Initialization operation instruction name
Private Sub IniCommandNames()
    ReDim mCommandNames(512)
    mCommandNames(vbOpenDoor) = "Remote Open Door"
    
    mCommandNames(vbSearchEquptOnNetNum) = "Search Device"
    mCommandNames(vbSetEquptNetNum) = "Set Device NetWork Flag"
    mCommandNames(vbWriteUserCard) = "Upload user information to the device"
    mCommandNames(vbUploadFile) = "Upload files to device"
    mCommandNames(vbDeleteAllCard) = "Clear all users"
    mCommandNames(vbDeleteCard) = "Delete user"
    mCommandNames(vbReadCard) = "Read All Users"
    mCommandNames(vbDownloadFile) = "Download files from device"
    mCommandNames(vbReadRecord) = "Read all punch in records"
    mCommandNames(vbStartWatch) = "Enable real-time monitoring of punch in records"
    mCommandNames(vbWriteNetworkServerDetail) = "Set device server IP"
    mCommandNames(vbWriteClientWorkMode) = "Set device network mode"
    mCommandNames(VbRepairRecord) = "Reload records"
    mCommandNames(vbReadEquptVer) = "Gets the device firmware version number"
    
    
End Sub

Private Function GetDeviceConnectDetail() As DriveInfo
    Dim oInfo As DriveInfo
    Set oInfo = New DriveInfo
    
    
    'Check Input Text
    Dim sSN As String
    Dim sConnectPassword As String
    Dim sDeviceIP As String
    Dim sDeviceUDPPort As String
    
    sSN = txtSN.Text
    sConnectPassword = txtConnectPassword.Text
    sDeviceIP = txtIP.Text
    sDeviceUDPPort = TxtNetPort.Text
    
    If Not CheckSN(sSN) Then Exit Function
    If Not CheckPassword(sConnectPassword) Then Exit Function
    If Not CheckIPFormat(sDeviceIP) Then Exit Function
    If Not CheckNetPort(sDeviceUDPPort) Then Exit Function
    
    

    oInfo.EquptType = FCFace
    oInfo.ConnType = OnUDP
    
    oInfo.IP = sDeviceIP ' Device IP
    oInfo.NetPort = CInt(sDeviceUDPPort) ' Device UDP port
    
    oInfo.SN = sSN
    oInfo.Password = sConnectPassword ' Device Connect Password
    
    
    oInfo.TimeOutMSEL = 1000
    oInfo.RestartCount = 3
    
    Set GetDeviceConnectDetail = oInfo
End Function


Private Function CheckSN(ByRef sSN As String) As Boolean
    'Check SN Format
    If Len(sSN) <> 16 Then
        GoTo Err_SNFormat
    End If
    
    
    
    CheckSN = True
    Exit Function
    
Err_SNFormat:
    MsgBox "SN format is error!", 16, "Error"
    CheckSN = False
End Function

Private Function CheckPassword(ByRef sPassword As String) As Boolean
    'Check Connect Password Format
    Dim i As Long, lChar As Long
    CheckPassword = False
    
    If Len(sPassword) = 0 Then
        sPassword = "FFFFFFFF"
        CheckPassword = True
        Exit Function
    End If
    
    If Len(sPassword) > 8 Then
        sPassword = Mid$(sPassword, 1, 8)
    End If
    
    For i = 1 To Len(sPassword)
        lChar = Asc(UCase$(Mid$(sPassword, i, 1)))
        If Not ((lChar >= 48 And lChar <= 57) Or (lChar >= 65 And lChar <= 70)) Then
            MsgBox "Connect password format is error!", 16, "Error"
            Exit For
        End If
    Next
    
    If Len(sPassword) < 8 Then
        sPassword = sPassword & String$(8 - Len(sPassword), "F")
    End If
    CheckPassword = True
End Function

Private Function CheckIPFormat(ByRef sIP As String) As Boolean
    'Check SN Format
    Dim sTmp() As String
    Dim iValue As Long
    Dim i As Long
    
    Dim bIP(3) As Byte
    If Len(sIP) = 0 Then
        GoTo Err_IPFormat 'format error
    End If
    
    sTmp = Split(sIP, ".")
    If UBound(sTmp) <> 3 Then
        GoTo Err_IPFormat 'format error
    End If
    
    On Error GoTo Err_IPFormat
    
    For i = 0 To 3
        If Len(sTmp(i)) > 3 Then
            GoTo Err_IPFormat 'format error
        End If
        
        If Not IsNumeric(sTmp(i)) Then
            GoTo Err_IPFormat 'format error
        End If
        
        iValue = CInt(sTmp(i))
        If iValue < 0 Or iValue > 255 Then
            GoTo Err_IPFormat 'format error
        End If
    Next
    CheckIPFormat = True
    Exit Function
Err_IPFormat:
    CheckIPFormat = False
    MsgBox "IP format is error!", 16, "Error"
End Function

Private Function CheckNetPort(ByRef sNetPort As String) As Boolean
    'Check Net Port Format
    On Error GoTo Err_NetPortFormat
    
    Dim lPort As Long
    If Len(sNetPort) > 5 Then
        GoTo Err_NetPortFormat 'format error
    End If
    
    If Not IsNumeric(sNetPort) Then
        GoTo Err_NetPortFormat 'format error
    End If
    
    lPort = CLng(sNetPort)
    If lPort <= 0 Or lPort >= 65535 Then
        GoTo Err_NetPortFormat 'format error
    End If
    
    CheckNetPort = True
    Exit Function
Err_NetPortFormat:
    CheckNetPort = False
    MsgBox "Net port format is error!", 16, "Error"
End Function

Private Function GetSearchDeviceConnectDetail(ByRef sLocalIP As String, ByRef sLocalPort As Long) As DriveInfo
    Dim oInfo As DriveInfo
    Set oInfo = New DriveInfo

    oInfo.EquptType = FCFace
    oInfo.ConnType = OnUDP
    
    oInfo.IP = "255.255.255.255" ' Device IP
    oInfo.NetPort = 8101 ' Device UDP port
    
    oInfo.SN = "0000000000000000"
    oInfo.Password = "FFFFFFFF"
    
    oInfo.LocalIP = sLocalIP
    oInfo.LocalNetPort = sLocalPort
    
    
    oInfo.TimeOutMSEL = 5000
    oInfo.RestartCount = 3
    
    Set GetSearchDeviceConnectDetail = oInfo
End Function







Private Sub cmdClearLog_Click()
    txtLog.Text = ""
End Sub





'*****************************************    Open Door   **********************************************
Private Sub cmdOpenDoor_Click()
    Dim oCommandDetail As DriveInfo
    Set oCommandDetail = GetDeviceConnectDetail()
    If oCommandDetail Is Nothing Then Exit Sub
    
    mIO.OpenDoor oCommandDetail, Nothing
End Sub
'*******************************************************************************************************






'**********************************Close All Alarm*******************************************************
Private Sub cmdCloseAllAlarm_Click()
    Dim oCommandDetail As DriveInfo
    Set oCommandDetail = GetDeviceConnectDetail()
    If oCommandDetail Is Nothing Then Exit Sub
    
    mIO.CloseAlarm oCommandDetail, 0, 65535
End Sub
'*******************************************************************************************************







Private Sub cmdSearchDevice_Click()
    Dim ips As ClsNet
    Dim i As Integer
    Dim sIPList() As String
    Dim sLocalIP As String
    Dim lLocalPort As Long
    
    Dim oSerachInfo As DriveInfo
    
    lLocalPort = 20000
    
    Set ips = New ClsNet
    If ips.IPAddress(sIPList) Then
        For i = 0 To UBound(sIPList)
            If Len(sIPList(i)) > 0 And sIPList(i) <> "0.0.0.0" Then
                sLocalIP = sIPList(i)
                Set oSerachInfo = GetSearchDeviceConnectDetail(sLocalIP, lLocalPort)
                
                'bind context
                Dim oContext As clsCommandContext
                Set oContext = New clsCommandContext
                oContext.LocalIP = sLocalIP
                oContext.LocalPort = lLocalPort
                Set oSerachInfo.Tag = oContext
                
                AddLog "Begin search device, Local IP:" & sLocalIP, ""
                
                mIO.SearchEquptOnNetNum oSerachInfo, mSearchNetWorkNum
                
                lLocalPort = lLocalPort + 1
            End If
        Next
    End If
    mSearchNetWorkNum = mSearchNetWorkNum + 1
End Sub


Private Sub SearchDeviceCallblack(objConnInfo As FCDrive8800.DriveInfo, ByVal sResult As String)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag
    
    Dim oValueList As DriveValueList
    
    
    
    
    Set oValueList = mIO.AchieveValuetoList(objConnInfo, vbSearchEquptOnNetNum, sResult)
    
    
    Dim sTCPHex As String
    Dim oTcp As DriveTCPInfo
    sTCPHex = oValueList.GetValue_String("TCPPar")
    Set oTcp = New DriveTCPInfo
    oTcp.SetTCPParHexString sTCPHex
    
    
    
    Dim sLog As String
    sLog = "SN:" & oValueList.GetValue_String("SN") & ","
    sLog = sLog & "IP:" & oTcp.IP & ","
    sLog = sLog & "Local:" & oContext.LocalIP
    
    AddLog sLog, "Search Device"
End Sub






















'**************************************************Upload User*************************************

Private Sub cmdUploadUserToDevice_Click()
    'Upload User To Device
    Dim oCommandDetail As DriveInfo
    Set oCommandDetail = GetDeviceConnectDetail()
    If oCommandDetail Is Nothing Then Exit Sub
    
    'Assembly personnel information
    Dim oUsers As DriveCardLists
    Dim oUser As DriveCard
    Dim i As Long
    
    Set oUsers = New DriveCardLists
    

    
    'Add People to List
    For i = 1 To 100
        Set oUser = oUsers.Add
        
        'Assignment
        With oUser
            .SetUserID i + 100000 'User ID

            .SetCardData i + 200000    'Card No.

            .SetPassword "123456"  'Personal password
            .SetTimeLimit CDate("2099-12-30 23:59:59") 'Validity Biggest 2099-12-30 23:59:59，Smallest 2000-12-30 23:59:59
            .SetTimeGroup 1, 1 'Opening Time zone No.  1-64
            
            'Effective times
            .SetReadCount 65535
            
            .Identity = 0 'User identity；0--user；1--admin
            
            'Card Category Normally Open Function Card
            .SetLongOpen 0   'Normal Opening
            'Card Status  0--Normal；1--Reported Lost；2--Blacklist；3--Deleted
            .SetState 0
            'User Name
            .SetName "User" & i
            'Department Name
            .SetDeptName "DemoDept"
            'Holiday
            .SetHolidayPwrStr String(32, "0")

    
            'Access sign of anti-passback 0/3--Entry and exit valid；1--Entry valid；2--Exit valid
            .SetDoorInOutState 1, 0
        End With
    Next
    
    'Create a context for the command and add the personnel list to the context environment
    Dim oContext As clsCommandContext
    Set oContext = New clsCommandContext
    Set oContext.Users = oUsers
    
    
    Set oCommandDetail.Tag = oContext
    
    oContext.CurrentProgress = "WriteUserCard"
    oCommandDetail.Desc = "UploadUser"
    
    AddLog "Start uploading personnel....", ""
    mIO.WriteUserCard oCommandDetail, oUsers
    'Wait for the command to execute and trigger an event after completion mIO_FrameAchieve，iFunc = vbWriteUserCard
    
End Sub

Private Sub UploadUser_WriteUserCardCallBlack(oCommandDetail As FCDrive8800.DriveInfo)
    'The personnel has been uploaded to the device
    Dim oContext As clsCommandContext
    Set oContext = oCommandDetail.Tag 'Get Association Context
    
    'In the second step, the device firmware version number and fingerprint algorithm version number are obtained to prepare for uploading fingerprints
    
    oContext.CurrentProgress = "ReadEquptVer"
    mIO.ReadEquptVer oCommandDetail
End Sub

Private Sub UploadUser_ReadEquptVerCallBlack(oCommandDetail As FCDrive8800.DriveInfo, sResult As String)
    'The firmware version information of the device has been read, and the photo, fingerprint and palm vein of the personnel have been uploaded
    Dim oContext As clsCommandContext
    Set oContext = oCommandDetail.Tag 'Get Association Context
    
    Dim oValueList As DriveValueList
    Set oValueList = mIO.AchieveValuetoList(oCommandDetail, vbReadEquptVer, sResult)
    oContext.FingerprintVer = CLng(oValueList.GetValue_String("Fingerprint"))
    
    'Ready to start uploading people photos
    oContext.CommandOperateIndex = 0
    UploadUserPhoto oCommandDetail
End Sub

Private Sub UploadUserMoveNext(oCommandDetail As FCDrive8800.DriveInfo, oContext As clsCommandContext)
    oContext.CommandOperateIndex = oContext.CommandOperateIndex + 1
    UploadUserPhoto oCommandDetail
End Sub

Private Sub UploadUserPhoto(oCommandDetail As FCDrive8800.DriveInfo)
    'Upload personnel photos here
    Dim oContext As clsCommandContext
    Set oContext = oCommandDetail.Tag 'Get Association Context
    
    oContext.CurrentProgress = "UploadPhoto"
   
    
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
gtLblNextUser:
    If oContext.CommandOperateIndex >= oUsers.Count Then
        AddLog "Personnel upload completed！", ""
        
        Set oContext = Nothing
        Exit Sub
    End If
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    'Check the personnel photo files
    Dim sUserPhoto As String
    Dim sUserName As String
    Dim dUserID As Double
    
    sUserName = oUser.GetName()
    dUserID = oUser.GetUserID()
    
    sUserPhoto = App.Path & "\Images\" & sUserName & ".jpg"
    If Len(Dir(sUserPhoto)) = 0 Then
        'Personnel do not have photos, start uploading the next one
'        oContext.CommandOperateIndex = oContext.CommandOperateIndex + 1
'        GoTo gtLblNextUser

        'Start uploading fingerprints
        UploadUser_BeginUploadFingerprint oCommandDetail, oContext
        Exit Sub
    End If
    
    AddLog "Starting to upload photos, personnel：" & sUserName & ",Photo file：" & sUserPhoto, ""
    
    
    'Calculate CRC32 for photos
    Dim iFileOpenNum As Integer
    Dim iFileLen As Long
    Dim bFileData() As Byte 'Store photo file content
    
    
    Dim iFileType As Integer
    Dim iFileNum As Integer
    
    Dim dFileCRC32 As Double
    
    'Reading photos from files
    iFileOpenNum = FreeFile
    Open sUserPhoto For Binary Access Read As #iFileOpenNum     ' Open File。
    iFileLen = LOF(iFileOpenNum)

    ReDim bFileData(iFileLen - 1)
    
    Get #iFileOpenNum, , bFileData
    Close #iFileOpenNum
    'Calculate CRC32 for photos
    dFileCRC32 = Uint32(CreateCRC32(bFileData(0), iFileLen))
    
    
    iFileType = 1 'Personnel portrait photo
    iFileNum = 1
    
    'Start uploading personnel photos
    oCommandDetail.TimeOutMSEL = 10000
    oCommandDetail.RestartCount = 10
    mIO.CallSubByName "UploadFileEx", 0, oCommandDetail, dUserID, iFileType, iFileNum, bFileData, dFileCRC32, True
    'Waiting for event callback vbUploadFile，and processed by the function UploadPhotoOverCallblack
    
End Sub


'After uploading personnel photos, start uploading the next one
Private Sub UploadPhotoOverCallblack(oCommandDetail As FCDrive8800.DriveInfo)

    
    Dim oContext As clsCommandContext
    Set oContext = oCommandDetail.Tag
    
    
    If oContext.CurrentProgress = "UploadFingerprint" Then
        UploadFingerprintOverCallblack oCommandDetail
        Exit Sub
    End If
    
    If oContext.CurrentProgress = "UploadPalmVein" Then
        UploadPalmVeinOverCallblack oCommandDetail
        Exit Sub
    End If
    
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    'Print log
    
    AddLog "Uploading personnel photos completely, personnel：" & oUser.GetName(), ""
    
    
'    'Advance upload progress

    'Continue uploading
    UploadUser_BeginUploadFingerprint oCommandDetail, oContext
End Sub



Private Sub UploadUser_BeginUploadFingerprint(oCommandDetail As FCDrive8800.DriveInfo, oContext As clsCommandContext)
    oContext.FPIndex = 0
    oContext.FPFileIndex = 0
    
    If chkUploadFP.Value = 1 Then
        UploadUserFingerprints oCommandDetail
    ElseIf chkUploadPalm.Value = 1 Then
        UploadUser_BeginUploadPalmVein oCommandDetail, oContext
    Else
        UploadUserMoveNext oCommandDetail, oContext
    End If
    
End Sub


Private Sub UploadUserFingerprints(oCommandDetail As FCDrive8800.DriveInfo)
    'Upload personnel fingerprint here
    Dim oContext As clsCommandContext
    Set oContext = oCommandDetail.Tag 'Get Association Context
    
    oContext.CurrentProgress = "UploadFingerprint"
    
    
    
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    'Check the personnel fingerprint files
    Dim sUserFingerprintFile As String
    Dim sUserName As String
    Dim dUserID As Double
    
    sUserName = oUser.GetName()
    dUserID = oUser.GetUserID()
    
    Dim iFPIndex As Integer
    iFPIndex = oContext.FPFileIndex
    
Lbl_FindNext:
    Do While True
        sUserFingerprintFile = App.Path & "\UserFingerprint\FP_" & sUserName & "_" & dUserID & "_" & iFPIndex & ".bin"
        iFPIndex = iFPIndex + 1
        If Len(Dir(sUserFingerprintFile)) > 0 Then
            'File exists
            Exit Do
        End If
        sUserFingerprintFile = ""
        If iFPIndex >= 10 Then
            Exit Do
        End If
    Loop
    
    oContext.FPFileIndex = iFPIndex
    
    If Len(sUserFingerprintFile) = 0 Then
        'No fingerprint uploading finished, start uploading palm vein
        UploadUser_BeginUploadPalmVein oCommandDetail, oContext
        Exit Sub
    End If
    
    AddLog "Starting to upload fingerprint, personnel：" & sUserName & ",User ID:" & dUserID & ",FPIndex:" & oContext.FPIndex & ",file：" & sUserFingerprintFile, ""
    
    
    'Calculate CRC32 for fingerprint
    Dim iFileOpenNum As Integer
    Dim iFileLen As Long
    Dim bFileData() As Byte 'Store fingerprint file content
    
    
    Dim iFileType As Integer
    Dim iFileNum As Integer
    
    Dim dFileCRC32 As Double
    
    'Reading fingerprint from files
    iFileOpenNum = FreeFile
    Open sUserFingerprintFile For Binary Access Read As #iFileOpenNum     ' Open File。
    iFileLen = LOF(iFileOpenNum)

    ReDim bFileData(iFileLen - 1)
    
    Get #iFileOpenNum, , bFileData
    Close #iFileOpenNum
    
    
    '检查文件中包含的数据是否为指纹特征码
    Dim bRec As Boolean
    Dim bFPDataBuf() As Byte
    
    bRec = ModFPDataConv.FPAlgorithmConvert(bFileData, bFPDataBuf, oContext.FingerprintVer)
    If Not bRec Then
        '指纹检测和转换失败时，进行下一个
        GoTo Lbl_FindNext
    End If
    iFileLen = UBound(bFPDataBuf) + 1
    Erase bFileData
    
    'Calculate CRC32 for fingerprint
    dFileCRC32 = Uint32(CreateCRC32(bFPDataBuf(0), iFileLen))
    
    
    iFileType = 2 'Personnel fingerprint
    iFileNum = oContext.FPIndex
    
    'Start uploading personnel fingerprint
    oCommandDetail.TimeOutMSEL = 10000
    oCommandDetail.RestartCount = 10
    mIO.CallSubByName "UploadFileEx", 0, oCommandDetail, dUserID, iFileType, iFileNum, bFPDataBuf, dFileCRC32, True
    'Waiting for event callback vbUploadFile，and processed by the function UploadPhotoOverCallblack
    
End Sub

'After uploading personnel fingerprint
Private Sub UploadFingerprintOverCallblack(oCommandDetail As FCDrive8800.DriveInfo)

    
    Dim oContext As clsCommandContext
    Set oContext = oCommandDetail.Tag
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    'Print log
    
    AddLog "Uploading personnel fingerprint completely, User Name：" & oUser.GetName() & ",User ID:" & oUser.GetUserID() & ",FPIndex:" & oContext.FPIndex, ""
    
    
    'Advance upload progress
    oContext.FPIndex = oContext.FPIndex + 1
    
    If oContext.FPIndex >= 3 Then
        'Each person is only allowed to have three fingerprints
        
        'We're going to start uploading the palm vein
        UploadUser_BeginUploadPalmVein oCommandDetail, oContext
    Else
        'Continue uploading
        UploadUserFingerprints oCommandDetail
    End If
    
   
End Sub

Private Sub UploadUser_BeginUploadPalmVein(oCommandDetail As FCDrive8800.DriveInfo, oContext As clsCommandContext)
    oContext.PalmVeinIndex = 0
    oContext.PalmVeinFileIndex = 0
    
    
    If chkUploadPalm.Value = 1 Then
        UploadUserPalmVein oCommandDetail
    Else
        UploadUserMoveNext oCommandDetail, oContext
    End If
End Sub



Private Sub UploadUserPalmVein(oCommandDetail As FCDrive8800.DriveInfo)
    'Upload personnel palm vein here
    Dim oContext As clsCommandContext
    Set oContext = oCommandDetail.Tag 'Get Association Context
    
    oContext.CurrentProgress = "UploadPalmVein"

    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    'Check the personnel palm vein files
    Dim sUserPalmVeinFile As String
    Dim sUserName As String
    Dim dUserID As Double
    
    sUserName = oUser.GetName()
    dUserID = oUser.GetUserID()
    
    Dim iPalmVeinIndex As Integer
    iPalmVeinIndex = oContext.PalmVeinFileIndex
    
    Do While True
        sUserPalmVeinFile = App.Path & "\UserPalmVein\PalmVein_" & sUserName & "_" & dUserID & "_" & iPalmVeinIndex + 1 & ".bin"
        iPalmVeinIndex = iPalmVeinIndex + 1
        If Len(Dir(sUserPalmVeinFile)) > 0 Then
            'File exists
            Exit Do
        End If
        sUserPalmVeinFile = ""
        If iPalmVeinIndex >= 2 Then 'Each person has only two palmar veins, one for the left hand and one for the right hand
            Exit Do
        End If
    Loop
    
    oContext.PalmVeinFileIndex = iPalmVeinIndex
    
    If Len(sUserPalmVeinFile) = 0 Then
        'No palm vein uploading finished, start uploading next user
        UploadUserMoveNext oCommandDetail, oContext
        Exit Sub
    End If
    
    AddLog "Starting to upload palm vein, personnel：" & sUserName & ",User ID:" & dUserID & ",Palm Vein Index:" & oContext.PalmVeinIndex & ",file：" & sUserPalmVeinFile, ""
    
    
    'Calculate CRC32 for palm vein
    Dim iFileOpenNum As Integer
    Dim iFileLen As Long
    Dim bFileData() As Byte 'Store palm vein file content
    
    
    Dim iFileType As Integer
    Dim iFileNum As Integer
    
    Dim dFileCRC32 As Double
    
    'Reading palm vein from files
    iFileOpenNum = FreeFile
    Open sUserPalmVeinFile For Binary Access Read As #iFileOpenNum     ' Open File。
    iFileLen = LOF(iFileOpenNum)

    ReDim bFileData(iFileLen - 1)
    
    Get #iFileOpenNum, , bFileData
    Close #iFileOpenNum
    'Calculate CRC32 for palm vein
    dFileCRC32 = Uint32(CreateCRC32(bFileData(0), iFileLen))
    
    
    iFileType = 5 'Personnel palm vein
    iFileNum = oContext.PalmVeinIndex + 1 '1 or 2; 1--right hand;2--left hand
    
    'Start uploading personnel palm vein
    oCommandDetail.TimeOutMSEL = 10000
    oCommandDetail.RestartCount = 10
    mIO.CallSubByName "UploadFileEx", 0, oCommandDetail, dUserID, iFileType, iFileNum, bFileData, dFileCRC32, True
    'Waiting for event callback vbUploadFile，and processed by the function UploadPhotoOverCallblack
    
End Sub

'After uploading personnel palm vein
Private Sub UploadPalmVeinOverCallblack(oCommandDetail As FCDrive8800.DriveInfo)

    
    Dim oContext As clsCommandContext
    Set oContext = oCommandDetail.Tag
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    'Print log
    
    AddLog "Uploading personnel palm vein completely, User Name：" & oUser.GetName() & ",User ID:" & oUser.GetUserID() & ",Palm Vein Index:" & oContext.PalmVeinIndex + 1, ""
    
    
    'Advance upload progress
    oContext.PalmVeinIndex = oContext.PalmVeinIndex + 1
    
    If oContext.PalmVeinIndex >= 2 Then
        'Each person can upload two palm vein feature codes, one for the left hand and the other for the right hand
        
        'After uploading, start uploading the next person
        UploadUserMoveNext oCommandDetail, oContext
    Else
        'Continue uploading
        UploadUserPalmVein oCommandDetail
    End If
    
   
End Sub

'***************************************************************************************************************











'**************Empty All Users****************
Private Sub cmdClearUser_Click()
    Dim oCommandDetail As DriveInfo
    Set oCommandDetail = GetDeviceConnectDetail()
    If oCommandDetail Is Nothing Then Exit Sub
    
    oCommandDetail.TimeOutMSEL = 60000
    
    Call mIO.CallSubByName("ClearCard", 0, oCommandDetail)
    'Wait for the command to execute and trigger an event after completion mIO_FrameAchieve，iFunc = vbDeleteAllCard
    
End Sub

'******************************************









'**************Remove personnel from the device****************
Private Sub cmdDeleteUser_Click()
    Dim oCommandDetail As DriveInfo
    Set oCommandDetail = GetDeviceConnectDetail()
    If oCommandDetail Is Nothing Then Exit Sub
    
    
    'Assembly of personnel information to be deleted
    Dim oUsers As DriveCardLists
    Dim oUser As DriveCard
    Dim i As Long
    
    Set oUsers = New DriveCardLists
    
    'Add personnel to be deleted to the list
    For i = 1 To 10
        Set oUser = oUsers.Add
        
        oUser.SetUserID i + 100000 'Personnel to be deletedID
    Next
    

    
    AddLog "Starting to delete personnel....", ""
    mIO.DeleteUserCard oCommandDetail, oUsers
    'Wait for the command to execute and trigger an event after completion mIO_FrameAchieve，iFunc = vbDeleteCard
    
End Sub
'******************************************









'**************Download all user information and photos from the device****************
Private Sub cmdDownloadAllUser_Click()
    Dim oCommandDetail As DriveInfo
    Set oCommandDetail = GetDeviceConnectDetail()
    If oCommandDetail Is Nothing Then Exit Sub
    
    oCommandDetail.TimeOutMSEL = 10000
   
    AddLog "Start downloading all personnel information from the device....", ""
    Call mIO.CallSubByName("GetCards", 0, oCommandDetail)
    'Wait for the command to execute and trigger an event after completion mIO_ReadCardAchieve
End Sub

Private Sub mIO_ReadCardAchieve(objConnInfo As FCDrive8800.DriveInfo, objCol As FCDrive8800.DriveCardLists)
    'Download all user information from the device completed
    
    'Print personnel information to
    Dim iUserCount As Long
    iUserCount = objCol.Count
    AddLog "Download personnel from the device completed, number of personnel downloaded：" & iUserCount, ""
    
    If iUserCount = 0 Then Exit Sub
    
    
    
    'Create a context object and prepare to read personnel photos
    Dim oContext As clsCommandContext
    Dim sBuf As cStringBulider
    Dim oUser As DriveCard
    Dim i As Long
    
    Set sBuf = New cStringBulider
    
    
    Set oContext = New clsCommandContext
    Set oContext.Users = objCol
    Set objConnInfo.Tag = oContext
    
    For i = 1 To iUserCount
        Set oUser = objCol.Item(i)
        
        Call sBuf.Append("User Name:").Append(oUser.GetName()).Append(" ")
        Call sBuf.Append("User ID:").AppendEx(oUser.GetUserID()).Append(" ")
        Call sBuf.Append("User PIN:").AppendEx(oUser.GetPassword()).Append(" ")
        Call sBuf.Append("Card:").AppendEx(oUser.GetCardData()).Append(" ")
        Call sBuf.Append("Photo:").AppendEx(oUser.FaceCode).Append(" ")
        Call sBuf.Append("Fingerprint:").AppendEx(oUser.FingerprintCode).Append(" ")
        Call sBuf.Append("Palm Vein:").AppendEx(oUser.PalmCode).Append(vbNewLine)
    Next
    
    
    'Write user information to a file
    Dim sUserLogFile As String
    Dim iFileOpenNum As Integer

    sUserLogFile = App.Path & "\Users.txt"
    
    
    If Len(Dir(sUserLogFile)) > 0 Then
        Kill sUserLogFile
    End If
    

    
    'Writing Files
    iFileOpenNum = FreeFile
    Open sUserLogFile For Binary Access Write As #iFileOpenNum     ' Open File。

    Put #iFileOpenNum, , sBuf.ToString()
    Close #iFileOpenNum
    
    AddLog "The person downloaded from the device has been stored in a file：" & sUserLogFile, ""
    
    'Start reading personnel photos
    DownloadUserPhoto objConnInfo
End Sub




Private Sub DownloadUserPhoto(objConnInfo As FCDrive8800.DriveInfo)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag 'Get Association Context
    
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
gtLblNextUser:
    If oUsers.Count = oContext.CommandOperateIndex Then
        AddLog "Download all users from the device, command execution completed！", ""
        
        Set oContext = Nothing
        Exit Sub
    End If
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    
    'Check whether the examiner registered photos, fingerprints, palmar veins
    If oUser.FaceCode = 0 And oUser.PalmCode = 0 And oUser.FingerprintCode = 0 Then
        'No biometrics registered, go to the next person
        oContext.CommandOperateIndex = oContext.CommandOperateIndex + 1
        GoTo gtLblNextUser
    End If
    
    
    'Check whether personnel photos include photos
    If oUser.FaceCode = 0 Then
        'Personnel have no photos, start downloading fingerprints
        
        If oUser.FingerprintCode > 0 Then
            oContext.FPIndex = 0
            DownloadUserFingerprints objConnInfo
        ElseIf oUser.PalmCode > 0 Then
            oContext.PalmVeinIndex = 0
            DownloadUserPalmVein objConnInfo
        End If
        
        Exit Sub
    End If
    
    
    Dim iFileType As Integer
    Dim iFileNum As Integer
    
    iFileType = 1 'User Photo
    iFileNum = 1
    
    Dim dUserID As Double
    dUserID = oUser.GetUserID
    oContext.CurrentProgress = "DownloadPhoto"
    
    AddLog "Start downloading user photos  User Name：" & oUser.UserName & "，User ID：" & oUser.UserID, ""
    mIO.CallSubByName "DownloadFileByFace", 0, objConnInfo, iFileType, iFileNum, dUserID
    'Wait for the command to execute and trigger an event after completion mIO_FrameAchieve，iFunc = vbDownloadFile
End Sub


Private Sub DownloadUserPhotoCallblack(objConnInfo As FCDrive8800.DriveInfo, sCommandResult As String)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag 'Get Association Context
    
    If oContext.CurrentProgress = "DownloadFingerprint" Then
        DownloadUserFingerprintsCallblack objConnInfo, sCommandResult
        Exit Sub
    End If
    
    If oContext.CurrentProgress = "DownloadPalmVein" Then
        DownloadUserPalmVeinCallblack objConnInfo, sCommandResult
        Exit Sub
    End If
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    

    Dim sLog As String
    If Len(sCommandResult) < 20 Then
        Select Case sCommandResult
            Case "ERR-CRC"
                sLog = "Photo download failed, CRC32 verification error"
            Case "ERR-NULL"
                sLog = "Photo download failed, user does not have photos"
        End Select
    Else
        Dim b() As Byte
        b = mIO.HexToByte(sCommandResult)
        Dim iFileOpenNum As Integer
        Dim iFileLen As Long
        Dim sFile As String
        Dim sPath As String
        
        iFileOpenNum = FreeFile
        
        sPath = App.Path & "\DownloadPhoto"
        If Len(Dir(sPath, vbDirectory)) = 0 Then
            MkDir sPath 'Directory does not exist, create directory
        End If
        
        
        sFile = sPath & "\DownloadPhoto_" & oUser.GetName() & "_" & oUser.GetUserID() & ".jpg"
        If Len(Dir(sFile)) > 0 Then
            Kill sFile
        End If
        

        Open sFile For Binary Access Write As #iFileOpenNum     ' Open File。
        Put #iFileOpenNum, , b
        Close #iFileOpenNum
        
        sLog = "Photo download completed, save path：" & sFile

    End If
    
    
    sLog = "User photo download completed, username：" & oUser.GetName() & ",User ID:" & oUser.GetUserID() & "   " & sLog
    
    AddLog sLog, ""
    
    If oUser.FingerprintCode > 0 Then
        oContext.FPIndex = 0
        DownloadUserFingerprints objConnInfo
    ElseIf oUser.PalmCode > 0 Then
        oContext.PalmVeinIndex = 0
        DownloadUserPalmVein objConnInfo
    Else
        'Continue downloading next person photo
        oContext.CommandOperateIndex = oContext.CommandOperateIndex + 1
        DownloadUserPhoto objConnInfo
    End If
End Sub



Private Sub DownloadUserFingerprints(objConnInfo As FCDrive8800.DriveInfo)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag 'Get Association Context
    
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    

    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    'Check whether the person contains fingerprints
    If oUser.FingerprintCode = 0 Then
        'Personnel no prints, start downloading the palm vein signature
        oContext.PalmVeinIndex = 0
        DownloadUserPalmVein objConnInfo
        Exit Sub
    End If
    
    
    'Check whether the fingerprint has been downloaded
    If oUser.FingerprintCode = oContext.FPIndex Then
        'Fingerprints downloaded. Start downloading the palm vein
        oContext.PalmVeinIndex = 0
        DownloadUserPalmVein objConnInfo

        Exit Sub
    End If
    
    
    Dim iFileType As Integer
    Dim iFileNum As Integer
    
    iFileType = 2 'User Fingerprints
    iFileNum = oContext.FPIndex
    
    Dim dUserID As Double
    dUserID = oUser.GetUserID
    
    oContext.CurrentProgress = "DownloadFingerprint"
    AddLog "Start downloading user fingerprint  User Name：" & oUser.UserName & "，User ID：" & oUser.UserID & " FPIndex:" & oContext.FPIndex, ""
    mIO.CallSubByName "DownloadFileByFace", 0, objConnInfo, iFileType, iFileNum, dUserID
    'Wait for the command to execute and trigger an event after completion mIO_FrameAchieve，iFunc = vbDownloadFile
End Sub


Private Sub DownloadUserFingerprintsCallblack(objConnInfo As FCDrive8800.DriveInfo, sCommandResult As String)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag 'Get Association Context
    
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    

    Dim sLog As String
    If Len(sCommandResult) < 20 Then
        Select Case sCommandResult
            Case "ERR-CRC"
                sLog = "Fingerprint download failed, CRC32 verification error"
            Case "ERR-NULL"
                sLog = "Fingerprint download failed, user does not have fingerprint"
        End Select
    Else
        Dim b() As Byte
        b = mIO.HexToByte(sCommandResult)
        Dim iFileOpenNum As Integer
        Dim iFileLen As Long
        Dim sFile As String
        Dim sPath As String
        
        iFileOpenNum = FreeFile
        
        sPath = App.Path & "\UserFingerprint"
        If Len(Dir(sPath, vbDirectory)) = 0 Then
            MkDir sPath 'Directory does not exist, create directory
        End If
        
        
        sFile = sPath & "\FP_" & oUser.GetName() & "_" & oUser.GetUserID() & "_" & oContext.FPIndex & ".bin"
        If Len(Dir(sFile)) > 0 Then
            Kill sFile
        End If
        

        Open sFile For Binary Access Write As #iFileOpenNum     ' Open File。
        Put #iFileOpenNum, , b
        Close #iFileOpenNum
        
        sLog = "Fingerprint download completed, save path：" & sFile

    End If
    
    
    sLog = "User photo download completed, username：" & oUser.GetName() & ",User ID:" & oUser.GetUserID() & ",FPIndex:" & oContext.FPIndex & "   " & sLog
    
    AddLog sLog, ""
    
    'Continue downloading next person  next fingerprint
    oContext.FPIndex = oContext.FPIndex + 1 'Download the next fingerprint
    DownloadUserFingerprints objConnInfo
End Sub




Private Sub DownloadUserPalmVein(objConnInfo As FCDrive8800.DriveInfo)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag 'Get Association Context
    
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    

    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)
    
    'The examiner registered the palmar vein
    If oUser.PalmCode = 0 Then
        'Personnel did not register palmar vein, start downloading the next personnel
        oContext.CommandOperateIndex = oContext.CommandOperateIndex + 1
        DownloadUserPhoto objConnInfo
        Exit Sub
    End If
    
    
    'Check whether the fingerprint has been downloaded
    If oUser.PalmCode = oContext.PalmVeinIndex Then
        'Palm vein downloaded.start downloading the next personnel
        oContext.CommandOperateIndex = oContext.CommandOperateIndex + 1
        DownloadUserPhoto objConnInfo

        Exit Sub
    End If
    
    
    Dim iFileType As Integer
    Dim iFileNum As Integer
    
    iFileType = 6 'User palm vein
    iFileNum = oContext.PalmVeinIndex + 1 ' 1 or 2
    
    Dim dUserID As Double
    dUserID = oUser.GetUserID
    
    oContext.CurrentProgress = "DownloadPalmVein"
    AddLog "Start downloading user palm vein,  User Name：" & oUser.UserName & "，User ID：" & oUser.UserID & " Palm vein Index:" & oContext.PalmVeinIndex + 1, ""
    mIO.CallSubByName "DownloadFileByFace", 0, objConnInfo, iFileType, iFileNum, dUserID
    'Wait for the command to execute and trigger an event after completion mIO_FrameAchieve，iFunc = vbDownloadFile
End Sub


Private Sub DownloadUserPalmVeinCallblack(objConnInfo As FCDrive8800.DriveInfo, sCommandResult As String)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag 'Get Association Context
    
    
    Dim oUsers As DriveCardLists
    Set oUsers = oContext.Users 'Obtain the list of associated personnel
    
    Dim oUser As DriveCard
    Set oUser = oUsers.Item(oContext.CommandOperateIndex + 1)

    Dim sLog As String
    If Len(sCommandResult) < 20 Then
        Select Case sCommandResult
            Case "ERR-CRC"
                sLog = "Photo download failed, CRC32 verification error"
            Case "ERR-NULL"
                sLog = "Photo download failed, user does not have photos"
        End Select
    Else
        Dim b() As Byte
        b = mIO.HexToByte(sCommandResult)
        Dim iFileOpenNum As Integer
        Dim iFileLen As Long
        Dim sFile As String
        Dim sPath As String
        
        iFileOpenNum = FreeFile
        
        sPath = App.Path & "\UserPalmVein"
        If Len(Dir(sPath, vbDirectory)) = 0 Then
            MkDir sPath 'Directory does not exist, create directory
        End If
        
        
        sFile = sPath & "\PalmVein_" & oUser.GetName() & "_" & oUser.GetUserID() & "_" & (oContext.PalmVeinIndex + 1) & ".bin"
        If Len(Dir(sFile)) > 0 Then
            Kill sFile
        End If
        

        Open sFile For Binary Access Write As #iFileOpenNum     ' Open File。
        Put #iFileOpenNum, , b
        Close #iFileOpenNum
        
        sLog = "Photo download completed, save path：" & sFile

    End If
    
    
    sLog = "User photo download completed, username：" & oUser.GetName() & ",User ID:" & oUser.GetUserID() & "   " & sLog
    
    AddLog sLog, ""
    
    'Continue downloading next person next palm vein
    oContext.PalmVeinIndex = oContext.PalmVeinIndex + 1 ' Let's download the next palm vein
    DownloadUserPalmVein objConnInfo
End Sub




'*******************************************************************************************************





















'******************Download punch in records and on-site photos from the device************************
Private Sub cmdDownloadRecord_Click()
    Dim oCommandDetail As DriveInfo
    Set oCommandDetail = GetDeviceConnectDetail()
    If oCommandDetail Is Nothing Then Exit Sub
    
    oCommandDetail.TimeOutMSEL = 10000
    
   
    AddLog "Start downloading all punch in records from the device....", ""
    mIO.ReadRecord oCommandDetail, e_RecordFileCode.vbCardRecord, 0
    'Wait for the command to execute and trigger an event after completion mIO_ReadRecordAchieve
End Sub



Private Sub mIO_ReadRecordAchieve(objConnInfo As FCDrive8800.DriveInfo, ByVal lRecordCount As String, objRecords As FCDrive8800.DriveRecordLists)
    'Download all punch in records from the device completed
    
    'Print personnel information to
    Dim iRecordCount As Long
    iRecordCount = objRecords.Count
    AddLog "Downloading punch in records from the device completed, number of downloaded records：" & iRecordCount, ""
    
    If iRecordCount = 0 Then Exit Sub
    
    
    
    'Create a context object and prepare to read personnel photos
    Dim oContext As clsCommandContext
    Dim sBuf As cStringBulider
    Dim oRecord As DriveRecord
    Dim i As Long
    
    Set sBuf = New cStringBulider
    
    
    Set oContext = New clsCommandContext
    Set oContext.Records = objRecords
    Set objConnInfo.Tag = oContext
    
    For i = 1 To iRecordCount
        Set oRecord = objRecords.Item(i)
        
        Call sBuf.Append("Serial Number:").Append(oRecord.RecordNum).Append(" ")
        Call sBuf.Append("User ID:").AppendEx(oRecord.GetUserID()).Append(" ")
        Call sBuf.Append("Time:").Append(Format(oRecord.GetDateTime(), "yyyy-MM-dd HH:mm:ss")).Append(" ")
        Call sBuf.Append("Event:").AppendEx(oRecord.EventCode()).Append(" ")
        Call sBuf.Append("Event Title:").Append(GetRecordEventCodeTitle(oRecord.EventCode())).Append(" ")
        Call sBuf.Append("IsEntry:").AppendEx(IIf(oRecord.EventPort = 1, "Entry", "Exit")).Append(" ")
        Call sBuf.Append("Photo:").AppendEx(oRecord.Photo).Append(vbNewLine)
    Next
    
    
    'Write punch in record information to a file
    Dim sRecordLogFile As String
    Dim iFileOpenNum As Integer

    sRecordLogFile = App.Path & "\Records.txt"
    
    
    If Len(Dir(sRecordLogFile)) > 0 Then
        Kill sRecordLogFile
    End If
    

    
    'Write to file
    iFileOpenNum = FreeFile
    Open sRecordLogFile For Binary Access Write As #iFileOpenNum     ' Open File。

    Put #iFileOpenNum, , sBuf.ToString()
    Close #iFileOpenNum
    
    AddLog "All punch in records downloaded from the device have been completed and stored in a file：" & sRecordLogFile, ""
    
    'Start reading punch-in records and on-site photos
    DownloadRecordPhoto objConnInfo
End Sub


Private Sub DownloadRecordPhoto(objConnInfo As FCDrive8800.DriveInfo)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag 'Get Association Context
    
    
    Dim oRecords As DriveRecordLists
    Set oRecords = oContext.Records 'Obtain a list of associated clock in records
    
gtLblNextRecord:
    If oRecords.Count = oContext.CommandOperateIndex Then
        AddLog "Download all punch in records from the device, command execution completed！", ""
        
        Set oContext = Nothing
        Exit Sub
    End If
    
    Dim oRecord As DriveRecord
    Set oRecord = oRecords.Item(oContext.CommandOperateIndex + 1)
    
    'Check whether the record contains photos
    If oRecord.Photo = False Then
        'There are no photos in the punch in record, start downloading the next one
        oContext.CommandOperateIndex = oContext.CommandOperateIndex + 1
        GoTo gtLblNextRecord
        Exit Sub
    End If
    
    
    Dim iFileType As Byte
    Dim iFileNum As Byte
    
    iFileType = 3
    iFileNum = 1
    
    Dim dRecordID As Double
    dRecordID = oRecord.RecordNum
    
    
    AddLog "Start downloading punch-in records and on-site photos  record number：" & oRecord.RecordNum & "，User ID：" & oRecord.GetUserID, ""
    objConnInfo.Desc = "Record"
    
    
    mIO.CallSubByName "DownloadFileByFace", 0, objConnInfo, iFileType, iFileNum, dRecordID
    'Wait for the command to execute and trigger an event after completion mIO_FrameAchieve，iFunc = vbDownloadFile
End Sub


Private Sub DownloadRecordPhotoCallblack(objConnInfo As FCDrive8800.DriveInfo, sCommandResult As String)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag 'Get Association Context
    
    
    Dim oRecords As DriveRecordLists
    Set oRecords = oContext.Records 'Obtain a list of associated punch in records
    
    Dim oRecord As DriveRecord
    Set oRecord = oRecords.Item(oContext.CommandOperateIndex + 1)
    
    oContext.CommandOperateIndex = oContext.CommandOperateIndex + 1
    
    
    
    Dim sLog As String
    If Len(sCommandResult) < 20 Then
        Select Case sCommandResult
            Case "ERR-CRC"
                sLog = "Photo download failed, CRC32 verification error"
            Case "ERR-NULL"
                sLog = "Photo download failed, there are no photos in the record"
        End Select
    Else
        Dim b() As Byte
        b = mIO.HexToByte(sCommandResult)
        Dim iFileOpenNum As Integer
        Dim iFileLen As Long
        Dim sFile As String
        Dim sPath As String
        
        iFileOpenNum = FreeFile
        
        sPath = App.Path & "\DownloadRecordPhoto"
        If Len(Dir(sPath, vbDirectory)) = 0 Then
            MkDir sPath 'Directory does not exist, create directory
        End If
        
        
        sFile = sPath & "\RecordPhoto_R" & oRecord.RecordNum & "_U" & oRecord.GetUserID() & ".jpg"
        If Len(Dir(sFile)) > 0 Then
            Kill sFile
        End If
        

        Open sFile For Binary Access Write As #iFileOpenNum     ' Open File。
        Put #iFileOpenNum, , b
        Close #iFileOpenNum
        
        sLog = "Photo download completed, save path：" & sFile

    End If
    
    
    sLog = "Punch in record on-site photo download completed，record number：" & oRecord.RecordNum & "，User ID：" & oRecord.GetUserID & "   " & sLog
    
    AddLog sLog, ""
    
    'Continue downloading next person photo
    DownloadRecordPhoto objConnInfo
End Sub
'*******************************************************************************************************













'******************Download punch in records and on-site photos from the device************************


Private Sub cmdReloadRecord_Click()
    Dim oCommandDetail As DriveInfo
    Set oCommandDetail = GetDeviceConnectDetail()
    If oCommandDetail Is Nothing Then Exit Sub
    
    oCommandDetail.TimeOutMSEL = 10000
    
   
    AddLog "Start reload record from the device....", ""
    'Step 1: First fix the records and set all records as new records
    mIO.RepairRecord oCommandDetail, e_RecordFileCode.vbCardRecord
    'Wait for the command to execute and trigger an event after completion mIO_FrameAchieve，iFunc = VbRepairRecord
End Sub


'*******************************************************************************************************




















'*******************Start real-time monitoring of punch in records***********************

Private Sub CmdBeginWatch_Click()
    Dim oCommandDetail As DriveInfo
    Set oCommandDetail = GetDeviceConnectDetail()
    If oCommandDetail Is Nothing Then Exit Sub
    
    Dim sLocal As String
    sLocal = cmbLocalIP.Text
    
    
    oCommandDetail.LocalIP = sLocal
    oCommandDetail.LocalNetPort = 8888
    
    Call mIO.ChunnelForciblyState(oCommandDetail, True) 'When enabling monitoring, it is necessary to bind the local IP and port number
    
    
    mIO.BeginWatch oCommandDetail
    'Wait for the command to execute and trigger an event after completion mIO_FrameAchieve，iFunc = vbStartWatch
    'After monitoring is turned on, real-time recording will trigger events mIO_WatchEvents
    
    
    'Write the server IP information of the device
    mIO.CallSubByName "WriteNetworkServerDetail", 0, oCommandDetail, oCommandDetail.LocalNetPort, oCommandDetail.LocalIP, ""

    
    
    'Configure the device server network mode to UDP
    Dim iClientModel As Long
    iClientModel = 1 '0--disable;1--UDP;2--TCP Client;
    mIO.CallSubByName "WriteClientWorkMode", 0, oCommandDetail, iClientModel
    'AddCommandCallBack vbWriteClientWorkMode, "WriteOverCallBack"
End Sub

Private Sub mIO_WatchEvents(objConnInfo As FCDrive8800.DriveInfo, ByVal iWatchType As Integer, ByVal sValue As String)
    'Process punch in records here
    
    If iWatchType = &H22 Then
        AddLog objConnInfo.SN & "--Connection liveness--" & objConnInfo.IP & ":" & objConnInfo.NetPort, "Watch"
        Exit Sub
    End If
    
    If iWatchType = &HA0 Then
        AddLog objConnInfo.SN & "--Connection handshake--" & objConnInfo.IP & ":" & objConnInfo.NetPort, "Watch"
        'Send handshake response
        SendServerEcho objConnInfo
        Exit Sub
    End If
    
    Dim oRecord As DriveRecord
    Set oRecord = New DriveRecord
    oRecord.SetCode iWatchType
    oRecord.SetRecordFCFaceStr iWatchType, sValue
    
    
    Dim sBuf As cStringBulider
    Set sBuf = New cStringBulider
    
    Call sBuf.Append("Receive Time:").Append(Format(Now(), "yyyy-MM-dd HH:mm:ss")).Append(" ")
    Call sBuf.Append("Serial Number:").Append(oRecord.RecordNum).Append(" ")
    Call sBuf.Append("User ID:").AppendEx(oRecord.GetUserID()).Append(" ")
    Call sBuf.Append("Time:").Append(Format(oRecord.GetDateTime(), "yyyy-MM-dd HH:mm:ss")).Append(" ")
    Call sBuf.Append("Event:").AppendEx(oRecord.EventCode()).Append(" ")
    Call sBuf.Append("Event Title:").Append(GetRecordEventCodeTitle(oRecord.EventCode())).Append(" ")
    
    Call sBuf.Append("IsEntry:").AppendEx(IIf(oRecord.EventPort = 1, "Entry", "Exit")).Append(" ")
    Call sBuf.Append("Photo:").AppendEx(oRecord.Photo).Append(vbNewLine)
    
    AddLog sBuf.ToString(), "Watch"
    
    'Save to file

    Dim sRecordLogFile As String
    Dim iFileOpenNum As Integer

    sRecordLogFile = App.Path & "\WatchLogs.txt"
    
    'Write to file
    iFileOpenNum = FreeFile
    Dim iFileLen As Long
    
    Open sRecordLogFile For Binary Access Write As #iFileOpenNum     ' 打开文件。
    iFileLen = LOF(iFileOpenNum) + 1
    
    
    Put #iFileOpenNum, iFileLen, sBuf.ToString()
    Close #iFileOpenNum
    
    
    'Check whether the record contains photos
    If oRecord.Photo Then
        
        Dim oCommandDetail As DriveInfo
        Set oCommandDetail = GetDeviceConnectDetail()
        
        Dim oContext As clsCommandContext
        Set oContext = New clsCommandContext 'Create
        Set oContext.Record = oRecord
        
        Set oCommandDetail.Tag = oContext 'Context Bound
        
        'Start downloading recorded photos
        Dim iFileType As Byte
        Dim iFileNum As Byte
        
        iFileType = 3
        iFileNum = 1
        
        Dim dRecordID As Double
        dRecordID = oRecord.RecordNum
        
        
        AddLog "Start downloading live photos of real-time monitoring records  record number：" & oRecord.RecordNum & "，User ID：" & oRecord.GetUserID, ""
        oCommandDetail.Desc = "Watch"
        
        
        mIO.CallSubByName "DownloadFileByFace", 0, oCommandDetail, iFileType, iFileNum, dRecordID
    End If
    
End Sub


Private Sub SendServerEcho(objConnInfo As DriveInfo)
    Dim sIP As String
    sIP = "192.168.1.42" 'Local IP
    
    objConnInfo.EquptType = FCFace
    Call mIO.CallSubByName("SendServerEcho", 0, objConnInfo, sIP)

End Sub




Private Sub DownloadWatchRecordPhotoCallblack(objConnInfo As FCDrive8800.DriveInfo, sCommandResult As String)
    Dim oContext As clsCommandContext
    Set oContext = objConnInfo.Tag 'Get Association Context
    
    
    Dim oRecord As DriveRecord
    Set oRecord = oContext.Record 'Obtain associated punch in records

    
    Dim sLog As String
    If Len(sCommandResult) < 20 Then
        Select Case sCommandResult
            Case "ERR-CRC"
                sLog = "Photo download failed, CRC32 verification error"
            Case "ERR-NULL"
                sLog = "Photo download failed, there are no photos in the record"
        End Select
    Else
        Dim b() As Byte
        b = mIO.HexToByte(sCommandResult)
        Dim iFileOpenNum As Integer
        Dim iFileLen As Long
        Dim sFile As String
        Dim sPath As String
        
        iFileOpenNum = FreeFile
        
        sPath = App.Path & "\DownloadWatchRecordPhoto"
        If Len(Dir(sPath, vbDirectory)) = 0 Then
            MkDir sPath 'Directory does not exist, create directory
        End If
        
        
        sFile = sPath & "\RecordPhoto_R" & oRecord.RecordNum & "_U" & oRecord.GetUserID() & ".jpg"
        If Len(Dir(sFile)) > 0 Then
            Kill sFile
        End If
        

        Open sFile For Binary Access Write As #iFileOpenNum     ' Open File。
        Put #iFileOpenNum, , b
        Close #iFileOpenNum
        
        sLog = "Photo download completed, save path：" & sFile

    End If
    
    
    sLog = "Punch in record monitoring, on-site photo download completed, record serial number：" & oRecord.RecordNum & "，User ID：" & oRecord.GetUserID & "   " & sLog
    
    AddLog sLog, "Watch"
    
End Sub
'*******************************************************************************************************
















Private Sub Form_Load()
    Set mIO = New DriveMain
    IniCommandNames
    LoadRecordEventCodes
    mSearchNetWorkNum = 1
    LoadLocalIP
End Sub

Private Sub Form_Unload(Cancel As Integer)
    mIO.StopConnAll
    mIO.Unload
    Set mIO = Nothing
End Sub

Private Sub mIO_CommandTimeout(objConnInfo As FCDrive8800.DriveInfo, ByVal iFunc As Integer, ByVal iStep As Integer)
    AddLog "Command timeout", mCommandNames(iFunc), objConnInfo
End Sub

Private Sub mIO_FrameAchieve(objConnInfo As FCDrive8800.DriveInfo, ByVal iFunc As Integer, ByVal sValue As String)
    AddLog "Command ok", mCommandNames(iFunc), objConnInfo
    If iFunc = vbSearchEquptOnNetNum Then
        'Search Device Result
        Call SearchDeviceCallblack(objConnInfo, sValue)
        Exit Sub
    End If

    If iFunc = vbWriteUserCard Then
        AddLog "Uploading personnel list finished, it's ready to upload personnel photos！", ""
        Call UploadUser_WriteUserCardCallBlack(objConnInfo)
        Exit Sub
        
    End If
    
    If iFunc = vbUploadFile Then
        Call UploadPhotoOverCallblack(objConnInfo)
        Exit Sub
    End If
    
    If iFunc = vbReadEquptVer Then
        If objConnInfo.Desc = "UploadUser" Then
            UploadUser_ReadEquptVerCallBlack objConnInfo, sValue
        End If
    End If
    
    If iFunc = vbDownloadFile Then
        If objConnInfo.Desc = "Record" Then
            Call DownloadRecordPhotoCallblack(objConnInfo, sValue) 'Download on-site photos of punch-in records
        ElseIf objConnInfo.Desc = "Watch" Then
            Call DownloadWatchRecordPhotoCallblack(objConnInfo, sValue) '下载打卡记录监控的现场照片
        Else
            Call DownloadUserPhotoCallblack(objConnInfo, sValue) 'Download user profile photos
        End If
        Exit Sub
    End If
    
    If iFunc = VbRepairRecord Then
        'Step 2: Re-read the record
        Call cmdDownloadRecord_Click
    End If
End Sub


Private Sub mIO_PasswordErr(objConnInfo As FCDrive8800.DriveInfo, ByVal iFunc As Integer)
    
    AddLog "Connect password error", mCommandNames(iFunc), objConnInfo
End Sub




Private Sub mIO_SendProcess(objConnInfo As FCDrive8800.DriveInfo, ByVal iFunc As Integer, ByVal iStep As Long, ByVal iStepCount As Long)
    AddLog "Send command", mCommandNames(iFunc), objConnInfo
End Sub



Private Sub AddLog(sLog As String, sCmdName As String, Optional oConnInfo As DriveInfo)
    Dim sPrintLog As String
    
    sPrintLog = Format(Now(), "HH:mm:ss.fff") & "--"
    
    If Len(sCmdName) > 0 Then
        sPrintLog = sPrintLog & sCmdName & "--" & sLog
    Else
        sPrintLog = sPrintLog & sLog
    End If
    
    If Not oConnInfo Is Nothing Then
        sPrintLog = sPrintLog & "--SN:" & oConnInfo.SN & ",Remote IP:" & oConnInfo.IP & ",Local IP:" & oConnInfo.LocalIP
    End If
    
    
    
    Dim sLogFile As String
    Dim iFileOpenNum As Integer

    sLogFile = App.Path & "\Logs.txt"
    
    'Write to file
    iFileOpenNum = FreeFile
    Dim iFileLen As Long
    
    Open sLogFile For Binary Access Write As #iFileOpenNum     ' 打开文件。
    iFileLen = LOF(iFileOpenNum) + 1
    Put #iFileOpenNum, iFileLen, sPrintLog & vbNewLine
    Close #iFileOpenNum
    
    
    
    txtLog.Text = sPrintLog & vbNewLine & txtLog.Text
    
    If Len(txtLog.Text) > 20000 Then
        txtLog.Text = Mid$(txtLog.Text, 1, 10000)
    End If
End Sub


Private Sub LoadRecordEventCodes()
    ReDim mRecordEventCodes(1000)
    Dim sRecordTxt As String
    Dim sLines() As String
    
    Dim sFile As String
    sFile = App.Path & "\RecordEventCodes.txt"
    If Len(Dir(sFile)) = 0 Then
        Exit Sub
    End If
    
    Dim iFileNum As Long
    Dim iFileLen As Long
    Dim bFileData() As Byte
    
    iFileNum = FreeFile
    Open sFile For Binary Access Read As #iFileNum
    iFileLen = LOF(iFileNum)

    ReDim bFileData(iFileLen - 1)
    
    Get #iFileNum, , bFileData
    Close #iFileNum
    
    
    sRecordTxt = StrConv(bFileData, vbUnicode)
    sLines = Split(sRecordTxt, vbNewLine)
    
    Dim iLineCount As Long
    Dim sLine As String
    Dim sCols() As String
    Dim iEventCode As Long
    Dim sEventCodeTitle As String
    Dim i As Long
    
    iLineCount = UBound(sLines)
    For i = 0 To iLineCount
        sLine = sLines(i)
        If Len(sLine) > 0 Then
            If InStr(sLine, "-") > 0 Then
                sCols = Split(sLine, "--")
                If UBound(sCols) >= 1 Then
                    iEventCode = CInt(Trim(sCols(0)))
                    sEventCodeTitle = Trim(sCols(1))
                    
                    If iEventCode < 1000 Then
                        mRecordEventCodes(iEventCode) = sEventCodeTitle
                    End If
                End If
            End If
            
        End If
        
        
    Next
End Sub


Private Function GetRecordEventCodeTitle(ByVal iEventCode As Long) As String
    If iEventCode < 1000 Then
        If Len(mRecordEventCodes(iEventCode)) = 0 Then
            GetRecordEventCodeTitle = "-"
        Else
            GetRecordEventCodeTitle = mRecordEventCodes(iEventCode)
        End If
        
        
    Else
        GetRecordEventCodeTitle = "-"
    End If
End Function



Private Function LoadLocalIP()
    Dim ips As ClsNet
    Dim i As Integer
    Dim sIPList() As String
    Dim sLocalIP As String

    cmbLocalIP.Clear
    
    Set ips = New ClsNet
    If ips.IPAddress(sIPList) Then
        For i = 0 To UBound(sIPList)
            If Len(sIPList(i)) > 0 And sIPList(i) <> "0.0.0.0" Then
                sLocalIP = sIPList(i)
                cmbLocalIP.AddItem sLocalIP
            End If
        Next
    End If
    
    If cmbLocalIP.ListCount > 0 Then
        cmbLocalIP.ListIndex = 0
    End If
End Function

