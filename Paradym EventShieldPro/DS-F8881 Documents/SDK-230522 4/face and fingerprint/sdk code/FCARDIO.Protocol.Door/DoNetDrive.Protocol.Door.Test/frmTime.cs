using DoNetDrive.Protocol.Door.Door8800.Time;
using DoNetDrive.Protocol.Door.Door8800.Time.TimeErrorCorrection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.Door.Test
{
    public partial class frmTime : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static frmTime onlyObj;
        public static frmTime GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmTime(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private frmTime(INMain main) : base(main)
        {
            InitializeComponent();
        }
        #endregion

        private void frmTime_Load(object sender, EventArgs e)
        {
            LoadUILanguage();
        }

        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            GetLanguage(groupBox1);
            GetLanguage(label26);
            GetLanguage(label1);
            GetLanguage(label2);
            GetLanguage(btnReadSystemTime);
            GetLanguage(btnWriteSystemTime);
            GetLanguage(btnWriteBroadcastTime);
            GetLanguage(groupBox20);
            GetLanguage(rBtnSpeedUp);
            GetLanguage(rBtnSlowDown);
            GetLanguage(label75);
            GetLanguage(label72);
            GetLanguage(btnReadTimeCorrectionParameter);
            GetLanguage(btnWriteTimeCorrectionParameter);
            GetLanguage(groupBox2);
            GetLanguage(label4);
            GetLanguage(btnWriteCustomDateTime);
            var disable = GetLanguage("Disable");
            cbxCorrectionSeconds.Items.Clear();
            cbxCorrectionSeconds.Items.Add(disable);
            for (int i = 1; i < 256; i++)
            {
                cbxCorrectionSeconds.Items.Add(i);
            }
            cbxCorrectionSeconds.SelectedIndex = 0;
        }

        #region 设备时间读写
        private void BtnReadSystemTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTime cmd = new ReadTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTime_Result result = cmde.Command.getResult() as ReadTime_Result;
                string ControllerDate = result.ControllerDate.ToString("yyyy-MM-dd HH:mm:ss"); //设备时间
                int Seconds = 0; //误差秒数
                string tip = string.Empty;
                Invoke(() =>
                {
                    txtSystemTime.Text = ControllerDate;
                    txtComputerTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    TimeSpan ts = Convert.ToDateTime(txtComputerTime.Text) - result.ControllerDate;
                    Seconds = Math.Abs(Convert.ToInt32(ts.TotalSeconds));
                    if (Seconds < 4 && Seconds > -4)
                    {
                        tip = GetLanguage("Msg1");
                    }
                    else
                    {
                        tip = GetLanguage("Msg2") + Seconds + GetLanguage("Msg3");
                    }
                    txtErrorTime.Text = tip;
                });
                string ControllerDateInfo = GetLanguage("Msg4")+"：" + txtComputerTime.Text +
                                            $"，{GetLanguage("Msg5")}：" + ControllerDate + "，" + tip;
                mMainForm.AddCmdLog(cmde, ControllerDateInfo);
            };
        }

        private void BtnWriteSystemTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteTime cmd = new WriteTime(cmdDtl);
            mMainForm.AddCommand(cmd);
        }

        private void BtnWriteBroadcastTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteTimeBroadcast cmd = new WriteTimeBroadcast(cmdDtl);
            mMainForm.AddCommand(cmd);
        }
        private void BtnWriteCustomDateTime_Click(object sender, EventArgs e)
        {
            DateTime CustomTime = Convert.ToDateTime(CustomDateTime.Text);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteCustomTime cmd = new WriteCustomTime(cmdDtl, new WriteCustomTime_Parameter(CustomTime));
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 误差自修正参数读写
        private void BtnReadTimeCorrectionParameter_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTimeError cmd = new ReadTimeError(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTimeError_Result result = cmde.Command.getResult() as ReadTimeError_Result;
                string CorrectionState = result.TimeErrorCorrection[0] == 0 ? GetLanguage("rBtnSlowDown") : GetLanguage("rBtnSpeedUp"); //误差修正状态
                int CorrectionSeconds = result.TimeErrorCorrection[1]; //误差修正秒数
                string tip = string.Empty;
                if (CorrectionSeconds == 0)
                {
                    tip = GetLanguage("Disable");
                }
                else
                {
                    tip = GetLanguage("Msg6") + CorrectionState + CorrectionSeconds + GetLanguage("Msg3");
                }

                Invoke(() =>
                {
                    rBtnSlowDown.Checked = result.TimeErrorCorrection[0] == 0;
                    cbxCorrectionSeconds.Text = CorrectionSeconds == 0 ? GetLanguage("Disable") : CorrectionSeconds.ToString();
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void BtnWriteTimeCorrectionParameter_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxCorrectionSeconds.Text.Trim(), reg) || string.IsNullOrEmpty(cbxCorrectionSeconds.Text.Trim()))
            {
                if (cbxCorrectionSeconds.Text != GetLanguage("Disable"))
                {
                    MsgErr(GetLanguage("Msg7"));
                    return;
                }
            }
            if (Regex.IsMatch(cbxCorrectionSeconds.Text.Trim(), reg))
            {
                if (Convert.ToUInt32(cbxCorrectionSeconds.Text) < 0 || Convert.ToUInt32(cbxCorrectionSeconds.Text) > 255)
                {
                    MsgErr(GetLanguage("Msg7"));
                    return;
                }
            }

            byte[] TimeErrorCorrection = new byte[2];

            if (rBtnSpeedUp.Checked == true)
            {
                TimeErrorCorrection[0] = 1;
            }
            else
            {
                TimeErrorCorrection[0] = 0;
            }

            if (cbxCorrectionSeconds.Text == GetLanguage("Disable"))
            {
                TimeErrorCorrection[1] = 0;
            }
            else
            {
                TimeErrorCorrection[1] = Convert.ToByte(cbxCorrectionSeconds.Text);
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteTimeError cmd = new WriteTimeError(cmdDtl, new WriteTimeError_Parameter(TimeErrorCorrection));
            mMainForm.AddCommand(cmd);
        }
        #endregion
    }
}
