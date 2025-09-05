namespace DoNetDrive.Protocol.Door.Test
{
    partial class frmTime
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtErrorTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnWriteBroadcastTime = new System.Windows.Forms.Button();
            this.btnWriteSystemTime = new System.Windows.Forms.Button();
            this.txtComputerTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSystemTime = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.btnReadSystemTime = new System.Windows.Forms.Button();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.btnWriteTimeCorrectionParameter = new System.Windows.Forms.Button();
            this.label72 = new System.Windows.Forms.Label();
            this.cbxCorrectionSeconds = new System.Windows.Forms.ComboBox();
            this.label75 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.rBtnSpeedUp = new System.Windows.Forms.RadioButton();
            this.rBtnSlowDown = new System.Windows.Forms.RadioButton();
            this.btnReadTimeCorrectionParameter = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CustomDateTime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnWriteCustomDateTime = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.panel14.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtErrorTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnWriteBroadcastTime);
            this.groupBox1.Controls.Add(this.btnWriteSystemTime);
            this.groupBox1.Controls.Add(this.txtComputerTime);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSystemTime);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.btnReadSystemTime);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(607, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备时间";
            // 
            // txtErrorTime
            // 
            this.txtErrorTime.Location = new System.Drawing.Point(275, 71);
            this.txtErrorTime.MaxLength = 16;
            this.txtErrorTime.Name = "txtErrorTime";
            this.txtErrorTime.ReadOnly = true;
            this.txtErrorTime.Size = new System.Drawing.Size(151, 21);
            this.txtErrorTime.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(273, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "误差时间：";
            // 
            // btnWriteBroadcastTime
            // 
            this.btnWriteBroadcastTime.Location = new System.Drawing.Point(491, 21);
            this.btnWriteBroadcastTime.Name = "btnWriteBroadcastTime";
            this.btnWriteBroadcastTime.Size = new System.Drawing.Size(102, 23);
            this.btnWriteBroadcastTime.TabIndex = 23;
            this.btnWriteBroadcastTime.Text = "校准时间_广播";
            this.btnWriteBroadcastTime.UseVisualStyleBackColor = true;
            this.btnWriteBroadcastTime.Click += new System.EventHandler(this.BtnWriteBroadcastTime_Click);
            // 
            // btnWriteSystemTime
            // 
            this.btnWriteSystemTime.Location = new System.Drawing.Point(383, 21);
            this.btnWriteSystemTime.Name = "btnWriteSystemTime";
            this.btnWriteSystemTime.Size = new System.Drawing.Size(102, 23);
            this.btnWriteSystemTime.TabIndex = 22;
            this.btnWriteSystemTime.Text = "更新设备时间";
            this.btnWriteSystemTime.UseVisualStyleBackColor = true;
            this.btnWriteSystemTime.Click += new System.EventHandler(this.BtnWriteSystemTime_Click);
            // 
            // txtComputerTime
            // 
            this.txtComputerTime.Location = new System.Drawing.Point(6, 71);
            this.txtComputerTime.MaxLength = 16;
            this.txtComputerTime.Name = "txtComputerTime";
            this.txtComputerTime.ReadOnly = true;
            this.txtComputerTime.Size = new System.Drawing.Size(172, 21);
            this.txtComputerTime.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "计算机时间：";
            // 
            // txtSystemTime
            // 
            this.txtSystemTime.Location = new System.Drawing.Point(6, 32);
            this.txtSystemTime.MaxLength = 16;
            this.txtSystemTime.Name = "txtSystemTime";
            this.txtSystemTime.ReadOnly = true;
            this.txtSystemTime.Size = new System.Drawing.Size(172, 21);
            this.txtSystemTime.TabIndex = 19;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(6, 17);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(65, 12);
            this.label26.TabIndex = 18;
            this.label26.Text = "设备时间：";
            // 
            // btnReadSystemTime
            // 
            this.btnReadSystemTime.Location = new System.Drawing.Point(275, 21);
            this.btnReadSystemTime.Name = "btnReadSystemTime";
            this.btnReadSystemTime.Size = new System.Drawing.Size(102, 23);
            this.btnReadSystemTime.TabIndex = 17;
            this.btnReadSystemTime.Text = "读设备时间";
            this.btnReadSystemTime.UseVisualStyleBackColor = true;
            this.btnReadSystemTime.Click += new System.EventHandler(this.BtnReadSystemTime_Click);
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.btnWriteTimeCorrectionParameter);
            this.groupBox20.Controls.Add(this.label72);
            this.groupBox20.Controls.Add(this.cbxCorrectionSeconds);
            this.groupBox20.Controls.Add(this.label75);
            this.groupBox20.Controls.Add(this.label77);
            this.groupBox20.Controls.Add(this.panel14);
            this.groupBox20.Controls.Add(this.btnReadTimeCorrectionParameter);
            this.groupBox20.Location = new System.Drawing.Point(12, 120);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(291, 123);
            this.groupBox20.TabIndex = 96;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "时钟自动修正参数";
            // 
            // btnWriteTimeCorrectionParameter
            // 
            this.btnWriteTimeCorrectionParameter.Location = new System.Drawing.Point(105, 94);
            this.btnWriteTimeCorrectionParameter.Name = "btnWriteTimeCorrectionParameter";
            this.btnWriteTimeCorrectionParameter.Size = new System.Drawing.Size(90, 23);
            this.btnWriteTimeCorrectionParameter.TabIndex = 92;
            this.btnWriteTimeCorrectionParameter.Text = "设置修正参数";
            this.btnWriteTimeCorrectionParameter.UseVisualStyleBackColor = true;
            this.btnWriteTimeCorrectionParameter.Click += new System.EventHandler(this.BtnWriteTimeCorrectionParameter_Click);
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(119, 62);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(23, 12);
            this.label72.TabIndex = 91;
            this.label72.Text = "/秒";
            // 
            // cbxCorrectionSeconds
            // 
            this.cbxCorrectionSeconds.FormattingEnabled = true;
            this.cbxCorrectionSeconds.IntegralHeight = false;
            this.cbxCorrectionSeconds.ItemHeight = 12;
            this.cbxCorrectionSeconds.Items.AddRange(new object[] {
            "禁用",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "255"});
            this.cbxCorrectionSeconds.Location = new System.Drawing.Point(9, 59);
            this.cbxCorrectionSeconds.MaxLength = 3;
            this.cbxCorrectionSeconds.Name = "cbxCorrectionSeconds";
            this.cbxCorrectionSeconds.Size = new System.Drawing.Size(104, 20);
            this.cbxCorrectionSeconds.TabIndex = 90;
            this.cbxCorrectionSeconds.Text = "禁用";
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(9, 44);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(41, 12);
            this.label75.TabIndex = 89;
            this.label75.Text = "秒数：";
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Location = new System.Drawing.Point(7, 25);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(0, 12);
            this.label77.TabIndex = 77;
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.rBtnSpeedUp);
            this.panel14.Controls.Add(this.rBtnSlowDown);
            this.panel14.Location = new System.Drawing.Point(9, 20);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(144, 21);
            this.panel14.TabIndex = 88;
            // 
            // rBtnSpeedUp
            // 
            this.rBtnSpeedUp.AutoSize = true;
            this.rBtnSpeedUp.Location = new System.Drawing.Point(0, 3);
            this.rBtnSpeedUp.Name = "rBtnSpeedUp";
            this.rBtnSpeedUp.Size = new System.Drawing.Size(47, 16);
            this.rBtnSpeedUp.TabIndex = 51;
            this.rBtnSpeedUp.Text = "调快";
            this.rBtnSpeedUp.UseVisualStyleBackColor = true;
            // 
            // rBtnSlowDown
            // 
            this.rBtnSlowDown.AutoSize = true;
            this.rBtnSlowDown.Checked = true;
            this.rBtnSlowDown.Location = new System.Drawing.Point(72, 3);
            this.rBtnSlowDown.Name = "rBtnSlowDown";
            this.rBtnSlowDown.Size = new System.Drawing.Size(47, 16);
            this.rBtnSlowDown.TabIndex = 52;
            this.rBtnSlowDown.TabStop = true;
            this.rBtnSlowDown.Text = "调慢";
            this.rBtnSlowDown.UseVisualStyleBackColor = true;
            // 
            // btnReadTimeCorrectionParameter
            // 
            this.btnReadTimeCorrectionParameter.Location = new System.Drawing.Point(9, 94);
            this.btnReadTimeCorrectionParameter.Name = "btnReadTimeCorrectionParameter";
            this.btnReadTimeCorrectionParameter.Size = new System.Drawing.Size(90, 23);
            this.btnReadTimeCorrectionParameter.TabIndex = 86;
            this.btnReadTimeCorrectionParameter.Text = "读取修正参数";
            this.btnReadTimeCorrectionParameter.UseVisualStyleBackColor = true;
            this.btnReadTimeCorrectionParameter.Click += new System.EventHandler(this.BtnReadTimeCorrectionParameter_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CustomDateTime);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnWriteCustomDateTime);
            this.groupBox2.Location = new System.Drawing.Point(309, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(305, 123);
            this.groupBox2.TabIndex = 97;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "自定义时间";
            // 
            // CustomDateTime
            // 
            this.CustomDateTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.CustomDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CustomDateTime.Location = new System.Drawing.Point(12, 32);
            this.CustomDateTime.Name = "CustomDateTime";
            this.CustomDateTime.Size = new System.Drawing.Size(172, 21);
            this.CustomDateTime.TabIndex = 90;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 89;
            this.label4.Text = "时间日期：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 77;
            // 
            // btnWriteCustomDateTime
            // 
            this.btnWriteCustomDateTime.Location = new System.Drawing.Point(12, 62);
            this.btnWriteCustomDateTime.Name = "btnWriteCustomDateTime";
            this.btnWriteCustomDateTime.Size = new System.Drawing.Size(90, 23);
            this.btnWriteCustomDateTime.TabIndex = 86;
            this.btnWriteCustomDateTime.Text = "更新设备时间";
            this.btnWriteCustomDateTime.UseVisualStyleBackColor = true;
            this.btnWriteCustomDateTime.Click += new System.EventHandler(this.BtnWriteCustomDateTime_Click);
            // 
            // frmTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 251);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox20);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTime";
            this.Load += new System.EventHandler(this.frmTime_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtComputerTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSystemTime;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btnReadSystemTime;
        private System.Windows.Forms.Button btnWriteBroadcastTime;
        private System.Windows.Forms.Button btnWriteSystemTime;
        private System.Windows.Forms.TextBox txtErrorTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.RadioButton rBtnSpeedUp;
        private System.Windows.Forms.RadioButton rBtnSlowDown;
        private System.Windows.Forms.Button btnReadTimeCorrectionParameter;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.ComboBox cbxCorrectionSeconds;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Button btnWriteTimeCorrectionParameter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnWriteCustomDateTime;
        private System.Windows.Forms.DateTimePicker CustomDateTime;
    }
}