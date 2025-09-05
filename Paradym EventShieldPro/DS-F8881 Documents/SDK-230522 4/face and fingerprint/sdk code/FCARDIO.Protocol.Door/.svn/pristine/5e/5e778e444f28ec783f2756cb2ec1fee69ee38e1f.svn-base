using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.Transaction;
using DoNetDrive.Protocol.Transaction;
using DoNetTool.Common.Extensions;
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
    public partial class FrmRecord : frmNodeForm
    {
        #region 单例模式
        public static string[] mWatchTypeNameList;

        /// <summary>
        /// 事件代码名称列表
        /// </summary>
        public static List<string[]> mTransactionCodeNameList;
        public static string[] mCardTransactionList, mSystemTransactionList;

        private static object lockobj = new object();
        private static FrmRecord onlyObj;
        public static FrmRecord GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new FrmRecord(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private FrmRecord(INMain main) : base(main)
        {
            InitializeComponent();
        }

        static FrmRecord()
        {
            var time = 0x80ca3050;
            var s = time >> 26 & 0x3f;
            var s2 = s | 0x3f >>26 ;
            var m = time >> 20 & 0x3f;
            var m2 = m | 0x3f >> 20 ;
            var h = time >> 15 & 0x1f;
            var d = time >> 10 & 0x1f;
            var mm = time >> 6 & 0xf;
            var y = time & 0x3f;

           
            mWatchTypeNameList = new string[] { "", "读卡信息", "系统信息" };
            mCardTransactionList = new string[256];
            mSystemTransactionList = new string[256];

            mTransactionCodeNameList = new List<string[]>(10);
            mTransactionCodeNameList.Add(null);//0是没有的
            mTransactionCodeNameList.Add(mCardTransactionList);
            mTransactionCodeNameList.Add(mSystemTransactionList);

            mCardTransactionList[1] = "消费";
            mCardTransactionList[2] = "充值";
            mCardTransactionList[3] = "补贴充值";
            mCardTransactionList[4] = "退款";
            mCardTransactionList[5] = "订餐";
            mCardTransactionList[6] = "就餐";
            mCardTransactionList[7] = "补贴过期清零";
            mCardTransactionList[8] = "计次卡减次";
            mCardTransactionList[9] = "计次卡充值";
            mCardTransactionList[10] = "卡片过期";
            mCardTransactionList[11] = "无效消费时段";
            mCardTransactionList[12] = "余额不足";
            mCardTransactionList[13] = "挂失卡";
            mCardTransactionList[14] = "黑名单";
            mCardTransactionList[15] = "超出次限额";
            mCardTransactionList[16] = "超出日限额";
            mCardTransactionList[17] = "超出日限次";
            mCardTransactionList[18] = "超出月限额";
            mCardTransactionList[19] = "超出月限次";
            mCardTransactionList[20] = "未激活—时间未到";
            mCardTransactionList[21] = "IC卡校验失败金额块校验";
            mCardTransactionList[22] = "定额餐段-未授权";
            mCardTransactionList[23] = "未订餐";
            mCardTransactionList[24] = "定额餐段-未到进餐时间";
            mCardTransactionList[25] = "定额餐段-超出餐段限额";
            mCardTransactionList[26] = "定额餐段-超出餐段限次";
            mCardTransactionList[27] = "非本系统卡";
            mCardTransactionList[28] = "计次卡在非定额模式下刷卡";
            mCardTransactionList[29] = "消费机没有该卡类";
            mCardTransactionList[30] = "消费机没有卡类信息";
            mCardTransactionList[31] = "免费卡";
            mCardTransactionList[32] = "超额消费，密码输入错误或者取消输入密码退出";
            mCardTransactionList[33] = "两个账户都不可用";
            mCardTransactionList[34] = "超出计次卡限次";
            mCardTransactionList[35] = "超出对应餐段计次卡限次";
            mCardTransactionList[36] = "补贴充值时，超过限额超过补贴上限";
            mCardTransactionList[37] = "记次卡功能未开启";
            mCardTransactionList[38] = "重复刷卡";
            mCardTransactionList[39] = "写卡错误";
            mCardTransactionList[43] = "消费机补贴充值失败，补贴超出限额";
            mCardTransactionList[100] = "子账户消费";
            mCardTransactionList[101] = "子账户充值";
            mCardTransactionList[102] = "子账户过期清零";

            mSystemTransactionList[1] = "系统加电--开机";
            mSystemTransactionList[2] = "系统错误复位（看门狗）";
            mSystemTransactionList[3] = "设备格式化记录";
            mSystemTransactionList[5] = "电压过高";
            mSystemTransactionList[6] = "电压过低";
            mSystemTransactionList[7] = "温度过高";
            mSystemTransactionList[8] = "温度过低";
            mSystemTransactionList[9] = "透水警告   ";
            mSystemTransactionList[10] = "系统待机";
            mSystemTransactionList[11] = "系统唤醒";
            mSystemTransactionList[12] = "进入管理菜单";
            mSystemTransactionList[13] = "拆机警告";
            mSystemTransactionList[14] = "网线断开";
            mSystemTransactionList[15] = "网线接入";
            mSystemTransactionList[16] = "Wifi断开";
            mSystemTransactionList[17] = "Wifi接入";
            mSystemTransactionList[18] = "更改Wifi参数";
            mSystemTransactionList[19] = "更改IP参数";
            mSystemTransactionList[20] = "系统关机";
        }
        #endregion

        string[] mPOSWorkMode = { "","标准收费", "定额收费", "菜单收费", "订餐机", "补贴机", "子账收费", "子账补贴" };

        private void FrmRecord_Load(object sender, EventArgs e)
        {
            cmbTransactionDatabaseType.Items.AddRange(new []{ "读卡信息", "系统信息"});
            cmbTransactionDatabaseType.SelectedIndex = 0;
        }
        private void butTransactionDatabaseDetail_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var cmd = new ReadTransactionDatabaseDetail(cmdDtl);
            mMainForm.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadTransactionDatabaseDetail_Result;
                for (int i = 0; i < 2; i++)
                {
                    TextBox txtQuantity = FindControl(groupBox1, "txtQuantity" + (i + 1).ToString()) as TextBox;
                    TextBox txtNewRecord = FindControl(groupBox1, "txtNewRecord" + (i + 1).ToString()) as TextBox;
                    TextBox txtWriteIndex = FindControl(groupBox1, "txtWriteIndex" + (i + 1).ToString()) as TextBox;
                    TextBox txtStartIndex = FindControl(groupBox1, "txtStartIndex" + (i + 1).ToString()) as TextBox;
                    TextBox txtEndIndex = FindControl(groupBox1, "txtEndIndex" + (i + 1).ToString()) as TextBox;
                    Invoke(() =>
                    {
                        txtQuantity.Text = result.DatabaseDetail.ListTransaction[i].DataBaseMaxSize.ToString();
                        txtWriteIndex.Text = result.DatabaseDetail.ListTransaction[i].WriteIndex.ToString();
                        txtNewRecord.Text = result.DatabaseDetail.ListTransaction[i].readable().ToString();
                        txtStartIndex.Text = result.DatabaseDetail.ListTransaction[i].StartIndex.ToString();
                        txtEndIndex.Text = result.DatabaseDetail.ListTransaction[i].EndIndex.ToString();
                    });
                }
            };
        }

        private void butClearTransactionDatabase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var cmd = new TransactionDatabaseEmpty(cmdDtl);
            mMainForm.AddCommand(cmd);

        }

        private void butClearTransaction_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();

            int type = cmbTransactionDatabaseType.SelectedIndex;
            int WriteIndex = int.Parse(txtWriteIndex.Text.ToString());
            var par = new ClearTransactionDatabase_Parameter(Get_TransactionDatabaseType(type));
            var cmd = new ClearTransactionDatabase(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butClearIndex_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();

            int type = cmbTransactionDatabaseType.SelectedIndex;
            int WriteIndex = int.Parse(txtWriteIndex.Text.ToString());
            var par = new ClearTransactionDatabase_Parameter(Get_TransactionDatabaseType(type));
            var cmd = new ClearTransactionDatabase_StartIndex(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butResetIndex_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();

            int type = cmbTransactionDatabaseType.SelectedIndex;
            int WriteIndex = int.Parse(txtWriteIndex.Text.ToString());
            var par = new ClearTransactionDatabase_Parameter(Get_TransactionDatabaseType(type));
            var cmd = new ClearTransactionDatabase_ResetIndex(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butWriteIndex_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();

            int type = cmbTransactionDatabaseType.SelectedIndex;
            int WriteIndex = int.Parse(txtWriteIndex.Text.ToString());
            var par = new WriteTransactionDatabaseIndex_Parameter(Get_TransactionDatabaseType(type), WriteIndex);
            var cmd = new WriteTransactionDatabaseIndex(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butWriteStartIndex_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();

            int type = cmbTransactionDatabaseType.SelectedIndex;
            int WriteIndex = int.Parse(txtWriteStartIndex.Text.ToString());
            var par = new WriteTransactionDatabaseIndex_Parameter(Get_TransactionDatabaseType(type), WriteIndex);
            var cmd = new WriteTransactionDatabaseStartIndex(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butWriteEndIndex_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();

            int type = cmbTransactionDatabaseType.SelectedIndex;
            int WriteIndex = int.Parse(txtWriteEnd.Text.ToString());
            var par = new WriteTransactionDatabaseIndex_Parameter(Get_TransactionDatabaseType(type), WriteIndex);
            var cmd = new WriteTransactionDatabaseEndIndex(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        #region 判断记录类型
        private static e_TransactionDatabaseType Get_TransactionDatabaseType(int type)
        {
            type = type + 1;
            var i = e_TransactionDatabaseType.OnCardTransaction;


            if (type == 2)
            {
                i = e_TransactionDatabaseType.OnSystemTransaction;
            }

            return i;
        }
        #endregion

        private void butReadTransactionDatabase_Click(object sender, EventArgs e)
        {
            int type = cmbTransactionDatabaseType.SelectedIndex;
            int Quantity = int.Parse(txtReadTransactionDatabaseQuantity.Text);
            int PacketSize = 0;
            if (txtReadTransactionDatabasePacketSize.Text != "")
            {
                PacketSize = int.Parse(txtReadTransactionDatabasePacketSize.Text);
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 1000;
            cmdDtl.RestartCount = 20;

            var par = new ReadTransactionDatabase_Parameter((int)Get_TransactionDatabaseType(type), Quantity);
            if (PacketSize != 0)
            {
                par.PacketSize = PacketSize;
            }

            var cmd = new ReadTransactionDatabase(cmdDtl, par);
            cmdDtl.Timeout = 4000;
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {

                var result = cmde.Command.getResult() as ReadTransactionDatabase_Result;
                mMainForm.AddCmdLog(cmde, $"读取成功，读取数量：{result.Quantity},实际解析数量：{result.TransactionList.Count}");

                if (result.TransactionList.Count > 0)
                {
                    StringBuilder sLogs = new StringBuilder(result.TransactionList.Count * 100);
                    sLogs.AppendLine($"事件类型：{mWatchTypeNameList[result.TransactionList[0].TransactionType]}");
                    sLogs.Append("读取计数：").Append(result.Quantity).Append("；实际数量：").Append(result.TransactionList.Count).Append("；剩余新记录数：").Append(result.readable).AppendLine();

                    //按序号排序
                    result.TransactionList.Sort((x, y) => x.SerialNumber.CompareTo(y.SerialNumber));
                    foreach (var t in result.TransactionList)
                    {
                        PrintTransactionList(t, sLogs);
                    }
                    string sFile = SaveFile(sLogs, $"读取记录_{DateTime.Now:yyyyMMddHHmmss}.txt");
                    mMainForm.AddCmdLog(cmde, $"记录在保存文件：{sFile}");
                }
            };
        }

        private void PrintTransactionList(AbstractTransaction tr, StringBuilder sLogs)
        {
            sLogs.Append("序号：").Append(tr.SerialNumber.ToString());
            if (tr.IsNull())
            {
                sLogs.AppendLine(" --- 空记录");
                return;
            }
            sLogs.Append("，时间：").Append(tr.TransactionDate.ToDateTimeStr());
            sLogs.Append("，事件代码：").Append(tr.TransactionCode);
            if (tr.TransactionType == 1)
            {
                CardTransaction cardTrans = tr as CardTransaction;
                sLogs.Append("卡号：").Append(cardTrans.CardData).Append("，卡类：").Append(cardTrans.CardType).Append("，折扣：").Append(cardTrans.Discount.ToString());
                sLogs.Append("，现金余额：").Append(cardTrans.POSMoneyTotal).Append("，现金账户：").Append(cardTrans.POSMoney).Append("，消费餐段：").Append(cardTrans.POSTimeGroup.ToString());
                sLogs.Append("，机器类型：").Append(mPOSWorkMode[cardTrans.POSWorkMode]).Append("，消费次序：").Append(cardTrans.POSSerialNumber.ToString()).Append("，补贴余额：").AppendLine(cardTrans.POSSubsidyMoneyTotal.ToString());
            }
            else
            {
                string[] codeNameList = mTransactionCodeNameList[tr.TransactionType];
                sLogs.Append("(").Append(codeNameList[tr.TransactionCode]).AppendLine(")");
            }
        }

        private void butReadTransactionDatabaseByIndex_Click(object sender, EventArgs e)
        {
            int type = cmbTransactionDatabaseType.SelectedIndex;
            int Quantity = int.Parse(txtReadTransactionQuantity.Text);

            int index = Convert.ToInt32(txtReadIndex.Value);

            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 1000;
            cmdDtl.RestartCount = 20;

            var par = new ReadTransactionDatabaseByIndex_Parameter((int)Get_TransactionDatabaseType(type), index, Quantity);
          
            var cmd = new ReadTransactionDatabaseByIndex(cmdDtl, par);
            cmdDtl.Timeout = 4000;
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {

                var result = cmde.Command.getResult() as ReadTransactionDatabaseByIndex_Result;
                mMainForm.AddCmdLog(cmde, $"读取成功，读取数量：{result.Quantity},实际解析数量：{result.TransactionList.Count}");

                if (result.TransactionList.Count > 0)
                {
                    StringBuilder sLogs = new StringBuilder(result.TransactionList.Count * 100);
                    sLogs.AppendLine($"事件类型：{mWatchTypeNameList[result.TransactionList[0].TransactionType]}");
                    //sLogs.Append("读取计数：").Append(result.Quantity).Append("；实际数量：").Append(result.TransactionList.Count).Append("；剩余新记录数：").Append(result.readable).AppendLine();

                    //按序号排序
                    result.TransactionList.Sort((x, y) => x.SerialNumber.CompareTo(y.SerialNumber));
                    foreach (var t in result.TransactionList)
                    {
                        PrintTransactionList(t, sLogs);
                    }
                    string sFile = SaveFile(sLogs, $"读取记录_{DateTime.Now:yyyyMMddHHmmss}.txt");
                    mMainForm.AddCmdLog(cmde, $"记录在保存文件：{sFile}");
                }
            };
        }

        public static string SaveFile(StringBuilder sLogs, string sFileName)
        {
            string sPath = System.IO.Path.Combine(Application.StartupPath, "记录日志");
            if (!System.IO.Directory.Exists(sPath))
                System.IO.Directory.CreateDirectory(sPath);

            string sFile = System.IO.Path.Combine(sPath, sFileName);

            System.IO.File.WriteAllText(sFile, sLogs.ToString(), Encoding.UTF8);
            return sFile;
        }
    }
}
