
namespace UploadPersonDemo
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
            this.Btn_search = new System.Windows.Forms.Button();
            this.Cmb_IP = new System.Windows.Forms.ComboBox();
            this.Num_UdpPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Txt_log = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Num_UpdServerPort = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.Btn_Bind = new System.Windows.Forms.Button();
            this.Btn_UploadPerson = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Num_UdpPort)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_UpdServerPort)).BeginInit();
            this.SuspendLayout();
            // 
            // Btn_search
            // 
            this.Btn_search.Enabled = false;
            this.Btn_search.Location = new System.Drawing.Point(689, 8);
            this.Btn_search.Name = "Btn_search";
            this.Btn_search.Size = new System.Drawing.Size(96, 23);
            this.Btn_search.TabIndex = 0;
            this.Btn_search.Text = "search";
            this.Btn_search.UseVisualStyleBackColor = true;
            this.Btn_search.Click += new System.EventHandler(this.Btn_search_Click);
            // 
            // Cmb_IP
            // 
            this.Cmb_IP.FormattingEnabled = true;
            this.Cmb_IP.Location = new System.Drawing.Point(52, 12);
            this.Cmb_IP.Name = "Cmb_IP";
            this.Cmb_IP.Size = new System.Drawing.Size(150, 20);
            this.Cmb_IP.TabIndex = 1;
            // 
            // Num_UdpPort
            // 
            this.Num_UdpPort.Location = new System.Drawing.Point(611, 10);
            this.Num_UdpPort.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.Num_UdpPort.Name = "Num_UdpPort";
            this.Num_UdpPort.Size = new System.Drawing.Size(72, 21);
            this.Num_UdpPort.TabIndex = 2;
            this.Num_UdpPort.Value = new decimal(new int[] {
            8101,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(552, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "UdpPort:";
            // 
            // Txt_log
            // 
            this.Txt_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Txt_log.Location = new System.Drawing.Point(3, 17);
            this.Txt_log.Multiline = true;
            this.Txt_log.Name = "Txt_log";
            this.Txt_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Txt_log.Size = new System.Drawing.Size(770, 553);
            this.Txt_log.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Txt_log);
            this.groupBox1.Location = new System.Drawing.Point(12, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 573);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // Num_UpdServerPort
            // 
            this.Num_UpdServerPort.Location = new System.Drawing.Point(308, 11);
            this.Num_UpdServerPort.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.Num_UpdServerPort.Name = "Num_UpdServerPort";
            this.Num_UpdServerPort.Size = new System.Drawing.Size(72, 21);
            this.Num_UpdServerPort.TabIndex = 2;
            this.Num_UpdServerPort.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(213, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "UdpServerPort:";
            // 
            // Btn_Bind
            // 
            this.Btn_Bind.Location = new System.Drawing.Point(398, 8);
            this.Btn_Bind.Name = "Btn_Bind";
            this.Btn_Bind.Size = new System.Drawing.Size(75, 23);
            this.Btn_Bind.TabIndex = 7;
            this.Btn_Bind.Text = "Bind";
            this.Btn_Bind.UseVisualStyleBackColor = true;
            this.Btn_Bind.Click += new System.EventHandler(this.Btn_Bind_Click);
            // 
            // Btn_UploadPerson
            // 
            this.Btn_UploadPerson.Location = new System.Drawing.Point(689, 43);
            this.Btn_UploadPerson.Name = "Btn_UploadPerson";
            this.Btn_UploadPerson.Size = new System.Drawing.Size(96, 23);
            this.Btn_UploadPerson.TabIndex = 8;
            this.Btn_UploadPerson.Text = "UploadPerson";
            this.Btn_UploadPerson.UseVisualStyleBackColor = true;
            this.Btn_UploadPerson.Click += new System.EventHandler(this.Btn_UploadPerson_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 657);
            this.Controls.Add(this.Btn_UploadPerson);
            this.Controls.Add(this.Btn_Bind);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Num_UpdServerPort);
            this.Controls.Add(this.Num_UdpPort);
            this.Controls.Add(this.Cmb_IP);
            this.Controls.Add(this.Btn_search);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Num_UdpPort)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_UpdServerPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_search;
        private System.Windows.Forms.ComboBox Cmb_IP;
        private System.Windows.Forms.NumericUpDown Num_UdpPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Txt_log;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown Num_UpdServerPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Btn_Bind;
        private System.Windows.Forms.Button Btn_UploadPerson;
    }
}

