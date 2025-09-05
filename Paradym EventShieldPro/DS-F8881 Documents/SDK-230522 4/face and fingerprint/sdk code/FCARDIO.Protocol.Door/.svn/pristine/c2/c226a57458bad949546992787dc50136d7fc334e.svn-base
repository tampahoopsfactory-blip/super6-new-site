using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting;
using System.Text.RegularExpressions;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.Deadline;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.Version;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SystemStatus;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter;
using System.Collections;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.Watch;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.FireAlarm;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SmogAlarm;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.Alarm;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.WorkStatus;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.Controller;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.CacheContent;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.KeepAliveInterval;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.TheftFortify;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.BalcklistAlarmOption;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.ExploreLockMode;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.Check485Line;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPClient;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.CardDeadlineTipDay;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.ControlPanelTamperAlarm;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.HTTPPageLandingSwitch;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.LawfulCardReleaseAlarmSwitch;

namespace DoNetDrive.Protocol.Door.Test
{

    public partial class frmSystem : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static frmSystem onlyObj;
        public static frmSystem GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmSystem(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private frmSystem(INMain main) : base(main)
        {
            InitializeComponent();
        }
        #endregion

        private void frmSystem_Load(object sender, EventArgs e)
        {
            LoadUILanguage();

        }

        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            //设备参数设置
            GetLanguage(tabSysParameterPage);
            var read = GetLanguage("Read");
            var write = GetLanguage("Write");
            GetLanguage(butWriteSN_Broadcast);
            GetLanguage(gbPassword);
            GetLanguage(Lab_ConnectPassword);
            GetLanguage(butResetConnectPassword);
            GetLanguage(gbRunParameters);
            GetLanguage(Lab_Deadline);
            GetLanguage(Lab_DeanlineDay);
            GetLanguage(Lab_Version);
            GetLanguage(gbTCP);
            GetLanguage(Lab_MAC);
            GetLanguage(Lab_Ip);
            GetLanguage(Lab_IPMask);
            GetLanguage(Lab_IPGateway);
            GetLanguage(Lab_DNSBackup);
            GetLanguage(Lab_ProtocolType);
            GetLanguage(Lab_TCPPort);
            GetLanguage(Lab_UDPPort);
            GetLanguage(Lab_ServerPort);
            GetLanguage(Lab_ServerIP);
            GetLanguage(Lab_AutoIP);
            GetLanguage(Lab_ServerAddr);
            GetLanguage(gpDeviceRunInfo);
            GetLanguage(Lab_RunDay);
            GetLanguage(Lab_RestartCount);
            GetLanguage(Lab_Temperature);
            GetLanguage(Lab_StartTime);
            GetLanguage(Lab_FormatCount);
            GetLanguage(Lab_UPS);
            GetLanguage(Lab_Voltage);
            GetLanguage(gpWatch);
            GetLanguage(Lab_WatchStatus);
            //   GetLanguage(lbWatchState_1);
            // GetLanguage(lbWatchState_2);
            lbWatchState.Text = GetLanguage("lbWatchState_1");
            GetLanguage(btnBeginWatch);
            GetLanguage(btnCloseWatch);
            GetLanguage(btnReadWatchState);
            GetLanguage(btnBeginWatchBroadcast);
            GetLanguage(btnCloseWatchBroadcast);
            GetLanguage(gp_Cache);
            GetLanguage(Lab_CacheContent);
            GetLanguage(gpTcpClientModel);
            GetLanguage(Lab_KeepAliveInterval1);
            GetLanguage(Lab_KeepAliveInterval2);
            GetLanguage(btnInitalData);
            GetLanguage(btnSearchEquptOnNetNum);
            GetLanguage(btnCloseTypeAlarm);

            butReadSN.Text = read;
            butReadConnectPassword.Text = read;
            btnReadVersion.Text = read;
            btnReadDeadline.Text = read;
            btnReadSystemStatus.Text = read;
            butRendTCPSetting.Text = read;
            btnReadCacheContent.Text = read;
            btnReadKeepAliveInterval.Text = read;

            butWriteSN.Text = write;
            butWriteConnectPassword.Text = write;
            btnWriteDeadline.Text = write;
            butWriteTCPSetting.Text = write;
            btnWriteCacheContent.Text = write;
            btnWriteKeepAliveInterval.Text = write;
            //扩展功能
            GetLanguage(tabPage3);
            GetLanguage(groupBox13);
            GetLanguage(groupBox14);
            GetLanguage(groupBox15);
            GetLanguage(groupBox17);
            GetLanguage(Lab_ardDeadlineTipDay);
            GetLanguage(groupBox18);
            GetLanguage(groupBox19);
            GetLanguage(groupBox20);
            var nonuse = GetLanguage("nonuse");
            var use = GetLanguage("use");
            label72.Text = GetLanguage("Lab_DeanlineDay");
            rBtnNoBalcklistAlarm.Text = nonuse;
            rBtnNoExploreLockMode.Text = nonuse;
            rBtnNoCheck485Line.Text = nonuse;
            rBtnNoControlPanelTamperAlarm.Text = nonuse;
            rBtnNoHTTPPageLandingSwitch.Text = nonuse;
            rBtnNoLawfulCardReleaseAlarmSwitch.Text = nonuse;

            rBtnBalcklistAlarm.Text = use;
            rBtnExploreLockMode.Text = use;
            rBtnCheck485Line.Text = use;
            rBtnControlPanelTamperAlarm.Text = use;
            rBtnHTTPPageLandingSwitch.Text = use;
            rBtnLawfulCardReleaseAlarmSwitch.Text = use;

            btnReadBalcklistAlarmOption.Text = read;
            btnReadExploreLockMode.Text = read;
            btnReadCheck485Line.Text = read;
            btnReadCardDeadlineTipDay.Text = read;
            btnReadrBtnControlPanelTamperAlarm.Text = read;
            btnReadHTTPPageLandingSwitch.Text = read;
            btnReadLawfulCardReleaseAlarmSwitch.Text = read;

            btnWriteBalcklistAlarmOption.Text = write;
            btnWriteExploreLockMode.Text = write;
            btnWriteCheck485Line.Text = write;
            btnWriteCardDeadlineTipDay.Text = write;
            btnWriterBtnControlPanelTamperAlarm.Text = write;
            btnWriteHTTPPageLandingSwitch.Text = write;
            btnWriteLawfulCardReleaseAlarmSwitch.Text = write;

            //设备状态信息
            GetLanguage(tabPage1);
            GetLanguage(groupBox9);
            GetLanguage(dgvEquipmentStatusInfo);
            GetLanguage(label66);
            GetLanguage(label65);
            GetLanguage(btnWorkStatusInfo);
            GetLanguage(groupBox16);
            GetLanguage(dgvTCPClientList);
            GetLanguage(btnReadTCPClientList);
            GetLanguage(btnStopTCPClientConnection);
            GetLanguage(btnStopAllTCPClientConnection);
            lbSWatchState.Text = GetLanguage("lbWatchState_1");
            //功能参数

            GetLanguage(tabPage2);
            GetLanguage(groupBox3);
            GetLanguage(label20);
            GetLanguage(rBtnCover);
            GetLanguage(rBtnNoCover);
            GetLanguage(label21);
            GetLanguage(label22);
            GetLanguage(label23);
            GetLanguage(label24);
            GetLanguage(label25);
            GetLanguage(label34);
            GetLanguage(label33);
            GetLanguage(label35);
            GetLanguage(label36);
            //  GetLanguage(Alarm);
            //  GetLanguage(CloseAlarm);
            // GetLanguage(AlarmState);
            GetLanguage(groupBox4);
            GetLanguage(label42);
            GetLanguage(label37);
            //  GetLanguage(Enter);
            //  GetLanguage(Limit);
            GetLanguage(groupBox5);
            GetLanguage(label47);
            GetLanguage(label52);
            //   GetLanguage(seconds);
            GetLanguage(label51);
            GetLanguage(label55);
            GetLanguage(label50);
            GetLanguage(label49);
            GetLanguage(label64);
            GetLanguage(btnSetTheftFortify);
            GetLanguage(btnSetTheftDisarming);
            GetLanguage(groupBox6);
            GetLanguage(label56);
            GetLanguage(rBtnOneCheck);
            GetLanguage(rBtnAllCheck);
            GetLanguage(label57);
            GetLanguage(groupBox7);
            GetLanguage(label58);
            GetLanguage(label59);
            GetLanguage(rBtnPayRent);
            GetLanguage(rBtnPayManagementFee);
            GetLanguage(label61);
            GetLanguage(label60);
            GetLanguage(btnAllRead);
            GetLanguage(rBtnEnableValidation);

            //读取
            btnReadRecordMode.Text = read;
            btnReadKeyboard.Text = read;
            btnReadLockInteraction.Text = read;
            btnReadFireAlarmOption.Text = read;
            btnReadOpenAlarmOption.Text = read;
            btnReadIntervalTime.Text = read;
            btnReadBroadcast.Text = read;
            btnReadReaderCheckMode.Text = read;
            btnReadBuzzer.Text = read;
            btnReadSmogAlarmOption.Text = read;
            btnReadEnterDoorLimit.Text = read;
            btnReadTheftAlarmSetting.Text = read;
            btnReadCheckInOut.Text = read;
            btnReadCardPeriodSpeak.Text = read;
            btnReadReadCardSpeak.Text = read;
            //写入
            btnWriteRecordMode.Text = write;
            btnWriteKeyboard.Text = write;
            btnWirteLockInteraction.Text = write;
            btnWriteFireAlarmOption.Text = write;
            btnWriteOpenAlarmOption.Text = write;
            btnWriteIntervalTime.Text = write;
            btnWriteBroadcast.Text = write;
            btnWriteReaderCheckMode.Text = write;
            btnWriteBuzzer.Text = write;
            btnWriteSmogAlarmOption.Text = write;
            btnWriteEnterDoorLimit.Text = write;
            btnWriteTheftAlarmSetting.Text = write;
            btnWriteCheckInOut.Text = write;
            btnWriteCardPeriodSpeak.Text = write;
            btnWriteReadCardSpeak.Text = write;

            var policeType = GetLanguage("cbxPoliceType").Split(',');

            cbxPoliceType.Items.Clear();
            cbxPoliceType.Items.AddRange(policeType);

            var smogAlarmOption = GetLanguage("cbxSmogAlarmOption").Split(',');

            cbxSmogAlarmOption.Items.Clear();
            cbxSmogAlarmOption.Items.AddRange(smogAlarmOption);

            var optionArr = GetLanguage("cbxOption").Split(',');

            cbxOption.Items.Clear();
            cbxOption.Items.AddRange(optionArr);



            var protocolType = GetLanguage("cbxProtocolType").Split(',');

            cbxProtocolType.Items.Clear();
            cbxProtocolType.Items.AddRange(protocolType);

            var autoIP = GetLanguage("cbxAutoIP").Split(',');
            cbxAutoIP.Items.Clear();
            cbxAutoIP.Items.AddRange(autoIP);

            var door = GetLanguage("Door");
            cBoxDoor1.Text = door + 1;
            cBoxDoor2.Text = door + 2;
            cBoxDoor3.Text = door + 3;
            cBoxDoor4.Text = door + 4;

            var alarm = GetLanguage("Alarm");
            var closeAlarm = GetLanguage("CloseAlarm");
            var alarmState = GetLanguage("AlarmState");
            btnAlarm.Text = alarm;
            btnSmogAlarm.Text = alarm;

            btnCloseAlarm.Text = closeAlarm;
            btnCloseSmogAlarm.Text = closeAlarm;

            btnAlarmState.Text = alarmState;
            btnSmogAlarmState.Text = alarmState;

            rBtnNoEnable.Text = nonuse;
            rBtnNoBuzzer.Text = nonuse;
            rBtnNoTheft.Text = nonuse;
            rBtnNoPeriodSpeak.Text = nonuse;
            rBtnNoEnableReadCardSpeak.Text = nonuse;

            rBtnEnable.Text = use;
            rBtnBuzzer.Text = use;
            rBtnTheft.Text = use;
            rBtnPeriodSpeak.Text = use;
            rBtnEnableReadCardSpeak.Text = use;

            var seconds = GetLanguage("seconds");

            label32.Text = seconds;
            label53.Text = seconds;
            label54.Text = seconds;
            label48.Text = seconds;

            var enter = GetLanguage("Enter");
            var limit = GetLanguage("Limit");

            label45.Text = 2 + enter;
            label46.Text = 1 + enter;
            label44.Text = 3 + enter;
            label43.Text = 4 + enter;

            label41.Text = 1 + limit;
            label40.Text = 2 + limit;
            label39.Text = 3 + limit;
            label38.Text = 4 + limit;



            #region 解除报警
            DataGridViewCheckBoxColumn ck = new DataGridViewCheckBoxColumn();
            this.dgvAlarmType.Columns.Add(ck);
            this.dgvAlarmType.Columns[0].HeaderText = GetLanguage("dgvAlarmType1");
            this.dgvAlarmType.Columns[0].Width = 38;
            this.dgvAlarmType.Rows.Add(13);
            this.dgvAlarmType.Columns.Add("", GetLanguage("dgvAlarmType2"));
            this.dgvAlarmType.Columns[1].Width = 200;
            this.dgvAlarmType.Rows[0].Cells[1].Value = GetLanguage("dgvAlarmType3");
            this.dgvAlarmType.Rows[1].Cells[1].Value = GetLanguage("dgvAlarmType4");
            this.dgvAlarmType.Rows[2].Cells[1].Value = GetLanguage("dgvAlarmType5");
            this.dgvAlarmType.Rows[3].Cells[1].Value = GetLanguage("dgvAlarmType6");
            this.dgvAlarmType.Rows[4].Cells[1].Value = GetLanguage("dgvAlarmType7");
            this.dgvAlarmType.Rows[5].Cells[1].Value = GetLanguage("dgvAlarmType8");
            this.dgvAlarmType.Rows[6].Cells[1].Value = GetLanguage("dgvAlarmType9");
            this.dgvAlarmType.Rows[7].Cells[1].Value = GetLanguage("dgvAlarmType10");
            this.dgvAlarmType.Rows[8].Cells[1].Value = GetLanguage("dgvAlarmType11");
            this.dgvAlarmType.Rows[9].Cells[1].Value = GetLanguage("dgvAlarmType12");
            this.dgvAlarmType.Rows[10].Cells[1].Value = GetLanguage("dgvAlarmType13");
            this.dgvAlarmType.Rows[11].Cells[1].Value = GetLanguage("dgvAlarmType14");
            this.dgvAlarmType.Rows[12].Cells[1].Value = GetLanguage("dgvAlarmType15");

            this.dgvAlarmType.AllowUserToAddRows = false;
            for (int i = 0; i < this.dgvAlarmType.Rows.Count; i++)
            {
                this.dgvAlarmType.Rows[i].Cells[0].Value = true;
            }
            #endregion

            #region 获取设备状态信息
            this.dgvEquipmentStatusInfo.Rows.Add(9);
            this.dgvEquipmentStatusInfo.Rows[0].Cells[0].Value = door + "1";
            this.dgvEquipmentStatusInfo.Rows[1].Cells[0].Value = door + "2";
            this.dgvEquipmentStatusInfo.Rows[2].Cells[0].Value = door + "3";
            this.dgvEquipmentStatusInfo.Rows[3].Cells[0].Value = door + "4";
            this.dgvEquipmentStatusInfo.Rows[4].Cells[0].Value = GetLanguage("dgvAlarmType10"); //"消防报警";
            this.dgvEquipmentStatusInfo.Rows[5].Cells[0].Value = GetLanguage("dgvAlarmType8");// "匪警报警";
            this.dgvEquipmentStatusInfo.Rows[6].Cells[0].Value = GetLanguage("dgvAlarmType11");// "烟雾报警";
            this.dgvEquipmentStatusInfo.Rows[7].Cells[0].Value = GetLanguage("dgvAlarmType9");// "防盗主机";
            this.dgvEquipmentStatusInfo.Rows[8].Cells[0].Value = GetLanguage("dgvAlarmType13"); //"防拆报警";
            this.dgvEquipmentStatusInfo.AllowUserToAddRows = false;
            #endregion

            cbxCloseAlarmDoor.Items.Clear();
            cbxCloseAlarmDoor.Items.Add(GetLanguage("CloseAlarmDoor"));
            for (int i = 1; i < 4; i++)
            {
                cbxCloseAlarmDoor.Items.Add(door + i);
            }
            cbxCloseAlarmDoor.SelectedIndex = 0;

            Cmb_FireAlarmOption.Items.Clear();
            var fireAlarmOption = "禁用,报警输出，并开所有门，只能软件解除,报警输出，不开所有门，只能软件解除,有信号报警并开门，无信号解除报警并关门,有报警信号时开一次门，就像按钮开门一样";
            Cmb_FireAlarmOption.Items.AddRange(fireAlarmOption.Split(','));
        }

        #region SN的读写操作

        private void butReadSN_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadSN cmd = new ReadSN(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                SN_Result result = cmde.Command.getResult() as SN_Result;
                string sn = result.SNBuf.GetString();
                Invoke(() =>
                {
                    txtSN.Text = sn;
                });
                mMainForm.AddCmdLog(cmde, sn);
            };
        }

        public void ReadSN()
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadSN cmd = new ReadSN(cmdDtl);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                SN_Result result = cmde.Command.getResult() as SN_Result;
                string sn = result.SNBuf.GetString();

                mMainForm.AddCmdLog(cmde, sn);
            };
        }

        private bool CheckSN()
        {
            string sn = txtSN.Text;
            if (sn.Length != 16)
            {
                MsgErr($"{GetLanguage("Msg1")}！");
                return false;
            }
            int len = System.Text.Encoding.ASCII.GetByteCount(sn);
            if (len != 16)
            {
                MsgErr($"{GetLanguage("Msg1")}！");
                return false;
            }
            return true;
        }

        private void butWriteSN_Click(object sender, EventArgs e)
        {
            if (!CheckSN()) return;
            string sn = txtSN.Text;
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteSN cmd = new WriteSN(cmdDtl, new SN_Parameter(sn));
            mMainForm.AddCommand(cmd);


        }

        private void butWriteSN_Broadcast_Click(object sender, EventArgs e)
        {
            if (!CheckSN()) return;
            string sn = txtSN.Text;
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteSN_Broadcast cmd = new WriteSN_Broadcast(cmdDtl, new SN_Parameter(sn));
            mMainForm.AddCommand(cmd);

        }
        #endregion

        #region 通讯密码
        private void butReadConnectPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadConnectPassword cmd = new ReadConnectPassword(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                Password_Result result = cmde.Command.getResult() as Password_Result;
                string pwd = result.Password;
                Invoke(() =>
                {
                    txtConnectPassword.Text = pwd;
                });
                mMainForm.AddCmdLog(cmde, pwd);
            };
        }

        public void ReadPassword()
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadConnectPassword cmd = new ReadConnectPassword(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                Password_Result result = cmde.Command.getResult() as Password_Result;
                string pwd = result.Password;

                mMainForm.AddCmdLog(cmde, pwd);
            };
        }

        private void butWriteConnectPassword_Click(object sender, EventArgs e)
        {
            string pwd = txtConnectPassword.Text;
            if (pwd.Length != 8)
            {
                MsgErr($"{GetLanguage("Msg2")}！");
                return;
            }
            if (!pwd.IsHex())
            {
                MsgErr($"{GetLanguage("Msg2")}！");
                return;
            }


            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteConnectPassword cmd = new WriteConnectPassword(cmdDtl, new Password_Parameter(pwd));
            mMainForm.AddCommand(cmd);


        }

        private void butResetConnectPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ResetConnectPassword cmd = new ResetConnectPassword(cmdDtl);
            mMainForm.AddCommand(cmd);

        }
        #endregion

        #region TCP参数
        private void ButRendTCPSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTCPSetting cmd = new ReadTCPSetting(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTCPSetting_Result result = cmde.Command.getResult() as ReadTCPSetting_Result;

                Invoke(() =>
                {
                    txtMAC.Text = result.TCP.mMAC;
                    txtIP.Text = result.TCP.mIP;
                    txtIPMask.Text = result.TCP.mIPMask;
                    txtIPGateway.Text = result.TCP.mIPGateway;
                    txtDNS.Text = result.TCP.mDNS;
                    txtDNSBackup.Text = result.TCP.mDNSBackup;
                    txtTCPPort.Text = result.TCP.mTCPPort.ToString();
                    txtUDPPort.Text = result.TCP.mUDPPort.ToString();
                    txtServerIP.Text = result.TCP.mServerIP;
                    txtServerAddr.Text = result.TCP.mServerAddr;
                    txtServerPort.Text = result.TCP.mServerPort.ToString();

                    cbxProtocolType.SelectedIndex = result.TCP.mProtocolType;
                    cbxAutoIP.SelectedIndex = result.TCP.mAutoIP == true ? 1 : 0;
                });
                string TCPInfo = DebugTCPDetail(result.TCP);
                mMainForm.AddCmdLog(cmde, TCPInfo);
            };
        }

        private string DebugTCPDetail(TCPDetail tcp)
        {
            string MAC = tcp.mMAC; //MAC地址
            string IP = tcp.mIP; //IP
            string IPMask = tcp.mIPMask; //子网掩码
            string IPGateway = tcp.mIPGateway; //网关地址
            string DNS = tcp.mDNS; //DNS
            string DNSBackup = tcp.mDNSBackup; //备用DNS
            string TCPPort = tcp.mTCPPort.ToString(); //本地TCP端口
            string UDPPort = tcp.mUDPPort.ToString(); //本地UDP端口
            string ServerIP = tcp.mServerIP; //服务器IP
            string ServerAddr = tcp.mServerAddr; //服务器域名
            string ServerPort = tcp.mServerPort.ToString(); //服务器端口

            int ProtocolType = tcp.mProtocolType; //TCP工作模式
            bool AutoIP = tcp.mAutoIP; //是否自动获得IP


            string TCPInfo = GetLanguage("Lab_MAC") + MAC +
                             "  IP：" + IP +
                             GetLanguage("Lab_IPMask") + IPMask +
                             GetLanguage("Lab_IPGateway") + IPGateway +
                             "  DNS：" + DNS +
                             GetLanguage("Lab_DNSBackup") + DNSBackup +
                             GetLanguage("Lab_TCPPort") + TCPPort +
                             GetLanguage("Lab_UDPPort") + UDPPort +
                             GetLanguage("Lab_ServerIP") + ServerIP +
                             GetLanguage("Lab_ServerAddr") + ServerAddr +
                             GetLanguage("Lab_ServerPort") + ServerPort;
            return TCPInfo;
        }

        private void ButWriteTCPSetting_Click(object sender, EventArgs e)
        {
            string reg = @"([A-Fa-f0-9]{2}-){5}[A-Fa-f0-9]{2}";
            string reg2 = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            string reg3 = @"^\+?[1-9][0-9]*$";
            string reg4 = @"^(?=^.{3,255}$)[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+$";

            if (!Regex.IsMatch(txtMAC.Text.Trim(), reg))
            {
                MsgErr($"{GetLanguage("Msg3")}！");
                return;
            }

            if (!Regex.IsMatch(txtIP.Text.Trim(), reg2))
            {
                MsgErr($"{GetLanguage("Msg4")}！");
                return;
            }
            if (!Regex.IsMatch(txtIPMask.Text.Trim(), reg2))
            {
                MsgErr($"{GetLanguage("Msg5")}！");
                return;
            }
            if (!Regex.IsMatch(txtIPGateway.Text.Trim(), reg2))
            {
                MsgErr($"{GetLanguage("Msg6")}！");
                return;
            }
            if (!Regex.IsMatch(txtDNS.Text.Trim(), reg2))
            {
                MsgErr($"{GetLanguage("Msg7")}！");
                return;
            }
            if (!Regex.IsMatch(txtDNSBackup.Text.Trim(), reg2))
            {
                MsgErr($"{GetLanguage("Msg8")}！");
                return;
            }
            if (!Regex.IsMatch(txtServerIP.Text.Trim(), reg2))
            {
                MsgErr($"{GetLanguage("Msg9")}！");
                return;
            }
            if (!Regex.IsMatch(txtTCPPort.Text.Trim(), reg3))
            {
                MsgErr($"{GetLanguage("Msg10")}！");
                return;
            }
            if (Convert.ToInt32(txtTCPPort.Text.Trim()) > 65535)
            {
                MsgErr($"{GetLanguage("Msg10")}！");
                return;
            }
            if (!Regex.IsMatch(txtUDPPort.Text.Trim(), reg3))
            {
                MsgErr($"{GetLanguage("Msg21")}！");
                return;
            }
            if (Convert.ToInt32(txtUDPPort.Text.Trim()) > 65535)
            {
                MsgErr($"{GetLanguage("Msg21")}！");
                return;
            }
            if (!Regex.IsMatch(txtServerPort.Text.Trim(), reg3))
            {
                MsgErr($"{GetLanguage("Msg22")}！");
                return;
            }
            if (Convert.ToInt32(txtServerPort.Text.Trim()) > 65535)
            {
                MsgErr($"{GetLanguage("Msg22")}！");
                return;
            }
            //if (!Regex.IsMatch(txtServerAddr.Text.Trim(), reg4))
            //{
            //    MsgErr("请输入正确服务器域名！");
            //    return;
            //}
            if (Convert.ToInt16(cbxProtocolType.SelectedIndex) == 0)
            {
                MsgErr($"{GetLanguage("Msg23")}！");
                return;
            }
            if (Convert.ToInt16(cbxAutoIP.SelectedIndex) == -1)
            {
                MsgErr($"{GetLanguage("Msg24")}！");
                return;
            }

            TCPDetail tcp = new TCPDetail();
            tcp.mIP = txtIP.Text.Trim();
            tcp.mMAC = txtMAC.Text.Trim();
            tcp.mIPMask = txtIPMask.Text.Trim();
            tcp.mIPGateway = txtIPGateway.Text.Trim();
            tcp.mDNS = txtDNS.Text.Trim();
            tcp.mDNSBackup = txtDNSBackup.Text.Trim();
            tcp.mTCPPort = Convert.ToInt32(txtTCPPort.Text.Trim());
            tcp.mUDPPort = Convert.ToInt32(txtUDPPort.Text.Trim());
            tcp.mServerIP = txtServerIP.Text.Trim();
            tcp.mServerAddr = txtServerAddr.Text.Trim();
            tcp.mServerPort = Convert.ToInt32(txtServerPort.Text.Trim());

            tcp.mProtocolType = Convert.ToUInt16(cbxProtocolType.SelectedIndex);

            if (cbxAutoIP.SelectedIndex == 1)
            {
                tcp.mAutoIP = true;
            }
            else
            {
                tcp.mAutoIP = false;
            }

            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x89);
            //tcp.GetBytes(buf);

            //TCPDetail newtcp = new TCPDetail();
            //newtcp.SetBytes(buf);

            //string TCPInfo = "MAC地址:" + newtcp.mMAC +
            //                    "  IP：" + newtcp.mIP +
            //                    "  子网掩码：" + newtcp.mIPMask +
            //                    "  网关地址：" + newtcp.mIPGateway +
            //                    "  DNS：" + newtcp.mDNS +
            //                    "  备用DNS：" + newtcp.mDNSBackup +
            //                    "  本地TCP端口：" + newtcp.mTCPPort +
            //                    "  本地UDP端口：" + newtcp.mUDPPort +
            //                    "  服务器IP：" + newtcp.mServerIP +
            //                    "  服务器域名：" + newtcp.mServerAddr +
            //                    "  TCP工作模式：" + newtcp.mProtocolType +
            //                    "  自动获得IP：" + newtcp.mAutoIP +
            //                    "  服务器端口：" + newtcp.mServerPort;
            //mMainForm.AddCmdLog(null, TCPInfo);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteTCPSetting cmd = new WriteTCPSetting(cmdDtl, new WriteTCPSetting_Parameter(tcp));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 设备有效期
        private void BtnReadDeadline_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadDeadline cmd = new ReadDeadline(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadDeadline_Result result = cmde.Command.getResult() as ReadDeadline_Result;

                ushort Deadline = result.Deadline; //有效期
                string DeadlineInfo = string.Empty;
                if (Deadline == 0)
                {
                    DeadlineInfo = GetLanguage("Msg25");
                }
                else if (Deadline == 65535)
                {
                    DeadlineInfo = GetLanguage("Msg26");
                }
                else
                {
                    DeadlineInfo = Deadline.ToString() + GetLanguage("Msg27"); ;
                }

                Invoke(() =>
                {
                    cbxDeadline.Text = DeadlineInfo.Replace(GetLanguage("Msg27"), "");
                });
                string DeadlineDay = GetLanguage("Msg28") + ":" + DeadlineInfo;
                mMainForm.AddCmdLog(cmde, DeadlineDay);
            };
        }

        private void BtnWriteDeadline_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxDeadline.Text.Trim(), reg))
            {
                if (cbxDeadline.Text != GetLanguage("Msg26") && cbxDeadline.Text != GetLanguage("Msg25"))
                {
                    MsgErr(GetLanguage("Msg29") + "！");
                    return;
                }
            }
            if (Regex.IsMatch(cbxDeadline.Text.Trim(), reg))
            {
                if (Convert.ToUInt32(cbxDeadline.Text) < 0 || Convert.ToUInt32(cbxDeadline.Text) > 65535)
                {
                    MsgErr(GetLanguage("Msg29") + "！");
                    return;
                }
            }

            ushort deadlineDay = 0;
            string deadlineInfo = cbxDeadline.Text;
            if (deadlineInfo == GetLanguage("Msg26"))
            {
                deadlineDay = 65535;
            }
            else if (deadlineInfo == GetLanguage("Msg27"))
            {
                deadlineDay = 0;
            }
            else
            {
                deadlineDay = Convert.ToUInt16(cbxDeadline.Text);
            }

            //WriteDeadline_Parameter wp = new WriteDeadline_Parameter();
            //wp.Deadline = deadlineDay;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x02);
            //wp.GetBytes(buf);

            //WriteDeadline_Parameter wp2 = new WriteDeadline_Parameter();
            //wp2.SetBytes(buf);

            //string DeadlineDay = "设备剩余有效期天数:" + deadlineDay;
            //mMainForm.AddCmdLog(null, DeadlineDay);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteDeadline cmd = new WriteDeadline(cmdDtl, new WriteDeadline_Parameter(deadlineDay));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 设备版本号
        private void BtnReadVersion_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadVersion cmd = new ReadVersion(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadVersion_Result result = cmde.Command.getResult() as ReadVersion_Result;
                string version = result.Version.ToString();
                Invoke(() =>
                {
                    txtVersion.Text = "Ver " + version;
                });
                version = GetLanguage("Msg20") + version;
                mMainForm.AddCmdLog(cmde, version);
            };
        }
        #endregion

        #region 设备运行信息
        private void BtnReadSystemStatus_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadSystemStatus cmd = new ReadSystemStatus(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadSystemStatus_Result result = cmde.Command.getResult() as ReadSystemStatus_Result;
                string RunDay = result.RunDay.ToString() + GetLanguage("Msg27"); //设备已运行天数
                string FormatCount = result.FormatCount.ToString() + GetLanguage("Msg30"); //格式化次数
                string RestartCount = result.RestartCount.ToString() + GetLanguage("Msg30"); //看门狗复位次数
                string UPS = result.UPS == 0 ? GetLanguage("Msg31") : GetLanguage("Msg32"); //UPS工作状态
                string Temperature = result.Temperature; //设备温度
                string Voltage = result.Voltage; //接入电压
                string StartTime = result.StartTime; //上电时间

                Invoke(() =>
                {
                    txtRunDay.Text = RunDay;
                    txtFormatCount.Text = FormatCount;
                    txtRestartCount.Text = RestartCount;
                    txtUPS.Text = UPS;
                    txtTemperature.Text = Temperature;
                    txtVoltage.Text = Voltage;
                    txtStartTime.Text = StartTime;

                });
                string TCPInfo = GetLanguage("Lab_RunDay") + RunDay +
                                 GetLanguage("Lab_FormatCount") + FormatCount +
                                 GetLanguage("Lab_RestartCount") + RestartCount +
                                GetLanguage("Lab_UPS") + UPS +
                                 GetLanguage("Lab_Temperature") + Temperature +
                                 GetLanguage("Lab_Voltage") + Voltage +
                                GetLanguage("Lab_StartTime") + StartTime;
                mMainForm.AddCmdLog(cmde, TCPInfo);
            };
        }
        #endregion

        #region 记录存储方式
        private void BtnReadRecordMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadRecordMode cmd = new ReadRecordMode(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadRecordMode_Result result = cmde.Command.getResult() as ReadRecordMode_Result;
                string ModeStr = result.Mode == 0 ? GetLanguage("Msg33") : GetLanguage("Msg34"); //记录存储方式
                Invoke(() =>
                {
                    if (result.Mode == 0)
                    {
                        rBtnCover.Checked = true;
                    }
                    else
                    {
                        rBtnNoCover.Checked = true;
                    }
                });
                ModeStr = GetLanguage("Msg12") + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteRecordMode_Click(object sender, EventArgs e)
        {
            byte mode = 0;
            if (rBtnNoCover.Checked == true)
            {
                mode = 1;
            }

            //WriteRecordMode_Parameter wp = new WriteRecordMode_Parameter();
            //wp.Mode = mode;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x02);
            //wp.GetBytes(buf);

            //WriteRecordMode_Parameter wp2 = new WriteRecordMode_Parameter();
            //wp2.SetBytes(buf);

            //string DeadlineDay = "记录存储方式:" + mode;
            //mMainForm.AddCmdLog(null, DeadlineDay);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteRecordMode cmd = new WriteRecordMode(cmdDtl, new WriteRecordMode_Parameter(mode));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 读卡器密码键盘启用功能开关
        private void BtnReadKeyboard_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadKeyboard cmd = new ReadKeyboard(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadKeyboard_Result result = cmde.Command.getResult() as ReadKeyboard_Result;
                string KeyboardInfo = string.Empty;
                Invoke(() =>
                {
                    for (int i = 0; i < result.Keyboard.Count; i++)
                    {
                        if (result.Keyboard[i] == true)
                        {
                            if (i == 0)
                            {
                                cBox1.Checked = true;
                            }
                            else if (i == 1)
                            {
                                cBox2.Checked = true;
                            }
                            else if (i == 2)
                            {
                                cBox3.Checked = true;
                            }
                            else if (i == 3)
                            {
                                cBox4.Checked = true;
                            }
                            else if (i == 4)
                            {
                                cBox5.Checked = true;
                            }
                            else if (i == 5)
                            {
                                cBox6.Checked = true;
                            }
                            else if (i == 6)
                            {
                                cBox7.Checked = true;
                            }
                            else if (i == 7)
                            {
                                cBox8.Checked = true;
                            }

                            KeyboardInfo = KeyboardInfo + $"  { GetLanguage("Msg35") }：" + (i + 1) + "，" + GetLanguage("Msg36");
                        }
                        else
                        {
                            KeyboardInfo = KeyboardInfo + $"  { GetLanguage("Msg35") }：" + (i + 1) + "，" + GetLanguage("Msg37");
                        }
                    }
                });
                mMainForm.AddCmdLog(cmde, KeyboardInfo);
            };
        }

        private void BtnWriteKeyboard_Click(object sender, EventArgs e)
        {
            BitArray bitSet = new BitArray(8);
            bitSet[0] = cBox1.Checked;
            bitSet[1] = cBox2.Checked;
            bitSet[2] = cBox3.Checked;
            bitSet[3] = cBox4.Checked;
            bitSet[4] = cBox5.Checked;
            bitSet[5] = cBox6.Checked;
            bitSet[6] = cBox7.Checked;
            bitSet[7] = cBox8.Checked;

            //WriteKeyboard_Parameter wp = new WriteKeyboard_Parameter();
            //wp.Keyboard = bitSet;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x02);
            //wp.GetBytes(buf);

            //WriteKeyboard_Parameter wp2 = new WriteKeyboard_Parameter();
            //wp2.SetBytes(buf);

            //string DeadlineDay = "键盘开关1:" + bitSet[0] +
            //                     "  键盘开关2:" + bitSet[1] +
            //                     "  键盘开关3:" + bitSet[2] +
            //                     "  键盘开关4:" + bitSet[3] +
            //                     "  键盘开关5:" + bitSet[4] +
            //                     "  键盘开关6:" + bitSet[5] +
            //                     "  键盘开关7:" + bitSet[6] +
            //                     "  键盘开关8:" + bitSet[7];
            //mMainForm.AddCmdLog(null, DeadlineDay);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteKeyboard cmd = new WriteKeyboard(cmdDtl, new WriteKeyboard_Parameter(bitSet));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 互锁参数
        private void BtnReadLockInteraction_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadLockInteraction cmd = new ReadLockInteraction(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadLockInteraction_Result result = cmde.Command.getResult() as ReadLockInteraction_Result;
                string DoorLockInfo = string.Empty;
                Invoke(() =>
                {
                    for (int i = 0; i < result.DoorPort.DoorMax; i++)
                    {
                        if (result.DoorPort.DoorPort[i] == 1)
                        {
                            if (i == 0)
                            {
                                cBoxDoor1.Checked = true;
                            }
                            else if (i == 1)
                            {
                                cBoxDoor2.Checked = true;
                            }
                            else if (i == 2)
                            {
                                cBoxDoor3.Checked = true;
                            }
                            else if (i == 3)
                            {
                                cBoxDoor4.Checked = true;
                            }

                            DoorLockInfo = DoorLockInfo + "  " + GetLanguage("Door") + (i + 1) + " " + GetLanguage("Msg38");
                        }
                        if (string.IsNullOrWhiteSpace(DoorLockInfo))
                        {
                            DoorLockInfo = GetLanguage("Msg39");
                        }
                    }
                });
                mMainForm.AddCmdLog(cmde, DoorLockInfo);
            };
        }

        private void BtnWirteLockInteraction_Click(object sender, EventArgs e)
        {
            DoorPortDetail dpd = new DoorPortDetail(4);
            if (cBoxDoor1.Checked)
            {
                dpd.DoorPort[0] = 1;
            }
            else
            {
                dpd.DoorPort[0] = 0;
            }
            if (cBoxDoor2.Checked)
            {
                dpd.DoorPort[1] = 1;
            }
            else
            {
                dpd.DoorPort[1] = 0;
            }
            if (cBoxDoor3.Checked)
            {
                dpd.DoorPort[2] = 1;
            }
            else
            {
                dpd.DoorPort[2] = 0;
            }
            if (cBoxDoor4.Checked)
            {
                dpd.DoorPort[3] = 1;
            }
            else
            {
                dpd.DoorPort[3] = 0;
            }

            //WriteLockInteraction_Parameter wp = new WriteLockInteraction_Parameter();
            //wp.DoorPort = dpd;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x02);
            //wp.GetBytes(buf);

            //WriteLockInteraction_Parameter wp2 = new WriteLockInteraction_Parameter();
            //wp2.SetBytes(buf);

            //string DeadlineDay = "门1:" + dpd.DoorPort[0] +
            //                     "  门2:" + dpd.DoorPort[1] +
            //                     "  门3:" + dpd.DoorPort[2] +
            //                     "  门4:" + dpd.DoorPort[3];
            //mMainForm.AddCmdLog(null, DeadlineDay);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteLockInteraction cmd = new WriteLockInteraction(cmdDtl, new WriteLockInteraction_Parameter(dpd));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 消防报警
        private void BtnReadFireAlarmOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadFireAlarmOption cmd = new ReadFireAlarmOption(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadFireAlarmOption_Result result = cmde.Command.getResult() as ReadFireAlarmOption_Result;
                int OptionType = result.Option; //消防报警参数
                string OptionTypeStr = string.Empty;
                if (OptionType == 0)
                {
                    OptionTypeStr = GetLanguage("Msg40");
                }
                else if (OptionType == 1)
                {
                    OptionTypeStr = GetLanguage("Msg41");
                }
                else if (OptionType == 2)
                {
                    OptionTypeStr = GetLanguage("Msg42");
                }
                else if (OptionType == 3)
                {
                    OptionTypeStr = GetLanguage("Msg43");
                }
                else if (OptionType == 4)
                {
                    OptionTypeStr = GetLanguage("Msg44");
                }
                Invoke(() =>
                {
                    cbxOption.SelectedIndex = OptionType;
                });
                string Info = GetLanguage("Msg45") + "：" + OptionTypeStr;
                mMainForm.AddCmdLog(cmde, Info);
            };
        }

        private void BtnWriteFireAlarmOption_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(cbxOption.SelectedIndex) == -1)
            {
                MsgErr(GetLanguage("Msg46"));
                return;
            }
            byte Option = Convert.ToByte(cbxOption.SelectedIndex);

            //WriteFireAlarmOption_Parameter wp = new WriteFireAlarmOption_Parameter();
            //wp.Option = Option;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x01);
            //wp.GetBytes(buf);

            //WriteFireAlarmOption_Parameter wp2 = new WriteFireAlarmOption_Parameter();
            //wp2.SetBytes(buf);

            //string Info = "消防报警模式:" + Option;
            //mMainForm.AddCmdLog(null, Info);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteFireAlarmOption cmd = new WriteFireAlarmOption(cmdDtl, new WriteFireAlarmOption_Parameter(Option));
            mMainForm.AddCommand(cmd);
        }
        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            SendFireAlarm cmd = new SendFireAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("Msg47"));
            };
        }

        private void BtnCloseAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            CloseFireAlarm cmd = new CloseFireAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("Msg48"));
            };
        }

        private void BtnAlarmState_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadFireAlarmState cmd = new ReadFireAlarmState(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                string ModeStr = cmd.FireAlarmState == 0 ? GetLanguage("Msg49") : GetLanguage("Msg50"); //消防报警状态
                Invoke(() =>
                {
                    mMainForm.AddCmdLog(cmde, ModeStr);
                });
            };
        }
        #endregion

        #region 匪警报警
        private void BtnReadOpenAlarmOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadOpenAlarmOption cmd = new ReadOpenAlarmOption(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadOpenAlarmOption_Result result = cmde.Command.getResult() as ReadOpenAlarmOption_Result;
                int OptionType = result.Option; //匪警报警参数
                string OptionTypeStr = string.Empty;
                if (OptionType == 0)
                {
                    OptionTypeStr = GetLanguage("Msg51");
                }
                else if (OptionType == 1)
                {
                    OptionTypeStr = GetLanguage("Msg52");
                }
                else if (OptionType == 2)
                {
                    OptionTypeStr = GetLanguage("Msg53");
                }
                else if (OptionType == 3)
                {
                    OptionTypeStr = GetLanguage("Msg54");
                }
                Invoke(() =>
                {
                    cbxPoliceType.SelectedIndex = OptionType;
                });
                string Info = GetLanguage("Msg55") + "：" + OptionTypeStr;
                mMainForm.AddCmdLog(cmde, Info);
            };
        }

        private void BtnWriteOpenAlarmOption_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(cbxPoliceType.SelectedIndex) == -1)
            {
                MsgErr(GetLanguage("Msg56"));
                return;
            }
            byte Option = Convert.ToByte(cbxPoliceType.SelectedIndex);

            //WriteOpenAlarmOption_Parameter wp = new WriteOpenAlarmOption_Parameter();
            //wp.Option = Option;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x01);
            //wp.GetBytes(buf);

            //WriteOpenAlarmOption_Parameter wp2 = new WriteOpenAlarmOption_Parameter();
            //wp2.SetBytes(buf);

            //string Info = "匪警报警模式:" + Option;
            //mMainForm.AddCmdLog(null, Info);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteOpenAlarmOption cmd = new WriteOpenAlarmOption(cmdDtl, new WriteOpenAlarmOption_Parameter(Option));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 读卡间隔时间
        private void BtnReadIntervalTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReaderIntervalTime cmd = new ReadReaderIntervalTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadReaderIntervalTime_Result result = cmde.Command.getResult() as ReadReaderIntervalTime_Result;

                ushort IntervalTime = result.IntervalTime; //读卡间隔时间
                string IntervalTimeInfo = string.Empty;
                string time = GetLanguage("Msg13");
                if (IntervalTime == 0)
                {
                    IntervalTimeInfo = GetLanguage("Msg57");
                }
                else
                {
                    IntervalTimeInfo = IntervalTime.ToString() + time;
                }
                Invoke(() =>
                {
                    cbxIntervalTime.Text = IntervalTimeInfo.Replace(time, "");
                });
                string IntervalTimeStr = GetLanguage("Msg58") + "：" + IntervalTimeInfo;
                mMainForm.AddCmdLog(cmde, IntervalTimeStr);
            };
        }

        private void BtnWriteIntervalTime_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxIntervalTime.Text.Trim(), reg))
            {
                if (cbxIntervalTime.Text != GetLanguage("Msg57"))
                {
                    MsgErr(GetLanguage("Msg59") + "！");
                    return;
                }
            }
            if (Regex.IsMatch(cbxIntervalTime.Text.Trim(), reg))
            {
                if (Convert.ToUInt32(cbxIntervalTime.Text) < 0 || Convert.ToUInt32(cbxIntervalTime.Text) > 65535)
                {
                    MsgErr(GetLanguage("Msg59") + "！");
                    return;
                }
            }

            ushort IntervalTime = 0;
            string deadlineInfo = cbxIntervalTime.Text;
            if (deadlineInfo == GetLanguage("Msg57"))
            {
                IntervalTime = 0;
            }
            else
            {
                IntervalTime = Convert.ToUInt16(cbxIntervalTime.Text);
            }

            //WriteReaderIntervalTime_Parameter wp = new WriteReaderIntervalTime_Parameter();
            //wp.IntervalTime = IntervalTime;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x02);
            //wp.GetBytes(buf);

            //WriteReaderIntervalTime_Parameter wp2 = new WriteReaderIntervalTime_Parameter();
            //wp2.SetBytes(buf);

            //string DeadlineDay = "读卡间隔时间:" + IntervalTime;
            //mMainForm.AddCmdLog(null, DeadlineDay);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteReaderIntervalTime cmd = new WriteReaderIntervalTime(cmdDtl, new WriteReaderIntervalTime_Parameter(IntervalTime));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 语音播报语音段开关
        private void BtnReadBroadcast_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadBroadcast cmd = new ReadBroadcast(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadBroadcast_Result result = cmde.Command.getResult() as ReadBroadcast_Result;

                Invoke(() =>
                {
                    byte[] broadcast = new byte[4];
                    Array.Copy(result.Broadcast.Broadcast, 6, broadcast, 0, 4);
                    StringBuilder BroadcastInfo = new StringBuilder(64); //语音段开关

                    Array.Reverse(broadcast);
                    BitArray bit = new BitArray(broadcast);
                    for (int i = 30; i >= 0; i--)
                    {
                        BroadcastInfo.Append(bit[i] ? 1 : 0);
                    }

                    txtBroadcast.Text = BroadcastInfo.ToString();
                    string IntervalTimeStr = GetLanguage("Msg60") + "：" + txtBroadcast.Text + $"  {GetLanguage("Msg61")} 31←1";
                    mMainForm.AddCmdLog(cmde, IntervalTimeStr);
                });

            };
        }

        private void BtnWriteBroadcast_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-1]*$";
            if (!Regex.IsMatch(txtBroadcast.Text.Trim(), reg) || txtBroadcast.Text.Trim().Length != 31)
            {
                MsgErr(GetLanguage("Msg62") + "！");
                return;
            }
            byte[] bData = new byte[10];

            string strBit = txtBroadcast.Text.Trim();
            strBit = "0" + strBit;

            byte[] tmpData = new byte[4];
            BitArray bit = new BitArray(tmpData);
            int strIndex = 0;
            for (int i = 30; i >= 0; i--)
            {
                bit[i] = (strBit.Substring(++strIndex, 1) == "1");
            }

            bit.CopyTo(tmpData, 0);
            Array.Reverse(tmpData);
            Array.Copy(tmpData, 0, bData, 6, 4);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteBroadcast cmd = new WriteBroadcast(cmdDtl, new WriteBroadcast_Parameter(bData));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 读卡器数据校验
        private void BtnReadReaderCheckMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReaderCheckMode cmd = new ReadReaderCheckMode(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadReaderCheckMode_Result result = cmde.Command.getResult() as ReadReaderCheckMode_Result;
                string ModeStr = string.Empty; //读卡器数据校验
                Invoke(() =>
                {
                    if (result.ReaderCheckMode == 0)
                    {
                        rBtnNoEnable.Checked = true;
                        ModeStr = GetLanguage("Msg63");
                    }
                    else if (result.ReaderCheckMode == 1)
                    {
                        rBtnEnable.Checked = true;
                        ModeStr = GetLanguage("Msg64");
                    }
                    else
                    {
                        rBtnEnableValidation.Checked = true;
                        ModeStr = GetLanguage("Msg65");
                    }
                });
                ModeStr = GetLanguage("Msg66") + "：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteReaderCheckMode_Click(object sender, EventArgs e)
        {
            byte mode = 0;
            if (rBtnEnable.Checked == true)
            {
                mode = 1;
            }
            else if (rBtnEnableValidation.Checked == true)
            {
                mode = 2;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteReaderCheckMode cmd = new WriteReaderCheckMode(cmdDtl, new WriteReaderCheckMode_Parameter(mode));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 主板蜂鸣器
        private void BtnReadBuzzer_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadBuzzer cmd = new ReadBuzzer(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadBuzzer_Result result = cmde.Command.getResult() as ReadBuzzer_Result;
                string ModeStr = result.Buzzer == 0 ? GetLanguage("Msg97") : GetLanguage("Msg98"); //记录存储方式
                Invoke(() =>
                {
                    if (result.Buzzer == 0)
                    {
                        rBtnNoBuzzer.Checked = true;
                    }
                    else
                    {
                        rBtnBuzzer.Checked = true;
                    }
                });
                ModeStr = GetLanguage("Msg67") + "：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteBuzzer_Click(object sender, EventArgs e)
        {
            byte buzzer = 0;
            if (rBtnBuzzer.Checked == true)
            {
                buzzer = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteBuzzer cmd = new WriteBuzzer(cmdDtl, new WriteBuzzer_Parameter(buzzer));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 烟雾报警
        private void BtnReadSmogAlarmOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadSmogAlarmOption cmd = new ReadSmogAlarmOption(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadSmogAlarmOption_Result result = cmde.Command.getResult() as ReadSmogAlarmOption_Result;
                int OptionType = result.Option; //烟雾报警参数
                string OptionTypeStr = string.Empty;
                if (OptionType == 0)
                {
                    OptionTypeStr = GetLanguage("Msg68");
                }
                else if (OptionType == 1)
                {
                    OptionTypeStr = GetLanguage("Msg69");
                }
                else if (OptionType == 2)
                {
                    OptionTypeStr = GetLanguage("Msg70");
                }
                else if (OptionType == 3)
                {
                    OptionTypeStr = GetLanguage("Msg71");
                }
                Invoke(() =>
                {
                    cbxSmogAlarmOption.SelectedIndex = OptionType;
                });
                string Info = GetLanguage("Msg72") + "：" + OptionTypeStr;
                mMainForm.AddCmdLog(cmde, Info);
            };
        }

        private void BtnWriteSmogAlarmOption_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(cbxSmogAlarmOption.SelectedIndex) == -1)
            {
                MsgErr(GetLanguage("Msg73") + "！");
                return;
            }
            byte Option = Convert.ToByte(cbxSmogAlarmOption.SelectedIndex);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteSmogAlarmOption cmd = new WriteSmogAlarmOption(cmdDtl, new WriteSmogAlarmOption_Parameter(Option));
            mMainForm.AddCommand(cmd);
        }
        private void BtnSmogAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            SendSmogAlarm cmd = new SendSmogAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("Msg74"));
            };
        }

        private void BtnCloseSmogAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            CloseSmogAlarm cmd = new CloseSmogAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("Msg75"));
            };
        }

        private void BtnSmogAlarmState_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadSmogAlarmState cmd = new ReadSmogAlarmState(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                string ModeStr = cmd.SmogAlarmState == 0 ? GetLanguage("Msg76") : GetLanguage("Msg77"); //烟雾报警状态
                Invoke(() =>
                {
                    mMainForm.AddCmdLog(cmde, ModeStr);
                });
            };
        }
        #endregion

        #region 门内人数限制
        private void BtnReadEnterDoorLimit_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadEnterDoorLimit cmd = new ReadEnterDoorLimit(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadEnterDoorLimit_Result result = cmde.Command.getResult() as ReadEnterDoorLimit_Result;
                string GlobalLimit = result.Limit.GlobalLimit.ToString(); //全局上限
                string Door1Limit = result.Limit.DoorLimitArray[0].ToString(); //1号门上限
                string Door2Limit = result.Limit.DoorLimitArray[1].ToString(); //2号门上限
                string Door3Limit = result.Limit.DoorLimitArray[2].ToString(); //3号门上限
                string Door4Limit = result.Limit.DoorLimitArray[3].ToString(); //4号门上限

                string GlobalEnter = result.Limit.GlobalEnter.ToString(); //全局人数
                string Door1Enter = result.Limit.DoorEnterArray[0].ToString(); //1号门人数
                string Door2Enter = result.Limit.DoorEnterArray[1].ToString(); //2号门人数
                string Door3Enter = result.Limit.DoorEnterArray[2].ToString(); //3号门人数
                string Door4Enter = result.Limit.DoorEnterArray[3].ToString(); //4号门人数
                Invoke(() =>
                {
                    txtGlobalLimit.Text = GlobalLimit;
                    txtDoor1Limit.Text = Door1Limit;
                    txtDoor2Limit.Text = Door2Limit;
                    txtDoor3Limit.Text = Door3Limit;
                    txtDoor4Limit.Text = Door4Limit;

                    txtGlobalEnter.Text = GlobalEnter;
                    txtDoor1Enter.Text = Door1Enter;
                    txtDoor2Enter.Text = Door2Enter;
                    txtDoor3Enter.Text = Door3Enter;
                    txtDoor4Enter.Text = Door4Enter;

                });
                string EnterDoorLimitInfo = GetLanguage("label42") + GlobalLimit +
                                 $"  1{GetLanguage("Limit")}：" + Door1Limit +
                                 $"  2{GetLanguage("Limit")}：" + Door2Limit +
                                 $"  3{GetLanguage("Limit")}：" + Door3Limit +
                                 $"  4{GetLanguage("Limit")}：" + Door4Limit +
                                 GetLanguage("label37") + GlobalEnter +
                                 $"  1{GetLanguage("Enter")}：" + Door1Enter +
                                 $"  2{GetLanguage("Enter")}：" + Door2Enter +
                                 $"  3{GetLanguage("Enter")}：" + Door3Enter +
                                 $"  4{GetLanguage("Enter")}：" + Door4Enter;
                mMainForm.AddCmdLog(cmde, EnterDoorLimitInfo);
            };
        }

        private void BtnWriteEnterDoorLimit_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            UInt32 ui32 = 0;
            if (!Regex.IsMatch(txtGlobalLimit.Text.Trim(), reg))
            {
                MsgErr(GetLanguage("Msg78") + "！");
                return;
            }
            if (!UInt32.TryParse(txtGlobalLimit.Text.Trim(), out ui32))
            {
                MsgErr(GetLanguage("Msg79") + "！");
                return;
            }
            var Msg80 = GetLanguage("Msg80");
            if (!Regex.IsMatch(txtDoor1Limit.Text.Trim(), reg))
            {
                MsgErr(string.Format(Msg80, 1));
                return;
            }
            if (!UInt32.TryParse(txtDoor1Limit.Text.Trim(), out ui32))
            {
                MsgErr($"1{GetLanguage("Msg80")}！");
                return;
            }

            if (!Regex.IsMatch(txtDoor2Limit.Text.Trim(), reg))
            {
                MsgErr(string.Format(Msg80, 2));
                return;
            }
            if (!UInt32.TryParse(txtDoor2Limit.Text.Trim(), out ui32))
            {
                MsgErr($"2{GetLanguage("Msg80")}！");
                return;
            }

            if (!Regex.IsMatch(txtDoor3Limit.Text.Trim(), reg))
            {
                MsgErr(string.Format(Msg80, 3));
                return;
            }
            if (!UInt32.TryParse(txtDoor3Limit.Text.Trim(), out ui32))
            {
                MsgErr($"3{GetLanguage("Msg80")}！");
                return;
            }

            if (!Regex.IsMatch(txtDoor4Limit.Text.Trim(), reg))
            {
                MsgErr(string.Format(Msg80, 4));
                return;
            }
            if (!UInt32.TryParse(txtDoor4Limit.Text.Trim(), out ui32))
            {
                MsgErr($"4{GetLanguage("Msg80")}！");
                return;
            }
            var Msg82 = GetLanguage("Msg82");
            if (!Regex.IsMatch(txtDoor1Enter.Text.Trim(), reg))
            {
                MsgErr(string.Format(Msg82, 1));
                return;
            }
            if (!UInt32.TryParse(txtDoor1Enter.Text.Trim(), out ui32))
            {
                MsgErr($"1{GetLanguage("Msg83")}！");
                return;
            }

            if (!Regex.IsMatch(txtDoor2Enter.Text.Trim(), reg))
            {
                MsgErr(string.Format(Msg82, 2));
                return;
            }
            if (!UInt32.TryParse(txtDoor2Enter.Text.Trim(), out ui32))
            {
                MsgErr($"2{GetLanguage("Msg83")}！");
                return;
            }

            if (!Regex.IsMatch(txtDoor3Enter.Text.Trim(), reg))
            {
                MsgErr(string.Format(Msg82, 3));
                return;
            }
            if (!UInt32.TryParse(txtDoor3Enter.Text.Trim(), out ui32))
            {
                MsgErr($"3{GetLanguage("Msg83")}！");
                return;
            }

            if (!Regex.IsMatch(txtDoor4Enter.Text.Trim(), reg))
            {
                MsgErr(string.Format(Msg82, 4));
                return;
            }
            if (!UInt32.TryParse(txtDoor4Enter.Text.Trim(), out ui32))
            {
                MsgErr($"4{GetLanguage("Msg83")}！");
                return;
            }


            DoorLimit dl = new DoorLimit();
            dl.GlobalLimit = Convert.ToUInt32(txtGlobalLimit.Text.Trim());
            dl.DoorLimitArray[0] = Convert.ToUInt32(txtDoor1Limit.Text.Trim());
            dl.DoorLimitArray[1] = Convert.ToUInt32(txtDoor2Limit.Text.Trim());
            dl.DoorLimitArray[2] = Convert.ToUInt32(txtDoor3Limit.Text.Trim());
            dl.DoorLimitArray[3] = Convert.ToUInt32(txtDoor4Limit.Text.Trim());

            dl.DoorEnterArray[0] = Convert.ToUInt32(txtDoor1Enter.Text.Trim());
            dl.DoorEnterArray[1] = Convert.ToUInt32(txtDoor2Enter.Text.Trim());
            dl.DoorEnterArray[2] = Convert.ToUInt32(txtDoor3Enter.Text.Trim());
            dl.DoorEnterArray[3] = Convert.ToUInt32(txtDoor4Enter.Text.Trim());

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteEnterDoorLimit cmd = new WriteEnterDoorLimit(cmdDtl, new WriteEnterDoorLimit_Parameter(dl));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 智能防盗主机参数
        private void BtnReadTheftAlarmSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTheftAlarmSetting cmd = new ReadTheftAlarmSetting(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTheftAlarmSetting_Result result = cmde.Command.getResult() as ReadTheftAlarmSetting_Result;
                string UseStr = result.Setting.Use ? GetLanguage("Msg84") : GetLanguage("Msg85"); //功能开关
                string InTime = result.Setting.InTime.ToString(); //进入延迟
                string OutTime = result.Setting.OutTime.ToString(); //退出延迟
                string AlarmTime = result.Setting.AlarmTime.ToString(); //报警时长
                string BeginPassword = result.Setting.BeginPassword.ToString(); //布防密码
                string ClosePassword = result.Setting.ClosePassword.ToString(); //撤防密码

                Invoke(() =>
                {
                    if (result.Setting.Use)
                    {
                        rBtnTheft.Checked = true;
                    }
                    else
                    {
                        rBtnNoTheft.Checked = true;
                    }
                    cbxInTime.Text = InTime;
                    cbxOutTime.Text = OutTime;
                    cbxAlarmTime.Text = AlarmTime;
                    txtBeginPassword.Text = BeginPassword;
                    txtClosePassword.Text = ClosePassword;
                });
                var time = GetLanguage("Msg13");
                string TheftAlarmSettingInfo = GetLanguage("label47") + UseStr +
                                               GetLanguage("label52") + InTime + time +
                                               GetLanguage("label51") + OutTime + time +
                                               GetLanguage("label55") + AlarmTime + time +
                                               GetLanguage("label50") + BeginPassword +
                                               GetLanguage("label49") + ClosePassword;
                mMainForm.AddCmdLog(cmde, TheftAlarmSettingInfo);
            };
        }

        private void BtnWriteTheftAlarmSetting_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxInTime.Text.Trim(), reg) || cbxInTime.Text == "")
            {
                MsgErr(GetLanguage("Msg86") + "！");
                return;
            }
            UInt16 ui32 = 0;
            if (Regex.IsMatch(cbxInTime.Text.Trim(), reg))
            {
                if (!UInt16.TryParse(cbxInTime.Text, out ui32))
                {
                    MsgErr(GetLanguage("Msg86") + "！");
                    return;
                }
                if (Convert.ToUInt32(cbxInTime.Text) < 0 || Convert.ToUInt32(cbxInTime.Text) > 255)
                {
                    MsgErr(GetLanguage("Msg86") + "！");
                    return;
                }
            }
            if (!Regex.IsMatch(cbxOutTime.Text.Trim(), reg) || cbxOutTime.Text == "")
            {
                MsgErr(GetLanguage("Msg87") + "！");
                return;
            }
            if (Regex.IsMatch(cbxOutTime.Text.Trim(), reg))
            {
                if (!UInt16.TryParse(cbxOutTime.Text, out ui32))
                {
                    MsgErr(GetLanguage("Msg87") + "！");
                    return;
                }
                if (Convert.ToUInt32(cbxOutTime.Text) < 0 || Convert.ToUInt32(cbxOutTime.Text) > 255)
                {
                    MsgErr(GetLanguage("Msg87") + "！");
                    return;
                }
            }
            if (!Regex.IsMatch(cbxAlarmTime.Text.Trim(), reg) || cbxAlarmTime.Text == "")
            {
                MsgErr(GetLanguage("Msg88") + "！");
                return;
            }
            if (Regex.IsMatch(cbxAlarmTime.Text.Trim(), reg))
            {
                if (!UInt16.TryParse(cbxAlarmTime.Text, out ui32))
                {
                    MsgErr(GetLanguage("Msg86") + "！");
                    return;
                }
                if (Convert.ToUInt32(cbxAlarmTime.Text) < 0 || Convert.ToUInt32(cbxAlarmTime.Text) > 65535)
                {
                    MsgErr(GetLanguage("Msg88") + "！");
                    return;
                }
            }

            if (!Regex.IsMatch(txtBeginPassword.Text.Trim(), reg))
            {
                MsgErr(GetLanguage("Msg89") + "！");
                return;
            }
            if (txtBeginPassword.Text.Trim().Length > 8)
            {
                MsgErr(GetLanguage("Msg90") + "！");
                return;
            }
            if (!Regex.IsMatch(txtClosePassword.Text.Trim(), reg))
            {
                MsgErr(GetLanguage("Msg91") + "！");
                return;
            }
            if (txtClosePassword.Text.Trim().Length > 8)
            {
                MsgErr(GetLanguage("Msg92") + "！");
                return;
            }

            TheftAlarmSetting ts = new TheftAlarmSetting();
            ts.Use = rBtnTheft.Checked;
            ts.InTime = Convert.ToUInt16(cbxInTime.Text);
            ts.OutTime = Convert.ToUInt16(cbxOutTime.Text);
            ts.AlarmTime = Convert.ToUInt16(cbxAlarmTime.Text);
            ts.BeginPassword = txtBeginPassword.Text;
            ts.ClosePassword = txtClosePassword.Text;

            //WriteTheftAlarmSetting_Parameter wp = new WriteTheftAlarmSetting_Parameter();
            //wp.Setting = ts;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x02);
            //wp.GetBytes(buf);

            //WriteTheftAlarmSetting_Parameter wp2 = new WriteTheftAlarmSetting_Parameter();
            //wp2.SetBytes(buf);

            //string TheftAlarmSettingInfo = "功能开关：" + wp2.Setting.Use +
            //                                "  进入延迟：" + wp2.Setting.InTime + "秒" +
            //                                "  退出延迟：" + wp2.Setting.OutTime + "秒" +
            //                                "  报警时长：" + wp2.Setting.AlarmTime + "秒" +
            //                                "  布防密码：" + wp2.Setting.BeginPassword +
            //                                "  撤防密码：" + wp2.Setting.ClosePassword;
            //mMainForm.AddCmdLog(null, TheftAlarmSettingInfo);


            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteTheftAlarmSetting cmd = new WriteTheftAlarmSetting(cmdDtl, new WriteTheftAlarmSetting_Parameter(ts));
            mMainForm.AddCommand(cmd);
        }
        private void BtnSetTheftFortify_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            SetTheftFortify cmd = new SetTheftFortify(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("Msg93"));
            };
        }

        private void BtnSetTheftDisarming_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            SetTheftDisarming cmd = new SetTheftDisarming(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("SetTheftDisarming"));
            };
        }
        #endregion

        #region 设置防潜回模式
        private void BtnReadCheckInOut_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadCheckInOut cmd = new ReadCheckInOut(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCheckInOut_Result result = cmde.Command.getResult() as ReadCheckInOut_Result;
                string ModeStr = result.Mode == 1 ? GetLanguage("Msg94") : GetLanguage("Msg95"); //防潜回模式
                Invoke(() =>
                {
                    if (result.Mode == 1)
                    {
                        rBtnOneCheck.Checked = true;
                    }
                    else
                    {
                        rBtnAllCheck.Checked = true;
                    }
                });
                ModeStr = GetLanguage("Msg96") + "：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteCheckInOut_Click(object sender, EventArgs e)
        {
            byte Mode = 1;
            if (rBtnAllCheck.Checked == true)
            {
                Mode = 2;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteCheckInOut cmd = new WriteCheckInOut(cmdDtl, new WriteCheckInOut_Parameter(Mode));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 卡片到期提示
        private void BtnReadCardPeriodSpeak_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadCardPeriodSpeak cmd = new ReadCardPeriodSpeak(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCardPeriodSpeak_Result result = cmde.Command.getResult() as ReadCardPeriodSpeak_Result;
                string ModeStr = result.Use == 0 ? GetLanguage("Msg97") : GetLanguage("Msg98"); //卡片到期提示是否启用
                Invoke(() =>
                {
                    if (result.Use == 0)
                    {
                        rBtnNoPeriodSpeak.Checked = true;
                    }
                    else
                    {
                        rBtnPeriodSpeak.Checked = true;
                    }
                });
                ModeStr = GetLanguage("Msg99") + "：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteCardPeriodSpeak_Click(object sender, EventArgs e)
        {
            byte buzzer = 0;
            if (rBtnPeriodSpeak.Checked == true)
            {
                buzzer = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteCardPeriodSpeak cmd = new WriteCardPeriodSpeak(cmdDtl, new WriteCardPeriodSpeak_Parameter(buzzer));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 定时读卡播报语音消息参数
        private void BtnReadReadCardSpeak_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReadCardSpeak cmd = new ReadReadCardSpeak(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadReadCardSpeak_Result result = cmde.Command.getResult() as ReadReadCardSpeak_Result;
                string UseStr = result.SpeakSetting.Use ? GetLanguage("Msg98") : GetLanguage("Msg97"); //定时读卡播报语音消息功能是否启用
                string MsgIndexStr = result.SpeakSetting.MsgIndex == 1 ? GetLanguage("Msg100") : GetLanguage("Msg101"); //消息编号类型
                string STime = result.SpeakSetting.BeginDate.ToString("yyyy-MM-dd HH");
                string ETime = result.SpeakSetting.EndDate.ToString("yyyy-MM-dd HH");
                Invoke(() =>
                {
                    if (result.SpeakSetting.Use)
                    {
                        rBtnEnableReadCardSpeak.Checked = true;
                    }
                    else
                    {
                        rBtnNoEnableReadCardSpeak.Checked = true;
                    }
                    if (result.SpeakSetting.MsgIndex == 1)
                    {
                        rBtnPayRent.Checked = true;
                    }
                    else
                    {
                        rBtnPayManagementFee.Checked = true;
                    }
                    txtSTime.Text = STime;
                    txtETime.Text = ETime;
                });
                var use = GetLanguage("label47");
                UseStr = use + UseStr +
                        " " + GetLanguage("label59") + "：" + MsgIndexStr +
                         $"  {GetLanguage("label61")}：" + STime +
                         use + ETime;
                mMainForm.AddCmdLog(cmde, UseStr);
            };
        }

        private void BtnWriteReadCardSpeak_Click(object sender, EventArgs e)
        {
            string reg = @"^\d{4}-\d{2}-\d{2} \d{2}" + GetLanguage("Msg105");
            if (!Regex.IsMatch(txtSTime.Text.Trim(), reg) || txtSTime.Text == "")
            {
                MsgErr(GetLanguage("Msg102") + "！");
                return;
            }
            if (Convert.ToInt16(txtSTime.Text.Substring(0, 4)) > 2099
                || Convert.ToInt16(txtSTime.Text.Substring(5, 2)) > 12
                || Convert.ToInt16(txtSTime.Text.Substring(8, 2)) > 31
                || Convert.ToInt16(txtSTime.Text.Substring(11, 2)) > 23)
            {
                MsgErr(GetLanguage("Msg102") + "！");
                return;
            }
            if (!Regex.IsMatch(txtETime.Text.Trim(), reg) || txtETime.Text == "")
            {
                MsgErr(GetLanguage("Msg103") + "！");
                return;
            }
            if (Convert.ToInt16(txtETime.Text.Substring(0, 4)) > 2099
                || Convert.ToInt16(txtETime.Text.Substring(5, 2)) > 12
                || Convert.ToInt16(txtETime.Text.Substring(8, 2)) > 31
                || Convert.ToInt16(txtETime.Text.Substring(11, 2)) > 23)
            {
                MsgErr(GetLanguage("Msg103") + "！");
                return;
            }
            if (Convert.ToInt16(txtSTime.Text.Substring(0, 4)) > Convert.ToInt16(txtETime.Text.Substring(0, 4)))
            {
                MsgErr(GetLanguage("Msg104") + "！");
                return;
            }
            else if (Convert.ToInt16(txtSTime.Text.Substring(0, 4)) == Convert.ToInt16(txtETime.Text.Substring(0, 4)))
            {
                if (Convert.ToInt16(txtSTime.Text.Substring(5, 2)) > Convert.ToInt16(txtETime.Text.Substring(5, 2)))
                {
                    MsgErr(GetLanguage("Msg104") + "！");
                    return;
                }
                else if (Convert.ToInt16(txtSTime.Text.Substring(5, 2)) == Convert.ToInt16(txtETime.Text.Substring(5, 2)))
                {
                    if (Convert.ToInt16(txtSTime.Text.Substring(8, 2)) > Convert.ToInt16(txtETime.Text.Substring(8, 2)))
                    {
                        MsgErr(GetLanguage("Msg104") + "！");
                        return;
                    }
                    else if (Convert.ToInt16(txtSTime.Text.Substring(8, 2)) == Convert.ToInt16(txtETime.Text.Substring(8, 2)))
                    {
                        if (Convert.ToInt16(txtSTime.Text.Substring(11, 2)) > Convert.ToInt16(txtETime.Text.Substring(11, 2)))
                        {
                            MsgErr(GetLanguage("Msg104") + "！");
                            return;
                        }
                    }
                }
            }

            int MsgIndex = 1;
            if (rBtnPayManagementFee.Checked == true)
            {
                MsgIndex = 2;
            }

            ReadCardSpeak rcs = new ReadCardSpeak();
            rcs.Use = rBtnEnableReadCardSpeak.Checked;
            rcs.MsgIndex = MsgIndex;
            var temp = GetLanguage("Msg105");
            rcs.BeginDate = Convert.ToDateTime(txtSTime.Text.Replace(temp, "") + ":00:00");
            rcs.EndDate = Convert.ToDateTime(txtETime.Text.Replace(temp, "") + ":00:00");

            //WriteReadCardSpeak_Parameter wp = new WriteReadCardSpeak_Parameter();
            //wp.SpeakSetting = rcs;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x0A);
            //wp.GetBytes(buf);

            //WriteReadCardSpeak_Parameter wp2 = new WriteReadCardSpeak_Parameter();
            //wp2.SetBytes(buf);

            //string ReadCardSpeakInfo = "功能开关：" + wp2.SpeakSetting.Use +
            //                                 "  消息编号类型：" + wp2.SpeakSetting.MsgIndex +
            //                                 "  起始时段：" + wp2.SpeakSetting.BeginDate +
            //                                 "  功能开关：" + wp2.SpeakSetting.EndDate;
            //mMainForm.AddCmdLog(null, ReadCardSpeakInfo);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteReadCardSpeak cmd = new WriteReadCardSpeak(cmdDtl, new WriteReadCardSpeak_Parameter(rcs));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 读取所有功能参数
        private void BtnAllRead_Click(object sender, EventArgs e)
        {
            BtnReadRecordMode_Click(sender, e); //记录存储方式
            BtnReadKeyboard_Click(sender, e); //读卡器密码键盘启用功能开关
            BtnReadLockInteraction_Click(sender, e); //互锁参数
            BtnReadFireAlarmOption_Click(sender, e); //消防报警
            BtnReadOpenAlarmOption_Click(sender, e); //匪警报警
            BtnReadIntervalTime_Click(sender, e); //读卡间隔时间
            BtnReadBroadcast_Click(sender, e); //语音播报语音段开关
            BtnReadReaderCheckMode_Click(sender, e); //读卡器数据校验
            BtnReadBuzzer_Click(sender, e); //主板蜂鸣器
            BtnReadSmogAlarmOption_Click(sender, e); //烟雾报警
            BtnReadEnterDoorLimit_Click(sender, e); //门内人数限制
            BtnReadTheftAlarmSetting_Click(sender, e); //智能防盗主机参数
            BtnReadCheckInOut_Click(sender, e); //设置防潜回模式
            BtnReadCardPeriodSpeak_Click(sender, e); //卡片到期提示
            BtnReadReadCardSpeak_Click(sender, e); //定时读卡播报语音消息参数
        }
        #endregion

        #region 实时监控
        private void BtnBeginWatch_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            BeginWatch cmd = new BeginWatch(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("Msg106"));
            };
        }

        private void BtnCloseWatch_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            CloseWatch cmd = new CloseWatch(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("Msg107"));
            };
        }

        private void BtnReadWatchState_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadWatchState cmd = new ReadWatchState(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                string ModeStr = cmd.WatchState == 0 ? GetLanguage("Msg108") : GetLanguage("Msg109"); //监控状态
                Invoke(() =>
                {
                    if (cmd.WatchState == 1)
                    {
                        lbWatchState.Text = GetLanguage("lbWatchState_2");
                        lbWatchState.ForeColor = Color.Green;
                    }
                    else
                    {
                        lbWatchState.Text = GetLanguage("lbWatchState_1");
                        lbWatchState.ForeColor = Color.Red;
                    }
                });
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnBeginWatchBroadcast_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            BeginWatch_Broadcast cmd = new BeginWatch_Broadcast(cmdDtl);
            mMainForm.AddCommand(cmd);
            ////处理返回值
            //cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            //{
            //    mMainForm.AddCmdLog(cmde, "已开启监控_广播");
            //};
        }

        private void BtnCloseWatchBroadcast_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            CloseWatch_Broadcast cmd = new CloseWatch_Broadcast(cmdDtl);
            mMainForm.AddCommand(cmd);
            ////处理返回值
            //cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            //{
            //    mMainForm.AddCmdLog(cmde, "已开启监控_广播");
            //};
        }


        #endregion

        #region 解除报警 
        private void BtnCloseTypeAlarm_Click(object sender, EventArgs e)
        {
            byte Door; //需要解除报警的门
            ushort AlarmType = 0; //需要解除的报警类型
            if (Convert.ToInt16(cbxCloseAlarmDoor.SelectedIndex) == -1)
            {
                MsgErr(GetLanguage("Msg110") + "！");
                return;
            }
            if (Convert.ToInt16(cbxCloseAlarmDoor.SelectedIndex) == 0)
            {
                Door = 255;
            }
            else
            {
                Door = Convert.ToByte(cbxCloseAlarmDoor.SelectedIndex);
            }

            byte[] tmpData = new byte[2];
            BitArray bit = new BitArray(tmpData);
            for (int i = 0; i < this.dgvAlarmType.Rows.Count; i++)
            {
                bit[i] = Convert.ToBoolean(this.dgvAlarmType.Rows[i].Cells[0].EditedFormattedValue);
            }
            bit.CopyTo(tmpData, 0);
            AlarmType = BitConverter.ToUInt16(tmpData, 0);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            CloseAlarm cmd = new CloseAlarm(cmdDtl, new CloseAlarm_Parameter(Door, AlarmType));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 获取设备状态信息
        private void BtnWorkStatusInfo_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadWorkStatus cmd = new ReadWorkStatus(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadWorkStatus_Result result = cmde.Command.getResult() as ReadWorkStatus_Result;
                StringBuilder DoorInfo = new StringBuilder();
                string RelayState1Str = GetLanguage("Msg111");
                string RelayState2Str = GetLanguage("Msg112");
                string RelayState3Str = GetLanguage("Msg113");
                string DoorLongOpenState1Str = GetLanguage("Msg114");
                string DoorLongOpenState2Str = GetLanguage("Msg115");
                string DoorAlarmState1Str = GetLanguage("Msg116");
                string DoorAlarmState2Str = string.Empty;
                string DoorState1Str = GetLanguage("Msg117");
                string DoorState2Str = GetLanguage("Msg118");
                string AlarmState1Str = GetLanguage("Msg119");
                string AlarmState2Str = GetLanguage("Msg120");
                string LockState1Str = GetLanguage("Msg121");
                string LockState2Str = GetLanguage("Msg122");
                string LockState3Str = GetLanguage("Msg123");
                string PortLockState1Str = GetLanguage("Msg124");
                string PortLockState2Str = GetLanguage("Msg125");

                Invoke(() =>
                {
                    //继电器物理状态
                    for (int i = 0; i < result.RelayState.DoorMax; i++)
                    {
                        if (result.RelayState.DoorPort[i] == 0)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[7].Value = RelayState1Str;
                            DoorInfo.Append("  " + GetLanguage("Door") + (i + 1) + GetLanguage("Msg126") + " ：" + RelayState1Str + "");
                        }
                        else if (result.RelayState.DoorPort[i] == 1)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[7].Value = RelayState2Str;
                            DoorInfo.Append("  " + GetLanguage("Door") + (i + 1) + GetLanguage("Msg126") + " ：" + RelayState1Str + "");
                        }
                        else if (result.RelayState.DoorPort[i] == 2)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[7].Value = RelayState3Str;
                            DoorInfo.Append("  " + GetLanguage("Door") + (i + 1) + GetLanguage("Msg126") + " ：" + RelayState1Str + "");
                        }
                    }
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                    //运行状态
                    for (int i = 0; i < result.DoorLongOpenState.DoorMax; i++)
                    {
                        if (result.DoorLongOpenState.DoorPort[i] == 0)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[1].Value = DoorLongOpenState1Str;
                            DoorInfo.Append("  " + GetLanguage("Door") + (i + 1) + GetLanguage("Msg127") + " ：" + DoorLongOpenState1Str + "");
                        }
                        else if (result.DoorLongOpenState.DoorPort[i] == 1)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[1].Value = DoorLongOpenState2Str;
                            DoorInfo.Append("  " + GetLanguage("Door") + (i + 1) + GetLanguage("Msg127") + " ：" + DoorLongOpenState2Str + "");
                        }
                    }
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                    //门磁开关
                    for (int i = 0; i < result.DoorState.DoorMax; i++)
                    {
                        if (result.DoorState.DoorPort[i] == 0)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[2].Value = DoorState1Str;
                            DoorInfo.Append("  " + GetLanguage("Door") + (i + 1) + GetLanguage("Msg128") + " ：" + DoorState1Str + "");
                        }
                        else if (result.DoorState.DoorPort[i] == 1)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[2].Value = DoorState2Str;
                            DoorInfo.Append("  " + GetLanguage("Door") + (i + 1) + GetLanguage("Msg128") + "：" + DoorState2Str + "");
                        }
                    }
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                    //门报警状态
                    for (int i = 0; i < result.DoorAlarmState.DoorMax; i++)
                    {
                        if (result.DoorAlarmState.DoorPort[i] == 0)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[6].Value = DoorAlarmState1Str;
                            DoorInfo.Append("  " + GetLanguage("Door") + (i + 1) + GetLanguage("Msg129") + "：" + DoorAlarmState1Str + "");
                        }
                        else
                        {
                            byte[] ByteDoorAlarmStateSet = new byte[] { result.DoorAlarmState.DoorPort[i] };
                            BitArray bitSet = new BitArray(ByteDoorAlarmStateSet);
                            for (int j = 0; j < 6; j++)
                            {
                                if (bitSet[j])
                                {
                                    if (j == 0)
                                    {
                                        DoorAlarmState2Str = DoorAlarmState2Str + "  " + GetLanguage("Msg11");
                                    }
                                    else if (j == 1)
                                    {
                                        DoorAlarmState2Str = DoorAlarmState2Str + "  " + GetLanguage("Msg130");
                                    }
                                    else if (j == 2)
                                    {
                                        DoorAlarmState2Str = DoorAlarmState2Str + "  " + GetLanguage("Msg131");
                                    }
                                    else if (j == 3)
                                    {
                                        DoorAlarmState2Str = DoorAlarmState2Str + "  " + GetLanguage("Msg132");
                                    }
                                    else if (j == 4)
                                    {
                                        DoorAlarmState2Str = DoorAlarmState2Str + "  " + GetLanguage("Msg133");
                                    }
                                    else if (j == 5)
                                    {
                                        DoorAlarmState2Str = DoorAlarmState2Str + "  " + GetLanguage("Msg134");
                                    }
                                }
                            }
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[6].Value = DoorAlarmState2Str;
                            DoorInfo.Append("  " + GetLanguage("Door") + (i + 1) + GetLanguage("Msg129") + "：" + DoorAlarmState2Str + "");
                        }
                    }
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                    //设备报警状态
                    byte[] ByteSet = new byte[] { result.AlarmState };
                    BitArray bit = new BitArray(ByteSet);
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 0)
                        {
                            if (bit[i])
                            {
                                this.dgvEquipmentStatusInfo.Rows[5].Cells[6].Value = AlarmState2Str;
                                DoorInfo.Append($"  {GetLanguage("Msg135")}：" + AlarmState2Str + "");
                            }
                            else
                            {
                                this.dgvEquipmentStatusInfo.Rows[5].Cells[6].Value = AlarmState1Str;
                                DoorInfo.Append($"  {GetLanguage("Msg135")}：" + AlarmState1Str + "");
                            }
                        }
                        if (i == 1)
                        {
                            if (bit[i])
                            {
                                this.dgvEquipmentStatusInfo.Rows[7].Cells[6].Value = AlarmState2Str;
                                DoorInfo.Append($"  {GetLanguage("Msg136")}：" + AlarmState2Str + "");
                            }
                            else
                            {
                                this.dgvEquipmentStatusInfo.Rows[7].Cells[6].Value = AlarmState1Str;
                                DoorInfo.Append($"  {GetLanguage("Msg136")}：" + AlarmState1Str + "");
                            }
                        }
                        if (i == 2)
                        {
                            if (bit[i])
                            {
                                this.dgvEquipmentStatusInfo.Rows[4].Cells[6].Value = AlarmState2Str;
                                DoorInfo.Append($"  {GetLanguage("Msg137")}：" + AlarmState2Str + "");
                            }
                            else
                            {
                                this.dgvEquipmentStatusInfo.Rows[4].Cells[6].Value = AlarmState1Str;
                                DoorInfo.Append($"  {GetLanguage("Msg137")}：" + AlarmState1Str + "");
                            }
                        }
                        if (i == 3)
                        {
                            if (bit[i])
                            {
                                this.dgvEquipmentStatusInfo.Rows[6].Cells[6].Value = AlarmState2Str;
                                DoorInfo.Append($"  {GetLanguage("Msg138")}：" + AlarmState2Str + "");
                            }
                            else
                            {
                                this.dgvEquipmentStatusInfo.Rows[6].Cells[6].Value = AlarmState1Str;
                                DoorInfo.Append($"  {GetLanguage("Msg138")}：" + AlarmState1Str + "");
                            }
                        }
                        if (i == 5)
                        {
                            if (bit[i])
                            {
                                this.dgvEquipmentStatusInfo.Rows[8].Cells[6].Value = AlarmState2Str;
                                DoorInfo.Append($"  {GetLanguage("Msg139")}：" + AlarmState2Str + "");
                            }
                            else
                            {
                                this.dgvEquipmentStatusInfo.Rows[8].Cells[6].Value = AlarmState1Str;
                                DoorInfo.Append($"  {GetLanguage("Msg139")}：" + AlarmState1Str + "");
                            }
                        }
                    }
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                    //继电器逻辑状态
                    for (int i = 0; i < 8; i++)
                    {
                        if (i < 4)
                        {
                            if (result.LockState.DoorPort[i] == 0)
                            {
                                this.dgvEquipmentStatusInfo.Rows[i].Cells[3].Value = LockState1Str;
                                DoorInfo.Append($"  {GetLanguage("Door")}" + (i + 1) + $"  {GetLanguage("Msg140")}：" + LockState1Str + "");
                            }
                            else if (result.LockState.DoorPort[i] == 1)
                            {
                                this.dgvEquipmentStatusInfo.Rows[i].Cells[3].Value = LockState2Str;
                                DoorInfo.Append($"  {GetLanguage("Door")}" + (i + 1) + $"  {GetLanguage("Msg140")}：" + LockState2Str + "");
                            }
                            else if (result.LockState.DoorPort[i] == 2)
                            {
                                this.dgvEquipmentStatusInfo.Rows[i].Cells[3].Value = LockState3Str;
                                DoorInfo.Append($"  {GetLanguage("Door")}" + (i + 1) + $"  {GetLanguage("Msg140")}：" + LockState3Str + "");
                            }
                        }
                        else if (i == 4)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[3].Value = LockState1Str;
                            DoorInfo.Append($"  {GetLanguage("Msg141")}：" + LockState1Str + "");
                        }
                        else if (i == 5)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[3].Value = LockState1Str;
                            DoorInfo.Append($"  {GetLanguage("Msg142")}：" + LockState1Str + "");
                        }
                        else if (i == 6)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[3].Value = LockState1Str;
                            DoorInfo.Append($"  {GetLanguage("Msg143")}：" + LockState1Str + "");
                        }
                        else if (i == 7)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[3].Value = LockState1Str;
                            DoorInfo.Append($"  {GetLanguage("Msg144")}：" + LockState1Str + "");
                        }
                    }
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                    //锁定状态
                    for (int i = 0; i < result.PortLockState.DoorMax; i++)
                    {
                        if (result.PortLockState.DoorPort[i] == 0)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[4].Value = PortLockState1Str;
                            DoorInfo.Append($"  {GetLanguage("Door")}" + (i + 1) + $"  {GetLanguage("Msg145")}：" + PortLockState1Str + "");
                        }
                        else if (result.PortLockState.DoorPort[i] == 1)
                        {
                            this.dgvEquipmentStatusInfo.Rows[i].Cells[4].Value = PortLockState2Str;
                            DoorInfo.Append($"  {GetLanguage("Door")}" + (i + 1) + $"  {GetLanguage("Msg145")}：" + PortLockState2Str + "");
                        }
                    }
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                    Invoke(() =>
                    {
                        if (result.WatchState == 1)
                        {
                            lbSWatchState.Text = GetLanguage("lbWatchState_2");
                            lbSWatchState.ForeColor = Color.Green;
                            DoorInfo.Append($"  {GetLanguage("Lab_WatchStatus")}{GetLanguage("lbWatchState_2")}");
                        }
                        else
                        {
                            lbSWatchState.Text = GetLanguage("lbWatchState_1");
                            lbSWatchState.ForeColor = Color.Red;
                            DoorInfo.Append($"  {GetLanguage("Lab_WatchStatus")}{GetLanguage("lbWatchState_1")}");
                        }
                    });
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                    //门内总人数
                    this.txtAllNum.Text = result.EnterTotal.GlobalEnter.ToString();
                    var doorenter = GetLanguage("Msg147");
                    DoorInfo.Append($"  {GetLanguage("Msg146")}" + result.EnterTotal.DoorEnterArray[0].ToString());
                    this.dgvEquipmentStatusInfo.Rows[0].Cells[5].Value = result.EnterTotal.DoorEnterArray[0].ToString(); //门1人数
                    DoorInfo.Append($"  {string.Format(doorenter, 1)}：" + result.EnterTotal.DoorEnterArray[0].ToString());
                    this.dgvEquipmentStatusInfo.Rows[1].Cells[5].Value = result.EnterTotal.DoorEnterArray[1].ToString(); //门2人数
                    DoorInfo.Append($"  {string.Format(doorenter, 2)}：" + result.EnterTotal.DoorEnterArray[1].ToString());
                    this.dgvEquipmentStatusInfo.Rows[2].Cells[5].Value = result.EnterTotal.DoorEnterArray[2].ToString(); //门3人数
                    DoorInfo.Append($"  {string.Format(doorenter, 3)}：" + result.EnterTotal.DoorEnterArray[2].ToString());
                    this.dgvEquipmentStatusInfo.Rows[3].Cells[5].Value = result.EnterTotal.DoorEnterArray[3].ToString(); //门4人数
                    DoorInfo.Append($"  {string.Format(doorenter, 4)}：" + result.EnterTotal.DoorEnterArray[3].ToString());
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                    //防盗主机布防状态
                    string TheftStateStr = string.Empty;
                    if (result.TheftState == 1)
                    {
                        TheftStateStr = GetLanguage("Msg148");
                    }
                    else if (result.TheftState == 2)
                    {
                        TheftStateStr = GetLanguage("Msg149");
                    }
                    else if (result.TheftState == 3)
                    {
                        TheftStateStr = GetLanguage("Msg150");
                    }
                    else if (result.TheftState == 4)
                    {
                        TheftStateStr = GetLanguage("Msg151");
                    }
                    else if (result.TheftState == 5)
                    {
                        TheftStateStr = GetLanguage("Msg152");
                    }
                    else if (result.TheftState == 6)
                    {
                        TheftStateStr = GetLanguage("Msg153");
                    }
                    DoorInfo.Append($"  {GetLanguage("Msg154")}：" + TheftStateStr);
                    mMainForm.AddCmdLog(cmde, DoorInfo.ToString());
                    DoorInfo.Clear();
                });
            };
        }
        #endregion

        #region 防盗主机布防状态
        private void BtnTheftAlarmSettingState_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTheftAlarmState cmd = new ReadTheftAlarmState(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTheftAlarmState_Result result = cmde.Command.getResult() as ReadTheftAlarmState_Result;
                StringBuilder TheftStateInfo = new StringBuilder();
                TheftStateInfo.Append(GetLanguage("Msg154") + "：");
                string TheftStateStr = string.Empty;
                Invoke(() =>
                {
                    if (result.TheftState == 1)
                    {
                        TheftStateStr = GetLanguage("Msg148");
                    }
                    else if (result.TheftState == 2)
                    {
                        TheftStateStr = GetLanguage("Msg149");
                    }
                    else if (result.TheftState == 3)
                    {
                        TheftStateStr = GetLanguage("Msg150");
                    }
                    else if (result.TheftState == 4)
                    {
                        TheftStateStr = GetLanguage("Msg151");
                    }
                    else if (result.TheftState == 5)
                    {
                        TheftStateStr = GetLanguage("Msg152");
                    }
                    else if (result.TheftState == 6)
                    {
                        TheftStateStr = GetLanguage("Msg153");
                    }
                    txtBeginState.Text = TheftStateStr;
                    TheftStateInfo.Append(TheftStateStr);
                    mMainForm.AddCmdLog(cmde, TheftStateInfo.ToString());
                });
            };
        }
        #endregion

        #region 初始化设备
        private void BtnInitalData_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            cmdDtl.Timeout = 100000;
            FormatController cmd = new FormatController(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("Msg155") + "！");
            };
        }
        #endregion

        #region 搜索设备
        private Random mRandom = new Random();

        /// <summary>
        /// 获取一个随机数
        /// </summary>
        /// <param name="iMin">下限</param>
        /// <param name="iMax">上限</param>
        /// <returns></returns>
        private int GetRandomNum(int iMin, int iMax)
        {
            var rnd = mRandom.NextDouble();
            return iMin + (int)(rnd * (iMax - iMin + 1));
        }

        private void BtnSearchEquptOnNetNum_Click(object sender, EventArgs e)
        {

            ushort NetCode = (ushort)GetRandomNum(100, 60000);
            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 4000;
            cmdDtl.RestartCount = 0;
            int ReSend = 0;
            if (cmdDtl == null) return;

            SearchControltor_Parameter par = new SearchControltor_Parameter(NetCode);
            SearchControltor cmd = new SearchControltor(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                //搜索控制器
                SearchControltor_Result result = cmde.Command.getResult() as SearchControltor_Result;
                string log = "SN:" + result.SN + "," + DebugTCPDetail(result.TCP);

                mMainForm.AddCmdLog(cmde, log);

                //修改网络标识
                Invoke(() =>
                {
                    var tmpCmdDtl = mMainForm.GetCommandDetail();
                    tmpCmdDtl.Timeout = 2000;
                    OnlineAccessCommandDetail detail = tmpCmdDtl as OnlineAccessCommandDetail;
                    detail.SN = result.SN;
                    WriteControltorNetCode writeCmd = new WriteControltorNetCode(tmpCmdDtl, par);
                    mMainForm.AddCommand(writeCmd);
                });

            };
            //超时
            cmdDtl.CommandTimeout += (sdr1, cmde1) =>
             {
                 ReSend += 1;
                 if (ReSend < 3)
                 {
                     SearchControltor recmd = new SearchControltor(cmde1.CommandDetail, par);
                     mMainForm.AddCommand(recmd);
                 }

             };


            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 缓存区操作
        private void BtnReadCacheContent_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadCacheContent cmd = new ReadCacheContent(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                CacheContent_Result result = cmde.Command.getResult() as CacheContent_Result;
                string CacheContent = result.CacheContent;
                Invoke(() =>
                {
                    txtCacheContent.Text = CacheContent;
                });
                CacheContent = GetLanguage("Lab_CacheContent") + "：" + CacheContent;
                mMainForm.AddCmdLog(cmde, CacheContent);
            };
        }

        private void BtnWriteCacheContent_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(txtCacheContent.Text.Trim(), reg) || txtCacheContent.Text.Trim().Length > 30 || string.IsNullOrEmpty(txtCacheContent.Text.Trim()))
            {
                MsgErr(GetLanguage("Msg156") + "！");
                return;
            }
            string cacheContent = txtCacheContent.Text.Trim();

            //CacheContent_Parameter wp = new CacheContent_Parameter();
            //wp.CacheContent = cacheContent;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x1E);
            //wp.GetBytes(buf);

            //CacheContent_Parameter wp2 = new CacheContent_Parameter();
            //wp2.SetBytes(buf);

            //cacheContent = "缓存区内容：" + cacheContent;
            //mMainForm.AddCmdLog(null, cacheContent);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteCacheContent cmd = new WriteCacheContent(cmdDtl, new CacheContent_Parameter(cacheContent));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 客户端控制器保活间隔
        private void BtnReadKeepAliveInterval_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadKeepAliveInterval cmd = new ReadKeepAliveInterval(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadKeepAliveInterval_Result result = cmde.Command.getResult() as ReadKeepAliveInterval_Result;

                ushort IntervalTime = result.IntervalTime; //保活间隔时间
                string IntervalTimeInfo = string.Empty;
                var temp = GetLanguage("Msg13");
                if (IntervalTime == 0)
                {
                    IntervalTimeInfo = GetLanguage("Msg157");
                }
                else
                {
                    IntervalTimeInfo = IntervalTime.ToString();
                }

                Invoke(() =>
                {
                    cbxKeepAliveInterval.Text = IntervalTimeInfo.Replace(temp, "");
                });
                string IntervalTimeStr = GetLanguage("Lab_KeepAliveInterval1") + "：" + IntervalTimeInfo + GetLanguage("Lab_KeepAliveInterval2");
                mMainForm.AddCmdLog(cmde, IntervalTimeStr);
            };
        }

        private void BtnWriteKeepAliveInterval_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxKeepAliveInterval.Text.Trim(), reg))
            {
                if (cbxKeepAliveInterval.Text != GetLanguage("Msg157"))
                {
                    MsgErr(GetLanguage("Msg158") + "！");
                    return;
                }
            }
            if (Regex.IsMatch(cbxKeepAliveInterval.Text.Trim(), reg))
            {
                if (Convert.ToUInt32(cbxKeepAliveInterval.Text) < 0 || Convert.ToUInt32(cbxKeepAliveInterval.Text) > 65535)
                {
                    MsgErr(GetLanguage("Msg158") + "！");
                    return;
                }
            }

            ushort IntervalTime = 0;
            string deadlineInfo = cbxKeepAliveInterval.Text;
            if (deadlineInfo == GetLanguage("Msg157"))
            {
                IntervalTime = 0;
            }
            else
            {
                IntervalTime = Convert.ToUInt16(cbxKeepAliveInterval.Text);
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteKeepAliveInterval cmd = new WriteKeepAliveInterval(cmdDtl, new WriteKeepAliveInterval_Parameter(IntervalTime));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 黑名单报警
        private void BtnReadBalcklistAlarmOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadBalcklistAlarmOption cmd = new ReadBalcklistAlarmOption(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadBalcklistAlarmOption_Result result = cmde.Command.getResult() as ReadBalcklistAlarmOption_Result;
                string ModeStr = result.Use == 0 ? GetLanguage("Msg159") : GetLanguage("Msg160"); //黑名单报警功能是否启用
                Invoke(() =>
                {
                    if (result.Use == 0)
                    {
                        rBtnNoBalcklistAlarm.Checked = true;
                    }
                    else
                    {
                        rBtnBalcklistAlarm.Checked = true;
                    }
                });
                ModeStr = GetLanguage("Msg161") + "：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteBalcklistAlarmOption_Click(object sender, EventArgs e)
        {
            byte use = 0;
            if (rBtnBalcklistAlarm.Checked == true)
            {
                use = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteBalcklistAlarmOption cmd = new WriteBalcklistAlarmOption(cmdDtl, new WriteBalcklistAlarmOption_Parameter(use));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 防探测功能
        private void BtnReadExploreLockMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadExploreLockMode cmd = new ReadExploreLockMode(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadExploreLockMode_Result result = cmde.Command.getResult() as ReadExploreLockMode_Result;
                string ModeStr = result.Use == 0 ? GetLanguage("Msg159") : GetLanguage("Msg160"); //防探测功能是否启用
                Invoke(() =>
                {
                    if (result.Use == 0)
                    {
                        rBtnNoExploreLockMode.Checked = true;
                    }
                    else
                    {
                        rBtnExploreLockMode.Checked = true;
                    }
                });
                ModeStr = GetLanguage("Msg167") + "：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteExploreLockMode_Click(object sender, EventArgs e)
        {
            byte use = 0;
            if (rBtnExploreLockMode.Checked == true)
            {
                use = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteExploreLockMode cmd = new WriteExploreLockMode(cmdDtl, new WriteExploreLockMode_Parameter(use));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 485线路反接检测开关
        private void BtnReadCheck485Line_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadCheck485Line cmd = new ReadCheck485Line(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCheck485Line_Result result = cmde.Command.getResult() as ReadCheck485Line_Result;
                string ModeStr = result.Use == 0 ? GetLanguage("Msg159") : GetLanguage("Msg160"); //485线路反接检测开关是否启用
                Invoke(() =>
                {
                    if (result.Use == 0)
                    {
                        rBtnNoCheck485Line.Checked = true;
                    }
                    else
                    {
                        rBtnCheck485Line.Checked = true;
                    }
                });
                ModeStr = GetLanguage("groupBox15") + "：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteCheck485Line_Click(object sender, EventArgs e)
        {
            byte use = 0;
            if (rBtnCheck485Line.Checked == true)
            {
                use = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteCheck485Line cmd = new WriteCheck485Line(cmdDtl, new WriteCheck485Line_Parameter(use));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region TCP客户端操作
        private void BtnReadTCPClientList_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTCPClientList cmd = new ReadTCPClientList(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                Invoke(() =>
                {
                    TCPClient_Result result = cmde.Command.getResult() as TCPClient_Result;
                    this.dgvTCPClientList.AllowUserToAddRows = false;
                    this.dgvTCPClientList.Rows.Clear();
                    if (result.tCPClientDetail.TCPClientNum > 0)
                    {
                        this.dgvTCPClientList.Rows.Add(result.tCPClientDetail.TCPClientNum);
                    }
                    for (int i = 0; i < result.tCPClientDetail.TCPClientNum; i++)
                    {
                        this.dgvTCPClientList.Rows[i].Cells[1].Value = i + 1;
                        this.dgvTCPClientList.Rows[i].Cells[2].Value = result.tCPClientDetail.IP[i];
                        this.dgvTCPClientList.Rows[i].Cells[3].Value = result.tCPClientDetail.TCPPort[i];
                        if (result.tCPClientDetail.ConnectTime[i].Year != 1)
                        {
                            this.dgvTCPClientList.Rows[i].Cells[4].Value = result.tCPClientDetail.ConnectTime[i];
                        }
                    }
                    mMainForm.AddCmdLog(cmde, GetLanguage("Msg162") + "：" + result.tCPClientDetail.TCPClientNum);
                });
            };
        }

        private void BtnStopTCPClientConnection_Click(object sender, EventArgs e)
        {
            TCPClientDetail tCPClientDetail = new TCPClientDetail();
            if (this.dgvTCPClientList.Rows.Count > 0)
            {
                tCPClientDetail.IP = new string[this.dgvTCPClientList.Rows.Count];
                tCPClientDetail.TCPPort = new ushort[this.dgvTCPClientList.Rows.Count];
                for (int i = 0; i < this.dgvTCPClientList.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(this.dgvTCPClientList.Rows[i].Cells[0].EditedFormattedValue))
                    {
                        tCPClientDetail.IP[0] = this.dgvTCPClientList.Rows[i].Cells[2].Value.ToString();
                        tCPClientDetail.TCPPort[0] = Convert.ToUInt16(this.dgvTCPClientList.Rows[i].Cells[3].Value);
                        break;
                    }
                }
                if (string.IsNullOrEmpty(tCPClientDetail.IP[0])) //没有选择，默认选择列表第一条数据进行断开连接
                {
                    tCPClientDetail.IP[0] = this.dgvTCPClientList.Rows[0].Cells[2].Value.ToString();
                    tCPClientDetail.TCPPort[0] = Convert.ToUInt16(this.dgvTCPClientList.Rows[0].Cells[3].Value);
                }
            }
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            StopTCPClientConnection cmd = new StopTCPClientConnection(cmdDtl, new TCPClient_Parameter(tCPClientDetail));
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                Invoke(() =>
                {
                    string TCPClientInfo = "IP：" + tCPClientDetail.IP[0] +
                                           $" {GetLanguage("Msg163")}：" + tCPClientDetail.TCPPort[0];
                    mMainForm.AddCmdLog(cmde, TCPClientInfo);
                });
            };
        }

        private void BtnStopAllTCPClientConnection_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            //cmdDtl.Timeout = 5000;
            if (cmdDtl == null) return;
            StopAllTCPClientConnection cmd = new StopAllTCPClientConnection(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, GetLanguage("Msg164") + "！");
            };
        }
        #endregion

        #region 有效期即将过期提醒时间
        private void BtnReadCardDeadlineTipDay_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadCardDeadlineTipDay cmd = new ReadCardDeadlineTipDay(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCardDeadlineTipDay_Result result = cmde.Command.getResult() as ReadCardDeadlineTipDay_Result;
                string TipDayInfo = string.Empty;
                Invoke(() =>
                {
                    cbxCardDeadlineTipDay.Text = result.Day == 0 ? GetLanguage("Msg157") : "" + result.Day + "";
                });
                if (result.Day == 0)
                {
                    TipDayInfo = GetLanguage("Msg165");
                }
                else
                {
                    TipDayInfo = GetLanguage("Msg168") + "：" + result.Day + GetLanguage("Msg169");
                }
                mMainForm.AddCmdLog(cmde, TipDayInfo);
            };
        }

        private void BtnWriteCardDeadlineTipDay_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxCardDeadlineTipDay.Text.Trim(), reg) || string.IsNullOrEmpty(cbxCardDeadlineTipDay.Text.Trim()))
            {
                if (cbxCardDeadlineTipDay.Text != GetLanguage("Msg157"))
                {
                    MsgErr(GetLanguage("Msg166") + "！");
                    return;
                }
            }
            if (Regex.IsMatch(cbxCardDeadlineTipDay.Text.Trim(), reg))
            {
                if (Convert.ToUInt32(cbxCardDeadlineTipDay.Text) < 0 || Convert.ToUInt32(cbxCardDeadlineTipDay.Text) > 255)
                {
                    MsgErr(GetLanguage("Msg166") + "！");
                    return;
                }
            }

            byte tipDay = 0;
            string tipDayInfo = cbxCardDeadlineTipDay.Text;
            if (tipDayInfo == GetLanguage("Msg157"))
            {
                tipDay = 0;
            }
            else
            {
                tipDay = Convert.ToByte(tipDayInfo);
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteCardDeadlineTipDay cmd = new WriteCardDeadlineTipDay(cmdDtl, new WriteCardDeadlineTipDay_Parameter(tipDay));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 控制板防拆报警功能
        private void BtnReadrBtnControlPanelTamperAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadControlPanelTamperAlarm cmd = new ReadControlPanelTamperAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadControlPanelTamperAlarm_Result result = cmde.Command.getResult() as ReadControlPanelTamperAlarm_Result;
                string ModeStr = result.Use == 0 ? GetLanguage("Msg159") : GetLanguage("Msg160"); //控制板防拆报警开关是否启用
                Invoke(() =>
                {
                    if (result.Use == 0)
                    {
                        rBtnNoControlPanelTamperAlarm.Checked = true;
                    }
                    else
                    {
                        rBtnControlPanelTamperAlarm.Checked = true;
                    }
                });
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriterBtnControlPanelTamperAlarm_Click(object sender, EventArgs e)
        {
            byte use = 0;
            if (rBtnControlPanelTamperAlarm.Checked == true)
            {
                use = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteControlPanelTamperAlarm cmd = new WriteControlPanelTamperAlarm(cmdDtl, new WriteControlPanelTamperAlarm_Parameter(use));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region HTTP网页登陆开关
        private void BtnReadHTTPPageLandingSwitch_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadHTTPPageLandingSwitch cmd = new ReadHTTPPageLandingSwitch(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadHTTPPageLandingSwitch_Result result = cmde.Command.getResult() as ReadHTTPPageLandingSwitch_Result;
                string ModeStr = result.Use == 0 ? GetLanguage("Msg159") : GetLanguage("Msg160"); //HTTP网页登陆开关是否启用
                Invoke(() =>
                {
                    if (result.Use == 0)
                    {
                        rBtnNoHTTPPageLandingSwitch.Checked = true;
                    }
                    else
                    {
                        rBtnHTTPPageLandingSwitch.Checked = true;
                    }
                });
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteHTTPPageLandingSwitch_Click(object sender, EventArgs e)
        {
            byte use = 0;
            if (rBtnHTTPPageLandingSwitch.Checked == true)
            {
                use = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteHTTPPageLandingSwitch cmd = new WriteHTTPPageLandingSwitch(cmdDtl, new WriteHTTPPageLandingSwitch_Parameter(use));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 开门超时报警时，合法卡解除报警开关
        private void BtnReadLawfulCardReleaseAlarmSwitch_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadLawfulCardReleaseAlarmSwitch cmd = new ReadLawfulCardReleaseAlarmSwitch(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadLawfulCardReleaseAlarmSwitch_Result result = cmde.Command.getResult() as ReadLawfulCardReleaseAlarmSwitch_Result;
                string ModeStr = result.Use == 0 ? GetLanguage("Msg159") : GetLanguage("Msg160"); //合法卡解除报警开关是否启用
                Invoke(() =>
                {
                    if (result.Use == 0)
                    {
                        rBtnNoLawfulCardReleaseAlarmSwitch.Checked = true;
                    }
                    else
                    {
                        rBtnLawfulCardReleaseAlarmSwitch.Checked = true;
                    }
                });
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteLawfulCardReleaseAlarmSwitch_Click(object sender, EventArgs e)
        {
            byte use = 0;
            if (rBtnLawfulCardReleaseAlarmSwitch.Checked == true)
            {
                use = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteLawfulCardReleaseAlarmSwitch cmd = new WriteLawfulCardReleaseAlarmSwitch(cmdDtl, new WriteLawfulCardReleaseAlarmSwitch_Parameter(use));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        private void Lab_KeepAliveInterval1_Click(object sender, EventArgs e)
        {

        }

        private void label59_Click(object sender, EventArgs e)
        {

        }

        private void Btn_ReadFireAlarmOption_Click(object sender, EventArgs ev)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            cmdDtl.CommandCompleteEvent += (s, e) =>
            {
                var result = e.Result as ReadFireAlarmOption_Result;
                Invoke(() =>
                {
                    Cmb_FireAlarmOption.SelectedIndex = result.Option;
                });
            };
            var cmd = new ReadFireAlarmOption(cmdDtl);
            mMainForm.AddCommand(cmd);
        }

        private void Btn_WriteFireAlarmOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            var cmd = new WriteFireAlarmOption(cmdDtl, new WriteFireAlarmOption_Parameter((byte)Cmb_FireAlarmOption.SelectedIndex));
            mMainForm.AddCommand(cmd);
        }
    }
}
