using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.USB.CardReader.Test
{
    public partial class frmNodeForm : Form
    {

        /// <summary>
        /// 定义主窗体结构
        /// </summary>
        protected INMain mMainForm;

        /// <summary>
        /// 在UI线程上执行一个方法
        /// </summary>
        /// <param name="p"></param>
        protected void Invoke(Action p)
        {
            Invoke((Delegate)p);
        }

        /// <summary>
        /// 线程休眠
        /// </summary>
        /// <param name="time"></param>
        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        #region 提示框
        public void MsgTip(string sText)
        {
            MessageBox.Show(sText, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MsgErr(string sText)
        {
            MessageBox.Show(sText, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion


        public frmNodeForm()
        {
            InitializeComponent();
        }

        public frmNodeForm(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }

        private void frmNodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void frmNodeForm_Load(object sender, EventArgs e)
        {

        }

    }
}
