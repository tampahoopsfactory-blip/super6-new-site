namespace TcpDemo
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbLocalIP = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTCPClientList = new System.Windows.Forms.ComboBox();
            this.butTCPServerBind = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTCPServerPort = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTCPServerPort)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbLocalIP);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbTCPClientList);
            this.groupBox1.Controls.Add(this.butTCPServerBind);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtTCPServerPort);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(499, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP Server";
            // 
            // cmbLocalIP
            // 
            this.cmbLocalIP.FormattingEnabled = true;
            this.cmbLocalIP.Location = new System.Drawing.Point(320, 23);
            this.cmbLocalIP.Name = "cmbLocalIP";
            this.cmbLocalIP.Size = new System.Drawing.Size(146, 20);
            this.cmbLocalIP.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(261, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Local IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "TcpClient:";
            // 
            // cmbTCPClientList
            // 
            this.cmbTCPClientList.FormattingEnabled = true;
            this.cmbTCPClientList.Location = new System.Drawing.Point(81, 65);
            this.cmbTCPClientList.Name = "cmbTCPClientList";
            this.cmbTCPClientList.Size = new System.Drawing.Size(385, 20);
            this.cmbTCPClientList.TabIndex = 3;
            // 
            // butTCPServerBind
            // 
            this.butTCPServerBind.Location = new System.Drawing.Point(166, 23);
            this.butTCPServerBind.Name = "butTCPServerBind";
            this.butTCPServerBind.Size = new System.Drawing.Size(75, 23);
            this.butTCPServerBind.TabIndex = 2;
            this.butTCPServerBind.Text = "Bind";
            this.butTCPServerBind.UseVisualStyleBackColor = true;
            this.butTCPServerBind.Click += new System.EventHandler(this.butTCPServerBind_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port:";
            // 
            // txtTCPServerPort
            // 
            this.txtTCPServerPort.Location = new System.Drawing.Point(81, 25);
            this.txtTCPServerPort.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.txtTCPServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtTCPServerPort.Name = "txtTCPServerPort";
            this.txtTCPServerPort.Size = new System.Drawing.Size(67, 21);
            this.txtTCPServerPort.TabIndex = 0;
            this.txtTCPServerPort.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Location = new System.Drawing.Point(12, 120);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(499, 380);
            this.panel1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(499, 380);
            this.textBox1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 512);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTCPServerPort)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTCPClientList;
        private System.Windows.Forms.Button butTCPServerBind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown txtTCPServerPort;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmbLocalIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
    }
}

