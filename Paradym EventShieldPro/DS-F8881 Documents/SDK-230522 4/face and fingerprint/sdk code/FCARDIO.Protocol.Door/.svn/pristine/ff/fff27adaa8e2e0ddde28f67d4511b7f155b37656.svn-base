using DoNetDrive.Protocol.USB.CardReader.ICCard.SearchCard;
using DoNetDrive.Protocol.USB.CardReader.ICCard.Sector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Common.Extensions;


namespace DoNetDrive.Protocol.USB.CardReader.Test
{
    public partial class frmICCard : frmNodeForm
    {

        public static string[] mCardTypeList;

        public int Type;

        #region 单例模式
        private static object lockobj = new object();
        private static frmICCard onlyObj;
        public static frmICCard GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmICCard(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private frmICCard(INMain main) : base(main)
        {
            InitializeComponent();
            InitControl(0);
        }
        #endregion

        static frmICCard()
        {
            mCardTypeList = new string[] { "", "MF1 IC卡 S50", "NFC标签卡", "NFC手机", "身份证", "CPU IC卡 S50", "CPU卡", "MF1 IC卡 S70", "CPU IC卡 S70", "ID卡" };
        }

        private void BtnSearchCard_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            SearchCard cmd = new SearchCard(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                SearchCard_Result result = cmde.Command.getResult() as SearchCard_Result;

                Invoke(() =>
                {
                    plSector.Visible = result.IsSuccess;
                    Type = result.Type;
                    if (result.IsSuccess)
                    {
                        txtCardData.Text = result.CardData.ToString();
                        txtCardType.Text = mCardTypeList[Type];


                    }
                    else
                    {
                        txtCardData.Text = "";
                        txtCardType.Text = "";
                    }
                    if (true)
                    {

                    }
                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void InitControl(byte type)
        {
            cmbNumber.Items.Clear();
            cmbStartBlock.Items.Clear();
            cmbReadCount.Items.Clear();

            for (int i = 0; i < 15; i++)
            {
                cmbNumber.Items.Add(i.ToString());
            }
           
            cmbNumber.SelectedIndex = 1;
            cmbStartBlock.Items.Add("0");
            cmbStartBlock.Items.Add("1");
            cmbStartBlock.Items.Add("2");
            cmbStartBlock.Items.Add("3");
            cmbStartBlock.SelectedIndex = 0;

            string[] listCount = new string[64];
            for (int i = 0; i < 64; i++)
            {
                listCount[i] = (i + 1).ToString();
            }
            cmbReadCount.Items.AddRange(listCount);
            cmbReadCount.SelectedIndex = 15;

            cmbVerifyMode.Items.Clear();
            cmbVerifyMode.Items.AddRange(new string[] { "A密钥", "B密钥" });
            cmbVerifyMode.SelectedIndex = 0;
            //if (type != 0)
            //{
            //    int number = 0;
            //    int block = 0;
            //    if (type == 1 || type == 5)
            //    {
            //        number = 15;
            //        block = 3;
            //    }
            //    else if (type == 7 || type == 8)
            //    {

            //    }
            //}
        }

        private void BtnReadSector_Click(object sender, EventArgs e)
        {
            var eeeee = "".HexToByte();
            var b = new byte[0];
            var s = b.ToHex();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadSector_Parameter par = new ReadSector_Parameter(Type, (byte)cmbNumber.SelectedIndex, (byte)cmbStartBlock.SelectedIndex
                , (byte)(cmbReadCount.SelectedIndex + 1), (byte)(cmbVerifyMode.SelectedIndex + 1), txtPassword.Text);
            //ReadSector_Parameter par = new ReadSector_Parameter(1, (byte)1, (byte)0, (byte)48, (byte)1, "131F0153FC11");
            ReadSector cmd = new ReadSector(cmdDtl,par);
            mMainForm.AddCommand(cmd);
            txtContent.Text = "";
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadSector_Result result = cmde.Command.getResult() as ReadSector_Result;

                Invoke(() =>
                {
                    if (result.IsSuccess == 1)
                    {
                        txtContent.Text = result.ByteContent.ToHex();
                    }
                    

                });
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }

        private void BtnWriteSector_Click(object sender, EventArgs e)
        {

            string sPWD = txtPassword.Text.Trim();
            if (sPWD.Length != 12)
            {
                MessageBox.Show("参数错误");
                return;
            }
            string sData = txtContent.Text.Trim().Replace("\r",string.Empty).Replace("\n",string.Empty);
            if (!sPWD.IsHex() || !sData.IsHex())
            {
                MessageBox.Show("参数错误");
                return;
            }

            byte[] bData = sData.HexToByte();

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null)
            {
                MessageBox.Show("参数错误");
                return;
            }
            cmdDtl.Timeout = 500;

         WriteSector_Parameter par = new WriteSector_Parameter(Type, (byte)cmbNumber.SelectedIndex, (byte)cmbStartBlock.SelectedIndex
                ,  (byte)(cmbVerifyMode.SelectedIndex + 1), txtPassword.Text, bData);
            try
            {
                if( !par.checkedParameter() )
                {
                    MessageBox.Show("参数错误");
                    return;
                }
            }
            catch(Exception pe)
            {
                MessageBox.Show("参数错误");
                return;
            }
            WriteSector cmd = new WriteSector(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                WriteSector_Result result = cmde.Command.getResult() as WriteSector_Result;

                Invoke(() =>
                {
                    string[] tipList = new string[] { "成功"," 密码不正确","卡片已离开感应区","块数据长度不正确"};
                    
                    mMainForm.AddCmdLog(cmde, $"写扇区结果{tipList[result.Result - 1]}");
                });
            };
            cmdDtl.CommandErrorEvent += (sdr, cmde) =>
            {
            };
            cmdDtl.CommandTimeout += (sdr, cmde) =>
            {
            };

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            var b = new byte[0];
            var s = b.ToHex();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            for (int i = 11; i < 14; i++)
            {
                ReadSector_Parameter par = new ReadSector_Parameter(Type, (byte)i, (byte)0
                , (byte)48, (byte)1, $"131F{i}531234");
                ReadSector cmd = new ReadSector(cmdDtl, par);
                mMainForm.AddCommand(cmd);
            }
        }
    }
}
