using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Protocol.Door.Door8800.TimeGroup;
using DoNetDrive.Protocol.Elevator.FC8864.TimeGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.Elevator.Test
{
    public partial class frmTimeGroup : frmNodeForm
    {
        string[] WeekdayList = new string[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };
        private static object lockobj = new object();
        private static frmTimeGroup onlyObj;

        /// <summary>
        /// 64个
        /// </summary>
        List<WeekTimeGroup> ListWeekTimeGroup = new List<WeekTimeGroup>();
        public static frmTimeGroup GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmTimeGroup(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }


        private frmTimeGroup(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }

        private void frmTimeGroup_Load(object sender, EventArgs e)
        {
            InitTimeGroup();
            InitWeekday();
        }

        private void InitWeekday()
        {
            cbWeekday.Items.Clear();
            cbWeekday.Items.AddRange(WeekdayList);
            cbWeekday.SelectedIndex = 0;
        }

        #region 开门时段
        public void InitTimeGroup()
        {
            string[] time = new string[64];
            for (int i = 0; i < 64; i++)
            {
                time[i] = "开门时段" + (i + 1).ToString();
            }
            cbTimeGroup.Items.Clear();
            cbTimeGroup.Items.AddRange(time);
            cbTimeGroup.SelectedIndex = 0;

        }
        #endregion

        private void FrmTimeGroup_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }

        /// <summary>
        /// 采集开门时段 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReadTimeGroup_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            FC8864.TimeGroup.ReadTimeGroup cmd = new FC8864.TimeGroup.ReadTimeGroup(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.TimeGroup.ReadTimeGroup_Result;
                ListWeekTimeGroup = result.ListWeekTimeGroup;


                BindTimeSegment();
                //dataGridView1
                StringBuilder sb = new StringBuilder($"已读取到数量：{result.Count}，");
                for (int i = 0; i < ListWeekTimeGroup.Count; i++)
                {
                    sb.Append($"开门时段【{(i+1).ToString()}】，");
                    for (int j = 0; j < 7; j++)
                    {
                        DayTimeGroup dtg = ListWeekTimeGroup[i].GetItem(j);
                        sb.Append($"星期【{(j + 1).ToString()}】-");
                        int segmentCount = dtg.GetSegmentCount();
                        for (int k = 0; k < segmentCount; k++)
                        {
                            TimeSegment ts = dtg.GetItem(k);
                            sb.Append($"时段{(k+1).ToString()}：{ts.toString()}。");
                        }
                       
                    }
                }
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSegmentIndex"></param>
        /// <param name="type"></param>
        /// <param name="dateTime"></param>
        private void SetWeekTimeGroupValue(int timeSegmentIndex,int type, DateTime dateTime)
        {
            if (ListWeekTimeGroup.Count > cbTimeGroup.SelectedIndex)
            {
                TimeSegment ts = ListWeekTimeGroup[cbTimeGroup.SelectedIndex].GetItem(cbWeekday.SelectedIndex).GetItem(timeSegmentIndex);
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


        private void BindTimeSegment()
        {
            DateTime nw = DateTime.Now;
            WeekTimeGroup tg = ListWeekTimeGroup[0];
            var day = tg.GetItem(0);
            //var tz = day.GetItem(0) as TimeSegment;
            SetAllTimePicker(groupBox1, "beginTimePicker", "endTimePicker", day);
            
        }

        /// <summary>
        /// 上传所有开门时段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddTimeGroup_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            FC8864.TimeGroup.AddTimeGroup_Parameter par = new FC8864.TimeGroup.AddTimeGroup_Parameter(ListWeekTimeGroup);
            FC8864.TimeGroup.AddTimeGroup cmd = new FC8864.TimeGroup.AddTimeGroup(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");

            };
        }

        /// <summary>
        /// 清空所有开门时段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearTimeGroup_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            FC8864.TimeGroup.ClearTimeGroup cmd = new FC8864.TimeGroup.ClearTimeGroup(cmdDtl);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");

            };
        }

        /// <summary>
        /// 选择开门时段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbTimeGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListWeekTimeGroup.Count > 0)
            {
                WeekTimeGroup tg = ListWeekTimeGroup[cbTimeGroup.SelectedIndex];
                var day = tg.GetItem(0);
                SetAllTimePicker(groupBox1, "beginTimePicker", "endTimePicker", day);
            }
            
        }

        private void CbWeekday_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListWeekTimeGroup.Count > 0)
            {
                WeekTimeGroup tg = ListWeekTimeGroup[cbTimeGroup.SelectedIndex];
                var day = tg.GetItem(cbWeekday.SelectedIndex);
                SetAllTimePicker(groupBox1, "beginTimePicker", "endTimePicker", day);
            }
            
        }

        private void BeginTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(0, 1, beginTimePicker1.Value);
        }

        private void EndTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(0, 2, endTimePicker1.Value);
        }

        private void BeginTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(1, 1, beginTimePicker2.Value);
        }

        private void EndTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(1, 2, endTimePicker2.Value);
        }

        private void BeginTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(2, 1, beginTimePicker3.Value);
        }

        private void EndTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(2, 2, endTimePicker3.Value);
        }

        private void BeginTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(3, 1, beginTimePicker4.Value);
        }

        private void EndTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(3, 2, endTimePicker4.Value);
        }

        private void BeginTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(4, 1, beginTimePicker5.Value);
        }

        private void EndTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(4, 2, endTimePicker5.Value);
        }

        private void BeginTimePicker6_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(5, 1, beginTimePicker6.Value);
        }

        private void EndTimePicker6_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(5, 2, endTimePicker6.Value);
        }

        private void BeginTimePicker7_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(6, 1, beginTimePicker7.Value);
        }

        private void EndTimePicker7_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(6, 2, endTimePicker7.Value);
        }

        private void BeginTimePicker8_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(7, 1, beginTimePicker8.Value);
        }

        private void EndTimePicker8_ValueChanged(object sender, EventArgs e)
        {
            SetWeekTimeGroupValue(7, 2, endTimePicker8.Value);
        }

        private void BtnFillNowTime_Click(object sender, EventArgs e)
        {
            ListWeekTimeGroup.Clear();
            
            //开门时段
            for (int x = 0; x < 64; x++)
            {
                WeekTimeGroup weekTimeGroup = new WeekTimeGroup(8);
                //weekTimeGroup.mDay
                //星期一 至 星期日
                for (int y = 0; y < 7; y++)
                {
                    DayTimeGroup dayTimeGroup = weekTimeGroup.GetItem(y);
                    //每天时段
                    for (int i = 0; i < 8; i++)
                    {
                        DateTime dt = DateTime.Now;
                        //dt = dt.AddMinutes(-1);
                        TimeSegment  segment = dayTimeGroup.GetItem(i);
                        dt = dt.AddMinutes(i + 1);
                        segment.SetBeginTime(dt.Hour, dt.Minute);
                        dt = dt.AddMinutes(i + 1);
                        segment.SetEndTime(dt.Hour, dt.Minute);
                        DateTimePicker beginTimePicker = FindControl(groupBox1, "beginTimePicker" + (i + 1).ToString()) as DateTimePicker;
                        DateTimePicker endTimePicker = FindControl(groupBox1, "endTimePicker" + (i + 1).ToString()) as DateTimePicker;
                        beginTimePicker.Value = segment.GetBeginTime();
                        endTimePicker.Value = segment.GetEndTime();
                    }
                }
                
                ListWeekTimeGroup.Add(weekTimeGroup);
            }
            
        }
    }
}
