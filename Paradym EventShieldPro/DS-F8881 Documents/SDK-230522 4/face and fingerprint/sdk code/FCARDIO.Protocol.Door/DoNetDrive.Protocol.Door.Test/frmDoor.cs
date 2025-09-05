using DoNetDrive.Protocol.Door.Door8800.Door.ReaderOption;
using DoNetDrive.Protocol.Door.Door8800.Door.RelayOption;
using DoNetDrive.Protocol.Door.Door8800.Door.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Protocol.Door.Door8800.Utility;
using DoNetDrive.Protocol.Door.Door8800.Door;
using System.Collections;
using DoNetDrive.Protocol.Door.Door8800.Door.ReaderWorkSetting;
using DoNetDrive.Core.Util;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Protocol.Door.Door8800.Door.DoorWorkSetting;
using DoNetDrive.Protocol.Door.Door8800.Door.AutoLockedSetting;
using DoNetDrive.Protocol.Door.Door8800.Door.RelayReleaseTime;
using System.Text.RegularExpressions;
using DoNetDrive.Protocol.Door.Door8800.Door.ReaderInterval;
using DoNetDrive.Protocol.Door.Test.Model;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter;
using DoNetDrive.Protocol.Door.Door8800.Door.AntiPassback;
using DoNetDrive.Protocol.Door.Door8800.Door.PushButtonSetting;
using DoNetDrive.Protocol.Door.Door8800.Door.AnyCardSetting;
using DoNetDrive.Protocol.Door.Door8800.Door.VoiceBroadcastSetting;
using DoNetDrive.Protocol.Door.Door8800.Door.InOutSideReadOpenSetting;
using DoNetDrive.Protocol.Door.Door8800.Door.ManageKeyboardSetting;
using DoNetDrive.Protocol.Door.Door8800.Door.AreaAntiPassback;
using DoNetDrive.Protocol.Door.Door8800.Door.InterLockSetting;
using DoNetDrive.Protocol.Door.Door8800.Door.MultiCard;
using DoNetDrive.Protocol.Door.Door8800.Door.ReaderAlarm;
using DoNetDrive.Protocol.Door.Door89H.Door.ReadCardAndTakePictures;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting;

namespace DoNetDrive.Protocol.Door.Test
{
    public partial class frmDoor : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static frmDoor onlyObj;


        public static frmDoor GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmDoor(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private frmDoor(INMain main) : base(main)
        {
            InitializeComponent();
        }
        #endregion

        List<WeekTimeGroupDto> ListWeekTimeGroupDto = new List<WeekTimeGroupDto>();
        List<WeekTimeGroupDto> ListAutoLockedDto = new List<WeekTimeGroupDto>();
        WeekTimeGroup WeekTimeGroupPushButtonDto = new WeekTimeGroup(8);
        WeekTimeGroup WeekTimeGroupSensorAlarmDto = new WeekTimeGroup(8);
        WeekTimeGroup WeekTimeGroupWorkSettingDto = new WeekTimeGroup(8);
        List<List<decimal>> listGroupA = new List<List<decimal>>();
        List<List<decimal>> listGroupB = new List<List<decimal>>();
        List<MultiCard_GroupFix> listFix = new List<MultiCard_GroupFix>();


        private void frmDoor_Load(object sender, EventArgs e)
        {
            LoadUILanguage();
            cbxReleaseTime.SelectedIndex = 0;
            //非法读卡报警
            IniInvalidCardAlarmOptionUse();

            //胁迫报警密码
            AlarmPassword();
            AlarmOption();

            //开门超时提示参数
            OvertimeAlarmSetting();
            Alarm();
            Overtime();

            //门磁报警参数
            SensorAlarmSetting();
            Week();

            InitGridReaderWork();

            #region 读卡器字节数默认选项
            cbxDoor1ReaderOption.SelectedIndex = 2;
            cbxDoor2ReaderOption.SelectedIndex = 2;
            cbxDoor3ReaderOption.SelectedIndex = 2;
            cbxDoor4ReaderOption.SelectedIndex = 2;
            #endregion

            #region 继电器参数默认选项
            cbxDoor1RelayOption.SelectedIndex = 0;
            cbxDoor2RelayOption.SelectedIndex = 0;
            cbxDoor3RelayOption.SelectedIndex = 0;
            cbxDoor4RelayOption.SelectedIndex = 0;
            #endregion

            #region 门工作方式_星期
            cbxWeek.SelectedIndex = 0;
            #endregion

            #region 重复读卡间隔_检测模式
            cbxDetectionMode.SelectedIndex = 0;

            #endregion

            WeekTimeGroupPushButtonDto.InitTimeGroup();
            cmbPushButtonWeekday.SelectedIndex = 0;

            WeekTimeGroupSensorAlarmDto.InitTimeGroup();

            #region
            string[] tgAnyCard = new string[64];
            for (int i = 1; i <= 64; i++)
            {
                tgAnyCard[i - 1] = i.ToString();
            }
            cmbAnyCardTimeGroup.Items.AddRange(tgAnyCard);
            cmbAnyCardTimeGroup.SelectedIndex = 0;
            #endregion

            #region 区域互锁
            string[] InterLockNum = new string[63];
            for (int i = 1; i < 64; i++)
            {
                InterLockNum[i - 1] = i.ToString();
            }
            cmbNum.Items.AddRange(InterLockNum);
            cmbNum.SelectedIndex = 0;
            #endregion

            cmbManyCardOpenMode.SelectedIndex = 0;
            cmbAntiPassback.SelectedIndex = 0;

            cmbGroupNum.Items.Clear();
            cmbGroupNum.Items.AddRange(new string[] { "1", "2", "3", "4", "5" });
            cmbGroupType.SelectedIndex = 0;
            //int[] ilist = new int[5];
            //for (int i = 1; i < 6; i++)
            //{
            //    cmbGroupNum.Items.Add(i);
            //}

            cmbGroupNum.SelectedIndex = 0;
            InitCardDataList();
        }


        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            GetLanguage(label22);
            var door1 = GetLanguage("cBoxDoor1");
            var door2 = GetLanguage("cBoxDoor2");
            var door3 = GetLanguage("cBoxDoor3");
            var door4 = GetLanguage("cBoxDoor4");
            cBoxDoor1.Text = door1;
            cBoxDoor2.Text = door2;
            cBoxDoor3.Text = door3;
            cBoxDoor4.Text = door4;
            label24.Text = door1 + ":";
            label5.Text = door2 + ":";
            label12.Text = door3 + ":";
            label11.Text = door4 + ":";

            label16.Text = door1 + ":";
            label15.Text = door2 + ":";
            label14.Text = door3 + ":";
            label13.Text = door4 + ":";
            GetLanguage(tabPage1);
            GetLanguage(groupBox5);
            GetLanguage(btnReadReaderOption);
            GetLanguage(btnWriteReaderOption);
            GetLanguage(groupBox6);
            GetLanguage(btnReadRelayOption);
            GetLanguage(btnWriteRelayOption);
            GetLanguage(groupBox7);
            GetLanguage(btnOpenDoor);
            GetLanguage(btnCloseDoor);
            GetLanguage(btnHoldOpenDoor);
            GetLanguage(btnOpenDoor_CheckNum);
            GetLanguage(groupBox8);
            GetLanguage(btnLockDoor);
            GetLanguage(btnUnLockDoor);
            GetLanguage(groupBox11);
            GetLanguage(label38);
            GetLanguage(label39);
            GetLanguage(btnReadRelayReleaseTime);
            GetLanguage(btnWriteRelayReleaseTime);
            GetLanguage(groupBox16);
            GetLanguage(dataGridView1);
            GetLanguage(btnReadDoorWorkSetting);
            GetLanguage(btnWriteDoorWorkSetting);
            GetLanguage(groupBox9);
            GetLanguage(label57);
            GetLanguage(rBtnNoDoorWorkSetting);
            GetLanguage(rBtnDoorWorkSetting);
            GetLanguage(label17);
            GetLanguage(rBtnOpenDoorWay1);
            GetLanguage(rBtnOpenDoorWay2);
            GetLanguage(rBtnOpenDoorWay3);
            GetLanguage(rBtnOpenDoorWay4);
            GetLanguage(label18);
            GetLanguage(rBtnDoorTriggerMode1);
            GetLanguage(rBtnDoorTriggerMode2);
            GetLanguage(rBtnDoorTriggerMode3);
            GetLanguage(label19);
            GetLanguage(label20);
            GetLanguage(label21);
            GetLanguage(label29);
            GetLanguage(label28);
            GetLanguage(label33);
            GetLanguage(label32);
            GetLanguage(label37);
            GetLanguage(label36);
            GetLanguage(label25);
            GetLanguage(label23);
            GetLanguage(label27);
            GetLanguage(label26);
            GetLanguage(label31);
            GetLanguage(label30);
            GetLanguage(label35);
            GetLanguage(label34);
            GetLanguage(btnReadWorkSetting);
            GetLanguage(btnWriteWorkSetting);
            GetLanguage(groupBox10);
            GetLanguage(dataGridView2);
            GetLanguage(rBtnNoAutoLockedSetting);
            GetLanguage(rBtnAutoLockedSetting);
            GetLanguage(btnReadAutoLockedSetting);
            GetLanguage(btnWriteAutoLockedSetting);

            GetLanguage(tabPage3);
            GetLanguage(groupBox12);
            GetLanguage(rBtnNoReaderInterval);
            GetLanguage(rBtnReaderInterval);
            GetLanguage(label41);
            GetLanguage(label40);
            GetLanguage(groupBox13);
            GetLanguage(rBtnNotAnti);
            GetLanguage(rBtnAnti);
            GetLanguage(label42);
            GetLanguage(rBtnCheckInOut1);
            GetLanguage(rBtnCheckInOut2);
            GetLanguage(groupBox14);
            GetLanguage(label43);
            GetLanguage(lbNormallyOpen);
            GetLanguage(label62);
            GetLanguage(groupBox15);
            GetLanguage(cbAnyCardUse);
            GetLanguage(cbAnyCardAuto);
            GetLanguage(label44);
            GetLanguage(label63);
            GetLanguage(groupBox18);
            GetLanguage(cbInOutSideReadOpenSettingUse);
            GetLanguage(groupBox17);
            GetLanguage(cbVoiceBroadcastSettingUse);
            GetLanguage(groupBox19);
            GetLanguage(cbManageKeyboardSettingUse);
            GetLanguage(label64);
            GetLanguage(groupBox20);
            GetLanguage(label65);
            GetLanguage(label66);
            GetLanguage(lbSN);
            GetLanguage(lbIP);
            GetLanguage(lbPort);
            GetLanguage(groupBox21);
            GetLanguage(label70);
            GetLanguage(lbAreaType2);
            GetLanguage(lbAreaCode);
            GetLanguage(lbNum2);
            GetLanguage(lbIP2);
            GetLanguage(lbPort2);
            GetLanguage(tabPage4);
            GetLanguage(groupBox22);
            GetLanguage(label67);
            GetLanguage(label69);
            GetLanguage(label68);
            GetLanguage(label71);
            GetLanguage(label72);
            GetLanguage(lbgrouptype);
            GetLanguage(lbgroupnum);
            GetLanguage(cbConvertHex);
            GetLanguage(btnDeleteGroup);
            GetLanguage(btnAutoFill);
            GetLanguage(groupBox23);
            GetLanguage(cbReaderAlarmUse);
            GetLanguage(groupBox24);
            GetLanguage(label73);
            GetLanguage(cbInDoorUse);
            GetLanguage(label75);
            GetLanguage(label77);
            GetLanguage(label79);
            GetLanguage(label74);
            GetLanguage(cbOutDoorUse);
            GetLanguage(label76);
            GetLanguage(label78);
            GetLanguage(label80);
            GetLanguage(dataGridView5);
            GetLanguage(tabPage2);
            GetLanguage(groupBox1);
            GetLanguage(label1);
            GetLanguage(label81);
            GetLanguage(groupBox2);
            GetLanguage(label2);
            GetLanguage(label3);
            GetLanguage(label4);
            GetLanguage(groupBox3);
            GetLanguage(label8);
            GetLanguage(label6);
            GetLanguage(label7);
            GetLanguage(groupBox4);
            GetLanguage(label9);
            GetLanguage(label10);
            GetLanguage(label97);
            GetLanguage(label96);
            GetLanguage(label93);
            GetLanguage(label92);
            GetLanguage(label89);
            GetLanguage(label88);
            GetLanguage(label85);
            GetLanguage(label84);
            GetLanguage(label95);
            GetLanguage(label94);
            GetLanguage(label91);
            GetLanguage(label90);
            GetLanguage(label87);
            GetLanguage(label86);
            GetLanguage(label83);
            GetLanguage(label82);
            GetLanguage(label61);
            GetLanguage(label60);
            GetLanguage(label56);
            GetLanguage(label55);
            GetLanguage(label52);
            GetLanguage(label51);
            GetLanguage(label48);
            GetLanguage(label47);
            GetLanguage(label59);
            GetLanguage(label58);
            GetLanguage(label54);
            GetLanguage(label53);
            GetLanguage(label50);
            GetLanguage(label49);
            GetLanguage(label46);
            GetLanguage(label45);
            var readBtn = GetLanguage("btnReadRelayOption");
            var write = GetLanguage("btnWriteRelayOption");

            btnReadReaderInterval.Text = readBtn;
            btnReadAntiPassback.Text = readBtn;
            btnReadPushButton.Text = readBtn;
            btnReadInOutSideReadOpenSetting.Text = readBtn;
            btnReadVoiceBroadcastSetting.Text = readBtn;
            btnReadAnyCard.Text = readBtn;
            btnReadManageKeyboardSetting.Text = readBtn;
            btnReadAreaAntiPassback.Text = readBtn;
            btnReadInterLockSetting.Text = readBtn;

            btnReadManyCardOpenMode.Text = readBtn;
            btnReadReaderAlarm.Text = readBtn;
            btnReadReadCardAndTakePictures.Text = readBtn;

            butReadInvalidCardAlarmOption.Text = readBtn;
            butReadOvertimeAlarmSetting.Text = readBtn;
            butReadSensorAlarmSetting.Text = readBtn;

            btnWriteReadCardAndTakePictures.Text = write;
            btnWriteReaderAlarm.Text = write;
            btnWriteManyCardOpenMode.Text = write;

            ButWriteInvalidCardAlarmOption.Text = write;
            butWriteOvertimeAlarmSetting.Text = write;
            butWriteSensorAlarmSetting.Text = write;

            btnWriteInterLockSetting.Text = write;
            btnWriteAreaAntiPassback.Text = write;
            btnWriteManageKeyboardSetting.Text = write;
            btnWriteAnyCard.Text = write;
            btnWriteWriteVoiceBroadcastSetting.Text = write;
            btnWriteInOutSideReadOpenSetting.Text = write;
            btnWritePushButton.Text = write;
            btnWriteAntiPassback.Text = write;
            btnWriteReaderInterval.Text = write;


            string[] relayOption = new string[] { "COM & NC", "COM & NO" };
            var doorRelayOption = GetLanguage("DoorRelayOption");
            cbxDoor1RelayOption.Items.Clear();
            cbxDoor1RelayOption.Items.AddRange(relayOption);
            cbxDoor1RelayOption.Items.Add(doorRelayOption);

            cbxDoor2RelayOption.Items.Clear();
            cbxDoor2RelayOption.Items.AddRange(relayOption);
            cbxDoor2RelayOption.Items.Add(doorRelayOption);

            cbxDoor3RelayOption.Items.Clear();
            cbxDoor3RelayOption.Items.AddRange(relayOption);
            cbxDoor3RelayOption.Items.Add(doorRelayOption);

            cbxDoor4RelayOption.Items.Clear();
            cbxDoor4RelayOption.Items.AddRange(relayOption);
            cbxDoor4RelayOption.Items.Add(doorRelayOption);

            var detectionMode = GetLanguage("cbxDetectionMode").Split(',');
            cbxDetectionMode.Items.Clear();
            cbxDetectionMode.Items.AddRange(detectionMode);
            string[] str = GetLanguage("Door1ReaderOption").Split(',');
            cbxDoor1ReaderOption.Items.Clear();
            cbxDoor1ReaderOption.Items.AddRange(str);
            cbxDoor1ReaderOption.SelectedIndex = 0;

            var areaType = GetLanguage("cmbAreaType").Split(',');
            cmbAreaType.Items.Clear();
            cmbAreaType.Items.AddRange(areaType);
            cmbAreaType.SelectedIndex = 0;


            cmbInterLockSettingAreaType.Items.Clear();
            cmbInterLockSettingAreaType.Items.AddRange(areaType);
            cmbInterLockSettingAreaType.SelectedIndex = 0;

            cbxDoor2ReaderOption.Items.Clear();
            cbxDoor2ReaderOption.Items.AddRange(str);
            cbxDoor2ReaderOption.SelectedIndex = 0;

            cbxDoor3ReaderOption.Items.Clear();
            cbxDoor3ReaderOption.Items.AddRange(str);
            cbxDoor3ReaderOption.SelectedIndex = 0;

            cbxDoor4ReaderOption.Items.Clear();
            cbxDoor4ReaderOption.Items.AddRange(str);
            cbxDoor4ReaderOption.SelectedIndex = 0;

            cmdDoorNum.Items.Clear();
            cmdDoorNum.Items.AddRange(new string[] { "1", "2", "3", "4" });
            cmdDoorNum.SelectedIndex = 0;
            cmbInterval.Items.Clear();
            string[] time = new string[256];
            string disable = GetLanguage("Msg2");
            string seconds = GetLanguage("Msg1");
            for (int i = 0; i < 256; i++)
            {
                time[i] = i.ToString() + seconds;
                if (time[0] == ("0" + seconds))
                    time[0] = disable;
            }
            cmbInterval.Items.AddRange(time);
            cmbInterval.Items.Add("65535" + seconds);
            cmbInterval.SelectedIndex = cmbInterval.Items.Count - 1;
            string temp1 = GetLanguage("Msg3");
            string temp2 = GetLanguage("Msg4");
            string temp3 = GetLanguage("Msg5");
            string use = GetLanguage("Msg6");
            cmbVerifyType.Items.Clear();
            if (!CheckProtocolTypeIs89H())
            {
                cmbVerifyType.Items.AddRange(new string[] { disable, temp1 });
            }
            else
            {
                cmbVerifyType.Items.AddRange(new string[] { disable, temp2, temp1, temp3 });
            }
            cmbVerifyType.SelectedIndex = 0;

            cmdAlarmPassword.Items.Clear();
            cmdAlarmPassword.Items.AddRange(new string[] { use, disable });
            cmdAlarmPassword.SelectedIndex = 0;


            cmbAlarmOption.Items.Clear();
            cmbAlarmOption.Items.AddRange(GetLanguage("AlarmOption").Split(','));
            cmbAlarmOption.SelectedIndex = 1;

            cmbManyCardOpenMode.Items.Clear();
            cmbManyCardOpenMode.Items.AddRange(GetLanguage("cmbManyCardOpenMode").Split(','));
            cmbManyCardOpenMode.SelectedIndex = 0;

            cmbAntiPassback.Items.Clear();
            cmbAntiPassback.Items.AddRange(GetLanguage("cmbAntiPassback").Split(','));
            cmbAntiPassback.SelectedIndex = 0;

            cmbGroupType.Items.Clear();
            cmbGroupType.Items.AddRange(GetLanguage("cmbGroupType").Split(','));
            cmbGroupType.SelectedIndex = 0;

            cmbOvertimeAlarmSetting.Items.Clear();
            cmbOvertimeAlarmSetting.Items.AddRange(new string[] { disable, use });
            cmbOvertimeAlarmSetting.SelectedIndex = 0;

            cmbAlarm.Items.Clear();
            cmbAlarm.Items.AddRange(new string[] { GetLanguage("Msg11"), GetLanguage("Msg12") });
            cmbAlarm.SelectedIndex = 0;

            cmbOverTime.Items.Clear();
            string[] time1 = new string[100];

            for (int i = 0; i < 100; i++)
            {
                time1[i] = i + 1 + seconds;
            }
            cmbOverTime.Items.AddRange(time1);
            cmbOverTime.SelectedIndex = 0;

            cmbSensorAlarmSetting.Items.Clear();
            cmbSensorAlarmSetting.Items.AddRange(new string[] { use, disable });
            cmbSensorAlarmSetting.SelectedIndex = 0;
            var week = GetLanguage("WeekOption").Split(',');
            cmbWeek.Items.Clear();
            cmbWeek.Items.AddRange(week);
            cmbWeek.SelectedIndex = 0;

            cmbPushButtonWeekday.Items.Clear();
            cmbPushButtonWeekday.Items.AddRange(week);
            cmbPushButtonWeekday.SelectedIndex = 0;

            cmdInvalidCardAlarmOptionUse.Items.Clear();
            cmdInvalidCardAlarmOptionUse.Items.AddRange(new string[] { use, disable });
            cmdInvalidCardAlarmOptionUse.SelectedIndex = 0;

            cmbReadInvalidCardTime.Items.Clear();
            string[] array = new string[256];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i.ToString();
            }
            cmbReadInvalidCardTime.Items.AddRange(array);
            cmbReadInvalidCardTime.SelectedIndex = 0;
        }
        private void InitGridReaderWork()
        {
            Random r = new Random();
            for (int i = 0; i < 7; i++)
            {
                WeekTimeGroupDto dto = new WeekTimeGroupDto();
                dto.WeekDay = GetWeekStr(i);
                dto.Ex = "-";
                dto.IsEx = "true";
                ListWeekTimeGroupDto.Add(dto);
                ListAutoLockedDto.Add(dto);
                //sb.AppendLine(GetWeekStr(i));
                for (int j = 0; j < 8; j++)
                {
                    dto = new WeekTimeGroupDto();
                    dto.WeekDay = (j + 1).ToString();

                    int checkway = r.Next(4);
                    dto.id0 = checkway == 0; dto.id1 = checkway == 1; dto.id2 = checkway == 2; dto.id3 = checkway == 3;
                    dto.WeekDayIndex = i;
                    if (j == 0)
                    {
                        dto.StartTime = "00:00";
                        dto.EndTime = "23:59";
                    }
                    else
                    {
                        dto.StartTime = "00:00";
                        dto.EndTime = "00:00";
                    }
                    ListWeekTimeGroupDto.Add(dto);
                    ListAutoLockedDto.Add(dto);
                }
            }
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new BindingList<WeekTimeGroupDto>(ListWeekTimeGroupDto);

            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = new BindingList<WeekTimeGroupDto>(ListAutoLockedDto);
            //for (int i = 0; i < 7; i++)
            //{
            //    for (int j = 2; j < dataGridView1.Columns.Count; j++)
            //    {

            //    }

            //}
            Invoke(() =>
            {


            });
        }

        #region "胁迫报警密码"
        private void AlarmPassword()
        {

        }
        //报警模式
        private void AlarmOption()
        {

        }
        private void butReadAlarmPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new Door8800.Door.DoorPort_Parameter(int.Parse(cmdDoorNum.Text));
            var cmd = new Door8800.Door.AlarmPassword.ReadAlarmPassword(cmdDtl, par);
            mMainForm.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door8800.Door.AlarmPassword.AlarmPassword_Result;
                Invoke(() =>
                {
                    cmdAlarmPassword.SelectedIndex = result.Use ? 0 : 1;
                    cmbAlarmOption.SelectedIndex = (result.AlarmOption - 1);
                    Password.Text = result.Password;

                });

                mMainForm.AddLog($"{GetLanguage("Msg7")}{result.DoorNum},{GetLanguage("Msg8")}:{result.Use},{GetLanguage("Msg9")}：{result.Password},{GetLanguage("Msg10")}：{result.AlarmOption}");
            };
        }
        private void butWriteAlarmPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            bool use = (cmdAlarmPassword.SelectedIndex == 0);
            byte door = byte.Parse(cmdDoorNum.Text);
            String pwd = Password.Text.ToString();
            int alarmOption = cmbAlarmOption.SelectedIndex + 1;

            var par = new Door8800.Door.AlarmPassword.WriteAlarmPassword_parameter(door, use, pwd, alarmOption);
            var cmd = new Door8800.Door.AlarmPassword.WriteAlarmPassword(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                // mMainForm.AddLog($"命令成功：门号:{door},功能开关:{use},密码：{Password},报警选项：{alarmOption}");
                mMainForm.AddLog($"{GetLanguage("Msg7")}{door},{GetLanguage("Msg8")}:{use},{GetLanguage("Msg9")}：{Password},{GetLanguage("Msg10")}：{alarmOption}");
            };

        }
        #endregion

        #region "开门超时提示参数"
        private void OvertimeAlarmSetting()
        {

        }

        private void Alarm()
        {

        }

        private void Overtime()
        {

        }

        /// <summary>
        /// 读取开门超时提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butReadOvertimeAlarmSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new Door8800.Door.DoorPort_Parameter(int.Parse(cmdDoorNum.Text));
            var cmd = new Door8800.Door.OvertimeAlarmSetting.ReadOvertimeAlarmSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door8800.Door.OvertimeAlarmSetting.OvertimeAlarmSetting_Result;
                Invoke(() =>
                {
                    cmbOvertimeAlarmSetting.SelectedIndex = result.Use ? 1 : 0;
                    cmbOverTime.SelectedIndex = result.Overtime - 1;
                    cmdAlarmPassword.SelectedIndex = result.Alarm ? 0 : 1;
                });
                string usetip = result.Use ? GetLanguage("Msg6") : GetLanguage("Msg2");
                string Alarmtip = result.Alarm ? GetLanguage("Msg11") : GetLanguage("Msg12");
                mMainForm.AddCmdLog(cmde, $"{GetLanguage("Msg7")}{result.DoorNum},{GetLanguage("Msg8")}:{usetip}，{GetLanguage("Msg13")}：{result.Overtime},{GetLanguage("Msg14")} ：{Alarmtip}");
            };
        }

        /// <summary>
        /// 设置读取开门超时提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butWriteOvertimeAlarmSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();

            bool use = (cmbOvertimeAlarmSetting.SelectedIndex == 0);
            byte door = byte.Parse(cmdDoorNum.Text);
            byte overtime = byte.Parse((cmbOverTime.SelectedIndex + 1).ToString());
            bool alarm = (cmdAlarmPassword.SelectedIndex == 0);

            var par = new Door8800.Door.OvertimeAlarmSetting.WriteOvertimeAlarmSetting_Parameter(door, use, overtime, alarm);
            var cmd = new Door8800.Door.OvertimeAlarmSetting.WriteOvertimeAlarmSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"{GetLanguage("Msg7")}{door},{GetLanguage("Msg8")}:{use}");
            };
        }
        #endregion

        #region "门磁报警参数"
        private void SensorAlarmSetting()
        {

        }
        //门磁报警不生效时段
        private void Week()
        {

        }
        //改变下拉框
        private void cmbSensorAlarmSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSensorAlarmSetting.Text == GetLanguage("Msg6"))
            {
                cmbWeek.Show();
                label10.Show();

            }
            else
            {
                cmbWeek.Hide();
                label10.Hide();
            }


        }
        private void butReadSensorAlarmSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new Door8800.Door.DoorPort_Parameter(int.Parse(cmdDoorNum.Text));
            var cmd = new Door8800.Door.SensorAlarmSetting.ReadSensorAlarmSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door8800.Door.SensorAlarmSetting.SensorAlarmSetting_Result;
                //cmbSensorAlarmSetting
                Invoke(() =>
                {
                    cmdDoorNum.Text = result.DoorNum.ToString();

                    WeekTimeGroupSensorAlarmDto = result.WeekTimeGroup;
                    SetAllTimePicker(plSensorAlarm, "beginSATP", "endSATP", WeekTimeGroupSensorAlarmDto.GetItem(0));
                });
                mMainForm.AddLog($"{GetLanguage("Msg7")}{result.DoorNum},{GetLanguage("Msg8")}:{result.Use}");
            };
        }
        private void butWriteSensorAlarmSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            bool use = (cmbSensorAlarmSetting.SelectedIndex == 0);
            byte door = byte.Parse(cmdDoorNum.Text);
            var par = new Door8800.Door.SensorAlarmSetting.WriteSensorAlarmSetting_Parameter(door, use, WeekTimeGroupSensorAlarmDto);
            var cmd = new Door8800.Door.SensorAlarmSetting.WriteSensorAlarmSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"{GetLanguage("Msg7")}{door},{GetLanguage("Msg8")}:{use}");
            };
        }

        private void BeginSATP_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupSensorAlarmDto, cmbWeek.SelectedIndex, int.Parse(dtp.Name.Substring(9)) - 1, 1, dtp.Value);
        }

        private void EndSATP_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupSensorAlarmDto, cmbWeek.SelectedIndex, int.Parse(dtp.Name.Substring(7)) - 1, 2, dtp.Value);
        }

        private void CmbWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeekTimeGroupSensorAlarmDto != null)
            {
                var day = WeekTimeGroupSensorAlarmDto.GetItem(cmbWeek.SelectedIndex);
                SetAllTimePicker(plSensorAlarm, "beginSATP", "endSATP", day);
            }
        }
        #endregion

        #region 读卡器参数读写
        public bool CheckProtocolTypeIs89H()
        {
            return mMainForm.CheckProtocolTypeIs89H();
        }

        private void BtnReadReaderOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReaderOption cmd = new ReadReaderOption(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReaderOption_Result result = cmde.Command.getResult() as ReaderOption_Result;
                StringBuilder sb = new StringBuilder();
                string[] str = GetLanguage("Door1ReaderOption").Split(',');
                //str[0] = "字节数：【1、韦根26(三字节)】";
                //str[1] = "字节数：【2、韦根34(四字节)】";
                //str[2] = "字节数：【3、韦根26(二字节)】";
                //str[3] = "字节数：【4、禁用】";

                Invoke(() =>
                {
                    int count = 2;
                    if (CheckProtocolTypeIs89H())
                    {
                        count = 4;
                    }
                    string sDoor = GetLanguage("Msg15");
                    for (int i = 0; i < 4; i++)
                    {
                        if (i + 1 > count)
                        {
                            break;
                        }
                        //门1读卡器字节数
                        if (i == 0)
                        {
                            cbxDoor1ReaderOption.SelectedIndex = result.Door[i] - 1;
                        }
                        //门2读卡器字节数
                        if (i == 1)
                        {
                            cbxDoor2ReaderOption.SelectedIndex = result.Door[i] - 1;
                        }
                        //门3读卡器字节数
                        if (i == 2)
                        {
                            cbxDoor3ReaderOption.SelectedIndex = result.Door[i] - 1;
                        }
                        //门4读卡器字节数
                        if (i == 3)
                        {
                            cbxDoor4ReaderOption.SelectedIndex = result.Door[i] - 1;
                        }
                        sb.Append(sDoor + (i + 1) + str[result.Door[i] - 1]);
                    }
                });
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteReaderOption_Click(object sender, EventArgs e)
        {


            byte[] Door = new byte[4];
            //门1读卡器字节数
            Door[0] = (byte)(cbxDoor1ReaderOption.SelectedIndex + 1);
            Door[1] = (byte)(cbxDoor2ReaderOption.SelectedIndex + 1);
            if (CheckProtocolTypeIs89H())
            {
                Door[2] = (byte)(cbxDoor3ReaderOption.SelectedIndex + 1);
                Door[3] = (byte)(cbxDoor4ReaderOption.SelectedIndex + 1);
            }
            else
            {
                Door[2] = 0;
                Door[3] = 0;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            if (CheckProtocolTypeIs89H())
            {
                ReaderOption_Parameter par = new ReaderOption_Parameter(Door);
                WriteReaderOption<ReaderOption_Parameter> cmd = new WriteReaderOption<ReaderOption_Parameter>(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }
            else if (CheckProtocolTypeIs89H())
            {
                Door89H.Door.ReaderOption.ReaderOption_Parameter par = new Door89H.Door.ReaderOption.ReaderOption_Parameter(Door);
                WriteReaderOption<Door89H.Door.ReaderOption.ReaderOption_Parameter> cmd = new WriteReaderOption<Door89H.Door.ReaderOption.ReaderOption_Parameter>(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }
        }
        #endregion

        #region 继电器参数读写
        private void BtnReadRelayOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadRelayOption cmd = new ReadRelayOption(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                RelayOption_Result result = cmde.Command.getResult() as RelayOption_Result;
                StringBuilder sb = new StringBuilder();
                string[] str = new string[3];
                str[0] = GetLanguage("Msg16");
                str[1] = GetLanguage("Msg17");
                str[2] = GetLanguage("Msg18");

                Invoke(() =>
                {
                    var door = GetLanguage("Msg15");
                    for (int i = 0; i < 4; i++)
                    {
                        //门1继电器参数
                        if (i == 0)
                        {
                            if (result.Relay[i] == 1)
                            {
                                cbxDoor1RelayOption.SelectedIndex = 0;
                                sb.Append(door + (i + 1) + str[0]);
                            }
                            else if (result.Relay[i] == 2)
                            {
                                cbxDoor1RelayOption.SelectedIndex = 1;
                                sb.Append(door + (i + 1) + str[1]);
                            }
                            else if (result.Relay[i] == 3)
                            {
                                cbxDoor1RelayOption.SelectedIndex = 2;
                                sb.Append(door + (i + 1) + str[2]);
                            }
                        }
                        //门2继电器参数
                        if (i == 1)
                        {
                            if (result.Relay[i] == 1)
                            {
                                cbxDoor2RelayOption.SelectedIndex = 0;
                                sb.Append(door + (i + 1) + str[0]);
                            }
                            else if (result.Relay[i] == 2)
                            {
                                cbxDoor2RelayOption.SelectedIndex = 1;
                                sb.Append(door + (i + 1) + str[1]);
                            }
                            else if (result.Relay[i] == 3)
                            {
                                cbxDoor2RelayOption.SelectedIndex = 2;
                                sb.Append(door + (i + 1) + str[2]);
                            }
                        }
                        //门3继电器参数
                        if (i == 2)
                        {
                            if (result.Relay[i] == 1)
                            {
                                cbxDoor3RelayOption.SelectedIndex = 0;
                                sb.Append(door + (i + 1) + str[0]);
                            }
                            else if (result.Relay[i] == 2)
                            {
                                cbxDoor3RelayOption.SelectedIndex = 1;
                                sb.Append(door + (i + 1) + str[1]);
                            }
                            else if (result.Relay[i] == 3)
                            {
                                cbxDoor3RelayOption.SelectedIndex = 2;
                                sb.Append(door + (i + 1) + str[2]);
                            }
                        }
                        //门4继电器参数
                        if (i == 3)
                        {
                            if (result.Relay[i] == 1)
                            {
                                cbxDoor4RelayOption.SelectedIndex = 0;
                                sb.Append(door + (i + 1) + str[0]);
                            }
                            else if (result.Relay[i] == 2)
                            {
                                cbxDoor4RelayOption.SelectedIndex = 1;
                                sb.Append(door + (i + 1) + str[1]);
                            }
                            else if (result.Relay[i] == 3)
                            {
                                cbxDoor4RelayOption.SelectedIndex = 2;
                                sb.Append(door + (i + 1) + str[2]);
                            }
                        }
                    }
                });
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteRelayOption_Click(object sender, EventArgs e)
        {
            byte[] Relay = new byte[4];
            Relay[0] = Convert.ToByte(cbxDoor1RelayOption.SelectedIndex + 1); //门1继电器参数
            Relay[1] = Convert.ToByte(cbxDoor2RelayOption.SelectedIndex + 1); //门2继电器参数
            Relay[2] = Convert.ToByte(cbxDoor3RelayOption.SelectedIndex + 1); //门3继电器参数
            Relay[3] = Convert.ToByte(cbxDoor4RelayOption.SelectedIndex + 1); //门4继电器参数

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteRelayOption cmd = new WriteRelayOption(cmdDtl, new RelayOption_Parameter(Relay));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 远程开关门
        private void BtnOpenDoor_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }

            DoorDetail<bool> Door = new DoorDetail<bool>();
            Door.SetDoor(1, cBoxDoor1.Checked);
            Door.SetDoor(2, cBoxDoor2.Checked);
            Door.SetDoor(3, cBoxDoor3.Checked);
            Door.SetDoor(4, cBoxDoor4.Checked);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            OpenDoor cmd = new OpenDoor(cmdDtl, new Remote_Parameter(Door));
            mMainForm.AddCommand(cmd);
        }

        private void BtnCloseDoor_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }

            DoorDetail<bool> Door = new DoorDetail<bool>();
            Door.SetDoor(1, cBoxDoor1.Checked);
            Door.SetDoor(2, cBoxDoor2.Checked);
            Door.SetDoor(3, cBoxDoor3.Checked);
            Door.SetDoor(4, cBoxDoor4.Checked);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            CloseDoor cmd = new CloseDoor(cmdDtl, new Remote_Parameter(Door));
            mMainForm.AddCommand(cmd);
        }

        private void BtnHoldOpenDoor_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }

            DoorDetail<bool> Door = new DoorDetail<bool>();
            Door.SetDoor(1, cBoxDoor1.Checked);
            Door.SetDoor(2, cBoxDoor2.Checked);
            Door.SetDoor(3, cBoxDoor3.Checked);
            Door.SetDoor(4, cBoxDoor4.Checked);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            HoldDoor cmd = new HoldDoor(cmdDtl, new Remote_Parameter(Door));
            mMainForm.AddCommand(cmd);
        }

        private void BtnOpenDoor_CheckNum_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }

            byte CheckNum;
            CheckNum = Convert.ToByte(StringUtility.GetRandomNum(1, 254));

            DoorDetail<bool> Door = new DoorDetail<bool>();
            Door.SetDoor(1, cBoxDoor1.Checked);
            Door.SetDoor(2, cBoxDoor2.Checked);
            Door.SetDoor(3, cBoxDoor3.Checked);
            Door.SetDoor(4, cBoxDoor4.Checked);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            OpenDoor_CheckNum cmd = new OpenDoor_CheckNum(cmdDtl, new Remote_CheckNum_Parameter(Door, CheckNum));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 门的锁定与解除
        private void BtnLockDoor_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }

            DoorDetail<bool> Door = new DoorDetail<bool>();
            Door.SetDoor(1, cBoxDoor1.Checked);
            Door.SetDoor(2, cBoxDoor2.Checked);
            Door.SetDoor(3, cBoxDoor3.Checked);
            Door.SetDoor(4, cBoxDoor4.Checked);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            LockDoor cmd = new LockDoor(cmdDtl, new Remote_Parameter(Door));
            mMainForm.AddCommand(cmd);
        }

        private void BtnUnLockDoor_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }

            DoorDetail<bool> Door = new DoorDetail<bool>();
            Door.SetDoor(1, cBoxDoor1.Checked);
            Door.SetDoor(2, cBoxDoor2.Checked);
            Door.SetDoor(3, cBoxDoor3.Checked);
            Door.SetDoor(4, cBoxDoor4.Checked);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            UnlockDoor cmd = new UnlockDoor(cmdDtl, new Remote_Parameter(Door));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 读卡认证方式的读写
        private void BtnReadDoorWorkSetting_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReaderWorkSetting cmd = new ReadReaderWorkSetting(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                byte[] ByteDoorAlarmStateSet = null;
                BitArray bitSet = null;
                ReaderWorkSetting_Result result = cmde.Command.getResult() as ReaderWorkSetting_Result;
                ListWeekTimeGroupDto.Clear();
                for (int i = 0; i < 7; i++)
                {
                    WeekTimeGroupDto dto = new WeekTimeGroupDto();
                    dto.WeekDay = GetWeekStr(i);
                    dto.Ex = "-";
                    dto.IsEx = "true";
                    ListWeekTimeGroupDto.Add(dto);

                    for (int j = 0; j < 8; j++)
                    {
                        var tz = result.weekTimeGroup_ReaderWork.GetItem(i).GetItem(j) as TimeSegment_ReaderWork;
                        ByteDoorAlarmStateSet = new byte[] { tz.GetCheckWay() };
                        bitSet = new BitArray(ByteDoorAlarmStateSet);

                        string strCheckWay = Convert.ToString(tz.GetCheckWay(), 2).PadLeft(4, '0');

                        dto = new WeekTimeGroupDto();
                        dto.WeekDay = (j + 1).ToString();
                        dto.WeekDayIndex = i;
                        dto.StartTime = result.weekTimeGroup_ReaderWork.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                        dto.EndTime = result.weekTimeGroup_ReaderWork.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                        dto.id0 = strCheckWay[3] == '1';// bitSet[0];
                        dto.id1 = strCheckWay[2] == '1';// bitSet[1];
                        dto.id2 = strCheckWay[1] == '1';// bitSet[2];
                        dto.id3 = strCheckWay[0] == '1';// bitSet[3];
                        ListWeekTimeGroupDto.Add(dto);
                    }
                }
                Invoke(() =>
                {
                    dataGridView1.DataSource = new BindingList<WeekTimeGroupDto>(ListWeekTimeGroupDto);
                });


                /*
                string tip = "门认证方式_门" + result.Door.ToString() + "，时段详情：";
                StringBuilder sbCheckWay = new StringBuilder(8);
                StringBuilder sbCheckWayStr = new StringBuilder();
                byte[] ByteDoorAlarmStateSet = null;
                BitArray bitSet = null;
                sb.AppendLine(tip);
                for (int i = 0; i < 7; i++)
                {
                    sb.AppendLine(GetWeekStr(i));
                    for (int j = 0; j < 8; j++)
                    {
                        sb.Append("  时段" + (j + 1) + "：" + StringUtility.TimeHourAndMinuteStr(result.weekTimeGroup_ReaderWork.GetItem(i).GetItem(j).GetBeginTime(), result.weekTimeGroup_ReaderWork.GetItem(i).GetItem(j).GetEndTime()));
                        var tz = result.weekTimeGroup_ReaderWork.GetItem(i).GetItem(j) as TimeSegment_ReaderWork;
                        ByteDoorAlarmStateSet = new byte[] { tz.GetCheckWay() };
                        bitSet = new BitArray(ByteDoorAlarmStateSet);
                        sbCheckWay.Clear();
                        sbCheckWayStr.Clear();
                        for (int k = 7; k >= 0; k--)
                        {
                            sbCheckWay.Append(bitSet[k] ? 1 : 0);
                            if (k == 0)
                            {
                                sbCheckWayStr.Append(bitSet[k] ? "读卡开门；" : "");
                            }
                            else if (k == 1)
                            {
                                sbCheckWayStr.Append(bitSet[k] ? "密码开门；" : "");
                            }
                            else if (k == 2)
                            {
                                sbCheckWayStr.Append(bitSet[k] ? "读卡加密码；" : "");
                            }
                            else if (k == 3)
                            {
                                sbCheckWayStr.Append(bitSet[k] ? "手动输入卡加密码" : "");
                            }
                        }
                        sb.AppendLine("  认证方式：【" + sbCheckWay + "(" + sbCheckWayStr + ")】");
                    }
                }

                Invoke(() =>
                {
                    txtDoorWorkSetting.Text = sb.ToString();
                });
                */
            };
        }

        /// <summary>
        /// 获得数值代表的星期
        /// </summary>
        /// <param name="index">数值（0-6，0代表星期一...6代表星期日）</param>
        /// <returns></returns>
        private string GetWeekStr(int index)
        {
            string weekStr = string.Empty;
            if (index == 0)
            {
                return GetLanguage("Msg20");
            }
            else if (index == 1)
            {
                return GetLanguage("Msg21");
            }
            else if (index == 2)
            {
                return GetLanguage("Msg22");
            }
            else if (index == 3)
            {
                return GetLanguage("Msg23");
            }
            else if (index == 4)
            {
                return GetLanguage("Msg24");
            }
            else if (index == 5)
            {
                return GetLanguage("Msg25");
            }
            else if (index == 6)
            {
                return GetLanguage("Msg26");
            }
            return weekStr;
        }

        /// <summary>
        /// 设置读卡认证方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWriteDoorWorkSetting_Click(object sender, EventArgs e)
        {
            //if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            //{
            //    MsgErr(GetLanguage("Msg19"));
            //    return;
            //}
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WeekTimeGroup_ReaderWork tg = new WeekTimeGroup_ReaderWork(8);
            ConvertDtoToModel(tg);

            WriteReaderWorkSetting_Parameter par = new WriteReaderWorkSetting_Parameter((byte)(cmdDoorNum.SelectedIndex + 1), tg);
            WriteReaderWorkSetting write = new WriteReaderWorkSetting(cmdDtl, par);
            mMainForm.AddCommand(write);
            //byte door = 1;
            //if (cBoxDoor1.Checked)
            //{
            //    door = 1;
            //    WriteReaderWorkSetting_Parameter par = new WriteReaderWorkSetting_Parameter(door, tg);
            //    WriteReaderWorkSetting write = new WriteReaderWorkSetting(cmdDtl, par);
            //    mMainForm.AddCommand(write);
            //}
            //if (cBoxDoor2.Checked)
            //{
            //    door = 2;
            //    WriteReaderWorkSetting_Parameter par = new WriteReaderWorkSetting_Parameter(door, tg);
            //    WriteReaderWorkSetting write = new WriteReaderWorkSetting(cmdDtl, par);
            //    mMainForm.AddCommand(write);
            //}
            //if (cBoxDoor3.Checked)
            //{
            //    door = 3;
            //    WriteReaderWorkSetting_Parameter par = new WriteReaderWorkSetting_Parameter(door, tg);
            //    WriteReaderWorkSetting write = new WriteReaderWorkSetting(cmdDtl, par);
            //    mMainForm.AddCommand(write);
            //}
            //if (cBoxDoor4.Checked)
            //{
            //    door = 4;
            //    WriteReaderWorkSetting_Parameter par = new WriteReaderWorkSetting_Parameter(door, tg);
            //    WriteReaderWorkSetting write = new WriteReaderWorkSetting(cmdDtl, par);
            //    mMainForm.AddCommand(write);
            //}
        }

        /// <summary>
        /// GridView数据 转换成 WeekTimeGroup_ReaderWork
        /// </summary>
        /// <param name="tg"></param>
        private void ConvertDtoToModel(WeekTimeGroup_ReaderWork tg)
        {
            for (int i = 0; i < 7; i++)
            {
                var day = tg.GetItem(i);
                for (int j = 0; j < 8; j++)
                {
                    var dto = ListWeekTimeGroupDto.FirstOrDefault(t => t.WeekDay == (j + 1).ToString() && t.WeekDayIndex == i);
                    //DateTime nw = DateTime.Now;
                    var tz = day.GetItem(j) as TimeSegment_ReaderWork;
                    string[] tsStart = dto.StartTime.Split(':');
                    string[] tsEnd = dto.EndTime.Split(':');
                    tz.SetBeginTime(int.Parse(tsStart[0]), int.Parse(tsStart[1]));
                    tz.SetEndTime(int.Parse(tsEnd[0]), int.Parse(tsEnd[1]));
                    string strDoor1 = (dto.id3 ? "1" : "0") + (dto.id2 ? "1" : "0") + (dto.id1 ? "1" : "0") + (dto.id0 ? "1" : "0");
                    byte checkWay = Convert.ToByte(strDoor1, 2);

                    tz.SetCheckWay(checkWay);
                }
            }

        }

        #endregion

        #region 门工作方式
        private void BtnReadWorkSetting_Click(object sender, EventArgs e)
        {
            /*
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }
            */
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadDoorWorkSetting cmd = new ReadDoorWorkSetting(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                DoorWorkSetting_Result result = cmde.Command.getResult() as DoorWorkSetting_Result;
                StringBuilder sb = new StringBuilder();
                string OpenDoorWayStr = string.Empty;
                string DoorTriggerModeStr = string.Empty;
                if (result.Use == 0)
                {
                    sb.AppendLine(GetLanguage("Msg15") + result.Door + "：" + GetLanguage("Msg27"));
                    mMainForm.AddCmdLog(cmde, sb.ToString());
                }
                else
                {
                    //开门方式
                    if (result.OpenDoorWay == 1)
                    {
                        OpenDoorWayStr = GetLanguage("Msg28");
                    }
                    else if (result.OpenDoorWay == 2)
                    {
                        OpenDoorWayStr = GetLanguage("Msg29");
                    }
                    else if (result.OpenDoorWay == 3)
                    {
                        OpenDoorWayStr = GetLanguage("Msg30");
                    }
                    else if (result.OpenDoorWay == 4)
                    {
                        OpenDoorWayStr = GetLanguage("Msg31");
                    }
                    sb.AppendLine(GetLanguage("Msg15") + result.Door + $"：{GetLanguage("Msg32")}：" + OpenDoorWayStr);
                    //常开触发模式
                    if (result.DoorTriggerMode == 1)
                    {
                        DoorTriggerModeStr = GetLanguage("Msg33");
                    }
                    else if (result.DoorTriggerMode == 2)
                    {
                        DoorTriggerModeStr = GetLanguage("Msg34");
                    }
                    else if (result.DoorTriggerMode == 3)
                    {
                        DoorTriggerModeStr = GetLanguage("Msg35");
                    }
                    if (result.OpenDoorWay == 4)
                    {
                        sb.AppendLine($"；{GetLanguage("Msg36")}：" + DoorTriggerModeStr);
                    }
                    mMainForm.AddCmdLog(cmde, sb.ToString());
                    //开门方式是首卡或者常开状态时
                    if (result.OpenDoorWay == 3 || result.OpenDoorWay == 4)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            sb.Clear();
                            //sb.AppendLine(StringUtility.GetWeekStr(i));
                            for (int j = 0; j < 8; j++)
                            {
                                sb.Append($"  {GetLanguage("Msg37")}" + (j + 1) + "：" + StringUtility.TimeHourAndMinuteStr(result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime(), result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime()));
                            }
                            mMainForm.AddCmdLog(null, sb.ToString());
                        }
                    }
                }

                Invoke(() =>
                {
                    if (result.Use == 0)
                    {
                        rBtnNoDoorWorkSetting.Checked = true;
                    }
                    else
                    {
                        rBtnDoorWorkSetting.Checked = true;
                        //开门方式
                        if (result.OpenDoorWay == 1)
                        {
                            rBtnOpenDoorWay1.Checked = true;
                        }
                        else if (result.OpenDoorWay == 2)
                        {
                            rBtnOpenDoorWay2.Checked = true;
                        }
                        else if (result.OpenDoorWay == 3)
                        {
                            rBtnOpenDoorWay3.Checked = true;
                        }
                        else if (result.OpenDoorWay == 4)
                        {
                            rBtnOpenDoorWay4.Checked = true;
                        }
                        //常开触发模式
                        if (result.DoorTriggerMode == 1)
                        {
                            rBtnDoorTriggerMode1.Checked = true;
                        }
                        else if (result.DoorTriggerMode == 2)
                        {
                            rBtnDoorTriggerMode2.Checked = true;
                        }
                        else if (result.DoorTriggerMode == 3)
                        {
                            rBtnDoorTriggerMode3.Checked = true;
                        }
                        //开门方式是首卡或者常开状态时
                        if (result.OpenDoorWay == 3 || result.OpenDoorWay == 4)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    if (j == 0)
                                    {
                                        beginTimePicker1.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                                        endTimePicker1.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                                    }
                                    else if (j == 1)
                                    {
                                        beginTimePicker2.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                                        endTimePicker2.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                                    }
                                    else if (j == 2)
                                    {
                                        beginTimePicker3.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                                        endTimePicker3.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                                    }
                                    else if (j == 3)
                                    {
                                        beginTimePicker4.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                                        endTimePicker4.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                                    }
                                    else if (j == 4)
                                    {
                                        beginTimePicker5.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                                        endTimePicker5.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                                    }
                                    else if (j == 5)
                                    {
                                        beginTimePicker6.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                                        endTimePicker6.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                                    }
                                    else if (j == 6)
                                    {
                                        beginTimePicker7.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                                        endTimePicker7.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                                    }
                                    else if (j == 7)
                                    {
                                        beginTimePicker8.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                                        endTimePicker8.Text = result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                                    }
                                }
                            }
                        }
                    }
                });
            };
        }

        private void BtnWriteWorkSetting_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte door = (byte)(cmdDoorNum.SelectedIndex + 1); //门
            byte use = 0; //功能是否启用
            byte openDoorWay = 1; //开门方式
            byte doorTriggerMode = 3; //门常开触发模式
            byte retainValue = 0; //保留值
            WeekTimeGroup tg = new WeekTimeGroup(8);
            if (rBtnDoorWorkSetting.Checked)
            {
                use = 1;
                if (rBtnOpenDoorWay2.Checked) //多卡
                {
                    openDoorWay = 2;
                }
                else if (rBtnOpenDoorWay3.Checked) //首卡
                {
                    openDoorWay = 3;
                }
                else if (rBtnOpenDoorWay4.Checked) //常开
                {
                    openDoorWay = 4;
                    if (rBtnDoorTriggerMode1.Checked) //常开触发模式_合法卡
                    {
                        doorTriggerMode = 1;
                    }
                    else if (rBtnDoorTriggerMode2.Checked) //常开触发模式_常开卡
                    {
                        doorTriggerMode = 2;
                    }
                }

                if (rBtnOpenDoorWay3.Checked || rBtnOpenDoorWay4.Checked)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        var day = tg.GetItem(i);
                        DateTime nw = DateTime.Now;
                        for (int j = 0; j < 8; j++)
                        {
                            var tz = day.GetItem(j) as TimeSegment;
                            if (j == 0)
                            {
                                tz.SetBeginTime(beginTimePicker1.Text == "" ? 0 : DateTime.Parse(beginTimePicker1.Text).Hour, beginTimePicker1.Text == "" ? 0 : DateTime.Parse(beginTimePicker1.Text).Minute);
                                tz.SetEndTime(endTimePicker1.Text == "" ? 0 : DateTime.Parse(endTimePicker1.Text).Hour, endTimePicker1.Text == "" ? 0 : DateTime.Parse(endTimePicker1.Text).Minute);
                            }
                            else if (j == 1)
                            {
                                tz.SetBeginTime(beginTimePicker2.Text == "" ? 0 : DateTime.Parse(beginTimePicker2.Text).Hour, beginTimePicker1.Text == "" ? 0 : DateTime.Parse(beginTimePicker2.Text).Minute);
                                tz.SetEndTime(endTimePicker2.Text == "" ? 0 : DateTime.Parse(endTimePicker2.Text).Hour, endTimePicker1.Text == "" ? 0 : DateTime.Parse(endTimePicker2.Text).Minute);
                            }
                            else if (j == 2)
                            {
                                tz.SetBeginTime(beginTimePicker3.Text == "" ? 0 : DateTime.Parse(beginTimePicker3.Text).Hour, beginTimePicker1.Text == "" ? 0 : DateTime.Parse(beginTimePicker3.Text).Minute);
                                tz.SetEndTime(endTimePicker3.Text == "" ? 0 : DateTime.Parse(endTimePicker3.Text).Hour, endTimePicker1.Text == "" ? 0 : DateTime.Parse(endTimePicker3.Text).Minute);
                            }
                            else if (j == 3)
                            {
                                tz.SetBeginTime(beginTimePicker4.Text == "" ? 0 : DateTime.Parse(beginTimePicker4.Text).Hour, beginTimePicker1.Text == "" ? 0 : DateTime.Parse(beginTimePicker4.Text).Minute);
                                tz.SetEndTime(endTimePicker4.Text == "" ? 0 : DateTime.Parse(endTimePicker4.Text).Hour, endTimePicker1.Text == "" ? 0 : DateTime.Parse(endTimePicker4.Text).Minute);
                            }
                            else if (j == 4)
                            {
                                tz.SetBeginTime(beginTimePicker5.Text == "" ? 0 : DateTime.Parse(beginTimePicker5.Text).Hour, beginTimePicker1.Text == "" ? 0 : DateTime.Parse(beginTimePicker5.Text).Minute);
                                tz.SetEndTime(endTimePicker5.Text == "" ? 0 : DateTime.Parse(endTimePicker5.Text).Hour, endTimePicker1.Text == "" ? 0 : DateTime.Parse(endTimePicker5.Text).Minute);
                            }
                            else if (j == 5)
                            {
                                tz.SetBeginTime(beginTimePicker6.Text == "" ? 0 : DateTime.Parse(beginTimePicker6.Text).Hour, beginTimePicker1.Text == "" ? 0 : DateTime.Parse(beginTimePicker6.Text).Minute);
                                tz.SetEndTime(endTimePicker6.Text == "" ? 0 : DateTime.Parse(endTimePicker6.Text).Hour, endTimePicker1.Text == "" ? 0 : DateTime.Parse(endTimePicker6.Text).Minute);
                            }
                            else if (j == 6)
                            {
                                tz.SetBeginTime(beginTimePicker7.Text == "" ? 0 : DateTime.Parse(beginTimePicker7.Text).Hour, beginTimePicker1.Text == "" ? 0 : DateTime.Parse(beginTimePicker7.Text).Minute);
                                tz.SetEndTime(endTimePicker7.Text == "" ? 0 : DateTime.Parse(endTimePicker7.Text).Hour, endTimePicker1.Text == "" ? 0 : DateTime.Parse(endTimePicker7.Text).Minute);
                            }
                            else if (j == 7)
                            {
                                tz.SetBeginTime(beginTimePicker8.Text == "" ? 0 : DateTime.Parse(beginTimePicker8.Text).Hour, beginTimePicker1.Text == "" ? 0 : DateTime.Parse(beginTimePicker8.Text).Minute);
                                tz.SetEndTime(endTimePicker8.Text == "" ? 0 : DateTime.Parse(endTimePicker8.Text).Hour, endTimePicker1.Text == "" ? 0 : DateTime.Parse(endTimePicker8.Text).Minute);
                            }
                        }
                    }
                }
            }
            WriteDoorWorkSetting_Parameter par = new WriteDoorWorkSetting_Parameter(door, use, openDoorWay, doorTriggerMode, retainValue, tg);
            WriteDoorWorkSetting write = new WriteDoorWorkSetting(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void CbxWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeekTimeGroupWorkSettingDto != null)
            {
                var day = WeekTimeGroupWorkSettingDto.GetItem(cbxWeek.SelectedIndex);
                SetAllTimePicker(DoorOpenTimePanel, "beginTimePicker", "endTimePicker", day);
            }
        }

        private void BeginTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupWorkSettingDto, cbxWeek.SelectedIndex, int.Parse(dtp.Name.Substring(15)) - 1, 1, dtp.Value);
        }

        private void EndTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupWorkSettingDto, cbxWeek.SelectedIndex, int.Parse(dtp.Name.Substring(13)) - 1, 2, dtp.Value);
        }

        /// <summary>
        /// 选择不启用高级功能按钮时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RBtnNoDoorWorkSetting_CheckedChanged(object sender, EventArgs e)
        {
            OpenDoorWayPanel.Visible = false;
            DoorTriggerModePanel.Visible = false;
            DoorOpenTimePanel.Visible = false;
        }

        /// <summary>
        /// 选择启用高级功能按钮时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RBtnDoorWorkSetting_CheckedChanged(object sender, EventArgs e)
        {
            OpenDoorWayPanel.Visible = true;
        }
        /// <summary>
        /// 选择开门方式是普通按钮时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RBtnOpenDoorWay1_CheckedChanged(object sender, EventArgs e)
        {
            DoorOpenTimePanel.Visible = false;
            DoorTriggerModePanel.Visible = false;
        }
        /// <summary>
        /// 选择开门方式是多卡按钮时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RBtnOpenDoorWay2_CheckedChanged(object sender, EventArgs e)
        {
            DoorOpenTimePanel.Visible = false;
            DoorTriggerModePanel.Visible = false;
        }
        /// <summary>
        /// 选择开门方式是首卡按钮时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RBtnOpenDoorWay3_CheckedChanged(object sender, EventArgs e)
        {
            DoorOpenTimePanel.Visible = true;
            DoorTriggerModePanel.Visible = false;
        }

        /// <summary>
        /// 选择开门方式是常开按钮时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RBtnOpenDoorWay4_CheckedChanged(object sender, EventArgs e)
        {
            DoorTriggerModePanel.Visible = true;
            DoorOpenTimePanel.Visible = true;
        }


        #endregion

        #region 定时锁定门参数读写
        private void ConvertDtoToModel(WeekTimeGroup tg)
        {
            for (int i = 0; i < 7; i++)
            {
                var day = tg.GetItem(i);
                for (int j = 0; j < 8; j++)
                {
                    var dto = ListAutoLockedDto.FirstOrDefault(t => t.WeekDay == (j + 1).ToString() && t.WeekDayIndex == i);
                    //DateTime nw = DateTime.Now;
                    var tz = day.GetItem(j) as TimeSegment;
                    string[] tsStart = dto.StartTime.Split(':');
                    string[] tsEnd = dto.EndTime.Split(':');
                    tz.SetBeginTime(int.Parse(tsStart[0]), int.Parse(tsStart[1]));
                    tz.SetEndTime(int.Parse(tsEnd[0]), int.Parse(tsEnd[1]));
                }
            }

        }
        private void BtnReadAutoLockedSetting_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }
            StringBuilder sb = new StringBuilder();
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAutoLockedSetting cmd = new ReadAutoLockedSetting(cmdDtl, new DoorPort_Parameter(door));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                AutoLockedSetting_Result result = cmde.Command.getResult() as AutoLockedSetting_Result;
                ListAutoLockedDto.Clear();
                for (int i = 0; i < 7; i++)
                {
                    WeekTimeGroupDto dto = new WeekTimeGroupDto();
                    dto.WeekDay = GetWeekStr(i);
                    dto.Ex = "-";
                    dto.IsEx = "true";
                    ListAutoLockedDto.Add(dto);

                    for (int j = 0; j < 8; j++)
                    {
                        var tz = result.weekTimeGroup.GetItem(i).GetItem(j) as TimeSegment;
                        dto = new WeekTimeGroupDto();
                        dto.WeekDay = (j + 1).ToString();
                        dto.WeekDayIndex = i;
                        dto.StartTime = tz.GetBeginTime().ToString("HH:mm");
                        dto.EndTime = tz.GetEndTime().ToString("HH:mm");
                        ListAutoLockedDto.Add(dto);
                    }
                }
                Invoke(() =>
                {
                    dataGridView2.DataSource = new BindingList<WeekTimeGroupDto>(ListAutoLockedDto);
                });

                string useStr = result.Use == 0 ? GetLanguage("Msg38") : GetLanguage("Msg39");
                string tip = GetLanguage("Msg40") + result.Door.ToString() + $"：{GetLanguage("Msg41")}：" + useStr + $"，{GetLanguage("Msg42")}：";
                sb.AppendLine(tip);
                var Msg37 = GetLanguage("Msg37");
                for (int i = 0; i < 7; i++)
                {
                    //sb.AppendLine(StringUtility.GetWeekStr(i));
                    for (int j = 0; j < 8; j++)
                    {
                        sb.AppendLine("  " + Msg37 + (j + 1) + "：" + StringUtility.TimeHourAndMinuteStr(result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime(), result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime()));
                    }
                }
                sb.AppendLine("***************************************************************************************");
                Invoke(() =>
                {
                    if (result.Use == 0)
                    {
                        rBtnNoAutoLockedSetting.Checked = true;
                    }
                    txtAutoLockedSetting.Text = sb.ToString();
                });
            };
        }

        private void BtnWriteAutoLockedSetting_Click(object sender, EventArgs e)
        {
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WeekTimeGroup tg = new WeekTimeGroup(8);
            ConvertDtoToModel(tg);
            //for (int i = 0; i < 7; i++)
            //{
            //    var day = tg.GetItem(i);
            //    DateTime nw = DateTime.Now;
            //    var tz = day.GetItem(0) as TimeSegment;
            //    tz.SetBeginTime(0, 0);
            //    tz.SetEndTime(0, 0);
            //}

            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            byte use = 0;
            if (rBtnAutoLockedSetting.Checked)
            {
                use = 1;
            }
            AutoLockedSetting_Parameter par = new AutoLockedSetting_Parameter(door, use, tg);
            WriteAutoLockedSetting write = new WriteAutoLockedSetting(cmdDtl, par);
            mMainForm.AddCommand(write);
            /*
            if (cBoxDoor1.Checked)
            {
                door = 1;
                AutoLockedSetting_Parameter par = new AutoLockedSetting_Parameter(door, use, tg);
                WriteAutoLockedSetting write = new WriteAutoLockedSetting(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            if (cBoxDoor2.Checked)
            {
                door = 2;
                AutoLockedSetting_Parameter par = new AutoLockedSetting_Parameter(door, use, tg);
                WriteAutoLockedSetting write = new WriteAutoLockedSetting(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            if (cBoxDoor3.Checked)
            {
                door = 3;
                AutoLockedSetting_Parameter par = new AutoLockedSetting_Parameter(door, use, tg);
                WriteAutoLockedSetting write = new WriteAutoLockedSetting(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            if (cBoxDoor4.Checked)
            {
                door = 4;
                AutoLockedSetting_Parameter par = new AutoLockedSetting_Parameter(door, use, tg);
                WriteAutoLockedSetting write = new WriteAutoLockedSetting(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            */
        }
        #endregion

        #region 开锁时输出时长
        private void BtnReadRelayReleaseTime_Click(object sender, EventArgs e)
        {
            /*
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }
            */
            StringBuilder sb = new StringBuilder();
            ushort ReleaseTime = 0; //开锁时输出时长
            string tip = string.Empty;
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadRelayReleaseTime cmd = new ReadRelayReleaseTime(cmdDtl, new DoorPort_Parameter(door));
            mMainForm.AddCommand(cmd);
            /*
            if (cBoxDoor1.Checked)
            {
                door = 1;
                ReadRelayReleaseTime cmd = new ReadRelayReleaseTime(cmdDtl, new DoorPort_Parameter(door));
                mMainForm.AddCommand(cmd);
            }
            if (cBoxDoor2.Checked)
            {
                door = 2;
                ReadRelayReleaseTime cmd = new ReadRelayReleaseTime(cmdDtl, new DoorPort_Parameter(door));
                mMainForm.AddCommand(cmd);
            }
            if (cBoxDoor3.Checked)
            {
                door = 3;
                ReadRelayReleaseTime cmd = new ReadRelayReleaseTime(cmdDtl, new DoorPort_Parameter(door));
                mMainForm.AddCommand(cmd);
            }
            if (cBoxDoor4.Checked)
            {
                door = 4;
                ReadRelayReleaseTime cmd = new ReadRelayReleaseTime(cmdDtl, new DoorPort_Parameter(door));
                mMainForm.AddCommand(cmd);
            }
            */
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                RelayReleaseTime_Result result = cmde.Command.getResult() as RelayReleaseTime_Result;

                ReleaseTime = result.ReleaseTime; //开锁时输出时长
                sb.Clear();
                sb.AppendLine(GetLanguage("Msg15") + result.Door + $"：{GetLanguage("Msg43")}：");
                var Msg1 = GetLanguage("Msg1");
                if (ReleaseTime == 0)
                {
                    tip = "0.5" + Msg1;
                }
                else
                {
                    tip = ReleaseTime + Msg1;
                }

                Invoke(() =>
                {
                    cbxReleaseTime.Text = tip.Replace(Msg1, "");
                });
                sb.AppendLine(tip);
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteRelayReleaseTime_Click(object sender, EventArgs e)
        {
            /*
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }
            */
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxReleaseTime.Text.Trim(), reg))
            {
                if (cbxReleaseTime.Text != "0.5")
                {
                    MsgErr(GetLanguage("Msg44"));
                    return;
                }
            }
            if (Regex.IsMatch(cbxReleaseTime.Text.Trim(), reg))
            {
                if (Convert.ToUInt32(cbxReleaseTime.Text) < 0 || Convert.ToUInt32(cbxReleaseTime.Text) > 65535)
                {
                    MsgErr(GetLanguage("Msg44"));
                    return;
                }
            }

            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            ushort releaseTime = 0;
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            if (cbxReleaseTime.Text != "0.5")
            {
                releaseTime = Convert.ToUInt16(cbxReleaseTime.Text);
            }
            WriteRelayReleaseTime_Parameter par = new WriteRelayReleaseTime_Parameter(door, releaseTime);
            WriteRelayReleaseTime write = new WriteRelayReleaseTime(cmdDtl, par);
            mMainForm.AddCommand(write);
            /*
            if (cBoxDoor1.Checked)
            {
                door = 1;
                WriteRelayReleaseTime_Parameter par = new WriteRelayReleaseTime_Parameter(door, releaseTime);
                WriteRelayReleaseTime write = new WriteRelayReleaseTime(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            if (cBoxDoor2.Checked)
            {
                door = 2;
                WriteRelayReleaseTime_Parameter par = new WriteRelayReleaseTime_Parameter(door, releaseTime);
                WriteRelayReleaseTime write = new WriteRelayReleaseTime(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            if (cBoxDoor3.Checked)
            {
                door = 3;
                WriteRelayReleaseTime_Parameter par = new WriteRelayReleaseTime_Parameter(door, releaseTime);
                WriteRelayReleaseTime write = new WriteRelayReleaseTime(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            if (cBoxDoor4.Checked)
            {
                door = 4;
                WriteRelayReleaseTime_Parameter par = new WriteRelayReleaseTime_Parameter(door, releaseTime);
                WriteRelayReleaseTime write = new WriteRelayReleaseTime(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            */
        }
        #endregion

        #region 重复读卡间隔参数
        private void BtnReadReaderInterval_Click(object sender, EventArgs e)
        {
            /*
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }
            */
            StringBuilder sb = new StringBuilder();
            string UseStr = string.Empty;
            string DetectionModeStr = string.Empty;
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadReaderIntervalTime cmd0 = new ReadReaderIntervalTime(cmdDtl);
            mMainForm.AddCommand(cmd0);

            ReadReaderInterval cmd = new ReadReaderInterval(cmdDtl, new DoorPort_Parameter(door));
            mMainForm.AddCommand(cmd);


            /*
            if (cBoxDoor1.Checked)
            {
                door = 1;
                ReadReaderInterval cmd = new ReadReaderInterval(cmdDtl, new DoorPort_Parameter(door));
                mMainForm.AddCommand(cmd);
            }
            if (cBoxDoor2.Checked)
            {
                door = 2;
                ReadReaderInterval cmd = new ReadReaderInterval(cmdDtl, new DoorPort_Parameter(door));
                mMainForm.AddCommand(cmd);
            }
            if (cBoxDoor3.Checked)
            {
                door = 3;
                ReadReaderInterval cmd = new ReadReaderInterval(cmdDtl, new DoorPort_Parameter(door));
                mMainForm.AddCommand(cmd);
            }
            if (cBoxDoor4.Checked)
            {
                door = 4;
                ReadReaderInterval cmd = new ReadReaderInterval(cmdDtl, new DoorPort_Parameter(door));
                mMainForm.AddCommand(cmd);
            }
            */
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReaderInterval_Result result = cmde.Command.getResult() as ReaderInterval_Result;
                if (result != null)
                {
                    UseStr = result.Use == 0 ? GetLanguage("Msg38") : GetLanguage("Msg39");
                    if (result.DetectionMode == 1)
                    {
                        DetectionModeStr = GetLanguage("Msg45");
                    }
                    else if (result.DetectionMode == 2)
                    {
                        DetectionModeStr = GetLanguage("Msg46");
                    }
                    else if (result.DetectionMode == 3)
                    {
                        DetectionModeStr = GetLanguage("Msg47");
                    }

                    sb.Clear();
                    sb.AppendLine(GetLanguage("Msg15") + result.Door + $"：{GetLanguage("Msg48")}：" + UseStr);
                    if (result.Use == 1)
                    {
                        sb.AppendLine($"；{GetLanguage("Msg49")}：" + DetectionModeStr);
                    }

                    Invoke(() =>
                    {
                        if (result.Use == 0)
                        {
                            rBtnNoReaderInterval.Checked = true;
                        }
                        cbxDetectionMode.SelectedIndex = result.DetectionMode - 1;
                    });
                    mMainForm.AddCmdLog(cmde, sb.ToString());
                }
                ReadReaderIntervalTime_Result IntervalTimeResult = cmde.Command.getResult() as ReadReaderIntervalTime_Result;
                if (IntervalTimeResult != null)
                {
                    Invoke(() =>
                    {
                        if (IntervalTimeResult.IntervalTime == 65535)
                        {
                            cmbInterval.SelectedIndex = cmbInterval.Items.Count - 1;
                        }
                        else
                        {
                            cmbInterval.SelectedIndex = IntervalTimeResult.IntervalTime;
                        }

                    });
                }
            };
        }

        private void BtnWriteReaderInterval_Click(object sender, EventArgs e)
        {
            /*
            if (!cBoxDoor1.Checked && !cBoxDoor2.Checked && !cBoxDoor3.Checked && !cBoxDoor4.Checked)
            {
                MsgErr(GetLanguage("Msg19"));
                return;
            }
            */
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1); //门
            byte use = 0; //功能是否启用
            byte detectionMode = 1; //检测模式
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            if (rBtnReaderInterval.Checked)
            {
                use = 1;
            }
            detectionMode = Convert.ToByte(cbxDetectionMode.SelectedIndex + 1);
            WriteReaderInterval_Parameter par = new WriteReaderInterval_Parameter(door, use, detectionMode);
            WriteReaderInterval write = new WriteReaderInterval(cmdDtl, par);
            mMainForm.AddCommand(write);

            WriteReaderIntervalTime_Parameter intervalPar = new WriteReaderIntervalTime_Parameter();
            if (cmbInterval.SelectedIndex == cmbInterval.Items.Count - 1)
            {
                intervalPar.IntervalTime = 65535;
            }
            else
            {
                intervalPar.IntervalTime = (ushort)cmbInterval.SelectedIndex;
            }

            WriteReaderIntervalTime writeInterval = new WriteReaderIntervalTime(cmdDtl, intervalPar);
            mMainForm.AddCommand(writeInterval);
            /*
            if (cBoxDoor1.Checked)
            {
                door = 1;
                WriteReaderInterval_Parameter par = new WriteReaderInterval_Parameter(door, use, detectionMode);
                WriteReaderInterval write = new WriteReaderInterval(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            if (cBoxDoor2.Checked)
            {
                door = 2;
                WriteReaderInterval_Parameter par = new WriteReaderInterval_Parameter(door, use, detectionMode);
                WriteReaderInterval write = new WriteReaderInterval(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            if (cBoxDoor3.Checked)
            {
                door = 3;
                WriteReaderInterval_Parameter par = new WriteReaderInterval_Parameter(door, use, detectionMode);
                WriteReaderInterval write = new WriteReaderInterval(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            if (cBoxDoor4.Checked)
            {
                door = 4;
                WriteReaderInterval_Parameter par = new WriteReaderInterval_Parameter(door, use, detectionMode);
                WriteReaderInterval write = new WriteReaderInterval(cmdDtl, par);
                mMainForm.AddCommand(write);
            }
            */
        }
        #endregion

        #region 非法读卡报警

        private void IniInvalidCardAlarmOptionUse()
        {

        }

        private void butReadInvalidCardAlarmOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new Door8800.Door.DoorPort_Parameter(int.Parse(cmdDoorNum.Text));

            if (!CheckProtocolTypeIs89H())
            {
                var cmd = new Door8800.Door.InvalidCardAlarmOption.ReadInvalidCardAlarmOption(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }
            else
            {
                var cmd = new Door89H.Door.InvalidCardAlarmOption.ReadInvalidCardAlarmOption(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Door8800.Door.InvalidCardAlarmOption.InvalidCardAlarmOption_Result;
                Invoke(() =>
                {
                    cmdInvalidCardAlarmOptionUse.SelectedIndex = result.Use ? 0 : 1;
                    cmbReadInvalidCardTime.SelectedIndex = result.ReadInvalidCardTime;
                });
                string UseTip = result.Use ? GetLanguage("Msg6") : GetLanguage("Msg2");
                mMainForm.AddCmdLog(cmde, $"{GetLanguage("Msg7")}{result.DoorNum},{GetLanguage("Msg8")}:{UseTip}");
            };
        }
        private void ButWriteInvalidCardAlarmOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();

            bool use = (cmdInvalidCardAlarmOptionUse.SelectedIndex == 0);
            byte door = byte.Parse(cmdDoorNum.Text);

            var par = new Door8800.Door.InvalidCardAlarmOption.WriteInvalidCardAlarmOption_Parameter(door, use, (byte)(cmbReadInvalidCardTime.SelectedIndex));
            if (!CheckProtocolTypeIs89H())
            {
                var cmd = new Door8800.Door.InvalidCardAlarmOption.WriteInvalidCardAlarmOption(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }
            else
            {
                var cmd = new Door89H.Door.InvalidCardAlarmOption.WriteInvalidCardAlarmOption(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }


            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"{GetLanguage("Msg7")}{door},{GetLanguage("Msg8")}:{use}");
            };
        }
        #endregion

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            object ex = this.dataGridView1.Rows[e.RowIndex].Cells["IsEx"].Value;
            if (e.ColumnIndex == 0 && e.RowIndex % 8 != 0 && ex == null)
            {
                return;
            }
            if (e.ColumnIndex == 0)
            {
                string isEx = ex.ToString();
                //收缩
                if (this.dataGridView1.Columns[e.ColumnIndex].Name == "EX" && isEx == "true")
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        //隐藏行
                        this.dataGridView1.Rows[e.RowIndex + i].Visible = false;
                    }
                    //将IsEx设置为false，标明该节点已经收缩
                    this.dataGridView1.Rows[e.RowIndex].Cells["IsEx"].Value = "false";
                    this.dataGridView1.Rows[e.RowIndex].Cells["EX"].Value = "+";
                }
                else if (this.dataGridView1.Columns[e.ColumnIndex].Name == "EX" && isEx == "false")
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        this.dataGridView1.Rows[e.RowIndex + i].Visible = true;
                    }
                    this.dataGridView1.Rows[e.RowIndex].Cells["IsEx"].Value = "true";
                    this.dataGridView1.Rows[e.RowIndex].Cells["EX"].Value = "-";
                }
            }
            if (e.ColumnIndex >= 5 && e.ColumnIndex <= 8 && ex == null)
            {

                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if ((bool)cell.FormattedValue)
                {
                    cell.Value = false;
                    cell.EditingCellFormattedValue = false;
                }
                else
                {
                    //for (int i = 5; i <= 8; i++)
                    //{
                    //    DataGridViewCheckBoxCell allcell = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[i];
                    //    allcell.Value = false;
                    //    allcell.EditingCellFormattedValue = false;
                    //}
                    cell.Value = true;
                    cell.EditingCellFormattedValue = true;
                }
            }
            if (e.ColumnIndex >= 3 && e.ColumnIndex <= 4)
            {
                DataGridViewTextBoxColumn textbox = dataGridView1.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
                if (textbox != null) //如果该列是TextBox列
                {
                    dataGridView1.BeginEdit(true); //开始编辑状态
                    dataGridView1.ReadOnly = false;
                }

            }
            else
            {
                dataGridView1.BeginEdit(false); //开始编辑状态
                dataGridView1.ReadOnly = true;
            }
        }

        private void DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            object ex = this.dataGridView2.Rows[e.RowIndex].Cells["IsEx2"].Value;
            if (e.ColumnIndex == 0 && e.RowIndex % 8 != 0 && ex == null)
            {
                return;
            }
            if (e.ColumnIndex == 0)
            {
                string isEx = ex.ToString();
                //收缩
                if (this.dataGridView2.Columns[e.ColumnIndex].Name == "EX2" && isEx == "true")
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        //隐藏行
                        this.dataGridView2.Rows[e.RowIndex + i].Visible = false;
                    }
                    //将IsEx设置为false，标明该节点已经收缩
                    this.dataGridView2.Rows[e.RowIndex].Cells["IsEx2"].Value = "false";
                    this.dataGridView2.Rows[e.RowIndex].Cells["EX2"].Value = "+";
                }
                else if (this.dataGridView2.Columns[e.ColumnIndex].Name == "EX2" && isEx == "false")
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        this.dataGridView2.Rows[e.RowIndex + i].Visible = true;
                    }
                    this.dataGridView2.Rows[e.RowIndex].Cells["IsEx2"].Value = "true";
                    this.dataGridView2.Rows[e.RowIndex].Cells["EX2"].Value = "-";
                }
            }

            if (e.ColumnIndex >= 3 && e.ColumnIndex <= 4)
            {
                DataGridViewTextBoxColumn textbox = dataGridView2.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
                if (textbox != null) //如果该列是TextBox列
                {
                    dataGridView2.BeginEdit(true); //开始编辑状态
                    dataGridView2.ReadOnly = false;
                }

            }
            else
            {
                dataGridView2.BeginEdit(false); //开始编辑状态
                dataGridView2.ReadOnly = true;
            }
        }

        /// <summary>
        /// 读取防潜回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReadAntiPassback_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadCheckInOut cmd0 = new ReadCheckInOut(cmdDtl);
            mMainForm.AddCommand(cmd0);

            ReadAntiPassback cmd = new ReadAntiPassback(cmdDtl, new DoorPort_Parameter(door));
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                AntiPassback_Result antiPassback_Result = cmde.Command.getResult() as AntiPassback_Result;
                if (antiPassback_Result != null)
                {
                    Invoke(() =>
                    {
                        rBtnAnti.Checked = antiPassback_Result.Use;
                        rBtnNotAnti.Checked = !antiPassback_Result.Use;
                    });
                }

                ReadCheckInOut_Result readCheckInOut_Result = cmde.Command.getResult() as ReadCheckInOut_Result;
                if (readCheckInOut_Result != null)
                {
                    Invoke(() =>
                    {
                        rBtnCheckInOut1.Checked = readCheckInOut_Result.Mode == 1;
                        rBtnCheckInOut2.Checked = readCheckInOut_Result.Mode == 2;
                    });
                }
            };
        }

        /// <summary>
        /// 写入防潜回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWriteAntiPassback_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteCheckInOut_Parameter writeCheckInOut_Parameter = new WriteCheckInOut_Parameter();
            writeCheckInOut_Parameter.Mode = (byte)(rBtnCheckInOut1.Checked ? 1 : 2);
            WriteCheckInOut cmd0 = new WriteCheckInOut(cmdDtl, writeCheckInOut_Parameter);
            mMainForm.AddCommand(cmd0);

            WriteAntiPassback_Parameter writeAntiPassback_Parameter = new WriteAntiPassback_Parameter(door, rBtnAnti.Checked);
            WriteAntiPassback cmd = new WriteAntiPassback(cmdDtl, writeAntiPassback_Parameter);
            mMainForm.AddCommand(cmd);
        }

        #region 出门开关
        private void BtnReadPushButton_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadPushButtonSetting cmd = new ReadPushButtonSetting(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                PushButtonSetting_Result result = cmde.Command.getResult() as PushButtonSetting_Result;
                Invoke(() =>
                {
                    cbNormallyOpen.Checked = result.NormallyOpen;
                    cbReadPushButton.Checked = result.Use;

                    WeekTimeGroupPushButtonDto = result.weekTimeGroup;
                    SetAllTimePicker(plPushButton, "beginTP", "EndTP", WeekTimeGroupPushButtonDto.GetItem(0));
                });
            };
        }

        private void BtnWritePushButton_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;


            WritePushButtonSetting_Parameter par = new WritePushButtonSetting_Parameter(door, cbReadPushButton.Checked, cbNormallyOpen.Checked, WeekTimeGroupPushButtonDto);
            WritePushButtonSetting cmd = new WritePushButtonSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void CmbPushButtonWeekday_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeekTimeGroupPushButtonDto != null)
            {
                var day = WeekTimeGroupPushButtonDto.GetItem(cmbPushButtonWeekday.SelectedIndex);
                SetAllTimePicker(plPushButton, "beginTP", "EndTP", day);
            }
        }
        private void CbReadPushButton_CheckedChanged(object sender, EventArgs e)
        {
            plPushButton.Visible = cbReadPushButton.Checked;
            lbNormallyOpen.Visible = cbReadPushButton.Checked;
            cbNormallyOpen.Visible = cbReadPushButton.Checked;
        }
        #endregion

        private void BeginTP_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupPushButtonDto, cmbPushButtonWeekday.SelectedIndex, int.Parse(dtp.Name.Substring(7)) - 1, 1, dtp.Value);
        }


        private void EndTP_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupPushButtonDto, cmbPushButtonWeekday.SelectedIndex, int.Parse(dtp.Name.Substring(5)) - 1, 2, dtp.Value);
        }

        #region 全卡开门
        private void BtnReadAnyCard_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAnyCardSetting cmd = new ReadAnyCardSetting(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                AnyCardSetting_Result result = cmde.Command.getResult() as AnyCardSetting_Result;
                Invoke(() =>
                {
                    cbAnyCardAuto.Checked = result.AutoSave;
                    cbAnyCardUse.Checked = result.Use;
                    cmbAnyCardTimeGroup.SelectedIndex = (result.TimeGroup - 1);
                });
            };
        }

        private void BtnWriteAnyCard_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteAnyCardSetting_Parameter par = new WriteAnyCardSetting_Parameter(door, cbAnyCardUse.Checked, cbAnyCardAuto.Checked, cmbAnyCardTimeGroup.SelectedIndex + 1);
            WriteAnyCardSetting cmd = new WriteAnyCardSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }


        #endregion

        #region 设置语音播报功能
        private void BtnReadVoiceBroadcastSetting_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadVoiceBroadcastSetting cmd = new ReadVoiceBroadcastSetting(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                VoiceBroadcastSetting_Result result = cmde.Command.getResult() as VoiceBroadcastSetting_Result;
                Invoke(() =>
                {
                    cbVoiceBroadcastSettingUse.Checked = result.Use;
                });
            };
        }

        private void BtnWriteWriteVoiceBroadcastSetting_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteVoiceBroadcastSetting_Parameter par = new WriteVoiceBroadcastSetting_Parameter(door, cbVoiceBroadcastSettingUse.Checked);
            WriteVoiceBroadcastSetting cmd = new WriteVoiceBroadcastSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }
        #endregion

        #region 门内外同时读卡开门
        private void BtnReadInOutSideReadOpenSetting_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadInOutSideReadOpenSetting cmd = new ReadInOutSideReadOpenSetting(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                InOutSideReadOpenSetting_Result result = cmde.Command.getResult() as InOutSideReadOpenSetting_Result;
                Invoke(() =>
                {
                    cbInOutSideReadOpenSettingUse.Checked = result.Use;
                });
            };
        }

        private void BtnWriteInOutSideReadOpenSetting_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            InOutSideReadOpenSetting_Parameter par = new InOutSideReadOpenSetting_Parameter(door, cbInOutSideReadOpenSettingUse.Checked);
            WriteInOutSideReadOpenSetting cmd = new WriteInOutSideReadOpenSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        #endregion

        #region 键盘管理功能


        private void BtnReadManageKeyboardSetting_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadManageKeyboardSetting cmd = new ReadManageKeyboardSetting(cmdDtl, new DoorPort_Parameter((byte)(cmdDoorNum.SelectedIndex + 1)));
            mMainForm.AddCommand(cmd);

            //ReadPassword readPassword = new ReadPassword(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            //mMainForm.AddCommand(readPassword);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                WriteManageKeyboardSetting_Parameter result = cmde.Command.getResult() as WriteManageKeyboardSetting_Parameter;

                Invoke(() =>
                {
                    cbManageKeyboardSettingUse.Checked = result.Use;
                    if (result.Use)
                    {
                        txtPassword.Text = result.Password;
                    }
                });

                //Password_Result password_Result = cmde.Command.getResult() as Password_Result;
                //if (password_Result != null)
                //{
                //    Invoke(() =>
                //    {
                //        txtPassword.Text = password_Result.Password;
                //    });
                //}
            };
        }

        private void BtnWriteManageKeyboardSetting_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim().Length < 4)
            {
                MessageBox.Show(GetLanguage("Msg50"));
                return;
            }
            string pattern = @"\b(0[xX])?[A-Fa-f0-9]+\b";
            bool isHexNum = Regex.IsMatch(txtPassword.Text.Trim(), pattern);
            if (!isHexNum)
            {
                MessageBox.Show(GetLanguage("Msg51"));
                return;
            }
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteManageKeyboardSetting_Parameter par = new WriteManageKeyboardSetting_Parameter(door, cbManageKeyboardSettingUse.Checked, txtPassword.Text.Trim());
            WriteManageKeyboardSetting cmd = new WriteManageKeyboardSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }
        #endregion

        #region 区域防潜回
        private void CbAreaAntiPassbackUse_CheckedChanged(object sender, EventArgs e)
        {
            plAreaAntiPassback.Visible = cbAreaAntiPassbackUse.Checked;
            lbPort.Visible = lbSN.Visible = lbIP.Visible = txtIP.Visible = txtPort.Visible = txtSN.Visible = (cmbAreaType.SelectedIndex == 1 && cbAreaAntiPassbackUse.Checked);
        }

        private void BtnReadAreaAntiPassback_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAreaAntiPassback cmd = new ReadAreaAntiPassback(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);


            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                AreaAntiPassback_Result result = cmde.Command.getResult() as AreaAntiPassback_Result;
                Invoke(() =>
                {
                    cbAreaAntiPassbackUse.Checked = result.Use;
                    txtSN.Text = result.SN;
                    txtPort.Text = result.Port.ToString();
                    byte[] listb = result.IP;

                    txtIP.Text = listb[0] + "." + listb[1] + "." + listb[2] + "." + listb[3];
                    cmbAreaType.SelectedIndex = result.Type ? 0 : 1;
                });
            };
        }

        private void BtnWriteAreaAntiPassback_Click(object sender, EventArgs e)
        {
            string pattern = @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$";
            bool isHexNum = Regex.IsMatch(txtIP.Text.Trim(), pattern);
            if (!isHexNum)
            {
                MessageBox.Show(GetLanguage("Msg52"));
                return;
            }
            ushort s = 0;
            if (!ushort.TryParse(txtPort.Text.Trim(), out s))
            {
                MessageBox.Show(GetLanguage("Msg53"));
                return;
            }
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] bIP = new byte[4];
            string[] listip = txtIP.Text.Trim().Split('.');
            for (int i = 0; i < listip.Length; i++)
            {
                bIP[i] = byte.Parse(listip[i]);
            }
            WriteAreaAntiPassback_Parameter par = new WriteAreaAntiPassback_Parameter(door, cbAreaAntiPassbackUse.Checked, cmbAreaType.SelectedIndex == 0, txtSN.Text.Trim(), bIP, ushort.Parse(txtPort.Text.Trim()));
            WriteAreaAntiPassback cmd = new WriteAreaAntiPassback(cmdDtl, par);
            mMainForm.AddCommand(cmd);


            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void CmbAreaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbPort.Visible = lbSN.Visible = lbIP.Visible = txtIP.Visible = txtPort.Visible = txtSN.Visible = (cmbAreaType.SelectedIndex == 1 && cbAreaAntiPassbackUse.Checked);
        }
        #endregion

        #region 区域互锁
        private void CbInterLockSettingUse_CheckedChanged(object sender, EventArgs e)
        {
            //lbAreaType2.Visible = cmbInterLockSettingAreaType.Visible = cbInterLockSettingUse.Checked;
            lbPort2.Visible = lbIP2.Visible = lbAreaType2.Visible = lbNum2.Visible = lbAreaCode.Visible =
            cmbNum.Visible = txtInterLockSettingIP.Visible = txtInterLockSettingPort.Visible = cmbInterLockSettingAreaType.Visible = txtAreaCode.Visible =
               cbInterLockSettingUse.Checked;
        }

        private void BtnReadInterLockSetting_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadInterLockSetting cmd = new ReadInterLockSetting(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);


            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                InterLockSetting_Result result = cmde.Command.getResult() as InterLockSetting_Result;
                Invoke(() =>
                {
                    cbInterLockSettingUse.Checked = result.Use;
                    cmbNum.SelectedIndex = result.Num - 1;
                    txtInterLockSettingPort.Text = result.Port.ToString();
                    byte[] listb = result.IP;
                    txtAreaCode.Text = result.AreaCode.ToString();
                    txtInterLockSettingIP.Text = listb[0] + "." + listb[1] + "." + listb[2] + "." + listb[3];
                    cmbInterLockSettingAreaType.SelectedIndex = result.Type ? 0 : 1;
                });
            };
        }

        private void BtnWriteInterLockSetting_Click(object sender, EventArgs e)
        {
            string pattern = @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$";
            bool isHexNum = Regex.IsMatch(txtInterLockSettingIP.Text.Trim(), pattern);
            if (!isHexNum)
            {
                MessageBox.Show(GetLanguage("Msg52"));
                return;
            }
            ushort s = 0;
            if (!ushort.TryParse(txtInterLockSettingPort.Text.Trim(), out s))
            {
                MessageBox.Show(GetLanguage("Msg53"));
                return;
            }
            int iOut = 0;
            if (!int.TryParse(txtAreaCode.Text.Trim(), out iOut))
            {
                MessageBox.Show(GetLanguage("Msg54"));
                return;
            }
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] bIP = new byte[4];
            string[] listip = txtInterLockSettingIP.Text.Trim().Split('.');
            for (int i = 0; i < listip.Length; i++)
            {
                bIP[i] = byte.Parse(listip[i]);
            }
            WriteInterLockSetting_Parameter par = new WriteInterLockSetting_Parameter(door, cbInterLockSettingUse.Checked, cmbInterLockSettingAreaType.SelectedIndex == 0
                , int.Parse(txtAreaCode.Text.Trim()), Convert.ToByte(cmbNum.SelectedIndex + 1), bIP, ushort.Parse(txtInterLockSettingPort.Text.Trim()));
            WriteInterLockSetting cmd = new WriteInterLockSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);


            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void CmbInterLockSettingAreaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbPort2.Visible = lbIP2.Visible = lbNum2.Visible = lbAreaCode.Visible =
           cmbNum.Visible = txtInterLockSettingIP.Visible = txtInterLockSettingPort.Visible = txtAreaCode.Visible =
              (cmbInterLockSettingAreaType.SelectedIndex == 1 && cbInterLockSettingUse.Checked);
        }

        #endregion

        #region 多卡开门检测模式参数
        private void BtnReadManyCardOpenMode_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            var protocolType = mMainForm.GetProtocolType();
            ReadMultiCard cmd = null;
            if (protocolType == CommandDetailFactory.ControllerType.Door88)
            {
                cmd = new ReadMultiCard(cmdDtl, new ReadMultiCard_Parameter(cmdDoorNum.SelectedIndex + 1, false));
            }
            else if (CheckProtocolTypeIs89H())
            {
                cmd = new Door89H.Door.MultiCard.ReadMultiCard(cmdDtl, new ReadMultiCard_Parameter(cmdDoorNum.SelectedIndex + 1, false));
            }
            else if (protocolType == CommandDetailFactory.ControllerType.Door58)
            {
                cmd = new Door89H.Door.MultiCard.ReadMultiCard(cmdDtl, new ReadMultiCard_Parameter(cmdDoorNum.SelectedIndex + 1, true));
            }

            //ReadMultiCard cmd = new Door89H.Door.MultiCard.ReadMultiCard(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1, mMain.GetProtocolType()));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                MultiCard_Result result = cmde.Command.getResult() as MultiCard_Result;
                Invoke(() =>
                {
                    cmbManyCardOpenMode.SelectedIndex = result.ReaderWaitMode;
                    cmbAntiPassback.SelectedIndex = result.AntiPassback;
                    cmbVerifyType.SelectedIndex = result.VerifyType;
                    txtAGroupCount.Text = result.AGroupCount.ToString();
                    txtBGroupCount.Text = result.BGroupCount.ToString();

                    if (cmbVerifyType.SelectedItem.ToString() == GetLanguage("Msg4"))
                    {
                        listGroupA = result.GroupA;
                        listGroupB = result.GroupB;
                    }
                    else if (cmbVerifyType.SelectedItem.ToString() == GetLanguage("Msg3"))
                    {
                        listFix = result.GroupFix;
                    }


                    //foreach (KeyValuePair<int,List<string>> item in result.GroupA)
                    //{
                    //    for (int i = 0; i < item.Value.Count; i++)
                    //    {
                    //        listGroupA[50 * (item.Key -1) + i].Card = item.Value[i];
                    //    }
                    //    for (int i = item.Value.Count; i < 50 - item.Value.Count; i++)
                    //    {
                    //        listGroupA[50 * (item.Key - 1) + i].Card = "";
                    //    }
                    //}

                    //foreach (KeyValuePair<int, List<string>> item in result.BListCardData)
                    //{
                    //    for (int i = 0; i < item.Value.Count; i++)
                    //    {
                    //        listGroupB[100 * (item.Key - 1) + i].Card = item.Value[i];
                    //    }
                    //    for (int i = item.Value.Count; i < 100 - item.Value.Count; i++)
                    //    {
                    //        listGroupB[100 * (item.Key - 1) + i].Card = "";
                    //    }
                    //}


                    CmbVerifyType_SelectedIndexChanged(null, null);
                    CmbGroupNum_SelectedIndexChanged(null, null);
                });
            };
        }

        /// <summary>
        /// 写入多门
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWriteManyCardOpenMode_Click(object sender, EventArgs e)
        {
            byte bAcount = 0;
            byte bBcount = 0;
            if (cmbVerifyType.SelectedItem.ToString() == GetLanguage("Msg4"))
            {
                if (!byte.TryParse(txtAGroupCount.Text, out bAcount))
                {
                    MessageBox.Show(GetLanguage("Msg55"));
                    return;
                }
                if (!byte.TryParse(txtBGroupCount.Text, out bBcount))
                {
                    MessageBox.Show(GetLanguage("Msg56"));
                    return;
                }
            }


            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            var protocolType = mMainForm.GetProtocolType();


            if (protocolType == CommandDetailFactory.ControllerType.Door88)
            {
                WriteMultiCard_Parameter par = new WriteMultiCard_Parameter(door, (byte)cmbManyCardOpenMode.SelectedIndex, (byte)cmbAntiPassback.SelectedIndex
                    , (byte)cmbVerifyType.SelectedIndex, bAcount, bBcount
                    , listGroupA, listGroupB, listFix);
                WriteMultiCard cmd = new WriteMultiCard(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }
            else if (CheckProtocolTypeIs89H())
            {
                Door89H.Door.MultiCard.WriteMultiCard_Parameter par = new Door89H.Door.MultiCard.WriteMultiCard_Parameter(door, (byte)cmbManyCardOpenMode.SelectedIndex, (byte)cmbAntiPassback.SelectedIndex, (byte)cmbVerifyType.SelectedIndex
                        , bAcount, bBcount
                        , listGroupA, listGroupB, listFix);
                Door89H.Door.MultiCard.WriteMultiCard cmd = new Door89H.Door.MultiCard.WriteMultiCard(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }
            else if (protocolType == CommandDetailFactory.ControllerType.Door58)
            {
                WriteMultiCard_Parameter par = new WriteMultiCard_Parameter(door, (byte)cmbManyCardOpenMode.SelectedIndex, (byte)cmbAntiPassback.SelectedIndex, listFix);
                WriteMultiCard cmd = new WriteMultiCard(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }
        #endregion

        #region 多卡开门验证方式
        private void CmbVerifyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ab = GetLanguage("Msg4");
            plManyCardOpenVerify.Visible = cmbVerifyType.SelectedItem.ToString() == ab;
            plMutiCard.Visible = cmbVerifyType.SelectedItem.ToString() == ab;
            if (cmbVerifyType.SelectedItem.ToString() == GetLanguage("Msg3"))
            {
                dataGridView5.Visible = true;
                plMutiCard.Visible = true;

                cmbGroupNum.Items.Clear();
                int count = 8;

                string[] array = new string[count];
                for (int i = 1; i <= count; i++)
                {
                    array[i - 1] = i.ToString();
                }
                cmbGroupNum.Items.AddRange(array);
                cmbGroupNum.SelectedIndex = 0;

                lbgrouptype.Visible = false;
                cmbGroupType.Visible = false;
                dataGridView3.Visible = false;
                dataGridView4.Visible = false;
                dataGridView5.Visible = true;
            }

            if (cmbVerifyType.SelectedItem.ToString() == ab)
            {
                dataGridView3.Visible = true;
                dataGridView5.Visible = false;
                cmbGroupNum.Items.Clear();
                int count = 5;

                string[] array = new string[count];
                for (int i = 1; i <= count; i++)
                {
                    array[i - 1] = i.ToString();
                }
                cmbGroupNum.Items.AddRange(array);
                cmbGroupNum.SelectedIndex = 0;
            }
        }



        #endregion

        #region 多卡开门AB组设置

        private void CmbGroupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!CheckProtocolTypeIs89H())
            {
                return;
            }
            cmbGroupNum.Items.Clear();
            int count = 5;
            if (cmbGroupType.SelectedIndex == 1)
            {
                count = 20;
            }
            string[] array = new string[count];
            for (int i = 1; i <= count; i++)
            {
                array[i - 1] = i.ToString();
            }
            cmbGroupNum.Items.AddRange(array);
            cmbGroupNum.SelectedIndex = 0;
            //CmbGroupNum_SelectedIndexChanged(null, null);


        }

        private void InitCardDataList()
        {

            dataGridView3.AutoGenerateColumns = false;
            dataGridView4.AutoGenerateColumns = false;
            dataGridView5.AutoGenerateColumns = false;
            dataGridView4.Visible = false;
            dataGridView5.Visible = false;

            listGroupA = new List<List<decimal>>(5);
            listGroupB = new List<List<decimal>>(20);
            listFix = new List<MultiCard_GroupFix>(10);
            /**/
            for (int i = 0; i < 5; i++)
            {
                listGroupA.Add(new List<decimal>());

                for (int j = 1; j <= 50; j++)
                {
                    listGroupA[i].Add(0);
                }

            }
            for (int i = 0; i < 20; i++)
            {
                listGroupB.Add(new List<decimal>());

                for (int j = 1; j <= 100; j++)
                {
                    listGroupB[i].Add(0);
                }
            }

            for (int i = 1; i <= 10; i++)
            {
                MultiCard_GroupFix groupFix = new MultiCard_GroupFix();
                groupFix.GroupType = 1;
                groupFix.CardList = new List<decimal>();
                for (int j = 1; j <= 8; j++)
                {
                    groupFix.CardList.Add((decimal)(100000 + j));
                }
                listFix.Add(groupFix);
            }
        }

        private void CbConvertHex_CheckedChanged(object sender, EventArgs e)
        {
            /*
            //10 转 16
            if (cbConvertHex.Checked)
            {
                for (int i = 0; i < listGroupA.Count; i++)
                {
                    if (!string.IsNullOrEmpty(listGroupA[i].Card))
                    {
                        listGroupA[i].Card = int.Parse(listGroupA[i].Card).ToString("X");
                    }
                    
                }
                for (int i = 0; i < listGroupB.Count; i++)
                {
                    if (!string.IsNullOrEmpty(listGroupB[i].Card))
                    {
                        listGroupB[i].Card = int.Parse(listGroupB[i].Card).ToString("X");
                    }
                    
                }
                for (int i = 0; i < listFix.Count; i++)
                {
                    if (!string.IsNullOrEmpty(listFix[i].Card))
                    {
                        listFix[i].Card = int.Parse(listFix[i].Card).ToString("X");
                    }
                }
            }//16 转 10
            else
            {
                for (int i = 0; i < listGroupA.Count; i++)
                {
                    if (!string.IsNullOrEmpty(listGroupA[i].Card))
                        listGroupA[i].Card = Convert.ToInt32(listGroupA[i].Card, 16).ToString();
                }
                for (int i = 0; i < listGroupB.Count; i++)
                {
                    if (!string.IsNullOrEmpty(listGroupB[i].Card))
                        listGroupB[i].Card = Convert.ToInt32(listGroupB[i].Card,16).ToString();
                }
                for (int i = 0; i < listFix.Count; i++)
                {
                    if (!string.IsNullOrEmpty(listFix[i].Card))
                        listFix[i].Card = Convert.ToInt32(listFix[i].Card, 16).ToString();
                }
            }
            CmbGroupNum_SelectedIndexChanged(null, null);
            */
        }

        private void BtnAutoFill_Click(object sender, EventArgs e)
        {
            //if (cmbVerifyType.SelectedItem.ToString() == "AB组合")
            {
                listGroupA.Clear();
                for (int i = 1; i <= 5; i++)
                {
                    List<decimal> list = new List<decimal>();
                    for (int j = 1; j <= 50; j++)
                    {
                        list.Add((decimal)(100 * i + j));
                    }
                    listGroupA.Add(list);
                }
                listGroupB.Clear();
                for (int i = 1; i <= 20; i++)
                {
                    List<decimal> list = new List<decimal>();
                    for (int j = 1; j <= 100; j++)
                    {
                        list.Add((decimal)(1000 * i + j));
                    }
                    listGroupB.Add(list);
                }
            }
            //else if (cmbVerifyType.SelectedItem.ToString() == "固定组合")
            {
                listFix.Clear();
                for (int i = 1; i <= 10; i++)
                {
                    MultiCard_GroupFix groupFix = new MultiCard_GroupFix();
                    groupFix.GroupType = 1;
                    groupFix.CardList = new List<decimal>();
                    for (int j = 1; j <= 8; j++)
                    {
                        groupFix.CardList.Add((ulong)(i * 100000 + j));
                    }
                    listFix.Add(groupFix);
                }
            }
            CmbGroupNum_SelectedIndexChanged(null, null);
        }

        private void CmbGroupNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (mMainForm.GetProtocolType().Contains("Door58"))
            //{
            //    return;
            //}
            if (cmbVerifyType.SelectedItem.ToString() == GetLanguage("Msg4"))
            {
                if (cmbGroupType.SelectedIndex == 0)
                {
                    dataGridView3.Rows.Clear();
                    //dataGridView3.DataSource = new BindingList<CardData>(listGroupA.Skip(cmbGroupNum.SelectedIndex * 50).Take(50).ToArray());
                    var list = listGroupA[cmbGroupNum.SelectedIndex];
                    for (int i = 1; i <= 50; i++)
                    {
                        int index = this.dataGridView3.Rows.Add();
                        this.dataGridView3.Rows[index].Cells[0].Value = i.ToString().PadLeft(2, '0');
                        if (i > list.Count)
                        {
                            this.dataGridView3.Rows[index].Cells[1].Value = "";
                        }
                        else
                        {
                            this.dataGridView3.Rows[index].Cells[1].Value = list[i - 1].ToString();
                        }
                    }
                    dataGridView3.Visible = true;
                    dataGridView4.Visible = false;
                    dataGridView5.Visible = false;
                }
                else if (cmbGroupType.SelectedIndex == 1)
                {
                    dataGridView4.Rows.Clear();
                    //dataGridView3.DataSource = new BindingList<CardData>(listGroupA.Skip(cmbGroupNum.SelectedIndex * 50).Take(50).ToArray());
                    var list = listGroupB[cmbGroupNum.SelectedIndex];
                    for (int i = 1; i <= 100; i++)
                    {
                        int index = this.dataGridView4.Rows.Add();
                        this.dataGridView4.Rows[index].Cells[0].Value = i.ToString().PadLeft(2, '0');
                        if (i > list.Count)
                        {
                            this.dataGridView4.Rows[index].Cells[1].Value = "";
                        }
                        else
                        {
                            this.dataGridView4.Rows[index].Cells[1].Value = list[i - 1].ToString();
                        }
                    }
                    dataGridView4.Visible = true;
                    dataGridView3.Visible = false;
                    dataGridView5.Visible = false;
                }
            }

            if (cmbVerifyType.SelectedItem.ToString() == GetLanguage("Msg3"))
            {
                dataGridView5.Rows.Clear();
                //dataGridView3.DataSource = new BindingList<CardData>(listGroupA.Skip(cmbGroupNum.SelectedIndex * 50).Take(50).ToArray());
                var list = listFix[cmbGroupNum.SelectedIndex].CardList;
                for (int i = 1; i <= 8; i++)
                {
                    int index = this.dataGridView5.Rows.Add();
                    this.dataGridView5.Rows[index].Cells[0].Value = i.ToString().PadLeft(2, '0');
                    if (i > list.Count)
                    {
                        this.dataGridView5.Rows[index].Cells[1].Value = "";
                    }
                    else
                    {
                        this.dataGridView5.Rows[index].Cells[1].Value = list[i - 1].ToString();
                    }
                }
                dataGridView5.Visible = true;
                dataGridView3.Visible = false;
                dataGridView4.Visible = false;
            }
        }
        private void DataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridViewTextBoxColumn textbox = dataGridView3.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
                if (textbox != null) //如果该列是TextBox列
                {
                    dataGridView3.BeginEdit(true); //开始编辑状态
                    dataGridView3.ReadOnly = false;
                }
            }
            else
            {
                dataGridView3.BeginEdit(false); //开始编辑状态
                dataGridView3.ReadOnly = true;
            }
        }

        private void DataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridViewTextBoxColumn textbox = dataGridView4.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
                if (textbox != null) //如果该列是TextBox列
                {
                    dataGridView4.BeginEdit(true); //开始编辑状态
                    dataGridView4.ReadOnly = false;
                }
            }
            else
            {
                dataGridView4.BeginEdit(false); //开始编辑状态
                dataGridView4.ReadOnly = true;
            }
        }

        private void DataGridView3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView3.CurrentCell.ColumnIndex == 1)
            {
                //e.CellStyle.BackColor = Color.FromName("window"); 
                //DataGridViewComboBoxEditingControl editingControl = e.Control as DataGridViewComboBoxEditingControl; 
                DataGridViewTextBoxEditingControl editingControl = e.Control as DataGridViewTextBoxEditingControl;
                editingControl.TextChanged += (se, ea) =>
                {   //要判断类型
                    if (true)
                    {
                        listGroupA[cmbGroupNum.SelectedIndex][dataGridView3.CurrentCell.RowIndex] = Convert.ToUInt64(dataGridView3.CurrentCell.EditedFormattedValue.ToString());
                    }

                };
            }

        }

        private void DataGridView4_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView4.CurrentCell.ColumnIndex == 1)
            {
                //e.CellStyle.BackColor = Color.FromName("window"); 
                //DataGridViewComboBoxEditingControl editingControl = e.Control as DataGridViewComboBoxEditingControl; 
                DataGridViewTextBoxEditingControl editingControl = e.Control as DataGridViewTextBoxEditingControl;
                editingControl.TextChanged += (se, ea) =>
                {
                    //listGroupB[dataGridView4.CurrentCell.RowIndex].Card = dataGridView4.CurrentCell.EditedFormattedValue.ToString();
                    if (true)
                    {
                        listGroupB[cmbGroupNum.SelectedIndex][dataGridView4.CurrentCell.RowIndex] = Convert.ToUInt64(dataGridView4.CurrentCell.EditedFormattedValue.ToString());
                    }
                };
            }
        }

        private void BtnDeleteGroup_Click(object sender, EventArgs e)
        {
            if (cmbVerifyType.SelectedItem.ToString() == GetLanguage("Msg4"))
            {
                if (cmbGroupType.SelectedIndex == 0)
                {
                    for (int i = 0; i < listGroupA.Count; i++)
                    {
                        for (int j = listGroupA[i].Count; j > 0; j--)
                        {
                            listGroupA[i].RemoveAt(j - 1);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < listGroupB.Count; i++)
                    {
                        for (int j = listGroupB[i].Count; j > 0; j--)
                        {
                            listGroupB[i].RemoveAt(j - 1);
                        }
                    }
                }
            }
            else if (cmbVerifyType.SelectedItem.ToString() == GetLanguage("Msg3"))
            {
                for (int i = 0; i < listFix.Count; i++)
                {
                    for (int j = listFix[i].CardList.Count; j > 0; j--)
                    {
                        listFix[i].CardList.RemoveAt(j - 1);
                    }
                }
            }
            CmbGroupNum_SelectedIndexChanged(null, null);
        }

        #endregion

        private void DataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridViewTextBoxColumn textbox = dataGridView5.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
                if (textbox != null) //如果该列是TextBox列
                {
                    dataGridView5.BeginEdit(true); //开始编辑状态
                    dataGridView5.ReadOnly = false;
                }

            }
            else
            {
                dataGridView5.BeginEdit(false); //开始编辑状态
                dataGridView5.ReadOnly = true;
            }
        }

        private void DataGridView5_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView5.CurrentCell.ColumnIndex == 1)
            {
                DataGridViewTextBoxEditingControl editingControl = e.Control as DataGridViewTextBoxEditingControl;
                editingControl.TextChanged += (se, ea) =>
                {
                    //listFix[dataGridView5.CurrentCell.RowIndex].Card = dataGridView5.CurrentCell.EditedFormattedValue.ToString();
                    if (true)
                    {
                        listFix[cmbGroupNum.SelectedIndex].CardList[dataGridView5.CurrentCell.RowIndex] = Convert.ToUInt64(dataGridView5.CurrentCell.EditedFormattedValue.ToString());
                    }
                };
            }
        }
        #region 读卡器防拆报警功能
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReadReaderAlarm_Click(object sender, EventArgs e)
        {

            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReaderAlarm cmd = new ReadReaderAlarm(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReaderAlarm_Result result = cmde.Command.getResult() as ReaderAlarm_Result;
                Invoke(() =>
                {
                    cbReaderAlarmUse.Checked = result.Use == 1;
                });
                string tip2 = result.Use == 1 ? GetLanguage("Msg57") : GetLanguage("Msg58");
                mMainForm.AddCmdLog(cmde, $"{GetLanguage("Msg59")}{door}，{GetLanguage("Msg8")}：{result.Use}、{tip2}");
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWriteReaderAlarm_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteReaderAlarm_Parameter par = new WriteReaderAlarm_Parameter(door, (byte)(cbReaderAlarmUse.Checked ? 1 : 0));
            WriteReaderAlarm cmd = new WriteReaderAlarm(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {

            };
        }

        #endregion

        private void BtnReadReadCardAndTakePictures_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReadCardAndTakePictures cmd = new ReadReadCardAndTakePictures(cmdDtl, new DoorPort_Parameter(cmdDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCardAndTakePictures_Result result = cmde.Command.getResult() as ReadCardAndTakePictures_Result;
                Invoke(() =>
                {
                    cbInDoorUse.Checked = result.InDoorUse;
                    cbOutDoorUse.Checked = result.OutDoorUse;

                    txtInDoorIP.Text = string.Join(".", result.InDoorIP.Select(t => t.ToString()));
                    txtOutDoorIP.Text = string.Join(".", result.OutDoorIP.Select(t => t.ToString()));

                    txtInDoorPort.Text = result.InDoorPort.ToString();
                    txtOutDoorPort.Text = result.OutDoorPort.ToString();

                    txtInDoorProtocol.Text = string.Join("", result.InDoorProtocol);
                    txtOutDoorProtocol.Text = string.Join("", result.OutDoorProtocol);
                });

            };
        }

        private void BtnWriteReadCardAndTakePictures_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmdDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] bIndoorIP = new byte[4];
            string sIP = txtInDoorIP.Text;
            if (TCPDetail.CheckIP(sIP))
            {
                string[] listip = sIP.Trim().Split('.');
                for (int i = 0; i < listip.Length; i++)
                {
                    bIndoorIP[i] = byte.Parse(listip[i]);
                }
            }
            else
            {
                MsgErr(GetLanguage("Msg60"));
                return;
            }


            byte[] bOutdoorIP = new byte[4];
            sIP = txtOutDoorIP.Text;
            if (TCPDetail.CheckIP(sIP))
            {
                string[] listip = sIP.Trim().Split('.');
                for (int i = 0; i < listip.Length; i++)
                {
                    bOutdoorIP[i] = byte.Parse(listip[i]);
                }
            }
            else
            {
                MsgErr(GetLanguage("Msg60"));
                return;
            }


            WriteReadCardAndTakePictures_Parameter par = new WriteReadCardAndTakePictures_Parameter(door, cbInDoorUse.Checked, bIndoorIP, ushort.Parse(txtInDoorPort.Text), txtInDoorProtocol.Text,
                cbOutDoorUse.Checked, bOutdoorIP, ushort.Parse(txtOutDoorPort.Text), txtOutDoorProtocol.Text);
            WriteReadCardAndTakePictures cmd = new WriteReadCardAndTakePictures(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }
    }
}
