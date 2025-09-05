using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using DoNetDrive.Protocol.USBDrive;
using DoNetDrive.Core.Connector.SerialPort;
using DoNetDrive.Protocol.Transaction;
using DotNetty.Buffers;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ReadFlag;
using DoNetDrive.Common.Extensions;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Test
{
    public partial class FrmMain : Form, INMain
    {
        ConnectorAllocator mAllocator;
        ConnectorObserverHandler mObserver;
        USBCommandObserverHandler mUSBObserver;
        private static HashSet<Form> NodeForms;
        IByteBufferAllocator mByteBufferAllocator;
        bool _IsClosed;

        /// <summary>
        /// 保存命令类型的功能名称
        /// </summary>
        private static Dictionary<string, string> mCommandClasss;
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
        #endregion
        private  void IniCommandClassNameList()
        {
            mCommandClasss = new Dictionary<string, string>();
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.SN.ReadSN).FullName, "读取SN");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.SN.WriteSN).FullName, "写入SN");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ExpireTime.WriteExpireTime).FullName, "写入设备有效期");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ExpireTime.ReadExpireTime).FullName, "读取设备有效期");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.Version.ReadVersion).FullName, "获取设备版本号");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.SystemStatus.ReadSystemStatus).FullName, "获取设备运行信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.RecordStorageMode.ReadRecordStorageMode).FullName, "获取记录存储方式");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.RecordStorageMode.WriteRecordStorageMode).FullName, "写入记录存储方式");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.CreateTime.ReadCreateTime).FullName, "读取生产日期");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.CreateTime.WriteCreateTime).FullName, "设置生产日期");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.StartupHoldTime.ReadStartupHoldTime).FullName, "获取开机保持时间");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.StartupHoldTime.WriteStartupHoldTime).FullName, "设置开机保持时间");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.LEDOpenHoldTime.ReadLEDOpenHoldTime).FullName, "读取LED开灯保持时间");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.LEDOpenHoldTime.WriteLEDOpenHoldTime).FullName, "设置LED开灯保持时间");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ReadFlag.OpenReadFlag).FullName, "打开读卡标记");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Time.ReadTime).FullName, "从设备中读取控制器时间");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Time.WriteTime).FullName, "将电脑的最新时间写入到设备中");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Time.WriteCustomTime).FullName, "将自定义时间写入到设备中");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDatabaseDetail.ReadPatrolEmplDatabaseDetail).FullName, "读取巡更人员信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.ClearPatrolEmplDataBase.ClearPatrolEmplDataBase).FullName, "删除所有巡更人员");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDatabase.ReadPatrolEmplDatabase).FullName, "读取所有巡更人员");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDetail.ReadPatrolEmplDetail).FullName, "读取单个巡更人员资料");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.WritePatrolEmpl.WritePatrolEmpl).FullName, "添加巡更人员");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.DeletePatrolEmpl.DeletePatrolEmpl).FullName, "删除巡更人员");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ReadTransactionDatabase.ReadTransactionDatabase).FullName, "读取新记录");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ReadTransactionDatabaseByIndex.ReadTransactionDatabaseByIndex).FullName, "读记录数据库");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.TransactionDatabaseDetail.ReadTransactionDatabaseDetail).FullName, "读取控制器中的卡片数据库信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.WriteTransactionDatabaseReadIndex.WriteTransactionDatabaseReadIndex).FullName, "更新记录指针");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.WriteTransactionDatabaseWriteIndex.WriteTransactionDatabaseWriteIndex).FullName, "修改指定记录数据库的写索引");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ClearTransactionDatabase.ClearTransactionDatabase).FullName, "清空指定类型的记录数据库");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ClearTransactionDatabase.TransactionDatabaseEmpty).FullName, "清空所有类型的记录数据库");

           
            var keys = mCommandClasss.Keys.ToList();
            foreach (var item in keys)
            {
                var index = item.LastIndexOf('.');
                var key = item.Substring(index + 1);
                if (mCommandClasss.ContainsKey(item))
                {
                    mCommandClasss[item] = GetLanguage(key);
                }
            }
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
            mObserver.DisposeResponseEvent += mObserver_DisposeResponseEvent;

            InitSerialPort();
            LoadSetting();
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
            AddIOLog(connector.GetConnectorDetail(), GetLanguage("Txt1"), msg);
        }

        private void mObserver_DisposeRequestEvent(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), GetLanguage("Txt2"), msg);
        }



        private void mAllocator_ConnectorErrorEvent(object sender, INConnectorDetail connector)
        {
            AddIOLog(connector, GetLanguage("Txt3"), GetLanguage("Txt4"));
        }

        private void mAllocator_ConnectorClosedEvent(object sender, INConnectorDetail connector)
        {
            AddIOLog(connector, GetLanguage("Txt5"), GetLanguage("Txt6"));
        }

        private void mAllocator_ConnectorConnectedEvent(object sender, INConnectorDetail connector)
        {
            string typeName = connector.GetTypeName();
            mAllocator.GetConnector(connector).AddRequestHandle(mObserver);

            AddIOLog(connector, GetLanguage("Txt7"),  GetLanguage("Txt8"));
        }

        /// <summary>
        /// 处理读卡消息事务
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="EventData"></param>
        private void MAllocator_TransactionMessage(INConnectorDetail connector, INData EventData)
        {
            if (EventData is USBDrive.USBDriveTransaction)
            {
                USBDriveTransaction USBtr = EventData as USBDriveTransaction;
                WatchReadCardTransaction cardTr = USBtr.EventData as WatchReadCardTransaction;

                ListViewItem oItem = new ListViewItem();
                string sLog = $"{ GetLanguage("Txt9")}：{cardTr.Card}(0x{cardTr.Card:X16})";

                oItem.Text = GetLanguage("Txt10");
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
            AddCmdLog(e, GetLanguage("Txt11"));
        }

        private void mAllocator_CommandTimeout(object sender, CommandEventArgs e)
        {
            //if (e.Command.GetType().FullName == typeof(FC8800.SystemParameter.SearchControltor.SearchControltor).FullName)
            //{
            //    AddCmdLog(e, "搜索完毕");
            //    return;
            //}
            AddCmdLog(e, GetLanguage("Txt12"));
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


            string sLog = $"{sName} { GetLanguage("Txt13")}：{time:0},{ GetLanguage("Txt14")}：{cmd.getProcessStep()} / {cmd.getProcessMax()}";
            mCommandProcessLog = sLog;
        }

        private void mAllocator_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            AddCmdLog(e, GetLanguage("Txt15"));
        }

        private const string Command_ReadSN = "DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.SN.ReadSN";
        private const string Command_WriteSN = "DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.SN.WriteSN";

        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            mAllocator_CommandProcessEvent(sender, e);
            AddCmdLog(e, GetLanguage("Txt16"));
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

        private void LoadSetting()
        {
            var languages = ConfigurationManager.AppSettings[$"Languages"].Split(',');
            cmbToolLanguage.Items.AddRange(languages);
            cmbToolLanguage.Text = ConfigurationManager.AppSettings[$"DefaultLanguage"];
            if (int.TryParse(ConfigurationManager.AppSettings[$"SerialPort"], out int serialPort))
            {
                if (cmbSerialPort.Items.Count >= serialPort)
                    cmbSerialPort.SelectedIndex = serialPort;
            }
            txtAddress.Text = ConfigurationManager.AppSettings[$"Address"];
        }
        private void SaveSetting()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SetConfigKey(config, "DefaultLanguage", cmbToolLanguage.Text);
            SetConfigKey(config, "SerialPort", cmbSerialPort.SelectedIndex.ToString());
            SetConfigKey(config, "Address", txtAddress.Text);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private void SetConfigKey(Configuration config, string key, string sValue)
        {
            var kv = config.AppSettings.Settings;
            if (kv[key] == null)
                kv.Add(key, sValue);
            else
                kv[key].Value = sValue;
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
        /// <summary>
        /// 多语言设置
        /// </summary>
        private void LoadUILanguage()
        {
            Text = GetLanguage("FormCaption") + "  Ver " + Application.ProductVersion;
            GetLanguage(butSystem);//  系统参数     
            GetLanguage(butTime);
            GetLanguage(butPatrol);
            GetLanguage(butRecord);
            GetLanguage(btnWatch);
            GetLanguage(groupBox1);
            GetLanguage(label1);
            GetLanguage(lblReLoadCOMList);
            GetLanguage(label2);
            GetLanguage(tabPage5);
            GetLanguage(tabPage6);
            GetLanguage(butClear);
            GetLanguage(chkShowIO);
            GetLanguage(butClearCommand);
            //   GetLanguage(lstIO);
            InilstCommand();
            IniLstIO();
            IniCommandClassNameList();
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
            var sCaptions = GetLanguage("lstCommand").SplitTrim(",");//"类型,内容,身份信息,串口信息,时间,耗时"
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
            string ret = $"{GetLanguage("Txt17")}：{cType} {GetLanguage("Txt18")}：{Local}";

            switch (conn.GetTypeName())
            {
                case ConnectorType.UDPServer:
                    ret = $"{GetLanguage("Txt17")}：{cType}  {GetLanguage("Txt19")}：{Local}";
                    break;
                case ConnectorType.TCPServer:
                    ret = $"{GetLanguage("Txt17")}：{cType} {GetLanguage("Txt19")}：{Local}";
                    break;
                case ConnectorType.SerialPort:
                    ret = $"{GetLanguage("Txt17")}：{cType} {Local}";
                    break;
                default:
                    ret = $"{GetLanguage("Txt17")}：{cType} {Local}";
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
                    cType = GetLanguage("Txt20");
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
        /// <returns>命令详情</returns>
        public USBDriveCommandDetail GetCommandDetail()
        {
            if (_IsClosed) return null;
            USBDriveCommandDetail cmdDtl = CommandDetailFactory.CreateDetail(CommandDetailFactory.ConnectType.SerialPort, "", GetSerialPort(),
                CommandDetailFactory.ControllerType.USBDrive_OfflinePatrol, txtAddress.Text, string.Empty) as USBDriveCommandDetail;
            DoNetDrive.Core.Connector.SerialPort.SerialPortDetail spd = cmdDtl.Connector as DoNetDrive.Core.Connector.SerialPort.SerialPortDetail;
            spd.Baudrate = 115200;

            return cmdDtl;
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
            var sCaptions = GetLanguage("lstIO").SplitTrim(",");//"标签,内容,串口信息,时间"
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

        private void ButTime_Click(object sender, EventArgs e)
        {
            frmTime frm = frmTime.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void ButPatrol_Click(object sender, EventArgs e)
        {
            frmPatrolEmpl frm = frmPatrolEmpl.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void ButRecord_Click(object sender, EventArgs e)
        {
            frmRecord frm = frmRecord.GetForm(this);
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



            cmdDtl.Timeout = 15000;
            cmdDtl.RestartCount = 0;
            OpenReadFlag cmd = new OpenReadFlag(cmdDtl);
            AddCommand(cmd);

            INConnector cnt = mAllocator.GetConnector(cmdDtl.Connector);
            if (cnt == null)
            {
                //未开启监控
                mAllocator.OpenConnector(cmdDtl.Connector);
                cnt = mAllocator.GetConnector(cmdDtl.Connector);

            }

            //使通道保持连接不关闭
            cnt.OpenForciblyConnect();
            USBCommandObserverHandler usbRequest =
                new USBCommandObserverHandler(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
            cnt.RemoveRequestHandle(typeof(USBDriveRequestHandle));//先删除，防止已存在就无法添加。
            cnt.AddRequestHandle(usbRequest);
        }

        /// <summary>
        /// 用于根据SN，命令参数、命令索引生产用于处理对应消息的处理类工厂函数
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="cmdIndex"></param>
        /// <returns></returns>
        private AbstractTransaction RequestHandleFactory(byte addr, byte cmdIndex)
        {
            return new WatchReadCardTransaction();
        }

        private void cmbToolLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLanguage();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetting();
        }
    }
}
