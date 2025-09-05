using DoNetDrive.Protocol.Door.Test.Model;
using DoNetDrive.Protocol.Elevator.FC8864.Holiday;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.Elevator.Test
{
    public partial class frmHoliday : frmNodeForm
    {
        List<HolidayDetailDto> listHoliday = new List<HolidayDetailDto>();
        #region 单例模式

        private static object lockobj = new object();
        private static frmHoliday onlyObj;
        public static frmHoliday GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmHoliday(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }
        #endregion

        private frmHoliday(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }

        private void frmHoliday_Load(object sender, EventArgs e)
        {
            cbIndex.Items.Clear();
            for (int i = 1; i < 31; i++)
            {
                cbIndex.Items.Add(i.ToString());
            }
            cbIndex.SelectedIndex = 0;
            cbType.SelectedIndex = 0;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);


        }

        private void BindDataGrid()
        {
            
        }


        #region 从控制板中读取节假日存储详情
        private void butReadHolidayDetail_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadHolidayDetail cmd = new ReadHolidayDetail(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                DoNetDrive.Protocol.Door.Door8800.Holiday.ReadHolidayDetail_Result result = cmde.Command.getResult() as DoNetDrive.Protocol.Door.Door8800.Holiday. ReadHolidayDetail_Result;
                var dtl = result.Detail;
                string log = $"容量：{dtl.Capacity} ， 已用：{dtl.Count}";
                mMainForm.AddCmdLog(cmde, log);
            };
        }
        #endregion

        #region 清空节假日
        private void ClearHoliday_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ClearHoliday cmd = new ClearHoliday(cmdDtl);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");

            };
        }
        #endregion

        #region 读取控制板中所有的节假日
        private void ReadAllHoliday_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAllHoliday cmd = new ReadAllHoliday(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                DoNetDrive.Protocol.Door.Door8800.Holiday. ReadAllHoliday_Result result = cmde.Command.getResult() as DoNetDrive.Protocol.Door.Door8800.Holiday.ReadAllHoliday_Result;
                foreach (Door.Door8800.Holiday.HolidayDetail item in result.Holidays)
                {
                    HolidayDetailDto dto = new HolidayDetailDto();

                    dto.HolidayTypeRender = ConvertHolidayType(item.HolidayType);
                    dto.RepeatYear = item.Holiday.Year == 2000 ? "是" : "否";
                    if (item.Holiday.Year == 2000)
                    {
                        dto.Holiday = new DateTime(DateTime.Now.Year, item.Holiday.Month, item.Holiday.Day);
                    }
                    else
                    {
                        dto.Holiday = item.Holiday;
                    }
                    
                    dto.Index = item.Index;
                    dto.Selected = false;
                    listHoliday.Add(dto);
                    //superGridControl1.PrimaryGrid.Rows.Add(new GridRow(new object[] { item.Index, item.Holiday, ConvertHolidayType(item.HolidayType),item.Year }));
                }
                Invoke(() => {
                    dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);

                });
               
                //dataGridView1
                string log = $"已读取到数量：{result.Count} ";
                mMainForm.AddCmdLog(cmde, log);
            };


        }
        #endregion

        /// <summary>
        /// 节假日类型 转换
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private string ConvertHolidayType(byte b)
        {
            string result = "";
            switch (b)
            {
                case 1:
                    result = "【1、00点-11点59分 不能开】";
                    break;
                case 2:
                    result = "【2、12点-24点59分 不能开】";
                    break;
                case 3:
                    result = "【3、全天不能开】";
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmHoliday_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }

        /// <summary>
        /// 增加序号 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddIndex_Click(object sender, EventArgs e)
        {
            if (cbIndex.SelectedIndex != cbIndex.Items.Count - 1)
            {
                cbIndex.SelectedIndex++;
            }
            else
            {
                cbIndex.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 增加日期 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddDay_Click(object sender, EventArgs e)
        {
            dtpDay.Value = dtpDay.Value.AddDays(1);
        }

        /// <summary>
        /// 增加至列表 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddList_Click(object sender, EventArgs e)
        {
            byte bIndex = Convert.ToByte(cbIndex.SelectedIndex + 1);
            HolidayDetailDto holidayDto = listHoliday.FirstOrDefault(t => t.Index == bIndex);
            bool bExist = true;
            if (holidayDto == null)
            {
                holidayDto = new HolidayDetailDto() { };
                bExist = false;
            }
            holidayDto.Selected = false;
            holidayDto.Index = bIndex;
            holidayDto.Holiday = dtpDay.Value;
            holidayDto.HolidayTypeRender = ConvertHolidayType(Convert.ToByte(cbType.SelectedIndex + 1));
            holidayDto.RepeatYear = cbYear.Checked ? "是" : "否";
            if (!bExist)
            {
                listHoliday.Add(holidayDto);
            }
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);
        }

        /// <summary>
        /// 从列表删除 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelList_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.Rows.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                bool bSelected = Convert.ToBoolean(checkCell.Value);
                if (bSelected)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[1];
                    listHoliday.RemoveAt(listHoliday.FindIndex(t => t.Index == Convert.ToByte(text.Value)));
                }
            }
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);
        }

        /// <summary>
        /// 添加设备节假日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddDecive_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            Door.Door8800.Holiday.HolidayDetail holiday = new Door.Door8800.Holiday.HolidayDetail() { HolidayType = Convert.ToByte(cbType.SelectedIndex + 1), Index = Convert.ToByte(cbIndex.SelectedIndex + 1) };
            int year = dtpDay.Value.Year;
            if (cbYear.Checked)
            {
                year = 0;
            }
            holiday.Holiday = new DateTime(year, dtpDay.Value.Month, dtpDay.Value.Day);
            List<Door.Door8800.Holiday.HolidayDetail> _list = new List<Door.Door8800.Holiday.HolidayDetail>() { holiday };
            AddHoliday_Parameter par = new AddHoliday_Parameter(_list);
            AddHoliday cmd = new AddHoliday(cmdDtl, par);
            mMainForm.AddCommand(cmd);
            BtnAddList_Click(null, null);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
                
            };
        }

        /// <summary>
        /// 删除设备节假日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<Door.Door8800.Holiday.HolidayDetail> _list = new List<Door.Door8800.Holiday.HolidayDetail>();
            _list.Add(new Door.Door8800.Holiday.HolidayDetail() { Index = Convert.ToByte(cbIndex.SelectedIndex + 1) });
            DeleteHoliday_Parameter par = new DeleteHoliday_Parameter(_list);
            DeleteHoliday cmd = new DeleteHoliday(cmdDtl,par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };
        }

        private void BtnAdd30_Click(object sender, EventArgs e)
        {
            listHoliday.Clear();
            for (int i = 0; i < 30; i++)
            {
                HolidayDetailDto holiday = new HolidayDetailDto() { Index = Convert.ToByte(i + 1), Holiday = dtpDay.Value.AddDays(i + 1)};
                holiday.RepeatYear = cbYear.Checked ? "是" : "否";
                holiday.HolidayType = Convert.ToByte(cbType.SelectedIndex + 1);
                holiday.HolidayTypeRender = ConvertHolidayType(holiday.HolidayType);
                listHoliday.Add(holiday);
            }
            dtpDay.Value = dtpDay.Value.AddDays(30);
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);

            //dataGridView1.DataSource = listHoliday;
        }

        private void BtnDelSelect_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<Door.Door8800.Holiday.HolidayDetail> _list = new List<Door.Door8800.Holiday.HolidayDetail>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                bool bSelected = Convert.ToBoolean(checkCell.Value);
                if (bSelected)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[1];
                    byte bIndex = Convert.ToByte(text.Value);
                    _list.Add(new Door.Door8800.Holiday.HolidayDetail() { Index = bIndex });
                    listHoliday.RemoveAt(listHoliday.FindIndex(t => t.Index == bIndex));
                }
            }
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);

            DeleteHoliday_Parameter par = new DeleteHoliday_Parameter(_list);
            DeleteHoliday cmd = new DeleteHoliday(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };
        }

        /// <summary>
        /// 添加列表节假日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButAddHoliday_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<Door.Door8800.Holiday.HolidayDetail> _list = new List<Door.Door8800.Holiday.HolidayDetail>();
            for (int i = 0; i < listHoliday.Count; i++)
            {
                Door.Door8800.Holiday.HolidayDetail holiday = new Door.Door8800.Holiday.HolidayDetail() { Index = listHoliday[i].Index, HolidayType = listHoliday[i].HolidayType };
                holiday.Holiday = listHoliday[i].Holiday;
                if (cbYear.Checked)
                {
                    holiday.Holiday = new DateTime(2000, holiday.Holiday.Month, holiday.Holiday.Day);
                }
                
                _list.Add(holiday);
            }
            AddHoliday_Parameter par = new AddHoliday_Parameter(_list);
            AddHoliday cmd = new AddHoliday(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };
        }

        private void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if ((bool)cell.FormattedValue)
                {
                    cell.Value = false;
                    cell.EditingCellFormattedValue = false;
                }
                else
                {
                    cell.Value = true;
                    cell.EditingCellFormattedValue = true;
                }
            }
        }

        private void CheckBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            //checkBoxX1.Checked;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    cell.Value = false;
                    cell.EditingCellFormattedValue = false;
                }
                else
                {
                    cell.Value = true;
                    cell.EditingCellFormattedValue = true;
                }
            }
        }
    }
}
