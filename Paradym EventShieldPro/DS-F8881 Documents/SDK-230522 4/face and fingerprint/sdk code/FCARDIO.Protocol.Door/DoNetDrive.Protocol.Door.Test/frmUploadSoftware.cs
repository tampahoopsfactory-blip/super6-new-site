using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Door.Door89H;

namespace DoNetDrive.Protocol.Door.Test
{
    public partial class frmUploadSoftware : frmNodeForm
    {
        private static object lockobj = new object();
        private static frmUploadSoftware onlyObj;
        public static frmUploadSoftware GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmUploadSoftware(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }


        private frmUploadSoftware(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }

        private void frmUploadSoftware_Load(object sender, EventArgs e)
        {

        }

        uint Crc32;
        byte[] SoftWareData;
        private void Btn_Select_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "固件文件|*.RCBin";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string sFile = ofd.FileName;
            var bSurFile = File.ReadAllBytes(sFile);
            int iFileLen = bSurFile.Length;
            string sVer = $"{bSurFile[16]}.{bSurFile[17]}";

            byte[] iCRCBuf = bSurFile.Copy(18, 4);
            Crc32 = iCRCBuf.ToInt32();
            int iSoftwareSize = iFileLen - 26;
            uint iFileCRC32 = bSurFile.Copy(iFileLen - 4, 4).ToInt32();
            SoftWareData = bSurFile.Copy(22, iSoftwareSize);
            uint itmpCRC32 = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(SoftWareData, 0, (uint)iSoftwareSize);
            if (itmpCRC32 != iFileCRC32)
            {
                SoftWareData = null;
                Crc32 = 0;
                MessageBox.Show("固件校验失败");
                return;
            }

            Txt_CRC.Text = iCRCBuf.ToHex();
            Txt_Ver.Text = sVer;
            Txt_FliePath.Text = sFile;
        }

        private void Btn_Upload_Click(object sender, EventArgs e)
        {
            if (Crc32 == 0 || SoftWareData == null)
            {
                MessageBox.Show("请选择固件");
                return;
            }
            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 10000;
            var par = new UpdateSoftware_Parameter(SoftWareData, Crc32);
            UpdateSoftware cmd = new UpdateSoftware(cmdDtl, par);
            cmdDtl.CommandCompleteEvent += CmdDtl_CommandCompleteEvent;
            mMainForm.AddCommand(cmd);
        }

        private void CmdDtl_CommandCompleteEvent(object sender, Core.Command.CommandEventArgs e)
        {
            UpdateSoftware_Result result = e.Result as UpdateSoftware_Result;
            MessageBox.Show("上传完成,返回结果:"+ result.Success);
        }
    }
}
