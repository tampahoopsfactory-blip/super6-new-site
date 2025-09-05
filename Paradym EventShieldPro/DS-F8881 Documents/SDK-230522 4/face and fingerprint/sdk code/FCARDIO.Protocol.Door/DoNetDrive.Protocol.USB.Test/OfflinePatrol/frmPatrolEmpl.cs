using DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.ClearPatrolEmplDataBase;
using DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.DeletePatrolEmpl;
using DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDatabase;
using DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDatabaseDetail;
using DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDetail;
using DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.WritePatrolEmpl;
using DoNetDrive.Protocol.USB.OfflinePatrol.Test.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Test
{
    public partial class frmPatrolEmpl : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static frmPatrolEmpl onlyObj;

        private List<PatrolEmplUI> PatrolEmplList;

        /// <summary>
        /// 卡号字典
        /// </summary>
        HashSet<uint> CardHashTable = null;

        /// <summary>
        /// 工号字典
        /// </summary>
        HashSet<ushort> CodeHashTable = null;
        public static frmPatrolEmpl GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmPatrolEmpl(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }
        #endregion
        private void FrmPatrolEmpl_Load(object sender, EventArgs e)
        {
            CardHashTable = new HashSet<uint>();
            CodeHashTable = new HashSet<ushort>();
            PatrolEmplList = new List<PatrolEmplUI>();
            LoadUILanguage();
        }

        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            GetLanguage(butReadDatabaseDetail);
            GetLanguage(butReadDatabase);
            GetLanguage(butClearDataBase);
            GetLanguage(btnWriteEmpl);
            GetLanguage(butClearGrid);
            GetLanguage(label1);
            GetLanguage(dataGridView1);
            GetLanguage(chkSelectAll);
            GetLanguage(label2);
            GetLanguage(label3);
            GetLanguage(label4);
            GetLanguage(btnAddDevice);
            GetLanguage(btnCheckCardData);
            GetLanguage(btnCheckPCode);
            GetLanguage(btnDeleteDevice);
            GetLanguage(groupBox1);
            GetLanguage(label5);
            GetLanguage(butCreateCardNumByRandom);
            GetLanguage(butCreateCardNumByOrder);
        }

        public frmPatrolEmpl(INMain main) : base(main)
        {
            InitializeComponent();
        }

        private void ButReadDatabaseDetail_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadPatrolEmplDatabaseDetail cmd = new ReadPatrolEmplDatabaseDetail(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                PatrolEmplDatabaseDetail_Result result = cmde.Command.getResult() as PatrolEmplDatabaseDetail_Result;
                mMainForm.AddCmdLog(cmde, $"{GetLanguage("Txt1")}：{result.DataBaseSize}，{GetLanguage("Txt2")}：{result.PatrolEmplSize}");
            };
        }

        private void ButReadDatabase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadPatrolEmplDatabase cmd = new ReadPatrolEmplDatabase(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadPatrolEmplDatabase_Result result = cmde.Command.getResult() as ReadPatrolEmplDatabase_Result;
                PatrolEmplList.Clear();
                var list = result.PatrolEmplList;
                int index = 1;
                foreach (var item in list)
                {

                    PatrolEmplUI model = new PatrolEmplUI(item);
                    model.PCode = item.PCode;
                    model.Name = item.Name;
                    model.Index = index.ToString();
                    PatrolEmplList.Add(model);
                    index++;
                }
                Invoke(() =>
                {
                    dataGridView1.DataSource = new BindingList<PatrolEmplUI>(PatrolEmplList);

                });
                //dataGridView1
                string log = $"{GetLanguage("Txt3")}：{list.Count} ";
                mMainForm.AddCmdLog(cmde, log);
            };
        }

        private void ButClearDataBase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ClearPatrolEmplDataBase cmd = new ClearPatrolEmplDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);
        }

        /// <summary>
        /// 上传名单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWriteEmpl_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            List<Data.PatrolEmpl> _list = new List<Data.PatrolEmpl>();
            for (int i = 0; i < PatrolEmplList.Count; i++)
            {
                Data.PatrolEmpl patrolEmpl = new Data.PatrolEmpl();
                patrolEmpl = PatrolEmplList[i].PatrolEmpl;
                patrolEmpl.Name = PatrolEmplList[i].Name;
                _list.Add(patrolEmpl);
            }
            WritePatrolEmpl_Parameter par = new WritePatrolEmpl_Parameter(_list);
            WritePatrolEmpl cmd = new WritePatrolEmpl(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                PatrolEmplList.Clear();


                Invoke(() =>
                {
                    dataGridView1.DataSource = new BindingList<PatrolEmplUI>(PatrolEmplList);

                });
                mMainForm.AddLog(GetLanguage("Txt4"));
            };

        }

        /// <summary>
        /// 清空列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButClearGrid_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.Rows.Count;
            PatrolEmplList.Clear();
            dataGridView1.DataSource = new BindingList<PatrolEmplUI>(PatrolEmplList);
        }

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
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
        private void BtnAddDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            uint uCardData = 0;
            ushort uPcode = 0;
            if (!uint.TryParse(txtCardData.Text, out uCardData))
            {
                MessageBox.Show(GetLanguage("Txt5"));
                return;
            }
            if (!ushort.TryParse(txtPCode.Text, out uPcode))
            {
                MessageBox.Show(GetLanguage("Txt6"));
                return;
            }
            List<Data.PatrolEmpl> _list = new List<Data.PatrolEmpl>();

            Data.PatrolEmpl patrolEmpl = new Data.PatrolEmpl();
            patrolEmpl.CardData = uCardData;
            patrolEmpl.PCode = uPcode;
            patrolEmpl.Name = txtName.Text;
            _list.Add(patrolEmpl);
            WritePatrolEmpl_Parameter par = new WritePatrolEmpl_Parameter(_list);
            WritePatrolEmpl cmd = new WritePatrolEmpl(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                Invoke(() =>
                {
                    dataGridView1.DataSource = new BindingList<PatrolEmplUI>(PatrolEmplList);

                });
                mMainForm.AddLog(GetLanguage("Txt4"));
            };
        }

        private void BtnDeleteDevice_Click(object sender, EventArgs e)
        {
            List<ushort> _list = new List<ushort>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[1];
                    var item = PatrolEmplList.FirstOrDefault(t => t.Index == text.Value.ToString());

                    _list.Add(item.PCode);
                    //ListPassword.Remove(item);
                }
            }
            if (_list.Count > 0)
            {
                var cmdDtl = mMainForm.GetCommandDetail();
                if (cmdDtl == null) return;

                DeletePatrolEmpl_Parameter par = new DeletePatrolEmpl_Parameter(_list);

                DeletePatrolEmpl cmd = new DeletePatrolEmpl(cmdDtl, par);
                mMainForm.AddCommand(cmd);
                cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
                {
                    mMainForm.AddLog(GetLanguage("Txt4"));

                };
            }
        }

        private void ButCreateCardNumByRandom_Click(object sender, EventArgs e)
        {
            int iCreateCount = CheckCreateCardCount();
            if (iCreateCount <= 0) return;

            for (int i = 0; i < iCreateCount; i++)
            {

                Data.PatrolEmpl empl = CreateNewPatrolEmpl(0);
                empl.PCode = Convert.ToUInt16(i + 1);
                AddPatrolEmplToList(empl);

            }
            dataGridView1.DataSource = new BindingList<PatrolEmplUI>(PatrolEmplList);
        }

        /// <summary>
        /// 将一个巡更人员详情添加到系统缓冲中
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        private bool AddPatrolEmplToList(Data.PatrolEmpl empl)
        {

            if (!CardHashTable.Contains(empl.CardData))
            {
                var ui = new PatrolEmplUI(empl);
                ui.Index = (PatrolEmplList.Count + 1).ToString();
                ui.Name = GetLanguage("Txt7") + empl.PCode;
                ui.PCode = empl.PCode;
                PatrolEmplList.Add(ui);
                CardHashTable.Add(empl.CardData);
                return true;
            }
            return false;
        }

        private Random mCardRnd = new Random();
        private Random mCodeRnd = new Random();
        private int mCardMax = 0xFFFFFF;
        private int mCardMin = 0x100000;

        /// <summary>
        /// 创建一个不重复的卡
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="iCardNum"></param>
        /// <returns></returns>
        private Data.PatrolEmpl CreateNewPatrolEmpl(uint iCardNum)
        {
            uint cardNum = 0;
            ushort code = 0;
            Data.PatrolEmpl empl;
            if (iCardNum == 0)
            {
                cardNum = (uint)(mCardRnd.Next(mCardMax) % (mCardMax - mCardMin + 1) + mCardMin);
                code = (ushort)mCodeRnd.Next(1, 999);
            }
            else
            {
                cardNum = iCardNum;
            }


            //检查卡片是否重复
            if (CardHashTable.Contains(cardNum))
            {
                if (iCardNum == 0)
                {
                    //有重复
                    return CreateNewPatrolEmpl(0);
                }
                else
                {
                    return null;
                }

            }


            empl = new Data.PatrolEmpl();

            empl.CardData = cardNum;
            return empl;
        }

        /// <summary>
        /// 检查待创建的卡号数量
        /// </summary>
        /// <returns></returns>
        private int CheckCreateCardCount()
        {
            int iCreateCount = 0;
            if (!int.TryParse(txtCount.Text, out iCreateCount))
            {
                MessageBox.Show($"{GetLanguage("Txt8")}：1-32000！");
                return 0;
            }
            if (iCreateCount > 32000)
            {
                MessageBox.Show($"{GetLanguage("Txt8")}：1-32000！");
                return 0;
            }
            if ((iCreateCount + PatrolEmplList.Count) > 32000)
            {
                iCreateCount = 32000 - PatrolEmplList.Count;

            }
            if (iCreateCount <= 0) return 0;

            return iCreateCount;
        }

        private void ButCreateCardNumByOrder_Click(object sender, EventArgs e)
        {

        }

        private void FrmPatrolEmpl_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }

        /// <summary>
        /// 点击表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                var dto = PatrolEmplList.FirstOrDefault(t => t.Index == text.Value.ToString());

                txtCardData.Text = dto.PatrolEmpl.CardData.ToString();
                txtPCode.Text = dto.PatrolEmpl.PCode.ToString();
                txtName.Text = dto.PatrolEmpl.Name;
            }
        }

        private void BtnCheckCardData_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            int carddata = 0;
            if (!int.TryParse(txtCardData.Text, out carddata))
            {
                MessageBox.Show(GetLanguage("Txt5"));
                return;
            }
            ReadPatrolEmplDetail_Parameter par = new ReadPatrolEmplDetail_Parameter(2, carddata);
            ReadPatrolEmplDetail cmd = new ReadPatrolEmplDetail(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadPatrolEmplDetail_Result result = cmde.Command.getResult() as ReadPatrolEmplDetail_Result;
                string log = GetLanguage("Txt10");
                if (result.PatrolEmpl.PCode != 0)
                {
                    log = $"{GetLanguage("Txt7")}：{ result.PatrolEmpl.PCode.ToString() }，{GetLanguage("Txt9")}：{ result.PatrolEmpl.CardData.ToString() }，姓名：{result.PatrolEmpl.Name}";
                    Invoke(() =>
                    {
                        txtCardData.Text = result.PatrolEmpl.CardData.ToString();
                        txtName.Text = result.PatrolEmpl.Name;
                        txtPCode.Text = result.PatrolEmpl.PCode.ToString();
                    });
                }

                mMainForm.AddCmdLog(cmde, log);
            };
        }

        private void BtnCheckPCode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            int code = 0;
            if (!int.TryParse(txtPCode.Text, out code))
            {
                MessageBox.Show(GetLanguage("Txt6"));
                return;
            }
            ReadPatrolEmplDetail_Parameter par = new ReadPatrolEmplDetail_Parameter(1, code);
            ReadPatrolEmplDetail cmd = new ReadPatrolEmplDetail(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadPatrolEmplDetail_Result result = cmde.Command.getResult() as ReadPatrolEmplDetail_Result;
                string log = GetLanguage("Txt10");
                if (result.PatrolEmpl.PCode != 0)
                {
                    log = $"{GetLanguage("Txt7")}：{ result.PatrolEmpl.PCode.ToString() }，{GetLanguage("Txt9")}：{ result.PatrolEmpl.CardData.ToString() }，姓名：{result.PatrolEmpl.Name}";
                    txtCardData.Text = result.PatrolEmpl.CardData.ToString();
                    txtName.Text = result.PatrolEmpl.Name;
                    txtPCode.Text = result.PatrolEmpl.PCode.ToString();
                }

                mMainForm.AddCmdLog(cmde, log);
            };
        }
    }
}
