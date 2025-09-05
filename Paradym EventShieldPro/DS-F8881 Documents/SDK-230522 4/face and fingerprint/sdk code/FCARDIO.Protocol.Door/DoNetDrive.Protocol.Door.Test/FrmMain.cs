using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Door.Door8800.Door.ReaderOption;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.Watch;
using DoNetDrive.Protocol.Door.Test.Language;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core;
using DoNetDrive.Core.Connector.TCPClient;
using DoNetDrive.Core.Connector.TCPServer.Client;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Core.Connector.TCPServer;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using DoNetDrive.Core.Factory;
using DoNetDrive.Connector.COM;

namespace DoNetDrive.Protocol.Door.Test
{
    public partial class frmMain : Form, INMain
    {
        ConnectorAllocator mAllocator;
        ConnectorObserverHandler mObserver;
        private static HashSet<frmNodeForm> NodeForms;
        private static HashSet<string> Door89HSNTable;
        private static string[] TransactionTypeName;


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

        static frmMain()
        {
            NodeForms = new HashSet<frmNodeForm>();


            Door89HSNTable = new HashSet<string>();
            Door89HSNTable.Add("FC-8910H");
            Door89HSNTable.Add("FC-8920H");
            Door89HSNTable.Add("FC-8940H");
            Door89HSNTable.Add("MC-5912T");
            Door89HSNTable.Add("MC-5912T");
            Door89HSNTable.Add("MC-5924T");
            Door89HSNTable.Add("MC-5948T");
            Door89HSNTable.Add("MC-5926T");



        }

        public static void AddNodeForms(frmNodeForm frm)
        {
            if (!NodeForms.Contains(frm))
            {
                NodeForms.Add(frm);
            }
        }


        bool _IsClosed;

        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {


            _IsClosed = false;
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(100);
                Invoke(new Action(IniForm));

            });
            tbEvent.SelectedIndex = 1;
        }



        /// <summary>
        /// 窗体初始化
        /// </summary>
        public void IniForm()
        {
            if (_IsClosed) return;

            mAllocator = ConnectorAllocator.GetAllocator();
            mObserver = new ConnectorObserverHandler();


            //导入 串口通讯库
            var defFactory = mAllocator.ConnectorFactory as DefaultConnectorFactory;
            defFactory.ConnectorFactoryDictionary.Add(ConnectorType.SerialPort, DoNetDrive.Connector.COM.SerialPortFactory.GetInstance());



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

            IniConnTypeList();
            // IniLstIO();
            //   InilstCommand();

            Task.Run((Action)ShowCommandProcesslog);
        }
        #region 通道事件
        /// <summary>
        /// 客户端离线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MAllocator_ClientOffline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            inc.AddRequestHandle(mObserver);
            switch (inc.GetConnectorType())
            {
                case ConnectorType.TCPServerClient://TCP客户端已连接
                    RemoveTCPServer_Client(inc.GetConnectorDetail());
                    break;
                case ConnectorType.UDPClient://UDP客户端已连接
                    //RemoveUDPClient(inc.GetConnectorDetail());
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        /// 客户端上线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MAllocator_ClientOnline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            inc.AddRequestHandle(mObserver);
            switch (inc.GetConnectorType())
            {
                case ConnectorType.TCPServerClient://TCP客户端已连接
                    AddTCPServer_Client(inc.GetConnectorDetail());
                    break;
                case ConnectorType.UDPClient://UDP客户端已连接
                    //AddUDPClient(inc.GetConnectorDetail());
                    break;
                default:
                    break;
            }
        }

        private void MObserver_DisposeResponseEvent(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), GetLanguage("Msg1"), msg);
        }


        private void MObserver_DisposeRequestEvent(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), GetLanguage("Msg2"), msg);
        }

        private void mAllocator_ConnectorErrorEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(false));
                    AddIOLog(connector, GetLanguage("Msg3"), GetLanguage("Msg4"));
                    break;
                case ConnectorType.TCPServer://TCP Server 服务器
                    Invoke(() => TCPServerBindOver(false));
                    AddIOLog(connector, GetLanguage("Msg5"), GetLanguage("Msg6"));
                    break;
                default:
                    if (connector.IsFaulted)
                    {
                        //AddLog(connector.GetError().Message);
                        Console.WriteLine(connector.GetError().Message);
                    }
                    AddIOLog(connector, GetLanguage("Msg7"), GetLanguage("Msg8"));
                    break;
            }
        }

        private void mAllocator_ConnectorClosedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(false));
                    AddIOLog(connector, GetLanguage("Msg3"), GetLanguage("Msg9"));
                    break;
                case ConnectorType.TCPServer://TCP Server 服务器
                    Invoke(() => TCPServerBindOver(false));
                    AddIOLog(connector, GetLanguage("Msg5"), GetLanguage("Msg10"));
                    break;
                default:
                    AddIOLog(connector, GetLanguage("Msg11"), GetLanguage("Msg12"));
                    break;
            }
        }

        private void mAllocator_ConnectorConnectedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(true));
                    AddIOLog(connector, GetLanguage("Msg3"), GetLanguage("Msg13"));
                    break;
                case ConnectorType.TCPServer://TCP Server 服务器
                    Invoke(() => TCPServerBindOver(true));
                    AddIOLog(connector, GetLanguage("Msg5"), GetLanguage("Msg14"));
                    break;
                default:
                    mAllocator.GetConnector(connector).AddRequestHandle(mObserver);

                    AddIOLog(connector, GetLanguage("Msg15"), GetLanguage("Msg16"));
                    break;
            }
        }

        #endregion

        #region 命令事件
        private void MAllocator_AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {
            AddCmdLog(e, GetLanguage("Msg17"));
        }

        private void mAllocator_CommandTimeout(object sender, CommandEventArgs e)
        {
            if (e.Command.GetType().FullName == typeof(Door8800.SystemParameter.SearchControltor.SearchControltor).FullName)
            {
                AddCmdLog(e, GetLanguage("Msg18"));
                return;
            }
            AddCmdLog(e, GetLanguage("Msg19"));
        }


        private void mAllocator_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            StringBuilder sLogBuf = new StringBuilder();
            if (e.Command is AbstractCommand)
            {
                
                AbstractCommand abscmd = e.Command as AbstractCommand;
                if(abscmd.GetStatus().IsCanceled)
                {
                    sLogBuf.Append("，已被用户取消");
                }
                if (abscmd.CommandException != null)
                {
                    ShowException(sLogBuf, abscmd.CommandException);

                    Console.WriteLine(sLogBuf.ToString());
                }

            }
            AddCmdLog(e, GetLanguage("Msg20") + sLogBuf.ToString());
        }

        private void ShowException(StringBuilder sLogBuf,Exception exception)
        {
            sLogBuf.Append("Error:").Append(exception.Message);
            if (exception.InnerException == null) return;
            sLogBuf.Append("InnerException:").AppendLine();
            ShowException(sLogBuf, exception.InnerException);
        }


        private const string Command_ReadSN = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.ReadSN";
        private const string Command_WriteSN = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.WriteSN";
        private const string Command_WriteSN_Broadcast = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.WriteSN_Broadcast";
        private const string Command_ReadConnectPassword = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.ReadConnectPassword";
        private const string Command_WriteConnectPassword = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.WriteConnectPassword";
        private const string Command_ResetConnectPassword = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.ResetConnectPassword";

        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            if (e.Command.GetType().FullName == typeof(Door8800.SystemParameter.SearchControltor.SearchControltor).FullName)
            {
                return;
            }
            mAllocator_CommandProcessEvent(sender, e);
            AddCmdLog(e, GetLanguage("Msg21"));
            string cName = e.Command.GetType().FullName;

            switch (cName)
            {
                case Command_ReadSN://读SN
                    Door8800.SystemParameter.SN.SN_Result sn = e.Command.getResult() as Door8800.SystemParameter.SN.SN_Result;
                    Invoke(() => txtSN.Text = sn.SNBuf.GetString());
                    break;
                case Command_WriteSN://写SN
                case Command_WriteSN_Broadcast:
                    Door8800.SystemParameter.SN.SN_Parameter snPar = e.Command.Parameter as Door8800.SystemParameter.SN.SN_Parameter;
                    Invoke(() => txtSN.Text = snPar.SNBuf.GetString());
                    break;
                case Command_ReadConnectPassword://读通讯密码
                    Door8800.SystemParameter.ConnectPassword.Password_Result pwd = e.Command.getResult() as Door8800.SystemParameter.ConnectPassword.Password_Result;
                    Invoke(() => txtPassword.Text = pwd.Password);
                    break;
                case Command_WriteConnectPassword://写通讯密码
                    Door8800.SystemParameter.ConnectPassword.Password_Parameter pwdPar = e.Command.Parameter as Door8800.SystemParameter.ConnectPassword.Password_Parameter;
                    Invoke(() => txtPassword.Text = pwdPar.Password);
                    break;
                case Command_ResetConnectPassword://复位通讯密码
                    Invoke(() => txtPassword.Text = "FFFFFFFF");
                    break;
                default:
                    break;
            }




        }
        #endregion

        #region 命令进度事件
        /// <summary>
        /// 命令日志
        /// </summary>
        private string mCommandProcessLog;
        /// <summary>
        /// 显示命令进度日志
        /// </summary>
        private void ShowCommandProcesslog()
        {
            do
            {
                if (_IsClosed) break;

                Invoke(() =>
                {
                    if (_IsClosed) return;
                    txtProcess.Text = mCommandProcessLog;

                });
                Sleep(300);
                if (_IsClosed) break;
            } while (!_IsClosed);
            // Console.WriteLine("ShowCommandProcesslog 已退出");
        }


        private void mAllocator_CommandProcessEvent(object sender, CommandEventArgs e)
        {
            var cmd = e.Command;
            string sName = cmd.GetType().FullName;
            if (mCommandClasss.ContainsKey(sName)) sName = mCommandClasss[sName];

            double time = 0;
            var dtl = e.CommandDetail;
            if (dtl.BeginTime != DateTime.MinValue && dtl.EndTime != DateTime.MinValue)
            {
                time = (dtl.EndTime - dtl.BeginTime).TotalMilliseconds;
            }
            else
            {
                if (dtl.BeginTime != DateTime.MinValue)
                    time = (DateTime.Now - e.CommandDetail.BeginTime).TotalMilliseconds;
            }


            string sLog = $"{sName}{GetLanguage("Msg22")}{time:0},{GetLanguage("Msg23")}{cmd.getProcessStep()} / {cmd.getProcessMax()}";
            mCommandProcessLog = sLog;
        }



        #endregion

        #region 获取通道描述信息
        private string GetConnectorDetail(INConnector conn)
        {
            return GetConnectorDetail(conn.GetConnectorDetail());
        }
        private string GetConnectorDetail(INConnectorDetail conn)
        {
            string Local, Remote, cType;
            GetConnectorDetail(conn, out cType, out Local, out Remote);
            string ret = $"{GetLanguage("Msg24")}{cType} {GetLanguage("Msg25")}{Local} ,{GetLanguage("Msg26")}{Remote}";

            switch (conn.GetTypeName())
            {
                case ConnectorType.UDPServer:
                    ret = $"{GetLanguage("Msg24")}{cType} {GetLanguage("Msg27")}{Local}";
                    break;
                case ConnectorType.TCPServer:
                    ret = $"{GetLanguage("Msg24")}{cType} {GetLanguage("Msg27")}{Local}";
                    break;
                case ConnectorType.SerialPort:
                    ret = $"{GetLanguage("Msg24")}{cType} {Local}";
                    break;
                default:
                    ret = $"{GetLanguage("Msg24")}{cType} {Local}";
                    break;
            }

            return $"{ret}:{DateTime.Now.ToTimeffff()}";

        }

        /// <summary>
        /// 获取连接通道详情
        /// </summary>
        /// <param name="conn">连接通道描述符</param>
        /// <param name="Local">返回描述本地信息</param>
        /// <param name="Remote">返回描述远程信息</param>
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
                    cType = GetLanguage("Msg28");
                    Local = $"{local.ToString()}";
                    Remote = $"{tcpclient.Addr}:{tcpclient.Port}";
                    break;
                case ConnectorType.TCPServerClient:
                    cType = GetLanguage("Msg29");
                    var tcpclientOnly = conn as TCPServerClientDetail;
                    Local = $"{local.ToString()}";
                    Remote = $"{oConn.RemoteAddress().ToString()}";
                    break;
                case ConnectorType.UDPClient:
                    cType = GetLanguage("Msg30");
                    var udpOnly = conn as TCPClientDetail;
                    Local = $"{local.ToString()}";
                    Remote = $"{udpOnly.Addr}:{udpOnly.Port}";
                    break;
                case ConnectorType.UDPServer:
                    cType = GetLanguage("Msg31");
                    Local = $"{local.ToString()}";
                    break;
                case ConnectorType.TCPServer:
                    cType = GetLanguage("Msg32");
                    Local = $"{local.ToString()}";
                    break;
                case ConnectorType.SerialPort:
                    cType = GetLanguage("Msg33");
                    var com = conn as DoNetDrive.Connector.COM.SerialPortDetail;
                    Local = $"COM{local.Port}:{com.Baudrate}";
                    break;
                default:
                    cType = conn.GetTypeName();
                    Local = $"{conn.GetKey()}";
                    break;
            }
        }
        #endregion

        #region 窗体关闭
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _IsClosed = true;
            Sleep(500);
            foreach (var frm in NodeForms)
            {
                frm.Dispose();
            }
            NodeForms.Clear();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //释放资源
            mAllocator.Dispose();
            Sleep(500);
            //Sleep(50000);
        }
        #endregion


        #region 公共函数
        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="s">需要显示的日志</param>
        public void AddLog(string s)
        {
            if (_IsClosed) return;
            if (lstCommand.InvokeRequired)
            {
                lstCommand.BeginInvoke((Action<string>)AddLog, s);
                return;
            }
            AddCmdLog(null, s);
            //string log = txtLog.Text;
            //if (log.Length > 20000)
            //{
            //    log = log.Substring(0, 10000);
            //}
            //txtLog.Text = s + "\r\n" + log;
        }



        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="s">需要显示的日志</param>
        public void AddLog(StringBuilder s)
        {
            if (_IsClosed) return;

            AddLog(s.ToString());
        }

        /// <summary>
        /// 将命令加入到分配器开始执行
        /// </summary>
        /// <param name="cmd"></param>
        public void AddCommand(INCommand cmd)
        {
            if (cmd.CommandDetail == null) return;
            mAllocator.AddCommand(cmd);
        }


        public INCommandDetail GetCommandDetail()
        {
            switch (cmdConnType.SelectedIndex)
            {
                case 1://TCP 客户端方式通讯
                    return GetCommandDetail(txtTCPClientAddr.Text);
                case 2://UDP 
                    return GetCommandDetail(txtUDPAddr.Text);
                default:
                    return GetCommandDetail(string.Empty);
            }

        }

        private class OnlineAccessCommandDetailEx : OnlineAccess.OnlineAccessCommandDetail
        {

            private static byte[] EmptySN_FF = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                                                            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF};
            public const string SNTitle = "ffffffffffffffff";

            public OnlineAccessCommandDetailEx(INConnectorDetail cnt, string s, string p) : base(cnt, s, p)
            {

            }

            public override byte[] SNByte
            {
                get
                {
                    if (base.mSN.Equals("ffffffffffffffff"))
                    {
                        return EmptySN_FF;
                    }
                    return base.SNByte;
                }
            }
        }

        /// <summary>
        /// 获取一个命令详情，已经装配好通讯目标的所有信息
        /// </summary>
        /// <returns>命令详情</returns>
        public INCommandDetail GetCommandDetail(string IPAddr)
        {
            if (_IsClosed) return null;
            CommandDetailFactory.ConnectType connectType = CommandDetailFactory.ConnectType.TCPClient;
            CommandDetailFactory.ControllerType protocolType = CommandDetailFactory.ControllerType.Door88;
            string addr = string.Empty, sn, password;
            int iRemotePort = 0;

            protocolType = mProtocolTypeTable[cmdProtocolType.SelectedIndex];

            sn = txtSN.Text;
            if (string.IsNullOrEmpty(sn))
            {
                sn = "0000000000000000";
            }
            if (sn.GetBytes().Length != 16)
            {
                sn = "0000000000000000";
            }

            password = txtPassword.Text;
            if (!password.IsHex())
            {
                password = Door8800Command.NULLPassword;
            }
            if (password.Length != 8)
            {
                password = Door8800Command.NULLPassword;
            }


            switch (cmdConnType.SelectedIndex)//串口,TCP客户端,UDP,TCP服务器
            {
                case 0://串口
                    if (cmbSerialPort.SelectedIndex == -1)
                    {
                        MsgTip(GetLanguage("Msg35"));
                        return null;
                    }
                    
                    addr = string.Empty;
                    iRemotePort = cmbSerialPort.Text.Substring(3).ToInt32();
                    
                    return GetCommandDetail(new SerialPortDetail((byte)iRemotePort), protocolType, sn, password);
                case 1://TCP 客户端方式通讯
                    connectType = CommandDetailFactory.ConnectType.TCPClient;
                    addr = IPAddr;// txtTCPClientAddr.Text;
                    if (!int.TryParse(txtTCPClientPort.Text, out iRemotePort))
                    {
                        iRemotePort = 8000;
                    }
                    break;
                case 2://UDP 
                    if (!mUDPIsBind)
                    {
                        MsgErr(GetLanguage("Msg36"));
                        return null;
                    }
                    connectType = CommandDetailFactory.ConnectType.UDPClient;
                    addr = IPAddr;// txtUDPAddr.Text;
                    if (!int.TryParse(txtUDPPort.Text, out iRemotePort))
                    {
                        iRemotePort = 8000;
                    }
                    break;
                case 3://TCP服务器
                    if (!mTCPServerBind)
                    {
                        MsgErr(GetLanguage("Msg37"));
                        return null;
                    }

                    connectType = CommandDetailFactory.ConnectType.TCPServerClient;
                    if (cmbTCPClient.SelectedItem == null)
                    {
                        MsgErr(GetLanguage("Msg38"));
                        return null;
                    }
                    TCPServerClientDetail_Item oItem = cmbTCPClient.SelectedItem as TCPServerClientDetail_Item;

                    addr = oItem.Key;
                    break;
                default:
                    break;
            }
            


            
            return GetCommandDetail(connectType,protocolType,addr,iRemotePort,sn,password);

        }

        public INCommandDetail GetCommandDetail(CommandDetailFactory.ConnectType connectType, 
            CommandDetailFactory.ControllerType protocolType, string sRemoteIP, int iRemotePort, 
            string sn,string password)
        {

            INCommandDetail cmdDtl = CommandDetailFactory.CreateDetail(connectType, sRemoteIP, iRemotePort,
                protocolType, sn, password);

            if (sn.Equals(OnlineAccessCommandDetailEx.SNTitle))
            {
                cmdDtl = new OnlineAccessCommandDetailEx(cmdDtl.Connector, sn, password);
            }



            if (connectType == CommandDetailFactory.ConnectType.UDPClient)
            {
                UDPClientDetail dtl = cmdDtl.Connector as UDPClientDetail;
                dtl.LocalAddr = cmbLocalIP.Text;
                dtl.LocalPort = txtUDPLocalPort.Text.ToInt32();
            }
            cmdDtl.Timeout = 600;
            cmdDtl.RestartCount = 3;

            return cmdDtl;
        }

        public INCommandDetail GetCommandDetail(INConnectorDetail connectdtl,
           CommandDetailFactory.ControllerType protocolType, 
           string sn, string password)
        {

            INCommandDetail cmdDtl = CommandDetailFactory.CreateDetail(connectdtl,
                protocolType, sn, password);

            cmdDtl.Timeout = 1500;
            cmdDtl.RestartCount = 3;

            return cmdDtl;
        }
        #endregion


        /// <summary>
        /// 获取协议类型
        /// </summary>
        /// <returns>控制器型号</returns>
        public CommandDetailFactory.ControllerType GetProtocolType()
        {
            return (CommandDetailFactory.ControllerType)cmdProtocolType.SelectedItem;
        }

        public bool CheckProtocolTypeIs89H()
        {
            var eType = GetProtocolType();
            return eType == CommandDetailFactory.ControllerType.Door89H ||
                 eType == CommandDetailFactory.ControllerType.Door59 ||
                 eType == CommandDetailFactory.ControllerType.Door5926T;
        }


        #region 通讯日志
        private bool mShowIOEvent = true;
        private void chkShowIO_CheckedChanged(object sender, EventArgs e)
        {
            mShowIOEvent = chkShowIO.Checked;
        }


        /// <summary>
        /// 初始化通讯日志列表
        /// </summary>
        private void IniLstIO()
        {
            lstIO.BeginUpdate();

            var cols = lstIO.Columns;
            cols.Clear();
            var sCaptions = GetLanguage("lstIO").SplitTrim(",");
            var iWidths = new int[] { 60, 260, 90, 125, 125, 100 };
            for (int i = 0; i < sCaptions.Length; i++)
            {
                ColumnHeader col = new ColumnHeader();
                col.Text = sCaptions[i];
                col.TextAlign = HorizontalAlignment.Center;
                col.Width = iWidths[i];
                cols.Add(col);
            }
            lstIO.HideSelection = true;
            lstIO.LabelEdit = false;
            lstIO.MultiSelect = false;
            lstIO.FullRowSelect = true;
            lstIO.GridLines = true;
            lstIO.ShowItemToolTips = true;
            lstIO.EndUpdate();

            mIOItems = new ConcurrentQueue<ListViewItem>();
            Task.Run(() =>
            {
                do
                {
                    if (_IsClosed) break;
                    if (!mIOItems.IsEmpty)
                    {
                        Invoke(() =>
                        {
                            lstIO.BeginUpdate();

                            do
                            {
                                ListViewItem oItem;
                                if (mIOItems.TryDequeue(out oItem))
                                {
                                    lstIO.Items.Add(oItem);
                                }
                            } while (!mIOItems.IsEmpty);

                            lstIO.EndUpdate();
                        });

                    }
                    Sleep(1000);
                    if (_IsClosed) break;
                } while (!_IsClosed);

                //Console.WriteLine("IniLstIO 刷新线程 已退出");
            });
        }

        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        private ConcurrentQueue<ListViewItem> mIOItems;

        /// <summary>
        /// 添加一个通讯日志
        /// </summary>
        /// <param name="connDetail"></param>
        /// <param name="sTag">标签</param>
        /// <param name="txt">内容</param>
        public void AddIOLog(INConnectorDetail connDetail, string sTag, string txt)
        {
            if (!mShowIOEvent) return;

            string Local, Remote, cType;
            GetConnectorDetail(connDetail, out cType, out Local, out Remote);

            ListViewItem oItem = new ListViewItem();
            oItem.Text = sTag;
            oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, txt));
            oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, cType));
            oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, Remote));
            oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, Local));
            oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, DateTime.Now.ToTimeffff()));
            oItem.ToolTipText = txt;
            mIOItems.Enqueue(oItem);
        }


        /// <summary>
        /// 清空所有通讯日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butClear_Click(object sender, EventArgs e)
        {

            lstIO.Items.Clear();
        }
        #endregion

        #region 功能菜单

        private void butSystem_Click(object sender, EventArgs e)
        {
            frmSystem frm = frmSystem.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butTime_Click(object sender, EventArgs e)
        {
            frmTime frm = frmTime.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butDoor_Click(object sender, EventArgs e)
        {
            frmDoor frm = frmDoor.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);

        }

        private void butHoliday_Click(object sender, EventArgs e)
        {

            frmHoliday frm = frmHoliday.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void ButPassword_Click(object sender, EventArgs e)
        {
            frmPassword frm = frmPassword.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void ButTimeGroup_Click(object sender, EventArgs e)
        {
            frmTimeGroup frm = frmTimeGroup.GetForm(this);

            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butCard_Click(object sender, EventArgs e)
        {
            frmCard frm = frmCard.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);


        }
        /// <summary>
        /// 显示窗口在侧边栏
        /// </summary>
        /// <param name="chi"></param>
        private void ShowFrm(Form chi)
        {
            Screen scr = Screen.PrimaryScreen;

            foreach (Screen ss in Screen.AllScreens)
            {
                var rc = ss.Bounds;
                if (rc.Left < this.Left && (rc.Left + rc.Width) > this.Left)
                {
                    if (rc.Top < this.Top && (rc.Top + rc.Bottom) > this.Top)
                    {
                        scr = ss;
                        break;
                    }
                }

            }

            var scrRc = scr.Bounds;
            int iLeft = (scrRc.Width - (Width + chi.Width)) / 2;
            int iTop = (scrRc.Height - (Height > chi.Height ? Height : chi.Height)) / 2;

            this.Left = scrRc.Left + iLeft;
            this.Top = scrRc.Top + iTop;

            chi.Left = scrRc.Left + iLeft + this.Width;
            chi.Top = scrRc.Top + iTop;


        }

        private void butRecord_Click(object sender, EventArgs e)
        {
            frmRecord frm = frmRecord.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butUploadSoftware_Click(object sender, EventArgs e)
        {
            frmUploadSoftware frm = frmUploadSoftware.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }
        #endregion

        /// <summary>
        /// 保存命令类型的功能名称
        /// </summary>
        private Dictionary<string, string> mCommandClasss;

        /// <summary>
        /// 协议类型
        /// </summary>
        CommandDetailFactory.ControllerType[] mProtocolTypeTable = new CommandDetailFactory.ControllerType[4];
        /// <summary>
        /// 初始化命令类型的功能名称
        /// </summary>
        private void IniCommandClassNameList()
        {
            mCommandClasss = new Dictionary<string, string>();

            mCommandClasss.Add(typeof(Door8800.SystemParameter.SN.ReadSN).FullName, "读取SN");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.SN.WriteSN).FullName, "写SN");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.SN.WriteSN_Broadcast).FullName, "广播写SN");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.ConnectPassword.ReadConnectPassword).FullName, "获取通讯密码");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.ConnectPassword.WriteConnectPassword).FullName, "设置通讯密码");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.ConnectPassword.ResetConnectPassword).FullName, "重置通讯密码");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.TCPSetting.ReadTCPSetting).FullName, "读取TCP参数");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.TCPSetting.WriteTCPSetting).FullName, "写入TCP参数");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.Deadline.ReadDeadline).FullName, "读取设备有效期");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.Deadline.WriteDeadline).FullName, "写入设备有效期");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.Version.ReadVersion).FullName, "读取设备版本号");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.SystemStatus.ReadSystemStatus).FullName, "读取设备运行信息");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadRecordMode).FullName, "读取记录存储方式");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteRecordMode).FullName, "写入记录存储方式");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadKeyboard).FullName, "读取键盘开关");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteKeyboard).FullName, "设置键盘开关");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadLockInteraction).FullName, "读取互锁参数");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteLockInteraction).FullName, "设置互锁参数");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadFireAlarmOption).FullName, "读取消防报警参数");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteFireAlarmOption).FullName, "设置消防报警参数");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadOpenAlarmOption).FullName, "读取匪警报警参数");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteOpenAlarmOption).FullName, "设置匪警报警参数");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadReaderIntervalTime).FullName, "读取读卡间隔时间");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteReaderIntervalTime).FullName, "设置读卡间隔时间");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadBroadcast).FullName, "读取语音段开关");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteBroadcast).FullName, "设置语音段开关");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadReaderCheckMode).FullName, "读取读卡器校验");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteReaderCheckMode).FullName, "设置读卡器校验");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadBuzzer).FullName, "读取主板蜂鸣器");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteBuzzer).FullName, "设置主板蜂鸣器");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadSmogAlarmOption).FullName, "读取烟雾报警参数");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteSmogAlarmOption).FullName, "设置烟雾报警参数");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadEnterDoorLimit).FullName, "读取门内人数限制");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteEnterDoorLimit).FullName, "设置门内人数限制");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadTheftAlarmSetting).FullName, "读取智能防盗主机参数");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteTheftAlarmSetting).FullName, "设置智能防盗主机参数");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadCheckInOut).FullName, "读取防潜回模式");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteCheckInOut).FullName, "设置防潜回模式");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadCardPeriodSpeak).FullName, "读取卡片到期提示");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteCardPeriodSpeak).FullName, "设置卡片到期提示");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.ReadReadCardSpeak).FullName, "读取定时读卡播报语音消息参数");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FunctionParameter.WriteReadCardSpeak).FullName, "设置定时读卡播报语音消息参数");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.Watch.ReadWatchState).FullName, "读取实时监控状态");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.Watch.BeginWatch).FullName, "开启实时监控");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.Watch.CloseWatch).FullName, "关闭实时监控");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.Watch.BeginWatch_Broadcast).FullName, "开启实时监控_广播");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.Watch.CloseWatch_Broadcast).FullName, "关闭实时监控_广播");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.FireAlarm.ReadFireAlarmState).FullName, "读取消防报警状态");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FireAlarm.SendFireAlarm).FullName, "消防报警通知");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.FireAlarm.CloseFireAlarm).FullName, "解除消防报警");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.SmogAlarm.ReadSmogAlarmState).FullName, "读取烟雾报警状态");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.SmogAlarm.SendSmogAlarm).FullName, "烟雾报警通知");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.SmogAlarm.CloseSmogAlarm).FullName, "解除烟雾报警");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.Alarm.CloseAlarm).FullName, "解除报警");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.WorkStatus.ReadWorkStatus).FullName, "获取设备状态信息");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.WorkStatus.ReadTheftAlarmState).FullName, "获取防盗主机布防状态信息");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.Controller.FormatController).FullName, "初始化数据");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.SearchControltor.SearchControltor).FullName, "自动搜索设备");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.SearchControltor.WriteControltorNetCode).FullName, "修改设备的网络代码");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.CacheContent.ReadCacheContent).FullName, "读取缓存区内容");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.CacheContent.WriteCacheContent).FullName, "设置缓存区内容");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.KeepAliveInterval.ReadKeepAliveInterval).FullName, "读取保活间隔时间");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.KeepAliveInterval.WriteKeepAliveInterval).FullName, "设置保活间隔时间");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.TheftFortify.SetTheftFortify).FullName, "防盗报警布防");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.TheftFortify.SetTheftDisarming).FullName, "防盗报警撤防");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.BalcklistAlarmOption.ReadBalcklistAlarmOption).FullName, "读取黑名单报警参数");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.BalcklistAlarmOption.WriteBalcklistAlarmOption).FullName, "设置黑名单报警功能");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.ExploreLockMode.ReadExploreLockMode).FullName, "读取防探测功能开关参数");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.ExploreLockMode.WriteExploreLockMode).FullName, "设置防探测功能开关");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.Check485Line.ReadCheck485Line).FullName, "读取485线路反接检测开关");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.Check485Line.WriteCheck485Line).FullName, "设置485线路反接检测开关");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.TCPClient.ReadTCPClientList).FullName, "读取TCP客户端列表");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.TCPClient.StopTCPClientConnection).FullName, "停止TCP客户端连接");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.TCPClient.StopAllTCPClientConnection).FullName, "停止所有客户端连接");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.CardDeadlineTipDay.ReadCardDeadlineTipDay).FullName, "读取有效期提醒阀值");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.CardDeadlineTipDay.WriteCardDeadlineTipDay).FullName, "设置有效期提醒阀值");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.ControlPanelTamperAlarm.ReadControlPanelTamperAlarm).FullName, "读取控制板防拆报警功能开关");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.ControlPanelTamperAlarm.WriteControlPanelTamperAlarm).FullName, "设置控制板防拆报警功能开关");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.HTTPPageLandingSwitch.ReadHTTPPageLandingSwitch).FullName, "读取HTTP网页登陆开关");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.HTTPPageLandingSwitch.WriteHTTPPageLandingSwitch).FullName, "设置HTTP网页登陆开关");

            mCommandClasss.Add(typeof(Door8800.SystemParameter.LawfulCardReleaseAlarmSwitch.ReadLawfulCardReleaseAlarmSwitch).FullName, "读取合法卡解除报警开关");
            mCommandClasss.Add(typeof(Door8800.SystemParameter.LawfulCardReleaseAlarmSwitch.WriteLawfulCardReleaseAlarmSwitch).FullName, "设置合法卡解除报警开关");

            mCommandClasss.Add(typeof(Door8800.Time.ReadTime).FullName, "读系统时间");
            mCommandClasss.Add(typeof(Door8800.Time.WriteTime).FullName, "写系统时间");
            mCommandClasss.Add(typeof(Door8800.Time.WriteCustomTime).FullName, "写系统时间");
            mCommandClasss.Add(typeof(Door8800.Time.WriteTimeBroadcast).FullName, "写设备时间_广播命令");

            mCommandClasss.Add(typeof(Door8800.Time.TimeErrorCorrection.ReadTimeError).FullName, "读取误差自修正参数");
            mCommandClasss.Add(typeof(Door8800.Time.TimeErrorCorrection.WriteTimeError).FullName, "写入误差自修正参数");

            mCommandClasss.Add(typeof(Door8800.Door.ReaderOption.ReadReaderOption).FullName, "读取读卡器字节数");
            mCommandClasss.Add(typeof(Door8800.Door.ReaderOption.WriteReaderOption<ReaderOption_Parameter>).FullName, "写入读卡器字节数");

            mCommandClasss.Add(typeof(Door8800.Door.RelayOption.ReadRelayOption).FullName, "读取继电器参数");
            mCommandClasss.Add(typeof(Door8800.Door.RelayOption.WriteRelayOption).FullName, "写入继电器参数");

            mCommandClasss.Add(typeof(Door8800.Door.Remote.OpenDoor).FullName, "远程开门");
            mCommandClasss.Add(typeof(Door8800.Door.Remote.CloseDoor).FullName, "远程关门");
            mCommandClasss.Add(typeof(Door8800.Door.Remote.HoldDoor).FullName, "设置门常开");
            mCommandClasss.Add(typeof(Door8800.Door.Remote.OpenDoor_CheckNum).FullName, "远程开门_验证");
            mCommandClasss.Add(typeof(Door8800.Door.Remote.LockDoor).FullName, "锁定门");
            mCommandClasss.Add(typeof(Door8800.Door.Remote.UnlockDoor).FullName, "解除锁定门");

            mCommandClasss.Add(typeof(Door8800.Door.ReaderWorkSetting.ReadReaderWorkSetting).FullName, "读取门认证方式");
            mCommandClasss.Add(typeof(Door8800.Door.ReaderWorkSetting.WriteReaderWorkSetting).FullName, "设置门认证方式");

            mCommandClasss.Add(typeof(Door8800.Door.DoorWorkSetting.ReadDoorWorkSetting).FullName, "读取门工作方式");
            mCommandClasss.Add(typeof(Door8800.Door.DoorWorkSetting.WriteDoorWorkSetting).FullName, "设置门工作方式");

            mCommandClasss.Add(typeof(Door8800.Door.RelayReleaseTime.ReadRelayReleaseTime).FullName, "读取开锁时输出时长");
            mCommandClasss.Add(typeof(Door8800.Door.RelayReleaseTime.WriteRelayReleaseTime).FullName, "设置开锁时输出时长");

            mCommandClasss.Add(typeof(Door8800.Door.AutoLockedSetting.ReadAutoLockedSetting).FullName, "读取定时锁定门参数");
            mCommandClasss.Add(typeof(Door8800.Door.AutoLockedSetting.WriteAutoLockedSetting).FullName, "设置定时锁定门参数");

            mCommandClasss.Add(typeof(Door8800.Door.ReaderInterval.ReadReaderInterval).FullName, "读取重复读卡间隔参数");
            mCommandClasss.Add(typeof(Door8800.Door.ReaderInterval.WriteReaderInterval).FullName, "设置重复读卡间隔参数");

            mCommandClasss.Add(typeof(Door8800.Door.ReaderAlarm.WriteReaderAlarm).FullName, "设置读卡器防拆报警");
            mCommandClasss.Add(typeof(Door8800.Door.ReaderAlarm.ReadReaderAlarm).FullName, "读取读卡器防拆报警");

            mCommandClasss.Add(typeof(Door8800.Door.InterLockSetting.WriteInterLockSetting).FullName, "设置区域互锁");
            mCommandClasss.Add(typeof(Door8800.Door.InterLockSetting.ReadInterLockSetting).FullName, "读取区域互锁");

            mCommandClasss.Add(typeof(Door8800.Door.AreaAntiPassback.WriteAreaAntiPassback).FullName, "设置区域防潜回功能");
            mCommandClasss.Add(typeof(Door8800.Door.AreaAntiPassback.ReadAreaAntiPassback).FullName, "读取区域防潜回");

            mCommandClasss.Add(typeof(Door8800.Door.MultiCard.WriteMultiCard).FullName, "设置多卡组合参数");
            mCommandClasss.Add(typeof(Door8800.Door.MultiCard.ReadMultiCard).FullName, "读取多卡组合参数");

            mCommandClasss.Add(typeof(Door8800.Door.ManageKeyboardSetting.WriteManageKeyboardSetting).FullName, "设置键盘管理功能");
            mCommandClasss.Add(typeof(Door8800.Door.ManageKeyboardSetting.ReadManageKeyboardSetting).FullName, "读取键盘管理功能");

            mCommandClasss.Add(typeof(Door8800.Door.InOutSideReadOpenSetting.WriteInOutSideReadOpenSetting).FullName, "设置门内外同时读卡开门");
            mCommandClasss.Add(typeof(Door8800.Door.InOutSideReadOpenSetting.ReadInOutSideReadOpenSetting).FullName, "读取门内外同时读卡开门");

            mCommandClasss.Add(typeof(Door8800.Door.VoiceBroadcastSetting.WriteVoiceBroadcastSetting).FullName, "设置语音播报功能");
            mCommandClasss.Add(typeof(Door8800.Door.VoiceBroadcastSetting.ReadVoiceBroadcastSetting).FullName, "读取语音播报功能");

            mCommandClasss.Add(typeof(Door8800.Door.AnyCardSetting.WriteAnyCardSetting).FullName, "设置全卡开门功能");
            mCommandClasss.Add(typeof(Door8800.Door.AnyCardSetting.ReadAnyCardSetting).FullName, "读取全卡开门功能");

            mCommandClasss.Add(typeof(Door8800.Door.SensorAlarmSetting.WriteSensorAlarmSetting).FullName, "写入门磁报警功能");
            mCommandClasss.Add(typeof(Door8800.Door.SensorAlarmSetting.ReadSensorAlarmSetting).FullName, "读取门磁报警功能");

            mCommandClasss.Add(typeof(Door8800.Door.PushButtonSetting.WritePushButtonSetting).FullName, "写入出门按钮功能");
            mCommandClasss.Add(typeof(Door8800.Door.PushButtonSetting.ReadPushButtonSetting).FullName, "读取出门按钮功能");

            mCommandClasss.Add(typeof(Door8800.Door.OvertimeAlarmSetting.WriteOvertimeAlarmSetting).FullName, "写入开门超时报警功能");
            mCommandClasss.Add(typeof(Door8800.Door.OvertimeAlarmSetting.ReadOvertimeAlarmSetting).FullName, "读取开门超时报警功能");

            mCommandClasss.Add(typeof(Door8800.Door.InvalidCardAlarmOption.WriteInvalidCardAlarmOption).FullName, "设置非法读卡报警参数");
            mCommandClasss.Add(typeof(Door8800.Door.InvalidCardAlarmOption.ReadInvalidCardAlarmOption).FullName, "读取非法读卡报警参数");


            mCommandClasss.Add(typeof(Door8800.Card.ReadCardDatabaseDetail).FullName, "读取卡片存储详情");
            mCommandClasss.Add(typeof(Door89H.Card.ReadCardDataBase).FullName, "从控制器中读取所有卡片");
            mCommandClasss.Add(typeof(Door8800.Card.ReadCardDataBase).FullName, "从控制器中读取所有卡片");
            mCommandClasss.Add(typeof(Door8800.Card.ClearCardDataBase).FullName, "从控制器中清空所有卡片");
            mCommandClasss.Add(typeof(Door89H.Card.ReadCardDetail).FullName, "从控制器中读取单个卡详情");
            mCommandClasss.Add(typeof(Door8800.Card.ReadCardDetail).FullName, "从控制器中读取单个卡详情");
            mCommandClasss.Add(typeof(Door8800.Card.WriteCardListBySort).FullName, "上传卡片到排序区");
            mCommandClasss.Add(typeof(Door89H.Card.WriteCardListBySort).FullName, "上传卡片到排序区");
            mCommandClasss.Add(typeof(Door8800.Card.WriteCardListBySequence).FullName, "上传卡片到顺序区");
            mCommandClasss.Add(typeof(Door89H.Card.WriteCardListBySequence).FullName, "上传卡片到顺序区");
            mCommandClasss.Add(typeof(Door8800.Card.DeleteCard).FullName, "从控制器删除卡片");
            mCommandClasss.Add(typeof(Door89H.Card.DeleteCard).FullName, "从控制器删除卡片");

            mCommandClasss.Add(typeof(Door8800.Password.ReadPasswordDetail).FullName, "读取密码容量信息");
            mCommandClasss.Add(typeof(Door8800.Password.ClearPassword).FullName, "清空所有密码");
            mCommandClasss.Add(typeof(Door8800.Password.ReadAllPassword).FullName, "从控制器读取所有密码");
            mCommandClasss.Add(typeof(Door8800.Password.AddPassword).FullName, "将密码列表写入到控制器");
            mCommandClasss.Add(typeof(Door8800.Password.DeletePassword).FullName, "将密码列表从控制器删除");
            mCommandClasss.Add(typeof(Door89H.Password.ReadAllPassword).FullName, "从控制器读取所有密码");
            mCommandClasss.Add(typeof(Door89H.Password.AddPassword).FullName, "将密码列表写入到控制器");
            mCommandClasss.Add(typeof(Door89H.Password.DeletePassword).FullName, "将密码列表从控制器删除");


            mCommandClasss.Add(typeof(Door8800.Holiday.ReadHolidayDetail).FullName, "从控制板中读取节假日存储详情");
            mCommandClasss.Add(typeof(Door8800.Holiday.ClearHoliday).FullName, "清空控制器中的所有节假日");
            mCommandClasss.Add(typeof(Door8800.Holiday.ReadAllHoliday).FullName, "读取控制板中已存储的所有节假日");
            mCommandClasss.Add(typeof(Door8800.Holiday.AddHoliday).FullName, "添加节假日到控制版");
            mCommandClasss.Add(typeof(Door8800.Holiday.DeleteHoliday).FullName, "从控制器删除节假日");


            mCommandClasss.Add(typeof(Door8800.TimeGroup.ClearTimeGroup).FullName, "清空所有开门时段");
            mCommandClasss.Add(typeof(Door8800.TimeGroup.ReadTimeGroup).FullName, "读取所有开门时段");
            mCommandClasss.Add(typeof(Door8800.TimeGroup.AddTimeGroup).FullName, "添加开门时段");

            mCommandClasss.Add(typeof(Door8800.Transaction.ReadTransactionDatabaseDetail).FullName, "读取控制器中的卡片数据库信息");
            mCommandClasss.Add(typeof(Door8800.Transaction.ClearTransactionDatabase).FullName, "清空指定类型的记录数据库");
            mCommandClasss.Add(typeof(Door8800.Transaction.ReadTransactionDatabaseByIndex).FullName, "按指定序号读记录");
            mCommandClasss.Add(typeof(Door8800.Transaction.ReadTransactionDatabase).FullName, "读取新记录");
            mCommandClasss.Add(typeof(Door89H.Transaction.ReadTransactionDatabase).FullName, "读取新记录");
            mCommandClasss.Add(typeof(Door8800.Transaction.WriteTransactionDatabaseReadIndex).FullName, "更新记录指针");
            mCommandClasss.Add(typeof(Door8800.Transaction.WriteTransactionDatabaseWriteIndex).FullName, "修改指定记录数据库的写索引");

            mCommandClasss.Add(typeof(Door89H.Transaction.ReadTransactionDatabaseByIndex).FullName, "按指定序号读记录");

            var newDic = new Dictionary<string, string>();
            foreach (var item in mCommandClasss.Keys)
            {
                int index = item.LastIndexOf('.') + 1;
                string name = item.Substring(index);
                newDic.Add(item, GetLanguage(name));
            }
            mCommandClasss = newDic;
        }
        #region 串口管理


        /// <summary>
        /// 重新加载串口列表
        /// </summary>
        private void IniSerialPortList()
        {
            cmbSerialPort.Items.Clear();
            var Ports = System.IO.Ports.SerialPort.GetPortNames();
            if (Ports.Length > 0)
            {
                cmbSerialPort.Items.AddRange(Ports);
                cmbSerialPort.SelectedIndex = 0;
            }



        }

        private void butReloadSerialPort_Click(object sender, EventArgs e)
        {
            IniSerialPortList();
        }

        #endregion

        #region UDP
        /// <summary>
        /// UDP是否已绑定
        /// </summary>
        private bool mUDPIsBind = false;

        private void butUDPBind_Click(object sender, EventArgs e)
        {
            if (!txtUDPLocalPort.Text.IsNum())
            {
                MsgErr($"{GetLanguage("Msg38")}！");
                return;
            }
            int port = txtUDPLocalPort.Text.ToInt32();
            string sLocalIP = cmbLocalIP.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(sLocalIP))
            {
                MsgErr($"{GetLanguage("Msg39")}！");
                return;
            }


            DoNetDrive.Core.Connector.UDP.UDPServerDetail detail = new UDPServerDetail(sLocalIP, port);
            if (mUDPIsBind)
            {
                //关闭UDP服务器
                mAllocator.CloseConnector(detail);
                butUDPBind.Text = GetLanguage("Msg40");
                mUDPIsBind = false;
                txtUDPLocalPort.Enabled = true;
                cmbLocalIP.Enabled = true;
            }
            else
            {
                butUDPBind.Enabled = false;
                mUDPIsBind = true;
                txtUDPLocalPort.Enabled = false;
                cmbLocalIP.Enabled = false;
                //打开UDP服务器
                mAllocator.OpenConnector(detail);

                //等待后续事件，事件触发 mAllocator_ConnectorConnectedEvent 表示绑定成功
                //事件触发 mAllocator_ConnectorClosedEvent 表示绑定失败


            }
        }
        /// <summary>
        /// UDP绑定完毕
        /// </summary>
        /// <param name="bind">true 表示绑定成功</param>
        private void UDPBindOver(bool bind)
        {
            if (bind)
            {
                butUDPBind.Text = GetLanguage("Msg41");
            }
            else
            {
                mUDPIsBind = false;
                cmbLocalIP.Enabled = true;
                txtUDPLocalPort.Enabled = true;
            }

            butUDPBind.Enabled = true;
        }
        #endregion

        #region 初始化通讯类型
        /// <summary>
        /// 初始化通讯类型列表
        /// </summary>
        private void IniConnTypeList()
        {



            string sLanguage = ConfigurationManager.AppSettings["Languages"];
            cmbToolLanguage.Items.AddRange(sLanguage.SplitTrim(","));
            cmbToolLanguage.SelectedIndex = 0;

            mProtocolTypeTable[0] = CommandDetailFactory.ControllerType.Door58;
            mProtocolTypeTable[1] = CommandDetailFactory.ControllerType.Door88;
            mProtocolTypeTable[2] = CommandDetailFactory.ControllerType.Door89H;
            mProtocolTypeTable[3] = CommandDetailFactory.ControllerType.Door59;
            cmdProtocolType.Items.Clear();
            for (int i = 0; i < mProtocolTypeTable.Length; i++)
            {
                cmdProtocolType.Items.Add(mProtocolTypeTable[i]);
            }
            cmdProtocolType.SelectedIndex = 3;
            _IsClosed = false;

            int iTop = gbTCPClient.Top, iLeft = gbTCPClient.Left;
            gbSerialPort.Top = iTop; gbSerialPort.Left = iLeft;
            gbServer.Top = iTop; gbServer.Left = iLeft;
            gbUDP.Top = iTop; gbUDP.Left = iLeft;

            IniSerialPortList();

            IniLoadLocalIP();

            TCPServerClients = new Dictionary<string, TCPServerClientDetail_Item>();
        }

        /// <summary>
        /// 初始化本机IP
        /// </summary>
        private void IniLoadLocalIP()
        {
            cmbLocalIP.Items.Clear();

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
                    cmbLocalIP.Items.Add(ip.ToString());
                }
            }

            if (cmbLocalIP.Items.Count > 0)
            {
                cmbLocalIP.SelectedIndex = 0;
            }
        }

        private void cmdConnType_SelectedIndexChanged(object sender, EventArgs e)
        {

            ShowConnTypePanel();
        }
        /// <summary>
        /// 显示通讯方式面板
        /// </summary>
        private void ShowConnTypePanel()
        {
            bool[] pnlShow = new bool[4];
            pnlShow[cmdConnType.SelectedIndex] = true;

            GroupBox[] pnls = new GroupBox[] { gbSerialPort, gbTCPClient, gbUDP, gbServer };

            for (int i = 0; i < 4; i++)
            {
                pnls[i].Visible = pnlShow[i];
            }
        }

        #endregion

        #region 提示框
        public void MsgTip(string sText)
        {
            MessageBox.Show(sText, GetLanguage("Msg42"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MsgErr(string sText)
        {
            MessageBox.Show(sText, GetLanguage("Msg7"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region TCP 服务器
        /// <summary>
        /// TCPServer是否已绑定
        /// </summary>
        private bool mTCPServerBind;

        /// <summary>
        /// 包含所有客户端的项
        /// </summary>
        private Dictionary<string, TCPServerClientDetail_Item> TCPServerClients;

        /// <summary>
        /// 保存TCP客户端的详情
        /// </summary>
        private class TCPServerClientDetail_Item
        {
            /// <summary>
            /// 客户端的身份SN
            /// </summary>
            public string SN;
            /// <summary>
            /// 表示客户端的唯一Key
            /// </summary>
            public string Key;

            /// <summary>
            /// 客户端本地IP
            /// </summary>
            public IPDetail Local;

            /// <summary>
            /// 客户端的远程IP
            /// </summary>
            public IPDetail Remote;

            public TCPServerClientDetail_Item(TCPServerClientDetail detail)
            {
                SN = "";
                Key = detail.GetKey();
                Remote = new IPDetail(detail.Addr, detail.Port);
                Local = new IPDetail(detail.LocalAddr, detail.LocalPort);
            }

            public override string ToString()
            {
                if (string.IsNullOrEmpty(SN))
                {
                    return $"{Remote.Addr}:{Remote.Port}";
                }
                else
                {
                    return $"{SN}({Remote.Addr}:{Remote.Port})";
                }

            }
        }

        private void butBeginTCPServer_Click(object sender, EventArgs e)
        {
            if (!txtServerPort.Text.IsNum())
            {
                MsgErr(GetLanguage("Msg38") + "！");
                return;
            }
            int port = txtServerPort.Text.ToInt32();
            string sLocalIP = cmbLocalIP.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(sLocalIP))
            {
                MsgErr(GetLanguage("Msg39") + "！");
                return;
            }

            DoNetDrive.Core.Connector.TCPServer.TCPServerDetail detail = new TCPServerDetail(sLocalIP, port);
            if (mTCPServerBind)
            {
                //关闭UDP服务器
                mAllocator.CloseConnector(detail);
                butBeginTCPServer.Text = GetLanguage("Msg40");
                mTCPServerBind = false;
                txtServerPort.Enabled = true;
                cmbLocalIP.Enabled = true;
            }
            else
            {
                butBeginTCPServer.Enabled = false;
                mTCPServerBind = true;
                txtServerPort.Enabled = false;
                cmbLocalIP.Enabled = false;

                //打开UDP服务器
                mAllocator.OpenConnector(detail);

                //等待后续事件，事件触发 mAllocator_ConnectorConnectedEvent 表示绑定成功
                //事件触发 mAllocator_ConnectorClosedEvent 表示绑定失败


            }

        }
        /// <summary>
        /// TCPServer绑定完毕
        /// </summary>
        /// <param name="bind">true 表示绑定成功</param>
        private void TCPServerBindOver(bool bind)
        {
            if (bind)
            {
                butBeginTCPServer.Text = GetLanguage("Msg41");
            }
            else
            {
                mTCPServerBind = false;
                cmbLocalIP.Enabled = true;
                txtServerPort.Enabled = true;
            }

            butBeginTCPServer.Enabled = true;
        }

        /// <summary>
        /// 关闭一个已连接的TCP连接通道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butCloseTCPClient_Click(object sender, EventArgs e)
        {
            TCPServerClientDetail_Item oItem = cmbTCPClient.SelectedItem as TCPServerClientDetail_Item;
            if (oItem == null)
            {
                MsgErr($"{GetLanguage("Msg44")}！");
                return;
            }

            TCPServerClientDetail detail = new TCPServerClientDetail(oItem.Key);
            mAllocator.CloseConnector(detail);

        }

        /// <summary>
        /// 将客户端添加到列表中
        /// </summary>
        /// <param name="detail"></param>
        private void AddTCPServer_Client(INConnectorDetail detail)
        {
            if (cmbTCPClient.InvokeRequired)
            {
                Invoke(() => AddTCPServer_Client(detail));
                return;
            }
            TCPServerClientDetail oClient = detail as TCPServerClientDetail;
            var oItem = new TCPServerClientDetail_Item(oClient);

            cmbTCPClient.Items.Add(oItem);
            cmbTCPClient.SelectedIndex = cmbTCPClient.Items.Count - 1;
            TCPServerClients.Add(oItem.Key, oItem);

            AddIOLog(detail, GetLanguage("Msg45"), GetLanguage("Msg46"));
        }

        /// <summary>
        /// 从列表中删除TCP客户端
        /// </summary>
        /// <param name="detail"></param>
        private void RemoveTCPServer_Client(INConnectorDetail detail)
        {
            if (cmbTCPClient.InvokeRequired)
            {
                Invoke(() => RemoveTCPServer_Client(detail));
                return;
            }
            TCPServerClientDetail oClient = detail as TCPServerClientDetail;

            if (!TCPServerClients.ContainsKey(oClient.GetKey())) return;

            var oItem = TCPServerClients[oClient.GetKey()];
            cmbTCPClient.Items.Remove(oItem);
            cmbTCPClient.SelectedIndex = cmbTCPClient.Items.Count - 1;
            TCPServerClients.Remove(oItem.Key);
            AddIOLog(detail, GetLanguage("Msg47"), GetLanguage("Msg48"));
        }

        #endregion


        #region 命令结果日志

        private void InilstCommand()
        {
            lstCommand.BeginUpdate();

            var cols = lstCommand.Columns;
            cols.Clear();
            var sCaptions = GetLanguage("lstCommand").SplitTrim(",");
            var iWidths = new int[] { 100, 300, 120, 125, 100, 80 };
            for (int i = 0; i < sCaptions.Length; i++)
            {
                ColumnHeader col = new ColumnHeader();
                col.Text = sCaptions[i];
                col.TextAlign = HorizontalAlignment.Center;
                col.Width = iWidths[i];
                cols.Add(col);
            }
            lstCommand.HideSelection = true;
            lstCommand.LabelEdit = false;
            lstCommand.MultiSelect = false;
            lstCommand.FullRowSelect = true;
            lstCommand.GridLines = true;
            lstCommand.ShowItemToolTips = true;

            lstCommand.EndUpdate();
        }

        /// <summary>
        /// 添加命令日志
        /// </summary>
        /// <param name="e">命令描述符</param>
        /// <param name="txt">命令需要输出的内容</param>
        public void AddCmdLog(CommandEventArgs e, string txt)
        {
            ListViewItem oItem = new ListViewItem();

            INCommandDetail cmdDtl = e?.CommandDetail; string sType = e?.Command.GetType().FullName;
            if (_IsClosed) return;
            if (e != null)
            {
                double Timemill = 0;
                if (cmdDtl.EndTime == DateTime.MinValue || cmdDtl.BeginTime == DateTime.MinValue)
                {
                    Timemill = 0;
                }
                else
                {
                    Timemill = (cmdDtl.EndTime - cmdDtl.BeginTime).TotalMilliseconds;//命令耗时毫秒数
                }




                if (mCommandClasss.ContainsKey(sType))
                {
                    oItem.Text = mCommandClasss[sType];
                }
                else
                {
                    oItem.Text = sType;
                }

                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, txt));
                string Local, Remote, cType;
                GetConnectorDetail(cmdDtl.Connector, out cType, out Local, out Remote);
                OnlineAccess.OnlineAccessCommandDetail fcDtl = cmdDtl as OnlineAccess.OnlineAccessCommandDetail;
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, fcDtl.SN));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, Remote));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, DateTime.Now.ToTimeffff()));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, Timemill.ToString("0")));
                oItem.ToolTipText = txt;
            }
            else
            {
                oItem.Text = "-";
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, txt));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, string.Empty));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, string.Empty));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, string.Empty));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, string.Empty));
                oItem.ToolTipText = txt;
            }
            AddCmdItem(oItem);
        }

        private void AddCmdItem(ListViewItem oItem)
        {
            if (lstCommand.InvokeRequired)
            {
                Invoke(() => AddCmdItem(oItem));
                return;
            }
            lstCommand.BeginUpdate();
            lstCommand.Items.Insert(0, oItem);
            lstCommand.EndUpdate();
        }

        private void butClearCommand_Click(object sender, EventArgs e)
        {
            lstCommand.Items.Clear();
        }
        #endregion



        #region 数据监控


        private void buWatch_Click(object sender, EventArgs e)
        {
            var cmdDtl = GetCommandDetail();
            if (cmdDtl == null) return;

            INConnector cnt = mAllocator.GetConnector(cmdDtl.Connector);
            if (cnt == null)
            {
                //未开启监控
                mAllocator.OpenConnector(cmdDtl.Connector);
                cnt = mAllocator.GetConnector(cmdDtl.Connector);

            }

            BeginWatch cmd = new BeginWatch(cmdDtl);
            AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                AddCmdLog(cmde, GetLanguage("Msg49"));
            };

            //使通道保持连接不关闭
            cnt.OpenForciblyConnect();
            Door8800RequestHandle fC8800Request =
                new Door8800RequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
            cnt.RemoveRequestHandle(typeof(Door8800RequestHandle));//先删除，防止已存在就无法添加。
            cnt.AddRequestHandle(fC8800Request);



        }

        /// <summary>
        /// 用于根据SN，命令参数、命令索引生产用于处理对应消息的处理类工厂函数
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="cmdIndex"></param>
        /// <param name="cmdPar"></param>
        /// <returns></returns>
        private Transaction.AbstractTransaction RequestHandleFactory(string sn, byte cmdIndex, byte cmdPar)
        {
            bool bIsDoor89H = true;

            bIsDoor89H = Door89HSNTable.Contains(sn.Substring(0, 8));

            //在这里需要根据SN进行类型判定，也可以根据SN来进行查表
            if (cmdIndex >= 1 && cmdIndex <= 6)
            {
                if (bIsDoor89H)
                {
                    return Door89H.Transaction.ReadTransactionDatabaseByIndex.NewTransactionTable[cmdIndex]();
                }
                else
                {
                    return Door8800.Transaction.ReadTransactionDatabaseByIndex.NewTransactionTable[cmdIndex]();
                }
            }
            switch (cmdIndex)
            {
                case 0x23://连接确认信息
                    break;
                case 0x22://连接测试--心跳保活包
                    break;
                default:
                    break;
            }
            return null;
        }





        /// <summary>
        /// 监控消息
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="EventData"></param>
        private void MAllocator_TransactionMessage(INConnectorDetail connector, Core.Data.INData EventData)
        {
            Door8800Transaction fcTrn = EventData as Door8800Transaction;
            StringBuilder strbuf = new StringBuilder();
            var evn = fcTrn.EventData;
            strbuf.Append("SN:").Append(fcTrn.SN).Append($"；{GetLanguage("Msg50")}：").Append(TransactionTypeName[fcTrn.CmdIndex]).Append($"；{GetLanguage("Msg51")}：").Append(fcTrn.EventData.TransactionDate.ToDateTimeStr());
            strbuf.Append($"；{GetLanguage("Msg52")}：").Append(evn.TransactionCode);
            if (evn.TransactionType < 7)//1-6
            {
                string[] codeNameList = frmRecord.mTransactionCodeNameList[evn.TransactionType];
                strbuf.Append("(").Append(codeNameList[evn.TransactionCode]).Append(")");
            }

            if (fcTrn.CmdIndex == 1)
            {
                Door8800.Data.CardTransaction cardtrn = evn as Door8800.Data.CardTransaction;
                strbuf.Append($"；{GetLanguage("Msg53")}：").Append(cardtrn.CardData).Append($"；{GetLanguage("Msg54")}：").Append(cardtrn.DoorNum().ToString());
                strbuf.Append(cardtrn.IsEnter() ? $"({GetLanguage("Msg55")})" : $"({GetLanguage("Msg56")})");
            }
            if (fcTrn.CmdIndex > 1 && fcTrn.CmdIndex < 6)
            {
                Door8800.Data.AbstractDoorTransaction cardtrn = evn as Door8800.Data.AbstractDoorTransaction;
                strbuf.Append($"；{GetLanguage("Msg54")}：").Append(cardtrn.Door);
            }
            AddCmdLog(null, strbuf.ToString());
        }


        #endregion

        private void ButSearch_Click(object sender, EventArgs e)
        {

            var cmdDtl = GetCommandDetail();
            if (cmdDtl == null) return;

            var par = new DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor.SearchControltor_Parameter((ushort)DoNetDrive.Protocol.Door.Door8800.Utility.StringUtility.GetRandomNum(1, 65535));

            if (cmdConnType.SelectedIndex == 2 && chkUDPBroadcast.Checked)//UDP 
            {
                cmdDtl = GetCommandDetail("255.255.255.255");
                par.UDPBroadcast = true;
            }

            var cmd = new DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor.SearchControltor(cmdDtl, par);
            cmdDtl.Timeout = 10000;
            cmdDtl.RestartCount = 0;
            AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Result as Door8800.SystemParameter.SearchControltor.SearchControltor_Result;
                AddCmdLog(cmde, $"{GetLanguage("Msg57")},SN={result.SN},IP:{result.TCP.mIP}:{result.TCP.mTCPPort}");
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mAllocator.StopCommand(GetCommandDetail().Connector);
        }


        #region 多语言

        private string GetLanguage(string sKey)
        {
            return ToolLanguage.GetLanguage(Name, sKey);
        }
        private string GetLanguage(string sKey, params object[] args)
        {
            string str = ToolLanguage.GetLanguage(Name, sKey);
            return string.Format(str, args);
        }


        private void GetLanguage(ToolStripItem ctr)
        {
            ctr.Text = ToolLanguage.GetLanguage(Name, ctr.Name);
        }
        private void GetLanguage(Control ctr)
        {
            ctr.Text = ToolLanguage.GetLanguage(Name, ctr.Name);
        }
        private void GetLanguage(DataGridView dg)
        {
            string sCols = GetLanguage($"{dg.Name}_Cols");
            var sArr = sCols.SplitTrim(",");
            var cols = dg.Columns;
            for (int i = 0; i < cols.Count; i++)
            {
                var col = cols[i];
                col.HeaderText = sArr[i];
            }

        }


        private void LoadComboxItemsLanguage(ComboBox cbx, string sKey)
        {
            int iOldSelectIndex = cbx.SelectedIndex;
            cbx.Items.Clear();
            string sItems = GetLanguage(sKey);
            if (string.IsNullOrEmpty(sItems))
                return;

            string[] sArr = sItems.SplitTrim(",");
            cbx.Items.AddRange(sArr);
            if (cbx.Items.Count > iOldSelectIndex)
            {
                cbx.SelectedIndex = iOldSelectIndex;
            }
            else
            {
                cbx.SelectedIndex = 0;
            }
        }

        private void LoadUILanguage()
        {
            IniCommandClassNameList();
            Text = GetLanguage("FormCaption") + "  Ver " + Application.ProductVersion;
            GetLanguage(butSystem);
            GetLanguage(butTime);
            GetLanguage(butDoor);
            GetLanguage(butHoliday);
            GetLanguage(ButPassword);
            GetLanguage(ButTimeGroup);
            GetLanguage(butCard);
            GetLanguage(butRecord);
            GetLanguage(butUploadSoftware);
            GetLanguage(Lba_ConnType);
            GetLanguage(Lba_ProtocolType);
            GetLanguage(gbTCPClient);
            GetLanguage(gp_controller);
            GetLanguage(Lab_TCPClientPort);
            GetLanguage(Lab_Password);
            GetLanguage(gbServer);
            GetLanguage(Lab_ServerPort);
            GetLanguage(butBeginTCPServer);
            GetLanguage(butCloseTCPClient);
            GetLanguage(Lab_TCPClient);
            GetLanguage(gbSerialPort);
            GetLanguage(Lba_SerialPort);
            GetLanguage(butReloadSerialPort);
            GetLanguage(Lba_UDPLocalPort);
            GetLanguage(butUDPBind);
            GetLanguage(chkUDPBroadcast);
            GetLanguage(Lab_UDPPort);
            GetLanguage(tabPage1);
            GetLanguage(tabPage2);
            GetLanguage(butClear);
            GetLanguage(butClearCommand);
            GetLanguage(chkShowIO);
            GetLanguage(lblLocalAddress);
            GetLanguage(button2);
            GetLanguage(butSearch);
            GetLanguage(butWatch);
            GetLanguage(Lab_Process);
            cmdConnType.Items.Clear();
            cmdConnType.Items.AddRange(GetLanguage("cmdConnType").SplitTrim(","));
            cmdConnType.SelectedIndex = 2;
            chkUDPBroadcast.Checked = true;
            ShowConnTypePanel();
            var tTypeName = GetLanguage("TransactionTypeName").Split(',');
            TransactionTypeName = new string[7];
            for (int i = 0; i < tTypeName.Length; i++)
            {
                TransactionTypeName[i + 1] = tTypeName[i];
            }
            InilstCommand();
            IniLstIO();
            //TransactionTypeName[1] = "读卡记录";
            //TransactionTypeName[2] = "出门开关记录";
            //TransactionTypeName[3] = "门磁记录";
            //TransactionTypeName[4] = "软件操作记录";
            //TransactionTypeName[5] = "报警记录";
            //TransactionTypeName[6] = "系统记录";

            foreach (var frm in NodeForms)
            {
                frm.LoadUILanguage();
            }
        }
        private void cmbToolLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLanguage();
        }

        private void LoadLanguage()
        {
            var sFile = cmbToolLanguage.Text;
            sFile = ConfigurationManager.AppSettings[$"Language_{sFile}"];
            sFile = Path.Combine(Application.StartupPath, sFile);
            if (File.Exists(sFile))
                ToolLanguage.LoadLanguage(sFile);
            LoadUILanguage();
        }
        #endregion

        private void btnSN_Click(object sender, EventArgs e)
        {
            var cmdDtl = GetCommandDetail();
            if (cmdDtl == null) return;
            //for (int i = 0; i < 10000; i++)
            //{
                ReadSN cmd = new ReadSN(cmdDtl);
                AddCommand(cmd);
            //}
            

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                SN_Result result = cmde.Command.getResult() as SN_Result;
                string sn = result.SNBuf.GetString();
                Invoke(() =>
                {
                    txtSN.Text = sn;
                });
                AddCmdLog(cmde, sn);
            };
        }
    }
}
