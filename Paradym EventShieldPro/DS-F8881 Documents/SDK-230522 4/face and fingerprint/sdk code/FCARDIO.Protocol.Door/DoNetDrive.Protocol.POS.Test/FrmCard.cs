using DoNetDrive.Protocol.POS.Card;
using DoNetDrive.Protocol.POS.Data;
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
    public partial class FrmCard : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static FrmCard onlyObj;
        public static FrmCard GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new FrmCard(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private FrmCard(INMain main) : base(main)
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// 卡号字典
        /// </summary>
        HashSet<int> CardHashTable = null;

        private BindingList<CardDetail> ListCardDetail = new BindingList<CardDetail>();

        string[] mCardType = { "正常卡", "黑名单", "挂失卡" };

        private void FrmCard_Load(object sender, EventArgs e)
        {
            cmbCardType.Items.AddRange(mCardType);
            cmbCardType.SelectedIndex = 0;

            CardHashTable = new HashSet<int>();
            dgvCard.AutoGenerateColumns = false;
        }

        private void butReadDataBase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadCardDataBase cmd = new ReadCardDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCardDataBase_Result result = cmde.Command.getResult() as ReadCardDataBase_Result;
                string tip = $"卡片信息--最大容量：{result.SortSize},已存数量：{result.UseSize}";

                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butReadAllMenu_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAllCard cmd = new ReadAllCard(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAllCard_Result result = cmde.Command.getResult() as ReadAllCard_Result;
                string tip = $"";
                Invoke(() =>
                {
                    dgvCard.AutoGenerateColumns = false;
                    dgvCard.DataSource = new BindingList<CardDetail>(result.CardDetails);
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
            ClearCardDataBase cmd = new ClearCardDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        private void dgvCard_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex, row = e.RowIndex;
            if (row < 0) return;

            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvCard.Rows[e.RowIndex].Cells[e.ColumnIndex];
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

            var gdRow = dgvCard.Rows[row];
            var card = gdRow.DataBoundItem as CardDetail;
            //StringBuilder strBuf = new StringBuilder();

            //DebugCardDetail(CardUI.CardDetail, strBuf);
            //txtDebug.Text = strBuf.ToString();
            CardDetailToControl(card);
        }

        /// <summary>
        /// 将卡片输出到控件中
        /// </summary>
        /// <param name="card"></param>
        private void CardDetailToControl(CardDetail card)
        {
            txtCardData.Text = card.CardData.ToString();
            cmbCardType.SelectedIndex = card.CardType;
        }

        private void butClearList_Click(object sender, EventArgs e)
        {
            ListCardDetail.Clear();
            dgvCard.DataSource = ListCardDetail;
            CardHashTable.Clear();
        }

        private void butAddToList_Click(object sender, EventArgs e)
        {
            
            int card = 0;
            if (!int.TryParse(txtCardData.Text.Trim(), out card))
            {
                MessageBox.Show("卡号格式不正确");
                return;
            }

            CardDetail dto = new CardDetail();
            dto.CardData = card;
            dto.CardType = (byte)(cmbCardType.SelectedIndex);
          
            ListCardDetail.Add(dto);

            Invoke(() =>
            {
                dgvCard.DataSource = new BindingList<CardDetail>(ListCardDetail);
            });
        }

        private void butAddToDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            int cardData = 0;
            if (!int.TryParse(txtCardData.Text.Trim(), out cardData))
            {
                MessageBox.Show("卡号格式不正确");
                return;
            }

            List<CardDetail> _list = new List<CardDetail>();
            CardDetail card = new CardDetail();
            card.CardData = cardData;
            card.CardType = (byte)(cmbCardType.SelectedIndex);
            _list.Add(card);
            WriteCard_Parameter par = new WriteCard_Parameter(_list);

            AddCard cmd = new AddCard(cmdDtl, par);
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
            List<CardDetail> _list = new List<CardDetail>();
            for (int i = 0; i < ListCardDetail.Count; i++)
            {
                CardDetail card = new CardDetail() { };
                card.CardData = ListCardDetail[i].CardData;
                card.CardType = ListCardDetail[i].CardType;
                _list.Add(card);
            }
            if (_list.Count == 0)
            {
                return;
            }
            WriteCard_Parameter par = new WriteCard_Parameter(_list);
            AddCard cmd = new AddCard(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };
        }

        private void butReadMenu_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            int card = 0;
            if (!int.TryParse(txtCardData.Text, out card))
            {
                MessageBox.Show("卡号格式不正确");
                return;
            }
            var par = new ReadCardDetail_Parameter(card);
            var cmd = new ReadCardDetail(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadCardDetail_Result;

                if (!result.IsReady)
                {
                    mMainForm.AddCmdLog(cmde, $"卡号不存在");
                }
                else
                {
                    //PersonToControl(result.Person);
                    mMainForm.AddCmdLog(cmde, $"卡号存在");
                }
            };
        }

        private void butDeleteFromList_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvCard.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvCard.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dgvCard.Rows[i].Cells[1];
                    var item = ListCardDetail.FirstOrDefault(t => t.CardData == int.Parse(text.Value.ToString()));
                    ListCardDetail.Remove(item);
                }
            }
            dgvCard.DataSource = new BindingList<CardDetail>(ListCardDetail);
        }

        private void butDeleteFromDevice_Click(object sender, EventArgs e)
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<CardDetail> _list = new List<CardDetail>();
            for (int i = 0; i < dgvCard.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvCard.Rows[i].Cells[0];
                DataGridViewTextBoxCell cellContent = (DataGridViewTextBoxCell)dgvCard.Rows[i].Cells[1];
                if ((bool)cell.FormattedValue)
                {
                    _list.Add(new CardDetail() { CardData = Convert.ToInt32(cellContent.Value) });
                }
            }
            if (_list.Count == 0)
            {
                return;
            }


            WriteCard_Parameter par = new WriteCard_Parameter(_list);
            DeleteCard cmd = new DeleteCard(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };
        }

        private void butCreateByRandom_Click(object sender, EventArgs e)
        {
            int iCreateCount = CheckCreateCardCount();
            if (iCreateCount <= 0) return;

            //string sBeginNum = frmInputBox.ShowBox("起始序号", "请输入卡号的起始序号，取值范围：1-4000000000", "1", 10);
            int iBeginNum = Convert.ToInt32(txtStartCode.Value);
            //if (!UInt64.TryParse(.ToString(), out iBeginNum))
            //{
            //    return;
            //}
            if (iBeginNum == 0) iBeginNum = 1;


            ListCardDetail.RaiseListChangedEvents = false;
            CardDetail card;
            while (iCreateCount > 0)
            {
                card = CreateNewCardDetail(iBeginNum++);
                if (card != null)
                {
                    card.CardData = 1;
                    AddMenuBaseToList(card);

                    iCreateCount--;
                }

            }
            ListCardDetail.RaiseListChangedEvents = true;
            ListCardDetail.ResetBindings();
            dgvCard.DataSource = ListCardDetail;
        }

        private bool AddMenuBaseToList(CardDetail card)
        {
            if (!CardHashTable.Contains(card.CardData))
            {
                ListCardDetail.Add(card);
                CardHashTable.Add(card.CardData);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 创建一个不重复的卡
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="iCardNum"></param>
        /// <returns></returns>
        private CardDetail CreateNewCardDetail(int code)
        {

            //检查卡片是否重复
            if (CardHashTable.Contains(code))
            {
                if (code == 0)
                {
                    //有重复
                    return CreateNewCardDetail(0);
                }
                else
                {
                    return null;
                }

            }
            CardDetail card = new CardDetail();
            card.CardData = code;
            card.CardType = 1;
            return card;
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
                MessageBox.Show("输入的数字不正确，取值范围：1-32000！");
                return 0;
            }
            if (iCreateCount > 32000)
            {
                MessageBox.Show("输入的数字不正确，取值范围：1-32000！");
                return 0;
            }
            if ((iCreateCount + ListCardDetail.Count) > 32000)
            {
                iCreateCount = 32000 - ListCardDetail.Count;

            }
            if (iCreateCount <= 0) return 0;

            return iCreateCount;
        }
    }
}
