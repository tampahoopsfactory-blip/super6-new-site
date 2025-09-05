VERSION 5.00
Begin VB.Form FrmMain 
   Caption         =   "指纹仪测试软件"
   ClientHeight    =   10965
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   14670
   LinkTopic       =   "Form1"
   ScaleHeight     =   10965
   ScaleWidth      =   14670
   StartUpPosition =   2  '屏幕中心
   Begin VB.CommandButton cmdIdentify 
      Caption         =   "特征码比对"
      Height          =   495
      Left            =   7800
      TabIndex        =   41
      Top             =   9120
      Width           =   1455
   End
   Begin VB.CommandButton Command1 
      Caption         =   "比对缓冲区指纹"
      Height          =   495
      Left            =   11520
      TabIndex        =   40
      Top             =   8400
      Width           =   1935
   End
   Begin VB.TextBox txtUserID 
      Height          =   375
      Left            =   8400
      MaxLength       =   5
      TabIndex        =   38
      Text            =   "1"
      Top             =   8400
      Width           =   975
   End
   Begin VB.CommandButton cmdAddTptArray 
      Caption         =   "添加模板到缓冲区"
      Height          =   495
      Left            =   9480
      TabIndex        =   37
      Top             =   8400
      Width           =   1935
   End
   Begin VB.Frame famFingerprintEnroll 
      Caption         =   "指纹登记"
      Height          =   6015
      Left            =   360
      TabIndex        =   27
      Top             =   4800
      Width           =   6855
      Begin VB.ComboBox cmbFPVer 
         Height          =   300
         Left            =   3720
         Style           =   2  'Dropdown List
         TabIndex        =   36
         Top             =   3277
         Width           =   1695
      End
      Begin VB.CommandButton cmdFPConv 
         Caption         =   "指纹转换"
         Height          =   375
         Left            =   1440
         TabIndex        =   34
         Top             =   3240
         Width           =   1215
      End
      Begin VB.TextBox txtCheckFP 
         Height          =   2175
         Left            =   120
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   33
         Top             =   3720
         Width           =   6615
      End
      Begin VB.CommandButton cmdFPCheck 
         Caption         =   "指纹检测"
         Height          =   375
         Left            =   120
         TabIndex        =   32
         Top             =   3240
         Width           =   1215
      End
      Begin VB.CommandButton cmdStopEnroll 
         Caption         =   "停止"
         Height          =   375
         Left            =   4680
         TabIndex        =   31
         Top             =   240
         Width           =   975
      End
      Begin VB.Timer TmrEnroll 
         Interval        =   100
         Left            =   2640
         Top             =   240
      End
      Begin VB.CommandButton cmdRegEnroll 
         Caption         =   "开始登记"
         Height          =   375
         Left            =   5760
         TabIndex        =   30
         Top             =   240
         Width           =   975
      End
      Begin VB.TextBox txtFingerprintTemplate 
         Height          =   2415
         Left            =   120
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   29
         Top             =   720
         Width           =   6615
      End
      Begin VB.Label lblFPVer 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "目标版本："
         Height          =   180
         Left            =   2760
         TabIndex        =   35
         Top             =   3337
         Width           =   900
      End
      Begin VB.Label Label5 
         AutoSize        =   -1  'True
         Caption         =   "已生成的指纹特征码(Base64)"
         Height          =   180
         Left            =   120
         TabIndex        =   28
         Top             =   337
         Width           =   2340
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "读卡"
      Height          =   975
      Left            =   7560
      TabIndex        =   22
      Top             =   7080
      Width           =   6855
      Begin VB.CommandButton cmdStopReadCard 
         Caption         =   "停止"
         Height          =   375
         Left            =   4920
         TabIndex        =   26
         Top             =   360
         Width           =   855
      End
      Begin VB.Timer tmrReadCard 
         Enabled         =   0   'False
         Left            =   4320
         Top             =   240
      End
      Begin VB.TextBox txtCardNum 
         Height          =   375
         Left            =   1320
         Locked          =   -1  'True
         TabIndex        =   25
         Top             =   360
         Width           =   2895
      End
      Begin VB.CommandButton cmdReadCard 
         Caption         =   "读卡"
         Height          =   375
         Left            =   5880
         TabIndex        =   23
         Top             =   360
         Width           =   855
      End
      Begin VB.Label Label4 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "卡号："
         Height          =   180
         Left            =   720
         TabIndex        =   24
         Top             =   457
         Width           =   540
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "指纹仪声音控制"
      Height          =   975
      Left            =   7560
      TabIndex        =   16
      Top             =   6000
      Width           =   6855
      Begin VB.ComboBox cmbLEDUse 
         Height          =   300
         Left            =   4560
         Style           =   2  'Dropdown List
         TabIndex        =   20
         Top             =   480
         Width           =   1215
      End
      Begin VB.ComboBox cmdLedKind 
         Height          =   300
         Left            =   1320
         Style           =   2  'Dropdown List
         TabIndex        =   18
         Top             =   480
         Width           =   1815
      End
      Begin VB.CommandButton cmdLEDSet 
         Caption         =   "设置"
         Height          =   375
         Left            =   5880
         TabIndex        =   17
         Top             =   443
         Width           =   855
      End
      Begin VB.Label Label3 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "开关："
         Height          =   180
         Left            =   3960
         TabIndex        =   21
         Top             =   540
         Width           =   540
      End
      Begin VB.Label Label2 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "颜色："
         Height          =   180
         Left            =   720
         TabIndex        =   19
         Top             =   540
         Width           =   540
      End
   End
   Begin VB.Frame famSoundPlay 
      Caption         =   "指纹仪声音控制"
      Height          =   975
      Left            =   7560
      TabIndex        =   12
      Top             =   4920
      Width           =   6855
      Begin VB.CommandButton cmdSoundPlay 
         Caption         =   "播放"
         Height          =   375
         Left            =   5880
         TabIndex        =   15
         Top             =   443
         Width           =   855
      End
      Begin VB.ComboBox cmbSoundPlay 
         Height          =   300
         ItemData        =   "FrmMain.frx":0000
         Left            =   1320
         List            =   "FrmMain.frx":0002
         Style           =   2  'Dropdown List
         TabIndex        =   14
         Top             =   480
         Width           =   4455
      End
      Begin VB.Label LblSoundPlay 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "音频内容："
         Height          =   180
         Left            =   360
         TabIndex        =   13
         Top             =   540
         Width           =   900
      End
   End
   Begin VB.TextBox txtFPImageDetail 
      Height          =   375
      Left            =   360
      Locked          =   -1  'True
      TabIndex        =   11
      Top             =   4320
      Width           =   6855
   End
   Begin VB.CommandButton cmdReadCapture 
      Caption         =   "获取一个指纹图像"
      Height          =   350
      Left            =   360
      TabIndex        =   10
      Top             =   3840
      Width           =   2175
   End
   Begin VB.PictureBox Picture1 
      AutoRedraw      =   -1  'True
      Height          =   3840
      Left            =   8760
      ScaleHeight     =   3780
      ScaleWidth      =   3780
      TabIndex        =   9
      Top             =   360
      Width           =   3840
   End
   Begin VB.TextBox txtDeviceDetail 
      Height          =   495
      Left            =   360
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      TabIndex        =   8
      Top             =   3120
      Width           =   6855
   End
   Begin VB.CommandButton cmdGetDeviceDetail 
      Caption         =   "获取指纹仪相关信息"
      Height          =   350
      Left            =   360
      TabIndex        =   7
      Top             =   2520
      Width           =   2175
   End
   Begin VB.CommandButton cmdCloseDevice 
      Caption         =   "关闭指纹仪连接"
      Height          =   350
      Left            =   4320
      TabIndex        =   6
      Top             =   1800
      Width           =   1815
   End
   Begin VB.CommandButton cmdOpenDevice 
      Caption         =   "打开指纹仪连接"
      Height          =   350
      Left            =   2280
      TabIndex        =   5
      Top             =   1800
      Width           =   1815
   End
   Begin VB.TextBox txtDevName 
      Height          =   375
      Left            =   2040
      Locked          =   -1  'True
      TabIndex        =   4
      Top             =   263
      Width           =   3975
   End
   Begin VB.CommandButton cmdDestroyContext 
      Caption         =   "关闭操作上下文"
      Height          =   350
      Left            =   4320
      TabIndex        =   2
      Top             =   1320
      Width           =   1815
   End
   Begin VB.CommandButton cmdCreateContext 
      Caption         =   "创建操作上下文"
      Height          =   350
      Left            =   2280
      TabIndex        =   1
      Top             =   1320
      Width           =   1815
   End
   Begin VB.CommandButton cmdEnumerate 
      Caption         =   "列出所有设备"
      Height          =   350
      Left            =   480
      TabIndex        =   0
      Top             =   1320
      Width           =   1695
   End
   Begin VB.Label Label6 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "模板ID："
      Height          =   180
      Left            =   7680
      TabIndex        =   39
      Top             =   8490
      Width           =   720
   End
   Begin VB.Line Line1 
      X1              =   360
      X2              =   6240
      Y1              =   2280
      Y2              =   2280
   End
   Begin VB.Label Label1 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "指纹仪设备名："
      Height          =   180
      Left            =   600
      TabIndex        =   3
      Top             =   360
      Width           =   1260
   End
End
Attribute VB_Name = "FrmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (dest As Any, src As Any, ByVal cb As Long)
Private Declare Sub ZeroMemory Lib "kernel32" Alias "RtlZeroMemory" (dest As Any, ByVal numBytes As Long)

Private mContextID As Long
Private mDevID As String
Private mIsOpen As Boolean
Private mImgWidth As Long, mImgHeight As Long, mImgDPI As Long, mFeatureSize As Long, mTemplateSize As Long
Private mImageBuf() As Byte
Private mStopReadCard As Boolean '停止读卡

'指纹登记全局变量
'是否已开启登记，已登记次数，临时模板缓冲区
Private mBeginEnroll As Boolean, mEnrollFeatureCount As Integer
Private mEnrollFeature1() As Byte, mEnrollFeature2() As Byte, mEnrollFeature3() As Byte, mEnrollErrCount As Integer
Private mEnrollLeave As Boolean
'是否已完成登记，正式模板缓冲区
Private mIsEnrollOver As Boolean, mTemplate() As Byte


'将一个指纹仪灰度图像显示到窗口上
Private Declare Function DrawGrayBitmap Lib "RC4" (ByVal hWnd As Long, ByRef FpImageBuffer As Byte, ByVal aWidth As Long, ByVal aHeight As Long) As Long








Private Sub Form_Load()
    mContextID = -1
    mDevID = vbNullString
    
    cmbSoundPlay.Clear
    Dim sArrs() As String
    Dim i As Long
    sArrs = Split("请按指纹,请重新按指纹,请再次按指纹,登记成功,谢谢,该指纹已有注册,请按上一个指纹,请读卡,Press Fp,Try Again,Try Again 2, Enroll OK,OK,Enrolled,Old Fp,Scan Card", ",")
    For i = 0 To UBound(sArrs)
        cmbSoundPlay.AddItem sArrs(i)
    Next
    cmbSoundPlay.ListIndex = 0
    
    
    sArrs = Split("LED-绿色,LED-红色,指纹头背光灯", ",")
    cmdLedKind.Clear
    For i = 0 To UBound(sArrs)
        cmdLedKind.AddItem sArrs(i)
    Next
    cmdLedKind.ListIndex = 0
    
    
    sArrs = Split("打开,关闭", ",")
    cmbLEDUse.Clear
    For i = 0 To UBound(sArrs)
        cmbLEDUse.AddItem sArrs(i)
    Next
    cmbLEDUse.ListIndex = 0
    
    
    sArrs = Split("Ver 70,Ver 80,Ver 85,Ver 89", ",")
    cmbFPVer.Clear
    For i = 0 To UBound(sArrs)
        cmbFPVer.AddItem sArrs(i)
    Next
    cmbFPVer.ListIndex = 0
    
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Release
End Sub

Private Sub Release()
    '释放所有资源
    If mIsOpen Then cmdCloseDevice_Click '关闭打开的指纹仪
    If mContextID <> -1 Then Call DestroyContext  '关闭指纹仪操作上下文
    
    If mImgWidth > 0 Then
        Erase mImageBuf
    End If
    
    mDevID = vbNullString
    txtDevName.Text = vbNullString
    txtFPImageDetail.Text = vbNullString
End Sub

'枚举所有已连接的指纹仪
Private Sub cmdEnumerate_Click()
    Dim i As Integer
    Dim bStrDevID(63) As Byte
    Dim bStrDevName(254) As Byte
    Dim sDevID As String, sDevName As String
    Dim vRet As Long
    txtDevName.Text = vbNullString
    mDevID = vbNullString
    Dim bFind As Boolean
    
    Release
    
    
    For i = 0 To 5
        ZeroMemory bStrDevID(0), 64
        ZeroMemory bStrDevName(0), 255
        vRet = pisEnumerateDevice(i, bStrDevID(0), bStrDevName(0))
        If vRet = FPResultOK Then
            If (bStrDevID(0) > 0) Then
                sDevID = StrConv(bStrDevID, vbUnicode)
            End If
            If (bStrDevName(0) > 0) Then
                sDevName = StrConv(bStrDevName, vbUnicode)
            End If
            sDevID = VBA.Replace$(sDevID, Chr(0), vbNullString)
            sDevName = VBA.Replace$(sDevName, Chr(0), vbNullString)
            txtDevName.Text = sDevName
            mDevID = sDevID
            '自动创建上下文
            Call CreateContext
            '自动打开指纹仪
            Call cmdOpenDevice_Click
            bFind = True
            Exit For
        End If
    Next
    
    If Not bFind Then
        MsgBox "未查询到指纹仪！", 16, "错误"
    Else
        MsgBox "查询并已连接到指纹仪"
    End If
    
End Sub

Private Sub cmdCreateContext_Click()
    Dim bRet As Boolean
    bRet = CreateContext
End Sub

Private Sub cmdDestroyContext_Click()
    Release
End Sub

'创建指纹机操作上下文
Private Function CreateContext() As Boolean
    Dim vRet As Long
    If mContextID = -1 Then
        If Len(mDevID) = 0 Then
            'MsgBox "请先查询指纹仪连接状态！", 16, "错误"
            Call cmdEnumerate_Click
            Exit Function
        End If
        vRet = pisCreateContext(mContextID)
        If vRet = FPResultOK Then CreateContext = True
    Else
        CreateContext = True
    End If
End Function

'删除指纹仪操作上下文
Private Sub DestroyContext()
    Dim vRet As Long
    If mContextID = -1 Then Exit Sub
    vRet = pisDestroyContext(mContextID)
    mContextID = -1
End Sub



Private Sub cmdOpenDevice_Click()
    If Len(mDevID) = 0 Then Exit Sub
    If mContextID = -1 Then Exit Sub
    Dim vRet As Long
    vRet = pisOpenDevice(mContextID, mDevID)
    If vRet = FPResultOK Then
        mIsOpen = True
        
        '自动获取指纹仪工作参数
        Call cmdGetDeviceDetail_Click
        
    End If
End Sub
    
Private Sub cmdCloseDevice_Click()
    If Not mIsOpen Then Exit Sub
    Dim vRet As Long
    vRet = pisCloseDevice(mContextID)
    mIsOpen = False
    
    txtDeviceDetail.Text = vbNullString
End Sub

'检查是否已打开指纹仪的连接
Private Function CheckOpenDevice() As Boolean
    CheckOpenDevice = False
    If Len(mDevID) = 0 Then
        MsgBox "请先查询指纹仪！", 64, "错误"
        Exit Function
    End If
    If mContextID = -1 Then
        MsgBox "请先创建一个操作上下文！", 64, "错误"
        Exit Function
    End If
    If Not mIsOpen Then
        MsgBox "请先建立一个和指纹仪的连接！", 64, "错误"
        Exit Function
    End If
    CheckOpenDevice = True
End Function

'获取指纹仪的工作参数详情
Private Sub cmdGetDeviceDetail_Click()
    txtDeviceDetail.Text = vbNullString
    If Not CheckOpenDevice() Then Exit Sub
    Dim strEngineInfo As String
    Dim imaWidth As Long, imaHeight As Long, imaRes As Long, featureSize As Long, templateSize As Long
    Dim vRet As Long
    strEngineInfo = Space(300)
    mImgWidth = 0
    mImgHeight = 0
    featureSize = 0
    templateSize = 0
    
    vRet = pisGetInfo(mContextID, strEngineInfo, imaWidth, imaHeight, imaRes, featureSize, templateSize)
    If vRet = FPResultOK Then
        strEngineInfo = Mid$(strEngineInfo, 1, InStr(strEngineInfo, Chr(0)) - 1)
        strEngineInfo = "算法引擎：" & strEngineInfo & "，图像尺寸：" & imaWidth & " * " & imaHeight & "，图像扫描精度：" & imaRes & "DPI" & vbNewLine
        strEngineInfo = strEngineInfo & "临时模板尺寸：" & featureSize & "， 正式模板尺寸；" & templateSize
        txtDeviceDetail.Text = strEngineInfo
        
        mImgWidth = imaWidth
        mImgHeight = imaHeight
        mImgDPI = imaRes
        mFeatureSize = featureSize
        mTemplateSize = templateSize
        ReDim mImageBuf(mImgWidth * mImgHeight)
        
    Else
        MsgBox "获取指纹仪参数失败！", 16, "错误"
    End If
End Sub



Private Sub cmdReadCapture_Click()
    If Not CheckOpenDevice() Then Exit Sub
    Dim vRet As Long
    Dim iCheck As Long, iFPArea As Long
    Dim sTip As String
    vRet = pisCapture(mContextID, mImageBuf(0))
    txtFPImageDetail.Text = vbNullString
    If vRet = FPResultOK Then
        sTip = "获取图像成功"
        DrawFPImage mImageBuf
        vRet = pisCheckFp(mContextID, mImageBuf(0), mImgWidth, mImgHeight, mImgDPI, iCheck, iFPArea)
        If vRet = FPResultOK Then
            If iCheck = 1 Then
                sTip = sTip & "，指纹占比：" & iFPArea
                WriteBMP mImageBuf, UBound(mImageBuf), "指纹仪图像base64.txt"
                
               
                
                'txtCheckFP.Text = ModBase64.EncodeBase64Byte(mImageBuf)
            Else
                sTip = sTip & "，但是图像中不包含指纹或不清晰！"
            End If
        Else
            sTip = sTip & "，图像检测失败！"
        End If
        
        
    Else
        sTip = "获取图像失败"
    End If
    
    txtFPImageDetail.Text = sTip
End Sub

Private Sub DrawFPImage(ByRef bBuf() As Byte)
    Dim bCopy() As Byte
    ReDim bCopy(UBound(bBuf))
    CopyMemory bCopy(0), bBuf(0), UBound(bBuf) + 1
    DrawGrayBitmap Picture1.hWnd, bCopy(0), mImgWidth, mImgHeight
    Erase bCopy
End Sub




Private Sub WriteBMP(ByRef bImgBuf() As Byte, ByVal iLen As Long, ByVal sFileName As String)
    Dim sPath As String
    sPath = App.Path & "\" & sFileName
    If Len(Dir(sPath)) > 0 Then Kill sPath
    
    Dim iFileNum As Integer
    iFileNum = VBA.FreeFile()
    Open sPath For Binary Access Write As iFileNum
    Put #iFileNum, 1, ModBase64.EncodeBase64Byte(bImgBuf)
    Close iFileNum
    

End Sub


'开始登记指纹
Private Sub cmdRegEnroll_Click()
    If Not CheckOpenDevice() Then Exit Sub
    If mFeatureSize = 0 Then
        MsgBox "没有获取指纹仪工作参数！", 16, "错误"
        Exit Sub
    End If
    cmdRegEnroll.Enabled = False
    TmrEnroll.Interval = 200
    cmdStopEnroll.Visible = True
    TmrEnroll.Enabled = True
    mEnrollErrCount = 0
    mBeginEnroll = True
    mIsEnrollOver = False
    mEnrollFeatureCount = 0
    
    ReDim mEnrollFeature1(mFeatureSize - 1)  '开辟临时模板缓冲区
    ReDim mEnrollFeature2(mFeatureSize - 1)
    ReDim mEnrollFeature3(mFeatureSize - 1)
    
    
    ReDim mTemplate(mTemplateSize - 1) '开辟正式模板缓冲区
    mEnrollLeave = True '指纹已离开
    Call pisSoundPlay(mContextID, 0) '播放 -- 请按指纹
    txtFingerprintTemplate.Text = "请按指纹 0/3"
End Sub

Private Sub TmrEnroll_Timer()
    If mBeginEnroll = False Then
        StopEnroll
        Exit Sub
    End If
    
    Dim vRet As Long
    Dim iCheck As Long, iFPArea As Long
    Dim sTip As String
    vRet = pisCapture(mContextID, mImageBuf(0))

    If vRet = FPResultOK Then
        vRet = pisCheckFp(mContextID, mImageBuf(0), mImgWidth, mImgHeight, mImgDPI, iCheck, iFPArea)
        If vRet = FPResultOK Then
            If iCheck = 1 Then
                If mEnrollLeave Then
                    mEnrollLeave = False
                    If iFPArea < 40 Then
                        If mEnrollErrCount > 10 Then
                            Call pisSoundPlay(mContextID, 1) '播放 -- 请重新按指纹
                            mEnrollErrCount = 0
                        Else
                            mEnrollErrCount = mEnrollErrCount + 1
                        End If
                    Else
                        DrawFPImage mImageBuf
                        
                        '指纹清晰，生成临时模板
                        mEnrollErrCount = 0
                        Dim bTmp() As Byte
                        ReDim bTmp(mFeatureSize - 1)
                        vRet = pisProcess(mContextID, mImageBuf(0), mImgWidth, mImgHeight, mImgDPI, bTmp(0))
                        If vRet = FPResultOK Then
                            
                            Select Case mEnrollFeatureCount
                                Case 0
                                    CopyMemory mEnrollFeature1(0), bTmp(0), mFeatureSize
                                Case 1
                                    CopyMemory mEnrollFeature2(0), bTmp(0), mFeatureSize
                                Case 2
                                    CopyMemory mEnrollFeature3(0), bTmp(0), mFeatureSize
                            End Select
                            
                            
                            mEnrollFeatureCount = mEnrollFeatureCount + 1
                            If mEnrollFeatureCount = 3 Then
                                '登记完成，生成正式模板
                                vRet = pisCreateTemplate(mContextID, mEnrollFeature1(0), mEnrollFeature2(0), mEnrollFeature3(0), mTemplate(0))
                                If vRet = FPResultOK Then
                                    mBeginEnroll = False
                                    Call pisSoundPlay(mContextID, 3) '播放 -- 指纹登记成功
                                    txtFingerprintTemplate.Text = ModBase64.EncodeBase64Byte(mTemplate)
                                    WriteToFile "RegFPData.bin", mTemplate
                                Else
                                    '全部重来
                                    mEnrollErrCount = 0
                                    
                                    mEnrollFeatureCount = 0
                                    Call pisSoundPlay(mContextID, 1) '播放 -- 请重新按指纹
                                    txtFingerprintTemplate.Text = "请按指纹 0/3"
                                End If
                            Else
                                Call pisSoundPlay(mContextID, 2) '播放 -- 请再次按指纹
                                txtFingerprintTemplate.Text = "请按指纹 " & mEnrollFeatureCount & "/3"
                            End If
                        End If
                        
                    End If
                End If
            Else
                mEnrollLeave = True
            End If
        End If
    End If
End Sub


Private Sub WriteToFile(ByVal sFileName As String, bBuf() As Byte)
    Dim sFile As String
    sFile = App.Path & "\" & sFileName
    If Len(Dir(sFile)) > 0 Then
        Kill sFile
    End If
      

    Dim iFileOpenNum As Integer
    
    'Write to file
    iFileOpenNum = FreeFile
    Dim iFileLen As Long
    
    Open sFile For Binary Access Write As #iFileOpenNum     ' 打开文件。
    Put #iFileOpenNum, 1, bBuf
    Close #iFileOpenNum
End Sub


Private Sub cmdStopEnroll_Click()
    mBeginEnroll = False
    cmdStopEnroll.Visible = False
End Sub


Private Sub StopEnroll()
    TmrEnroll.Enabled = False
    mBeginEnroll = False
    cmdStopEnroll.Visible = False
    cmdRegEnroll.Enabled = True
    
    mEnrollFeatureCount = 0
    Erase mEnrollFeature1, mEnrollFeature2, mEnrollFeature3, mTemplate '释放缓冲区
End Sub

Private Sub cmdFPCheck_Click()
    '检查指纹模板
    Dim sFP As String
    sFP = txtFingerprintTemplate.Text
    If Len(sFP) = 0 Then
        Exit Sub
    End If
    
    
    Dim sVerBuf As String
    Dim iRec As Long
    
    Call FPCONV_Init
    
    sVerBuf = Space(200)
    
    iRec = FPCONV_GetConvDLLModel(sVerBuf)
    
    If iRec = FPResultOK Then
        Call GetCString(sVerBuf)
        txtCheckFP.Text = "检测动态库版本号：" & sVerBuf & vbNewLine
    Else
        MsgBox "检测失败，动态库调用失败！", 16, "错误"
        Exit Sub
    End If
    
    Dim bBuf() As Byte
    bBuf = ModBase64.DecodeBase64Byte(sFP)
    iRec = FPCONV_GetFpDataValidity(bBuf(0))
    If iRec = FPResultOK Then
        txtCheckFP.Text = txtCheckFP.Text & "指纹验证成功！" & vbNewLine
    Else
        txtCheckFP.Text = txtCheckFP.Text & "指纹验证失败！" & vbNewLine
        MsgBox "指纹验证失败！", 16, "提示"
        Exit Sub
    End If
    
    Dim lfpVer As Long, lFpSize As Long
    
    iRec = FPCONV_GetFpDataVersionAndSize(bBuf(0), lfpVer, lFpSize)
    If iRec = FPResultOK Then
        txtCheckFP.Text = txtCheckFP.Text & "输入数据长度:" & UBound(bBuf) + 1 & vbNewLine
        txtCheckFP.Text = txtCheckFP.Text & "指纹版本号:" & GetFPVerByINT(lfpVer) & vbNewLine
        txtCheckFP.Text = txtCheckFP.Text & "指纹缓冲区长度：" & lFpSize & vbNewLine
    Else
        txtCheckFP.Text = txtCheckFP.Text & "获取指纹信息失败！"
    End If
    
End Sub

Private Function GetFPVerByINT(ByVal iFPVer As Integer) As String
    Select Case iFPVer
        Case DATA_VER_70
            GetFPVerByINT = "VER_70"
        Case DATA_VER_80
            GetFPVerByINT = "VER_80"
        Case DATA_VER_85
            GetFPVerByINT = "VER_85"
        Case DATA_VER_89
            GetFPVerByINT = "VER_89"
        Case Else
            GetFPVerByINT = "未知版本:" & iFPVer
    End Select
End Function

Private Sub cmdFPConv_Click()
    Dim iRec As Long
    
    Call FPCONV_Init

    
    Dim sFP As String
    sFP = txtFingerprintTemplate.Text
    If Len(sFP) = FPResultOK Then
        Exit Sub
    End If
    
    Dim bBuf() As Byte
    bBuf = ModBase64.DecodeBase64Byte(sFP)
    iRec = FPCONV_GetFpDataValidity(bBuf(0))
    If iRec <> FPResultOK Then
        MsgBox "指纹验证失败！", 16, "提示"
        Exit Sub
    End If
    
    Dim lfpVer As Long, lFpSize As Long
    
    iRec = FPCONV_GetFpDataVersionAndSize(bBuf(0), lfpVer, lFpSize)
    If iRec <> FPResultOK Then
        MsgBox "获取指纹信息失败！", 16, "提示"
        Exit Sub
    End If
    
    Dim lDstVer As Long
    Select Case cmbFPVer.ListIndex
        Case 0
            lDstVer = DATA_VER_70
        Case 1
            lDstVer = DATA_VER_80
        Case 2
            lDstVer = DATA_VER_85
        Case 3
            lDstVer = DATA_VER_89
        Case Else
            Exit Sub
    End Select
    
    If lDstVer = lfpVer Then
        MsgBox "无需转换，版本一直！", 64, "提示"
        Exit Sub
    End If
    '获取目标算法的缓冲区长度
    Dim lDstSize As Long, bDstFP() As Byte
    FPCONV_GetFpDataSize lDstVer, lDstSize
    ReDim bDstFP(lDstSize - 1)
    
    '开始转换
    iRec = FPCONV_Convert(lfpVer, bBuf(0), lDstVer, bDstFP(0))
    If iRec = FPResultOK Then
        txtFingerprintTemplate.Text = ModBase64.EncodeBase64Byte(bDstFP)
        MsgBox "转成完毕！", 64, "提示"
    Else
        txtFingerprintTemplate.Text = "转成失败！"
        MsgBox "转成失败！", 64, "提示"
    End If
    
End Sub



'播放声音
Private Sub cmdSoundPlay_Click()
    If Not CheckOpenDevice() Then Exit Sub
    Dim vRet As Long
    vRet = pisSoundPlay(mContextID, cmbSoundPlay.ListIndex)
    If vRet <> 0 Then
        MsgBox "声音播放失败，请关闭连接重新打开后在尝试！", 64, "错误"
    End If
End Sub

'驱动LED灯
Private Sub cmdLEDSet_Click()
    If Not CheckOpenDevice() Then Exit Sub
    Dim vRet As Long
    vRet = pisLedControl(mContextID, cmdLedKind.ListIndex + 1, cmbLEDUse.ListIndex)
    If vRet <> 0 Then
        MsgBox "LED驱动失败，请关闭连接重新打开后在尝试！", 64, "错误"
    End If
End Sub


'读ID卡
Private Sub cmdReadCard_Click()
    If Not CheckOpenDevice() Then Exit Sub
    txtCardNum.Text = vbNullString
    Dim vRet As Long
    vRet = pisScanCard(mContextID, 1) '设定开始读卡
    If vRet = 0 Then
        cmdReadCard.Enabled = False
        tmrReadCard.Interval = 100
        tmrReadCard.Enabled = True
        mStopReadCard = False
        cmdStopReadCard.Visible = True
    Else
        MsgBox "启动读卡模块失败，请关闭连接重新打开后在尝试！", 64, "错误"
    End If
End Sub

'停止读卡
Private Sub StopReadCard()
    cmdReadCard.Enabled = True
    tmrReadCard.Enabled = False
    Call pisScanCard(mContextID, 0)
    cmdStopReadCard.Visible = False
End Sub



'读卡定时器，定时查询是否已读到卡
Private Sub tmrReadCard_Timer()
    If mStopReadCard Then
        Call StopReadCard
        Exit Sub
    End If
    
    Dim vRet As Long
    Dim bCardBuf() As Byte
    ReDim bCardBuf(200)
    vRet = pisGetCardNumber(mContextID, bCardBuf(0)) '设定开始读卡
    If vRet = 0 Then
        Dim sCard As String
        sCard = GetStringByByteBuf(bCardBuf)
        txtCardNum.Text = sCard
        
        Call StopReadCard
    End If
End Sub

Private Sub cmdStopReadCard_Click()
    mStopReadCard = True
    cmdStopReadCard.Visible = False
End Sub


Private Function GetStringByByteBuf(ByRef bBuf() As Byte) As String
    Dim str As String
    Dim i As Long
    Dim iCount As Long
    iCount = UBound(bBuf)
    
    For i = 0 To iCount
        If bBuf(i) = 0 Then
            iCount = i - 1
            Exit For
        End If
    Next
    Dim b() As Byte
    ReDim b(iCount)
    CopyMemory b(0), bBuf(0), iCount + 1
    str = StrConv(b, vbUnicode)
    GetStringByByteBuf = str
End Function



Private Sub GetCString(ByRef sValue As String)
    Dim iIndex As Integer
    iIndex = InStr(sValue, Chr(0))
    If iIndex > 0 Then
        sValue = Mid$(sValue, 1, iIndex - 1)
    End If
    
End Sub



Private Sub cmdAddTptArray_Click()
    If Not CheckOpenDevice() Then Exit Sub
    
    '添加模板到缓冲区
    Dim sHex As String
    Dim sFP As String
    Dim iRec As Long
    sFP = txtFingerprintTemplate.Text
    If Len(sFP) = FPResultOK Then
        Exit Sub
    End If
    
    Dim bBuf() As Byte
    bBuf = ModBase64.DecodeBase64Byte(sFP)
    iRec = FPCONV_GetFpDataValidity(bBuf(0))
    If iRec <> FPResultOK Then
        MsgBox "指纹验证失败！", 16, "提示"
        Exit Sub
    End If
    
    Dim lfpVer As Long, lFpSize As Long
    
    iRec = FPCONV_GetFpDataVersionAndSize(bBuf(0), lfpVer, lFpSize)
    If iRec <> FPResultOK Then
        MsgBox "获取指纹信息失败！", 16, "提示"
        Exit Sub
    End If
    
    If Not lfpVer = DATA_VER_85 Then
        MsgBox "指纹版本不正确，需要Ver85版本！", 16, "提示"
        Exit Sub
    End If
    
    Dim sUserID As String
    sUserID = txtUserID.Text
    If Len(sUserID) = 0 Then
        MsgBox "请输入模板号", 16, "提示"
        Exit Sub
    End If
    Dim lUserID As Long
    lUserID = Val(sUserID)
    If lUserID <= 0 Then
        MsgBox "请输入模板号", 16, "提示"
        Exit Sub
    End If
    iRec = pisAddTptArray(mContextID, lUserID, bBuf(0))
    If iRec <> FPResultOK Then
        MsgBox "加入到缓冲区失败！", 16, "提示"
        Exit Sub
    Else
        MsgBox "添加成功！", 64, "提示"
    End If
End Sub


Private Sub Command1_Click()
    If Not CheckOpenDevice() Then Exit Sub
    
    Dim sHex As String
    Dim sFP As String
    Dim iRec As Long
    sFP = txtFingerprintTemplate.Text
    If Len(sFP) = FPResultOK Then
        Exit Sub
    End If
    
    Dim bBuf() As Byte
    bBuf = ModBase64.DecodeBase64Byte(sFP)
    iRec = FPCONV_GetFpDataValidity(bBuf(0))
    If iRec <> FPResultOK Then
        MsgBox "指纹验证失败！", 16, "提示"
        Exit Sub
    End If
    
    Dim lfpVer As Long, lFpSize As Long
    
    iRec = FPCONV_GetFpDataVersionAndSize(bBuf(0), lfpVer, lFpSize)
    If iRec <> FPResultOK Then
        MsgBox "获取指纹信息失败！", 16, "提示"
        Exit Sub
    End If
    
    If Not lfpVer = DATA_VER_85 Then
        MsgBox "指纹版本不正确，需要Ver85版本！", 16, "提示"
        Exit Sub
    End If

    Dim lUserID As Long
    lUserID = 0
   
    iRec = pisIdentifyTpl(mContextID, bBuf(0), lUserID)
    If iRec <> FPResultOK Then
        MsgBox "模板比对失败！", 16, "提示"
        Exit Sub
    Else
        MsgBox "模板比对成功，模板号：" & lUserID, 64, "提示"
    End If
End Sub


Private Sub cmdIdentify_Click()
    If Not CheckOpenDevice() Then Exit Sub
    
    cmdIdentify.Enabled = False
    Dim vRet As Long
    Dim iCheck As Long, iFPArea As Long
    Dim sTip As String
    Dim bTmp() As Byte
    Dim bCheck As Boolean
    
                        
    vRet = pisCapture(mContextID, mImageBuf(0))

    If vRet = FPResultOK Then
        vRet = pisCheckFp(mContextID, mImageBuf(0), mImgWidth, mImgHeight, mImgDPI, iCheck, iFPArea)
        If vRet = FPResultOK Then
            If iCheck = 1 Then
                If iFPArea > 40 Then
                    DrawFPImage mImageBuf
                    
                    ReDim bTmp(mFeatureSize - 1)
                    vRet = pisProcess(mContextID, mImageBuf(0), mImgWidth, mImgHeight, mImgDPI, bTmp(0))
                    If vRet = FPResultOK Then
                        
                        Dim bUpdateTmp() As Byte
                        Dim lTID As Long, lupdatedFlag As Long
                        lTID = 0
                        lupdatedFlag = 0
                        
                        ReDim bUpdateTmp(mTemplateSize - 1)
                        vRet = pisIdentify(mContextID, bTmp(0), lTID, bUpdateTmp(0), lupdatedFlag)
                        If vRet = FPResultOK Then
                            bCheck = True
                            MsgBox "指纹验证成功,模板号：" & lTID, 64, "提示"
                        End If
                    End If
                End If
            End If
        End If
    End If
    If Not bCheck Then
        MsgBox "指纹验证失败！", 16, "错误"
    End If
    cmdIdentify.Enabled = True
End Sub

