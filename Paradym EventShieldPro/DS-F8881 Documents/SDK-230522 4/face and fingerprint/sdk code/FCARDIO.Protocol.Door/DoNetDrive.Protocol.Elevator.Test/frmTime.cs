using DoNetDrive.Protocol.Elevator.FC8864.Time;
using DoNetDrive.Protocol.Elevator.FC8864.Time.TimeErrorCorrection;
using System;
using System.Text.RegularExpressions;

namespace DoNetDrive.Protocol.Elevator.Test
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
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.Time. ReadTime_Result;
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
                        tip = "无误差";
                    }
                    else
                    {
                        tip = "误差" + Seconds + "秒";
                    }
                    txtErrorTime.Text = tip;
                });
                string ControllerDateInfo = "电脑时间：" + txtComputerTime.Text +
                                            "，控制板时间：" + ControllerDate + "，" + tip;
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
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.Time.TimeErrorCorrection.ReadTimeError_Result;
                string CorrectionState = result.TimeErrorCorrection[0] == 0 ? "调慢" : "调快"; //误差修正状态
                int CorrectionSeconds = result.TimeErrorCorrection[1]; //误差修正秒数
                string tip = string.Empty;
                if (CorrectionSeconds == 0)
                {
                    tip = "禁用";
                }
                else
                {
                    tip = "每天自动" + CorrectionState + CorrectionSeconds + "秒";
                }

                Invoke(() =>
                {
                    rBtnSlowDown.Checked = result.TimeErrorCorrection[0] == 0;
                    cbxCorrectionSeconds.Text = CorrectionSeconds == 0 ? "禁用" : CorrectionSeconds.ToString();
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void BtnWriteTimeCorrectionParameter_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxCorrectionSeconds.Text.Trim(), reg) || string.IsNullOrEmpty(cbxCorrectionSeconds.Text.Trim()))
            {
                if (cbxCorrectionSeconds.Text != "禁用")
                {
                    MsgErr("请输入正确修正秒数！");
                    return;
                }
            }
            if (Regex.IsMatch(cbxCorrectionSeconds.Text.Trim(), reg))
            {
                if (Convert.ToUInt32(cbxCorrectionSeconds.Text) < 0 || Convert.ToUInt32(cbxCorrectionSeconds.Text) > 255)
                {
                    MsgErr("请输入正确修正秒数！");
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

            if (cbxCorrectionSeconds.Text == "禁用")
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
