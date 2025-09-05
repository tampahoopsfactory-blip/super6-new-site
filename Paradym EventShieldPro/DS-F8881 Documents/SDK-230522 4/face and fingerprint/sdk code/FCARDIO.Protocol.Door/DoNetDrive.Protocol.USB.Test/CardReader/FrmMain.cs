using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Common.Extensions;
using System.Collections.Concurrent;
using DoNetDrive.Protocol.USBDrive;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN;

namespace DoNetDrive.Protocol.USB.CardReader.Test
{
    public partial class FrmMain : Form, INMain
    {
        ConnectorAllocator mAllocator;
        ConnectorObserverHandler mObserver;
        private static HashSet<Form> NodeForms;
        bool _IsClosed;

        /// <summary>
        /// 保存命令类型的功能名称
        /// </summary>
        private static Dictionary<string, string> mCommandClasss;

        private static void IniCommandClassNameList()
        {
            mCommandClasss = new Dictionary<string, string>();
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN.ReadSN).FullName, "读取SN");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN.WriteSN).FullName, "写入SN");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.Version.ReadVersion).FullName, "获取设备版本号");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.CreateTime.ReadCreateTime).FullName, "读取生产日期");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.CreateTime.WriteCreateTime).FullName, "写入生产日期");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.ReadCardType.ReadReadCardType).FullName, "读取记录存储方式");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.ReadCardType.WriteReadCardType).FullName, "写入记录存储方式");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.OutputFormat.ReadOutputFormat).FullName, "读取输出格式");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.OutputFormat.WriteOutputFormat).FullName, "写入输出格式");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardControl.ReadICCardControl).FullName, "读取扇区验证");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardControl.WriteICCardControl).FullName, "写入扇区验证");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardCustomNum.ReadICCardCustomNum).FullName, "读取卡号参数");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardCustomNum.WriteICCardCustomNum).FullName, "写入卡号参数");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.TTLOutput.ReadTTLOutput).FullName, "读取TTL输出参数");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.TTLOutput.WriteTTLOutput).FullName, "写入TTL输出参数");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.Initialize.Initialize).FullName, "初始化读卡器");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.Buzzer.WriteBuzzer).FullName, "控制蜂鸣器");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.LED.WriteLED).FullName, "控制LED灯");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.AgencyCode.ReadAgencyCode).FullName, "读取经销商代码");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.SystemParameter.AgencyCode.WriteAgencyCode).FullName, "设置经销商代码");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.ICCard.SearchCard.SearchCard).FullName, "寻卡");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.ICCard.Sector.ReadSector).FullName, "读扇区内容");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.ICCard.Sector.WriteSector).FullName, "写扇区内容");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.CardReader.ICCard.Sector.ReadAllSector).FullName, "读取扇区全部内容");

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

        static FrmMain()
        {
            IniCommandClassNameList();
            NodeForms = new HashSet<Form>();
        }

        public FrmMain()
        {
            InitializeComponent();
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

            //mAllocator.ClientOnline += mAllocator_ClientOnline;
            //mAllocator.ClientOffline += mAllocator_ClientOffline;

            mObserver.DisposeRequestEvent += mObserver_DisposeRequestEvent;
            mObserver.DisposeResponseEvent += mObserver_DisposeResponseEvent; ;

            InitSerialPort();
            IniLstIO();
            InilstCommand();
            Task.Run((Action)ShowCommandProcesslog);
        }

        public static void AddNodeForms(Form frm)
        {
            if (!NodeForms.Contains(frm))
            {
                NodeForms.Add(frm);
            }
        }

        private void lblReLoadCOMList_Click(object sender, EventArgs e)
        {
            InitSerialPort();
        }

        /// <summary>
        /// 初始化串口列表
        /// </summary>
        private void InitSerialPort()
        {
            cmbSerialPort.Items.Clear();
            var Ports = System.IO.Ports.SerialPort.GetPortNames();
            if (Ports.Length > 0)
            {
                cmbSerialPort.Items.AddRange(Ports);
                cmbSerialPort.SelectedIndex = 0;
            }
        }

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
        }

        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }
        #region 回调事件


        private void mObserver_DisposeResponseEvent(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), "发送数据", msg);
        }

        private void mObserver_DisposeRequestEvent(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), "接收数据", msg);
        }



        private void mAllocator_ConnectorErrorEvent(object sender, INConnectorDetail connector)
        {
            AddIOLog(connector, "错误", "连接失败");
        }

        private void mAllocator_ConnectorClosedEvent(object sender, INConnectorDetail connector)
        {
            AddIOLog(connector, "关闭", "连接通道已关闭");
        }

        private void mAllocator_ConnectorConnectedEvent(object sender, INConnectorDetail connector)
        {
            string typeName = connector.GetTypeName();
            mAllocator.GetConnector(connector).AddRequestHandle(mObserver);

            AddIOLog(connector, "成功", "通道连接成功");
        }

        private void MAllocator_TransactionMessage(INConnectorDetail connector, INData EventData)
        {
            if (EventData is USBDrive.USBDriveTransaction)
            {
                USBDriveTransaction USBtr = EventData as USBDriveTransaction;
                Watch.WatchReadCardTransaction cardTr = USBtr.EventData as Watch.WatchReadCardTransaction;

                ListViewItem oItem = new ListViewItem();
                string sLog = $"卡类型：{cardTr.CardType} ,卡字节：{cardTr.CardDataLen}，卡号：{cardTr.Card}(0x{cardTr.Card:X16})";

                oItem.Text = "读卡消息";
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, sLog));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, string.Empty));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, string.Empty));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, cardTr.TransactionDate.ToDateTimeStr()));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, string.Empty));
                oItem.ToolTipText = sLog;

                AddCmdItem(oItem);
            }

        }

        private void MAllocator_AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {
            AddCmdLog(e, "通讯密码错误");
        }

        private void mAllocator_CommandTimeout(object sender, CommandEventArgs e)
        {
            if (e.CommandDetail.UserData?.ToString() == "TestReadSN")
                return;
            //if (e.Command.GetType().FullName == typeof(FC8800.SystemParameter.SearchControltor.SearchControltor).FullName)
            //{
            //    AddCmdLog(e, "搜索完毕");
            //    return;
            //}
            AddCmdLog(e, "命令超时");
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

        private void mAllocator_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            if (e.CommandDetail.UserData?.ToString() == "TestReadSN")
                return;
            AddCmdLog(e, "命令错误");
        }

        private const string Command_ReadSN = "DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN.ReadSN";
        private const string Command_WriteSN = "DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN.WriteSN";

        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            mAllocator_CommandProcessEvent(sender, e);
            AddCmdLog(e, "命令完成");
            string cName = e.Command.GetType().FullName;

            switch (cName)
            {
                case Command_ReadSN://读SN
                    SystemParameter.SN.SN_Result sn = e.Command.getResult() as SystemParameter.SN.SN_Result;
                    Invoke(() => txtAddress.Text = sn.SN.ToString());
                    break;
                case Command_WriteSN://写SN
                    SystemParameter.SN.SN_Parameter snPar = e.Command.Parameter as SystemParameter.SN.SN_Parameter;
                    Invoke(() => txtAddress.Text = snPar.SN.ToString());
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void FrmMain_Load(object sender, EventArgs e)
        {

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButClearCommand_Click(object sender, EventArgs e)
        {
            lstCommand.Items.Clear();
        }

        private void ButClear_Click(object sender, EventArgs e)
        {
            lstIO.Items.Clear();
        }
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


        private void InilstCommand()
        {
            lstCommand.BeginUpdate();

            var cols = lstCommand.Columns;
            cols.Clear();
            var sCaptions = "类型,内容,身份信息,串口信息,时间,耗时".SplitTrim(",");
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
                string Local, cType;
                GetConnectorDetail(cmdDtl.Connector, out cType, out Local);
                USBDriveCommandDetail fcDtl = cmdDtl as USBDriveCommandDetail;
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, fcDtl.Addr));
                oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, Local));
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
        #endregion

        #region 获取通道描述信息
        private string GetConnectorDetail(INConnector conn)
        {
            return GetConnectorDetail(conn.GetConnectorDetail());
        }
        private string GetConnectorDetail(INConnectorDetail conn)
        {
            string Local, cType;
            GetConnectorDetail(conn, out cType, out Local);
            string ret = $"通道类型：{cType} 本地IP：{Local}";

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
        /// <returns></returns>
        private void GetConnectorDetail(INConnectorDetail conn, out string cType, out string Local)
        {
            Local = string.Empty;
            cType = string.Empty;

            var oConn = mAllocator.GetConnector(conn);
            if (oConn == null) return;

            IPDetail local = oConn.LocalAddress();
            conn = oConn.GetConnectorDetail();

            switch (conn.GetTypeName())
            {
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

        /// <summary>
        /// 获取选择的串口
        /// </summary>
        /// <returns></returns>
        public int GetSerialPort()
        {
            return (cmbSerialPort.Text.Replace("COM", string.Empty).ToInt32());
        }

        /// <summary>
        /// 获取一个命令详情，已经装配好通讯目标的所有信息
        /// </summary>
        /// <param name="iPort"></param>
        /// <returns></returns>
        public USBDriveCommandDetail GetCommandDetail(int iPort)
        {
            if (_IsClosed) return null;
            USBDriveCommandDetail cmdDtl = CommandDetailFactory.CreateDetail(
                CommandDetailFactory.ConnectType.SerialPort, "", iPort,
                CommandDetailFactory.ControllerType.USBDrive_CardReader, "", string.Empty) as USBDriveCommandDetail;
            DoNetDrive.Core.Connector.SerialPort.SerialPortDetail spd = cmdDtl.Connector as DoNetDrive.Core.Connector.SerialPort.SerialPortDetail;
            spd.Baudrate = 19200;
            return cmdDtl;
        }

        /// <summary>
        /// 获取一个命令详情，已经装配好通讯目标的所有信息
        /// </summary>
        /// <returns>命令详情</returns>
        public USBDriveCommandDetail GetCommandDetail()
        {
            return GetCommandDetail(GetSerialPort());
        }
        #endregion

        #region 通讯日志
        private bool mShowIOEvent = true;
        private void chkShowIO_CheckedChanged(object sender, EventArgs e)
        {
            mShowIOEvent = chkShowIO.Checked;
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

            string Local, cType;
            GetConnectorDetail(connDetail, out cType, out Local);

            ListViewItem oItem = new ListViewItem();
            oItem.Text = sTag;
            oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, txt));
            oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, Local));
            oItem.SubItems.Add(new ListViewItem.ListViewSubItem(oItem, DateTime.Now.ToTimeffff()));
            oItem.ToolTipText = txt;
            mIOItems.Enqueue(oItem);
        }

        /// <summary>
        /// 初始化通讯日志列表
        /// </summary>
        private void IniLstIO()
        {
            lstIO.BeginUpdate();

            var cols = lstIO.Columns;
            cols.Clear();
            var sCaptions = "标签,内容,串口信息,时间".SplitTrim(",");
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

        #endregion


        private void ButSystem_Click(object sender, EventArgs e)
        {
            frmSystem frm = frmSystem.GetForm(this);
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

        private void ButRecord_Click(object sender, EventArgs e)
        {

        }

        private void ButSearchCard_Click(object sender, EventArgs e)
        {
            frmICCard frm = frmICCard.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);

        }

        private void ButEncryptionDecryption_Click(object sender, EventArgs e)
        {
            frmEncryptionDecryption frm = frmEncryptionDecryption.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void BtnWatch_Click(object sender, EventArgs e)
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

            //使通道保持连接不关闭
            cnt.OpenForciblyConnect();
            USBDriveRequestHandle usbRequest =
                new USBDriveRequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
            cnt.RemoveRequestHandle(typeof(USBDriveRequestHandle));//先删除，防止已存在就无法添加。
            cnt.AddRequestHandle(usbRequest);

        }

        /// <summary>
        /// 用于根据SN，命令参数、命令索引生产用于处理对应消息的处理类工厂函数
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="cmdIndex"></param>
        /// <returns></returns>
        private Transaction.AbstractTransaction RequestHandleFactory(byte addr, byte cmdIndex)
        {
            switch (cmdIndex)
            {
                case 0x01://读卡消息
                    return new DoNetDrive.Protocol.USB.CardReader.Watch.WatchReadCardTransaction();
                default:
                    break;
            }
            return null;
        }

        private void BurReadSN_Click(object sender, EventArgs e)
        {

            var Ports = System.IO.Ports.SerialPort.GetPortNames();

            foreach (var p in Ports)
            {
                AddCmdLog(null,$" 读取串口号： {p}");

                int port = int.Parse(p.Replace("COM", string.Empty));

                var cmdDtl = GetCommandDetail(port);
                cmdDtl.UserData = "TestReadSN";
                cmdDtl.Timeout = 400;
                cmdDtl.RestartCount = 0;
                if (cmdDtl == null) return;


                ReadSN cmd = new ReadSN(cmdDtl);
                AddCommand(cmd);
                var connter = mAllocator.GetConnector(cmdDtl.Connector) as AbstractConnector;
                connter.ChannelKeepaliveMaxTime = 2;
                //处理返回值
                cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
                {
                    SN_Result result = cmde.Command.getResult() as SN_Result;
                    string sn = result.SN.ToString();
                    Invoke(() =>
                    {
                        txtAddress.Text = sn;
                    });
                    AddCmdLog(cmde, $"COM{port} -- 地址号：{sn}");
                };
                cmdDtl.CommandErrorEvent += (sdr, cmde) =>
                {
                    AddCmdLog(cmde, $"COM{port} -- 读地址号命令错误");
                };
                cmdDtl.CommandTimeout += (sdr, cmde) => {
                    AddCmdLog(cmde, $"COM{port} -- 读地址号命令超时");
                };

            }
            
        }


    }
}
