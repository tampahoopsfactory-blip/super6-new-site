using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Connector.TCPClient;
using DoNetDrive.Core.Connector.TCPServer;
using DoNetDrive.Core.Connector.TCPServer.Client;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Protocol.Elevator.Test.Model;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Common.Extensions;

namespace DoNetDrive.Protocol.Elevator.Test
{
    public partial class frmMain : Form, INMain
    {
        ConnectorAllocator mAllocator;
        ConnectorObserverHandler mObserver;
        private static HashSet<Form> NodeForms;
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
            NodeForms = new HashSet<Form>();
            IniCommandClassNameList();

            TransactionTypeName = new string[7];
            TransactionTypeName[1] = "读卡记录";
            TransactionTypeName[2] = "出门开关记录";
            TransactionTypeName[3] = "门磁记录";
            TransactionTypeName[4] = "软件操作记录";
            TransactionTypeName[5] = "报警记录";
            TransactionTypeName[6] = "系统记录";

        }

        public static void AddNodeForms(Form frm)
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
        }


        /// <summary>
        /// 窗体初始化
        /// </summary>
        public void IniForm()
        {
            if (_IsClosed) return;

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

            IniConnTypeList();
            IniLstIO();
            InilstCommand();
            
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
            AddIOLog(connector.GetConnectorDetail(), "发送数据", msg);
        }


        private void MObserver_DisposeRequestEvent(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), "接收数据", msg);
        }

        private void mAllocator_ConnectorErrorEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(false));
                    AddIOLog(connector, "UDP绑定", "UDP绑定失败");
                    break;
                case ConnectorType.TCPServer://TCP Server 服务器
                    Invoke(() => TCPServerBindOver(false));
                    AddIOLog(connector, "TCP服务", "TCP服务器开启失败");
                    break;
                default:
                    AddIOLog(connector, "错误", "连接失败");
                    break;
            }
        }

        private void mAllocator_ConnectorClosedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(false));
                    AddIOLog(connector, "UDP绑定", "UDP绑定已关闭");
                    break;
                case ConnectorType.TCPServer://TCP Server 服务器
                    Invoke(() => TCPServerBindOver(false));
                    AddIOLog(connector, "TCP服务", "TCP服务已关闭");
                    break;
                default:
                    AddIOLog(connector, "关闭", "连接通道已关闭");
                    break;
            }
        }

        private void mAllocator_ConnectorConnectedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(true));
                    AddIOLog(connector, "UDP绑定", "UDP绑定成功");
                    break;
                case ConnectorType.TCPServer://TCP Server 服务器
                    Invoke(() => TCPServerBindOver(true));
                    AddIOLog(connector, "TCP服务", "TCP服务已启动");
                    break;
                default:
                    mAllocator.GetConnector(connector).AddRequestHandle(mObserver);

                    AddIOLog(connector, "成功", "通道连接成功");
                    break;
            }
        }

        #endregion

        #region 命令事件
        private void MAllocator_AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {
            AddCmdLog(e, "通讯密码错误");
        }

        private void mAllocator_CommandTimeout(object sender, CommandEventArgs e)
        {
            //if (e.Command.GetType().FullName == typeof(Door8800.SystemParameter.SearchControltor.SearchControltor).FullName)
            //{
            //    AddCmdLog(e, "搜索完毕");
            //    return;
            //}
            AddCmdLog(e, "命令超时");
        }


        private void mAllocator_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            AddCmdLog(e, "命令错误");
        }


        private const string Command_ReadSN = "DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.SN.ReadSN";
        private const string Command_WriteSN = "DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.SN.WriteSN";
        private const string Command_WriteSN_Broadcast = "DoNetDrive.Protocol.Door.FC8864.SystemParameter.SN.WriteSN_Broadcast";
        private const string Command_ReadConnectPassword = " DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ConnectPassword.ReadConnectPassword";
        private const string Command_WriteConnectPassword = "DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ConnectPassword.WriteConnectPassword";
        private const string Command_ResetConnectPassword = "DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ConnectPassword.ResetConnectPassword";

        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            /*
            if (e.Command.GetType().FullName == typeof(Door8800.SystemParameter.SearchControltor.SearchControltor).FullName)
            {
                return;
            }
            */
            mAllocator_CommandProcessEvent(sender, e);
            AddCmdLog(e, "命令完成");
            string cName = e.Command.GetType().FullName;
            /*   */
            switch (cName)
            {
                case Command_ReadSN://读SN
                    var sn = e.Command.getResult() as Protocol.Door.Door8800.SystemParameter.SN.SN_Result;
                    Invoke(() => txtSN.Text = sn.SNBuf.GetString());
                    break;
              
                case Command_ReadConnectPassword://读通讯密码
                    var pwd = e.Command.getResult() as Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Result;
                    Invoke(() => txtPassword.Text = pwd.Password);
                    break;
                case Command_WriteConnectPassword://写通讯密码
                    var pwdPar = e.Command.Parameter as Door.Door8800.SystemParameter.ConnectPassword.Password_Parameter;
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


            string sLog = $"{sName} 已耗时：{time:0},进度：{cmd.getProcessStep()} / {cmd.getProcessMax()}";
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
            string ret = $"通道类型：{cType} 本地IP：{Local} ,远端IP：{Remote}";

            switch (conn.GetTypeName())
            {
                case ConnectorType.UDPServer:
                    ret = $"通道类型：{cType}  本地绑定IP：{Local}";
                    break;
                case ConnectorType.TCPServer:
                    ret = $"通道类型：{cType} 本地绑定IP：{Local}";
                    break;
                case ConnectorType.SerialPort:
                    ret = $"通道类型：{cType} {Local}";
                    break;
                default:
                    ret = $"通道类型：{cType} {Local}";
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
                    cType = "TCP客户端";
                    Local = $"{local.ToString()}";
                    Remote = $"{tcpclient.Addr}:{tcpclient.Port}";
                    break;
                case ConnectorType.TCPServerClient:
                    cType = "TCP客户端节点";
                    var tcpclientOnly = conn as TCPServerClientDetail;
                    Local = $"{local.ToString()}";
                    Remote = $"{tcpclientOnly.Remote.ToString()}";
                    break;
                case ConnectorType.UDPClient:
                    cType = "UDP客户端";
                    var udpOnly = conn as TCPClientDetail;
                    Local = $"{local.ToString()}";
                    Remote = $"{udpOnly.Addr}:{udpOnly.Port}";
                    break;
                case ConnectorType.UDPServer:
                    cType = "UDP服务器";
                    Local = $"{local.ToString()}";
                    break;
                case ConnectorType.TCPServer:
                    cType = "TCP服务器";
                    Local = $"{local.ToString()}";
                    break;
                case ConnectorType.SerialPort:
                    cType = "串口";
                    var com = conn as Core.Connector.SerialPort.SerialPortDetail;
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


        /// <summary>
        /// 获取一个命令详情，已经装配好通讯目标的所有信息
        /// </summary>
        /// <returns>命令详情</returns>
        public INCommandDetail GetCommandDetail()
        {
            if (_IsClosed) return null;
            CommandDetailFactory.ConnectType connectType = CommandDetailFactory.ConnectType.TCPClient;
            CommandDetailFactory.ControllerType protocolType = CommandDetailFactory.ControllerType.Door88;
            string addr = string.Empty, sn, password;
            int port = 0;
            switch (cmdConnType.SelectedIndex)//串口,TCP客户端,UDP,TCP服务器
            {
                case 0://串口
                    if (cmbSerialPort.SelectedIndex == -1)
                    {
                        MsgTip("请先选择一个串口号！");
                        return null;
                    }
                    connectType = CommandDetailFactory.ConnectType.SerialPort;
                    addr = string.Empty;
                    port = cmbSerialPort.Text.Substring(3).ToInt32();
                    break;
                case 1://TCP 客户端方式通讯
                    connectType = CommandDetailFactory.ConnectType.TCPClient;
                    addr = txtTCPClientAddr.Text;
                    if (!int.TryParse(txtTCPClientPort.Text, out port))
                    {
                        port = 8000;
                    }
                    break;
                case 2://UDP 
                    if (!mUDPIsBind)
                    {
                        MsgErr("请先绑定UDP端口");
                        return null;
                    }
                    connectType = CommandDetailFactory.ConnectType.UDPClient;
                    addr = txtUDPAddr.Text;
                    if (!int.TryParse(txtUDPPort.Text, out port))
                    {
                        port = 8000;
                    }
                    break;
                case 3://TCP服务器
                    if (!mTCPServerBind)
                    {
                        MsgErr("请先开启TCP服务");
                        return null;
                    }

                    connectType = CommandDetailFactory.ConnectType.TCPServerClient;
                    if (cmbTCPClient.SelectedItem == null)
                    {
                        MsgErr("请选择一个TCP客户端！");
                        return null;
                    }
                    TCPServerClientDetail_Item oItem = cmbTCPClient.SelectedItem as TCPServerClientDetail_Item;

                    addr = oItem.Key;
                    break;
                default:
                    break;
            }
          

            if (port > 65535) port = 8000;

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



            var cmdDtl = CommandDetailFactory.CreateDetail(connectType, addr, port,
                protocolType, sn, password);

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
        #endregion


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
            mIOMessageList = new ConcurrentQueue<IOMessage>();
            Task.Run(() =>
            {
                do
                {
                    if (_IsClosed) break;
                    if (!mIOMessageList.IsEmpty)
                    {
                        Invoke(() =>
                        {
                            //lstIO.BeginUpdate();

                            do
                            {
                                IOMessage oItem;
                                if (mIOMessageList.TryDequeue(out oItem))
                                {
                                    dgvIO.Rows.Insert(0, oItem.Title, oItem.Content, oItem.Type, oItem.Time, oItem.Remote, oItem.Local);
                                    //int index = this.dgvIO.Rows.Add();
                                    //dgvIO.Rows[index].Cells[0].Value = oItem.Title.Title;
                                    //dgvIO.Rows[index].Cells[1].Value = oItem.Content;
                                    //dgvIO.Rows[index].Cells[2].Value = oItem.Type;
                                    //dgvIO.Rows[index].Cells[3].Value = oItem.Time;
                                    //dgvIO.Rows[index].Cells[4].Value = oItem.Remote;
                                    //dgvIO.Rows[index].Cells[5].Value = oItem.Local;
                                }
                            } while (!mIOMessageList.IsEmpty);

                            //lstIO.EndUpdate();
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

        private ConcurrentQueue<IOMessage> mIOMessageList;

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
            IOMessage iOMessage = new IOMessage();
            iOMessage.Title = sTag;
            if (txt.Length > 50)
            {
                txt = txt.Substring(0, 50) + "\r\n" + txt.Substring(50);
            }
            
            iOMessage.Content = txt;
            iOMessage.Type = cType;
            iOMessage.Remote = Remote;
            iOMessage.Local = Local;
            iOMessage.Time = DateTime.Now.ToTimeffff();

            mIOMessageList.Enqueue(iOMessage);
        }


        /// <summary>
        /// 清空所有通讯日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butClear_Click(object sender, EventArgs e)
        {
            dgvIO.Rows.Clear();
            //lstIO.Items.Clear();
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
          
        }
        #endregion

        /// <summary>
        /// 保存命令类型的功能名称
        /// </summary>
        private static Dictionary<string, string> mCommandClasss;

        /// <summary>
        /// 协议类型
        /// </summary>
        CommandDetailFactory.ControllerType[] mProtocolTypeTable = new CommandDetailFactory.ControllerType[3];
        /// <summary>
        /// 初始化命令类型的功能名称
        /// </summary>
        private static void IniCommandClassNameList()
        {
            mCommandClasss = new Dictionary<string, string>();

            mCommandClasss.Add(typeof(FC8864.SystemParameter.SN.ReadSN).FullName, "读取SN");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.ConnectPassword.ReadConnectPassword).FullName, "获取控制器通讯密码");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.ConnectPassword.WriteConnectPassword).FullName, "设置控制器通讯密码");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.ConnectPassword.ResetConnectPassword).FullName, "重置控制器通讯密码");

            mCommandClasss.Add(typeof(FC8864.SystemParameter.TCPSetting.ReadTCPSetting).FullName, "获取TCP参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.TCPSetting.WriteTCPSetting).FullName, "设置TCP参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.Deadline.ReadDeadline).FullName, "获取设备有效期");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.Deadline.WriteDeadline).FullName, "设置设备有效期");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.Version.ReadVersion).FullName, "获取设备版本号");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.SystemStatus.ReadSystemStatus).FullName, "获取设备运行信息");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadAllSystemSetting.ReadAllSystemSetting).FullName, "读取所有功能参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadRecordMode).FullName, "获取记录存储方式");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteRecordMode).FullName, "设置记录存储方式");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadKeyboard).FullName, "读取读卡器密码键盘启用功能开关");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteKeyboard).FullName, "设置读卡器密码键盘启用功能开关");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadFireAlarmOption).FullName, "获取消防报警参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteFireAlarmOption).FullName, "设置消防报警参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadBroadcast).FullName, "获取语音播报语音段开关");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteBroadcast).FullName, "设置语音播报语音段开关");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadReaderIntervalTime).FullName, "获取读卡间隔时间");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteReaderIntervalTime).FullName, "设置读卡间隔时间");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadReaderCheckMode).FullName, "获取读卡器数据校验");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteReaderCheckMode).FullName, "设置读卡器数据校验");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadBuzzer).FullName, "获取主板蜂鸣器");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteBuzzer).FullName, "设置主板蜂鸣器");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReaderByte.ReadReaderByte).FullName, "读取读卡器字节数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReaderByte.WriteReaderByte).FullName, "设置 读卡器字节数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.InvalidCardAlarmOption.ReadInvalidCardAlarmOption).FullName, "读取 非法读卡报警");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.InvalidCardAlarmOption.WriteInvalidCardAlarmOption).FullName, "设置非法读卡报警");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadSmogAlarmOption).FullName, "获取烟雾报警参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteSmogAlarmOption).FullName, "设置烟雾报警参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.AlarmPassword.ReadAlarmPassword).FullName, "读取胁迫报警功能");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.AlarmPassword.WriteAlarmPassword).FullName, "写入胁迫报警功能");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ExpirationPrompt.ReadExpirationPrompt).FullName, "读取卡片到期提示参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ExpirationPrompt.WriteExpirationPrompt).FullName, "设置卡片到期提示参数");
            //mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadTheftAlarmSetting).FullName, "获取智能防盗主机参数");
            //mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteTheftAlarmSetting).FullName, "设置智能防盗主机参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadCheckInOut).FullName, "获取防潜回模式");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteCheckInOut).FullName, "设置防潜回模式");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadReadCardSpeak).FullName, "获取定时读卡播报语音消息参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteReadCardSpeak).FullName, "设置定时读卡播报语音消息参数");
            //mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.ReadReadCardSpeak).FullName, "获取定时读卡播报语音消息参数");
            //mCommandClasss.Add(typeof(FC8864.SystemParameter.FunctionParameter.WriteReadCardSpeak).FullName, "设置定时读卡播报语音消息参数");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FireAlarm.SendFireAlarm).FullName, "通知设备触发消防报警");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.FireAlarm.CloseFireAlarm).FullName, "解除消防报警");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.CloseAlarm.WriteCloseAlarm).FullName, "解除报警");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.ManageCard.ReadManageCard).FullName, "读取 管理卡功能");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.ManageCard.WriteManageCard).FullName, "设置 管理卡功能");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.KeyboardCardIssuingManage.ReadKeyboardCardIssuingManage).FullName, "读取键盘发卡管理功能");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.KeyboardCardIssuingManage.WriteKeyboardCardIssuingManage).FullName, "设置 键盘发卡管理功能");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.InputTerminalFunction.ReadInputTerminalFunction).FullName, "读取输入端子功能定义");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.InputTerminalFunction.WriteInputTerminalFunction).FullName, "设置 输入端子功能定义");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.TCP485LineConnection.Read485LineConnection).FullName, "读取 TCP、485线路桥接");
            mCommandClasss.Add(typeof(FC8864.SystemParameter.TCP485LineConnection.Write485LineConnection).FullName, "设置 TCP、485线路桥接");

            mCommandClasss.Add(typeof(FC8864.Time.ReadTime).FullName, "从控制器中读取时间");
            mCommandClasss.Add(typeof(FC8864.Time.WriteTime).FullName, "将电脑的最新时间写入到控制器中");
            mCommandClasss.Add(typeof(FC8864.Time.WriteCustomTime).FullName, "将自定义时间写入到控制器中");
            mCommandClasss.Add(typeof(FC8864.Time.TimeErrorCorrection.ReadTimeError).FullName, "读取误差自修正参数");
            mCommandClasss.Add(typeof(FC8864.Time.TimeErrorCorrection.WriteTimeError).FullName, "设置误差自修正参数");

            mCommandClasss.Add(typeof(FC8864.Door.Relay.ReadRelay).FullName, "读取 继电器");
            mCommandClasss.Add(typeof(FC8864.Door.Relay.WriteRelay).FullName, "写入 继电器");
            mCommandClasss.Add(typeof(FC8864.Door.OpenDoor.WriteOpenDoor).FullName, "开门");
            mCommandClasss.Add(typeof(FC8864.Door.OpenDoor.WriteOpenDoorWithCode).FullName, "开门_带验证码");
            mCommandClasss.Add(typeof(FC8864.Door.CloseDoor.WriteCloseDoor).FullName, "关门");
            mCommandClasss.Add(typeof(FC8864.Door.DoorKeepOpen.WriteDoorKeepOpen).FullName, "设置门常开");
            mCommandClasss.Add(typeof(FC8864.Door.LockDoor.WriteLockDoor).FullName, "设置门锁定");
            mCommandClasss.Add(typeof(FC8864.Door.UnLockDoor.WriteUnLockDoor).FullName, "解除锁定");
            mCommandClasss.Add(typeof(FC8864.Door.AutoLockedSetting.ReadAutoLockedSetting).FullName, "读取定时锁定门参数");
            mCommandClasss.Add(typeof(FC8864.Door.AutoLockedSetting.WriteAutoLockedSetting).FullName, "设置定时锁定门参数");
            mCommandClasss.Add(typeof(FC8864.Door.UnlockingTime.ReadUnlockingTime).FullName, "读取开锁时输出时长");
            mCommandClasss.Add(typeof(FC8864.Door.UnlockingTime.WriteUnlockingTime).FullName, "设置开锁时输出时长");
            mCommandClasss.Add(typeof(FC8864.Door.OutDoorSwitch.ReadOutDoorSwitch).FullName, "读取出门开关参数");
            mCommandClasss.Add(typeof(FC8864.Door.OutDoorSwitch.WriteOutDoorSwitch).FullName, "设置出门开关参数");
            mCommandClasss.Add(typeof(FC8864.Door.FirstCardOpen.ReadFirstCardOpen).FullName, "读取 首卡开门参数");
            mCommandClasss.Add(typeof(FC8864.Door.FirstCardOpen.WriteFirstCardOpen).FullName, "设置 首卡开门参数");
            mCommandClasss.Add(typeof(FC8864.Door.GateMagneticAlarm.ReadGateMagneticAlarm).FullName, "读取 门磁报警参数");
            mCommandClasss.Add(typeof(FC8864.Door.GateMagneticAlarm.WriteGateMagneticAlarm).FullName, "设置门磁报警参数");
            mCommandClasss.Add(typeof(FC8864.Door.OpenDoorTimeoutAlarm.ReadOpenDoorTimeoutAlarm).FullName, "读取 开门超时报警参数");
            mCommandClasss.Add(typeof(FC8864.Door.OpenDoorTimeoutAlarm.WriteOpenDoorTimeoutAlarm).FullName, "设置 开门超时报警参数");
            mCommandClasss.Add(typeof(FC8864.Door.CancelDoorAlarm.WriteCancelDoorAlarm).FullName, "解除端口报警");
            mCommandClasss.Add(typeof(Protocol.Door.Door8800.Holiday.ReadHolidayDetail).FullName, "读取控制器节假日存储详情");
            mCommandClasss.Add(typeof(Protocol.Door.Door8800.Holiday.ClearHoliday).FullName, "清空控制器中的所有节假日");
            mCommandClasss.Add(typeof(Protocol.Door.Door8800.Holiday.ReadAllHoliday).FullName, "读取控制板中已存储的所有节假日");
            mCommandClasss.Add(typeof(Protocol.Door.Door8800.Holiday.AddHoliday).FullName, "添加节假日到控制版");
            mCommandClasss.Add(typeof(Protocol.Door.Door8800.Holiday.DeleteHoliday).FullName, "从控制器删除节假日");
            mCommandClasss.Add(typeof(FC8864.Password.ReadPasswordDetail).FullName, "从控制器读取密码容量信息");
            mCommandClasss.Add(typeof(FC8864.Password.ClearPassword).FullName, "清空所有密码");
            mCommandClasss.Add(typeof(FC8864.Password.ReadAllPassword).FullName, "从控制器读取所有密码");
            mCommandClasss.Add(typeof(FC8864.Password.AddPassword).FullName, "将密码列表写入到控制器");
            mCommandClasss.Add(typeof(FC8864.Password.DeletePassword).FullName, "将密码列表从控制器删除");
            mCommandClasss.Add(typeof(FC8864.TimeGroup.ReadTimeGroup).FullName, "读取所有开门时段");
            mCommandClasss.Add(typeof(FC8864.TimeGroup.ClearTimeGroup).FullName, "清空所有开门时段");
            mCommandClasss.Add(typeof(FC8864.TimeGroup.AddTimeGroup).FullName, "添加开门时段");
            mCommandClasss.Add(typeof(FC8864.Card.CardDatabaseDetail.ReadCardDatabaseDetail).FullName, "读取卡片存储详情");
            mCommandClasss.Add(typeof(FC8864.Card.CardDataBase.ReadCardDataBase).FullName, "从控制器中读取卡片数据");
            mCommandClasss.Add(typeof(FC8864.Card.CardDetail.ReadCardDetail).FullName, "读取单个卡片在控制器中的信息");
            mCommandClasss.Add(typeof(FC8864.Card.CardListBySequence.WriteCardListBySequence).FullName, "将卡片列表写入到控制器非排序区");
            mCommandClasss.Add(typeof(FC8864.Card.CardListBySort.WriteCardListBySort).FullName, "将卡片列表写入到控制器排序区");
            mCommandClasss.Add(typeof(FC8864.Card.ClearCardDataBase.ClearCardDataBase).FullName, "从控制器中清空所有卡片,可指定参数控制清空的区域");
            mCommandClasss.Add(typeof(FC8864.Card.DeleteCard.DeleteCard).FullName, "将卡片列表从到控制器中删除");
            mCommandClasss.Add(typeof(FC8864.Transaction.ClearTransactionDatabase.ClearTransactionDatabase).FullName, "清空指定类型的记录数据库");
            mCommandClasss.Add(typeof(FC8864.Transaction.ReadTransactionDatabase.ReadTransactionDatabase).FullName, "读取新记录");
            mCommandClasss.Add(typeof(FC8864.Transaction.ReadTransactionDatabaseByIndex.ReadTransactionDatabaseByIndex).FullName, "读记录数据库");
            mCommandClasss.Add(typeof(FC8864.Transaction.TransactionDatabaseDetail.ReadTransactionDatabaseDetail).FullName, "读取控制器中的卡片数据库信息");
            mCommandClasss.Add(typeof(FC8864.Transaction.TransactionDatabaseReadIndex.WriteTransactionDatabaseReadIndex).FullName, "更新记录指针");
            mCommandClasss.Add(typeof(FC8864.Transaction.WriteTransactionDatabaseWriteIndex.WriteTransactionDatabaseWriteIndex).FullName, "修改指定记录数据库的写索引");
            mCommandClasss.Add(typeof(FC8864.Transaction.ClearTransactionDatabase.TransactionDatabaseEmpty).FullName, "清空所有类型的记录数据库");
            //mCommandClasss.Add(typeof(FC8864.SystemParameter.).FullName, "重置控制器通讯密码");
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
                MsgErr("端口号不正确！");
                return;
            }
            int port = txtUDPLocalPort.Text.ToInt32();
            string sLocalIP = cmbLocalIP.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(sLocalIP))
            {
                MsgErr("没有绑定本地IP！");
                return;
            }


            DoNetDrive.Core.Connector.UDP.UDPServerDetail detail = new UDPServerDetail(sLocalIP, port);
            if (mUDPIsBind)
            {
                //关闭UDP服务器
                mAllocator.CloseConnector(detail);
                butUDPBind.Text = "开启服务";
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
                butUDPBind.Text = "关闭服务";
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
            cmdConnType.Items.AddRange("串口,TCP客户端,UDP,TCP服务器".SplitTrim(","));
            cmdConnType.SelectedIndex = 1;
            ShowConnTypePanel();


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
            MessageBox.Show(sText, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MsgErr(string sText)
        {
            MessageBox.Show(sText, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Key = detail.Key;
                Remote = new IPDetail(detail.Remote.Addr, detail.Remote.Port);
                Local = new IPDetail(detail.Local.Addr, detail.Local.Port);
            }

            public override string ToString()
            {
                if (string.IsNullOrEmpty(SN))
                {
                    return $"远程:{Remote.Addr}:{Remote.Port}";
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
                MsgErr("端口号不正确！");
                return;
            }
            int port = txtServerPort.Text.ToInt32();
            string sLocalIP = cmbLocalIP.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(sLocalIP))
            {
                MsgErr("没有绑定本地IP！");
                return;
            }

            DoNetDrive.Core.Connector.TCPServer.TCPServerDetail detail = new TCPServerDetail(sLocalIP, port);
            if (mTCPServerBind)
            {
                //关闭UDP服务器
                mAllocator.CloseConnector(detail);
                butBeginTCPServer.Text = "开启服务";
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
                butBeginTCPServer.Text = "关闭服务";
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
                MsgErr("请选择一个客户端！");
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

            AddIOLog(detail, "上线", "TCP 客户端已上线");
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

            if (!TCPServerClients.ContainsKey(oClient.Key)) return;

            var oItem = TCPServerClients[oClient.Key];
            cmbTCPClient.Items.Remove(oItem);
            cmbTCPClient.SelectedIndex = cmbTCPClient.Items.Count - 1;
            TCPServerClients.Remove(oItem.Key);
            AddIOLog(detail, "离线", "TCP 客户端已离线");
        }

        #endregion


        #region 命令结果日志

        private void InilstCommand()
        {
            
        }

        /// <summary>
        /// 添加命令日志
        /// </summary>
        /// <param name="e">命令描述符</param>
        /// <param name="txt">命令需要输出的内容</param>
        public void AddCmdLog(CommandEventArgs e, string txt)
        {
            CommandResult commandResult = new CommandResult();
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
                    commandResult.Title = mCommandClasss[sType];
                }
                else
                {
                    commandResult.Title = sType;
                }
                commandResult.Content = txt;
                string Local, Remote, cType;
                GetConnectorDetail(cmdDtl.Connector, out cType, out Local, out Remote);
                OnlineAccess.OnlineAccessCommandDetail fcDtl = cmdDtl as OnlineAccess.OnlineAccessCommandDetail;
                commandResult.SN = fcDtl.SN;
                commandResult.Remote = Remote;
                commandResult.Time = DateTime.Now.ToTimeffff();
                commandResult.Timemill = Timemill.ToString("0");
            }
            else
            {
                commandResult.Title = "-";
                
            }
            AddCmdItem(commandResult);
        }

        private void AddCmdItem(CommandResult commandResult)
        {
            if (dgvResult.InvokeRequired)
            {
                Invoke(() => AddCmdItem(commandResult));
                return;
            }
            dgvResult.Rows.Insert(0, commandResult.Title, commandResult.Content, commandResult.SN, commandResult.Remote, commandResult.Time, commandResult.Timemill);
            //int index = this.dgvResult.Rows.Add();
            //dgvResult.Rows[index].Cells[0].Value = commandResult.Title;
            //dgvResult.Rows[index].Cells[1].Value = commandResult.Content;
            //dgvResult.Rows[index].Cells[2].Value = commandResult.SN;
            //dgvResult.Rows[index].Cells[3].Value = commandResult.Remote;
            //dgvResult.Rows[index].Cells[4].Value = commandResult.Time;
            //dgvResult.Rows[index].Cells[5].Value = commandResult.Timemill;
        }

        private void butClearCommand_Click(object sender, EventArgs e)
        {
            dgvResult.Rows.Clear();
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
            /*
            BeginWatch cmd = new BeginWatch(cmdDtl);
            AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                AddCmdLog(cmde, "已开启监控");
            };
            */
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
            return null;
        }





        /// <summary>
        /// 监控消息
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="EventData"></param>
        private void MAllocator_TransactionMessage(INConnectorDetail connector, Core.Data.INData EventData)
        {
            /*
            Door8800Transaction fcTrn = EventData as Door8800Transaction;
            StringBuilder strbuf = new StringBuilder();
            var evn = fcTrn.EventData;
            strbuf.Append("SN:").Append(fcTrn.SN).Append("；消息类型：").Append(TransactionTypeName[fcTrn.CmdIndex]).Append("；时间：").Append(fcTrn.EventData.TransactionDate.ToDateTimeStr());
            strbuf.Append("；事件代码：").Append(evn.TransactionCode);
            if (evn.TransactionType < 7)//1-6
            {
                string[] codeNameList =frmRecord.mTransactionCodeNameList[evn.TransactionType];
                strbuf.Append("(").Append(codeNameList[evn.TransactionCode]).Append(")");
            }

            if (fcTrn.CmdIndex == 1)
            {
                Door8800.Data.CardTransaction cardtrn = evn as Door8800.Data.CardTransaction;
                strbuf.Append("；卡号：").Append(cardtrn.CardData).Append("；门号：").Append(cardtrn.DoorNum().ToString());
                strbuf.Append(cardtrn.IsEnter()?"(进门)":"(出门)");
            }
            if(fcTrn.CmdIndex>1 && fcTrn.CmdIndex<6)
            {
                Door8800.Data.AbstractDoorTransaction cardtrn = evn as Door8800.Data.AbstractDoorTransaction;
                strbuf.Append("；门号：").Append(cardtrn.Door);
            }
            AddCmdLog(null, strbuf.ToString());
            */
        }


        #endregion

        private void DgvIO_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxColumn textbox = dgvIO.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
            if (textbox != null) //如果该列是TextBox列
            {
                dgvIO.BeginEdit(true); //开始编辑状态
                dgvIO.ReadOnly = false;
            }
        }
    }
}
