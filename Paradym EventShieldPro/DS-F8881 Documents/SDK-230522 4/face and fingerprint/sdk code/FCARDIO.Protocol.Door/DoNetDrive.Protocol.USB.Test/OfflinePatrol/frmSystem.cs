
using DoNetDrive.Protocol.USB.OfflinePatrol.OperatedDevice.LCDFlash;
using DoNetDrive.Protocol.USB.OfflinePatrol.OperatedDevice.TriggerBuzzer;
using DoNetDrive.Protocol.USB.OfflinePatrol.OperatedDevice.TriggerDoubleLamp;
using DoNetDrive.Protocol.USB.OfflinePatrol.OperatedDevice.TriggerLED;
using DoNetDrive.Protocol.USB.OfflinePatrol.OperatedDevice.TriggerVibrate;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.CreateTime;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ExpireTime;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.Initialize;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.LEDOpenHoldTime;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.RecordStorageMode;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.SN;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.StartupHoldTime;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.SystemStatus;
using DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.Version;
using System;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Test
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
                        FrmMain.AddNodeForms(onlyObj);
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


        #region 地址码
        private void BtnReadSN_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadSN cmd = new ReadSN(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                SN_Result result = cmde.Command.getResult() as SN_Result;
                string sn = result.SN.ToString();
                Invoke(() =>
                {
                    txtAddress.Text = sn;
                });
                mMainForm.AddCmdLog(cmde, $"地址号：{sn}");
            };
        }

        private void BtnWriteSN_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte sn = 0;
            if (!byte.TryParse(txtAddress.Text, out sn))
            {
                MessageBox.Show("SN格式不正确！");
                return;
            }
            SN_Parameter par = new SN_Parameter(sn);
            WriteSN cmd = new WriteSN(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"写入SN成功");
            };
        }

        #endregion
        /// <summary>
        /// 读取 版本号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                Invoke(() =>
                {
                    txtVersion.Text = result.Version;
                });
                mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        /// <summary>
        /// 读取 截止日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReadExpireTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadExpireTime cmd = new ReadExpireTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ExpireTime_Result result = cmde.Command.getResult() as ExpireTime_Result;

                Invoke(() =>
                {
                    dtpExpireTime.Value = result.Time;
                });
                mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        /// <summary>
        /// 写入 截止日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWriteExpireTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ExpireTime_Parameter par = new ExpireTime_Parameter(dtpExpireTime.Value);
            WriteExpireTime cmd = new WriteExpireTime(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        /// <summary>
        /// 读取运行信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                Invoke(() =>
                {
                    lbTime.Text = result.Time.ToString("yyyy-MM-dd");
                    lbFormatCount.Text = result.FormatCount.ToString();
                    lbElectricity.Text = result.Electricity.ToString();
                    lbPatrolEmpl.Text = result.PatrolEmpl.ToString();
                    lbPatrolEmplCount.Text = result.PatrolEmplCount.ToString();
                    lbStartCount.Text = result.StartCount.ToString();
                    lbSystemRecordCount.Text = result.SystemRecordCount.ToString();
                    lbVoltage.Text = result.Voltage;
                    lbCardRecordCount.Text = result.CardRecordCount.ToString();
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        #region 开机保持
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReadStartupHoldTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadStartupHoldTime cmd = new ReadStartupHoldTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadStartupHoldTime_Result result = cmde.Command.getResult() as ReadStartupHoldTime_Result;

                Invoke(() =>
                {
                    cmbStartupHoldTime.SelectedItem = result.Time.ToString();
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWriteStartupHoldTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteStartupHoldTime_Parameter par = new WriteStartupHoldTime_Parameter();
            par.Time = Convert.ToByte(cmbStartupHoldTime.SelectedItem);
            WriteStartupHoldTime cmd = new WriteStartupHoldTime(cmdDtl, par);
            mMainForm.AddCommand(cmd);

           
        }

        #endregion

        #region 出厂日期
        private void ReadCreateTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadCreateTime cmd = new ReadCreateTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCreateTime_Result result = cmde.Command.getResult() as ReadCreateTime_Result;

                Invoke(() =>
                {
                    dtpCreateTime.Value = result.Time;
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void WriteCreateTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteCreateTime_Parameter par = new WriteCreateTime_Parameter(dtpCreateTime.Value);
            WriteCreateTime cmd = new WriteCreateTime(cmdDtl, par);
            mMainForm.AddCommand(cmd);

          
        }
        #endregion

        #region LED开灯保持
        private void ReadLEDOpenTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadLEDOpenHoldTime cmd = new ReadLEDOpenHoldTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadLEDOpenHoldTime_Result result = cmde.Command.getResult() as ReadLEDOpenHoldTime_Result;

                Invoke(() =>
                {
                    cmbLEDOpenTime.SelectedItem = result.Time.ToString();
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void WriteLEDOpenTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteLEDOpenHoldTime_Parameter par = new WriteLEDOpenHoldTime_Parameter();
            par.Time = Convert.ToByte(cmbLEDOpenTime.SelectedItem);
            WriteLEDOpenHoldTime cmd = new WriteLEDOpenHoldTime(cmdDtl, par);
            mMainForm.AddCommand(cmd);

          
        }
        #endregion

        private void BtnReadRecordStorageMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadRecordStorageMode cmd = new ReadRecordStorageMode(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadRecordStorageMode_Result result = cmde.Command.getResult() as ReadRecordStorageMode_Result;

                Invoke(() =>
                {
                    rbStorageMode0.Checked = result.Mode == 0;
                    rbStorageMode1.Checked = result.Mode == 1;
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void BtnWriteRecordStorageMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteRecordStorageMode_Parameter par = new WriteRecordStorageMode_Parameter((byte)(rbStorageMode0.Checked ? 0 : 1));
            WriteRecordStorageMode cmd = new WriteRecordStorageMode(cmdDtl, par);
            mMainForm.AddCommand(cmd);

          
        }
        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            GetLanguage(groupBox2);
            GetLanguage(label3);
            GetLanguage(groupBox3);
            GetLanguage(label4);
            GetLanguage(label5);
            GetLanguage(groupBox1);
            GetLanguage(label2);
            GetLanguage(rbStorageMode0);
            GetLanguage(rbStorageMode1);
            GetLanguage(groupBox4);
            GetLanguage(label6);
            GetLanguage(label13);
            GetLanguage(label18);
            GetLanguage(label7);
            GetLanguage(label12);
            GetLanguage(label8);
            GetLanguage(label11);
            GetLanguage(label9);
            GetLanguage(label10);
            GetLanguage(groupBox5);
            GetLanguage(label14);
            GetLanguage(label16);
            GetLanguage(label15);
            GetLanguage(groupBox6);
            GetLanguage(groupBox7);
            GetLanguage(btnTestBuzzer);
            GetLanguage(groupBox8);
            GetLanguage(btnOpenFlash);
            GetLanguage(btnCloseFlash);
            GetLanguage(groupBox9);
            GetLanguage(btnOpenLed);
            GetLanguage(btnCloseLed);
            GetLanguage(groupBox10);
            GetLanguage(btnCloseLamp);
            GetLanguage(btnOpenGreen);
            GetLanguage(btnOpenRed);
            GetLanguage(groupBox11);
            GetLanguage(label23);
            GetLanguage(btnTestVibrate);
            GetLanguage(btnInitialize);
            var btnRead = GetLanguage("btnRead");
            var btnWrite = GetLanguage("btnWrite");
            btnReadSN.Text = btnRead;
            btnWriteSN.Text = btnWrite;
            btnReadExpireTime.Text = btnRead;
            btnWriteExpireTime.Text = btnWrite;
            btnReadVersion.Text = btnRead;
            btnReadRecordStorageMode.Text = btnRead;
            btnWriteRecordStorageMode.Text = btnWrite;
            ReadCreateTime.Text = btnRead;
            WriteCreateTime.Text = btnWrite;
            btnReadStartupHoldTime.Text = btnRead;
            btnWriteStartupHoldTime.Text = btnWrite;
            ReadLEDOpenTime.Text = btnRead;
            WriteLEDOpenTime.Text = btnWrite;

        }
        private void FrmSystem_Load(object sender, EventArgs e)
        {
            cmbStartupHoldTime.Items.Clear();
            string[] list = new string[56];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = (i + 5).ToString();
            }
            cmbStartupHoldTime.Items.AddRange(list);
            cmbStartupHoldTime.SelectedIndex = 5;

            cmbLEDOpenTime.Items.Clear();
            string[] listTime = new string[255];
            for (int i = 0; i < listTime.Length; i++)
            {
                listTime[i] = (i + 1).ToString();
            }
            cmbLEDOpenTime.Items.AddRange(listTime);
            cmbLEDOpenTime.SelectedIndex = 5;

            cmbVibrateTime.Items.Clear();
            string[] listVibrateTime = new string[650];
            for (int i = 0; i < listVibrateTime.Length; i++)
            {
                listVibrateTime[i] = ((i + 1) * 100).ToString();
            }
            cmbVibrateTime.Items.AddRange(listVibrateTime);
            cmbVibrateTime.SelectedIndex = 5;
            LoadUILanguage();
        }

        private void BtnTestBuzzer_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            TriggerBuzzer cmd = new TriggerBuzzer(cmdDtl);
            mMainForm.AddCommand(cmd);

          
        }

        private void BtnOpenLed_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            TriggerLED_Parameter par = new TriggerLED_Parameter(128);
            TriggerLED cmd = new TriggerLED(cmdDtl, par);
            mMainForm.AddCommand(cmd);

          
        }

        private void BtnCloseLed_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            TriggerLED_Parameter par = new TriggerLED_Parameter(8);
            TriggerLED cmd = new TriggerLED(cmdDtl, par);
            mMainForm.AddCommand(cmd);

           
        }

        private void BtnTestVibrate_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            TriggerVibrate_Parameter par = new TriggerVibrate_Parameter(Convert.ToUInt16(cmbVibrateTime.SelectedItem));
            TriggerVibrate cmd = new TriggerVibrate(cmdDtl, par);
            mMainForm.AddCommand(cmd);

           
        }

        private void BtnOpenFlash_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            LCDFlash_Parameter par = new LCDFlash_Parameter(128);
            LCDFlash cmd = new LCDFlash(cmdDtl, par);
            mMainForm.AddCommand(cmd);

           
        }

        private void BtnCloseFlash_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            LCDFlash_Parameter par = new LCDFlash_Parameter(8);
            LCDFlash cmd = new LCDFlash(cmdDtl, par);
            mMainForm.AddCommand(cmd);

          
        }

        private void BtnCloseLamp_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            TriggerDoubleLamp_Parameter par = new TriggerDoubleLamp_Parameter(0);
            TriggerDoubleLamp cmd = new TriggerDoubleLamp(cmdDtl, par);
            mMainForm.AddCommand(cmd);

          
        }

        private void BtnOpenGreen_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            TriggerDoubleLamp_Parameter par = new TriggerDoubleLamp_Parameter(128);
            TriggerDoubleLamp cmd = new TriggerDoubleLamp(cmdDtl, par);
            mMainForm.AddCommand(cmd);

           
        }

        private void BtnOpenRed_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            TriggerDoubleLamp_Parameter par = new TriggerDoubleLamp_Parameter(8);
            TriggerDoubleLamp cmd = new TriggerDoubleLamp(cmdDtl, par);
            mMainForm.AddCommand(cmd);

           
        }

        private void BtnInitialize_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            Initialize cmd = new Initialize(cmdDtl);
            mMainForm.AddCommand(cmd);
        }


    }
}
