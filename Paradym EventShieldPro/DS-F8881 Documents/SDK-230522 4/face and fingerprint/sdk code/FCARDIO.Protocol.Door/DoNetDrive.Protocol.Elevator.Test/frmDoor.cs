using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Protocol.Elevator.FC8864.Data;
using DoNetDrive.Protocol.Elevator.FC8864.Door;
using DoNetDrive.Protocol.Elevator.FC8864.Door.AutoLockedSetting;
using DoNetDrive.Protocol.Elevator.FC8864.Door.CancelDoorAlarm;
using DoNetDrive.Protocol.Elevator.FC8864.Door.CloseDoor;
using DoNetDrive.Protocol.Elevator.FC8864.Door.DoorKeepOpen;
using DoNetDrive.Protocol.Elevator.FC8864.Door.DoorWorkSetting;
using DoNetDrive.Protocol.Elevator.FC8864.Door.FirstCardOpen;
using DoNetDrive.Protocol.Elevator.FC8864.Door.GateMagneticAlarm;
using DoNetDrive.Protocol.Elevator.FC8864.Door.LockDoor;
using DoNetDrive.Protocol.Elevator.FC8864.Door.OpenDoor;
using DoNetDrive.Protocol.Elevator.FC8864.Door.OpenDoorTimeoutAlarm;
using DoNetDrive.Protocol.Elevator.FC8864.Door.OutDoorSwitch;
using DoNetDrive.Protocol.Elevator.FC8864.Door.Relay;
using DoNetDrive.Protocol.Elevator.FC8864.Door.UnLockDoor;
using DoNetDrive.Protocol.Elevator.FC8864.Door.UnlockingTime;
using DoNetDrive.Protocol.Elevator.FC8864.Utility;
using DoNetDrive.Protocol.Elevator.Test.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.Elevator.Test
{
    public partial class frmDoor : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static frmDoor onlyObj;

        WeekTimeGroup WeekTimeGroupDoorWorkDto;
        WeekTimeGroup WeekTimeGroupOutDoorDto;
        List<WeekTimeGroupDto> ListAutoLockedDto = new List<WeekTimeGroupDto>();
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
        List<DoorUI> ListDoorUI1;
        List<DoorUI> ListDoorUI2;
        List<DoorUI> ListDoorUI3;
        List<DoorUI> ListDoorUI4;


        List<string> ListOutputFormat = new List<string> { "", "不输出", "输出", "读卡切换输出状态" };
        string[] ListAlarmCode = new string[] { "解除门磁报警", "解除开门超时报警" };
        private frmDoor(INMain main) : base(main)
        {
            InitializeComponent();
            InitGridReaderWork();
            WeekTimeGroupDoorWorkDto = new WeekTimeGroup(8);
            WeekTimeGroupDoorWorkDto.InitTimeGroup();

            WeekTimeGroupOutDoorDto = new WeekTimeGroup(8);
            WeekTimeGroupOutDoorDto.InitTimeGroup();

            ListDoorUI1 = new List<DoorUI>(16);
            ListDoorUI2 = new List<DoorUI>(16);
            ListDoorUI3 = new List<DoorUI>(16);
            ListDoorUI4 = new List<DoorUI>(16);
            for (int i = 1; i < 65; i++)
            {
                if (ListDoorUI1.Count < 16)
                    ListDoorUI1.Add(new DoorUI() { Index = i, OutputFormat = "不输出" });
                else if (ListDoorUI2.Count < 16)
                    ListDoorUI2.Add(new DoorUI() { Index = i, OutputFormat = "不输出" });
                else if (ListDoorUI3.Count < 16)
                    ListDoorUI3.Add(new DoorUI() { Index = i, OutputFormat = "不输出" });
                else if (ListDoorUI4.Count < 16)
                    ListDoorUI4.Add(new DoorUI() { Index = i, OutputFormat = "不输出" });
                cmbDoorNum.Items.Add(i.ToString());
            }
            cmbDoorNum.Items.Add("65");
            cmbDoorNum.SelectedIndex = 0;

            dgvDoor1.AutoGenerateColumns = false;
            dgvDoor1.DataSource = new BindingList<DoorUI>(ListDoorUI1);

            dgvDoor2.AutoGenerateColumns = false;
            dgvDoor2.DataSource = new BindingList<DoorUI>(ListDoorUI2);

            dgvDoor3.AutoGenerateColumns = false;
            dgvDoor3.DataSource = new BindingList<DoorUI>(ListDoorUI3);

            dgvDoor4.AutoGenerateColumns = false;
            dgvDoor4.DataSource = new BindingList<DoorUI>(ListDoorUI4);

            cmbAlarmCode.Items.AddRange(ListAlarmCode);
            cmbAlarmCode.SelectedIndex = 0;
        }

        private void InitGridReaderWork()
        {
            Random r = new Random();
            for (int i = 0; i < 7; i++)
            {
                WeekTimeGroupDto dto = new WeekTimeGroupDto();
                dto.WeekDay = StringUtility.GetWeekStr(i);
                dto.Ex = "-";
                dto.IsEx = "true";
                //ListWeekTimeGroupDto.Add(dto);
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
                    //ListWeekTimeGroupDto.Add(dto);
                    ListAutoLockedDto.Add(dto);
                }
            }

            dgvLockTime.AutoGenerateColumns = false;
            dgvLockTime.DataSource = new BindingList<WeekTimeGroupDto>(ListAutoLockedDto);
        }
        #endregion

        private void FrmDoor_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }

        private void BtnReadRelay_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadRelay cmd = new ReadRelay(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadRelay_Result result = cmde.Command.getResult() as ReadRelay_Result;
                var list = result.OutputFormatList;
                ListDoorUI1.Clear();
                ListDoorUI2.Clear();
                ListDoorUI3.Clear();
                ListDoorUI4.Clear();
                StringBuilder sb = new StringBuilder("继电器：");
                for (int i = 0; i < list.Length - 1; i++)
                {
                    if (ListDoorUI1.Count < 16)
                        ListDoorUI1.Add(new DoorUI() { Index = i + 1, OutputFormat = ListOutputFormat[list[i]] });
                    else if (ListDoorUI2.Count < 16)
                        ListDoorUI2.Add(new DoorUI() { Index = i + 1, OutputFormat = ListOutputFormat[list[i]] });
                    else if (ListDoorUI3.Count < 16)
                        ListDoorUI3.Add(new DoorUI() { Index = i + 1, OutputFormat = ListOutputFormat[list[i]] });
                    else if (ListDoorUI4.Count < 16)
                        ListDoorUI4.Add(new DoorUI() { Index = i + 1, OutputFormat = ListOutputFormat[list[i]] });

                    sb.Append($"【{i + 1}】、{ListOutputFormat[list[i]]}，");
                }

                Invoke(() =>
                {
                    dgvDoor1.DataSource = new BindingList<DoorUI>(ListDoorUI1);

                    dgvDoor2.DataSource = new BindingList<DoorUI>(ListDoorUI2);

                    dgvDoor3.DataSource = new BindingList<DoorUI>(ListDoorUI3);

                    dgvDoor4.DataSource = new BindingList<DoorUI>(ListDoorUI4);
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void GetDgvDoorCheckBoxValue(byte[] list)
        {
            for (int i = 0; i < dgvDoor1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dgvDoor1.Rows[i].Cells[0];
                bool bSelected = Convert.ToBoolean(checkCell.Value);
                if (bSelected)
                {
                    list[i] = 1;
                }
            }
            for (int i = 0; i < dgvDoor2.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dgvDoor2.Rows[i].Cells[0];
                bool bSelected = Convert.ToBoolean(checkCell.Value);
                if (bSelected)
                {
                    list[i + 16] = 1;
                }
            }
            for (int i = 0; i < dgvDoor3.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dgvDoor3.Rows[i].Cells[0];
                bool bSelected = Convert.ToBoolean(checkCell.Value);
                if (bSelected)
                {
                    list[i + 32] = 1;
                }
            }
            for (int i = 0; i < dgvDoor4.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dgvDoor4.Rows[i].Cells[0];
                bool bSelected = Convert.ToBoolean(checkCell.Value);
                if (bSelected)
                {
                    list[i + 48] = 1;
                }
            }
        }

        private void BtnWriteRelay_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[65];
            for (int i = 0; i < dgvDoor1.Rows.Count; i++)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dgvDoor1.Rows[i].Cells[2];
                list[i] = (byte)ListOutputFormat.FindIndex(t => t == cell.Value.ToString());
            }
            for (int i = 0; i < dgvDoor2.Rows.Count; i++)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dgvDoor2.Rows[i].Cells[2];
                list[i + 16] = (byte)ListOutputFormat.FindIndex(t => t == cell.Value.ToString());
            }
            for (int i = 0; i < dgvDoor3.Rows.Count; i++)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dgvDoor3.Rows[i].Cells[2];
                list[i + 32] = (byte)ListOutputFormat.FindIndex(t => t == cell.Value.ToString());
            }
            for (int i = 0; i < dgvDoor4.Rows.Count; i++)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dgvDoor4.Rows[i].Cells[2];
                list[i + 48] = (byte)ListOutputFormat.FindIndex(t => t == cell.Value.ToString());
            }
            list[64] = 1;
            WriteRelay_Parameter par = new WriteRelay_Parameter(list);
            WriteRelay cmd = new WriteRelay(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void DgvReplay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridViewComboBoxColumn combobox = dgvDoor1.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
                if (combobox != null) //如果该列是TextBox列
                {
                    dgvDoor1.BeginEdit(true); //开始编辑状态
                    dgvDoor1.ReadOnly = false;
                }
            }
        }

        private void BtnWriteOpenDoor_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[72];
            GetDgvDoorCheckBoxValue(list);
            if (txtOpenDoorCode.Text != "")
            {
                WriteOpenDoor_Parameter par = new WriteOpenDoor_Parameter(list, byte.Parse(txtOpenDoorCode.Text));
                WriteOpenDoorWithCode cmd = new WriteOpenDoorWithCode(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }
            else
            {
                WriteOpenDoor_Parameter par = new WriteOpenDoor_Parameter(list);
                WriteOpenDoor cmd = new WriteOpenDoor(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void BtnWriteCloseDoor_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[72];
            GetDgvDoorCheckBoxValue(list);

            WriteCloseDoor_Parameter par = new WriteCloseDoor_Parameter(list);
            WriteCloseDoor cmd = new WriteCloseDoor(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }


        private void BtnWriteDoorKeepOpen_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[72];
            GetDgvDoorCheckBoxValue(list);

            WriteDoorKeepOpen_Parameter par = new WriteDoorKeepOpen_Parameter(list);
            WriteDoorKeepOpen cmd = new WriteDoorKeepOpen(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void DgvDoor1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor1.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            if (e.ColumnIndex >= 2)
            {
                dgvDoor1.BeginEdit(true); //开始编辑状态
                dgvDoor1.ReadOnly = false;

            }
        }

        private void DgvDoor2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor2.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            if (e.ColumnIndex >= 2)
            {
                dgvDoor2.BeginEdit(true); //开始编辑状态
                dgvDoor2.ReadOnly = false;

            }
        }

        private void DgvDoor3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor3.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            if (e.ColumnIndex >= 2)
            {
                dgvDoor3.BeginEdit(true); //开始编辑状态
                dgvDoor3.ReadOnly = false;

            }
        }

        private void DgvDoor4_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor4.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            if (e.ColumnIndex >= 2)
            {
                dgvDoor4.BeginEdit(true); //开始编辑状态
                dgvDoor4.ReadOnly = false;

            }
        }

        private void BtnWriteLockDoor_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[64];
            GetDgvDoorCheckBoxValue(list);

            WriteLockDoor_Parameter par = new WriteLockDoor_Parameter(list);
            WriteLockDoor cmd = new WriteLockDoor(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void BtnWriteUnLockDoor_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[64];
            GetDgvDoorCheckBoxValue(list);

            WriteUnLockDoor_Parameter par = new WriteUnLockDoor_Parameter(list);
            WriteUnLockDoor cmd = new WriteUnLockDoor(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }
        #region 端口工作方式
        private void RBtnNoDoorWorkSetting_CheckedChanged(object sender, EventArgs e)
        {
            OpenDoorWayPanel.Visible = false;
            DoorTriggerModePanel.Visible = false;
            DoorOpenTimePanel.Visible = false;
            if (cbxWeek.SelectedIndex == -1)
            {
                cbxWeek.SelectedIndex = 0;
            }
        }

        private void RBtnDoorWorkSetting_CheckedChanged(object sender, EventArgs e)
        {
            OpenDoorWayPanel.Visible = true;
            if (cbxWeek.SelectedIndex == -1)
            {
                cbxWeek.SelectedIndex = 0;
            }
        }

        private void RBtnOpenDoorWay3_CheckedChanged(object sender, EventArgs e)
        {
            DoorOpenTimePanel.Visible = true;
            DoorTriggerModePanel.Visible = false;
        }

        private void RBtnOpenDoorWay4_CheckedChanged(object sender, EventArgs e)
        {
            DoorTriggerModePanel.Visible = true;
            DoorOpenTimePanel.Visible = true;
        }

        private void CbxWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeekTimeGroupDoorWorkDto != null)
            {
                var day = WeekTimeGroupDoorWorkDto.GetItem(cbxWeek.SelectedIndex);
                SetAllTimePicker(DoorOpenTimePanel, "beginTimePicker", "endTimePicker", day);
            }
        }

        private void BtnReadWorkSetting_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmbDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadDoorWorkSetting cmd = new ReadDoorWorkSetting(cmdDtl, new DoorPort_Parameter(cmbDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadDoorWorkSetting_Result result = cmde.Command.getResult() as ReadDoorWorkSetting_Result;
                StringBuilder sb = new StringBuilder();
                string OpenDoorWayStr = string.Empty;
                string DoorTriggerModeStr = string.Empty;
                if (result.Use == 0)
                {
                    sb.AppendLine("门" + result.Door + "：是否启用：【0、不启用】");
                    mMainForm.AddCmdLog(cmde, sb.ToString());
                }
                else
                {
                    //开门方式
                    if (result.OpenDoorWay == 1)
                    {
                        OpenDoorWayStr = "【1、普通】";
                    }

                    else if (result.OpenDoorWay == 3)
                    {
                        OpenDoorWayStr = "【3、首卡】";
                    }
                    else if (result.OpenDoorWay == 4)
                    {
                        OpenDoorWayStr = "【4、常开】";
                    }
                    sb.Append("门" + result.Door + "：开门方式：" + OpenDoorWayStr + "，");
                    //常开触发模式
                    if (result.DoorTriggerMode == 1)
                    {
                        DoorTriggerModeStr = "【1、合法卡】";
                    }
                    else if (result.DoorTriggerMode == 2)
                    {
                        DoorTriggerModeStr = "【2、常开卡】";
                    }
                    else if (result.DoorTriggerMode == 3)
                    {
                        DoorTriggerModeStr = "【3、自动开关】";
                    }
                    if (result.OpenDoorWay == 4)
                    {
                        sb.Append("；常开触发模式：" + DoorTriggerModeStr);
                    }

                    //开门方式是首卡或者常开状态时
                    if (result.OpenDoorWay == 3 || result.OpenDoorWay == 4)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            sb.Append("\r\n");
                            sb.Append(StringUtility.GetWeekStr(i));
                            for (int j = 0; j < 8; j++)
                            {
                                sb.Append("  时段" + (j + 1) + "：" + StringUtility.TimeHourAndMinuteStr(result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime(), result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime()));
                            }
                            // mMainForm.AddCmdLog(null, sb.ToString());
                        }
                    }
                    mMainForm.AddCmdLog(cmde, sb.ToString());
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
                            WeekTimeGroupDoorWorkDto = result.weekTimeGroup;
                            SetAllTimePicker(DoorOpenTimePanel, "beginTimePicker", "endTimePicker", WeekTimeGroupDoorWorkDto.GetItem(0));
                        }
                    }
                });
            };
        }

        private void BtnWriteWorkSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte door = (byte)(cmbDoorNum.SelectedIndex + 1); //门
            byte openDoorWay = 1; //开门方式
            byte doorTriggerMode = 3; //门常开触发模式
            byte retainValue = 0; //保留值
            if (rBtnDoorWorkSetting.Checked)
            {

                if (rBtnOpenDoorWay3.Checked) //首卡
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

            }
            byte use = (byte)(rBtnDoorWorkSetting.Checked ? 1 : 0);
            WriteDoorWorkSetting_Parameter par = new WriteDoorWorkSetting_Parameter(door, use, openDoorWay, doorTriggerMode, retainValue, WeekTimeGroupDoorWorkDto);
            WriteDoorWorkSetting write = new WriteDoorWorkSetting(cmdDtl, par);
            mMainForm.AddCommand(write);

        }

        private void RBtnOpenDoorWay1_CheckedChanged(object sender, EventArgs e)
        {
            DoorOpenTimePanel.Visible = false;
            DoorTriggerModePanel.Visible = false;
        }


        private void BeginTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupDoorWorkDto, cbxWeek.SelectedIndex, int.Parse(dtp.Name.Substring(15)) - 1, 1, dtp.Value);
        }

        private void EndTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupDoorWorkDto, cbxWeek.SelectedIndex, int.Parse(dtp.Name.Substring(13)) - 1, 2, dtp.Value);
        }

        private void BtnFillNowTime_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < 7; y++)
            {
                DayTimeGroup dayTimeGroup = WeekTimeGroupDoorWorkDto.GetItem(y);
                //每天时段
                for (int i = 0; i < 8; i++)
                {
                    DateTime dt = DateTime.Now;
                    //dt = dt.AddMinutes(-1);
                    TimeSegment segment = dayTimeGroup.GetItem(i);
                    dt = dt.AddMinutes(i + 1);
                    segment.SetBeginTime(dt.Hour, dt.Minute);
                    dt = dt.AddMinutes(i + 1);
                    segment.SetEndTime(dt.Hour, dt.Minute);
                    DateTimePicker beginTimePicker = FindControl(DoorOpenTimePanel, "beginTimePicker" + (i + 1).ToString()) as DateTimePicker;
                    DateTimePicker endTimePicker = FindControl(DoorOpenTimePanel, "endTimePicker" + (i + 1).ToString()) as DateTimePicker;
                    beginTimePicker.Value = segment.GetBeginTime();
                    endTimePicker.Value = segment.GetEndTime();
                }
            }
        }

        #endregion

        #region 定时锁定端口
        private void BtnReadAutoLockedSetting_Click(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();
            byte door = (byte)(cmbDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAutoLockedSetting cmd = new ReadAutoLockedSetting(cmdDtl, new DoorPort_Parameter(door));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAutoLockedSetting_Result result = cmde.Command.getResult() as ReadAutoLockedSetting_Result;
                ListAutoLockedDto.Clear();
                for (int i = 0; i < 7; i++)
                {
                    WeekTimeGroupDto dto = new WeekTimeGroupDto();
                    dto.WeekDay = StringUtility.GetWeekStr(i);
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
                    dgvLockTime.DataSource = new BindingList<WeekTimeGroupDto>(ListAutoLockedDto);
                    rBtnNoAutoLockedSetting.Checked = result.Use;
                });

                string useStr = !result.Use ? "【0、不启用】" : "【1、启用】";
                string tip = "定时锁定门_门" + result.Door.ToString() + "：是否启用：" + useStr + "，时段详情：";
                sb.Append(tip);
                for (int i = 0; i < 7; i++)
                {
                    sb.Append("\r\n");
                    sb.Append(StringUtility.GetWeekStr(i));
                    for (int j = 0; j < 8; j++)
                    {
                        sb.Append("  时段" + (j + 1) + "：" + StringUtility.TimeHourAndMinuteStr(result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime(), result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime()));
                    }
                }
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteAutoLockedSetting_Click(object sender, EventArgs e)
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WeekTimeGroup tg = new WeekTimeGroup(8);
            ConvertDtoToModel(tg);


            byte door = (byte)(cmbDoorNum.SelectedIndex + 1);

            WriteAutoLockedSetting_Parameter par = new WriteAutoLockedSetting_Parameter(door, rBtnAutoLockedSetting.Checked, tg);
            WriteAutoLockedSetting write = new WriteAutoLockedSetting(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

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

        private void DgvLockTime_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            object ex = dgvLockTime.Rows[e.RowIndex].Cells["IsEx2"].Value;
            if (e.ColumnIndex == 0 && e.RowIndex % 8 != 0 && ex == null)
            {
                return;
            }
            if (e.ColumnIndex == 0)
            {
                string isEx = ex.ToString();
                //收缩
                if (this.dgvLockTime.Columns[e.ColumnIndex].Name == "EX2" && isEx == "true")
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        //隐藏行
                        this.dgvLockTime.Rows[e.RowIndex + i].Visible = false;
                    }
                    //将IsEx设置为false，标明该节点已经收缩
                    this.dgvLockTime.Rows[e.RowIndex].Cells["IsEx2"].Value = "false";
                    this.dgvLockTime.Rows[e.RowIndex].Cells["EX2"].Value = "+";
                }
                else if (this.dgvLockTime.Columns[e.ColumnIndex].Name == "EX2" && isEx == "false")
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        this.dgvLockTime.Rows[e.RowIndex + i].Visible = true;
                    }
                    this.dgvLockTime.Rows[e.RowIndex].Cells["IsEx2"].Value = "true";
                    this.dgvLockTime.Rows[e.RowIndex].Cells["EX2"].Value = "-";
                }
            }

            if (e.ColumnIndex >= 3 && e.ColumnIndex <= 4)
            {
                DataGridViewTextBoxColumn textbox = dgvLockTime.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
                if (textbox != null) //如果该列是TextBox列
                {
                    dgvLockTime.BeginEdit(true); //开始编辑状态
                    dgvLockTime.ReadOnly = false;
                }

            }
            else
            {
                dgvLockTime.BeginEdit(false); //开始编辑状态
                dgvLockTime.ReadOnly = true;
            }
        }
        #endregion

        private void BtnReadUnlockingTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadRelay cmd = new ReadRelay(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadRelay_Result result = cmde.Command.getResult() as ReadRelay_Result;
                var list = result.OutputFormatList;
                ListDoorUI1.Clear();
                ListDoorUI2.Clear();
                ListDoorUI3.Clear();
                ListDoorUI4.Clear();
                StringBuilder sb = new StringBuilder("开锁时输出时长：");
                for (int i = 0; i < list.Length - 1; i++)
                {
                    if (ListDoorUI1.Count < 16)
                        ListDoorUI1.Add(new DoorUI() { Index = i, OutputFormat = ListOutputFormat[list[i]] });
                    else if (ListDoorUI2.Count < 16)
                        ListDoorUI2.Add(new DoorUI() { Index = i, OutputFormat = ListOutputFormat[list[i]] });
                    else if (ListDoorUI3.Count < 16)
                        ListDoorUI3.Add(new DoorUI() { Index = i, OutputFormat = ListOutputFormat[list[i]] });
                    else if (ListDoorUI4.Count < 16)
                        ListDoorUI4.Add(new DoorUI() { Index = i, OutputFormat = ListOutputFormat[list[i]] });

                    sb.Append($"【{i + 1}】、{ListOutputFormat[list[i]]}，");
                }

                Invoke(() =>
                {
                    dgvDoor1.DataSource = new BindingList<DoorUI>(ListDoorUI1);

                    dgvDoor2.DataSource = new BindingList<DoorUI>(ListDoorUI2);

                    dgvDoor3.DataSource = new BindingList<DoorUI>(ListDoorUI3);

                    dgvDoor4.DataSource = new BindingList<DoorUI>(ListDoorUI4);
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void WriteUnlockingTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[65];
            for (int i = 0; i < dgvDoor1.Rows.Count; i++)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dgvDoor1.Rows[i].Cells[2];
                list[i] = (byte)ListOutputFormat.FindIndex(t => t == cell.Value.ToString());
            }
            for (int i = 0; i < dgvDoor2.Rows.Count; i++)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dgvDoor2.Rows[i].Cells[2];
                list[i + 16] = (byte)ListOutputFormat.FindIndex(t => t == cell.Value.ToString());
            }
            for (int i = 0; i < dgvDoor3.Rows.Count; i++)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dgvDoor3.Rows[i].Cells[2];
                list[i + 32] = (byte)ListOutputFormat.FindIndex(t => t == cell.Value.ToString());
            }
            for (int i = 0; i < dgvDoor4.Rows.Count; i++)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dgvDoor4.Rows[i].Cells[2];
                list[i + 48] = (byte)ListOutputFormat.FindIndex(t => t == cell.Value.ToString());
            }
            list[64] = 1;
            WriteRelay_Parameter par = new WriteRelay_Parameter(list);
            WriteRelay cmd = new WriteRelay(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void FrmDoor_Load(object sender, EventArgs e)
        {

        }

        private void BtnReadUnlockingTime_Click_1(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadUnlockingTime cmd = new ReadUnlockingTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadUnlockingTime_Result result = cmde.Command.getResult() as ReadUnlockingTime_Result;
                var list = result.TimeList;
                ListDoorUI1.Clear();
                ListDoorUI2.Clear();
                ListDoorUI3.Clear();
                ListDoorUI4.Clear();
                StringBuilder sb = new StringBuilder("开锁输出时长：");
                for (int i = 0; i < list.Length - 1; i++)
                {
                    string second = list[i].ToString();
                    if (second == "0")
                    {
                        second = "0.5";
                    }
                    if (ListDoorUI1.Count < 16)
                        ListDoorUI1.Add(new DoorUI() { Index = i + 1, Time = second });
                    else if (ListDoorUI2.Count < 16)
                        ListDoorUI2.Add(new DoorUI() { Index = i + 1, Time = second });
                    else if (ListDoorUI3.Count < 16)
                        ListDoorUI3.Add(new DoorUI() { Index = i + 1, Time = second });
                    else if (ListDoorUI4.Count < 16)
                        ListDoorUI4.Add(new DoorUI() { Index = i + 1, Time = second });
                    
                    sb.Append($"【{i + 1}】、{second}秒，");
                }

                Invoke(() =>
                {
                    dgvDoor1.DataSource = new BindingList<DoorUI>(ListDoorUI1);

                    dgvDoor2.DataSource = new BindingList<DoorUI>(ListDoorUI2);

                    dgvDoor3.DataSource = new BindingList<DoorUI>(ListDoorUI3);

                    dgvDoor4.DataSource = new BindingList<DoorUI>(ListDoorUI4);
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteUnlockingTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ushort[] list = new ushort[65];
            for (int i = 0; i < dgvDoor1.Rows.Count; i++)
            {
                DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)dgvDoor1.Rows[i].Cells[3];
                string value = cell.Value.ToString();
                list[i] = value == "0.5" ? (ushort)0 : ushort.Parse(value);
            }
            for (int i = 0; i < dgvDoor2.Rows.Count; i++)
            {
                DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)dgvDoor2.Rows[i].Cells[3];
                string value = cell.Value.ToString();
                list[i + 16] = value == "0.5" ? (ushort)0 : ushort.Parse(value);
            }
            for (int i = 0; i < dgvDoor3.Rows.Count; i++)
            {
                DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)dgvDoor3.Rows[i].Cells[3];
                string value = cell.Value.ToString();
                list[i + 32] = value == "0.5" ? (ushort)0 : ushort.Parse(value);
            }
            for (int i = 0; i < dgvDoor4.Rows.Count; i++)
            {
                DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)dgvDoor4.Rows[i].Cells[3];
                string value = cell.Value.ToString();
                list[i + 48] = value == "0.5" ? (ushort)0 : ushort.Parse(value);
            }
            list[64] = 65535;
            WriteUnlockingTime_Parameter par = new WriteUnlockingTime_Parameter(list);
            WriteUnlockingTime cmd = new WriteUnlockingTime(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void BtnFillNowTimeOutDoor_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < 7; y++)
            {
                DayTimeGroup dayTimeGroup = WeekTimeGroupOutDoorDto.GetItem(y);
                //每天时段
                for (int i = 0; i < 8; i++)
                {
                    DateTime dt = DateTime.Now;
                    //dt = dt.AddMinutes(-1);
                    TimeSegment segment = dayTimeGroup.GetItem(i);
                    dt = dt.AddMinutes(i + 1);
                    segment.SetBeginTime(dt.Hour, dt.Minute);
                    dt = dt.AddMinutes(i + 1);
                    segment.SetEndTime(dt.Hour, dt.Minute);
                    DateTimePicker beginTimePicker = FindControl(OutDoorTimePanel, "beginTPOutDoor" + (i + 1).ToString()) as DateTimePicker;
                    DateTimePicker endTimePicker = FindControl(OutDoorTimePanel, "EndTPOutDoor" + (i + 1).ToString()) as DateTimePicker;
                    beginTimePicker.Value = segment.GetBeginTime();
                    endTimePicker.Value = segment.GetEndTime();
                }
            }
        }

        private void BtnReadOutDoorSwitch_Click(object sender, EventArgs e)
        {
            byte door = (byte)(cmbDoorNum.SelectedIndex + 1);
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadOutDoorSwitch cmd = new ReadOutDoorSwitch(cmdDtl, new DoorPort_Parameter(cmbDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadOutDoorSwitch_Result result = cmde.Command.getResult() as ReadOutDoorSwitch_Result;
                StringBuilder sb = new StringBuilder();
                if (!result.Use)
                {
                    sb.AppendLine("门" + result.Door + "：是否启用：【0、不启用】");
                    mMainForm.AddCmdLog(cmde, sb.ToString());
                }
                else
                {
                    
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            sb.Append("\r\n");
                            sb.Append(StringUtility.GetWeekStr(i));
                            for (int j = 0; j < 8; j++)
                            {
                                sb.Append("  时段" + (j + 1) + "：" + StringUtility.TimeHourAndMinuteStr(result.weekTimeGroup.GetItem(i).GetItem(j).GetBeginTime(), result.weekTimeGroup.GetItem(i).GetItem(j).GetEndTime()));
                            }
                            // mMainForm.AddCmdLog(null, sb.ToString());
                        }
                    }
                    mMainForm.AddCmdLog(cmde, sb.ToString());
                }
                Invoke(() =>
                {
                  
                    rBtnOutDoor.Checked = result.Use;
                  
                });
                WeekTimeGroupOutDoorDto = result.weekTimeGroup;
                SetAllTimePicker(OutDoorTimePanel, "beginTPOutDoor", "EndTPOutDoor", WeekTimeGroupOutDoorDto.GetItem(0));
            };
        }

        private void BtnWriteOutDoorSwitch_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte door = (byte)(cmbDoorNum.SelectedIndex + 1); //门
            WriteOutDoorSwitch_Parameter par = new WriteOutDoorSwitch_Parameter(door, rBtnOutDoor.Checked, WeekTimeGroupOutDoorDto);
            WriteOutDoorSwitch write = new WriteOutDoorSwitch(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void BeginTPOutDoor_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupOutDoorDto, cbxWeekOutDoor.SelectedIndex, int.Parse(dtp.Name.Substring(14)) - 1, 1, dtp.Value);
        }

        private void EndTPOutDoor_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupOutDoorDto, cbxWeekOutDoor.SelectedIndex, int.Parse(dtp.Name.Substring(12)) - 1, 2, dtp.Value);
        }

        private void CbxWeekOutDoor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeekTimeGroupOutDoorDto != null)
            {
                var day = WeekTimeGroupOutDoorDto.GetItem(cbxWeekOutDoor.SelectedIndex);
                SetAllTimePicker(OutDoorTimePanel, "beginTPOutDoor", "EndTPOutDoor", day);
            }
        }

        private void RBtnOutDoor_CheckedChanged(object sender, EventArgs e)
        {
            OutDoorTimePanel.Visible = true;
            if (cbxWeekOutDoor.SelectedIndex == -1)
            {
                cbxWeekOutDoor.SelectedIndex = 0;
            }
        }

        private void RBtnNoOutDoor_CheckedChanged(object sender, EventArgs e)
        {
            OutDoorTimePanel.Visible = false;
            if (cbxWeekOutDoor.SelectedIndex == -1)
            {
                cbxWeekOutDoor.SelectedIndex = 0;
            }
        }

        private void BtnReadFirstCardOpen_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadFirstCardOpen cmd = new ReadFirstCardOpen(cmdDtl, new DoorPort_Parameter(cmbDoorNum.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadFirstCardOpen_Result result = cmde.Command.getResult() as ReadFirstCardOpen_Result;
                Invoke(() =>
                {

                    cbIsCardPassword.Checked = result.IsCardPassword;
                    cbIsPassword.Checked = result.IsPassword;
                });
                string str = "门" + result.Door + "";
                str += "，非首卡时段刷卡或卡加密码：" + (result.IsCardPassword ? "【1、允许通行】" : "【0、不允许通行】");
                str += "，非首卡时段输入密码：" + (result.IsCardPassword ? "【1、允许通行】" : "【0、不允许通行】");
               
                mMainForm.AddCmdLog(cmde, "首卡开门参数：" + str);
            };
        }

        private void BtnWriteFirstCardOpen_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteFirstCardOpen cmd = new WriteFirstCardOpen(cmdDtl, new WriteFirstCardOpen_Parameter(cmbDoorNum.SelectedIndex + 1,cbIsCardPassword.Checked,cbIsPassword.Checked));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadGateMagneticAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadGateMagneticAlarm cmd = new ReadGateMagneticAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadGateMagneticAlarm_Result result = cmde.Command.getResult() as ReadGateMagneticAlarm_Result;
                var list = result.DoorNumList;
                ListDoorUI1.Clear();
                ListDoorUI2.Clear();
                ListDoorUI3.Clear();
                ListDoorUI4.Clear();
                StringBuilder sb = new StringBuilder("门磁报警参数：");
                for (int i = 0; i < list.Length; i++)
                {
                    if (ListDoorUI1.Count < 16)
                        ListDoorUI1.Add(new DoorUI() { Index = i + 1, Selected = list[i] == 1 });
                    else if (ListDoorUI2.Count < 16)
                        ListDoorUI2.Add(new DoorUI() { Index = i + 1, Selected = list[i] == 1 });
                    else if (ListDoorUI3.Count < 16)
                        ListDoorUI3.Add(new DoorUI() { Index = i + 1, Selected = list[i] == 1 });
                    else if (ListDoorUI4.Count < 16)
                        ListDoorUI4.Add(new DoorUI() { Index = i + 1, Selected = list[i] == 1  });

                    sb.Append($"端口【{i + 1}】：{(list[i] == 1 ? "功能--启用" : "功能--禁用")}，");
                }

                Invoke(() =>
                {
                    dgvDoor1.DataSource = new BindingList<DoorUI>(ListDoorUI1);

                    dgvDoor2.DataSource = new BindingList<DoorUI>(ListDoorUI2);

                    dgvDoor3.DataSource = new BindingList<DoorUI>(ListDoorUI3);

                    dgvDoor4.DataSource = new BindingList<DoorUI>(ListDoorUI4);
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteGateMagneticAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[64];
            for (int i = 0; i < dgvDoor1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor1.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                    list[i] = 1;
                else
                    list[i] = 0;
            }
            for (int i = 0; i < dgvDoor2.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor2.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                    list[i + 16] = 1;
                else
                    list[i + 16] = 0;
            }
            for (int i = 0; i < dgvDoor3.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor3.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                    list[i + 32] = 1;
                else
                    list[i + 32] = 0;
            }
            for (int i = 0; i < dgvDoor4.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor4.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                    list[i + 48] = 1;
                else
                    list[i + 48] = 0;
            }
            WriteGateMagneticAlarm_Parameter par = new WriteGateMagneticAlarm_Parameter(list);
            WriteGateMagneticAlarm cmd = new WriteGateMagneticAlarm(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private void BtnReadOpenDoorTimeoutAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadOpenDoorTimeoutAlarm cmd = new ReadOpenDoorTimeoutAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadOpenDoorTimeoutAlarm_Result result = cmde.Command.getResult() as ReadOpenDoorTimeoutAlarm_Result;
                var list = result.OpenDoorTimeoutAlarmList;
                ListDoorUI1.Clear();
                ListDoorUI2.Clear();
                ListDoorUI3.Clear();
                ListDoorUI4.Clear();
                StringBuilder sb = new StringBuilder("开门超时报警参数：");
                for (int i = 0; i < list.Length; i++)
                {
                    if (ListDoorUI1.Count < 16)
                        ListDoorUI1.Add(new DoorUI() { Index = i + 1, Selected = list[i].IsUse, Time = list[i].AllowTime.ToString()  });
                    else if (ListDoorUI2.Count < 16)
                        ListDoorUI2.Add(new DoorUI() { Index = i + 1, Selected = list[i].IsUse, Time = list[i].AllowTime.ToString() });
                    else if (ListDoorUI3.Count < 16)
                        ListDoorUI3.Add(new DoorUI() { Index = i + 1, Selected = list[i].IsUse, Time = list[i].AllowTime.ToString() });
                    else if (ListDoorUI4.Count < 16)
                        ListDoorUI4.Add(new DoorUI() { Index = i + 1, Selected = list[i].IsUse, Time = list[i].AllowTime.ToString() });

                    sb.Append($"端口【{i + 1}】，允许开门的时间：{list[i].AllowTime}，{(list[i].IsUse ? "开启警铃输出" : "不开启警铃输出")}，");
                }

                Invoke(() =>
                {
                    dgvDoor1.DataSource = new BindingList<DoorUI>(ListDoorUI1);

                    dgvDoor2.DataSource = new BindingList<DoorUI>(ListDoorUI2);

                    dgvDoor3.DataSource = new BindingList<DoorUI>(ListDoorUI3);

                    dgvDoor4.DataSource = new BindingList<DoorUI>(ListDoorUI4);
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteOpenDoorTimeoutAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            OpenDoorTimeoutAlarm[] list = new OpenDoorTimeoutAlarm[64];
            for (int i = 0; i < dgvDoor1.Rows.Count; i++)
            {
                
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor1.Rows[i].Cells[0];
                DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dgvDoor1.Rows[i].Cells[3];
                OpenDoorTimeoutAlarm model = new OpenDoorTimeoutAlarm();
                model.IsUse = (bool)cell.FormattedValue;
                model.AllowTime = Convert.ToUInt16(text.Value);
                list[i] = model;
            }
            for (int i = 0; i < dgvDoor2.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor2.Rows[i].Cells[0];
                DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dgvDoor2.Rows[i].Cells[3];
                OpenDoorTimeoutAlarm model = new OpenDoorTimeoutAlarm();
                model.IsUse = (bool)cell.FormattedValue;
                model.AllowTime = Convert.ToUInt16(text.Value);
                list[i + 16] = model;
            }
            for (int i = 0; i < dgvDoor3.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor3.Rows[i].Cells[0];
                DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dgvDoor3.Rows[i].Cells[3];
                OpenDoorTimeoutAlarm model = new OpenDoorTimeoutAlarm();
                model.IsUse = (bool)cell.FormattedValue;
                model.AllowTime = Convert.ToUInt16(text.Value);
                list[i + 32] = model;
            }
            for (int i = 0; i < dgvDoor4.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvDoor4.Rows[i].Cells[0];
                DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dgvDoor4.Rows[i].Cells[3];
                OpenDoorTimeoutAlarm model = new OpenDoorTimeoutAlarm();
                model.IsUse = (bool)cell.FormattedValue;
                model.AllowTime = Convert.ToUInt16(text.Value);
                list[i + 48] = model;
            }
            WriteOpenDoorTimeoutAlarm_Parameter par = new WriteOpenDoorTimeoutAlarm_Parameter(list);
            WriteOpenDoorTimeoutAlarm cmd = new WriteOpenDoorTimeoutAlarm(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void BtnWriteCancelDoorAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteCancelDoorAlarm cmd = new WriteCancelDoorAlarm(cmdDtl, new WriteCancelDoorAlarm_Parameter(cmbDoorNum.SelectedIndex + 1, cmbAlarmCode.SelectedIndex + 1));
            mMainForm.AddCommand(cmd);
        }
    }
}
