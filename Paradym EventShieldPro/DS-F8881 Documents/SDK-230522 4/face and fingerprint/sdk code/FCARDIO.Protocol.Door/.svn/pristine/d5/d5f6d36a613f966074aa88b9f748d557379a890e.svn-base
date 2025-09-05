using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data;
using DoNetDrive.Protocol.Door.Test.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using DoNetDrive.Common.Extensions;
using System.Linq;

namespace DoNetDrive.Protocol.Door.Test
{
    public partial class frmCard : frmNodeForm
    {
        private static object lockobj = new object();
        private static frmCard onlyObj;


        BindingList<CardDetail_UI> CardList = null;
        /// <summary>
        /// 卡号字典
        /// </summary>
        HashSet<UInt64> CardHashTable = null;

        public static frmCard GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmCard(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private frmCard(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }
        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            GetLanguage(groupBox1);
            GetLanguage(label1);
            GetLanguage(butCardDatabaseDetail);
            GetLanguage(button2);
            GetLanguage(butClearCardDataBase);
            GetLanguage(label2);
            GetLanguage(butCardListBySort);
            GetLanguage(butCardListBySequence);
            GetLanguage(butClearGrid);
            GetLanguage(dgCardList);
            GetLanguage(chkSelectAll);
            GetLanguage(checkBox7);
            GetLanguage(tabPage1);
            GetLanguage(label3);
            GetLanguage(label4);
            GetLanguage(label5);
            GetLanguage(label6);
            GetLanguage(label10);
            GetLanguage(label11);
            GetLanguage(label12);
            GetLanguage(label13);
            GetLanguage(label14);
            GetLanguage(label7);
            GetLanguage(label8);
            GetLanguage(label9);
            var door = GetLanguage("door");
            chkDoor1.Text = door + 1;
            chkDoor2.Text = door + 2;
            chkDoor3.Text = door + 3;
            chkDoor4.Text = door + 4;
            GetLanguage(cbHolidayUse);
            GetLanguage(button7);
            GetLanguage(butInsertList);
            GetLanguage(butDelList);
            GetLanguage(btnAddDevice);
            GetLanguage(butReadCardDetail);
            GetLanguage(btnDelDevice);
            GetLanguage(btnDelSelect);
            GetLanguage(tabPage3);
            GetLanguage(label15);
            GetLanguage(butCreateCardNumByRandom);
            GetLanguage(butCreateCardNumByOrder);
            GetLanguage(button13);
            GetLanguage(button14);
            GetLanguage(tabPage4);

            //操作类型
            cmbcardType.Items.Clear();
            cmbcardType.Items.AddRange(GetLanguage("cardType").Split(','));
            cmbcardType.SelectedIndex = 0;
            dgCardList.AutoGenerateColumns = false;
            CardList = new BindingList<CardDetail_UI>();
            CardHashTable = new HashSet<ulong>();
            dgCardList.DataSource = CardList;

            IniControl();
        }
        private void frmCard_Load(object sender, EventArgs e)
        {
            LoadUILanguage();
        }

        #region 读取授权卡存储信息
        private void button1_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            INCommand cmd;

            cmd = new Door8800.Card.ReadCardDatabaseDetail(cmdDtl);

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door8800.Card.ReadCardDatabaseDetail_Result;
                mMainForm.AddCmdLog(cmde, $"{GetLanguage("msg1")}{result.SortDataBaseSize.ToString()},{GetLanguage("msg2")}:{result.SortCardSize.ToString()}");
                mMainForm.AddCmdLog(cmde, $"{GetLanguage("msg3")}{result.SequenceDataBaseSize.ToString()},{GetLanguage("msg2")}:{result.SequenceCardSize.ToString()}");
            };
        }
        #endregion

        #region 读取所有授权卡
        public bool CheckProtocolTypeIs89H()
        {
            return mMainForm.CheckProtocolTypeIs89H();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            INCommand cmd;
            var par = new Door8800.Card.ReadCardDataBase_Parameter(cmbcardType.SelectedIndex + 1);

            if (CheckProtocolTypeIs89H())
            {
                cmd = new Door89H.Card.ReadCardDataBase(cmdDtl, par);
            }
            else
            {
                cmd = new Door8800.Card.ReadCardDataBase(cmdDtl, par);
            }
            CardList.Clear();
            CardHashTable.Clear();
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {

                Invoke(() => ReadCardDataBaseCallBlack(cmde));
            };
        }

        /// <summary>
        /// 读取卡数据库命令执行完毕，回调
        /// </summary>
        /// <param name="cmde"></param>
        private void ReadCardDataBaseCallBlack(CommandEventArgs cmde)
        {
            int iReadCount = 0;
            int iType = 0;
            string[] DBTypes = GetLanguage("cardType").Split(',');


            CardList.RaiseListChangedEvents = false;
            CardList.Clear();
            CardHashTable.Clear();
            StringBuilder sLogs = new StringBuilder();
            var result = cmde.Result as Door8800.Card.ReadCardDataBase_Result;
            if (result != null)
            {
                iType = result.CardType;
                iReadCount = result.DataBaseSize;
                if (iReadCount > 0)
                {
                    sLogs.AppendLine($"{GetLanguage("msg4")}:{iReadCount},{GetLanguage("msg5")}:{DBTypes[iType - 1]}");
                    sLogs.Capacity = result.CardList.Count * 100;
                    //Door8800的卡号
                    foreach (var c in result.CardList)
                    {
                        AddCardDataBaseToList(c);
                        DebugCardDetail(sLogs, c);

                    }
                }
            }
            else
            {
                var fc89Result = cmde.Result as Door89H.Card.ReadCardDataBase_Result;
                iType = fc89Result.CardType;
                iReadCount = fc89Result.DataBaseSize;
                if (iReadCount > 0)
                {
                    sLogs.AppendLine($"{GetLanguage("msg4")}:{iReadCount},{GetLanguage("msg5")}:{DBTypes[iType - 1]}");
                    sLogs.Capacity = fc89Result.CardList.Count * 100;
                    //Door89H的卡号
                    foreach (var c in fc89Result.CardList)
                    {
                        AddCardDataBaseToList(c);
                        DebugCardDetail(sLogs, c);
                    }
                }
            }

            CardList.RaiseListChangedEvents = true;
            CardList.ResetBindings();


            mMainForm.AddCmdLog(cmde, $"{GetLanguage("msg4")}:{iReadCount},{GetLanguage("msg5")}:{DBTypes[iType - 1]}");
            if (sLogs.Length > 0)
            {
                string sFile = SaveFile(sLogs, $"{GetLanguage("msg6")}{DateTime.Now:yyyyMMddHHmmss}.txt");
                mMainForm.AddCmdLog(cmde, $"{GetLanguage("msg7")}:{sFile}");
            }

        }
        public string SaveFile(StringBuilder sLogs, string sFileName)
        {
            string sPath = System.IO.Path.Combine(Application.StartupPath, GetLanguage("msg34"));
            if (!System.IO.Directory.Exists(sPath))
                System.IO.Directory.CreateDirectory(sPath);

            string sFile = System.IO.Path.Combine(sPath, $"{GetLanguage("msg9")}{DateTime.Now:yyyyMMddHHmmss}.txt");

            System.IO.File.WriteAllText(sFile, sLogs.ToString(), Encoding.UTF8);
            return sFile;
        }

        /// <summary>
        /// 将一个卡详情添加到系统缓冲中
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        private bool AddCardDataBaseToList(CardDetailBase card)
        {

            if (!CardHashTable.Contains(card.CardData))
            {
                var ui = new CardDetail_UI(card);
                ui.CardIndex = (CardList.Count + 1).ToString();
                CardList.Add(ui);
                CardHashTable.Add(card.CardData);
                return true;
            }
            return false;
        }

        private void DebugCardDetail(StringBuilder sLogs, CardDetailBase card)
        {
            var ui = new CardDetail_UI(card);
            DebugCardDetail(card, sLogs);
            sLogs.AppendLine();
        }
        #endregion

        /// <summary>
        /// 表格的行单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgCardList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex, row = e.RowIndex;
            if (row < 0) return;
            var gdRow = dgCardList.Rows[row];
            var CardUI = gdRow.DataBoundItem as CardDetail_UI;
            StringBuilder strBuf = new StringBuilder();

            DebugCardDetail(CardUI.CardDetail, strBuf);
            txtDebug.Text = strBuf.ToString();
            CardDetailToControl(CardUI.CardDetail);
        }

        #region 清空授权卡
        private void butClearCardDataBase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new Door8800.Card.ClearCardDataBase_Parameter(cmbcardType.SelectedIndex + 1);
            var cmd = new Door8800.Card.ClearCardDataBase(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 上传至非排列区
        private void butCardListBySequence_Click(object sender, EventArgs e)
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 15000;
            cmdDtl.RestartCount = 0;
            INCommand cmd = null;


            if (CheckProtocolTypeIs89H())
            {

                List<Door89H.Data.CardDetail> cards = new List<Door89H.Data.CardDetail>();
                foreach (var item in CardList)
                {
                    cards.Add(new Door89H.Data.CardDetail(item.CardDetail));
                }
                if (cards.Count > 0)
                {
                    var par = new Door89H.Card.WriteCardListBySequence_Parameter(cards);
                    cmd = new Door89H.Card.WriteCardListBySequence(cmdDtl, par);
                }
            }
            else
            {
                List<Door8800.Data.CardDetail> cards = new List<CardDetail>();
                foreach (var item in CardList)
                {
                    if (item.CardDetail.CardData < UInt32.MaxValue)
                    {
                        cards.Add(new Door8800.Data.CardDetail(item.CardDetail));
                    }
                }
                if (cards.Count > 0)
                {
                    var par = new Door8800.Card.WriteCardListBySequence_Parameter(cards);
                    cmd = new Door8800.Card.WriteCardListBySequence(cmdDtl, par);
                }
            }
            if (cmd == null) return;

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door8800.Card.WriteCardList_Result;
                WriteCardCallBlack(cmde, result);
            };
        }
        #endregion

        #region 上传至排列区
        private void butCardListBySort_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 5000;
            INCommand cmd = null;

            if (CheckProtocolTypeIs89H())
            {

                List<Door89H.Data.CardDetail> cards = new List<Door89H.Data.CardDetail>();
                foreach (var item in CardList)
                {
                    cards.Add(new Door89H.Data.CardDetail(item.CardDetail));
                }
                if (cards.Count > 0)
                {

                    var par = new Door89H.Card.WriteCardListBySort_Parameter(cards);
                    cmd = new Door89H.Card.WriteCardListBySort(cmdDtl, par);
                }
            }
            else
            {
                List<Door8800.Data.CardDetail> cards = new List<CardDetail>();
                foreach (var item in CardList)
                {
                    if (item.CardDetail.CardData < UInt32.MaxValue)
                    {
                        cards.Add(new Door8800.Data.CardDetail(item.CardDetail));
                    }
                }
                if (cards.Count > 0)
                {
                    var par = new Door8800.Card.WriteCardListBySort_Parameter(cards);
                    cmd = new Door8800.Card.WriteCardListBySort(cmdDtl, par);
                }

            }
            if (cmd == null) return;

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door8800.Card.WriteCardList_Result;
                WriteCardCallBlack(cmde, result);
            };
        }


        private void WriteCardCallBlack(CommandEventArgs cmde, Door8800.Card.WriteCardList_Result result)
        {
            if (result != null)
            {
                mMainForm.AddCmdLog(cmde, $"{GetLanguage("msg8")}:{result.FailTotal}");
                if (result.FailTotal > 0)
                {
                    StringBuilder strBuf = new StringBuilder();
                    foreach (var item in result.CardList)
                    {
                        strBuf.Append(item.ToString("00000000000000000000")).Append("(0x").Append(item.ToString("X18")).Append(")");
                    }
                    txtDebug.Text = strBuf.ToString();
                    System.IO.File.WriteAllText(System.IO.Path.Combine(Application.StartupPath, $"{GetLanguage("msg9")}.txt"), strBuf.ToString(), Encoding.UTF8);
                }
            }
        }
        #endregion


        #region 读取单个卡片在控制器中的信息

        /// <summary>
        /// 从设备读取单张卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butReadCardDetail_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            INCommand cmd;
            UInt64 card = 0;
            if (!UInt64.TryParse(txtCardData.Text, out card))
            {
                MsgErr(GetLanguage("msg10"));
                return;
            }


            try
            {
                if (CheckProtocolTypeIs89H())
                {
                    var par = new Door89H.Card.ReadCardDetail_Parameter(card);
                    cmd = new Door89H.Card.ReadCardDetail(cmdDtl, par);
                }
                else
                {
                    var par = new Door8800.Card.ReadCardDetail_Parameter((uint)card);
                    cmd = new Door8800.Card.ReadCardDetail(cmdDtl, par);
                }
            }
            catch (Exception createcarderr)
            {
                MsgErr(createcarderr.Message);
                return;
            }

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                Invoke(() => ReadCardDetailCallBlack(cmde));
            };
        }

        /// <summary>
        /// 读取卡片详情回调
        /// </summary>
        /// <param name="cmde"></param>
        private void ReadCardDetailCallBlack(CommandEventArgs cmde)
        {
            StringBuilder strBuf = new StringBuilder();
            var result = cmde.Result as Door8800.Card.ReadCardDetail_Result;
            if (result != null)
            {
                if (!result.IsReady)
                {
                    mMainForm.AddCmdLog(cmde, GetLanguage("msg11"));
                    return;
                }
                else
                {
                    DebugCardDetail(result.Card, strBuf);
                    CardDetailToControl(result.Card);
                }
            }
            else
            {
                var fc89Result = cmde.Result as Door89H.Card.ReadCardDetail_Result;
                if (!fc89Result.IsReady)
                {
                    mMainForm.AddCmdLog(cmde, GetLanguage("msg11"));
                    return;
                }
                else
                {
                    DebugCardDetail(fc89Result.Card, strBuf);
                    CardDetailToControl(fc89Result.Card);
                }

            }
            txtDebug.Text = strBuf.ToString();

            mMainForm.AddCmdLog(cmde, GetLanguage("msg11"));
        }
        /// <summary>
        /// 将卡片输出到buf中
        /// </summary>
        /// <param name="card"></param>
        private StringBuilder DebugCardDetail(CardDetailBase card, StringBuilder strBuf)
        {
            CardDetail_UI ui = new CardDetail_UI(card);
            strBuf.Append(GetLanguage("msg13")).Append(ui.CardData).Append("；" + GetLanguage("msg14")).Append(ui.Password);
            strBuf.Append("；" + GetLanguage("msg15")).Append(ui.Expiry).Append("；" + GetLanguage("msg16")).Append(ui.OpenTimes).AppendLine("；");
            strBuf.Append(GetLanguage("msg22")).Append(ui.doorAccess).Append("；" + GetLanguage("msg23")).Append(ui.TimeGroup);
            strBuf.Append("；" + GetLanguage("msg17")).Append(ui.CardStatus).Append("；" + GetLanguage("msg18")).AppendLine(ui.Privilege);
            strBuf.Append(GetLanguage("msg19")).Append(ui.Holiday).Append("(1->32)");
            strBuf.Append("；" + GetLanguage("msg20")).Append(ui.EnterStatus);
            strBuf.Append("；" + GetLanguage("msg21")).AppendLine(ui.ReadCardDate);
            return strBuf;
        }

        /// <summary>
        /// 将卡片输出到控件中
        /// </summary>
        /// <param name="card"></param>
        private void CardDetailToControl(CardDetailBase card)
        {
            CardDetail_UI ui = new CardDetail_UI(card);
            txtCardData16.Text = card.CardData.ToString("X");
            txtCardData.Text = card.CardData.ToString();
            txtPassword.Text = ui.Password;
            dtpDate.Value = card.Expiry;
            dtpTime.Value = card.Expiry;
            cmbOpenTimes.Text = ui.OpenTimes;
            if (card.CardStatus < 4)
                cmbCardStatus.SelectedIndex = card.CardStatus;
            CheckBox[] DoorList = { chkDoor1, chkDoor2, chkDoor3, chkDoor4 };
            ComboBox[] TGList = { cmbTimeGroup1, cmbTimeGroup2, cmbTimeGroup3, cmbTimeGroup4 };
            ComboBox[] EnterStatusList = { cmbEnterStatus1, cmbEnterStatus2, cmbEnterStatus3, cmbEnterStatus4 };
            int i;
            for (i = 1; i <= 4; i++)
            {
                //门权限
                DoorList[i - 1].Checked = card.GetDoor(i);
                //开门时段
                if (card.GetTimeGroup(i) < 65)
                    TGList[i - 1].SelectedIndex = card.GetTimeGroup(i) - 1;
                //出入状态
                int value = card.GetEnterStatusValue(i);
                if (value == 0 || value == 3)
                    EnterStatusList[i - 1].SelectedIndex = 0;
                if (value == 1 || value == 2)
                    EnterStatusList[i - 1].SelectedIndex = value;
            }
            if (card.Privilege < 5)
                cmbPrivilege.SelectedIndex = card.Privilege;
            cbHolidayUse.Checked = card.HolidayUse;
            if (card.HolidayUse)
                txtHoliday.Text = ui.Holiday;
            else
                txtHoliday.Text = new string('1', 30);

        }
        #endregion

        #region 控件初始化

        #region 初始化控件列表
        private void IniControl()
        {
            TimeGroup();
            EnterStatus();
            OpenTimes();
            CardStatus();

        }
        #endregion

        #region 出入标记
        public void EnterStatus()
        {
            ComboBox[] EnterStatusList = { cmbEnterStatus1, cmbEnterStatus2, cmbEnterStatus3, cmbEnterStatus4 };
            string[] arr = GetLanguage("enterStatus").Split(',');
            foreach (var cmb in EnterStatusList)
            {
                cmb.Items.Clear();
                cmb.Items.AddRange(arr);
                cmb.SelectedIndex = 0;
            }
        }
        #endregion

        #region 有效次数

        public void OpenTimes()
        {
            cmbOpenTimes.Items.Clear();
            cmbOpenTimes.Items.Add(GetLanguage("msg35"));
            var temp = GetLanguage("msg24");
            string[] time = new string[100];
            for (int i = 1; i <= 100; i++)
            {
                time[i - 1] = i + temp;
            }
            cmbOpenTimes.Items.AddRange(time);
            cmbOpenTimes.Items.Add(GetLanguage("msg36"));
            cmbOpenTimes.SelectedIndex = cmbOpenTimes.Items.Count - 1;
        }
        #endregion

        #region 卡片状态
        public void CardStatus()
        {
            cmbCardStatus.Items.Clear();
            cmbCardStatus.Items.AddRange(GetLanguage("cmbCardStatus").Split(','));
            cmbCardStatus.SelectedIndex = 0;

            cmbPrivilege.Items.Clear();
            cmbPrivilege.Items.AddRange(GetLanguage("cmbPrivilege").Split(','));
            cmbPrivilege.SelectedIndex = 0;
        }
        #endregion

        #region 开门时段
        public void TimeGroup()
        {
            string[] time = new string[64];
            ComboBox[] TGList = { cmbTimeGroup1, cmbTimeGroup2, cmbTimeGroup3, cmbTimeGroup4 };
            var temp = GetLanguage("msg25");
            for (int i = 0; i < 64; i++)
            {
                time[i] = temp + (i + 1);
            }

            foreach (var tgb in TGList)
            {
                tgb.Items.Clear();
                tgb.Items.AddRange(time);
                tgb.SelectedIndex = 0;
            }
        }
        #endregion
        #endregion

        #region 添加卡信息
        /// <summary>
        /// 将控件中的值转换为卡详情
        /// </summary>
        /// <returns></returns>
        private CardDetailBase ContorlToCardDetail()
        {
            Door89H.Data.CardDetail card = new Door89H.Data.CardDetail();
            string CardStr = txtCardData.Text;
            card.BigCard.BigValue = decimal.Parse(CardStr);
            if (card.BigCard.BigValue == 0)
            {
                MsgErr(GetLanguage("msg26"));
                return null;
            }
            string sPwd = txtPassword.Text;
            if (!string.IsNullOrEmpty(sPwd))
            {
                if (!sPwd.IsNum())
                {
                    MsgErr(GetLanguage("msg27"));
                    return null;
                }
                if (sPwd.Length < 4)
                {
                    MsgErr(GetLanguage("msg28"));
                    return null;
                }
                card.Password = sPwd.FillString(8, 'F');
            }
            //有效期
            var dtpD = dtpDate.Value; var dtpT = dtpTime.Value;
            card.Expiry = new DateTime(dtpD.Year, dtpD.Month, dtpD.Day, dtpT.Hour, dtpT.Minute, 59);
            //有效次数
            if (cmbOpenTimes.Text == CardDetail_UI.OpenTimes_Invalid)
                card.OpenTimes = 0;
            else
            {
                if (cmbOpenTimes.Text == CardDetail_UI.OpenTimes_Off)
                    card.OpenTimes = 65535;
                else
                {
                    string sTimes = cmbOpenTimes.Text.Replace(GetLanguage("msg24"), string.Empty).Trim();
                    if (sTimes.IsNum())
                    {
                        card.OpenTimes = sTimes.ToInt32();
                    }
                }
            }

            //卡状态
            card.CardStatus = (byte)cmbCardStatus.SelectedIndex;

            CheckBox[] DoorList = { chkDoor1, chkDoor2, chkDoor3, chkDoor4 };
            ComboBox[] TGList = { cmbTimeGroup1, cmbTimeGroup2, cmbTimeGroup3, cmbTimeGroup4 };
            ComboBox[] EnterStatusList = { cmbEnterStatus1, cmbEnterStatus2, cmbEnterStatus3, cmbEnterStatus4 };
            int i;
            for (i = 1; i <= 4; i++)
            {
                //门权限
                card.SetDoor(i, DoorList[i - 1].Checked);
                //开门时段
                card.SetTimeGroup(i, TGList[i - 1].SelectedIndex + 1);
                //出入状态
                int value = EnterStatusList[i - 1].SelectedIndex;
                card.SetEnterStatusValue(i, value);
            }
            card.Privilege = cmbPrivilege.SelectedIndex;

            card.HolidayUse = cbHolidayUse.Checked;
            //节假日
            if (card.HolidayUse)
            {
                string sHol = txtHoliday.Text.Trim();
                sHol.FillString(32, '0');
                var chars = sHol.ToCharArray();
                for (i = 0; i < 32; i++)
                {
                    card.SetHolidayValue(i + 1, chars[i] == '1');
                }
            }
            else
            {
                for (i = 0; i < 4; i++)
                {
                    card.Holiday[i] = 0;
                }
            }
            return card;
        }
        private CardDetailBase AddCardDetailToList()
        {
            CardDetailBase card = ContorlToCardDetail();
            if (card == null) return null;
            //检查卡片是否已存在
            if (CardHashTable.Contains(card.CardData))
            {
                int iCount = CardList.Count;
                for (int i = 0; i < iCount; i++)
                {
                    if (CardList[i].CardDetail.CardData == card.CardData)
                    {
                        CardList[i].CardDetail = card;
                        CardList.ResetItem(i);
                        break;
                    }
                }

            }
            else
            {
                CardHashTable.Add(card.CardData);
                CardList.Add(new CardDetail_UI(card));

            }
            return card;
        }

        /// <summary>
        /// 新增至列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butInsertList_Click(object sender, EventArgs e)
        {
            CardDetailBase card = AddCardDetailToList();

        }

        /// <summary>
        /// 增加至设备 写入非排序区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddDevice_Click(object sender, EventArgs e)
        {
            CardDetailBase card = AddCardDetailToList();
            if (card == null) return;

            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 10000;
            INCommand cmd;

            if (CheckProtocolTypeIs89H())
            {

                List<Door89H.Data.CardDetail> cards = new List<Door89H.Data.CardDetail>();
                cards.Add((Door89H.Data.CardDetail)card);
                var par = new Door89H.Card.WriteCardListBySequence_Parameter(cards);
                cmd = new Door89H.Card.WriteCardListBySequence(cmdDtl, par);
            }
            else
            {
                List<Door8800.Data.CardDetail> cards = new List<CardDetail>();
                cards.Add(new Door8800.Data.CardDetail(card));
                var par = new Door8800.Card.WriteCardListBySequence_Parameter(cards);
                cmd = new Door8800.Card.WriteCardListBySequence(cmdDtl, par);
            }


            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door8800.Card.WriteCardList_Result;
                WriteCardCallBlack(cmde, result);
            };
        }

        #endregion

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool bSelect = chkSelectAll.Checked;
            foreach (var item in CardList)
            {
                item.Selected = bSelect;
            }
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butClearGrid_Click(object sender, EventArgs e)
        {
            CardList.Clear();
            CardHashTable.Clear();
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
                MessageBox.Show(GetLanguage("msg29"));
                return 0;
            }
            if (iCreateCount > 32000)
            {
                MessageBox.Show(GetLanguage("msg29"));
                return 0;
            }
            if ((iCreateCount + CardList.Count) > 32000)
            {
                iCreateCount = 32000 - CardList.Count;

            }
            if (iCreateCount <= 0) return 0;

            return iCreateCount;
        }

        /// <summary>
        /// 生成随机卡 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butCreateCardNumByRandom_Click(object sender, EventArgs e)
        {
            int iCreateCount = CheckCreateCardCount();
            if (iCreateCount <= 0) return;

            CommandDetailFactory.ControllerType iType = mMainForm.GetProtocolType();
            CardList.RaiseListChangedEvents = false;
            CardDetailBase card;
            for (int i = 0; i < iCreateCount; i++)
            {

                card = CreateNewCardDetail(iType, 0);
                AddCardDataBaseToList(card);

            }
            CardList.RaiseListChangedEvents = true;
            CardList.ResetBindings();
        }

        /// <summary>
        /// 生成顺序卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butCreateCardNumByOrder_Click(object sender, EventArgs e)
        {
            int iCreateCount = CheckCreateCardCount();
            if (iCreateCount <= 0) return;

            string sBeginNum = frmInputBox.ShowBox(GetLanguage("msg31"), GetLanguage("msg32"), "1", 10);
            UInt64 iBeginNum = 0;
            if (!UInt64.TryParse(sBeginNum, out iBeginNum))
            {
                return;
            }
            if (iBeginNum <= 0) iBeginNum = 1;

            CardDetailBase card;
            CommandDetailFactory.ControllerType iType = mMainForm.GetProtocolType();

            CardList.RaiseListChangedEvents = false;
            while (iCreateCount > 0)
            {
                card = CreateNewCardDetail(iType, ++iBeginNum);
                if (card != null)
                {
                    AddCardDataBaseToList(card);

                    iCreateCount--;
                }

            }
            CardList.RaiseListChangedEvents = true;
            CardList.ResetBindings();

        }


        private Random mCardRnd = new Random();
        private int mCardMax = 0x6FFFFFFF;
        private int mCardMin = 0x10000000;
        /// <summary>
        /// 创建一个不重复的卡
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="iCardNum"></param>
        /// <returns></returns>
        private CardDetailBase CreateNewCardDetail(CommandDetailFactory.ControllerType iType, UInt64 iCardNum)
        {
            UInt64 cardNum = 0, cardNum2 = 0;
            CardDetailBase card;
            if (iCardNum == 0)
            {
                cardNum = (UInt64)(mCardRnd.Next(mCardMax) % (mCardMax - mCardMin + 1) + mCardMin);
                if (CheckProtocolTypeIs89H())
                {
                    cardNum2 = (UInt64)(mCardRnd.Next(mCardMax) % (mCardMax - mCardMin + 1) + mCardMin);
                    cardNum = (cardNum << 32) + cardNum2;
                }


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
                    return CreateNewCardDetail(iType, 0);
                }
                else
                {
                    return null;
                }

            }


            if (CheckProtocolTypeIs89H())
            {
                var fc89HCard = new Door89H.Data.CardDetail();
                card = fc89HCard;
                fc89HCard.BigCard.BigValue = (uint)cardNum;
            }
            else
            {
                card = new Door8800.Data.CardDetail();
                card.CardData = (uint)cardNum;
            }

            return card;
        }

        /// <summary>
        /// 从列表删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButDelList_Click(object sender, EventArgs e)
        {
            var lst = CardList.Where(t => t.Selected == false).ToArray();

            if (lst.Length == 0)
                return;

            CardList.RaiseListChangedEvents = false;
            CardList.Clear();
            CardHashTable.Clear();
            foreach (var c in lst)
            {
                CardList.Add(c);
                CardHashTable.Add(c.CardDetail.CardData);
            }
            CardList.RaiseListChangedEvents = true;
            CardList.ResetBindings();
        }

        /// <summary>
        /// 从设备删除单张卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 15000;
            cmdDtl.RestartCount = 0;
            INCommand cmd = null;
            CardDetailBase card = ContorlToCardDetail();
            if (card == null) return;
            if (CheckProtocolTypeIs89H())
            {

                List<Door89H.Data.CardDetail> cards = new List<Door89H.Data.CardDetail>();
                cards.Add((Door89H.Data.CardDetail)card);
                var par = new Door89H.Card.DeleteCard_Parameter(cards);
                cmd = new Door89H.Card.DeleteCard(cmdDtl, par);

            }
            else
            {
                List<Door8800.Data.CardDetail> cards = new List<CardDetail>();
                cards.Add(new Door8800.Data.CardDetail(card));
                var par = new Door8800.Card.DeleteCard_Parameter(cards);
                cmd = new Door8800.Card.DeleteCard(cmdDtl, par);
            }

            mMainForm.AddCommand(cmd);
        }
        /// <summary>
        /// 从设备删除选中卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelSelect_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 15000;
            cmdDtl.RestartCount = 0;
            INCommand cmd = null;

            if (CheckProtocolTypeIs89H())
            {

                List<Door89H.Data.CardDetail> cards = new List<Door89H.Data.CardDetail>();
                foreach (var item in CardList)
                {
                    if (item.Selected)
                        cards.Add(new Door89H.Data.CardDetail(item.CardDetail));
                }
                if (cards.Count > 0)
                {
                    var par = new Door89H.Card.DeleteCard_Parameter(cards);
                    cmd = new Door89H.Card.DeleteCard(cmdDtl, par);
                }

            }
            else
            {
                List<Door8800.Data.CardDetail> cards = new List<CardDetail>();
                foreach (var item in CardList)
                {
                    if (item.Selected)
                    {
                        if (item.CardDetail.CardData < UInt32.MaxValue)
                        {
                            cards.Add(new Door8800.Data.CardDetail(item.CardDetail));
                        }
                    }
                }
                if (cards.Count > 0)
                {
                    var par = new Door8800.Card.DeleteCard_Parameter(cards);
                    cmd = new Door8800.Card.DeleteCard(cmdDtl, par);
                }
            }
            if (cmd == null)
            {
                MsgErr(GetLanguage("msg33"));
                return;
            }


            mMainForm.AddCommand(cmd);
        }



        #region 卡片转换
        private bool mCardChanging;
        private void TxtCardData_TextChanged(object sender, EventArgs e)
        {
            if (mCardChanging) return;
            mCardChanging = true;
            string sCard = txtCardData.Text.Trim();
            if (!sCard.IsNum())
            {
                txtCardData.Text = "0";
                txtCardData16.Text = "00";
                mCardChanging = false;
                return;
            }
            UInt64 card = sCard.ToUInt64();

            txtCardData16.Text = card.ToString("X16");
            mCardChanging = false;
        }

        private void TxtCardData16_TextChanged(object sender, EventArgs e)
        {
            if (mCardChanging) return;
            mCardChanging = true;
            string sCardHex = txtCardData16.Text.Trim();
            if (!sCardHex.IsHex())
            {
                txtCardData.Text = "0";
                txtCardData16.Text = "00";
                mCardChanging = false;
                return;
            }
            UInt64 card = sCardHex.HexToUInt64();


            txtCardData.Text = card.ToString("d20");
            mCardChanging = false;
        }


        #endregion

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
