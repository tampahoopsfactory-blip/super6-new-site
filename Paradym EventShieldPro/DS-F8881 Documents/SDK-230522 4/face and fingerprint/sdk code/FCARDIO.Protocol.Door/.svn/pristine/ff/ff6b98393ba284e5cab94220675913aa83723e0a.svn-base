using DoNetDrive.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Test
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
        #region 多语言

        protected string GetLanguage(string sKey)
        {
            return ToolLanguage.GetLanguage(Name, sKey);
        }
        protected string GetLanguage(string sKey, params object[] args)
        {
            string str = ToolLanguage.GetLanguage(Name, sKey);
            return string.Format(str, args);
        }


        protected void GetLanguage(ToolStripItem ctr)
        {
            ctr.Text = ToolLanguage.GetLanguage(Name, ctr.Name);
        }
        protected void GetLanguage(Control ctr)
        {
            ctr.Text = ToolLanguage.GetLanguage(Name, ctr.Name);
        }
        protected void GetLanguage(DataGridView dg)
        {
            string sCols = GetLanguage($"{dg.Name}_Cols");
            var sArr = sCols.SplitTrim(",");
            var cols = dg.Columns;
            for (int i = 0; i < cols.Count; i++)
            {
                var col = cols[i];
                col.HeaderText = sArr[i];
            }

        }


        protected void LoadComboxItemsLanguage(ComboBox cbx, string sKey)
        {
            int iOldSelectIndex = cbx.SelectedIndex;
            cbx.Items.Clear();
            string sItems = GetLanguage(sKey);
            if (string.IsNullOrEmpty(sItems))
                return;

            string[] sArr = sItems.SplitTrim(",");
            cbx.Items.AddRange(sArr);
            if (cbx.Items.Count > iOldSelectIndex)
            {
                cbx.SelectedIndex = iOldSelectIndex;
            }
            else
            {
                cbx.SelectedIndex = 0;
            }
        }

        public virtual void LoadUILanguage()
        {
            //指纹机、人脸机 调试程序
            Text = GetLanguage("FormCaption");

        }
        #endregion
    }
}
