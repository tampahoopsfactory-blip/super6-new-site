using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.Subsidy;
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
    public partial class FrmSubsidy : frmNodeForm
    {
        private static object lockobj = new object();
        private static FrmSubsidy onlyObj;
        public static FrmSubsidy GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new FrmSubsidy(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private FrmSubsidy(INMain main) : base(main)
        {
            InitializeComponent();
        }

        private void butClearList_Click(object sender, EventArgs e)
        {

        }
        private BindingList<SubsidyDetail> ListSubsidyDetail = new BindingList<SubsidyDetail>();

        string[] mSubsidyType = { "充值到补贴账户，永不过期", "充值到子账户，会过期", "充值到补贴账户，会过期","充值到现金账户" };

        /// <summary>
        /// 卡号字典
        /// </summary>
        HashSet<int> CardHashTable = null;
        private void FrmSubsidy_Load(object sender, EventArgs e)
        {
            CardHashTable = new HashSet<int>();
            dgvSubsidy.AutoGenerateColumns = false;

            cmbSubsidyType.Items.AddRange(mSubsidyType);
            cmbSubsidyType.SelectedIndex = 0;
        }

        private void butReadDataBase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadSubsidyDataBase cmd = new ReadSubsidyDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadSubsidyDataBase_Result result = cmde.Command.getResult() as ReadSubsidyDataBase_Result;
                string tip = $"补贴信息--最大容量：{result.SortSize},已存数量：{result.UseSize}";

                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butReadAllMenu_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAllSubsidy cmd = new ReadAllSubsidy(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAllSubsidy_Result result = cmde.Command.getResult() as ReadAllSubsidy_Result;
                string tip = $"";
                Invoke(() =>
                {
                    dgvSubsidy.AutoGenerateColumns = false;
                    dgvSubsidy.DataSource = new BindingList<SubsidyDetail>(result.SubsidyDetails);
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
            ClearSubsidy cmd = new ClearSubsidy(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        private void butAddToList_Click(object sender, EventArgs e)
        {
            SubsidyDetail dto = new SubsidyDetail();
            dto.CardData = Convert.ToInt32(txtCardData.Value);
            dto.CustomNumber = Convert.ToByte(txtCustomNumber.Value);
            dto.SubsidyDate = dtpSubsidyDate.Value;
            dto.SubsidyMoney = txtSubsidyMoney.Value;
            dto.SubsidyState = (byte)(cbSubsidyState.Checked ? 1 : 0);
            dto.SubsidyType = (byte)(cmbSubsidyType.SelectedIndex + 1);
            //dto.TimeGroup = print;
            ListSubsidyDetail.Add(dto);

            Invoke(() =>
            {
                dgvSubsidy.DataSource = new BindingList<SubsidyDetail>(ListSubsidyDetail);
            });
        }

        private void butAddToDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            List<SubsidyDetail> _list = new List<SubsidyDetail>();
            SubsidyDetail dto = new SubsidyDetail();
            dto.CardData = Convert.ToInt32(txtCardData.Value);
            dto.CustomNumber = Convert.ToByte(txtCustomNumber.Value);
            dto.SubsidyDate = dtpSubsidyDate.Value;
            dto.SubsidyMoney = txtSubsidyMoney.Value;
            dto.SubsidyState = (byte)(cbSubsidyState.Checked ? 1 : 0);
            dto.SubsidyType = (byte)(cmbSubsidyType.SelectedIndex + 1);
            //dto.TimeGroup = print;
            _list.Add(dto);
            WriteSussidy_Parameter par = new WriteSussidy_Parameter(_list);

            AddSussidy cmd = new AddSussidy(cmdDtl, par);
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
            List<SubsidyDetail> _list = new List<SubsidyDetail>();
            for (int i = 0; i < ListSubsidyDetail.Count; i++)
            {
                SubsidyDetail dto = new SubsidyDetail() { };
                dto.CardData = ListSubsidyDetail[i].CardData;
                dto.CustomNumber = ListSubsidyDetail[i].CustomNumber;
                dto.SubsidyDate = ListSubsidyDetail[i].SubsidyDate;
                dto.SubsidyMoney = ListSubsidyDetail[i].SubsidyMoney;
                dto.SubsidyState = ListSubsidyDetail[i].SubsidyState;
                dto.SubsidyType = ListSubsidyDetail[i].SubsidyType;
                _list.Add(dto);
            }
            if (_list.Count == 0)
            {
                return;
            }
            WriteSussidy_Parameter par = new WriteSussidy_Parameter(_list);

            AddSussidy cmd = new AddSussidy(cmdDtl, par);
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
            int cardType = Convert.ToInt32(txtCardData.Value);
            //if (!int.TryParse(txtCode.Text, out usercode))
            //{
            //    MessageBox.Show("商品代码格式不正确");
            //    return;
            //}
            var par = new ReadSubsidyDetail_Parameter(cardType);
            var cmd = new ReadSubsidyDetail(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadSubsidyDetail_Result;

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


            ListSubsidyDetail.RaiseListChangedEvents = false;
            SubsidyDetail subsidyDetail;
            while (iCreateCount > 0)
            {
                subsidyDetail = CreateNewCardDetail(iBeginNum++);
                if (subsidyDetail != null)
                {
                    subsidyDetail.SubsidyDate = DateTime.Now;
                    subsidyDetail.SubsidyType = 1;
                    subsidyDetail.SubsidyMoney = (decimal)123.45;
                    AddSubsidyBaseToList(subsidyDetail);

                    iCreateCount--;
                }

            }
            ListSubsidyDetail.RaiseListChangedEvents = true;
            ListSubsidyDetail.ResetBindings();
            dgvSubsidy.DataSource = ListSubsidyDetail;
        }

        private bool AddSubsidyBaseToList(SubsidyDetail subsidyDetail)
        {
            if (!CardHashTable.Contains(subsidyDetail.CardData))
            {
                ListSubsidyDetail.Add(subsidyDetail);
                CardHashTable.Add(subsidyDetail.CardData);
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
            if ((iCreateCount + ListSubsidyDetail.Count) > 255)
            {
                iCreateCount = 255 - ListSubsidyDetail.Count;

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
        private SubsidyDetail CreateNewCardDetail(int card)
        {

            //检查卡片是否重复
            if (CardHashTable.Contains(card))
            {
                if (card == 0)
                {
                    //有重复
                    return CreateNewCardDetail(0);
                }
                else
                {
                    return null;
                }

            }
            SubsidyDetail subsidyDetail = new SubsidyDetail();
            subsidyDetail.CardData = card;
            return subsidyDetail;
        }

        private void dgvSubsidy_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
