using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.Reservation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetDrive.Protocol.POS.Test
{
    public partial class FrmReservation : frmNodeForm
    {
        private static object lockobj = new object();
        private static FrmReservation onlyObj;
        public static FrmReservation GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new FrmReservation(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private FrmReservation(INMain main) : base(main)
        {
            InitializeComponent();
        }

        private BindingList<ReservationDetail> ListReservationDetail = new BindingList<ReservationDetail>();

        /// <summary>
        /// 卡号字典
        /// </summary>
        HashSet<int> CardHashTable = null;

        private void FrmReservation_Load(object sender, EventArgs e)
        {
            CardHashTable = new HashSet<int>();
            dgvReservation.AutoGenerateColumns = false;
        }

        private void butReadDataBase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadDatabaseDetail cmd = new ReadDatabaseDetail(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadDatabaseDetail_Result result = cmde.Command.getResult() as ReadDatabaseDetail_Result;
                string tip = $"订餐信息--最大容量：{result.SortSize},已存数量：{result.UseSize}";

                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butReadAllMenu_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadDataBase cmd = new ReadDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadDataBase_Result result = cmde.Command.getResult() as ReadDataBase_Result;
                string tip = $"";
                Invoke(() =>
                {
                    dgvReservation.AutoGenerateColumns = false;
                    dgvReservation.DataSource = new BindingList<ReservationDetail>(result.ReservationDetails);
                    //dgvPrintContent.DataSource = result.PrintContents;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butClearDataBase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            cmdDtl.Timeout = 4000;
            CardHashTable.Clear();
            ClearDataBase cmd = new ClearDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        private void butAddToList_Click(object sender, EventArgs e)
        {
            ReservationDetail dto = new ReservationDetail();
            dto.CardData = (int)txtCardData.Value;
            dto.ReservationDate = dtpReservationDate.Value;
            CheckBox[] TGList = { cbTimeGroup1, cbTimeGroup2, cbTimeGroup3, cbTimeGroup4, cbTimeGroup5, cbTimeGroup6, cbTimeGroup7, cbTimeGroup8 };
            for (int i = 1; i <= 8; i++)
            {

                dto.SetTimeGroup(i, TGList[i - 1].Checked);

            }
            //dto.TimeGroup = print;
            ListReservationDetail.Add(dto);

            Invoke(() =>
            {
                dgvReservation.DataSource = new BindingList<ReservationDetail>(ListReservationDetail);
            });
        }

        private void butAddToDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            List<ReservationDetail> _list = new List<ReservationDetail>();
            ReservationDetail dto = new ReservationDetail();
            dto.CardData = (int)txtCardData.Value;
            dto.ReservationDate = dtpReservationDate.Value;
            CheckBox[] TGList = { cbTimeGroup1, cbTimeGroup2, cbTimeGroup3, cbTimeGroup4, cbTimeGroup5, cbTimeGroup6, cbTimeGroup7, cbTimeGroup8 };
            for (int i = 1; i <= 8; i++)
            {
                dto.SetTimeGroup(i, TGList[i - 1].Checked);
            }
            _list.Add(dto);
            AddReservationDetail_Parameter par = new AddReservationDetail_Parameter(_list);

            AddReservationDetail cmd = new AddReservationDetail(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };
        }

        private void butAddAll_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<ReservationDetail> _list = new List<ReservationDetail>();
            for (int i = 0; i < ListReservationDetail.Count; i++)
            {
                ReservationDetail dto = new ReservationDetail() { };
                dto.CardData = ListReservationDetail[i].CardData;
                dto.ReservationDate = ListReservationDetail[i].ReservationDate;
                dto.TimeGroup = ListReservationDetail[i].TimeGroup;
                _list.Add(dto);
            }
            if (_list.Count == 0)
            {
                return;
            }
            AddReservationDetail_Parameter par = new AddReservationDetail_Parameter(_list);

            AddReservationDetail cmd = new AddReservationDetail(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };
        }


        private void butDeleteFromList_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvReservation.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvReservation.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dgvReservation.Rows[i].Cells[1];
                    var item = ListReservationDetail.FirstOrDefault(t => t.CardData == byte.Parse(text.Value.ToString()));
                    ListReservationDetail.Remove(item);
                }
            }
            dgvReservation.DataSource = new BindingList<ReservationDetail>(ListReservationDetail);
        }

        private void butCreateByRandom_Click(object sender, EventArgs e)
        {
            int iCreateCount = CheckCreateCardCount();
            if (iCreateCount <= 0) return;

            //string sBeginNum = frmInputBox.ShowBox("起始序号", "请输入卡号的起始序号，取值范围：1-4000000000", "1", 10);
            byte iBeginNum = Convert.ToByte(txtStartCode.Value);
            //if (!UInt64.TryParse(.ToString(), out iBeginNum))
            //{
            //    return;
            //}
            if (iBeginNum == 0) iBeginNum = 1;


            ListReservationDetail.RaiseListChangedEvents = false;
            ReservationDetail detail;
            while (iCreateCount > 0)
            {
                detail = CreateNewDetail(iBeginNum++);
                if (detail != null)
                {
                    detail.ReservationDate = DateTime.Now;
                    detail.TimeGroup = 255;
                    AddCardBaseToList(detail);

                    iCreateCount--;
                }

            }
            ListReservationDetail.RaiseListChangedEvents = true;
            ListReservationDetail.ResetBindings();
            dgvReservation.DataSource = ListReservationDetail;
        }


        private bool AddCardBaseToList(ReservationDetail dto)
        {
            if (!CardHashTable.Contains(dto.CardData))
            {
                ListReservationDetail.Add(dto);
                CardHashTable.Add(dto.CardData);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查待创建的菜单数量
        /// </summary>
        /// <returns></returns>
        private int CheckCreateCardCount()
        {
            int iCreateCount = 0;
            if (!int.TryParse(txtCreateCount.Text, out iCreateCount))
            {
                MessageBox.Show("输入的数字不正确，取值范围：1-255！");
                return 0;
            }
            if (iCreateCount > 255)
            {
                MessageBox.Show("输入的数字不正确，取值范围：1-255！");
                return 0;
            }
            if ((iCreateCount + ListReservationDetail.Count) > 255)
            {
                iCreateCount = 255 - ListReservationDetail.Count;

            }
            if (iCreateCount <= 0) return 0;

            return iCreateCount;
        }

        /// <summary>
        /// 创建一个不重复的卡
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="iCardNum"></param>
        /// <returns></returns>
        private ReservationDetail CreateNewDetail(int card)
        {

            //检查卡片是否重复
            if (CardHashTable.Contains(card))
            {
                if (card == 0)
                {
                    //有重复
                    return CreateNewDetail(0);
                }
                else
                {
                    return null;
                }

            }
            ReservationDetail dto = new ReservationDetail();
            dto.CardData = card;
            return dto;
        }


        private void dgvReservation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex, row = e.RowIndex;
            if (row < 0) return;

            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvReservation.Rows[e.RowIndex].Cells[e.ColumnIndex];
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

            var gdRow = dgvReservation.Rows[row];
            var detail = gdRow.DataBoundItem as ReservationDetail;
            //StringBuilder strBuf = new StringBuilder();

            //DebugCardDetail(CardUI.CardDetail, strBuf);
            //txtDebug.Text = strBuf.ToString();
            DetailToControl(detail);
        }

        /// <summary>
        /// 将卡片输出到控件中
        /// </summary>
        /// <param name="card"></param>
        private void DetailToControl(ReservationDetail detail)
        {
            txtCardData.Value = detail.CardData;
            dtpReservationDate.Value = detail.ReservationDate;
            CheckBox[] TGList = { cbTimeGroup1, cbTimeGroup2, cbTimeGroup3, cbTimeGroup4, cbTimeGroup5, cbTimeGroup6, cbTimeGroup7, cbTimeGroup8 };
            for (int i = 1; i <= 8; i++)
            {
                TGList[i - 1].Checked = detail.GetTimeGroup(i);
            }
        }

        private void butClearList_Click(object sender, EventArgs e)
        {
            ListReservationDetail.Clear();
            dgvReservation.DataSource = ListReservationDetail;
            CardHashTable.Clear();
        }
    }
}
