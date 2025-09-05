using DoNetDrive.Common.Extensions;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Elevator.FC8864.Card.CardDataBase;
using DoNetDrive.Protocol.Elevator.FC8864.Card.CardDatabaseDetail;
using DoNetDrive.Protocol.Elevator.FC8864.Card.ClearCardDataBase;
using DoNetDrive.Protocol.Elevator.FC8864.Card.DeleteCard;
using DoNetDrive.Protocol.Elevator.FC8864.Data;
using DoNetDrive.Protocol.Elevator.Test.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.Elevator.Test
{
    public partial class frmCard : frmNodeForm
    {
        private static object lockobj = new object();
        private static frmCard onlyObj;


        List<CardDetail_UI> CardList = null;
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

        INMain mMainForm;

        private frmCard(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }

        private void frmCard_Load(object sender, EventArgs e)
        {
            //操作类型
            cmbcardType.Items.Clear();
            cmbcardType.Items.AddRange(new string[] { "排序卡", "非排序卡", "所有卡" });
            cmbcardType.SelectedIndex = 0;
            dgCardList.AutoGenerateColumns = false;
            CardList = new List<CardDetail_UI>();
            CardHashTable = new HashSet<ulong>();
            //dgCardList.DataSource = CardList;

            IniControl();

        }

        #region 读取授权卡存储信息
        private void button1_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            INCommand cmd;

            cmd = new ReadCardDatabaseDetail(cmdDtl);

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadCardDatabaseDetail_Result;
                mMainForm.AddCmdLog(cmde, $"命令成功：排序数据区：容量:{result.SortDataBaseSize.ToString()},已使用数量:{result.SortCardSize.ToString()}");
                mMainForm.AddCmdLog(cmde, $"非排序存储区：容量:{result.SequenceDataBaseSize.ToString()},已使用数量:{result.SequenceCardSize.ToString()}");
            };
        }
        #endregion

        #region 读取所有授权卡
        private void button2_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new Door.Door8800.Card.ReadCardDataBase_Parameter(cmbcardType.SelectedIndex + 1);


            ReadCardDataBase cmd = new ReadCardDataBase(cmdDtl, par);
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
            string[] DBTypes = new string[] { "", "排序区", "非排序区", "所有区域" };


           // CardList.RaiseListChangedEvents = false;
            CardList.Clear();
            CardHashTable.Clear();
            dgCardList.Rows.Clear();
            StringBuilder sLogs = new StringBuilder();
            var result = cmde.Result as ReadCardDataBase_Result;
            if (result != null)
            {
                iType = result.CardType;
                iReadCount = result.DataBaseSize;
                if (iReadCount > 0)
                {
                    sLogs.AppendLine($"读取到的卡片数:{iReadCount},带读取的卡片数据类型:{DBTypes[iType]}");
                    sLogs.Capacity = result.CardList.Count * 100;
                    //Door8800的卡号
                    int i = 1;
                    foreach (var c in result.CardList)
                    {
                        AddCardDataBaseToList(c);
                        //DebugCardDetail(sLogs, c);
                        BindDataGrid( c);
                        i++;
                    }
                }
            }

            //CardList.RaiseListChangedEvents = true;
            //CardList.ResetBindings();


            mMainForm.AddCmdLog(cmde, $"读取到的卡片数:{iReadCount},带读取的卡片数据类型:{DBTypes[iType]}");
            if (sLogs.Length > 0)
            {
                //string sFile = frmRecord.SaveFile(sLogs, $"读取所有卡片_{DateTime.Now:yyyyMMddHHmmss}.txt");
                //mMainForm.AddCmdLog(cmde, $"卡片日志保存路径:{sFile}");
            }

        }

        private void BindDataGrid(CardDetailBase c)
        {
            int index = dgCardList.Rows.Add();
            dgCardList.Rows[index].Cells[1].Value = (index + 1).ToString();
            dgCardList.Rows[index].Cells[2].Value = c.CardData.ToString();
            dgCardList.Rows[index].Cells[3].Value = c.Password;
            dgCardList.Rows[index].Cells[4].Value = c.Expiry;
            dgCardList.Rows[index].Cells[5].Value = CardDetail_UI.CardStatusList[c.CardStatus];
            dgCardList.Rows[index].Cells[6].Value = CardDetail_UI.CardPrivilegeList[c.Privilege];
            dgCardList.Rows[index].Cells[7].Value = c.OpenTimes;
            int z = 0;
            for (int j = 0; j < 64; j++)
            {
                dgCardList.Rows[index].Cells[8 + z].Value = c.DoorNumList[j];
                z++;
                dgCardList.Rows[index].Cells[8 + z].Value = c.TimeGroup[j];
                z++;
            }
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

            if (col == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgCardList.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            DataGridViewTextBoxCell tbCell = dgCardList.Rows[row].Cells[2] as DataGridViewTextBoxCell;
            var CardUI = CardList.FirstOrDefault(t => t.CardData == tbCell.Value.ToString());
            /*
            var gdRow = dgCardList.Rows[row];
            var CardUI = gdRow.DataBoundItem as CardDetail_UI;
            StringBuilder strBuf = new StringBuilder();

            DebugCardDetail(CardUI.CardDetail, strBuf);
            txtDebug.Text = strBuf.ToString();
             */
            CardDetailToControl(CardUI.CardDetail);
           
        }

        #region 清空授权卡
        private void butClearCardDataBase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new ClearCardDataBase_Parameter(cmbcardType.SelectedIndex + 1);
            var cmd = new ClearCardDataBase(cmdDtl, par);
            mMainForm.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功");
            };
        }
        #endregion

        #region 上传至非排列区
        private void butCardListBySequence_Click(object sender, EventArgs e)
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 15000;
            cmdDtl.RestartCount = 0;
            INCommand cmd = null;



            List<CardDetail> cards = new List<CardDetail>();
            foreach (var item in CardList)
            {
                if (item.CardDetail.CardData < UInt32.MaxValue)
                {
                    cards.Add(new CardDetail(item.CardDetail));
                }
            }
            if (cards.Count > 0)
            {
                var par = new FC8864.Card.CardListBySequence.WriteCardListBySequence_Parameter(cards);
                cmd = new FC8864.Card.CardListBySequence.WriteCardListBySequence(cmdDtl, par);
            }

            if (cmd == null) return;

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door.Door8800.Card.WriteCardList_Result;
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


            List<CardDetail> cards = new List<CardDetail>();
            foreach (var item in CardList)
            {
                if (item.CardDetail.CardData < UInt32.MaxValue)
                {
                    cards.Add(new CardDetail(item.CardDetail));
                }
            }
            if (cards.Count > 0)
            {
                var par = new FC8864.Card.CardListBySort.WriteCardListBySort_Parameter(cards);
                cmd = new FC8864.Card.CardListBySort.WriteCardListBySort(cmdDtl, par);
            }

            if (cmd == null) return;

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door.Door8800.Card. WriteCardList_Result;
                WriteCardCallBlack(cmde, result);
            };
        }


        private void WriteCardCallBlack(CommandEventArgs cmde, Door.Door8800.Card.WriteCardList_Result result)
        {
            if (result != null)
            {
                mMainForm.AddCmdLog(cmde, $"命令成功：写入失败的卡数量:{result.FailTotal}");
                if (result.FailTotal > 0)
                {
                    StringBuilder strBuf = new StringBuilder();
                    foreach (var item in result.CardList)
                    {
                        strBuf.Append(item.ToString("00000000000000000000")).Append("(0x").Append(item.ToString("X18")).Append(")");
                    }
                    txtDebug.Text = strBuf.ToString();
                    System.IO.File.WriteAllText(System.IO.Path.Combine(Application.StartupPath, "上传失败的卡号.txt"), strBuf.ToString(), Encoding.UTF8);
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
                MsgErr("请输入正确的卡号");
                return;
            }


            try
            {

                var par = new FC8864.Card.CardDetail.ReadCardDetail_Parameter(card);
                cmd = new FC8864.Card.CardDetail.ReadCardDetail(cmdDtl, par);
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
            var result = cmde.Result as FC8864.Card.CardDetail.ReadCardDetail_Result;
            if (result != null)
            {
                if (!result.IsReady)
                {
                    mMainForm.AddCmdLog(cmde, $"此卡未注册！");
                    return;
                }
                else
                {
                    DebugCardDetail(result.Card, strBuf);
                    CardDetailToControl(result.Card);
                }
            }

            txtDebug.Text = strBuf.ToString();

            mMainForm.AddCmdLog(cmde, "卡片已注册，详情查看【卡详情描述】");
        }
        /// <summary>
        /// 将卡片输出到buf中
        /// </summary>
        /// <param name="card"></param>
        private StringBuilder DebugCardDetail(CardDetailBase card, StringBuilder strBuf)
        {
            CardDetail_UI ui = new CardDetail_UI(card);
            strBuf.Append("卡号：").Append(ui.CardData).Append("；密码：").Append(ui.Password);
            strBuf.Append("；有效期：").Append(ui.Expiry).Append("；有效次数：").Append(ui.OpenTimes).AppendLine("；");
            strBuf.Append("权限：").Append(ui.doorAccess).Append("；开门时段：").Append(ui.TimeGroup);
            strBuf.Append("；状态：").Append(ui.CardStatus).Append("；特权：").AppendLine(ui.Privilege);
            strBuf.Append("节假日：").Append(ui.Holiday).Append("(1->32)");
            strBuf.Append("；出入标志：").Append(ui.EnterStatus);
            strBuf.Append("；最近读卡时间：").AppendLine(ui.ReadCardDate);
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

            for (int i = 0; i < 64; i++)
            {
                int dgListIndex = i / 8;
                int dgvRowIndex = i % 8;
                DataGridViewCheckBoxCell cbCell = dglist[dgListIndex].Rows[dgvRowIndex].Cells[1] as DataGridViewCheckBoxCell;
                cbCell.Value = card.DoorNumList[i];

                DataGridViewComboBoxCell cbbCell = dglist[dgListIndex].Rows[dgvRowIndex].Cells[2] as DataGridViewComboBoxCell;
                cbbCell.Value = card.TimeGroup[i].ToString();
            }

            //foreach (DataGridView dgv in dglist)
            //{
            //    foreach (DataGridViewRow row in dgv.Rows)
            //    {
            //        DataGridViewTextBoxCell tbCell = row.Cells[0] as DataGridViewTextBoxCell;
                    
            //    }
            //}

            //CheckBox[] DoorList = { chkDoor1, chkDoor2, chkDoor3, chkDoor4 };
            //ComboBox[] TGList = { cmbTimeGroup1, cmbTimeGroup2, cmbTimeGroup3, cmbTimeGroup4 };
            //ComboBox[] EnterStatusList = { cmbEnterStatus1, cmbEnterStatus2, cmbEnterStatus3, cmbEnterStatus4 };
            //int i;
            //for (i = 1; i <= 4; i++)
            //{
            //    //门权限
            //    DoorList[i - 1].Checked = card.GetDoor(i);
            //    //开门时段
            //    if (card.GetTimeGroup(i) < 65)
            //        TGList[i - 1].SelectedIndex = card.GetTimeGroup(i) - 1;
            //    //出入状态
            //    int value = card.GetEnterStatusValue(i);
            //    if (value == 0 || value == 3)
            //        EnterStatusList[i - 1].SelectedIndex = 0;
            //    if (value == 1 || value == 2)
            //        EnterStatusList[i - 1].SelectedIndex = value;
            //}
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
            OpenTimes();
            CardStatus();

        }
        #endregion

        #region 有效次数

        public void OpenTimes()
        {
            cmbOpenTimes.Items.Clear();
            cmbOpenTimes.Items.Add(CardDetail_UI.OpenTimes_Invalid);

            string[] time = new string[100];
            for (int i = 1; i <= 100; i++)
            {
                time[i - 1] = i + "次";
            }
            cmbOpenTimes.Items.AddRange(time);
            cmbOpenTimes.Items.Add(CardDetail_UI.OpenTimes_Off);
            cmbOpenTimes.SelectedIndex = cmbOpenTimes.Items.Count - 1;
        }
        #endregion

        #region 卡片状态
        public void CardStatus()
        {
            cmbCardStatus.Items.Clear();
            cmbCardStatus.Items.AddRange(CardDetail_UI.CardStatusList);
            cmbCardStatus.SelectedIndex = 0;

            cmbPrivilege.Items.Clear();
            cmbPrivilege.Items.AddRange(CardDetail_UI.CardPrivilegeList);
            cmbPrivilege.SelectedIndex = 0;
        }
        #endregion

        DataGridView[] dglist;
        #region 开门时段
        public void TimeGroup()
        {
            dglist = new DataGridView[8] { dataGridView0, dataGridView1, dataGridView2, dataGridView3, dataGridView4, dataGridView5, dataGridView6, dataGridView7 };
            string[] timegrouplist = new string[65];
            timegrouplist[0] = "0";
            for (int i = 0; i < 65; i++)
            {
                timegrouplist[i] = (i).ToString();
                DataGridViewCheckBoxColumn cbc = new DataGridViewCheckBoxColumn();
                if (i != 64)
                {
                    cbc.HeaderText = (i + 1).ToString().PadLeft(2, '0');
                }
                else
                {
                    cbc.HeaderText = "代按";
                }
               
                cbc.Width = 40;
                cbc.TrueValue = 1;
                cbc.FalseValue = 0;
                dgCardList.Columns.Add(cbc);

                DataGridViewTextBoxColumn tbc = new DataGridViewTextBoxColumn();
                tbc.HeaderText = "时段";
                tbc.Width = 40;
                dgCardList.Columns.Add(tbc);
            }


            for (int i = 0; i < dglist.Length; i++)
            {
                DataGridViewComboBoxColumn cbc = dglist[i].Columns[2] as DataGridViewComboBoxColumn;
                cbc.Items.AddRange(timegrouplist);
            }


            for (int j = 0; j < dglist.Length; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    int index = dglist[j].Rows.Add();
                    DataGridViewTextBoxCell txtcell = dglist[j].Rows[index].Cells[0] as DataGridViewTextBoxCell;
                    txtcell.Value = (j * 8 + i + 1).ToString();

                    //DataGridViewComboBoxCell cbcell = dglist[j].Rows[index].Cells[2] as DataGridViewComboBoxCell;
                    //cbcell.Value = "1";
                }

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
            CardDetail card = new CardDetail();
            card.TimeGroup = new byte[65];
            card.DoorNumList = new byte[65];
            for (int x = 0; x < 65; x++)
            {
                card.TimeGroup[x] = 0;
                card.DoorNumList[x] = 0;
            }
            string CardStr = txtCardData.Text;
            card.CardData = CardStr.ToUInt32();
            if (card.CardData == 0)
            {
                MsgErr("卡号输入不正确！");
                return null;
            }
            string sPwd = txtPassword.Text;
            if (!string.IsNullOrEmpty(sPwd))
            {
                if (!sPwd.IsNum())
                {
                    MsgErr("个人密码必须输入数字！");
                    return null;
                }
                if (sPwd.Length < 4)
                {
                    MsgErr("个人密码请输入 4-8个数字！");
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
                    string sTimes = cmbOpenTimes.Text.Replace("次", string.Empty).Trim();
                    if (sTimes.IsNum())
                    {
                        card.OpenTimes = sTimes.ToInt32();
                    }
                }
            }

            //卡状态
            card.CardStatus = (byte)cmbCardStatus.SelectedIndex;

            for (int x = 0; x < 64; x++)
            {
                int dgListIndex = x / 8;
                int dgvRowIndex = x % 8;
                DataGridViewCheckBoxCell cbCell = dglist[dgListIndex].Rows[dgvRowIndex].Cells[1] as DataGridViewCheckBoxCell;
                card.DoorNumList[x] = Convert.ToByte(cbCell.Value);

                DataGridViewComboBoxCell cbbCell = dglist[dgListIndex].Rows[dgvRowIndex].Cells[2] as DataGridViewComboBoxCell;
                card.TimeGroup[x] = Convert.ToByte(cbbCell.Value);
            }

            //CheckBox[] DoorList = { chkDoor1, chkDoor2, chkDoor3, chkDoor4 };
            //ComboBox[] TGList = { cmbTimeGroup1, cmbTimeGroup2, cmbTimeGroup3, cmbTimeGroup4 };
            //ComboBox[] EnterStatusList = { cmbEnterStatus1, cmbEnterStatus2, cmbEnterStatus3, cmbEnterStatus4 };
            //for (i = 1; i <= 4; i++)
            //{
            //    //门权限
            //    card.SetDoor(i, DoorList[i - 1].Checked);
            //    //开门时段
            //    card.SetTimeGroup(i, TGList[i - 1].SelectedIndex + 1);
            //    //出入状态
            //    int value = EnterStatusList[i - 1].SelectedIndex;
            //    card.SetEnterStatusValue(i, value);
            //}
            card.Privilege = cmbPrivilege.SelectedIndex;
            int i;
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
                        //CardList.ResetItem(i);
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
            BindDataGrid(card);
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


            List<FC8864.Data.CardDetail> cards = new List<CardDetail>();
            cards.Add(new FC8864.Data.CardDetail(card));
            var par = new FC8864.Card.CardListBySequence.WriteCardListBySequence_Parameter(cards);
            cmd = new FC8864.Card.CardListBySequence.WriteCardListBySequence(cmdDtl, par);


            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Door.Door8800.Card.WriteCardList_Result;
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
            for (int i = 0; i < dgCardList.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgCardList.Rows[i].Cells[0];
                cell.Value = bSelect;
                cell.EditingCellFormattedValue = bSelect;
            }
            //foreach (var item in CardList)
            //{
            //    item.Selected = bSelect;
            //}
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
            for (int i = dgCardList.Rows.Count - 1; i >= 0; i--)
            {
                dgCardList.Rows.RemoveAt(i);
            }
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
                MessageBox.Show("输入的数字不正确，取值范围：1-32000！");
                return 0;
            }
            if (iCreateCount > 32000)
            {
                MessageBox.Show("输入的数字不正确，取值范围：1-32000！");
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

            //CardList.RaiseListChangedEvents = false;
            CardDetailBase card;
            for (int i = 0; i < iCreateCount; i++)
            {

                card = CreateNewCardDetail(0);
                AddCardDataBaseToList(card);
                BindDataGrid( card);
            }
            
            //CardList.RaiseListChangedEvents = true;
            //CardList.ResetBindings();
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

            string sBeginNum = frmInputBox.ShowBox("起始序号", "请输入卡号的起始序号，取值范围：1-4000000000", "1", 10);
            UInt64 iBeginNum = 0;
            if (!UInt64.TryParse(sBeginNum, out iBeginNum))
            {
                return;
            }
            if (iBeginNum <= 0) iBeginNum = 1;

            CardDetailBase card;

            //CardList.RaiseListChangedEvents = false;
            while (iCreateCount > 0)
            {
                card = CreateNewCardDetail(++iBeginNum);
                if (card != null)
                {
                    AddCardDataBaseToList(card);

                    iCreateCount--;
                }

            }
            //CardList.RaiseListChangedEvents = true;
            //CardList.ResetBindings();

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
        private CardDetailBase CreateNewCardDetail(UInt64 iCardNum)
        {
            UInt64 cardNum = 0, cardNum2 = 0;
            CardDetailBase card;
            if (iCardNum == 0)
            {
                cardNum = (UInt64)(mCardRnd.Next(mCardMax) % (mCardMax - mCardMin + 1) + mCardMin);
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
                    return CreateNewCardDetail(0);
                }
                else
                {
                    return null;
                }

            }



            card = new CardDetail();
            card.CardData = (uint)cardNum;
            card.DoorNumList = new byte[65];
            for (int i = 0; i < 65; i++)
            {
                card.DoorNumList[i] = 1;
            }
            card.TimeGroup = new byte[64];
            Random r = new Random();
            int rValue = r.Next(65);
            card.DoorNumList[rValue] = 0;
            card.TimeGroup[rValue] = (byte)r.Next(64);
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

            //CardList.RaiseListChangedEvents = false;
            CardList.Clear();
            CardHashTable.Clear();
            foreach (var c in lst)
            {
                CardList.Add(c);
                CardHashTable.Add(c.CardDetail.CardData);
            }
            //CardList.RaiseListChangedEvents = true;
            //CardList.ResetBindings();
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

            List<CardDetail> cards = new List<CardDetail>();
            cards.Add(new CardDetail(card));
            var par = new DeleteCard_Parameter(cards);
            cmd = new DeleteCard(cmdDtl, par);

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


            List<CardDetail> cards = new List<CardDetail>();
            //foreach (var item in CardList)
            //{
            //    if (item.Selected)
            //    {
            //        if (item.CardDetail.CardData < UInt32.MaxValue)
            //        {
            //            cards.Add(new CardDetail(item.CardDetail));
            //        }
            //    }
            //}
            for (int i = 0; i < dgCardList.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgCardList.Rows[i].Cells[0];
                DataGridViewTextBoxCell tbcell = (DataGridViewTextBoxCell)dgCardList.Rows[i].Cells[2];
                if ((bool)cell.FormattedValue)
                {
                    CardDetail card = new CardDetail();
                    card.CardData = uint.Parse(tbcell.Value.ToString());
                    cards.Add(card);
                }
            }
            if (cards.Count > 0)
            {
                var par = new DeleteCard_Parameter(cards);
                cmd = new DeleteCard(cmdDtl, par);
            }
            if (cmd == null)
            {
                MsgErr("没有需要删除的卡片！");
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


        private void Dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex == 1)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            if (e.ColumnIndex == 2)
            {
                DataGridViewComboBoxColumn combobox = dgv.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
                if (combobox != null) //如果该列是TextBox列
                {
                    dgv.BeginEdit(true); //开始编辑状态
                    dgv.ReadOnly = false;
                }
            }
        }

    }
}
