using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Connector.TCPClient;
using DoNetDrive.Core.Connector.TCPServer.Client;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Core.Extension;
using DoNetDrive.Protocol;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.SystemParameter.SN;
using DoNetDrive.Protocol.POS.SystemParameter.Watch;
using DoNetDrive.Protocol.POS.Test.Model;
using DoNetDrive.Protocol.POS.Transaction;
using DoNetDrive.Protocol.Transaction;
using DotNetDrive.Protocol.POS.Test.Model;
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

namespace DotNetDrive.Protocol.POS.Test
{
    public partial class FrmMain : Form, INMain
    {
        ConnectorAllocator mAllocator;
        ConnectorObserverHandler mObserver;
        private static HashSet<Form> NodeForms;
        private static string[] TransactionTypeName;

        static FrmMain()
        {
            NodeForms = new HashSet<Form>();
            IniCommandClassNameList();

        }

        bool _IsClosed;

        /// <summary>
        /// 保存命令类型的功能名称
        /// </summary>
        private static Dictionary<string, string> mCommandClasss;

        public FrmMain()
        {
            InitializeComponent();
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

        public void Invoke<T>(Action<T> act, T par)
        {
            try
            {
                this.Invoke((Delegate)act, par);
            }
            catch (Exception)
            {
                return;
            }
        }

        public static void AddNodeForms(Form frm)
        {
            if (!NodeForms.Contains(frm))
            {
                NodeForms.Add(frm);
            }
        }

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
            mObserver.DisposeResponseEvent += MObserver_DisposeResponseEvent;

            IniConnTypeList();
            IniLstIO();
            InilstCommand();

            Task.Run((Action)ShowCommandProcesslog);

            butUDPBind_Click(null, null);
        }

        /// <summary>
        /// 监控消息
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="EventData"></param>
        private void MAllocator_TransactionMessage(INConnectorDetail connector, DoNetDrive.Core.Data.INData EventData)
        {
            CommandResult commandResult = new CommandResult();

            if (_IsClosed) return;

           
        }

        /// <summary>
        /// 初始化命令类型的功能名称
        /// </summary>
        private static void IniCommandClassNameList()
        {
            mCommandClasss = new Dictionary<string, string>();

            mCommandClasss.Add(typeof(ReadSN).FullName, "读取SN");
            mCommandClasss.Add(typeof(WriteSN).FullName, "写SN");
            mCommandClasss.Add(typeof(WriteSN_Broadcast).FullName, "广播写SN");

            mCommandClasss.Add(typeof(ReadConnectPassword).FullName, "获取通讯密码");
            mCommandClasss.Add(typeof(WriteConnectPassword).FullName, "设置通讯密码");
            mCommandClasss.Add(typeof(ResetConnectPassword).FullName, "重置通讯密码");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.TCPSetting.ReadTCPSetting).FullName, "读取TCP参数");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.TCPSetting.WriteTCPSetting).FullName, "写入TCP参数");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Deadline.ReadDeadline).FullName, "读取设备有效期");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Deadline.WriteDeadline).FullName, "写入设备有效期");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Version.ReadVersion).FullName, "读取设备版本号");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.SystemStatus.ReadSystemStatus).FullName, "读取设备运行信息");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.ReadName).FullName, "读取屏幕显示内容名称");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.WriteName).FullName, "写入屏幕显示内容名称");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.ReadTitle).FullName, "读取屏幕显示内容标题");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.WriteTitle).FullName, "写入屏幕显示内容标题");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.ReadMessage).FullName, "读取屏幕显示内容短消息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.WriteMessage).FullName, "写入屏幕显示内容短消息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.ReadLogo).FullName, "读取屏幕显示内容开机供应商Logo");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.WriteLogo).FullName, "写入屏幕显示内容开机供应商Logo");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.ReadDisplayContent).FullName, "读取屏幕显示内容消费时显示内容");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.WriteDisplayContent).FullName, "写入屏幕显示内容消费时显示内容");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.ReadLed).FullName, "读取屏幕显示内容背光灯");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay.WriteLed).FullName, "写入屏幕显示内容背光灯");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.RecordStorageMode.ReadRecordStorageMode).FullName, "读取记录存储方式");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.RecordStorageMode.WriteRecordStorageMode).FullName, "写入记录存储方式");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.USBDisk.ReadUSBDisk).FullName, "读取U盘");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.USBDisk.WriteUSBDisk).FullName, "写入U盘");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ReceiptPrint.ReadReceiptPrint).FullName, "读取小票打印");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ReceiptPrint.WriteReceiptPrint).FullName, "写入小票打印");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Voice.ReadVoice).FullName, "读取语音");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Voice.WriteVoice).FullName, "写入语音");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Voice.ReadVoiceStart).FullName, "读取开机语音及广告语音");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Voice.WriteVoiceStart).FullName, "写入开机语音及广告语音");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Relay.ReadRelay).FullName, "读取继电器");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Relay.WriteRelay).FullName, "写入继电器");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Watch.BeginWatch).FullName, "开启监控");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Watch.CloseWatch).FullName, "关闭监控");


            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Initialize.Initialize).FullName, "初始化设备");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Cache.ReadCache).FullName, "读取缓存区内容");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Cache.WriteCache).FullName, "写入缓存区内容");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Buzzer.ReadBuzzer).FullName, "读取主板蜂鸣器");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.Buzzer.WriteBuzzer).FullName, "写入主板蜂鸣器");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.WIFIAccount.ReadWIFIAccount).FullName, "读取WIFI账号及密码");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.WIFIAccount.WriteWIFIAccount).FullName, "写入WIFI账号及密码");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ReaderIntervalTime.ReadReaderIntervalTime).FullName, "读取读卡间隔");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ReaderIntervalTime.WriteReaderIntervalTime).FullName, "写入读卡间隔");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ReaderByte.ReadReaderByte).FullName, "读取读卡器字节");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ReaderByte.WriteReaderByte).FullName, "写入读卡器字节");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ForbiddenMifareOne.ReadForbiddenMifareOne).FullName, "读取禁用Mifare One 卡");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.ForbiddenMifareOne.WriteForbiddenMifareOne).FullName, "写入禁用Mifare One 卡");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.OffLineSubsidyLimit.ReadOffLineSubsidyLimit).FullName, "读取离线补贴充值，卡内补贴余额上限");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.SystemParameter.OffLineSubsidyLimit.WriteOffLineSubsidyLimit).FullName, "写入离线补贴充值，卡内补贴余额上限");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Time.ReadTime).FullName, "读系统时间");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Time.WriteTime).FullName, "写系统时间");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Time.TimeErrorCorrection.ReadTimeError).FullName, "读取误差自修正参数");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Time.TimeErrorCorrection.WriteTimeError).FullName, "写入误差自修正参数");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.POSWorkMode.ReadPOSWorkMode).FullName, "读取消费机工作模式");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.POSWorkMode.WritePOSWorkMode).FullName, "写入消费机工作模式");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.FixedFeeRule.ReadFixedFeeRule).FullName, "读取定额扣费规则");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.FixedFeeRule.WriteFixedFeeRule).FullName, "写入定额扣费规则");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.ConsumptionLimits.ReadConsumptionLimits).FullName, "读取消费机消费限额");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.ConsumptionLimits.WriteConsumptionLimits).FullName, "写入消费机消费限额");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.ConsumePassword.ReadConsumePassword).FullName, "读取消费时密码确认");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.ConsumePassword.WriteConsumePassword).FullName, "写入消费时密码确认");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.TemporaryChangeFixedFee.ReadTemporaryChangeFixedFee).FullName, "读取临时变更定额、定次消费额度");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.TemporaryChangeFixedFee.WriteTemporaryChangeFixedFee).FullName, "写入临时变更定额、定次消费额度");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.CancelConsume.ReadCancelConsume).FullName, "读取撤销消费");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.CancelConsume.WriteCancelConsume).FullName, "写入撤销消费");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.ICCardAccount.ReadICCardAccount).FullName, "读取IC卡账户");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.ICCardAccount.WriteICCardAccount).FullName, "写入IC卡账户");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.AdditionalCharges.ReadAdditionalCharges).FullName, "读取附加费用");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.AdditionalCharges.WriteAdditionalCharges).FullName, "写入附加费用");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.Discount.ReadDiscount).FullName, "读取折扣");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.Discount.WriteDiscount).FullName, "写入折扣");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.Integral.ReadIntegral).FullName, "读取积分");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.Integral.WriteIntegral).FullName, "写入积分");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.CountingCards.ReadCountingCards).FullName, "读取计次");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.ConsumeParameter.CountingCards.WriteCountingCards).FullName, "写入计次");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.TimeGroup.ClearTimeGroup).FullName, "清空所有开门时段");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.TimeGroup.ReadTimeGroup).FullName, "读取所有开门时段");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.TimeGroup.AddTimeGroup).FullName, "添加开门时段");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Card.ReadCardDataBase).FullName, "读取卡号名单信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Card.ClearCardDataBase).FullName, "清空所有名单命令");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Card.ReadAllCard).FullName, "读取所有名单");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Card.ReadCardDetail).FullName, "读取单个名单");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Card.AddCard).FullName, "添加名单");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Card.DeleteCard).FullName, "删除名单");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.CardType.ReadDatabaseDetail).FullName, "读取卡类信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.CardType.ClearDataBase).FullName, "清空所有卡类信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.CardType.ReadDataBase).FullName, "读取所有卡类信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.CardType.ReadCardTypeDetail).FullName, "读取单个卡类");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.CardType.AddCardType).FullName, "添加卡类");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.CardType.DeleteCardType).FullName, "删除卡类");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Menu.ReadMenuDataBase).FullName, "读取菜单信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Menu.ClearMenuDataBase).FullName, "清空所有菜单");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Menu.ReadAllMenu).FullName, "读取所有菜单");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Menu.ReadMenuDetail).FullName, "读取单个菜单");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Menu.AddMenu).FullName, "添加菜单");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Menu.DeleteMenu).FullName, "删除菜单");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Subsidy.ReadSubsidyDataBase).FullName, "读取补贴容量信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Subsidy.ClearSubsidy).FullName, "清空所有补贴");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Subsidy.ReadAllSubsidy).FullName, "读取所有补贴");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Subsidy.ReadSubsidyDetail).FullName, "读取单个补贴");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Subsidy.AddSussidy).FullName, "添加补贴");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Reservation.ReadDatabaseDetail).FullName, "读取订餐容量信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Reservation.ClearDataBase).FullName, "清空所有订餐");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Reservation.ReadDataBase).FullName, "读取所有订餐");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Reservation.AddReservationDetail).FullName, "添加订餐");

            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Transaction.ReadTransactionDatabaseDetail).FullName, "读取控制器中的卡片数据库信息");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Transaction.ClearTransactionDatabase).FullName, "清空指定类型的记录数据库");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Transaction.TransactionDatabaseEmpty).FullName, "清空所有类型的记录数据库");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Transaction.ReadTransactionDatabaseByIndex).FullName, "按指定序号读记录");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Transaction.ReadTransactionDatabase).FullName, "读取新记录");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Transaction.WriteTransactionDatabaseEndIndex).FullName, "更新记录尾号");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Transaction.WriteTransactionDatabaseIndex).FullName, "更新记录指针");
            mCommandClasss.Add(typeof(DoNetDrive.Protocol.POS.Transaction.WriteTransactionDatabaseStartIndex).FullName, "更新记录起始号");
        }

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


            UDPServerDetail detail = new UDPServerDetail(sLocalIP, port);
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
            cmdConnType.Items.AddRange("TCP,UDP".SplitTrim(","));
            cmdConnType.SelectedIndex = 0;
            ShowConnTypePanel();


            _IsClosed = false;

            int iTop = gbTCP.Top, iLeft = gbTCP.Left;
            gbUDP.Top = iTop; gbUDP.Left = iLeft;


            IniLoadLocalIP();

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
                cmbLocalIP.SelectedIndex = cmbLocalIP.Items.Count - 1;
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
            bool[] pnlShow = new bool[2];
            pnlShow[cmdConnType.SelectedIndex] = true;

            GroupBox[] pnls = new GroupBox[] { gbTCP, gbUDP };

            for (int i = 0; i < 2; i++)
            {
                pnls[i].Visible = pnlShow[i];
            }
        }

        #endregion


        #region 通道事件
        /// <summary>
        /// 客户端离线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MAllocator_ClientOffline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            inc.RemoveRequestHandle(typeof(ConnectorObserverHandler));
            //inc.RemoveRequestHandle(typeof(Door8800RequestHandle));
            switch (inc.GetConnectorType())
            {
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
                case ConnectorType.UDPClient://UDP客户端已连接

                    //inc.OpenForciblyConnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                ();
                    //Door8800RequestHandle fC8800Request =
                    //    new Door8800RequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
                    //inc.RemoveRequestHandle(typeof(Door8800RequestHandle));//先删除，防止已存在就无法添加。
                    //inc.AddRequestHandle(fC8800Request);

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

        private void mAllocator_ConnectorErrorEvent(object sender, INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(false));
                    AddIOLog(connector, "UDP绑定", "UDP绑定失败");
                    break;
                default:
                    AddIOLog(connector, "错误", "连接失败");
                    break;
            }
        }

        private void mAllocator_ConnectorClosedEvent(object sender, INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(false));
                    AddIOLog(connector, "UDP绑定", "UDP绑定已关闭");
                    break;
                default:
                    AddIOLog(connector, "关闭", "连接通道已关闭");
                    break;
            }
        }

        private void mAllocator_ConnectorConnectedEvent(object sender, INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(true));
                    AddIOLog(connector, "UDP绑定", "UDP绑定成功");
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


        private const string Command_ReadSN = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.ReadSN";
        private const string Command_WriteSN = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.WriteSN";
        private const string Command_WriteSN_Broadcast = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.WriteSN_Broadcast";
        private const string Command_ReadConnectPassword = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.ReadConnectPassword";
        private const string Command_WriteConnectPassword = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.WriteConnectPassword";
        private const string Command_ResetConnectPassword = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.ResetConnectPassword";

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
            var cmddtl = e.CommandDetail;
            switch (cName)
            {
                case Command_ReadSN://读SN

                    DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.SN_Result sn = e.Command.getResult() as DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.SN_Result;
                    Invoke(() => txtSN.Text = sn.SNBuf.GetString());
                    if (cmddtl.UserData != null)
                    {
                        string tmpStr = cmddtl.UserData as string;
                        if (tmpStr != null && tmpStr == "AutoReadSN")
                        {
                            DoNetDrive.Protocol.OnlineAccess.OnlineAccessCommandDetail ocd = cmddtl as DoNetDrive.Protocol.OnlineAccess.OnlineAccessCommandDetail;
                            ocd.SN = sn.SNBuf.GetString();
                            ReadConnectPassword cmd = new ReadConnectPassword(cmddtl);
                            AddCommand(cmd);
                        }

                    }
                    break;

                case Command_ReadConnectPassword://读通讯密码
                    DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Result pwd = e.Command.getResult() as DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Result;
                    Invoke(() => txtPassword.Text = pwd.Password);
                    break;
                case Command_WriteConnectPassword://写通讯密码
                    DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Parameter pwdPar = e.Command.Parameter as DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Parameter;
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
                //case ConnectorType.SerialPort:
                //    cType = "串口";
                //    var com = conn as Connector.SerialPort.SerialPortDetail;
                //    Local = $"COM{local.Port}:{com.Baudrate}";
                //    break;
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
        public DESDriveCommandDetail GetCommandDetail()
        {
            if (_IsClosed) return null;
            string addr = string.Empty, sn, password;
            int port = 0;
            INConnectorDetail conn = null;
            switch (cmdConnType.SelectedIndex)//串口,TCP客户端,UDP,TCP服务器
            {
                case 0://TCP
                    
                    addr = txtTCPAddr.Text;
                    if (!int.TryParse(txtTCPPort.Text, out port))
                    {
                        port = 8000;
                    }
                    conn = new TCPClientDetail(addr, port);
                    break;

                case 1://UDP 
                    if (!mUDPIsBind)
                    {
                        MsgErr("请先绑定UDP端口");
                        return null;
                    }
                    addr = txtUDPAddr.Text;
                    if (!int.TryParse(txtUDPPort.Text, out port))
                    {
                        port = 8000;
                    }
                    conn = new UDPClientDetail(addr, port);
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
                password = "3030303030303030";
            }
            if (password.Length != 16)
            {
                password = "3030303030303030";
            }

            DESDriveCommandDetail cmdDtl = new DESDriveCommandDetail(conn, sn, password);

            //var cmdDtl = CommandDetailFactory.CreateDetail(connectType, addr, port,
            //    protocolType, sn, password);

            if (cmdConnType.SelectedIndex == 1)
            {
                UDPClientDetail dtl = cmdDtl.Connector as UDPClientDetail;
                dtl.LocalAddr = cmbLocalIP.Text;
                dtl.LocalPort = txtUDPLocalPort.Text.ToInt32();
            }
            cmdDtl.Timeout = 1200;
            cmdDtl.RestartCount = 2;
            return cmdDtl;

        }

        private OnlineAccessCommandDetail SearchCommandDetail()
        {
            string sDestIP = "255.255.255.255";
            string sSearchSN = "0000000000000000", sPassword = "FFFFFFFF";
            int iDrivePort = mDrivePort;

            string sLocalIP = cmbLocalIP.SelectedItem?.ToString();
            var oUDPDtl = new UDPClientDetail(sDestIP, iDrivePort,
                sLocalIP, int.Parse(txtUDPLocalPort.Text));

            var dtl = new OnlineAccessCommandDetail(oUDPDtl, sSearchSN, sPassword);
            dtl.Timeout = 2000;
            dtl.RestartCount = 3;
            dtl.UserData = null;
            return dtl;
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
                DESDriveCommandDetail fcDtl = cmdDtl as DESDriveCommandDetail;
                commandResult.SN = fcDtl.SN;
                commandResult.Remote = Remote;
                commandResult.Time = DateTime.Now.ToTimeffff();
                commandResult.Timemill = Timemill.ToString("0");
            }
            else
            {
                commandResult.Title = "-";
                commandResult.Content = txt;
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

        private void butReadSN_Click(object sender, EventArgs e)
        {
            var cmdDtl = GetCommandDetail();
            if (cmdDtl == null) return;

            cmdDtl.UserData = "AutoReadSN";
            ReadSN cmd = new ReadSN(cmdDtl);
            AddCommand(cmd);

            cmdDtl.CommandTimeout += CmdDtl_CommandTimeout;
            cmdDtl.CommandCompleteEvent += CmdDtl_CommandCompleteEvent;
            cmdDtl.CommandErrorEvent += CmdDtl_CommandErrorEvent;
        }

        private void CmdDtl_CommandErrorEvent(object sender, CommandEventArgs e)
        {
        }

        private void CmdDtl_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            SN_Result sn = e.Command.getResult() as SN_Result;
            Invoke(() => txtSN.Text = sn.SNBuf.GetString());
        }

        private void CmdDtl_CommandTimeout(object sender, CommandEventArgs e)
        {
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

        private void butConsumeParameter_Click(object sender, EventArgs e)
        {
            frmConsumeParameter frm = frmConsumeParameter.GetForm(this);
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

        private void butMenu_Click(object sender, EventArgs e)
        {

            FrmMenu frm = FrmMenu.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butCardType_Click(object sender, EventArgs e)
        {
            FrmCardType frm = FrmCardType.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butCard_Click(object sender, EventArgs e)
        {
            FrmCard frm = FrmCard.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butRecord_Click(object sender, EventArgs e)
        {
            FrmRecord frm = FrmRecord.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butSubsidy_Click(object sender, EventArgs e)
        {
            FrmSubsidy frm = FrmSubsidy.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butReservation_Click(object sender, EventArgs e)
        {
            FrmReservation frm = FrmReservation.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butWatch_Click(object sender, EventArgs e)
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
                AddCmdLog(cmde, "已开启监控");
            };

            //使通道保持连接不关闭
            cnt.OpenForciblyConnect();
            DESRequestHandle posRequest =
                new DESRequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
            cnt.RemoveRequestHandle(typeof(DESRequestHandle));//先删除，防止已存在就无法添加。
            cnt.AddRequestHandle(posRequest);
        }

        /// <summary>
        /// 用于根据SN，命令参数、命令索引生产用于处理对应消息的处理类工厂函数
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="cmdIndex"></param>
        /// <param name="cmdPar"></param>
        /// <returns></returns>
        private AbstractTransaction RequestHandleFactory(string sn, byte cmdIndex, byte cmdPar)
        {

            //在这里需要根据SN进行类型判定，也可以根据SN来进行查表
            if (cmdIndex >= 1 && cmdIndex <= 2)
            {
                return ReadTransactionDatabaseByIndex.NewTransactionTable[cmdIndex]();
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

        #region 产生随机数
        /// <summary>
        /// 随机数产生器
        /// </summary>
        public static Random CodeRandom = new Random();

        /// <summary>
        /// 获得一个随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandomNum() => CodeRandom.Next(1, 65530);

        #endregion

        #region 设备搜索
        private int mDrivePort = 8101;

        /// <summary>
        /// 准备退出
        /// </summary>
        private bool mStop;

        List<string> mSNList;

        /// <summary>
        /// 自动搜索的随机数
        /// </summary>
        private int mSearchID;
        private void butSearch_Click(object sender, EventArgs e)
        {
            mSearchID = GetRandomNum();
            mStop = false;
            mSNList = new List<string>();
            dgDevice.Rows.Clear();
            BeginSearchDrive();
        }

        private void BeginSearchDrive()
        {
            if (this.InvokeRequired)
            {
                Invoke(BeginSearchDrive);
                return;
            }
            try
            {
                mDrivePort = int.Parse(txtUDPPort.Text);
            }
            catch (Exception)
            {

                mDrivePort = 8101;
                txtUDPLocalPort.Text = "8101";
            }
            if (mDrivePort < 0 || mDrivePort > 65535)
            {
                mDrivePort = 8101;
                txtUDPLocalPort.Text = "8101";
            }

            var cmdDtl = SearchCommandDetail();
            cmdDtl.Timeout = 2000;
            cmdDtl.RestartCount = 0;
            cmdDtl.UserData = mSearchID;
            var searchPar = new SearchControltor_Parameter((ushort)mSearchID);
            searchPar.UDPBroadcast = true;

            var searchCmd = new SearchControltor(cmdDtl, searchPar);
            mAllocator.AddCommand(searchCmd);
            cmdDtl.CommandCompleteEvent += search_CommandCompleteEvent;
            cmdDtl.CommandTimeout += search_CommandTimeout;
            cmdDtl.CommandErrorEvent += search_CommandErrorEvent;
        }

        private void search_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            //MessageBox.Show("命令错误");
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            if (mStop) return;
            Task.Run(BeginSearchDrive);
        }

        private void search_CommandTimeout(object sender, CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            //搜索结束;
            if (mStop) return;
            Task.Run(BeginSearchDrive);
        }



        private void search_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            if (mStop) return;

            SearchControltor_Result rst = e.Result as SearchControltor_Result;

            Task.Run(() =>
            {
                Invoke(AddSearchDrive, rst);
            });
        }

        private void AddSearchDrive(SearchControltor_Result oDriveResult)
        {
            if (oDriveResult == null)
                return;
            if (this.InvokeRequired)
            {
                Invoke(AddSearchDrive, oDriveResult);
                return;
            }
            if (oDriveResult == null)
                return;
            var oTCP = oDriveResult.TCP;
            string sSearchSN = oDriveResult.SN;
            if (sSearchSN.Length != 16) return;

            string sSNFormat = sSearchSN.Substring(0, 8);


            #region 将网络代码写入到设备
            var cmdDtl = SearchCommandDetail();
            var connDTL = cmdDtl.Connector as UDPClientDetail;
            cmdDtl.SN = sSearchSN;
            connDTL.Port = oTCP.mUDPPort;
            var searchPar = new SearchControltor_Parameter((ushort)mSearchID);
            searchPar.UDPBroadcast = true;

            var searchCmd = new WriteControltorNetCode(cmdDtl, searchPar);
            mAllocator.AddCommand(searchCmd);
            #endregion

            if (!mSNList.Contains(sSearchSN))
            {
                mSNList.Add(sSearchSN);
                DeviceInfo deviceInfo = new DeviceInfo(sSearchSN, oTCP.mIP, "");

                AddDeviceItem(deviceInfo);
            }

        }

        private void AddDeviceItem(DeviceInfo deviceInfo)
        {
            if (dgDevice.InvokeRequired)
            {
                Invoke(() => AddDeviceItem(deviceInfo));
                return;
            }

            dgDevice.Rows.Insert(0, dgDevice.Rows.Count + 1, deviceInfo.SN, deviceInfo.IP, deviceInfo.MAC);
        }

        private void dgDevice_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int col = e.ColumnIndex, row = e.RowIndex;
            if (row < 0) return;
            var gdRow = dgDevice.Rows[row];
            txtSN.Text = gdRow.Cells[1].Value.ToString();
        }
        #endregion

        private void butStopCommand_Click(object sender, EventArgs e)
        {
            mStop = true;
            var cmdDtl = GetCommandDetail();
            if (cmdDtl == null) return;
            mAllocator.StopCommand(cmdDtl);
        }
    }
}
