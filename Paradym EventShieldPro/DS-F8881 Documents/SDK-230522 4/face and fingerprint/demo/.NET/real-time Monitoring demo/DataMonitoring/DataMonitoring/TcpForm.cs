using DataMonitoring.Model;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Connector.TCPClient;
using DoNetDrive.Core.Connector.TCPServer;
using DoNetDrive.Core.Connector.TCPServer.Client;
using DoNetDrive.Core.Data;
using DoNetDrive.Protocol;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.Fingerprint.AdditionalData;
using DoNetDrive.Protocol.Fingerprint.Data.Transaction;
using DoNetDrive.Protocol.Fingerprint.Transaction;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DataMonitoring
{
    public partial class TcpForm : Form
    {
        ConnectorAllocator mAllocator;
        ConnectorObserverHandler mObserver;
        private static string[] TransactionTypeName;
        public TcpForm() => InitializeComponent();


        public void IniForm()
        {
            TransactionTypeName = new string[255];

            TransactionTypeName = new string[255];
            TransactionTypeName[1] = "认证记录 Certification records";
            TransactionTypeName[2] = "门操作记录 Door sensor records";
            TransactionTypeName[3] = "系统记录 System records";
            TransactionTypeName[4] = "体温记录 Temperature records";

            TransactionTypeName[0xA0] = "连接测试 Remote connection test";
            TransactionTypeName[0x22] = "保活包 Keep alive package";
            // if (_IsClosed) return;

            mAllocator = ConnectorAllocator.GetAllocator();
            mObserver = new ConnectorObserverHandler();

            mAllocator.CommandCompleteEvent += mAllocator_CommandCompleteEvent;
            mAllocator.CommandErrorEvent += mAllocator_CommandErrorEvent;
            mAllocator.CommandProcessEvent += mAllocator_CommandProcessEvent;
            mAllocator.CommandTimeout += mAllocator_CommandTimeout;
            mAllocator.AuthenticationErrorEvent += MAllocator_AuthenticationErrorEvent;

            mAllocator.TransactionMessage += MAllocator_TransactionMessage;

            mAllocator.ConnectorConnectedEvent += mAllocator_ConnectorConnectedEvent;
            mAllocator.ConnectorClosedEvent += mAllocator_ConnectorClosedEvent;
            mAllocator.ConnectorErrorEvent += mAllocator_ConnectorErrorEvent;

            mAllocator.ClientOnline += MAllocator_ClientOnline;
            mAllocator.ClientOffline += MAllocator_ClientOffline;

            mObserver.DisposeRequestEvent += MObserver_DisposeRequestEvent;
            mObserver.DisposeResponseEvent += MObserver_DisposeResponseEvent; ;

            // butUDPBind_Click(null, null);
        }

        private void MObserver_DisposeResponseEvent(INConnector connector, string msg)
        {
            AddIOLog(connector.GetConnectorDetail(), "发送数据 Send data", msg);
        }

        private void MObserver_DisposeRequestEvent(INConnector connector, string msg)
        {
            AddIOLog(connector.GetConnectorDetail(), "接收数据 Receive Data", msg);
        }

        private void MAllocator_ClientOffline(object sender, ServerEventArgs e)
        {

        }

        private class MyTCPServerClientDetail : TCPServerClientDetail
        {
            public String SN;
            public MyTCPServerClientDetail(string sKey, IPEndPoint remoteIP, int iClientID) : base(sKey)
            {

            }

            public override string ToString()
            {
                if (string.IsNullOrEmpty(SN))
                {
                    return Remote.ToString();
                }
                return $"{SN} {Remote.ToString()}";
            }

        }
        /// <summary>
        /// 客户端连接对象集合
        /// </summary>
        private ConcurrentDictionary<string, MyTCPServerClientDetail> mTCPClients;
        private void MAllocator_ClientOnline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            inc.AddRequestHandle(mObserver);
            switch (inc.GetConnectorType())
            {
                case ConnectorType.TCPServerClient://tcp 客户端已连接
                    ///  inc.OpenForciblyConnect();
                    TCPServerClientDetail clientDetail = inc.GetConnectorDetail() as TCPServerClientDetail;
                    AddTCPClient(inc.GetKey(), clientDetail.Remote);
                    Door8800RequestHandle Door8800Request =
                     new Door8800RequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
                    inc.RemoveRequestHandle(typeof(Door8800RequestHandle));// 先删除，防止已存在就无法添加。Delete first, in case there is no way to add.
                    inc.AddRequestHandle(Door8800Request);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Add TCP Client 到列表
        /// </summary>
        private void AddTCPClient(string sKey, IPDetail remoteIP)
        {
            MyTCPServerClientDetail myTCP = new MyTCPServerClientDetail(
                sKey, new IPEndPoint(IPAddress.Parse(remoteIP.Addr), remoteIP.Port),0);
            if (!mTCPClients.TryAdd(sKey, myTCP))
            {
                //重复添加客户端，Key:{sKey}
                //   AddLog(GetLanguage("AddTCPClient0", sKey));
                return;
            }
            else
            {
                Invoke(new Action(() =>
                {
                    ComBox_DeviceList.BeginUpdate();
                    ComBox_DeviceList.Items.Add(myTCP);
                    if (ComBox_DeviceList.Items.Count == 1)
                    {
                        ComBox_DeviceList.SelectedIndex = 0;
                    }
                    else
                    {
                        ComBox_DeviceList.SelectedIndex = ComBox_DeviceList.Items.Count - 1;
                    }
                    ComBox_DeviceList.EndUpdate();
                }));

            }
        }
        /// <summary>
        /// 用于根据SN，命令参数、命令索引生产用于处理对应消息的处理类工厂函数 Used to produce processing class factory functions for processing corresponding messages according to SN, command parameters and command index.
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="cmdIndex"></param>
        /// <param name="cmdPar"></param>
        /// <returns></returns>
        private AbstractTransaction RequestHandleFactory(string sn, byte cmdIndex, byte cmdPar)
        {

            //在这里需要根据SN进行类型判定，也可以根据SN来进行查表 Here, we need to determine the type based on SN, or we can look up the table based on SN.
            if (cmdIndex >= 1 && cmdIndex <= 4)
            {
                return ReadTransactionDatabaseByIndex.NewTransactionTable[cmdIndex]();
            }
            if (cmdIndex == 0x22)
            {
                return new DoNetDrive.Protocol.Door.Door8800.Data.Transaction.KeepaliveTransaction();
            }

            if (cmdIndex == 0xA0)
            {
                return new DoNetDrive.Protocol.Door.Door8800.Data.Transaction.ConnectMessageTransaction();
            }
            return null;
        }

        private void mAllocator_ConnectorErrorEvent(object sender, INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP 服务器 Server

                    AddIOLog(connector, "UDP绑定 UDP binding", "UDP绑定失败 UDP binding failed ");
                    break;
                case ConnectorType.TCPServer://UDP 服务器 Server

                    AddIOLog(connector, "TCP绑定 TCP binding", "TCP绑定失败 TCP binding failed ");
                    break;
                default:
                    AddIOLog(connector, "错误 Error", "连接失败 Connect failed");
                    break;
            }
        }

        private void mAllocator_ConnectorClosedEvent(object sender, INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器 Server 

                    AddIOLog(connector, "UDP绑定", "UDP绑定已关闭");
                    break;
                case ConnectorType.TCPServer://UDP 服务器 Server
                    AddIOLog(connector, "TCP绑定", "TCP绑定已关闭");
                    break;
                default:
                    AddIOLog(connector, "关闭 UDP binding", "连接通道已关闭 UDP binding failed");
                    break;
            }
        }

        private void mAllocator_ConnectorConnectedEvent(object sender, INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器 Server 

                    AddIOLog(connector, "UDP绑定 UDP binding", "UDP绑定成功 UDP binding Successfully");
                    break;
                case ConnectorType.TCPServer://UDP 服务器 Server
                    AddIOLog(connector, "TCP绑定 TCP binding", "TCP绑定成功 TCP binding Successfully");
                    break;
                default:
                    mAllocator.GetConnector(connector).AddRequestHandle(mObserver);

                    AddIOLog(connector, "Succeed", "通道连接成功 Channel Connection Successfully");
                    break;
            }
        }

        private void AddIOLog(INConnectorDetail connDetail, string sTag, string txt)
        {
            string Local, Remote, cType;
            GetConnectorDetail(connDetail, out cType, out Local, out Remote);

            this.Invoke((Action)(() =>
            {
                dgvResult.Rows.Insert(0, sTag, txt, cType, Remote, Local, DateTime.Now.ToTimeffff());
            }));
        }

        /// <summary>
        /// 监控消息 Monitoring messages
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="EventData"></param>
        private void MAllocator_TransactionMessage(INConnectorDetail connector, INData EventData)
        {
            CommandResult commandResult = new CommandResult();
            var conn = mAllocator.GetConnector(connector);
            if (conn != null)
            {
                if (!conn.IsForciblyConnect())
                {
                    conn.OpenForciblyConnect();
                }
            }
       //    connector.GetKey
            Door8800Transaction fcTrn = EventData as Door8800Transaction;
            StringBuilder strbuf = new StringBuilder();
            var evn = fcTrn.EventData;

            //消息类型 Info Type
            commandResult.Title = TransactionTypeName[fcTrn.CmdIndex];
            commandResult.SN = fcTrn.SN;

            //客户端信息 Client Info
            string Local, Remote, cType;

            GetConnectorDetail(connector, out cType, out Local, out Remote);
            commandResult.Remote = Remote;
            commandResult.Time = fcTrn.EventData.TransactionDate.ToDateTimeStr();
            commandResult.Timemill = "-";
            ConnectorModel model = new ConnectorModel();
            var sIP = string.Empty;
            var tcp = connector as TCPServerClientDetail;
            model.RemoteIP = tcp.Remote.Addr;
            model.Password = "FFFFFFFF";
            model.SN = fcTrn.SN;
            model.RemotePort = tcp.Remote.Port;
            model.Key= connector.GetKey();
            sIP = tcp.Remote.Addr;


            if (fcTrn.CmdIndex < 4)
            {
                if (fcTrn.CmdIndex == 1)
                {

                    CardTransaction cardtrn = evn as CardTransaction;
                    DataGridViewRow dv = CreateRow();

                    dv.Cells[2].Value = evn.TransactionDate.ToString("yyyy-MM-dd HH:mm:ss");
                    dv.Cells[3].Value = cardtrn.UserCode;
                    dv.Cells[4].Value = evn.SerialNumber;
                    dv.Cells[5].Value = evn.TransactionCode;
                    dv.Cells[6].Value = sIP + "__" + fcTrn.SN;
                    DownRecordPhotos(evn.SerialNumber, model, dv);
                    Invoke(() =>
                    {
                        DGV_Msg.Rows.Insert(0, dv);
                    });
                }
            }


        }
        Dictionary<string, string> deviceKey = new Dictionary<string, string>();
        private void AddDevic(ConnectorModel model)
        {
            if (!deviceKey.ContainsKey(model.RemoteIP))
            {
                deviceKey.Add(model.RemoteIP, "");
                int index = ComBox_DeviceList.Items.Add(model);
                ComBox_DeviceList.SelectedIndex = index;
            }
        }
        private DataGridViewRow CreateRow()
        {
            DataGridViewRow dv = new DataGridViewRow();
            dv.Cells.Add(new DataGridViewImageCell());
            for (int i = 0; i < 6; i++)
            {
                dv.Cells.Add(new DataGridViewTextBoxCell());
            }
            return dv;
        }
        private void DownRecordPhotos(int SerialNumber, ConnectorModel connector, DataGridViewRow dv)
        {
            INCommandDetail cmdDtl = GetCommandDetail(connector);
            ReadFile_Parameter par = new ReadFile_Parameter(SerialNumber, 3, 1);
            INCommand cmd = new ReadFile(cmdDtl, par);
            mAllocator.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadFeatureCode_Result;
                //    var result = cmd.getResult() as ReadFeatureCode_Result;
                if (result.FileHandle == 0)
                {

                }
                if (result.Result)
                {
                    if (result.FileDatas != null)
                    {
                        Invoke(() =>
                        {
                            dv.Cells[0].Value = Image.FromStream(new System.IO.MemoryStream(result.FileDatas));
                        });
                    }
                    DownBodyTemperature(SerialNumber, connector, dv);
                }
            };
        }


        private void DownBodyTemperature(int SerialNumber, ConnectorModel connector, DataGridViewRow dv)
        {
            INCommandDetail cmdDtl = GetCommandDetail(connector);
            cmdDtl.Timeout = 2000;
            var par = new ReadTransactionDatabaseByIndex_Parameter(4, SerialNumber, 1);

            var cmd = new ReadTransactionDatabaseByIndex(cmdDtl, par);
            mAllocator.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as DoNetDrive.Protocol.Door.Door8800.Transaction.ReadTransactionDatabaseByIndex_Result;
                if (result.TransactionList.Count > 0)
                {
                    var body = result.TransactionList[0] as BodyTemperatureTransaction;
                    Invoke(() => { dv.Cells[1].Value = ((double)body.BodyTemperature) / ((double)10); });
                }
            };
        }

        private void Invoke(Action p)
        {
            try
            {
                Invoke((Delegate)p);
            }
            catch (Exception)
            {

                return;
            }

        }
        /// <summary>
        /// 获取一个命令详情，已经装配好通讯目标的所有信息
        /// </summary>
        /// <returns>命令详情</returns>
        public INCommandDetail GetCommandDetail(ConnectorModel connector, CommandDetailFactory.ConnectType connectType = CommandDetailFactory.ConnectType.TCPServerClient)
        {
            var protocolType = CommandDetailFactory.ControllerType.Door89H;

            var cmdDtl = CommandDetailFactory.CreateDetail(connectType, connector.Key, 0,
                protocolType, connector.SN, connector.Password);

            if (connectType == CommandDetailFactory.ConnectType.UDPClient)
            {
                var dtl = cmdDtl.Connector as TCPClientDetail;
                dtl.LocalAddr = mServerIP;
                dtl.LocalPort = mServerPort;
            }
            cmdDtl.Timeout = 600;
            cmdDtl.RestartCount = 3;
            return cmdDtl;

        }
        private void MAllocator_AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {
            //  AddCmdLog(e, "通讯Password错误 Communication Password Error");

        }

        private void mAllocator_CommandTimeout(object sender, CommandEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void mAllocator_CommandProcessEvent(object sender, CommandEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void mAllocator_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            // throw new NotImplementedException();
        }
        /// <summary>
        /// 获取连接通道详情 Get connection channel details
        /// </summary>
        /// <param name="conn">连接通道描述符 Connection channel descriptor</param>
        /// <param name="Local">返回描述本地信息 Returns the description local information</param>
        /// <param name="Remote">返回描述远程信息 Returns the description remote information</param>
        /// <returns></returns>
        private void GetConnectorDetail(INConnectorDetail conn, out string cType, out string Local, out string Remote)
        {
            Local = string.Empty;
            Remote = string.Empty;
            cType = string.Empty;

            var oConn = mAllocator.GetConnector(conn);
            if (oConn == null) return;

            IPDetail local = oConn.LocalAddress();
            conn = oConn.GetConnectorDetail();

            switch (conn.GetTypeName())
            {
                case ConnectorType.TCPClient:
                    var tcpclient = conn as TCPClientDetail;
                    cType = "TCP客户端   TCP Client";
                    Local = $"{local.ToString()}";
                    Remote = $"{tcpclient.Addr}:{tcpclient.Port}";
                    break;
                case ConnectorType.TCPServerClient:
                    cType = "TCP客户端节点   TCP Client Node ";
                    var tcpclientOnly = conn as TCPServerClientDetail;
                    Local = $"{local.ToString()}";
                    Remote = $"{tcpclientOnly.Remote.ToString()}";
                    break;
                case ConnectorType.UDPClient:
                    cType = "UDP客户端  UDP Client";
                    var udpOnly = conn as TCPClientDetail;
                    Local = $"{local.ToString()}";
                    Remote = $"{udpOnly.Addr}:{udpOnly.Port}";
                    break;
                case ConnectorType.UDPServer:
                    cType = "UDP服务器  UDP Server";
                    Local = $"{local.ToString()}";
                    break;
                case ConnectorType.TCPServer:
                    cType = "TCP服务器  TCP Server";
                    Local = $"{local.ToString()}";
                    break;
                //case ConnectorType.SerialPort:
                //    cType = "串口 Serial Port";
                //    var com = conn as SerialPortDetail;
                //    Local = $"COM{local.Port}:{com.Baudrate}";
                //    break;
                default:
                    cType = conn.GetTypeName();
                    Local = $"{conn.GetKey()}";
                    break;
            }
        }
        private bool mUDPIsBind = false;
        private string mServerIP;
        private int mServerPort;
        private void But_bind_Click(object sender, EventArgs e)
        {
            if (Nub_LocalPort.Value <= 0)
            {
                MsgErr("端口号不正确！Incorrect port number");
                return;
            }
            if (mTCPClients == null) mTCPClients = new ConcurrentDictionary<string, MyTCPServerClientDetail>();
            int port = (int)Nub_LocalPort.Value;
            string sLocalIP = ComBox_LocalIP.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(sLocalIP))
            {
                MsgErr("没有绑定本地IP！ No local IP is bound");
                return;
            }
            TCPServerDetail serverDetail = new TCPServerDetail(sLocalIP, port);


            if (mUDPIsBind)
            {
                mAllocator.CloseConnector(serverDetail);
                var sKeys = mTCPClients.Keys.ToArray();
                foreach (var key in sKeys)
                {
                    TCPServerClientDetail cdt = new TCPServerClientDetail(key);
                    mAllocator.CloseConnector(cdt);
                }
                mTCPClients.Clear();
                ComBox_DeviceList.Items.Clear();
                But_bind.Text = "开启服务 Enable service";
                mUDPIsBind = false;
                Nub_LocalPort.Enabled = true;
                ComBox_LocalIP.Enabled = true;
            }
            else
            {
                But_bind.Enabled = false;
                mUDPIsBind = true;
                Nub_LocalPort.Enabled = false;
                ComBox_LocalIP.Enabled = false;
                mAllocator.OpenConnector(serverDetail);

                //打开UDP服务器 Open the UDP server
                //   mAllocator.OpenConnector(serverDetail);
                mServerIP = sLocalIP;
                mServerPort = port;
                //等待后续事件，事件触发 mAllocator_ConnectorConnectedEvent 表示绑定成功 Waiting for subsequent events, event triggers mAllocator_ConnectorClosedEvent means binding successfully.
                //事件触发 mAllocator_ConnectorClosedEvent 表示绑定失败   Event triggers mAllocator_ConnectorClosedEvent means binding failed


            }
        }
        public void MsgErr(string sText)
        {
            MessageBox.Show(sText, "错误 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// 初始化本机IP ：Initialize local IP
        /// </summary>
        private void IniLoadLocalIP()
        {
            ComBox_LocalIP.Items.Clear();

            IPHostEntry localentry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress oItem in localentry.AddressList)
            {
                IPAddress ip = oItem;

                if (ip.IsIPv4MappedToIPv6)
                {
                    ip = ip.MapToIPv4();
                }
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ComBox_LocalIP.Items.Add(ip.ToString());
                }
            }

            if (ComBox_LocalIP.Items.Count > 0)
            {
                ComBox_LocalIP.SelectedIndex = ComBox_LocalIP.Items.Count - 1;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IniLoadLocalIP();
            IniForm();

            ComBox_DeviceList.DisplayMember = "RemoteIP";
            But_bind_Click(null, null);
        }

        private void But_Set_Click(object sender, EventArgs e)
        {
            //ConnectorModel connector = new ConnectorModel();
            //connector.RemoteIP = Txt_DeviceIP.Text.Trim();
            //connector.RemotePort = (int)Nub_DevicePort.Value;
            //connector.SN = Txt_DeviceSN.Text.Trim();
            //connector.Password = Txt_DevicePwd.Text.Trim();

            //var cmdDtl = GetCommandDetail(connector,CommandDetailFactory.ConnectType.UDPClient);
            //ReadTCPSetting cmd = new ReadTCPSetting(cmdDtl);
            //mAllocator.AddCommand(cmd);
            //cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            //{
            //    ReadTCPSetting_Result result = cmde.Command.getResult() as ReadTCPSetting_Result;
            //    UpdateSeverInfo(result.TCP);
            //};
        }

        private void UpdateSeverInfo(TCPDetail tcpDetail)
        {
            ConnectorModel connector = new ConnectorModel();
            connector.RemoteIP = Txt_DeviceIP.Text.Trim();
            connector.RemotePort = (int)Nub_DevicePort.Value;
            connector.SN = Txt_DeviceSN.Text.Trim();
            connector.Password = Txt_DevicePwd.Text.Trim();
            var cmdDtl = GetCommandDetail(connector);
            tcpDetail.mServerIP = Txt_ServerIP.Text.Trim();
            tcpDetail.mServerPort = (int)Nub_LocalPort.Value;
            tcpDetail.mServerAddr = "www.pc15.net";
            tcpDetail.mTCPPort = 1;




            WriteTCPSetting cmd = new WriteTCPSetting(cmdDtl, new WriteTCPSetting_Parameter(tcpDetail));
            mAllocator.AddCommand(cmd);
        }


    }
}
