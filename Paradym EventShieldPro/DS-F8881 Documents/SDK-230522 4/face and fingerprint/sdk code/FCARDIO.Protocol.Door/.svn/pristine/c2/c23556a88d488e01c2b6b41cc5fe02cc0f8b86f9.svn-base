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
    public partial class frmEncryptionDecryption : frmNodeForm
    {

        public static string[] mCardTypeList;

        List<Model.ICCardData> DataList = new List<Model.ICCardData>();

        public byte Type;

        #region 单例模式
        private static object lockobj = new object();
        private static frmEncryptionDecryption onlyObj;
        public static frmEncryptionDecryption GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmEncryptionDecryption(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }
        public frmEncryptionDecryption(INMain main) : base(main)
        {
            InitializeComponent();
        }

        #endregion

        private void FrmEncryptionDecryption_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
               
                for (int j = 0; j < 4; j++)
                {
                    Model.ICCardData carddata = new Model.ICCardData() { Number = i, Block = j };
                    DataList.Add(carddata);
                }
            }
            dgvData.DataSource = new BindingList<Model.ICCardData>(DataList);
        }

        private void FrmEncryptionDecryption_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }

        private void BtnReadAll_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadSector_Parameter par = new ReadSector_Parameter(1, 1, "");
            ReadAllSector cmd = new ReadAllSector(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAllSector_Result result = cmde.Command.getResult() as ReadAllSector_Result;
                if (result.DataList.Count > 0)
                {
                    DataList.Clear();
                    foreach (var item in result.DataList)
                    {
                        DataList.Add(new Model.ICCardData() { Number = item.Number, Block = item.StartBlock, Data = item.ByteContent.ToHex() });
                    }
                    Invoke(() =>
                    {
                        dgvData.DataSource = new BindingList<Model.ICCardData>(DataList);

                    });
                }
                
                //mMainForm.AddCmdLog(cmde, $"{txtVersion.Text}");
            };
        }
    }
}
