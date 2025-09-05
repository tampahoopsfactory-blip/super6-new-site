
namespace DoNetDrive.Protocol.Fingerprint.Test
{
    partial class frmElevator
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
            this.btnReadPersonElevatorAccess = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnWritePersonElevatorAccess = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnUnlockRelay = new System.Windows.Forms.Button();
            this.btnLockRelay = new System.Windows.Forms.Button();
            this.btnHoldRelay = new System.Windows.Forms.Button();
            this.btnCloseRelay = new System.Windows.Forms.Button();
            this.btnOpenRelay = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnWriteWorkType = new System.Windows.Forms.Button();
            this.btnReadWorkType = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnWriteTimingOpen = new System.Windows.Forms.Button();
            this.btnReadTimingOpen = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnWriteReleaseTime = new System.Windows.Forms.Button();
            this.btnReadReleaseTime = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnWriteRelayType = new System.Windows.Forms.Button();
            this.btnReadRelayType = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReadPersonElevatorAccess
            // 
            this.btnReadPersonElevatorAccess.Location = new System.Drawing.Point(16, 34);
            this.btnReadPersonElevatorAccess.Name = "btnReadPersonElevatorAccess";
            this.btnReadPersonElevatorAccess.Size = new System.Drawing.Size(75, 23);
            this.btnReadPersonElevatorAccess.TabIndex = 0;
            this.btnReadPersonElevatorAccess.Text = "读取";
            this.btnReadPersonElevatorAccess.UseVisualStyleBackColor = true;
            this.btnReadPersonElevatorAccess.Click += new System.EventHandler(this.btnReadPersonElevatorAccess_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnWritePersonElevatorAccess);
            this.groupBox1.Controls.Add(this.btnReadPersonElevatorAccess);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(186, 69);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "人员电梯权限";
            // 
            // btnWritePersonElevatorAccess
            // 
            this.btnWritePersonElevatorAccess.Location = new System.Drawing.Point(97, 34);
            this.btnWritePersonElevatorAccess.Name = "btnWritePersonElevatorAccess";
            this.btnWritePersonElevatorAccess.Size = new System.Drawing.Size(75, 23);
            this.btnWritePersonElevatorAccess.TabIndex = 1;
            this.btnWritePersonElevatorAccess.Text = "设置";
            this.btnWritePersonElevatorAccess.UseVisualStyleBackColor = true;
            this.btnWritePersonElevatorAccess.Click += new System.EventHandler(this.btnWritePersonElevatorAccess_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox7);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(12, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(680, 417);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "电梯功能";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnUnlockRelay);
            this.groupBox7.Controls.Add(this.btnLockRelay);
            this.groupBox7.Controls.Add(this.btnHoldRelay);
            this.groupBox7.Controls.Add(this.btnCloseRelay);
            this.groupBox7.Controls.Add(this.btnOpenRelay);
            this.groupBox7.Location = new System.Drawing.Point(16, 192);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(566, 69);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "远程操作";
            // 
            // btnUnlockRelay
            // 
            this.btnUnlockRelay.Location = new System.Drawing.Point(340, 34);
            this.btnUnlockRelay.Name = "btnUnlockRelay";
            this.btnUnlockRelay.Size = new System.Drawing.Size(75, 23);
            this.btnUnlockRelay.TabIndex = 4;
            this.btnUnlockRelay.Text = "解锁";
            this.btnUnlockRelay.UseVisualStyleBackColor = true;
            this.btnUnlockRelay.Click += new System.EventHandler(this.btnUnlockRelay_Click);
            // 
            // btnLockRelay
            // 
            this.btnLockRelay.Location = new System.Drawing.Point(259, 34);
            this.btnLockRelay.Name = "btnLockRelay";
            this.btnLockRelay.Size = new System.Drawing.Size(75, 23);
            this.btnLockRelay.TabIndex = 3;
            this.btnLockRelay.Text = "锁定";
            this.btnLockRelay.UseVisualStyleBackColor = true;
            this.btnLockRelay.Click += new System.EventHandler(this.btnLockRelay_Click);
            // 
            // btnHoldRelay
            // 
            this.btnHoldRelay.Location = new System.Drawing.Point(178, 34);
            this.btnHoldRelay.Name = "btnHoldRelay";
            this.btnHoldRelay.Size = new System.Drawing.Size(75, 23);
            this.btnHoldRelay.TabIndex = 2;
            this.btnHoldRelay.Text = "常开";
            this.btnHoldRelay.UseVisualStyleBackColor = true;
            this.btnHoldRelay.Click += new System.EventHandler(this.btnHoldRelay_Click);
            // 
            // btnCloseRelay
            // 
            this.btnCloseRelay.Location = new System.Drawing.Point(97, 34);
            this.btnCloseRelay.Name = "btnCloseRelay";
            this.btnCloseRelay.Size = new System.Drawing.Size(75, 23);
            this.btnCloseRelay.TabIndex = 1;
            this.btnCloseRelay.Text = "关门";
            this.btnCloseRelay.UseVisualStyleBackColor = true;
            this.btnCloseRelay.Click += new System.EventHandler(this.btnCloseRelay_Click);
            // 
            // btnOpenRelay
            // 
            this.btnOpenRelay.Location = new System.Drawing.Point(16, 34);
            this.btnOpenRelay.Name = "btnOpenRelay";
            this.btnOpenRelay.Size = new System.Drawing.Size(75, 23);
            this.btnOpenRelay.TabIndex = 0;
            this.btnOpenRelay.Text = "开门";
            this.btnOpenRelay.UseVisualStyleBackColor = true;
            this.btnOpenRelay.Click += new System.EventHandler(this.btnOpenRelay_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnWriteWorkType);
            this.groupBox6.Controls.Add(this.btnReadWorkType);
            this.groupBox6.Location = new System.Drawing.Point(16, 20);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(186, 69);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "工作模式";
            // 
            // btnWriteWorkType
            // 
            this.btnWriteWorkType.Location = new System.Drawing.Point(97, 34);
            this.btnWriteWorkType.Name = "btnWriteWorkType";
            this.btnWriteWorkType.Size = new System.Drawing.Size(75, 23);
            this.btnWriteWorkType.TabIndex = 1;
            this.btnWriteWorkType.Text = "设置";
            this.btnWriteWorkType.UseVisualStyleBackColor = true;
            this.btnWriteWorkType.Click += new System.EventHandler(this.btnWriteWorkType_Click);
            // 
            // btnReadWorkType
            // 
            this.btnReadWorkType.Location = new System.Drawing.Point(16, 34);
            this.btnReadWorkType.Name = "btnReadWorkType";
            this.btnReadWorkType.Size = new System.Drawing.Size(75, 23);
            this.btnReadWorkType.TabIndex = 0;
            this.btnReadWorkType.Text = "读取";
            this.btnReadWorkType.UseVisualStyleBackColor = true;
            this.btnReadWorkType.Click += new System.EventHandler(this.btnReadWorkType_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnWriteTimingOpen);
            this.groupBox5.Controls.Add(this.btnReadTimingOpen);
            this.groupBox5.Location = new System.Drawing.Point(396, 105);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(186, 69);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "定时常开";
            // 
            // btnWriteTimingOpen
            // 
            this.btnWriteTimingOpen.Location = new System.Drawing.Point(97, 34);
            this.btnWriteTimingOpen.Name = "btnWriteTimingOpen";
            this.btnWriteTimingOpen.Size = new System.Drawing.Size(75, 23);
            this.btnWriteTimingOpen.TabIndex = 1;
            this.btnWriteTimingOpen.Text = "设置";
            this.btnWriteTimingOpen.UseVisualStyleBackColor = true;
            this.btnWriteTimingOpen.Click += new System.EventHandler(this.btnWriteTimingOpen_Click);
            // 
            // btnReadTimingOpen
            // 
            this.btnReadTimingOpen.Location = new System.Drawing.Point(16, 34);
            this.btnReadTimingOpen.Name = "btnReadTimingOpen";
            this.btnReadTimingOpen.Size = new System.Drawing.Size(75, 23);
            this.btnReadTimingOpen.TabIndex = 0;
            this.btnReadTimingOpen.Text = "读取";
            this.btnReadTimingOpen.UseVisualStyleBackColor = true;
            this.btnReadTimingOpen.Click += new System.EventHandler(this.btnReadTimingOpen_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnWriteReleaseTime);
            this.groupBox4.Controls.Add(this.btnReadReleaseTime);
            this.groupBox4.Location = new System.Drawing.Point(204, 105);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(186, 69);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "继电器输出时间";
            // 
            // btnWriteReleaseTime
            // 
            this.btnWriteReleaseTime.Location = new System.Drawing.Point(97, 34);
            this.btnWriteReleaseTime.Name = "btnWriteReleaseTime";
            this.btnWriteReleaseTime.Size = new System.Drawing.Size(75, 23);
            this.btnWriteReleaseTime.TabIndex = 1;
            this.btnWriteReleaseTime.Text = "设置";
            this.btnWriteReleaseTime.UseVisualStyleBackColor = true;
            this.btnWriteReleaseTime.Click += new System.EventHandler(this.btnWriteReleaseTime_Click);
            // 
            // btnReadReleaseTime
            // 
            this.btnReadReleaseTime.Location = new System.Drawing.Point(16, 34);
            this.btnReadReleaseTime.Name = "btnReadReleaseTime";
            this.btnReadReleaseTime.Size = new System.Drawing.Size(75, 23);
            this.btnReadReleaseTime.TabIndex = 0;
            this.btnReadReleaseTime.Text = "读取";
            this.btnReadReleaseTime.UseVisualStyleBackColor = true;
            this.btnReadReleaseTime.Click += new System.EventHandler(this.btnReadReleaseTime_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnWriteRelayType);
            this.groupBox3.Controls.Add(this.btnReadRelayType);
            this.groupBox3.Location = new System.Drawing.Point(12, 105);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(186, 69);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "继电器类型";
            // 
            // btnWriteRelayType
            // 
            this.btnWriteRelayType.Location = new System.Drawing.Point(97, 34);
            this.btnWriteRelayType.Name = "btnWriteRelayType";
            this.btnWriteRelayType.Size = new System.Drawing.Size(75, 23);
            this.btnWriteRelayType.TabIndex = 1;
            this.btnWriteRelayType.Text = "设置";
            this.btnWriteRelayType.UseVisualStyleBackColor = true;
            this.btnWriteRelayType.Click += new System.EventHandler(this.btnWriteRelayType_Click);
            // 
            // btnReadRelayType
            // 
            this.btnReadRelayType.Location = new System.Drawing.Point(16, 34);
            this.btnReadRelayType.Name = "btnReadRelayType";
            this.btnReadRelayType.Size = new System.Drawing.Size(75, 23);
            this.btnReadRelayType.TabIndex = 0;
            this.btnReadRelayType.Text = "读取";
            this.btnReadRelayType.UseVisualStyleBackColor = true;
            this.btnReadRelayType.Click += new System.EventHandler(this.btnReadRelayType_Click);
            // 
            // frmElevator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 516);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmElevator";
            this.Text = "电梯模块";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReadPersonElevatorAccess;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnWritePersonElevatorAccess;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnWriteReleaseTime;
        private System.Windows.Forms.Button btnReadReleaseTime;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnWriteRelayType;
        private System.Windows.Forms.Button btnReadRelayType;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnUnlockRelay;
        private System.Windows.Forms.Button btnLockRelay;
        private System.Windows.Forms.Button btnHoldRelay;
        private System.Windows.Forms.Button btnCloseRelay;
        private System.Windows.Forms.Button btnOpenRelay;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnWriteWorkType;
        private System.Windows.Forms.Button btnReadWorkType;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnWriteTimingOpen;
        private System.Windows.Forms.Button btnReadTimingOpen;
    }
}