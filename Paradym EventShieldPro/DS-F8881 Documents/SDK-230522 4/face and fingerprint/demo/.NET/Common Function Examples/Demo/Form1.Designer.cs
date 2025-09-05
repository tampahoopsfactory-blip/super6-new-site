
namespace Demo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbLocalNetAddr = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabConnectType = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.Num_UdpPort = new System.Windows.Forms.NumericUpDown();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearchEqut = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.Btn_DeletePerson = new System.Windows.Forms.Button();
            this.Btn_UploadPerson = new System.Windows.Forms.Button();
            this.readCardSaveInfo = new System.Windows.Forms.Button();
            this.btnReadCardListInfo = new System.Windows.Forms.Button();
            this.btnClearPerson = new System.Windows.Forms.Button();
            this.Num_UDPLocalPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearMessage = new System.Windows.Forms.Button();
            this.Btn_UDPBind = new System.Windows.Forms.Button();
            this.tabConnectType.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_UdpPort)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_UDPLocalPort)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbLocalNetAddr
            // 
            this.cmbLocalNetAddr.FormattingEnabled = true;
            this.cmbLocalNetAddr.Location = new System.Drawing.Point(69, 12);
            this.cmbLocalNetAddr.Name = "cmbLocalNetAddr";
            this.cmbLocalNetAddr.Size = new System.Drawing.Size(137, 20);
            this.cmbLocalNetAddr.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 27;
            this.label5.Text = "本机网卡";
            // 
            // tabConnectType
            // 
            this.tabConnectType.Controls.Add(this.tabPage1);
            this.tabConnectType.Location = new System.Drawing.Point(12, 38);
            this.tabConnectType.Name = "tabConnectType";
            this.tabConnectType.SelectedIndex = 0;
            this.tabConnectType.Size = new System.Drawing.Size(708, 108);
            this.tabConnectType.TabIndex = 29;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtSN);
            this.tabPage1.Controls.Add(this.label25);
            this.tabPage1.Controls.Add(this.Num_UdpPort);
            this.tabPage1.Controls.Add(this.txtIP);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnSearchEqut);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(700, 82);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "局域网通信";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 42;
            this.label3.Text = "密码：";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(274, 47);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(68, 21);
            this.txtPassword.TabIndex = 41;
            this.txtPassword.Text = "FFFFFFFF";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 40;
            this.label4.Text = "SN：";
            // 
            // txtSN
            // 
            this.txtSN.Location = new System.Drawing.Point(39, 44);
            this.txtSN.Name = "txtSN";
            this.txtSN.Size = new System.Drawing.Size(154, 21);
            this.txtSN.TabIndex = 39;
            this.txtSN.Text = "FC-8300T21074059";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(207, 20);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(65, 12);
            this.label25.TabIndex = 20;
            this.label25.Text = "搜索端口：";
            // 
            // Num_UdpPort
            // 
            this.Num_UdpPort.Location = new System.Drawing.Point(274, 15);
            this.Num_UdpPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Num_UdpPort.Name = "Num_UdpPort";
            this.Num_UdpPort.Size = new System.Drawing.Size(68, 21);
            this.Num_UdpPort.TabIndex = 19;
            this.Num_UdpPort.Value = new decimal(new int[] {
            8101,
            0,
            0,
            0});
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(39, 17);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(154, 21);
            this.txtIP.TabIndex = 0;
            this.txtIP.Text = "192.168.1.150";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP：";
            // 
            // btnSearchEqut
            // 
            this.btnSearchEqut.Location = new System.Drawing.Point(357, 13);
            this.btnSearchEqut.Name = "btnSearchEqut";
            this.btnSearchEqut.Size = new System.Drawing.Size(119, 23);
            this.btnSearchEqut.TabIndex = 18;
            this.btnSearchEqut.Text = "搜索设备";
            this.btnSearchEqut.UseVisualStyleBackColor = true;
            this.btnSearchEqut.Click += new System.EventHandler(this.btnSearchEqut_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Location = new System.Drawing.Point(8, 152);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(704, 186);
            this.tabControl2.TabIndex = 30;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.Btn_DeletePerson);
            this.tabPage3.Controls.Add(this.Btn_UploadPerson);
            this.tabPage3.Controls.Add(this.readCardSaveInfo);
            this.tabPage3.Controls.Add(this.btnReadCardListInfo);
            this.tabPage3.Controls.Add(this.btnClearPerson);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(696, 160);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "人员管理";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Btn_DeletePerson
            // 
            this.Btn_DeletePerson.Location = new System.Drawing.Point(171, 35);
            this.Btn_DeletePerson.Name = "Btn_DeletePerson";
            this.Btn_DeletePerson.Size = new System.Drawing.Size(134, 23);
            this.Btn_DeletePerson.TabIndex = 18;
            this.Btn_DeletePerson.Text = "删除人员";
            this.Btn_DeletePerson.UseVisualStyleBackColor = true;
            this.Btn_DeletePerson.Click += new System.EventHandler(this.Btn_DeletePerson_Click);
            // 
            // Btn_UploadPerson
            // 
            this.Btn_UploadPerson.Location = new System.Drawing.Point(171, 6);
            this.Btn_UploadPerson.Name = "Btn_UploadPerson";
            this.Btn_UploadPerson.Size = new System.Drawing.Size(134, 23);
            this.Btn_UploadPerson.TabIndex = 17;
            this.Btn_UploadPerson.Text = "上传人员";
            this.Btn_UploadPerson.UseVisualStyleBackColor = true;
            this.Btn_UploadPerson.Click += new System.EventHandler(this.Btn_UploadPerson_Click);
            // 
            // readCardSaveInfo
            // 
            this.readCardSaveInfo.Location = new System.Drawing.Point(5, 6);
            this.readCardSaveInfo.Name = "readCardSaveInfo";
            this.readCardSaveInfo.Size = new System.Drawing.Size(141, 23);
            this.readCardSaveInfo.TabIndex = 12;
            this.readCardSaveInfo.Text = "读取用户存储信息";
            this.readCardSaveInfo.UseVisualStyleBackColor = true;
            this.readCardSaveInfo.Click += new System.EventHandler(this.readCardSaveInfo_Click);
            // 
            // btnReadCardListInfo
            // 
            this.btnReadCardListInfo.Location = new System.Drawing.Point(5, 36);
            this.btnReadCardListInfo.Name = "btnReadCardListInfo";
            this.btnReadCardListInfo.Size = new System.Drawing.Size(141, 23);
            this.btnReadCardListInfo.TabIndex = 13;
            this.btnReadCardListInfo.Text = "读取所有用户";
            this.btnReadCardListInfo.UseVisualStyleBackColor = true;
            this.btnReadCardListInfo.Click += new System.EventHandler(this.btnReadCardListInfo_Click);
            // 
            // btnClearPerson
            // 
            this.btnClearPerson.Location = new System.Drawing.Point(398, 6);
            this.btnClearPerson.Name = "btnClearPerson";
            this.btnClearPerson.Size = new System.Drawing.Size(141, 23);
            this.btnClearPerson.TabIndex = 16;
            this.btnClearPerson.Text = "清空人员";
            this.btnClearPerson.UseVisualStyleBackColor = true;
            this.btnClearPerson.Click += new System.EventHandler(this.btnClearPerson_Click);
            // 
            // Num_UDPLocalPort
            // 
            this.Num_UDPLocalPort.Location = new System.Drawing.Point(294, 11);
            this.Num_UDPLocalPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Num_UDPLocalPort.Name = "Num_UDPLocalPort";
            this.Num_UDPLocalPort.Size = new System.Drawing.Size(68, 21);
            this.Num_UDPLocalPort.TabIndex = 19;
            this.Num_UDPLocalPort.Value = new decimal(new int[] {
            9001,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 31;
            this.label2.Text = "本地端口：";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(8, 369);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(708, 196);
            this.txtLog.TabIndex = 32;
            // 
            // btnClearMessage
            // 
            this.btnClearMessage.Location = new System.Drawing.Point(621, 340);
            this.btnClearMessage.Name = "btnClearMessage";
            this.btnClearMessage.Size = new System.Drawing.Size(91, 23);
            this.btnClearMessage.TabIndex = 33;
            this.btnClearMessage.Text = "清除消息";
            this.btnClearMessage.UseVisualStyleBackColor = true;
            this.btnClearMessage.Click += new System.EventHandler(this.btnClearMessage_Click);
            // 
            // Btn_UDPBind
            // 
            this.Btn_UDPBind.Location = new System.Drawing.Point(373, 9);
            this.Btn_UDPBind.Name = "Btn_UDPBind";
            this.Btn_UDPBind.Size = new System.Drawing.Size(75, 23);
            this.Btn_UDPBind.TabIndex = 34;
            this.Btn_UDPBind.Text = "绑定端口";
            this.Btn_UDPBind.UseVisualStyleBackColor = true;
            this.Btn_UDPBind.Click += new System.EventHandler(this.Btn_UDPBind_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 566);
            this.Controls.Add(this.Btn_UDPBind);
            this.Controls.Add(this.btnClearMessage);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.Num_UDPLocalPort);
            this.Controls.Add(this.tabConnectType);
            this.Controls.Add(this.cmbLocalNetAddr);
            this.Controls.Add(this.label5);
            this.Name = "Form1";
            this.Text = "常用示例";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabConnectType.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_UdpPort)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Num_UDPLocalPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLocalNetAddr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabConnectType;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.NumericUpDown Num_UdpPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearchEqut;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button readCardSaveInfo;
        private System.Windows.Forms.Button btnReadCardListInfo;
        private System.Windows.Forms.Button btnClearPerson;
        private System.Windows.Forms.NumericUpDown Num_UDPLocalPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClearMessage;
        private System.Windows.Forms.Button Btn_UDPBind;
        private System.Windows.Forms.Button Btn_DeletePerson;
        private System.Windows.Forms.Button Btn_UploadPerson;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSN;
    }
}

