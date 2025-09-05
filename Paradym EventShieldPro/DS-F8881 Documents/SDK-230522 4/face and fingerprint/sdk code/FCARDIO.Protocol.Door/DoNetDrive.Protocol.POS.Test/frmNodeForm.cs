using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using System;
using System.Windows.Forms;

namespace DotNetDrive.Protocol.POS.Test
{
    public partial class frmNodeForm : Form
    {

        /// <summary>
        /// 主窗体访问接口
        /// </summary>
        protected INMain mMainForm;

        /// <summary>
        /// 在UI线程上执行一个方法
        /// </summary>
        /// <param name="p"></param>
        protected void Invoke(Action p)
        {
            Invoke((Delegate)p);
        }

        /// <summary>
        /// 线程休眠
        /// </summary>
        /// <param name="time"></param>
        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

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


        public frmNodeForm()
        {
            InitializeComponent();
        }

        public frmNodeForm(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }

        private void frmNodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void frmNodeForm_Load(object sender, EventArgs e)
        {

        }

        public Control FindControl(Control parentControl, string findCtrlName)
        {
            Control _findedControl = null;
            if (!string.IsNullOrEmpty(findCtrlName) && parentControl != null)
            {
                foreach (Control ctrl in parentControl.Controls)
                {
                    if (ctrl.Name.Equals(findCtrlName))
                    {
                        _findedControl = ctrl;
                        break;
                    }
                }
            }
            return _findedControl;
        }


        public void SetAllTimePicker(Control parent, string beginControlName, string endControlName, DayTimeGroup group)
        {
            for (int i = 0; i <= 3; i++)
            {
                DateTimePicker beginTimePicker = FindControl(parent, beginControlName + (i + 1).ToString()) as DateTimePicker;
                DateTimePicker endTimePicker = FindControl(parent, endControlName + (i + 1).ToString()) as DateTimePicker;
                if (group != null)
                {
                    TimeSegment segment = group.GetItem(i);
                    if (segment.GetBeginTime() != DateTime.MinValue)
                    {
                        Invoke(() =>
                        {
                            beginTimePicker.Value = segment.GetBeginTime();
                            endTimePicker.Value = segment.GetEndTime();
                        });

                    }
                    else
                    {
                        Invoke(() =>
                        {
                            beginTimePicker.Value = new DateTime(2000, 1, 1, 0, 0, 0);
                            endTimePicker.Value = new DateTime(2000, 1, 1, 0, 0, 0);
                        });
                    }
                }


            }

        }


        public void SetWeekTimeGroupValue(WeekTimeGroup tg, int weekDayIndex, int timeSegmentIndex, int type, DateTime dateTime)
        {
            TimeSegment ts = tg.GetItem(weekDayIndex).GetItem(timeSegmentIndex);
            if (type == 1)
            {
                ts.SetBeginTime(dateTime.Hour, dateTime.Minute);
            }
            else
            {
                ts.SetEndTime(dateTime.Hour, dateTime.Minute);
            }
        }
    }
}
