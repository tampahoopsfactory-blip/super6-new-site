
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.AgencyCode;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.Buzzer;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.CreateTime;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardControl;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardCustomNum;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.Initialize;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.LED;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.OutputFormat;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.ReadCardType;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.TTLOutput;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN;
using DoNetDrive.Protocol.USB.CardReader.SystemParameter.Version;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.USB.CardReader.Test
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
            if (!byte.TryParse(txtAddress.Text,out sn))
            {
                MessageBox.Show("SN格式不正确！");
                return;
            }
            SN_Parameter par = new SN_Parameter(sn);
            WriteSN cmd = new WriteSN(cmdDtl,par);
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
                    txtVersion.Text = result.VerNum + "." + result.Revise;
                });
                mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
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
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWriteStartupHoldTime_Click(object sender, EventArgs e)
        {
           
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

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }
        #endregion

        #region LED开灯保持
        private void ReadLEDOpenTime_Click(object sender, EventArgs e)
        {
           
        }

        private void WriteLEDOpenTime_Click(object sender, EventArgs e)
        {
           
        }
        #endregion

        private void FrmSystem_Load(object sender, EventArgs e)
        {
            InitComboBox();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitComboBox()
        {
            cmbICCardControlNum.Items.Clear();
            cmbICCardCustomNumNum.Items.Clear();
            string[] list = new string[15];
            for (int i = 0; i < 15; i++)
            {
                list[i] = (i + 1).ToString();
            }
            cmbICCardControlNum.Items.AddRange(list);
            cmbICCardControlNum.SelectedIndex = 0;
            cmbICCardCustomNumNum.Items.AddRange(list);
            cmbICCardCustomNumNum.SelectedIndex = 0;

            cmbICCardControlSectorContentLength.Items.Clear();
            string[] listlength = new string[17];
            for (int i = 0; i < 17; i++)
            {
                listlength[i] = (i).ToString();
            }
            cmbICCardControlSectorContentLength.Items.AddRange(listlength);
            cmbICCardControlSectorContentLength.SelectedIndex = 0;

            cmbICCardCustomNumCardLength.Items.Clear();
            string[] listCardlength = new string[7];
            for (int i = 0; i < 7; i++)
            {
                listCardlength[i] = (i + 2).ToString();
            }
            cmbICCardCustomNumCardLength.Items.AddRange(listCardlength);
            cmbICCardCustomNumCardLength.SelectedIndex = 0;

            cmbTTLBaudRate.Items.Clear();
            string[] listBaudRate = new string[] { "1200", "2400", "4800", "9600", "11400", "19200", "38400", "43000", "56000", "57600", "115200" };
            cmbTTLBaudRate.Items.AddRange(listBaudRate);
            cmbTTLBaudRate.SelectedIndex = 0;

            cmbTTLParity.Items.Clear();
            string[] listParity = new string[] { "无", "偶数", "奇数" };
            cmbTTLParity.Items.AddRange(listParity);
            cmbTTLParity.SelectedIndex = 0;

            cmbTTLDataBits.Items.Clear();
            string[] listDataBits = new string[] { "4", "5", "6", "7", "8" };
            cmbTTLDataBits.Items.AddRange(listDataBits);
            cmbTTLDataBits.SelectedIndex = 0;

            cmbTTLStopBits.Items.Clear();
            string[] listStopBits = new string[] { "1", "1.5", "2" };
            cmbTTLStopBits.Items.AddRange(listStopBits);
            cmbTTLStopBits.SelectedIndex = 0;
        }

        private void BtnReadReadCardType_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReadCardType cmd = new ReadReadCardType(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadReadCardType_Result result = cmde.Command.getResult() as ReadReadCardType_Result;

                Invoke(() =>
                {
                    var list = result.BitList;
                    cbReadCardType1.Checked = list[0] == 1;
                    cbReadCardType2.Checked = list[1] == 1;
                    cbReadCardType3.Checked = list[2] == 1;
                    cbReadCardType4.Checked = list[3] == 1;
                    cbReadCardType5.Checked = list[4] == 1;
                    cbReadCardType6.Checked = list[5] == 1;
                    cbReadCardType7.Checked = list[6] == 1;
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void BtnWriteReadCardType_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[7];
            list[0] = Convert.ToByte(cbReadCardType1.Checked);
            list[1] = Convert.ToByte(cbReadCardType2.Checked);
            list[2] = Convert.ToByte(cbReadCardType3.Checked);
            list[3] = Convert.ToByte(cbReadCardType4.Checked);
            list[4] = Convert.ToByte(cbReadCardType5.Checked);
            list[5] = Convert.ToByte(cbReadCardType6.Checked);
            list[6] = Convert.ToByte(cbReadCardType7.Checked);
            WriteReadCardType_Parameter par = new WriteReadCardType_Parameter(list);
            WriteReadCardType cmd = new WriteReadCardType(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }

        private void BtnReadOutputFormat_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadOutputFormat cmd = new ReadOutputFormat(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadOutputFormat_Result result = cmde.Command.getResult() as ReadOutputFormat_Result;

                Invoke(() =>
                {
                    cmbFormat.SelectedIndex = result.Format;
                    cmbSort.SelectedIndex = result.Sort - 1;
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void BtnWriteOutputFormat_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte format = (byte)cmbFormat.SelectedIndex;
            byte sort = (byte)(cmbSort.SelectedIndex + 1);
            WriteOutputFormat_Parameter par = new WriteOutputFormat_Parameter(format,sort);
            WriteOutputFormat cmd = new WriteOutputFormat(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }

        private void FrmSystem_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }

        #region
        private void CbICCardControlUse_CheckedChanged(object sender, EventArgs e)
        {

        }

        #endregion

        private void BtnReadICCardControl_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadICCardControl cmd = new ReadICCardControl(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadICCardControl_Result result = cmde.Command.getResult() as ReadICCardControl_Result;

                Invoke(() =>
                {
                    cbICCardControlUse.Checked = result.IsOpen;
                    cmbICCardControlNum.SelectedIndex = result.Num - 1;
                    txtICCardControlPassword.Text = result.Password;
                    cmbICCardControlVerifyMode.SelectedIndex = result.VerifyMode - 1;
                    cmbICCardControlComputingMode.SelectedIndex = result.ComputingMode - 1;

                    cbIsOpenVerifySector.Checked = result.IsOpenVerifySector;
                    cmbICCardControlSectorContentLength.SelectedIndex = result.SectorContentLength;
                    cmbICCardControlVerifyDataStartIndex.SelectedIndex = result.VerifyDataStartIndex;
                    txtICCardControlVerifyContent.Text = result.VerifyContent;
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void BtnWriteICCardControl_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte num = (byte)(cmbICCardControlNum.SelectedIndex + 1);
            byte verifyMode = (byte)(cmbICCardControlVerifyMode.SelectedIndex + 1);
            byte computingMode = (byte)(cmbICCardControlComputingMode.SelectedIndex + 1);
            byte sectorContentLength = (byte)cmbICCardControlSectorContentLength.SelectedIndex;
            byte verifyDataStartIndex = (byte)cmbICCardControlVerifyDataStartIndex.SelectedIndex;

            WriteICCardControl_Parameter par = new WriteICCardControl_Parameter(cbICCardControlUse.Checked, num,txtICCardControlPassword.Text,verifyMode, computingMode,
                cbIsOpenVerifySector.Checked, sectorContentLength, verifyDataStartIndex,txtICCardControlVerifyContent.Text);
            WriteICCardControl cmd = new WriteICCardControl(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }

        private void BtnReadICCardCustomNum_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadICCardCustomNum cmd = new ReadICCardCustomNum(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadICCardCustomNum_Result result = cmde.Command.getResult() as ReadICCardCustomNum_Result;

                Invoke(() =>
                {
                    cbICCardCustomNumIsOpen.Checked = result.IsOpen;
                    cmbICCardCustomNumNum.SelectedIndex = result.Num - 1;
                    txtICCardCustomNumPassword.Text = result.Password;
                    cmbICCardCustomNumVerifyMode.SelectedIndex = result.VerifyMode - 1;
                    cmbICCardCustomNumComputingMode.SelectedIndex = result.ComputingMode - 1;

                    cmbICCardCustomNumCardLength.SelectedIndex = result.CardLength;
                    cmbICCardCustomNumCardDataStartIndex.SelectedIndex = result.CardDataStartIndex;
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void BtnWriteICCardCustomNum_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte num = (byte)(cmbICCardCustomNumNum.SelectedIndex + 1);
            byte verifyMode = (byte)(cmbICCardCustomNumVerifyMode.SelectedIndex + 1);
            byte computingMode = (byte)(cmbICCardCustomNumComputingMode.SelectedIndex + 1);
            byte cardLength = (byte)cmbICCardCustomNumCardLength.SelectedIndex;
            byte cardDataStartIndex = (byte)cmbICCardCustomNumCardDataStartIndex.SelectedIndex;

            WriteICCardCustomNum_Parameter par = new WriteICCardCustomNum_Parameter(cbICCardCustomNumIsOpen.Checked, num, txtICCardCustomNumPassword.Text, verifyMode, computingMode,
                 cardLength, cardDataStartIndex);
            WriteICCardCustomNum cmd = new WriteICCardCustomNum(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }

        private void BtnReadTTLOutput_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTTLOutput cmd = new ReadTTLOutput(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTTLOutput_Result result = cmde.Command.getResult() as ReadTTLOutput_Result;

                Invoke(() =>
                {
                    cbTTLIsOpen.Checked = result.IsOpen;
                    cmbTTLBaudRate.SelectedIndex = result.BaudRate;
                    cmbTTLParity.SelectedIndex = result.Parity;
                    cmbTTLDataBits.SelectedItem = result.DataBits.ToString();
                    cmbTTLStopBits.SelectedIndex = result.StopBits;
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void BtnWriteTTLOutput_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte baudRate = (byte)(cmbTTLBaudRate.SelectedIndex);
            byte parity = (byte)(cmbTTLParity.SelectedIndex);
            byte dataBits = Convert.ToByte (cmbTTLDataBits.SelectedItem);
            byte stopBits = (byte)cmbTTLStopBits.SelectedIndex;

            WriteTTLOutput_Parameter par = new WriteTTLOutput_Parameter(cbTTLIsOpen.Checked, baudRate, parity, dataBits, stopBits);
            WriteTTLOutput cmd = new WriteTTLOutput(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }

        private void BtnWriteBuzzer_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteBuzzer_Parameter par = new WriteBuzzer_Parameter(byte.Parse(txtBuzzerTime.Text));
            WriteBuzzer cmd = new WriteBuzzer(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }

        private void BtnWriteLED_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteLED_Parameter par = new WriteLED_Parameter((byte)(cmbLED.SelectedIndex + 1));
            WriteLED cmd = new WriteLED(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }

        private void BtnReadAgencyCode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAgencyCode cmd = new ReadAgencyCode(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAgencyCode_Result result = cmde.Command.getResult() as ReadAgencyCode_Result;

                Invoke(() =>
                {
                    txtCode.Text = result.Code;
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void BtnWriteAgencyCode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            if (txtCode.Text.Length >8)
            {
                MessageBox.Show("长度为8");
                return;
            }
            string pattern = @"^([0-9a-fA-F]+)$";
            bool isHexNum = Regex.IsMatch(txtCode.Text, pattern);
            if (!isHexNum)
            {
                MessageBox.Show("格式为十六进制");
                return;
            }
            WriteAgencyCode_Parameter par = new WriteAgencyCode_Parameter(txtCode.Text);
            WriteAgencyCode cmd = new WriteAgencyCode(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }

        private void BtnInitialize_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            Initialize cmd = new Initialize(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
               
            };
        }
    }
}
