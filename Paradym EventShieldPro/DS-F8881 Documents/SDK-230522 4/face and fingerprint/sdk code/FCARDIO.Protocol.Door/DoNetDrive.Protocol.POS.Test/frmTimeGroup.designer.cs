namespace DotNetDrive.Protocol.POS.Test
{
    partial class frmTimeGroup
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
            this.btnAllReadTimeGroup = new System.Windows.Forms.Button();
            this.btnAddTimeGroup = new System.Windows.Forms.Button();
            this.btnClearTimeGroup = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnReadTimeGroup = new System.Windows.Forms.Button();
            this.btnFillNowTime = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.endTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.cbTimeGroup = new System.Windows.Forms.ComboBox();
            this.beginTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.endTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.beginTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.endTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.beginTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.endTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.beginTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbWeekday = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAllReadTimeGroup
            // 
            this.btnAllReadTimeGroup.Location = new System.Drawing.Point(196, 12);
            this.btnAllReadTimeGroup.Name = "btnAllReadTimeGroup";
            this.btnAllReadTimeGroup.Size = new System.Drawing.Size(108, 23);
            this.btnAllReadTimeGroup.TabIndex = 0;
            this.btnAllReadTimeGroup.Text = "读取全部消费时段";
            this.btnAllReadTimeGroup.UseVisualStyleBackColor = true;
            this.btnAllReadTimeGroup.Click += new System.EventHandler(this.BtnReadAllTimeGroup_Click);
            // 
            // btnAddTimeGroup
            // 
            this.btnAddTimeGroup.Location = new System.Drawing.Point(328, 12);
            this.btnAddTimeGroup.Name = "btnAddTimeGroup";
            this.btnAddTimeGroup.Size = new System.Drawing.Size(132, 23);
            this.btnAddTimeGroup.TabIndex = 1;
            this.btnAddTimeGroup.Text = "上传所有消费时段";
            this.btnAddTimeGroup.UseVisualStyleBackColor = true;
            this.btnAddTimeGroup.Click += new System.EventHandler(this.BtnAddTimeGroup_Click);
            // 
            // btnClearTimeGroup
            // 
            this.btnClearTimeGroup.Location = new System.Drawing.Point(484, 12);
            this.btnClearTimeGroup.Name = "btnClearTimeGroup";
            this.btnClearTimeGroup.Size = new System.Drawing.Size(138, 23);
            this.btnClearTimeGroup.TabIndex = 2;
            this.btnClearTimeGroup.Text = "清空所有消费时段";
            this.btnClearTimeGroup.UseVisualStyleBackColor = true;
            this.btnClearTimeGroup.Click += new System.EventHandler(this.BtnClearTimeGroup_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnReadTimeGroup);
            this.groupBox1.Controls.Add(this.btnFillNowTime);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.endTimePicker4);
            this.groupBox1.Controls.Add(this.cbTimeGroup);
            this.groupBox1.Controls.Add(this.beginTimePicker4);
            this.groupBox1.Controls.Add(this.endTimePicker3);
            this.groupBox1.Controls.Add(this.beginTimePicker3);
            this.groupBox1.Controls.Add(this.endTimePicker2);
            this.groupBox1.Controls.Add(this.beginTimePicker2);
            this.groupBox1.Controls.Add(this.endTimePicker1);
            this.groupBox1.Controls.Add(this.beginTimePicker1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbWeekday);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(610, 227);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "消费时段：";
            // 
            // btnReadTimeGroup
            // 
            this.btnReadTimeGroup.Location = new System.Drawing.Point(208, -1);
            this.btnReadTimeGroup.Name = "btnReadTimeGroup";
            this.btnReadTimeGroup.Size = new System.Drawing.Size(108, 23);
            this.btnReadTimeGroup.TabIndex = 5;
            this.btnReadTimeGroup.Text = "读取消费时段";
            this.btnReadTimeGroup.UseVisualStyleBackColor = true;
            this.btnReadTimeGroup.Click += new System.EventHandler(this.BtnReadTimeGroup_Click);
            // 
            // btnFillNowTime
            // 
            this.btnFillNowTime.Location = new System.Drawing.Point(100, 188);
            this.btnFillNowTime.Name = "btnFillNowTime";
            this.btnFillNowTime.Size = new System.Drawing.Size(95, 23);
            this.btnFillNowTime.TabIndex = 36;
            this.btnFillNowTime.Text = "填充 现在时间";
            this.btnFillNowTime.UseVisualStyleBackColor = true;
            this.btnFillNowTime.Click += new System.EventHandler(this.BtnFillNowTime_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(32, 193);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 35;
            this.label18.Text = "方便测试：";
            // 
            // endTimePicker4
            // 
            this.endTimePicker4.CustomFormat = "HH:mm";
            this.endTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker4.Location = new System.Drawing.Point(520, 96);
            this.endTimePicker4.Name = "endTimePicker4";
            this.endTimePicker4.ShowUpDown = true;
            this.endTimePicker4.Size = new System.Drawing.Size(57, 21);
            this.endTimePicker4.TabIndex = 26;
            this.endTimePicker4.ValueChanged += new System.EventHandler(this.EndTimePicker4_ValueChanged);
            // 
            // cbTimeGroup
            // 
            this.cbTimeGroup.AutoCompleteCustomSource.AddRange(new string[] {
            "第 1 时段",
            "第 2 时段",
            "第 3 时段",
            "第 4 时段",
            "第 5 时段",
            "第 6 时段",
            "第 7 时段",
            "第 8 时段"});
            this.cbTimeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTimeGroup.FormattingEnabled = true;
            this.cbTimeGroup.Location = new System.Drawing.Point(76, 0);
            this.cbTimeGroup.Name = "cbTimeGroup";
            this.cbTimeGroup.Size = new System.Drawing.Size(97, 20);
            this.cbTimeGroup.TabIndex = 0;
            this.cbTimeGroup.SelectedIndexChanged += new System.EventHandler(this.CbTimeGroup_SelectedIndexChanged);
            // 
            // beginTimePicker4
            // 
            this.beginTimePicker4.CustomFormat = "HH:mm";
            this.beginTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.beginTimePicker4.Location = new System.Drawing.Point(455, 96);
            this.beginTimePicker4.Name = "beginTimePicker4";
            this.beginTimePicker4.ShowUpDown = true;
            this.beginTimePicker4.Size = new System.Drawing.Size(57, 21);
            this.beginTimePicker4.TabIndex = 25;
            this.beginTimePicker4.ValueChanged += new System.EventHandler(this.BeginTimePicker4_ValueChanged);
            // 
            // endTimePicker3
            // 
            this.endTimePicker3.CustomFormat = "HH:mm";
            this.endTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker3.Location = new System.Drawing.Point(380, 96);
            this.endTimePicker3.Name = "endTimePicker3";
            this.endTimePicker3.ShowUpDown = true;
            this.endTimePicker3.Size = new System.Drawing.Size(57, 21);
            this.endTimePicker3.TabIndex = 24;
            this.endTimePicker3.ValueChanged += new System.EventHandler(this.EndTimePicker3_ValueChanged);
            // 
            // beginTimePicker3
            // 
            this.beginTimePicker3.CustomFormat = "HH:mm";
            this.beginTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.beginTimePicker3.Location = new System.Drawing.Point(315, 96);
            this.beginTimePicker3.Name = "beginTimePicker3";
            this.beginTimePicker3.ShowUpDown = true;
            this.beginTimePicker3.Size = new System.Drawing.Size(57, 21);
            this.beginTimePicker3.TabIndex = 23;
            this.beginTimePicker3.ValueChanged += new System.EventHandler(this.BeginTimePicker3_ValueChanged);
            // 
            // endTimePicker2
            // 
            this.endTimePicker2.CustomFormat = "HH:mm";
            this.endTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker2.Location = new System.Drawing.Point(239, 96);
            this.endTimePicker2.Name = "endTimePicker2";
            this.endTimePicker2.ShowUpDown = true;
            this.endTimePicker2.Size = new System.Drawing.Size(57, 21);
            this.endTimePicker2.TabIndex = 22;
            this.endTimePicker2.ValueChanged += new System.EventHandler(this.EndTimePicker2_ValueChanged);
            // 
            // beginTimePicker2
            // 
            this.beginTimePicker2.CustomFormat = "HH:mm";
            this.beginTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.beginTimePicker2.Location = new System.Drawing.Point(174, 96);
            this.beginTimePicker2.Name = "beginTimePicker2";
            this.beginTimePicker2.ShowUpDown = true;
            this.beginTimePicker2.Size = new System.Drawing.Size(57, 21);
            this.beginTimePicker2.TabIndex = 21;
            this.beginTimePicker2.ValueChanged += new System.EventHandler(this.BeginTimePicker2_ValueChanged);
            // 
            // endTimePicker1
            // 
            this.endTimePicker1.CustomFormat = "HH:mm";
            this.endTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker1.Location = new System.Drawing.Point(100, 96);
            this.endTimePicker1.Name = "endTimePicker1";
            this.endTimePicker1.ShowUpDown = true;
            this.endTimePicker1.Size = new System.Drawing.Size(57, 21);
            this.endTimePicker1.TabIndex = 20;
            this.endTimePicker1.ValueChanged += new System.EventHandler(this.EndTimePicker1_ValueChanged);
            // 
            // beginTimePicker1
            // 
            this.beginTimePicker1.CustomFormat = "HH:mm";
            this.beginTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.beginTimePicker1.Location = new System.Drawing.Point(32, 96);
            this.beginTimePicker1.Name = "beginTimePicker1";
            this.beginTimePicker1.ShowUpDown = true;
            this.beginTimePicker1.Size = new System.Drawing.Size(57, 21);
            this.beginTimePicker1.TabIndex = 19;
            this.beginTimePicker1.ValueChanged += new System.EventHandler(this.BeginTimePicker1_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(518, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "结束时间4";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(453, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 9;
            this.label9.Text = "开始时间4";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(378, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "结束时间3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(313, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "开始时间3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(237, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "结束时间2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(172, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "开始时间2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(98, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "结束时间1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "开始时间1";
            // 
            // cbWeekday
            // 
            this.cbWeekday.AutoCompleteCustomSource.AddRange(new string[] {
            "星期一",
            "星期二",
            "星期三",
            "星期四",
            "星期五",
            "星期六",
            "星期日"});
            this.cbWeekday.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWeekday.FormattingEnabled = true;
            this.cbWeekday.Location = new System.Drawing.Point(226, 34);
            this.cbWeekday.Name = "cbWeekday";
            this.cbWeekday.Size = new System.Drawing.Size(121, 20);
            this.cbWeekday.TabIndex = 2;
            this.cbWeekday.SelectedIndexChanged += new System.EventHandler(this.CbWeekday_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(179, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "星期：";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 17);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 12);
            this.label19.TabIndex = 4;
            this.label19.Text = "消费时段";
            // 
            // frmTimeGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 295);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClearTimeGroup);
            this.Controls.Add(this.btnAddTimeGroup);
            this.Controls.Add(this.btnAllReadTimeGroup);
            this.Name = "frmTimeGroup";
            this.Text = "frmTimeGroup";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTimeGroup_FormClosed);
            this.Load += new System.EventHandler(this.frmTimeGroup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAllReadTimeGroup;
        private System.Windows.Forms.Button btnAddTimeGroup;
        private System.Windows.Forms.Button btnClearTimeGroup;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker endTimePicker4;
        private System.Windows.Forms.DateTimePicker beginTimePicker4;
        private System.Windows.Forms.DateTimePicker endTimePicker3;
        private System.Windows.Forms.DateTimePicker beginTimePicker3;
        private System.Windows.Forms.DateTimePicker endTimePicker2;
        private System.Windows.Forms.DateTimePicker beginTimePicker2;
        private System.Windows.Forms.DateTimePicker endTimePicker1;
        private System.Windows.Forms.DateTimePicker beginTimePicker1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbWeekday;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTimeGroup;
        private System.Windows.Forms.Button btnFillNowTime;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnReadTimeGroup;
    }
}