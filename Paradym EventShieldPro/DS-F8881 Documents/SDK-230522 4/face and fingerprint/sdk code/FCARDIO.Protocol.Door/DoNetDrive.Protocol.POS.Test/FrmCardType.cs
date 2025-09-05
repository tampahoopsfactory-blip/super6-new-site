using DoNetDrive.Protocol.POS.CardType;
using DoNetDrive.Protocol.POS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace DotNetDrive.Protocol.POS.Test
{
    public partial class FrmCardType : frmNodeForm
    {
        private static object lockobj = new object();
        private static FrmCardType onlyObj;
        public static FrmCardType GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new FrmCardType(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private FrmCardType(INMain main) : base(main)
        {
            InitializeComponent();
        }


        private void FrmCardType_Load(object sender, EventArgs e)
        {
            CardTypeHashTable = new HashSet<int>();
            dgvCardType.AutoGenerateColumns = false;
        }

        private BindingList<CardTypeDetail> ListCardTypeDetail = new BindingList<CardTypeDetail>();

        /// <summary>
        /// 卡号字典
        /// </summary>
        HashSet<int> CardTypeHashTable = null;

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
                string tip = $"卡类信息--最大容量：{result.SortSize},已存数量：{result.UseSize}";

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
                    dgvCardType.AutoGenerateColumns = false;
                    dgvCardType.DataSource = new BindingList<CardTypeDetail>(result.CardTypes);
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
            CardTypeHashTable.Clear();
            ClearDataBase cmd = new ClearDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        private void dgvCardType_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex, row = e.RowIndex;
            if (row < 0) return;

            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvCardType.Rows[e.RowIndex].Cells[e.ColumnIndex];
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

            var gdRow = dgvCardType.Rows[row];
            var cardType = gdRow.DataBoundItem as CardTypeDetail;
            //StringBuilder strBuf = new StringBuilder();

            //DebugCardDetail(CardUI.CardDetail, strBuf);
            //txtDebug.Text = strBuf.ToString();
            CardTypeDetailToControl(cardType);
        }

        /// <summary>
        /// 将卡片输出到控件中
        /// </summary>
        /// <param name="card"></param>
        private void CardTypeDetailToControl(CardTypeDetail cardType)
        {
            txtCardType.Value = cardType.CardType;
            txtDiscount.Value = cardType.Discount;
            txtIntegral.Value = cardType.Integral;
            CheckBox[] TGList = { cbTimeGroup1, cbTimeGroup2, cbTimeGroup3, cbTimeGroup4, cbTimeGroup5, cbTimeGroup6, cbTimeGroup7, cbTimeGroup8 };
            for (int i = 1; i <= 8; i++)
            {
                TGList[i - 1].Checked = cardType.GetTimeGroup(i);
            }
        }

        private void butAddToList_Click(object sender, EventArgs e)
        {
           
            CardTypeDetail dto = new CardTypeDetail();
            dto.Integral = Convert.ToByte(txtIntegral.Value);
            dto.CardType = Convert.ToByte(txtCardType.Value);
            dto.Discount = Convert.ToByte(txtDiscount.Value);
            CheckBox[] TGList = { cbTimeGroup1, cbTimeGroup2, cbTimeGroup3, cbTimeGroup4, cbTimeGroup5, cbTimeGroup6, cbTimeGroup7, cbTimeGroup8 };
            for (int i = 1; i <= 8; i++)
            {

                dto.SetTimeGroup(i, TGList[i - 1].Checked);
             
            }
            //dto.TimeGroup = print;
            ListCardTypeDetail.Add(dto);

            Invoke(() =>
            {
                dgvCardType.DataSource = new BindingList<CardTypeDetail>(ListCardTypeDetail);
            });
        }

        private void butAddToDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
          
            List<CardTypeDetail> _list = new List<CardTypeDetail>();
            CardTypeDetail dto = new CardTypeDetail();
            dto.Integral = Convert.ToByte(txtIntegral.Value);
            dto.CardType = Convert.ToByte(txtCardType.Value);
            dto.Discount = Convert.ToByte(txtDiscount.Value);
            CheckBox[] TGList = { cbTimeGroup1, cbTimeGroup2, cbTimeGroup3, cbTimeGroup4, cbTimeGroup5, cbTimeGroup6, cbTimeGroup7, cbTimeGroup8 };
            for (int i = 1; i <= 8; i++)
            {
                dto.SetTimeGroup(i, TGList[i - 1].Checked);
            }
            _list.Add(dto);
            WriteCardTypeDetail_Parameter par = new WriteCardTypeDetail_Parameter(_list);

            AddCardType cmd = new AddCardType(cmdDtl, par);
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
            List<CardTypeDetail> _list = new List<CardTypeDetail>();
            for (int i = 0; i < ListCardTypeDetail.Count; i++)
            {
                CardTypeDetail cardTypeDetail = new CardTypeDetail() { };
                cardTypeDetail.CardType = ListCardTypeDetail[i].CardType;
                cardTypeDetail.Discount = ListCardTypeDetail[i].Discount;
                cardTypeDetail.Integral = ListCardTypeDetail[i].Integral;
                cardTypeDetail.TimeGroup = ListCardTypeDetail[i].TimeGroup;
                _list.Add(cardTypeDetail);
            }
            if (_list.Count == 0)
            {
                return;
            }
            WriteCardTypeDetail_Parameter par = new WriteCardTypeDetail_Parameter(_list);
            AddCardType cmd = new AddCardType(cmdDtl, par);
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
            int cardType = Convert.ToInt32(txtCardType.Value);
            //if (!int.TryParse(txtCode.Text, out usercode))
            //{
            //    MessageBox.Show("商品代码格式不正确");
            //    return;
            //}
            var par = new ReadCardTypeDetail_Parameter(cardType);
            var cmd = new ReadCardTypeDetail(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadCardTypeDetail_Result;

                if (!result.IsReady)
                {
                    mMainForm.AddCmdLog(cmde, $"卡类不存在");
                }
                else
                {
                    //PersonToControl(result.Person);
                    mMainForm.AddCmdLog(cmde, $"卡类存在");
                }
            };
        }

        private void butDeleteFromList_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvCardType.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvCardType.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dgvCardType.Rows[i].Cells[1];
                    var item = ListCardTypeDetail.FirstOrDefault(t => t.CardType == byte.Parse(text.Value.ToString()));
                    ListCardTypeDetail.Remove(item);
                }
            }
            dgvCardType.DataSource = new BindingList<CardTypeDetail>(ListCardTypeDetail);
        }

        private void butDeleteFromDevice_Click(object sender, EventArgs e)
        {
            
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<CardTypeDetail> _list = new List<CardTypeDetail>();
            for (int i = 0; i < dgvCardType.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvCardType.Rows[i].Cells[0];
                DataGridViewTextBoxCell cellContent = (DataGridViewTextBoxCell)dgvCardType.Rows[i].Cells[1];
                if ((bool)cell.FormattedValue)
                {
                    _list.Add(new CardTypeDetail() { CardType = Convert.ToByte(cellContent.Value) });
                }
            }
            if (_list.Count ==0)
            {
                return;
            }

            WriteCardTypeDetail_Parameter par = new WriteCardTypeDetail_Parameter(_list);
            DeleteCardType cmd = new DeleteCardType(cmdDtl, par);
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
            byte iBeginNum = Convert.ToByte(txtStartCode.Value);
            //if (!UInt64.TryParse(.ToString(), out iBeginNum))
            //{
            //    return;
            //}
            if (iBeginNum == 0) iBeginNum = 1;


            ListCardTypeDetail.RaiseListChangedEvents = false;
            CardTypeDetail cardType;
            while (iCreateCount > 0)
            {
                cardType = CreateNewCardDetail(iBeginNum++);
                if (cardType != null)
                {
                    cardType.Discount = 100;
                    cardType.Integral = 1;
                    cardType.TimeGroup = 255;
                    AddCardTypeBaseToList(cardType);

                    iCreateCount--;
                }

            }
            ListCardTypeDetail.RaiseListChangedEvents = true;
            ListCardTypeDetail.ResetBindings();
            dgvCardType.DataSource = ListCardTypeDetail;
        }

        private bool AddCardTypeBaseToList(CardTypeDetail cardTypeDetail)
        {
            if (!CardTypeHashTable.Contains(cardTypeDetail.CardType))
            {
                ListCardTypeDetail.Add(cardTypeDetail);
                CardTypeHashTable.Add(cardTypeDetail.CardType);
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
            if ((iCreateCount + ListCardTypeDetail.Count) > 255)
            {
                iCreateCount = 255 - ListCardTypeDetail.Count;

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
        private CardTypeDetail CreateNewCardDetail(byte type)
        {

            //检查卡片是否重复
            if (CardTypeHashTable.Contains(type))
            {
                if (type == 0)
                {
                    //有重复
                    return CreateNewCardDetail(0);
                }
                else
                {
                    return null;
                }

            }
            CardTypeDetail cardType = new CardTypeDetail();
            cardType.CardType = type;
            return cardType;
        }

        private void butClearList_Click(object sender, EventArgs e)
        {
            ListCardTypeDetail.Clear();
            dgvCardType.DataSource = ListCardTypeDetail;
            CardTypeHashTable.Clear();
        }
    }
}
