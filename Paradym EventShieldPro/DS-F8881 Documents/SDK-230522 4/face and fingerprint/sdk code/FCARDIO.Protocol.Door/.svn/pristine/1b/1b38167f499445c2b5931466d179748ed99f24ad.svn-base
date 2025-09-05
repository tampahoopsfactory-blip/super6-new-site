using System;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.Elevator.Test
{
    /// <summary>
    /// 通用输入模式窗口
    /// </summary>
    public partial class frmInputBox : Form
    {
        /// <summary>
        /// 创建一个输入窗口
        /// </summary>
        /// <param name="sTitle">标题</param>
        /// <param name="sTip">提示内容</param>
        /// <param name="defData">默认值</param>
        /// <param name="iDataMax">输入框最大长度</param>
        public frmInputBox(string sTitle, string sTip, string defData, int iDataMax = 1024)
        {
            InitializeComponent();

            mTitle = sTitle;
            mTip = sTip;
            mDefData = defData;
            mValue = mDefData;
            mDataMax = iDataMax;
            ButStatus = ButCancel;
        }

        private string mTitle, mTip, mDefData;
        private int mDataMax;
        private int ButStatus;

        private string mValue;

        /// <summary>
        /// 取消按钮
        /// </summary>
        public const int ButCancel = 0;

        /// <summary>
        /// 保存按钮
        /// </summary>
        public const int ButSave = 1;

        /// <summary>
        /// 获取已保存的输入内容
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            return mValue;
        }

        /// <summary>
        /// 返回窗口状态
        /// </summary>
        /// <returns></returns>
        public int GetStatus()
        {
            return ButStatus;
        }

        /// <summary>
        /// 用于显示窗口的静态函数
        /// </summary>
        /// <param name="sTitle">标题</param>
        /// <param name="sTip">提示内容</param>
        /// <param name="defData">默认值</param>
        /// <param name="iDataMax">输入框最大长度</param>
        /// <returns></returns>
        public static string ShowBox(string sTitle, string sTip, string defData, int iDataMax = 1024)
        {
            var frm = new frmInputBox(sTitle, sTip, defData, iDataMax);
            frm.ShowDialog();
            int status = frm.GetStatus();
            string value = string.Empty;
            if (status == ButSave)
            {
                value = frm.GetValue();
            }

            frm = null;

            return value;
        }


        private void butSave_Click(object sender, EventArgs e)
        {
            ButStatus = ButSave;
            mValue = txtData.Text;
            Close();
        }

        private void frmInputBox_Load(object sender, EventArgs e)
        {
            Text = mTitle;
            lblText.Text = mTip;
            txtData.Text = mDefData;
            txtData.MaxLength = mDataMax;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            ButStatus = ButCancel;
            Close();
        }
    }
}
