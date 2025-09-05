Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Text
Imports DoNetDrive
Imports DoNetDrive.Common.Extensions
Imports DoNetDrive.Core
Imports DoNetDrive.Core.Command
Imports DoNetDrive.Core.Connector
Imports DoNetDrive.Core.Connector.TCPClient
Imports DoNetDrive.Core.Connector.TCPServer.Client
Imports DoNetDrive.Core.Connector.UDP
Imports DoNetDrive.Core.Data
Imports DoNetDrive.Protocol
Imports DoNetDrive.Protocol.Door.Door8800.Data
Imports DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting
Imports DoNetDrive.Protocol.Door8800
Imports DoNetDrive.Protocol.Fingerprint.AdditionalData
Imports DoNetDrive.Protocol.Fingerprint.Data.Transaction
Imports DoNetDrive.Protocol.Fingerprint.SystemParameter.Watch
Imports DoNetDrive.Protocol.Fingerprint.Transaction
Imports DoNetDrive.Protocol.Transaction
Imports CardTransaction = DoNetDrive.Protocol.Fingerprint.Data.Transaction.CardTransaction

Partial Public Class Form1
    Inherits Form

    Private mAllocator As ConnectorAllocator
    Private mObserver As ConnectorObserverHandler
    Private Shared TransactionTypeName As String()
    Public Shared mWatchTypeNameList As String()
    Public Shared mCardTransactionList, mDoorSensorTransactionList, mSystemTransactionList As String()
    Public Shared mTransactionCodeNameList As List(Of String())
    Public Shared mDownloadTypeList As String()
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub IniForm()
        TransactionTypeName = New String(254) {}
        TransactionTypeName = New String(254) {}
        TransactionTypeName(1) = "认证记录 Certification records"
        TransactionTypeName(2) = "门操作记录 Door sensor records"
        TransactionTypeName(3) = "系统记录 System records"
        TransactionTypeName(4) = "体温记录 Temperature records"
        TransactionTypeName(&HA0) = "连接测试 Remote connection test"
        TransactionTypeName(&H22) = "保活包 Keep alive package"
        mAllocator = ConnectorAllocator.GetAllocator()
        mObserver = New ConnectorObserverHandler()
        AddHandler mAllocator.CommandCompleteEvent, AddressOf mAllocator_CommandCompleteEvent

        AddHandler mAllocator.CommandErrorEvent, AddressOf mAllocator_CommandErrorEvent
        AddHandler mAllocator.CommandProcessEvent, AddressOf mAllocator_CommandProcessEvent
        AddHandler mAllocator.CommandTimeout, AddressOf mAllocator_CommandTimeout
        AddHandler mAllocator.AuthenticationErrorEvent, AddressOf MAllocator_AuthenticationErrorEvent
        AddHandler mAllocator.TransactionMessage, AddressOf MAllocator_TransactionMessage
        AddHandler mAllocator.ConnectorConnectedEvent, AddressOf mAllocator_ConnectorConnectedEvent
        AddHandler mAllocator.ConnectorClosedEvent, AddressOf mAllocator_ConnectorClosedEvent
        AddHandler mAllocator.ConnectorErrorEvent, AddressOf mAllocator_ConnectorErrorEvent
        AddHandler mAllocator.ClientOnline, AddressOf MAllocator_ClientOnline
        AddHandler mAllocator.ClientOffline, AddressOf MAllocator_ClientOffline
        AddHandler mObserver.DisposeRequestEvent, AddressOf MObserver_DisposeRequestEvent
        AddHandler mObserver.DisposeResponseEvent, AddressOf MObserver_DisposeResponseEvent
    End Sub

    Private Sub MObserver_DisposeResponseEvent(ByVal connector As INConnector, ByVal msg As String)
        AddIOLog(connector.GetConnectorDetail(), "发送数据 Send data", msg)
    End Sub

    Private Sub MObserver_DisposeRequestEvent(ByVal connector As INConnector, ByVal msg As String)
        AddIOLog(connector.GetConnectorDetail(), "接收数据 Receive Data", msg)
    End Sub

    Private Sub MAllocator_ClientOffline(ByVal sender As Object, ByVal e As ServerEventArgs)
    End Sub

    Private Sub MAllocator_ClientOnline(ByVal sender As Object, ByVal e As ServerEventArgs)
        Dim inc As INConnector = TryCast(sender, INConnector)
        inc.AddRequestHandle(mObserver)

        Select Case inc.GetConnectorType()
            Case ConnectorType.UDPClient
                inc.OpenForciblyConnect()
                Dim Door8800Request As Door8800RequestHandle = New Door8800RequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.[Default], AddressOf RequestHandleFactory)
                inc.RemoveRequestHandle(GetType(Door8800RequestHandle))
                inc.AddRequestHandle(Door8800Request)
            Case Else
        End Select
    End Sub

    Private Function RequestHandleFactory(ByVal sn As String, ByVal cmdIndex As Byte, ByVal cmdPar As Byte) As AbstractTransaction
        If cmdIndex >= 1 AndAlso cmdIndex <= 4 Then
            Return ReadTransactionDatabaseByIndex.NewTransactionTable(cmdIndex)()
        End If

        If cmdIndex = &H22 Then
            Return New DoNetDrive.Protocol.Door.Door8800.Data.Transaction.KeepaliveTransaction()
        End If

        If cmdIndex = &HA0 Then
            Return New DoNetDrive.Protocol.Door.Door8800.Data.Transaction.ConnectMessageTransaction()
        End If

        Return Nothing
    End Function

    Private Sub mAllocator_ConnectorErrorEvent(ByVal sender As Object, ByVal connector As INConnectorDetail)
        Select Case connector.GetTypeName()
            Case ConnectorType.UDPServer
                AddIOLog(connector, "UDP绑定 UDP binding", "UDP绑定失败 UDP binding failed ")
            Case Else
                AddIOLog(connector, "错误 Error", "连接失败 Connect failed")
        End Select
    End Sub

    Private Sub mAllocator_ConnectorClosedEvent(ByVal sender As Object, ByVal connector As INConnectorDetail)
        Select Case connector.GetTypeName()
            Case ConnectorType.UDPServer
                AddIOLog(connector, "UDP绑定", "UDP绑定已关闭")
            Case Else
                AddIOLog(connector, "关闭 UDP binding", "连接通道已关闭 UDP binding failed")
        End Select
    End Sub

    Private Sub mAllocator_ConnectorConnectedEvent(ByVal sender As Object, ByVal connector As INConnectorDetail)
        Select Case connector.GetTypeName()
            Case ConnectorType.UDPServer
                AddIOLog(connector, "UDP绑定 UDP binding", "UDP绑定成功 UDP binding Successfully")
            Case Else
                mAllocator.GetConnector(connector).AddRequestHandle(mObserver)
                AddIOLog(connector, "Succeed", "通道连接成功 Channel Connection Successfully")
        End Select
    End Sub

    Private Sub AddIOLog(ByVal connDetail As INConnectorDetail, ByVal sTag As String, ByVal txt As String)
        Dim Local, Remote, [cType] As String
        GetConnectorDetail(connDetail, [cType], Local, Remote)
        Me.Invoke(CType((Sub()
                             dgvResult.Rows.Insert(0, sTag, txt, [cType], Remote, Local, DateTime.Now.ToTimeffff())
                         End Sub), Action))
    End Sub

    Private Sub MAllocator_TransactionMessage(ByVal connector As INConnectorDetail, ByVal EventData As INData)
        Dim commandResult As CommandResult = New CommandResult()
        Dim fcTrn As Door8800Transaction = TryCast(EventData, Door8800Transaction)
        Dim strbuf As StringBuilder = New StringBuilder()
        Dim evn = fcTrn.EventData
        commandResult.Title = TransactionTypeName(fcTrn.CmdIndex)
        commandResult.SN = fcTrn.SN
        Dim Local, Remote, [cType] As String
        GetConnectorDetail(connector, [cType], Local, Remote)
        commandResult.Remote = Remote
        commandResult.Time = fcTrn.EventData.TransactionDate.ToString("yyyy-MM-dd HH:mm:ss")
        commandResult.Timemill = "-"
        Dim dtl As TCPClientDetail = TryCast(connector, TCPClientDetail)
        Dim model As ConnectorModel = New ConnectorModel()
        model.RemoteIP = dtl.Addr
        model.Password = "FFFFFFFF"
        model.SN = fcTrn.SN
        model.RemotePort = dtl.Port
        Invoke(Sub()
                   AddDevic(model)
               End Sub)
        If fcTrn.CmdIndex < 4 Then

            If fcTrn.CmdIndex = 1 Then
                Dim cardtrn As CardTransaction = TryCast(evn, CardTransaction)
                Dim dv As DataGridViewRow = CreateRow()
                dv.Cells(2).Value = evn.TransactionDate.ToString("yyyy-MM-dd HH:mm:ss")
                dv.Cells(3).Value = cardtrn.UserCode
                dv.Cells(4).Value = evn.SerialNumber
                dv.Cells(5).Value = evn.TransactionCode
                dv.Cells(6).Value = dtl.Addr & "__" + fcTrn.SN

                Dim cmdDtl = GetCommandDetail(model)
                DownRecordPhotos(evn.SerialNumber, model, dv)
                Invoke(Sub()
                           DGV_Msg.Rows.Insert(0, dv)
                       End Sub)
            End If
        End If
    End Sub

    Private deviceKey As Dictionary(Of String, String) = New Dictionary(Of String, String)()

    Private Sub AddDevic(ByVal model As ConnectorModel)
        If Not deviceKey.ContainsKey(model.RemoteIP) Then
            deviceKey.Add(model.RemoteIP, "")
            Dim index As Integer = ComBox_DeviceList.Items.Add(model)
            ComBox_DeviceList.SelectedIndex = index
        End If
    End Sub

    Private Function CreateRow() As DataGridViewRow
        Dim dv As DataGridViewRow = New DataGridViewRow()
        dv.Cells.Add(New DataGridViewImageCell())

        For i As Integer = 0 To 6 - 1
            dv.Cells.Add(New DataGridViewTextBoxCell())
        Next

        Return dv
    End Function

    Private Sub DownRecordPhotos(ByVal SerialNumber As Integer, ByVal connector As ConnectorModel, ByVal dv As DataGridViewRow)
        Dim cmdDtl As INCommandDetail = GetCommandDetail(connector)
        Dim par As ReadFile_Parameter = New ReadFile_Parameter(SerialNumber, 3, 1)
        Dim cmd As INCommand = New ReadFile(cmdDtl, par)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    Dim result = TryCast(cmd.getResult(), ReadFeatureCode_Result)


                                                    If result.FileHandle = 0 Then
                                                    End If

                                                    If result.Result Then

                                                        If result.FileDatas IsNot Nothing Then
                                                            Invoke(Sub()
                                                                       dv.Cells(0).Value = Image.FromStream(New System.IO.MemoryStream(result.FileDatas))
                                                                   End Sub)
                                                        End If

                                                        DownBodyTemperature(SerialNumber, connector, dv)
                                                    End If
                                                End Sub
    End Sub

    Private Sub DownBodyTemperature(ByVal SerialNumber As Integer, ByVal connector As ConnectorModel, ByVal dv As DataGridViewRow)
        Dim cmdDtl As INCommandDetail = GetCommandDetail(connector)
        cmdDtl.Timeout = 2000
        Dim par = New ReadTransactionDatabaseByIndex_Parameter(4, SerialNumber, 1)
        Dim cmd = New ReadTransactionDatabaseByIndex(cmdDtl, par)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    Dim result = TryCast(cmde.Command.getResult(), DoNetDrive.Protocol.Door.Door8800.Transaction.ReadTransactionDatabaseByIndex_Result)

                                                    If result.TransactionList.Count > 0 Then
                                                        Dim body = TryCast(result.TransactionList(0), BodyTemperatureTransaction)
                                                        Invoke(Sub()
                                                                   dv.Cells(1).Value = (CDbl(body.BodyTemperature)) / (CDbl(10))
                                                               End Sub)
                                                    End If
                                                End Sub
    End Sub

    Private Overloads Sub Invoke(ByVal p As Action)
        Try
            Invoke(CType(p, [Delegate]))
        Catch __unusedException1__ As Exception
            Return
        End Try
    End Sub

    Public Function GetCommandDetail(ByVal connector As ConnectorModel) As INCommandDetail
        Dim connectType As CommandDetailFactory.ConnectType = CommandDetailFactory.ConnectType.UDPClient
        Dim protocolType As CommandDetailFactory.ControllerType = CommandDetailFactory.ControllerType.Door89H
        Dim cmdDtl = CommandDetailFactory.CreateDetail(connectType, connector.RemoteIP, connector.RemotePort, protocolType, connector.SN, connector.Password)
        Dim dtl As UDPClientDetail = TryCast(cmdDtl.Connector, UDPClientDetail)
        dtl.LocalAddr = mServerIP
        dtl.LocalPort = mServerPort
        cmdDtl.Timeout = 600
        cmdDtl.RestartCount = 3
        Return cmdDtl
    End Function

    Private Sub MAllocator_AuthenticationErrorEvent(ByVal sender As Object, ByVal e As CommandEventArgs)
    End Sub

    Private Sub mAllocator_CommandTimeout(ByVal sender As Object, ByVal e As CommandEventArgs)
    End Sub

    Private Sub mAllocator_CommandProcessEvent(ByVal sender As Object, ByVal e As CommandEventArgs)
    End Sub

    Private Sub mAllocator_CommandErrorEvent(ByVal sender As Object, ByVal e As CommandEventArgs)
    End Sub

    Private Sub mAllocator_CommandCompleteEvent(ByVal sender As Object, ByVal e As CommandEventArgs)
    End Sub

    Private Sub GetConnectorDetail(ByVal conn As INConnectorDetail, <Out> ByRef [cType] As String, <Out> ByRef sLocal As String, <Out> ByRef Remote As String)
        sLocal = String.Empty
        Remote = String.Empty
        [cType] = String.Empty
        Dim oConn = mAllocator.GetConnector(conn)
        If oConn Is Nothing Then Return
        Dim local As IPDetail = oConn.LocalAddress()
        conn = oConn.GetConnectorDetail()

        Select Case conn.GetTypeName()
            Case ConnectorType.TCPClient
                Dim tcpclient = TryCast(conn, TCPClientDetail)
                [cType] = "TCP客户端   TCP Client"
                sLocal = $"{local}"
                Remote = $"{tcpclient.Addr}:{tcpclient.Port}"
            Case ConnectorType.TCPServerClient
                [cType] = "TCP客户端节点   TCP Client Node "
                Dim tcpclientOnly = TryCast(conn, TCPServerClientDetail)
                sLocal = $"{local}"
                Remote = $"{tcpclientOnly.Remote.ToString()}"
            Case ConnectorType.UDPClient
                [cType] = "UDP客户端  UDP Client"
                Dim udpOnly = TryCast(conn, TCPClientDetail)
                sLocal = $"{local}"
                Remote = $"{udpOnly.Addr}:{udpOnly.Port}"
            Case ConnectorType.UDPServer
                [cType] = "UDP服务器  UDP Server"
                sLocal = $"{local}"
            Case ConnectorType.TCPServer
                [cType] = "TCP服务器  TCP Server"
                sLocal = $"{local}"
                'Case ConnectorType.SerialPort
                '    [cType] = "串口 Serial Port"
                '    Dim com = TryCast(conn, SerialPortDetail)
                '    sLocal = $"COM{local.Port}:{com.Baudrate}"
            Case Else
                [cType] = conn.GetTypeName()
                sLocal = $"{conn.GetKey()}"
        End Select
    End Sub

    Private mUDPIsBind As Boolean = False
    Private mServerIP As String
    Private mServerPort As Integer

    Private Sub But_bind_Click(ByVal sender As Object, ByVal e As EventArgs) Handles But_bind.Click
        If Nub_LocalPort.Value <= 0 Then
            MsgErr("端口号不正确！Incorrect port number")
            Return
        End If

        Dim port As Integer = CInt(Nub_LocalPort.Value)
        Dim sLocalIP As String = ComBox_LocalIP.SelectedItem?.ToString()

        If String.IsNullOrEmpty(sLocalIP) Then
            MsgErr("没有绑定本地IP！ No local IP is bound")
            Return
        End If

        Dim detail As UDPServerDetail = New UDPServerDetail(sLocalIP, port)

        If mUDPIsBind Then
            mAllocator.CloseConnector(detail)
            But_bind.Text = "开启服务 Enable service"
            mUDPIsBind = False
            Nub_LocalPort.Enabled = True
            ComBox_LocalIP.Enabled = True
        Else
            But_bind.Enabled = False
            mUDPIsBind = True
            Nub_LocalPort.Enabled = False
            ComBox_LocalIP.Enabled = False
            mAllocator.OpenConnector(detail)
            mServerIP = sLocalIP
            mServerPort = port
        End If
    End Sub

    Public Sub MsgErr(ByVal sText As String)
        MessageBox.Show(sText, "错误 Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub IniLoadLocalIP()
        ComBox_LocalIP.Items.Clear()
        Dim localentry As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())

        For Each oItem As IPAddress In localentry.AddressList
            Dim ip As IPAddress = oItem

            If ip.IsIPv4MappedToIPv6 Then
                ip = ip.MapToIPv4()
            End If

            If ip.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                ComBox_LocalIP.Items.Add(ip.ToString())
            End If
        Next

        If ComBox_LocalIP.Items.Count > 0 Then
            ComBox_LocalIP.SelectedIndex = ComBox_LocalIP.Items.Count - 1
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        IniLoadLocalIP()
        IniForm()
        Initialize()
        ComBox_DeviceList.DisplayMember = "RemoteIP"
        e_TransactionDatabaseType()
        But_bind_Click(Nothing, Nothing)
    End Sub
    Public Sub e_TransactionDatabaseType()
        Dim array As String() = New String() {"读卡记录 Reading card record", "门磁记录 Door sensor record", "系统记录 System Record", "体温记录 Temperature record"}
        cboe_TransactionDatabaseType1.Items.Clear()
        cboe_TransactionDatabaseType1.Items.AddRange(array)
        cboe_TransactionDatabaseType1.SelectedIndex = 0
        cboe_TransactionDatabaseType2.Items.Clear()
        cboe_TransactionDatabaseType2.Items.AddRange(array)
        cboe_TransactionDatabaseType2.SelectedIndex = 0
        cboe_TransactionDatabaseType3.Items.Clear()
        cboe_TransactionDatabaseType3.Items.AddRange(array)
        cboe_TransactionDatabaseType3.SelectedIndex = 0
    End Sub
    Private Sub Initialize()
        mWatchTypeNameList = New String() {"", "读卡信息 Reading card info", "门磁信息 Door Sensor Info", "系统信息 System Info", "连接保活消息 Connection keep alive info ", "连接确认信息 Connection confirm info "}
        mCardTransactionList = New String(255) {}
        mDoorSensorTransactionList = New String(255) {}
        mSystemTransactionList = New String(255) {}
        mTransactionCodeNameList = New List(Of String())(10)
        mTransactionCodeNameList.Add(Nothing)
        mTransactionCodeNameList.Add(mCardTransactionList)
        mTransactionCodeNameList.Add(mDoorSensorTransactionList)
        mTransactionCodeNameList.Add(mSystemTransactionList)
        mCardTransactionList(1) = "刷卡验证 Reading card identification"
        mCardTransactionList(2) = "指纹验证 Fingerprint Identification"
        mCardTransactionList(3) = "人脸验证 Face Identification"
        mCardTransactionList(4) = "指纹 + 刷卡 Fingerprint + swipe card"
        mCardTransactionList(5) = "人脸 + 指纹 Face + fingerprint"
        mCardTransactionList(6) = "人脸 + 刷卡 Face + swipe card"
        mCardTransactionList(7) = "刷卡 + 密码 Swipe card + password"
        mCardTransactionList(8) = "人脸 + 密码 Face + password"
        mCardTransactionList(9) = "指纹 + 密码 ：Fingerprint + password"
        mCardTransactionList(10) = "手动输入用户号加密码验证 Manually enter the user number and password for verification"
        mCardTransactionList(11) = "指纹+刷卡+密码 Fingerprint + swipe card + password"
        mCardTransactionList(12) = "人脸+刷卡+密码 Face + swipe card + password"
        mCardTransactionList(13) = "人脸+指纹+密码 Face + fingerprint + password"
        mCardTransactionList(14) = "人脸+指纹+刷卡 Face + fingerprint + swipe card"
        mCardTransactionList(15) = "重复验证 Repeated Identification"
        mCardTransactionList(16) = "有效期过期 Expiry Date"
        mCardTransactionList(17) = "开门时段过期 Open period expired"
        mCardTransactionList(18) = "节假日时不能开门 Can't open the door on holidays"
        mCardTransactionList(19) = "未注册用户 Unregistered user"
        mCardTransactionList(20) = "探测锁定 Detection Lock"
        mCardTransactionList(21) = "有效次数已用尽 Effective times are exhausted"
        mCardTransactionList(22) = "锁定时验证，禁止开门 Verify when locked, do not open the door"
        mCardTransactionList(23) = "挂失卡 Reported missing card"
        mCardTransactionList(24) = "黑名单卡 Blacklist card"
        mCardTransactionList(25) = "免验证开门 Verification free open -- 按指纹时用户号为0 The user number is 0 when the fingerprint is pressed，刷卡时用户号是卡号 The user number is the card number when swiping the card"
        mCardTransactionList(26) = "禁止刷卡验证 Card verification is prohibited  --  【权限认证方式 】中禁用刷卡时 ：[permission authentication method] is disabled when swiping a card"
        mCardTransactionList(27) = "禁止指纹验证  --  【权限认证方式】中禁用指纹时 Disable fingerprint verification -- disable fingerprint in [permission authentication method]"
        mCardTransactionList(28) = "控制器已过期 Controller expired"
        mCardTransactionList(29) = "验证通过—有效期即将过期 Verification passed - the expiration date is about to expire"
        mDoorSensorTransactionList(1) = "开门 Unlock"
        mDoorSensorTransactionList(2) = "关门 Lock"
        mDoorSensorTransactionList(3) = "进入门磁报警状态 Enter door sensor alarm state"
        mDoorSensorTransactionList(4) = "退出门磁报警状态 Exit door sensor alarm state"
        mDoorSensorTransactionList(5) = "门未关好 Door open"
        mDoorSensorTransactionList(6) = "使用按钮开门 Use the button to open the door"
        mDoorSensorTransactionList(7) = "按钮开门时门已锁定 Door lock when press the button"
        mDoorSensorTransactionList(8) = "按钮开门时控制器已过期 Controller expired when press the button"
        mSystemTransactionList(1) = "软件开门 Software to open the door"
        mSystemTransactionList(2) = "软件关门 Software to close the door"
        mSystemTransactionList(3) = "软件常开 The software normally open"
        mSystemTransactionList(4) = "控制器自动进入常开 The controller enters the normally open automatically"
        mSystemTransactionList(5) = "控制器自动关闭门 The controller closes the door automatically"
        mSystemTransactionList(6) = "长按出门按钮常开 Long press the door button normally open"
        mSystemTransactionList(7) = "长按出门按钮常闭 Long press the door button normally close"
        mSystemTransactionList(8) = "软件锁定 Software lock"
        mSystemTransactionList(9) = "软件解除锁定 Software unlock"
        mSystemTransactionList(10) = "控制器定时锁定--到时间自动锁定 Controller timing lock - automatically lock on time"
        mSystemTransactionList(11) = "控制器定时锁定--到时间自动解除锁定 Controller timing lock - automatically unlock at time"
        mSystemTransactionList(12) = "报警--锁定 Alarm -- lock"
        mSystemTransactionList(13) = "报警--解除锁定 Alarm -- unlock"
        mSystemTransactionList(14) = "非法认证报警 Illegal authentication alarm"
        mSystemTransactionList(15) = "门磁报警 Door Sensor Alarm "
        mSystemTransactionList(16) = "胁迫报警 Duress Alarm"
        mSystemTransactionList(17) = "开门超时报警 Door Timeout Alarm"
        mSystemTransactionList(18) = "黑名单报警 Blacklist Alarm"
        mSystemTransactionList(19) = "消防报警 Fire Alarm"
        mSystemTransactionList(20) = "防拆报警 Tamper Alarm"
        mSystemTransactionList(21) = "非法认证报警解除 Removed illegal certification alarm removed"
        mSystemTransactionList(22) = "门磁报警解除 ：Removed door sensor alarm"
        mSystemTransactionList(23) = "胁迫报警解除 Removed duress alarm "
        mSystemTransactionList(24) = "开门超时报警解除 Removed door timeout alarm"
        mSystemTransactionList(25) = "黑名单报警解除 Removed blacklist alarm"
        mSystemTransactionList(26) = "消防报警解除 Removed fire alarm"
        mSystemTransactionList(27) = "防拆报警解除 Removed tamper alarm"
        mSystemTransactionList(28) = "系统加电 System is powered"
        mSystemTransactionList(29) = "系统错误复位（看门狗）System error reset (watchdog)"
        mSystemTransactionList(30) = "设备格式化记录 ：Device formatting record"
        mSystemTransactionList(31) = "读卡器接反 The line of card reader is reversed"
        mSystemTransactionList(32) = "读卡器线路未接好 The card reader is disconnected"
        mSystemTransactionList(33) = "无法识别的读卡器 Unrecognized card reader"
        mSystemTransactionList(34) = "网线已断开 The network cable is disconnected"
        mSystemTransactionList(35) = "网线已插入 The network cable has been inserted"
        mSystemTransactionList(36) = "WIFI 已连接 WIFI is connected"
        mSystemTransactionList(37) = "WIFI 已断开 WIFI is disconnected"
        mDownloadTypeList = New String(1) {}
        mDownloadTypeList(0) = "User photo"
        mDownloadTypeList(1) = "Record photos"

        cmbDownloadType.Items.Clear()
        cmbDownloadType.Items.AddRange(mDownloadTypeList)
        cmbDownloadType.SelectedIndex = 0
        cmbDownloadSerialNumber.SelectedIndex = 0
    End Sub
    Private Sub But_Set_Click(ByVal sender As Object, ByVal e As EventArgs) Handles But_Set.Click
        Dim connector As ConnectorModel = New ConnectorModel()
        connector.RemoteIP = Txt_DeviceIP.Text.Trim()
        connector.RemotePort = CInt(Nub_DevicePort.Value)
        connector.SN = Txt_DeviceSN.Text.Trim()
        connector.Password = Txt_DevicePwd.Text.Trim()
        Dim cmdDtl = GetCommandDetail(connector)
        Dim cmd As ReadTCPSetting = New ReadTCPSetting(cmdDtl)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    Dim result As ReadTCPSetting_Result = TryCast(cmde.Command.getResult(), ReadTCPSetting_Result)
                                                    UpdateSeverInfo(result.TCP)
                                                End Sub

    End Sub

    Private Sub UpdateSeverInfo(ByVal tcpDetail As TCPDetail)
        Dim connector As ConnectorModel = New ConnectorModel()
        connector.RemoteIP = Txt_DeviceIP.Text.Trim()
        connector.RemotePort = CInt(Nub_DevicePort.Value)
        connector.SN = Txt_DeviceSN.Text.Trim()
        connector.Password = Txt_DevicePwd.Text.Trim()
        Dim cmdDtl = GetCommandDetail(connector)
        tcpDetail.mServerIP = Txt_ServerIP.Text.Trim()
        tcpDetail.mServerPort = CInt(Nub_LocalPort.Value)
        tcpDetail.mServerAddr = "www.pc15.net"
        tcpDetail.mTCPPort = 1
        Dim cmd As WriteTCPSetting = New WriteTCPSetting(cmdDtl, New WriteTCPSetting_Parameter(tcpDetail))
        mAllocator.AddCommand(cmd)
    End Sub

    Private Sub butTransactionDatabaseDetail_Click(sender As Object, e As EventArgs) Handles butTransactionDatabaseDetail.Click
        Dim cModel = TryCast(ComBox_DeviceList.SelectedItem, ConnectorModel)
        If cModel Is Nothing Then
            MessageBox.Show("没有设备连接 No device connection")
            Return
        End If
        Dim cmdDtl = GetCommandDetail(cModel)
        Dim cmd = New ReadTransactionDatabaseDetail(cmdDtl)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    Dim result = TryCast(cmd.getResult(), ReadTransactionDatabaseDetail_Result)
                                                    For i As Integer = 0 To 4 - 1
                                                        Dim txtQuantity As TextBox = TryCast(FindControl(GroupBox3, "txtQuantity" & (i + 1).ToString()), TextBox)
                                                        Dim txtNewRecord As TextBox = TryCast(FindControl(GroupBox3, "txtNewRecord" & (i + 1).ToString()), TextBox)
                                                        Dim txtWriteIndex As TextBox = TryCast(FindControl(GroupBox3, "txtWriteIndex" & (i + 1).ToString()), TextBox)
                                                        Dim txtReadIndex As TextBox = TryCast(FindControl(GroupBox3, "txtReadIndex" & (i + 1).ToString()), TextBox)
                                                        Dim txtIsCircle As TextBox = TryCast(FindControl(GroupBox3, "txtIsCircle" & (i + 1).ToString()), TextBox)
                                                        Invoke(Sub()
                                                                   txtQuantity.Text = result.DatabaseDetail.ListTransaction(i).DataBaseMaxSize.ToString()
                                                                   txtWriteIndex.Text = result.DatabaseDetail.ListTransaction(i).WriteIndex.ToString()
                                                                   txtNewRecord.Text = result.DatabaseDetail.ListTransaction(i).readable().ToString()
                                                                   txtReadIndex.Text = result.DatabaseDetail.ListTransaction(i).ReadIndex.ToString()
                                                                   txtIsCircle.Text = If(result.DatabaseDetail.ListTransaction(i).IsCircle, "【1、循环 Cycle】", "【0、未循环 Not Cycle】")
                                                               End Sub)
                                                    Next
                                                End Sub

    End Sub

    Public Overloads Function FindControl(ByVal parentControl As Control, ByVal findCtrlName As String) As Control
        Dim _findedControl As Control = Nothing

        If Not String.IsNullOrEmpty(findCtrlName) AndAlso parentControl IsNot Nothing Then

            For Each ctrl As Control In parentControl.Controls

                If ctrl.Name.Equals(findCtrlName) Then
                    _findedControl = ctrl
                    Exit For
                End If
            Next
        End If

        Return _findedControl
    End Function

    ''' <summary>
    ''' 上传记录尾号
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub butTransactionDatabaseWriteIndex_Click(sender As Object, e As EventArgs) Handles butTransactionDatabaseWriteIndex.Click
        Dim type As Integer = cboe_TransactionDatabaseType1.SelectedIndex
        Dim WriteIndex As Integer = Integer.Parse(txtWriteIndex.Text.ToString())
        Dim cModel = TryCast(ComBox_DeviceList.SelectedItem, ConnectorModel)
        If cModel Is Nothing Then
            MessageBox.Show("没有设备连接 No device connection")
            Return
        End If
        Dim cmdDtl = GetCommandDetail(cModel)
        Dim par = New WriteTransactionDatabaseWriteIndex_Parameter(Get_TransactionDatabaseType(type), WriteIndex)
        Dim cmd = New WriteTransactionDatabaseWriteIndex(cmdDtl, par)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    '' mMainForm.AddLog($"命令成功  Command Successfully")
                                                    Me.Invoke(CType((Sub()
                                                                         dgvResult.Rows.Insert(0, "上传记录尾号", "命令成功  Command Successfully", "", "", "", DateTime.Now.ToTimeffff())
                                                                     End Sub), Action))

                                                End Sub
    End Sub
    ''' <summary>
    ''' 上传记录断点
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub butTransactionDatabaseReadIndex_Click(sender As Object, e As EventArgs) Handles butTransactionDatabaseReadIndex.Click
        Dim type As Integer = cboe_TransactionDatabaseType1.SelectedIndex
        Dim ReadIndex As Integer = Integer.Parse(txtReadIndex.Text.ToString())
        Dim cModel = TryCast(ComBox_DeviceList.SelectedItem, ConnectorModel)
        If cModel Is Nothing Then
            MessageBox.Show("没有设备连接 No device connection")
            Return
        End If
        Dim cmdDtl = GetCommandDetail(cModel)
        Dim par = New WriteTransactionDatabaseReadIndex_Parameter(Get_TransactionDatabaseType(type), ReadIndex)
        Dim cmd = New WriteTransactionDatabaseReadIndex(cmdDtl, par)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    Invoke(Sub()
                                                               dgvResult.Rows.Insert(0, "上传记录断点", "命令成功  Command Successfully", "", "", "", DateTime.Now.ToTimeffff())
                                                           End Sub)
                                                End Sub
    End Sub
    Private Function Get_TransactionDatabaseType(ByVal type As Integer) As e_TransactionDatabaseType
        type = type + 1
        Dim i = Fingerprint.Transaction.e_TransactionDatabaseType.OnCardTransaction

        If type = 2 Then
            i = Fingerprint.Transaction.e_TransactionDatabaseType.OnDoorSensorTransaction
        End If

        If type = 3 Then
            i = Fingerprint.Transaction.e_TransactionDatabaseType.OnSystemTransaction
        End If

        If type = 4 Then
            i = Fingerprint.Transaction.e_TransactionDatabaseType.OnBodyTemperatureTransaction
        End If

        Return i
    End Function
    ''' <summary>
    ''' 清空数据
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub butClearTransactionDatabase_Click(sender As Object, e As EventArgs) Handles butClearTransactionDatabase.Click
        Dim type As Integer = cboe_TransactionDatabaseType2.SelectedIndex
        Dim cModel = TryCast(ComBox_DeviceList.SelectedItem, ConnectorModel)
        If cModel Is Nothing Then
            MessageBox.Show("没有设备连接 No device connection")
            Return
        End If
        Dim cmdDtl = GetCommandDetail(cModel)
        Dim par = New ClearTransactionDatabase_Parameter(Get_TransactionDatabaseType(type))
        Dim cmd = New ClearTransactionDatabase(cmdDtl, par)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    '‘ mMainForm.AddLog($"清空数据 Command Successfully")
                                                    Invoke(Sub()
                                                               dgvResult.Rows.Insert(0, "清空数据", "命令成功  Command Successfully", "", "", "", DateTime.Now.ToTimeffff())
                                                           End Sub)
                                                End Sub
    End Sub

    Private Sub butTransactionDatabaseByIndex_Click(sender As Object, e As EventArgs) Handles butTransactionDatabaseByIndex.Click
        Dim type As Integer = cboe_TransactionDatabaseType3.SelectedIndex
        Dim Quantity As Integer = Integer.Parse(txtQuantity.Text.ToString())
        Dim ReadIndex As Integer = Integer.Parse(txtReadIndex0.Text.ToString())
        Dim cModel = TryCast(ComBox_DeviceList.SelectedItem, ConnectorModel)
        If cModel Is Nothing Then
            MessageBox.Show("没有设备连接 No device connection")
            Return
        End If
        Dim cmdDtl = GetCommandDetail(cModel)
        cmdDtl.Timeout = 2000
        Dim par = New ReadTransactionDatabaseByIndex_Parameter((cboe_TransactionDatabaseType3.SelectedIndex + 1), ReadIndex, Quantity)
        Dim cmd = New ReadTransactionDatabaseByIndex(cmdDtl, par)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    Dim result = TryCast(cmde.Command.getResult(), Protocol.Door.Door8800.Transaction.ReadTransactionDatabaseByIndex_Result)
                                                    ''  mMainForm.AddCmdLog(cmde, $"按序号读取成功，读取数量 Read the serial number successfully, read the number：{result.Quantity},实际解析数量 Actual analytic quantity：{result.TransactionList.Count}")

                                                    If result.Quantity > 0 Then
                                                        Dim sLogs As StringBuilder = New StringBuilder(result.TransactionList.Count * 100)
                                                        sLogs.AppendLine($"事件类型 Event Type：{mWatchTypeNameList(result.TransactionList(0).TransactionType)}")
                                                        sLogs.Append("读取计数 Read count：").Append(result.Quantity).Append("；实际数量 Actual Quantity：").Append(result.TransactionList.Count).AppendLine()

                                                        For Each t In result.TransactionList
                                                            PrintTransactionList(t, sLogs)
                                                        Next

                                                        Dim sFile As String = SaveFile(sLogs, $"按序号读取记录Read records by serial number_{DateTime.Now:yyyyMMddHHmmss}.txt")
                                                        Invoke(Sub()
                                                                   MessageBox.Show($"记录在保存文件 Record in save file：{sFile}")
                                                               End Sub)
                                                    End If
                                                End Sub
    End Sub

    Private Sub btnReadTransactionDatabase_Click(sender As Object, e As EventArgs) Handles btnReadTransactionDatabase.Click
        Dim type As Integer = cboe_TransactionDatabaseType3.SelectedIndex
        Dim Quantity As Integer = Integer.Parse(txtReadTransactionDatabaseQuantity.Text.ToString())
        Dim PacketSize As Integer = 0

        If txtReadTransactionDatabasePacketSize.Text <> "" Then
            PacketSize = Integer.Parse(txtReadTransactionDatabasePacketSize.Text.ToString())
        End If
        Dim cModel = TryCast(ComBox_DeviceList.SelectedItem, ConnectorModel)
        If cModel Is Nothing Then
            MessageBox.Show("没有设备连接 No device connection")
            Return
        End If
        Dim cmdDtl = GetCommandDetail(cModel)
        cmdDtl.Timeout = 1000
        cmdDtl.RestartCount = 20
        Dim par = New ReadTransactionDatabase_Parameter(CInt(Get_TransactionDatabaseType(type)), Quantity)

        If PacketSize <> 0 Then
            par.PacketSize = PacketSize
        End If

        Dim cmd = New ReadTransactionDatabase(cmdDtl, par)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    Dim result = TryCast(cmde.Command.getResult(), Protocol.Door.Door8800.Transaction.ReadTransactionDatabase_Result)
                                                    ''  mMainForm.AddCmdLog(cmde, $"读取成功，读取数量 Read successfully, read quantity：{result.Quantity},实际解析数量 Actual analytic quantity：{result.TransactionList.Count},剩余新记录数 Number of remaining new records：{result.readable}")

                                                    If result.TransactionList.Count > 0 Then
                                                        Dim sLogs As StringBuilder = New StringBuilder(result.TransactionList.Count * 100)
                                                        sLogs.AppendLine($"事件类型 Event Type：{mWatchTypeNameList(result.TransactionList(0).TransactionType)}")
                                                        sLogs.Append("读取计数 Read count：").Append(result.Quantity).Append("；实际数量 Actual Quantity：").Append(result.TransactionList.Count).Append("；剩余新记录数 Number of remaining new records：").Append(result.readable).AppendLine()
                                                        result.TransactionList.Sort(Function(x, y) x.SerialNumber.CompareTo(y.SerialNumber))

                                                        For Each t In result.TransactionList
                                                            PrintTransactionList(t, sLogs)
                                                        Next

                                                        Dim sFile As String = SaveFile(sLogs, $"读取记录 Read Records_{DateTime.Now:yyyyMMddHHmmss}.txt")
                                                        Invoke(Sub()
                                                                   MessageBox.Show($"记录在保存文件 Record in save file：{sFile}")
                                                               End Sub)
                                                    End If
                                                End Sub
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        Dim cModel = TryCast(ComBox_DeviceList.SelectedItem, ConnectorModel)
        If cModel Is Nothing Then
            MessageBox.Show("没有设备连接 No device connection")
            Return
        End If
        Dim cmdDtl = GetCommandDetail(cModel)
        If cmdDtl Is Nothing Then Return
        Dim iUsercode As Integer = 0

        If Not Integer.TryParse(txtDownloadUserCode.Text, iUsercode) OrElse iUsercode < 0 Then
            Return
        End If

        If cmbDownloadSerialNumber.SelectedIndex = -1 Then
            Return
        End If

        Dim serialNumber As Integer = Convert.ToInt32(cmbDownloadSerialNumber.SelectedItem)
        Dim cmd As INCommand
        Dim downloadType As Integer = cmbDownloadType.SelectedIndex
        If downloadType = 1 Then
            downloadType = 2
        End If

        Dim par As ReadFile_Parameter = New ReadFile_Parameter(iUsercode, downloadType + 1, serialNumber)
        cmd = New ReadFile(cmdDtl, par)
        mAllocator.AddCommand(cmd)
        AddHandler cmdDtl.CommandCompleteEvent, Sub(sdr, cmde)
                                                    Dim result = TryCast(cmd.getResult(), ReadFeatureCode_Result)

                                                    If result.FileHandle = 0 Then
                                                        ''  mMainForm.AddCmdLog(cmde, "The file to be downloaded does not exist")
                                                        Return
                                                    End If

                                                    If Not result.Result Then
                                                        ''  mMainForm.AddCmdLog(cmde, "File CRC32 failed to check！")
                                                        Return
                                                    End If

                                                    Invoke(Sub()
                                                               Dim sNewFile As String = System.IO.Path.Combine(Application.StartupPath, "Photo")
                                                               Directory.CreateDirectory(sNewFile)
                                                               sNewFile = System.IO.Path.Combine(sNewFile, $"tmpPhoto_{result.UserCode}.jpg")
                                                               File.WriteAllBytes(sNewFile, result.FileDatas)
                                                               PictureBox1.Image = Image.FromStream(New System.IO.MemoryStream(result.FileDatas))
                                                           End Sub)
                                                End Sub
    End Sub

    Private Sub BtnBeginWatch_Click(sender As Object, e As EventArgs) Handles BtnBeginWatch.Click
        Dim cModel = TryCast(ComBox_DeviceList.SelectedItem, ConnectorModel)
        If cModel Is Nothing Then
            MessageBox.Show("没有设备连接 No device connection")
            Return
        End If
        Dim cmdDtl = GetCommandDetail(cModel)
        If cmdDtl Is Nothing Then Return

        Dim cmd = New BeginWatch(cmdDtl)
        mAllocator.AddCommand(cmd)
    End Sub

    Private Sub PrintTransactionList(ByVal tr As AbstractTransaction, ByVal sLogs As StringBuilder)
        sLogs.Append("序号SN：").Append(tr.SerialNumber.ToString())

        If tr.IsNull() Then
            sLogs.AppendLine(" --- 空记录 Null Record ")
            Return
        End If

        sLogs.Append("          事件代码 Event Code：").Append(tr.TransactionCode)

        If tr.TransactionType = 4 Then
            Dim bt As BodyTemperatureTransaction = TryCast(tr, BodyTemperatureTransaction)
            sLogs.Append("          体温 BodyTemperature：").Append(bt.BodyTemperature)
            sLogs.AppendLine()
            Return
        End If

        sLogs.Append("          时间 Time：").Append(tr.TransactionDate.ToString("yyyy-MM-dd HH:mm:ss"))

        If tr.TransactionType = 3 Then
            Dim codeNameList As String() = mTransactionCodeNameList(3)
            sLogs.Append("(").Append(codeNameList(tr.TransactionCode)).Append(")")
        ElseIf tr.TransactionType = 1 Then
            Dim cardTrans As Fingerprint.Data.Transaction.CardTransaction = TryCast(tr, Fingerprint.Data.Transaction.CardTransaction)
            sLogs.Append("          用户号User No.：").Append(cardTrans.UserCode).Append("          照片 Photo：").AppendLine(If(cardTrans.Photo = 1, "有 have", "无 none"))
        Else

            If tr.TransactionType >= 2 AndAlso tr.TransactionType <= 2 Then
                Dim doorTr As AbstractDoorTransaction = TryCast(tr, AbstractDoorTransaction)
                sLogs.Append("          门号 Door No.：").Append(doorTr.Door & vbTab).AppendLine()
            Else
                sLogs.AppendLine()
            End If
        End If
    End Sub

    Public Shared Function SaveFile(ByVal sLogs As StringBuilder, ByVal sFileName As String) As String
        Dim sPath As String = System.IO.Path.Combine(Application.StartupPath, "记录日志 Record log")
        If Not System.IO.Directory.Exists(sPath) Then System.IO.Directory.CreateDirectory(sPath)
        Dim sFile As String = System.IO.Path.Combine(sPath, sFileName)
        System.IO.File.WriteAllText(sFile, sLogs.ToString(), Encoding.UTF8)
        Return sFile
    End Function
End Class

