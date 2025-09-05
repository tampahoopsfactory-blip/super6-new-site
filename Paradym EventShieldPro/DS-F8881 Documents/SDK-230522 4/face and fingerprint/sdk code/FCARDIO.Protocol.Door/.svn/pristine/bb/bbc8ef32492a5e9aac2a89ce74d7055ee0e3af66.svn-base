using DoNetDrive.Protocol.POS.ConsumeParameter.AdditionalCharges;
using DoNetDrive.Protocol.POS.ConsumeParameter.CancelConsume;
using DoNetDrive.Protocol.POS.ConsumeParameter.ConsumePassword;
using DoNetDrive.Protocol.POS.ConsumeParameter.ConsumptionLimits;
using DoNetDrive.Protocol.POS.ConsumeParameter.CountingCards;
using DoNetDrive.Protocol.POS.ConsumeParameter.Discount;
using DoNetDrive.Protocol.POS.ConsumeParameter.FixedFeeRule;
using DoNetDrive.Protocol.POS.ConsumeParameter.ICCardAccount;
using DoNetDrive.Protocol.POS.ConsumeParameter.Integral;
using DoNetDrive.Protocol.POS.ConsumeParameter.POSWorkMode;
using DoNetDrive.Protocol.POS.ConsumeParameter.ReservationRule;
using DoNetDrive.Protocol.POS.ConsumeParameter.TemporaryChangeFixedFee;
using DoNetDrive.Protocol.POS.Data;
using DoNetTool.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetDrive.Protocol.POS.Test
{
    public partial class frmConsumeParameter : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static frmConsumeParameter onlyObj;
        public static frmConsumeParameter GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmConsumeParameter(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private frmConsumeParameter(INMain main) : base(main)
        {
            InitializeComponent();
        }
        #endregion

        List<string> mPOSWorkMode = new List<string> { "标准收费", "定额收费", "菜单收费", "订餐机", "补贴机", "子账收费", "子账补贴" };

        string[] WeekdayList = new string[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };

        int mWeekdayIndex = 1;

        private BindingList<ReservationRuleDetail> ListReservationRuleDetail = new BindingList<ReservationRuleDetail>();

        /// <summary>
        /// 订餐规则
        /// </summary>
        WeekReservationRule mWeekReservationRule;

        private void frmConsumeParameter_Load(object sender, EventArgs e)
        {
            cmbPOSWorkMode.Items.AddRange(mPOSWorkMode.ToArray());
            cmbPOSWorkMode.SelectedIndex = 0;

            for (int i = 0; i < 8; i++)
            {
                ReservationRuleDetail detail = new ReservationRuleDetail();
                detail.SerialNumber = (byte)(i + 1);
                detail.BeginTime = new DateTime(2000,1,1,0,0,0);
                detail.EndTime = new DateTime(2000,1,1,0,0,0);
                ListReservationRuleDetail.Add(detail);
            }
            dgvReservationRule.AutoGenerateColumns = false; 
            dgvReservationRule.DataSource = new BindingList<ReservationRuleDetail>(ListReservationRuleDetail);

        }

        public frmConsumeParameter()
        {
            InitializeComponent();
        }

        private void butReadPOSWorkMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadPOSWorkMode cmd = new ReadPOSWorkMode(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadPOSWorkMode_Result result = cmde.Command.getResult() as ReadPOSWorkMode_Result;

                string tip = $"消费模式：{mPOSWorkMode[result.Mode - 1]}";
                Invoke(() =>
                {
                    cmbPOSWorkMode.SelectedIndex = result.Mode - 1;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWritePOSWorkMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WritePOSWorkMode_Parameter par = new WritePOSWorkMode_Parameter((byte)(cmbPOSWorkMode.SelectedIndex + 1));
            WritePOSWorkMode cmd = new WritePOSWorkMode(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadConsumptionLimits_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadConsumptionLimits cmd = new ReadConsumptionLimits(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadConsumptionLimits_Result result = cmde.Command.getResult() as ReadConsumptionLimits_Result;

                string tip = $"消费限额：单次限额:{result.LimitMoney},单日限额:{result.DayLimitMoney},单日限次:{result.DayLimit},月限额:{result.MonthLimitMoney},月限次:{result.MonthLimit},卡内最低保留余额:{result.MinimumReservedBalance}";
                Invoke(() =>
                {
                    txtLimitMoney.Value = result.LimitMoney;
                    txtDayLimitMoney.Value = result.DayLimitMoney;
                    txtDayLimit.Value = result.DayLimit;
                    txtMonthLimitMoney.Value = result.MonthLimitMoney;
                    txtMonthLimit.Value = result.MonthLimit;
                    txtMinimumReservedBalance.Value = result.MinimumReservedBalance;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteConsumptionLimits_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteConsumptionLimits_Parameter par = new WriteConsumptionLimits_Parameter((int)(txtLimitMoney.Value), (int)(txtDayLimitMoney.Value),(byte)(txtDayLimit.Value)
                ,(int)(txtMonthLimitMoney.Value),(byte)(txtMonthLimit.Value),(int)(txtMinimumReservedBalance.Value));
            WriteConsumptionLimits cmd = new WriteConsumptionLimits(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadConsumePassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadConsumePassword cmd = new ReadConsumePassword(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadConsumePassword_Result result = cmde.Command.getResult() as ReadConsumePassword_Result;
                bool bUseConsumePassword = result.Use == 1;
                string tip = $"消费时确认密码:【{(bUseConsumePassword? "启用":"不启用")}，免密码消费限额:{result.LimitMoney}】";
                Invoke(() =>
                {
                    cbUseConsumePassword.Checked = bUseConsumePassword;
                    txtPwdLimitMoney.Value = result.LimitMoney;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteConsumePassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteConsumePassword_Parameter par = new WriteConsumePassword_Parameter((byte)(cbUseConsumePassword.Checked ? 1:0), (int)(txtPwdLimitMoney.Value));
            WriteConsumePassword cmd = new WriteConsumePassword(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadTemporaryChange_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTemporaryChangeFixedFee cmd = new ReadTemporaryChangeFixedFee(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTemporaryChangeFixedFee_Result result = cmde.Command.getResult() as ReadTemporaryChangeFixedFee_Result;
                bool bUseConsumePassword = result.Use == 1;
                bool bReturnOriginal = result.ReturnOriginal == 1;
                string tip = $"临时变更定额、定次消费额度:【{(bUseConsumePassword ? "启用" : "不启用")}，消费后自动还原:{(bReturnOriginal ? "启用" : "不启用")}】";
                Invoke(() =>
                {
                    cbUseTemporaryChange.Checked = bUseConsumePassword;
                    cbReturnOriginal.Checked = bReturnOriginal;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteTemporaryChange_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteTemporaryChangeFixedFee_Parameter par = new WriteTemporaryChangeFixedFee_Parameter((byte)(cbUseConsumePassword.Checked ? 1 : 0), (byte)(cbReturnOriginal.Checked ? 1 : 0));
            WriteTemporaryChangeFixedFee cmd = new WriteTemporaryChangeFixedFee(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadCancelConsume_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadCancelConsume cmd = new ReadCancelConsume(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCancelConsume_Result result = cmde.Command.getResult() as ReadCancelConsume_Result;
                bool bUseCancelConsume = result.Use == 1;
                string tip = $"撤销消费:【{(bUseCancelConsume ? "启用" : "不启用")}，最大撤销天数:{result.CancelDays}】";
                Invoke(() =>
                {
                    cbUseCancelConsume.Checked = bUseCancelConsume;
                    txtCancelDays.Value = result.CancelDays;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteCancelConsume_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteCancelConsume_Parameter par = new WriteCancelConsume_Parameter((byte)(cbUseCancelConsume.Checked ? 1 : 0), (byte)txtCancelDays.Value);
            WriteCancelConsume cmd = new WriteCancelConsume(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadICCardAccount_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadICCardAccount cmd = new ReadICCardAccount(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadICCardAccount_Result result = cmde.Command.getResult() as ReadICCardAccount_Result;
                bool bUseCashAccount = result.UseCashAccount == 1;
                bool bUseSubsidyAccount = result.UseSubsidyAccount == 1;
                string tip = $"IC卡账户:【{(bUseCashAccount ? "启用" : "不启用")}，补贴账户:{(bUseSubsidyAccount ? "启用" : "不启用")}】";
                Invoke(() =>
                {
                    cbUseCashAccount.Checked = bUseCashAccount;
                    cbUseSubsidyAccount.Checked = bUseSubsidyAccount;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteICCardAccount_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteICCardAccount_Parameter par = new WriteICCardAccount_Parameter((byte)(cbUseCashAccount.Checked ? 1 : 0), (byte)(cbUseSubsidyAccount.Checked ? 1 : 0));
            WriteICCardAccount cmd = new WriteICCardAccount(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadAdditionalCharges_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAdditionalCharges cmd = new ReadAdditionalCharges(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAdditionalCharges_Result result = cmde.Command.getResult() as ReadAdditionalCharges_Result;
                bool bUseAdditionalCharges = result.Use == 1;
                string tip = $"附加费用，时段多次消费收取附加费:【{(bUseAdditionalCharges ? "启用" : "不启用")}】";
                if (bUseAdditionalCharges)
                {
                    tip += $"，时段消费超过 {result.FreeTimes} 次时收取附加费";
                }
                Invoke(() =>
                {
                    cbUseAdditionalCharges.Checked = bUseAdditionalCharges;
                    txtAdditionalChargesFreeTimes.Value = result.FreeTimes;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteAdditionalCharges_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteAdditionalCharges_Parameter par = new WriteAdditionalCharges_Parameter((byte)(cbUseAdditionalCharges.Checked ? 1 : 0), (byte)(txtAdditionalChargesFreeTimes.Value),0);
            WriteAdditionalCharges cmd = new WriteAdditionalCharges(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadDiscount_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadDiscount cmd = new ReadDiscount(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadDiscount_Result result = cmde.Command.getResult() as ReadDiscount_Result;
                bool bUseICCardDiscount = result.UseICCardDiscount == 1;
                bool bUsePOSDiscount = result.UsePOSDiscount == 1;
                bool bUseCardTypeDiscount = result.UseCardTypeDiscount == 1;
                bool bUseDoubleDiscount = result.UseDoubleDiscount == 1;
                string tip = $"折扣:【本机折扣:{result.POSDiscount}，{(bUseICCardDiscount ? "IC卡折扣启用" : "IC卡折扣不启用")}，{(bUseCardTypeDiscount ? "卡类折扣启用" : "卡类折扣不启用")}，{(bUsePOSDiscount ? "机器折扣启用" : "机器折扣不启用")}，{(bUseDoubleDiscount ? "折上折扣启用" : "折上折扣不启用")}】";
                Invoke(() =>
                {
                    cbUseICCardDiscount.Checked = bUseICCardDiscount;
                    cbUsePOSDiscount.Checked = bUsePOSDiscount;
                    cbUseCardTypeDiscount.Checked = bUseCardTypeDiscount;
                    cbDoubleDiscount.Checked = bUseDoubleDiscount;
                    txtPOSDiscount.Value = result.POSDiscount;

                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteDiscount_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteDiscount_Parameter par = new WriteDiscount_Parameter((byte)(cbUseICCardDiscount.Checked ? 1 : 0), (byte)(cbUsePOSDiscount.Checked ? 1 : 0), (byte)(cbUseCardTypeDiscount.Checked ? 1 : 0), (byte)(txtPOSDiscount.Value), (byte)(cbDoubleDiscount.Checked ? 1 : 0));
            WriteDiscount cmd = new WriteDiscount(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadIntegral_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadIntegral cmd = new ReadIntegral(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadIntegral_Result result = cmde.Command.getResult() as ReadIntegral_Result;
                bool bUse = result.Use == 1;
                string tip = $"积分:【{(bUse ? "积分启用" : "积分不启用")}，消费金额:{result.Money}，积分值:{result.Integral}，单次最大累计次数:{result.MaxCount}，单次消费最高积分:{result.MaxIntegral}】";
                Invoke(() =>
                {
                    cbUseIntegral.Checked = bUse;
                    txtMaxCount.Value = result.MaxCount;
                    txtMaxIntegral.Value = result.MaxIntegral;
                    txtIntegralMoney.Value = result.Money;
                    txtIntegral.Value = result.Integral;

                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteIntegral_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteIntegral_Parameter par = new WriteIntegral_Parameter((byte)(cbUseIntegral.Checked ? 1 : 0), (int)(txtIntegralMoney.Value), (int)(txtIntegral.Value), (int)(txtMaxIntegral.Value), (int)(txtMaxCount.Value));
            WriteIntegral cmd = new WriteIntegral(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadCountingCards_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadCountingCards cmd = new ReadCountingCards(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCountingCards_Result result = cmde.Command.getResult() as ReadCountingCards_Result;
                bool bUse = result.Use == 1;
                bool bUseResidueCount = result.UseResidueCount == 1;

                string tip = $"计次:【{(bUse ? "计次启用" : "计次不启用")}，单次消费扣除次数{result.DeductionCount},{(bUseResidueCount ? "启用计次卡消费后不扣除剩余次数":"不启用计次卡消费后不扣除剩余次数")}】";
                Invoke(() =>
                {
                    cbUseCountingCards.Checked = bUse;
                    cbUseResidueCount.Checked = bUseResidueCount;
                    txtDeductionCount.Value = result.DeductionCount;

                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteCountingCards_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteCountingCards_Parameter par = new WriteCountingCards_Parameter((byte)(cbUseCountingCards.Checked ? 1 : 0), (byte)(txtDeductionCount.Value), (byte)(cbUseResidueCount.Checked ? 1 : 0));
            WriteCountingCards cmd = new WriteCountingCards(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadFixedFeeRule_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadFixedFeeRule cmd = new ReadFixedFeeRule(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadFixedFeeRule_Result result = cmde.Command.getResult() as ReadFixedFeeRule_Result;

                Invoke(() =>
                {
                    dgvFixedFeeRule.AutoGenerateColumns = false;
                    dgvFixedFeeRule.DataSource = new BindingList<FixedFeeRuleDetail>(result.DataList);
                    //dgvFixedFeeRule.DataSource = result.PrintContents;
                });
                StringBuilder sb = new StringBuilder($"定额扣费规则：");
                foreach (var item in result.DataList)
                {
                    sb.AppendLine($"序号:{item.SerialNumber},开始时间:{item.BeginTime.ToString("HH:mm")},结束时间:{item.EndTime.ToString("HH:mm")},订餐:{(item.IsReservation == 1 ? "启用" : "禁用")},定额值:{item.FixedFee},消费限额:{item.ConsumptionLimits},限次:{item.Limite},计次卡限次:{item.CountingCardsLimitsCount},计次卡扣次:{item.CountingCardsDeductionCount},餐段名称:{item.MealTimeName}");
                }

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void butWriteFixedFeeRule_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            List<FixedFeeRuleDetail> fixedFeeRuleDetail = new List<FixedFeeRuleDetail>(8);
            for (int i = 0; i < dgvFixedFeeRule.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cellIsReservation = (DataGridViewCheckBoxCell)dgvFixedFeeRule.Rows[i].Cells[8];
                DataGridViewTextBoxCell cellBeginTime = (DataGridViewTextBoxCell)dgvFixedFeeRule.Rows[i].Cells[1];
                DataGridViewTextBoxCell cellEndTime = (DataGridViewTextBoxCell)dgvFixedFeeRule.Rows[i].Cells[2];
                DataGridViewTextBoxCell cellFixedFee = (DataGridViewTextBoxCell)dgvFixedFeeRule.Rows[i].Cells[3];
                DataGridViewTextBoxCell cellConsumptionLimits = (DataGridViewTextBoxCell)dgvFixedFeeRule.Rows[i].Cells[4];
                DataGridViewTextBoxCell cellLimite = (DataGridViewTextBoxCell)dgvFixedFeeRule.Rows[i].Cells[5];
                DataGridViewTextBoxCell cellCountingCardsDeductionCount = (DataGridViewTextBoxCell)dgvFixedFeeRule.Rows[i].Cells[6];
                DataGridViewTextBoxCell cellCountingCardsLimitsCount = (DataGridViewTextBoxCell)dgvFixedFeeRule.Rows[i].Cells[7];
                DataGridViewTextBoxCell cellMealTimeName = (DataGridViewTextBoxCell)dgvFixedFeeRule.Rows[i].Cells[9];

                FixedFeeRuleDetail model = new FixedFeeRuleDetail();
                model.SerialNumber = (byte)(i + 1);
                if ((bool)cellIsReservation.FormattedValue)
                    model.IsReservation = 1;
                else
                    model.IsReservation = 0;
                var beginhour = cellBeginTime.Value.ToString().Split(':')[0];
                var beginminute = cellBeginTime.Value.ToString().Split(':')[1];
                var endhour = cellEndTime.Value.ToString().Split(':')[0];
                var endminute = cellEndTime.Value.ToString().Split(':')[1];
                model.BeginTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,int.Parse(beginhour), int.Parse(beginminute), 0) ;
                model.EndTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,int.Parse(endhour), int.Parse(endminute), 0) ;
                model.ConsumptionLimits = (decimal)cellConsumptionLimits.Value;
                model.Limite = (byte)cellLimite.Value;
                model.FixedFee = (decimal)cellFixedFee.Value;
                model.CountingCardsDeductionCount = (byte)cellCountingCardsDeductionCount.Value;
                model.CountingCardsLimitsCount = (byte)cellCountingCardsLimitsCount.Value;
                model.MealTimeName = cellMealTimeName.Value.ToString();
                fixedFeeRuleDetail.Add(model);
            }
            WriteFixedFeeRule_Parameter par = new WriteFixedFeeRule_Parameter(fixedFeeRuleDetail);
            WriteFixedFeeRule cmd = new WriteFixedFeeRule(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        private void dgvFixedFeeRule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 8 )
            {

                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvFixedFeeRule.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            if ((e.ColumnIndex >= 1 && e.ColumnIndex <= 7) || e.ColumnIndex == 9)
            {
                DataGridViewTextBoxColumn textbox = dgvFixedFeeRule.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
                if (textbox != null) //如果该列是TextBox列
                {
                    dgvFixedFeeRule.BeginEdit(true); //开始编辑状态
                    dgvFixedFeeRule.ReadOnly = false;
                }

            }
            else
            {
                dgvFixedFeeRule.BeginEdit(false); //开始编辑状态
                dgvFixedFeeRule.ReadOnly = true;
            }
        }

        private void dgvReservationRule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
           
            if ((e.ColumnIndex >= 1 && e.ColumnIndex <= 4) )
            {
                DataGridViewTextBoxColumn textbox = dgvReservationRule.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
                if (textbox != null) //如果该列是TextBox列
                {
                    dgvReservationRule.BeginEdit(true); //开始编辑状态
                    dgvReservationRule.ReadOnly = false;
                }

            }
            else
            {
                dgvReservationRule.BeginEdit(false);
                dgvReservationRule.ReadOnly = true;
            }
        }

        private void butReadReservationRule_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReservationRule cmd = new ReadReservationRule(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadReservationRule_Result result = cmde.Command.getResult() as ReadReservationRule_Result;

                Invoke(() =>
                {
                    mWeekReservationRule = result.WeekReservationRule;
                    dgvReservationRule.DataSource = new BindingList<ReservationRuleDetail>(mWeekReservationRule.GetItem(mWeekdayIndex - 1).mReservationRules);
                    //dgvReservationRule.DataSource = new BindingList<ReservationRuleDetail>(result.DataList);
                    //dgvFixedFeeRule.DataSource = result.PrintContents;
                });
                StringBuilder sb = new StringBuilder($"订餐规则：");
                for (int i = 0; i < 7; i++)
                {
                    DayReservationRule dayReservationRule = mWeekReservationRule.GetItem(i);
                    sb.AppendLine(WeekdayList[i]);
                    for (int j = 0; j < 8; j++)
                    {
                        ReservationRuleDetail item = dayReservationRule.GetItem(j);
                        sb.AppendLine($"序号:{item.SerialNumber},开始时间:{item.ShowBeginTime},结束时间:{item.ShowEndTime},订餐星期:{item.ShowWeekday},订餐餐段:{item.MealTimeIndex}");
                    }
                }
                //foreach (var item in result.DataList)
                //{
                //    sb.AppendLine($"序号:{item.SerialNumber},开始时间:{item.BeginTime.ToString("HH:mm")},结束时间:{item.EndTime.ToString("HH:mm")},订餐:{(item.IsReservation == 1 ? "启用" : "禁用")},定额值:{item.FixedFee},消费限额:{item.ConsumptionLimits},限次:{item.Limite},计次卡限次:{item.CountingCardsLimitsCount},计次卡扣次:{item.CountingCardsDeductionCount},餐段名称:{item.MealTimeName}");
                //}

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void butWriteReservationRule_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteReservationRule_Parameter par = new WriteReservationRule_Parameter(mWeekReservationRule);
            WriteReservationRule cmd = new WriteReservationRule(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }


        private void butWeekday_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 7; i++)
            {
                Button btnWeekday = FindControl(gbReservationRule, "butWeekday" + i.ToString()) as Button;
                btnWeekday.BackColor = Color.Transparent;
            }
            Button button = sender as Button;
            button.BackColor = Color.LightGreen;
            mWeekdayIndex = button.Name.Substring(10, 1).ToInt32();

            dgvReservationRule.DataSource = new BindingList<ReservationRuleDetail>(mWeekReservationRule.GetItem(mWeekdayIndex - 1).mReservationRules);
        }

        private void dgvReservationRule_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvReservationRule.CurrentCell.ColumnIndex == 1 || dgvReservationRule.CurrentCell.ColumnIndex == 2 || dgvReservationRule.CurrentCell.ColumnIndex == 4)
            {
                DataGridViewTextBoxEditingControl editingControl = e.Control as DataGridViewTextBoxEditingControl;
                editingControl.TextChanged += (se, ea) =>
                {
                    int RowIndex = dgvReservationRule.CurrentCell.RowIndex;
                    int ColumnIndex = dgvReservationRule.CurrentCell.ColumnIndex;
                    //listFix[dataGridView5.CurrentCell.RowIndex].Card = dataGridView5.CurrentCell.EditedFormattedValue.ToString();
                    string value = dgvReservationRule.CurrentCell.EditedFormattedValue.ToString();
                    int MealTimeIndex = 0;
                    //订餐餐段
                    if (ColumnIndex == 4)
                    {
                        if (!int.TryParse(value,out MealTimeIndex) || (MealTimeIndex < 1 || MealTimeIndex > 8))
                        {
                            return;
                        }
                        mWeekReservationRule.GetItem(mWeekdayIndex - 1).mReservationRules[RowIndex].MealTimeIndex = MealTimeIndex;

                    }

                    if (ColumnIndex == 1)
                    {
                        Regex r = new Regex(@"^([01]\d|2[0123]):([0-5]\d|60)$");
                        if (r.Match(value).Success)
                        {
                            int hour = value.Split(':')[0].ToInt32();
                            int minute = value.Split(':')[1].ToInt32();
                            mWeekReservationRule.GetItem(mWeekdayIndex - 1).mReservationRules[RowIndex].BeginTime = new DateTime(2000,1,1, hour,minute,0);
                        }
                        else
                        {

                        }

                    }
                    if (ColumnIndex == 2)
                    {
                        Regex r = new Regex(@"^([01]\d|2[0-3]):([0-5]\d)$");
                        if (r.Match(value).Success)
                        {
                            int hour = value.Split(':')[0].ToInt32();
                            int minute = value.Split(':')[1].ToInt32();
                            mWeekReservationRule.GetItem(mWeekdayIndex - 1).mReservationRules[RowIndex].EndTime = new DateTime(2000, 1, 1, hour, minute, 0);
                        }
                        else
                        {

                        }

                    }
                };
            }
            if (dgvReservationRule.CurrentCell.ColumnIndex == 3)
            {
                DataGridViewComboBoxEditingControl editingControl = e.Control as DataGridViewComboBoxEditingControl;
                editingControl.SelectedIndexChanged += (se, ea) =>
                {
                    int RowIndex = dgvReservationRule.CurrentCell.RowIndex;
                    int ColumnIndex = dgvReservationRule.CurrentCell.ColumnIndex;
                    if (editingControl.SelectedIndex != 0)
                    {
                        mWeekReservationRule.GetItem(mWeekdayIndex - 1).mReservationRules[RowIndex].Weekday = editingControl.SelectedIndex;
                    }
                };
                   
            }
        }
    }
}
