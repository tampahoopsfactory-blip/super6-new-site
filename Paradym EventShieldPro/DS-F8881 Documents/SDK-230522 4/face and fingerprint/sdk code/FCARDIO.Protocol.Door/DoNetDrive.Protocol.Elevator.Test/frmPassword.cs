using DoNetDrive.Protocol.Elevator.FC8864.Password;
using DoNetDrive.Protocol.Elevator.Test.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.Elevator.Test
{
    public partial class frmPassword : frmNodeForm
    {
        private static object lockobj = new object();
        private static frmPassword onlyObj;

        private List<PasswordDto> ListPassword = new List<PasswordDto>();
        public static frmPassword GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmPassword(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }


        private frmPassword(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
            dataGridView1.AutoGenerateColumns = false;

            

            for (int i = 0; i < 64; i++)
            {
                DataGridViewCheckBoxColumn cbc = new DataGridViewCheckBoxColumn();
                cbc.HeaderText = (i+1).ToString().PadLeft(2,'0');
                cbc.Width = 40;
                cbc.DataPropertyName = "Door" + (i + 1).ToString();
                dataGridView1.Columns.Add(cbc);

            }
            DataGridViewCheckBoxColumn cbc0 = new DataGridViewCheckBoxColumn();
            cbc0.HeaderText = "代按";
            cbc0.Width = 60;
            dataGridView1.Columns.Add(cbc0);

            int index = 64;
            foreach (Control ctrl in plCheckBox.Controls)
            {
                ctrl.Name = "cbDoor" + (index).ToString();
                ctrl.Text = (index).ToString();
                index--;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPassword_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPassword_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }

        /// <summary>
        /// 读取密码库存储详情 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButReadPasswordDetail_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadPasswordDetail cmd = new ReadPasswordDetail(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Door.Door8800.Password.ReadPasswordDetail_Result;

                //dataGridView1
                string log = $"密码容量：{result.DataSize}，已存数量：{result.PasswordSize}";
                mMainForm.AddCmdLog(cmde, log);
            };
        }

        /// <summary>
        /// 从控制板读取所有密码表 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButReadAllPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadAllPassword cmd = new ReadAllPassword(cmdDtl);
            mMainForm.AddCommand(cmd);
            //mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ListPassword.Clear();
                var comdResult = cmde.Command.getResult() as ReadAllPassword_Result;
                int count = 0;
                ReadAllPassword_Result result = comdResult as ReadAllPassword_Result;
                foreach ( PasswordDetail detail in result.PasswordList)
                {
                    PasswordDto dto = new PasswordDto();
                    dto.DoorNumList = detail.DoorNumList;
                    dto.Password = detail.Password;
                    ListPassword.Add(dto);
                }
                count = result.PasswordList.Count;
               
                Invoke(() =>
                {
                    dataGridView1.Rows.Clear();
                    foreach (PasswordDetail detail in result.PasswordList)
                    {
                        int index =dataGridView1.Rows.Add();
                        dataGridView1.Rows[index].Cells[1].Value = detail.Password;
                        var list = detail.DoorNumList;
                        //dataGridView1.Rows[index].Cells[2].Value = list[64] == 1;
                        int j = 2;
                        foreach (byte item in detail.DoorNumList)
                        {
                            dataGridView1.Rows[index].Cells[j].Value = list[j -2] == 1;
                            j++;
                        }
                    }
                    //dataGridView1.DataSource = new BindingList<PasswordDto>(ListPassword);

                });
                //dataGridView1
                string log = $"已读取到数量：{count} ";
                mMainForm.AddCmdLog(cmde, log);
            };
        }

        /// <summary>
        /// 添加列表密码 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButAddPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            List<PasswordDetail> _list = new List<PasswordDetail>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                PasswordDetail password = new PasswordDetail();
                password.Password = ListPassword[i].Password;
                password.DoorNumList = ListPassword[i].DoorNumList;
                _list.Add(password);
            }
            Password_Parameter par = new Password_Parameter(_list);
            AddPassword cmd = new AddPassword(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ListPassword.Clear();
                var comdResult = cmde.Command.getResult() as ReadAllPassword_Result;
                int count = 0;
                if (comdResult != null && comdResult.PasswordList != null)
                {
                    foreach (PasswordDetail detail in comdResult.PasswordList)
                    {
                        PasswordDto dto = new PasswordDto();
                        dto.SetDoors(detail);
                        dto.Password = detail.Password;
                        ListPassword.Add(dto);
                    }
                    count = comdResult.PasswordList.Count;
                }
                

                Invoke(() =>
                {
                    //dataGridView1.DataSource = new BindingList<PasswordDto>(ListPassword);

                });
                mMainForm.AddLog($"命令成功：");
            };

        }

        /// <summary>
        /// 删除所有密码表 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButClearPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ClearPassword cmd = new ClearPassword(cmdDtl);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");

            };
        }

        /// <summary>
        /// 清空表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearList_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.Rows.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                //DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[1];
                //ListPassword.RemoveAt(ListPassword.FindIndex(t => t.Password == text.Value.ToString()));

                dataGridView1.Rows.RemoveAt(i);
            }
            //dataGridView1.DataSource = new BindingList<PasswordDto>(ListPassword);
        }

        /// <summary>
        /// 新增至列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButInsertList_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码");
                return;
            }
            PasswordDto dto = new PasswordDto();
         
            dto.Password = txtPassword.Text;
            byte[] list = new byte[65];
            list[64] = (byte)(cbDoor65.Checked ? 1 : 0);
            int index = 63;
            foreach (Control ctrl in plCheckBox.Controls)
            {
                CheckBox cb = ctrl as CheckBox;
                list[index] = (byte)(cb.Checked ? 1 : 0);
                index--;
            }

            dto.DoorNumList = list;
            ListPassword.Add(dto);

            index = dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[1].Value = dto.Password;
            //dataGridView1.Rows[index].Cells[2].Value = list[64] == 1;
            int j = 2;
            foreach (byte item in list)
            {
                dataGridView1.Rows[index].Cells[j].Value = list[j - 2] == 1;
                j++;
            }
        }

        /// <summary>
        /// 从列表删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButDelList_Click(object sender, EventArgs e)
        {
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 新增至设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码");
                return;
            }
            List<PasswordDetail> _list = new List<PasswordDetail>();
            PasswordDetail password = new PasswordDetail();
           
            password.Password = txtPassword.Text;
            string strDoor1 = "";
            password.Door = Convert.ToInt32(strDoor1, 2);
            _list.Add(password);
            Password_Parameter par = new Password_Parameter(_list);

            AddPassword cmd = new AddPassword(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };

        }

        /// <summary>
        /// 从设备删除单个密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelDevice_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码");
                return;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            List<PasswordDetail> _list = new List<PasswordDetail>();
            PasswordDetail detail = new PasswordDetail();
            detail.Password = txtPassword.Text;
            _list.Add(detail);
            DeletePassword_Parameter par = new DeletePassword_Parameter(_list);
            DeletePassword cmd = new DeletePassword(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");

            };
        }

        /// <summary>
        /// 从设备删除多个密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelSelect_Click(object sender, EventArgs e)
        {
            List<PasswordDetail> _list = new List<PasswordDetail>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[1];
                    //var item = ListPassword.FirstOrDefault(t => t.Password == text.Value.ToString());
                    PasswordDetail detail = new PasswordDetail();
                    detail.Password = text.Value.ToString();
                    //string strDoor1 = (item.Door1 ? "1" : "0") + (item.Door2 ? "1" : "0") + (item.Door3 ? "1" : "0") + (item.Door4 ? "1" : "0");
                    //detail.Door = Convert.ToInt32(strDoor1, 2);
                    _list.Add(detail);
                    //ListPassword.Remove(item);
                }
            }

            //dataGridView1.DataSource = new BindingList<PasswordDto>(ListPassword);
            if (_list.Count > 0)
            {
                var cmdDtl = mMainForm.GetCommandDetail();
                if (cmdDtl == null) return;

                DeletePassword_Parameter par = new DeletePassword_Parameter(_list);

                DeletePassword cmd = new DeletePassword(cmdDtl, par);
                mMainForm.AddCommand(cmd);
                cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
                {
                    mMainForm.AddLog($"命令成功：");

                };
            }
        }

        /// <summary>
        /// 生成测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRandom_Click(object sender, EventArgs e)
        {
            int iCount = 0;
            if (int.TryParse(txtCount.Text, out iCount))
            {
                ListPassword.Clear();
                Random rnd = new Random();
                Random rndDoor = new Random();
                Random rndTimes = new Random();
                ListPassword.Clear();
                for (int i = 0; i < iCount; i++)
                {
                    string password = rnd.Next(10000000, 99999999).ToString();
                    PasswordDto dto = new PasswordDto();
                    dto.Password = password;
                    if (ListPassword.FirstOrDefault(t => t.Password == password) != null)
                    {
                        password = rnd.Next(10000000, 99999999).ToString();
                    }
                    int door = rndDoor.Next(66);
                    dto.DoorNumList = new byte[65];
                    for (int z = 0; z < dto.DoorNumList.Length; z++)
                    {
                        dto.DoorNumList[z] = 1;
                    }
                    ListPassword.Add(dto);

                    int index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[1].Value = dto.Password;
                    //dataGridView1.Rows[index].Cells[2].Value = list[64] == 1;
                    int j = 2;
                    foreach (byte item in dto.DoorNumList)
                    {
                        dataGridView1.Rows[index].Cells[j].Value = true;
                        j++;
                    }
                }

                
            }
            else
            {
                MessageBox.Show("");
            }
        }

        private void SetDoors(string binary)
        {

        }

        private void CbReverse_CheckedChanged(object sender, EventArgs e)
        {
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
            else
            {
                DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dataGridView1.Rows[e.RowIndex].Cells[1];
                var dto = ListPassword.FirstOrDefault(t => t.Password == text.Value.ToString());
                txtPassword.Text = dto.Password;
                int index = 63;
                foreach (Control ctrl in plCheckBox.Controls)
                {
                    CheckBox cb = ctrl as CheckBox;
                    cb.Checked = dto.DoorNumList[index] == 1;
                    index--;
                }
                cbDoor65.Checked = dto.DoorNumList[64] == 1;
            }
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text.Length > 8)
            {

                MessageBox.Show("密码长度太长");
                txtPassword.Text = "";
            }
            int iOut = 0;
            if (!int.TryParse(txtPassword.Text, out iOut))
            {
                MessageBox.Show("密码格式不正确");
                txtPassword.Text = "";
            }
        }
    }
}
