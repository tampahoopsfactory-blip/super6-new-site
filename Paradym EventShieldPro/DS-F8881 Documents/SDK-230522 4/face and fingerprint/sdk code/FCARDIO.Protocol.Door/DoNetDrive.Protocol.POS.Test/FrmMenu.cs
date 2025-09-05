using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.Menu;
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
    public partial class FrmMenu : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static FrmMenu onlyObj;
        public static FrmMenu GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new FrmMenu(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private FrmMenu(INMain main) : base(main)
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// 卡号字典
        /// </summary>
        HashSet<int> MenuHashTable = null;

        private void FrmMenu_Load(object sender, EventArgs e)
        {
            MenuHashTable = new HashSet<int>();
            dgvMenu.AutoGenerateColumns = false;
        }

        private BindingList<MenuDetail> ListMenuDetail = new BindingList<MenuDetail>();

        public FrmMenu()
        {
            InitializeComponent();
        }

        private void butReadDataBase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadMenuDataBase cmd = new ReadMenuDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadMenuDataBase_Result result = cmde.Command.getResult() as ReadMenuDataBase_Result;
                string tip = $"商品信息--最大容量：{result.SortSize},已存数量：{result.UseSize}";

                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butReadAllMenu_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAllMenu cmd = new ReadAllMenu(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAllMenu_Result result = cmde.Command.getResult() as ReadAllMenu_Result;
                string tip = $"";
                Invoke(() =>
                {
                    dgvMenu.AutoGenerateColumns = false;
                    dgvMenu.DataSource = new BindingList<MenuDetail>(result.MenuDetails);
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
            MenuHashTable.Clear();
            ClearMenuDataBase cmd = new ClearMenuDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        private void butAddAll_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<MenuDetail> _list = new List<MenuDetail>();
            for (int i = 0; i < ListMenuDetail.Count; i++)
            {
                MenuDetail menu = new MenuDetail() {  };
                menu.MenuBarCode = ListMenuDetail[i].MenuBarCode;
                menu.MenuCode = ListMenuDetail[i].MenuCode;
                menu.MenuPrice = ListMenuDetail[i].MenuPrice;
                menu.MenuName = ListMenuDetail[i].MenuName;
                _list.Add(menu);
            }
            if (_list.Count == 0)
            {
                return;
            }
            WriteMenu_Parameter par = new WriteMenu_Parameter(_list);
            AddMenu cmd = new AddMenu(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };
        }

        private void butAddToList_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("请输入商品名称");
                return;
            }
            int code = 0;
            if (!int.TryParse(txtCode.Text.Trim(), out code))
            {
                MessageBox.Show("请输入商品代码");
                return;
            }
            int print = 0;
            if (!int.TryParse(txtPrice.Text.Trim(), out print))
            {
                MessageBox.Show("请输入商品价格");
                return;
            }
            MenuDetail dto = new MenuDetail();
            dto.MenuBarCode = txtBarCode.Text;
            dto.MenuCode = code;
            dto.MenuName = txtName.Text;
            dto.MenuPrice = print;
            ListMenuDetail.Add(dto);

            Invoke(() =>
            {
                dgvMenu.DataSource = new BindingList<MenuDetail>(ListMenuDetail);
            });
        }

        private void butDeleteFromList_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvMenu.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvMenu.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dgvMenu.Rows[i].Cells[1];
                    var item = ListMenuDetail.FirstOrDefault(t => t.MenuCode == int.Parse(text.Value.ToString()));
                    ListMenuDetail.Remove(item);
                }
            }
            dgvMenu.DataSource = new BindingList<MenuDetail>(ListMenuDetail);
        }

        private void butAddToDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("请输入商品名称");
                return;
            }
            int code = 0;
            if (!int.TryParse(txtCode.Text.Trim(), out code))
            {
                MessageBox.Show("请输入商品代码");
                return;
            }
            decimal price = 0;
            if (!decimal.TryParse(txtPrice.Text.Trim(), out price))
            {
                MessageBox.Show("请输入商品价格");
                return;
            }
            List<MenuDetail> _list = new List<MenuDetail>();
            MenuDetail menu = new MenuDetail();
            menu.MenuBarCode = txtBarCode.Text;
            menu.MenuCode = code;
            menu.MenuPrice = price;
            menu.MenuName = txtName.Text;
            _list.Add(menu);
            WriteMenu_Parameter par = new WriteMenu_Parameter(_list);

            AddMenu cmd = new AddMenu(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddLog($"命令成功：");
            };
        }

        private void butClearList_Click(object sender, EventArgs e)
        {
            ListMenuDetail.Clear();
            dgvMenu.DataSource = ListMenuDetail;
            MenuHashTable.Clear();
        }

        private void butReadMenu_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            int usercode = 0;
            if (!int.TryParse(txtCode.Text, out usercode))
            {
                MessageBox.Show("商品代码格式不正确");
                return;
            }
            var par = new ReadMenuDetail_Parameter(usercode);
            var cmd = new ReadMenuDetail(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadMenuDetail_Result;

                if (!result.IsReady)
                {
                    mMainForm.AddCmdLog(cmde, $"商品不存在");
                }
                else
                {
                    //PersonToControl(result.Person);
                    mMainForm.AddCmdLog(cmde, $"商品存在");
                }
            };
        }

        private void butDeleteFromDevice_Click(object sender, EventArgs e)
        {
         
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<MenuDetail> _list = new List<MenuDetail>();
            for (int i = 0; i < dgvMenu.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvMenu.Rows[i].Cells[0];
                DataGridViewTextBoxCell cellContent = (DataGridViewTextBoxCell)dgvMenu.Rows[i].Cells[1];
                if ((bool)cell.FormattedValue)
                {
                    _list.Add(new MenuDetail() { MenuCode = Convert.ToInt32(cellContent.Value) });
                }
            }
            if (_list.Count == 0)
            {
                return;
            }


            WriteMenu_Parameter par = new WriteMenu_Parameter(_list);
            DeleteMenu cmd = new DeleteMenu(cmdDtl, par);
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


            ListMenuDetail.RaiseListChangedEvents = false;
            MenuDetail menu;
            while (iCreateCount > 0)
            {
                menu = CreateNewMenuDetail(iBeginNum++);
                if (menu != null)
                {
                    menu.MenuPrice = 1;
                    menu.MenuName = "商品：" + menu.MenuCode.ToString();
                    AddMenuBaseToList(menu);

                    iCreateCount--;
                }

            }
            ListMenuDetail.RaiseListChangedEvents = true;
            ListMenuDetail.ResetBindings();
            dgvMenu.DataSource = ListMenuDetail;
        }

        private bool AddMenuBaseToList(MenuDetail menu)
        {
            if (!MenuHashTable.Contains(menu.MenuCode))
            {
                ListMenuDetail.Add(menu);
                MenuHashTable.Add(menu.MenuCode);
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
        private MenuDetail CreateNewMenuDetail(int code)
        {
           
            //检查卡片是否重复
            if (MenuHashTable.Contains(code))
            {
                if (code == 0)
                {
                    //有重复
                    return CreateNewMenuDetail( 0);
                }
                else
                {
                    return null;
                }

            }
            MenuDetail menu = new MenuDetail();
            menu.MenuCode = code;
            menu.MenuName = "";
            menu.MenuBarCode = "";
            return menu;
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
            if ((iCreateCount + ListMenuDetail.Count) > 32000)
            {
                iCreateCount = 32000 - ListMenuDetail.Count;

            }
            if (iCreateCount <= 0) return 0;

            return iCreateCount;
        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex, row = e.RowIndex;
            if (row < 0) return;

            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvMenu.Rows[e.RowIndex].Cells[e.ColumnIndex];
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

            var gdRow = dgvMenu.Rows[row];
            var menu = gdRow.DataBoundItem as MenuDetail;
            //StringBuilder strBuf = new StringBuilder();

            //DebugCardDetail(CardUI.CardDetail, strBuf);
            //txtDebug.Text = strBuf.ToString();
            MenuDetailToControl(menu);
        }

        /// <summary>
        /// 将卡片输出到控件中
        /// </summary>
        /// <param name="card"></param>
        private void MenuDetailToControl(MenuDetail menu)
        {
            txtCode.Text = menu.MenuCode.ToString();
            txtName.Text = menu.MenuName;
            txtBarCode.Text = menu.MenuBarCode;
            txtPrice.Value = Convert.ToDecimal(menu.MenuPrice) / (decimal)100;
        }
    }
}
