
using DoNetDrive.Protocol.POS.SystemParameter.Buzzer;
using DoNetDrive.Protocol.POS.SystemParameter.Cache;
using DoNetDrive.Protocol.POS.SystemParameter.ConnectPassword;
using DoNetDrive.Protocol.POS.SystemParameter.ConsumeLogStatisticsTime;
using DoNetDrive.Protocol.POS.SystemParameter.Deadline;
using DoNetDrive.Protocol.POS.SystemParameter.ForbiddenMifareOne;
using DoNetDrive.Protocol.POS.SystemParameter.ICSection;
using DoNetDrive.Protocol.POS.SystemParameter.ReceiptPrint;
using DoNetDrive.Protocol.POS.SystemParameter.RecordStorageMode;
using DoNetDrive.Protocol.POS.SystemParameter.Relay;
using DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay;
using DoNetDrive.Protocol.POS.SystemParameter.SN;
using DoNetDrive.Protocol.POS.SystemParameter.TCPSetting;
using DoNetDrive.Protocol.POS.SystemParameter.USBDisk;
using DoNetDrive.Protocol.POS.SystemParameter.Version;
using DoNetDrive.Protocol.POS.SystemParameter.Voice;
using DoNetDrive.Protocol.POS.SystemParameter.WIFIAccount;
using DoNetTool.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetDrive.Protocol.POS.Test
{
    public partial class frmSystem : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static frmSystem onlyObj;
        public static frmSystem GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmSystem(main);
                        FrmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private frmSystem(INMain main) : base(main)
        {
            InitializeComponent();
        }
        #endregion

        public frmSystem()
        {
            InitializeComponent();
        }

        string[] mRecordStorage = { "满循环", "满报警" };
        string[] mLED = { "常亮", "常灭", "刷卡时亮" };
        List<string> mPrintLocation = new List<string> { "","页头", "页尾" };
        List<string> mRelayMode = new List<string> { "COM-NO", "COM-NC", "双稳态" };

        #region SN
        private void butReadSN_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadSN cmd = new ReadSN(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                SN_Result result = cmde.Command.getResult() as SN_Result;
                string sn = "SN：" + Encoding.ASCII.GetString(result.SNBuf);
                Invoke(() =>
                {
                    txtSN.Text = sn;
                });
                mMainForm.AddCmdLog(cmde, sn);
            };
        }

        private void butWriteSN_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region 密码
        private void butReadConnectPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadConnectPassword cmd = new ReadConnectPassword(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                Password_Result result = cmde.Command.getResult() as Password_Result;
                string pwd = result.Password;
                Invoke(() =>
                {
                    txtConnectPassword.Text = pwd;
                });
                mMainForm.AddCmdLog(cmde, pwd);
            };
        }

        private void butWriteConnectPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            Password_Parameter par = new Password_Parameter(txtConnectPassword.Text);
            WriteConnectPassword cmd = new WriteConnectPassword(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };

        }

        #endregion
        private void btnReadVersion_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadVersion cmd = new ReadVersion(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadVersion_Result result = cmde.Command.getResult() as ReadVersion_Result;
                string version = result.Version.ToString();
                Invoke(() =>
                {
                    txtVersion.Text = "Ver " + version;
                });
                version = "版本号：" + version;
                mMainForm.AddCmdLog(cmde, version);
            };
        }

        #region TCP
        private void butRendTCPSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTCPSetting cmd = new ReadTCPSetting(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTCPSetting_Result result = cmde.Command.getResult() as ReadTCPSetting_Result;
                Invoke(() =>
                {
                    txtMAC.Text = result.TCP.mMAC;
                    txtIP.Text = result.TCP.mIP;
                    txtDNS.Text = result.TCP.mDNS;
                    txtDNSBackup.Text = result.TCP.mDNSBackup;
                    txtUDPPort.Text = result.TCP.mUDPPort.ToString();
                    txtTCPPort.Text = result.TCP.mTCPPort.ToString();
                    txtServerPort.Text = result.TCP.mServerPort.ToString();
                    txtServerIP.Text = result.TCP.mServerIP;
                    txtIPGateway.Text = result.TCP.mIPGateway;
                    txtIPMask.Text = result.TCP.mIPMask;
                    cbxProtocolType.SelectedIndex = result.TCP.mProtocolType;
                    cbxServerAddrType.SelectedIndex = result.TCP.mServerAddrType - 1;
                    cbxAutoIP.SelectedIndex = result.TCP.mAutoIP ? 1 : 0;

                });
                string TCPInfo = DebugTCPDetail(result.TCP);
                mMainForm.AddCmdLog(cmde, TCPInfo);
                //mMainForm.AddCmdLog(cmde, result.Deadline.ToString("yyyy-MM-dd"));
            };
        }

        private string DebugTCPDetail(TCPDetail tcp)
        {
            string MAC = tcp.mMAC; //MAC地址
            string IP = tcp.mIP; //IP
            string IPMask = tcp.mIPMask; //子网掩码
            string IPGateway = tcp.mIPGateway; //网关地址
            string DNS = tcp.mDNS; //DNS
            string DNSBackup = tcp.mDNSBackup; //备用DNS
            string TCPPort = tcp.mTCPPort.ToString(); //本地TCP端口
            string UDPPort = tcp.mUDPPort.ToString(); //本地UDP端口
            string ServerIP = tcp.mServerIP; //服务器IP
            string ServerAddr = tcp.mServerAddr; //服务器域名
            string ServerPort = tcp.mServerPort.ToString(); //服务器端口

            int ProtocolType = tcp.mProtocolType; //TCP工作模式
            bool AutoIP = tcp.mAutoIP; //是否自动获得IP


            string TCPInfo = "MAC地址：" + MAC +
                             "  IP：" + IP +
                             "  子网掩码：" + IPMask +
                             "  网关地址：" + IPGateway +
                             "  DNS：" + DNS +
                             "  备用DNS：" + DNSBackup +
                             "  本地TCP端口：" + TCPPort +
                             "  本地UDP端口：" + UDPPort +
                             "  服务器IP：" + ServerIP +
                             "  服务器域名：" + ServerAddr +
                             "  服务器端口：" + ServerPort;
            return TCPInfo;
        }

        private void butWriteTCPSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;


            string reg = @"([A-Fa-f0-9]{2}-){5}[A-Fa-f0-9]{2}";
            string reg2 = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            string reg3 = @"^\+?[1-9][0-9]*$";
            string reg4 = @"^(?=^.{3,255}$)[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+$";

            if (!Regex.IsMatch(txtMAC.Text.Trim(), reg))
            {
                MsgErr("请输入正确MAC地址！");
                return;
            }

            if (!Regex.IsMatch(txtIP.Text.Trim(), reg2))
            {
                MsgErr("请输入正确IP地址！");
                return;
            }
            if (!Regex.IsMatch(txtIPMask.Text.Trim(), reg2))
            {
                MsgErr("请输入正确子网掩码！");
                return;
            }
            if (!Regex.IsMatch(txtIPGateway.Text.Trim(), reg2))
            {
                MsgErr("请输入正确网关IP！");
                return;
            }
            if (!Regex.IsMatch(txtDNS.Text.Trim(), reg2))
            {
                MsgErr("请输入正确DNS！");
                return;
            }
            if (!Regex.IsMatch(txtDNSBackup.Text.Trim(), reg2))
            {
                MsgErr("请输入正确备用DNS！");
                return;
            }
            if (!Regex.IsMatch(txtServerIP.Text.Trim(), reg2))
            {
                MsgErr("请输入正确服务器IP！");
                return;
            }
            if (!Regex.IsMatch(txtTCPPort.Text.Trim(), reg3))
            {
                MsgErr("请输入正确本地TCP端口！");
                return;
            }
            if (Convert.ToInt32(txtTCPPort.Text.Trim()) > 65535)
            {
                MsgErr("请输入正确本地TCP端口！");
                return;
            }
            if (!Regex.IsMatch(txtUDPPort.Text.Trim(), reg3))
            {
                MsgErr("请输入正确本地UDP端口！");
                return;
            }
            if (Convert.ToInt32(txtUDPPort.Text.Trim()) > 65535)
            {
                MsgErr("请输入正确本地TCP端口！");
                return;
            }
            if (!Regex.IsMatch(txtServerPort.Text.Trim(), reg3))
            {
                MsgErr("请输入正确服务器端口！");
                return;
            }
            if (Convert.ToInt32(txtServerPort.Text.Trim()) > 65535)
            {
                MsgErr("请输入正确服务器端口！");
                return;
            }
            //if (!Regex.IsMatch(txtServerAddr.Text.Trim(), reg4))
            //{
            //    MsgErr("请输入正确服务器域名！");
            //    return;
            //}
            if (Convert.ToInt16(cbxProtocolType.SelectedIndex) == 0)
            {
                MsgErr("请选择TCP工作模式！");
                return;
            }
            if (Convert.ToInt16(cbxAutoIP.SelectedIndex) == -1)
            {
                MsgErr("请选择是否自动获得IP！");
                return;
            }

            TCPDetail tcp = new TCPDetail();
            tcp.mIP = txtIP.Text.Trim();
            tcp.mMAC = txtMAC.Text.Trim();
            tcp.mIPMask = txtIPMask.Text.Trim();
            tcp.mIPGateway = txtIPGateway.Text.Trim();
            tcp.mDNS = txtDNS.Text.Trim();
            tcp.mDNSBackup = txtDNSBackup.Text.Trim();
            tcp.mTCPPort = Convert.ToInt32(txtTCPPort.Text.Trim());
            tcp.mUDPPort = Convert.ToInt32(txtUDPPort.Text.Trim());
            tcp.mServerIP = txtServerIP.Text.Trim();
            tcp.mServerAddr = txtServerAddr.Text.Trim();
            tcp.mServerPort = Convert.ToInt32(txtServerPort.Text.Trim());

            tcp.mProtocolType = Convert.ToUInt16(cbxProtocolType.SelectedIndex);

            if (cbxAutoIP.SelectedIndex == 1)
            {
                tcp.mAutoIP = true;
            }
            else
            {
                tcp.mAutoIP = false;
            }
            WriteTCPSetting_Parameter par = new WriteTCPSetting_Parameter(tcp);
            WriteTCPSetting cmd = new WriteTCPSetting(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        #endregion

        private void butReadDeadline_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadDeadline cmd = new ReadDeadline(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadDeadline_Result result = cmde.Command.getResult() as ReadDeadline_Result;
                Invoke(() =>
                {
                    dtDeadline.Value = result.Deadline;
                });
                mMainForm.AddCmdLog(cmde, result.Deadline.ToString("yyyy-MM-dd"));
            };
        }

        private void butWriteDeadline_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteDeadline_Parameter par = new WriteDeadline_Parameter(dtDeadline.Value);
            WriteDeadline cmd = new WriteDeadline(cmdDtl,par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };

        }

        private void butReadRecordStorageMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadRecordStorageMode cmd = new ReadRecordStorageMode(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadRecordStorageMode_Result result = cmde.Command.getResult() as ReadRecordStorageMode_Result;
                Invoke(() =>
                {
                    cmbRecordStorageMode.SelectedIndex = result.Mode;
                });
                string tip = $"记录存储方式：{mRecordStorage[result.Mode]}";
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteRecordStorageMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteRecordStorageMode_Parameter par = new WriteRecordStorageMode_Parameter(cmbRecordStorageMode.SelectedIndex);
            WriteRecordStorageMode cmd = new WriteRecordStorageMode(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };

        }

        private void frmSystem_Load(object sender, EventArgs e)
        {
            cmbRecordStorageMode.Items.AddRange(mRecordStorage);
            cmbRecordStorageMode.SelectedIndex = 0;

            cmbRelayMode.Items.AddRange(mRelayMode.ToArray());
            cmbRelayMode.SelectedIndex = 0;

            cmbLED.Items.AddRange(mLED);
            cmbLED.SelectedIndex = 0;
        }

        private void butReadName_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadName cmd = new ReadName(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadName_Result result = cmde.Command.getResult() as ReadName_Result;
                Invoke(() =>
                {
                    txtName.Text = result.Name;
                });
                string tip = $"消费机名称：{result.Name}";
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteName_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteName_Parameter par = new WriteName_Parameter(txtName.Text.Trim());
            WriteName cmd = new WriteName(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private void butReadTitle_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadTitle cmd = new ReadTitle(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTitle_Result result = cmde.Command.getResult() as ReadTitle_Result;
                Invoke(() =>
                {
                    txtTitle.Text = result.Title;
                });
                string tip = $"消费机标题：{result.Title}";
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteTitle_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteTitle_Parameter par = new WriteTitle_Parameter(txtTitle.Text.Trim());
            WriteTitle cmd = new WriteTitle(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private void butReadMessage_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadMessage cmd = new ReadMessage(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadMessage_Result result = cmde.Command.getResult() as ReadMessage_Result;
                Invoke(() =>
                {
                    txtMessage.Text = result.Message;
                });
                string tip = $"短消息：{result.Message}";
                mMainForm.AddCmdLog(cmde, tip);
            };

        }

        private void butWriteMessage_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteMessage_Parameter par = new WriteMessage_Parameter(txtMessage.Text.Trim());
            WriteMessage cmd = new WriteMessage(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private void butReadShow_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadDisplayContent cmd = new ReadDisplayContent(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadDisplayContent_Result result = cmde.Command.getResult() as ReadDisplayContent_Result;
                bool showName = result.Name == 1;
                bool showPCode = result.PCode == 1;
                bool showDept = result.Dept == 1;
                bool showBalance = result.Balance == 1;
                Invoke(() =>
                {
                    cbShowName.Checked = showName;
                    cbShowPcode.Checked = showPCode;
                    cbShowDept.Checked = showDept;
                    cbShowMoney.Checked = showBalance;
                });
                string tip = $"消费时显示内容：人名:{(showName ? "ON" : "OFF")}，编号:{(showPCode ? "ON" : "OFF")}，部门：{(showDept ? "ON" : "OFF")}，余额：{(showBalance ? "ON" : "OFF")}";
                mMainForm.AddCmdLog(cmde, tip);
                //mMainForm.AddCmdLog(cmde, result.Message);
            };

        }

        private void butWriteShow_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte bShowName = (byte)(cbShowName.Checked ? 1 : 0);
            byte bShowPcode = (byte)(cbShowPcode.Checked ? 1 : 0);
            byte bShowDept = (byte)(cbShowDept.Checked ? 1 : 0);
            byte bShowMoney = (byte)(cbShowMoney.Checked ? 1 : 0);
            WriteDisplayContent_Parameter par = new WriteDisplayContent_Parameter(bShowName,bShowPcode,bShowDept,0,bShowMoney);
            WriteDisplayContent cmd = new WriteDisplayContent(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        private void butReadLogo_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadLogo cmd = new ReadLogo(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadLogo_Result result = cmde.Command.getResult() as ReadLogo_Result;
                Invoke(() =>
                {
                    txtLogo.Text = result.Logo;
                    txtPhone.Text = result.Phone;
                });
                string tip = $"供应商Logo：{result.Logo},服务电话：{result.Phone}。";
                mMainForm.AddCmdLog(cmde, tip);
                //mMainForm.AddCmdLog(cmde, result.Message);
            };
        }

        private void butWriteLogo_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte bShowName = (byte)(cbShowName.Checked ? 1 : 0);
            byte bShowPcode = (byte)(cbShowPcode.Checked ? 1 : 0);
            byte bShowDept = (byte)(cbShowDept.Checked ? 1 : 0);
            byte bShowMoney = (byte)(cbShowMoney.Checked ? 1 : 0);
            WriteLogo_Parameter par = new WriteLogo_Parameter(txtLogo.Text,txtPhone.Text);
            WriteLogo cmd = new WriteLogo(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        private void btnReadUSBDisk_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadUSBDisk cmd = new ReadUSBDisk(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadUSBDisk_Result result = cmde.Command.getResult() as ReadUSBDisk_Result;
                bool bUploadCardList = result.UploadCardList == 1;
                bool bUploadMenu = result.UploadMenu == 1;
                bool bUploadTimeGroup = result.UploadTimeGroup == 1;
                bool bUploadCardTypeList = result.UploadCardTypeList == 1;
                bool bUploadConsumeParameter = result.UploadConsumeParameter == 1;
                bool bUploadUpgrade = result.UploadUpgrade == 1;

                bool bDownloadCardList = result.DownloadCardList == 1;
                bool bDownloadMenu = result.DownloadMenu == 1;
                bool bDownloadTimeGroup = result.DownloadTimeGroup == 1;
                bool bDownloadCardTypeList = result.DownloadCardTypeList == 1;
                bool bDownloadConsumeParameter = result.DownloadConsumeParameter == 1;
                bool bDownloadTransaction = result.DownloadTransaction == 1;
                bool bDownloadSystemTransaction = result.DownloadSystemTransaction == 1;

                Invoke(() =>
                {
                    cbUploadCardList.Checked = bUploadCardList;
                    cbUploadMenu.Checked = bUploadMenu;
                    cbUploadTimeGroup.Checked = bUploadTimeGroup;
                    cbUploadCardTypeList.Checked = bUploadCardTypeList;
                    cbUploadConsumeParameter.Checked = bUploadConsumeParameter;
                    cbUpgrade.Checked = bUploadUpgrade;

                    cbDownloadCardList.Checked = bDownloadCardList;
                    cbDownloadMenu.Checked = bDownloadMenu;
                    cbDownloadTimeGroup.Checked = bDownloadTimeGroup;
                    cbDownloadCardTypeList.Checked = bDownloadCardTypeList;
                    cbDownloadConsumeParameter.Checked = bDownloadConsumeParameter;
                    cbDownloadTransaction.Checked = bDownloadTransaction;
                    cbDownloadSystemTransaction.Checked = bDownloadSystemTransaction;
                });
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"U盘上传功能：卡片名单:{(bUploadCardList ? "ON" : "OFF")}，卡类名单:{(bUploadCardTypeList ? "ON" : "OFF")}，商品菜单:{(bUploadMenu ? "ON" : "OFF")}，消费时段：{(bUploadTimeGroup ? "ON" : "OFF")}，消费参数：{(bUploadConsumeParameter ? "ON" : "OFF")}，固件升级：{(bUploadUpgrade ? "ON" : "OFF")}");
                sb.AppendLine($"U盘下载功能：卡片名单:{(bDownloadCardList ? "ON" : "OFF")}，卡类名单:{(bDownloadCardTypeList ? "ON" : "OFF")}，商品菜单:{(bDownloadMenu ? "ON" : "OFF")}" +
                    $"，消费时段：{(bDownloadTimeGroup ? "ON" : "OFF")}，消费参数：{(bDownloadConsumeParameter ? "ON" : "OFF")}，消费日志：{(bDownloadTransaction ? "ON" : "OFF")}，系统日志：{(bDownloadSystemTransaction ? "ON" : "OFF")}");
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void btnWriteUSBDisk_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte bUploadCardList = (byte)(cbUploadCardList.Checked ? 1 : 0);
            byte bUploadMenu = (byte)(cbUploadMenu.Checked ? 1 : 0);
            byte bUploadTimeGroup = (byte)(cbUploadTimeGroup.Checked ? 1 : 0);
            byte bUploadCardTypeList = (byte)(cbUploadCardTypeList.Checked ? 1 : 0);

            byte bUploadConsumeParameter = (byte)(cbUploadConsumeParameter.Checked ? 1 : 0);
            byte bUpgrade = (byte)(cbUpgrade.Checked ? 1 : 0);
            byte bDownloadCardList = (byte)(cbDownloadCardList.Checked ? 1 : 0);
            byte bDownloadMenu = (byte)(cbDownloadMenu.Checked ? 1 : 0);
            byte bDownloadTimeGroup = (byte)(cbDownloadTimeGroup.Checked ? 1 : 0);
            byte bDownloadCardTypeList = (byte)(cbDownloadCardTypeList.Checked ? 1 : 0);
            byte bDownloadConsumeParameter = (byte)(cbDownloadConsumeParameter.Checked ? 1 : 0);
            byte bDownloadTransaction = (byte)(cbDownloadTransaction.Checked ? 1 : 0);
            byte bDownloadSystemTransaction = (byte)(cbDownloadSystemTransaction.Checked ? 1 : 0);
            WriteUSBDisk_Parameter par = new WriteUSBDisk_Parameter(bUploadCardList,bUploadMenu,bUploadTimeGroup,bUploadCardTypeList,bUploadConsumeParameter,bUpgrade,bDownloadCardList,bDownloadMenu
                ,bDownloadTimeGroup,bDownloadCardTypeList,bDownloadConsumeParameter,bDownloadTransaction,bDownloadSystemTransaction);
            WriteUSBDisk cmd = new WriteUSBDisk(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "命令执行成功");
            };
        }

        private void butReadPrintCount_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReceiptPrint cmd = new ReadReceiptPrint(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadReceiptPrint_Result result = cmde.Command.getResult() as ReadReceiptPrint_Result;
                bool bIsOpen = result.IsOpen == 1;
                Invoke(() =>
                {
                    cbIsOpenPrint.Checked = bIsOpen;
                    cmbPrintCount.SelectedIndex = result.PrintCount - 1;
                });
                string tip = $"小票打印功能：{(bIsOpen ? "开启" : "关闭")}，份数：{result.PrintCount}";
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWritePrintCount_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte bIsOpenPrint = (byte)(cbIsOpenPrint.Checked ? 1 : 0);
            byte bPrintCount = (byte)(cmbPrintCount.SelectedIndex + 1);
            WriteReceiptPrint_Parameter par = new WriteReceiptPrint_Parameter(bIsOpenPrint, bPrintCount);
            WriteReceiptPrint cmd = new WriteReceiptPrint(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private void butReadPrintContent_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadPrintContent cmd = new ReadPrintContent(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadReceiptPrint_Result result = cmde.Command.getResult() as ReadReceiptPrint_Result;
                bool bIsOpen = result.IsOpen == 1;
                Invoke(() =>
                {
                    dgvPrintContent.AutoGenerateColumns = false;
                    dgvPrintContent.DataSource = new BindingList<PrintContent>(result.PrintContents);
                    //dgvPrintContent.DataSource = result.PrintContents;
                });
                StringBuilder sb = new StringBuilder($"打印内容：");
                foreach (var item in result.PrintContents)
                {
                    sb.AppendLine($"序号:{item.Index},状态:{(item.IsOpen == 1 ? "启用":"禁用")},打印内容:{item.Content},位置:{item.ShowLocation}");
                }
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void butWritePrintContent_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            List<PrintContent> printContents = new List<PrintContent>(6);
            for (int i = 0; i < dgvPrintContent.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cellIsOpen = (DataGridViewCheckBoxCell)dgvPrintContent.Rows[i].Cells[1];
                DataGridViewTextBoxCell cellContent = (DataGridViewTextBoxCell)dgvPrintContent.Rows[i].Cells[2];
                DataGridViewComboBoxCell cellLocation = (DataGridViewComboBoxCell)dgvPrintContent.Rows[i].Cells[3];

                PrintContent model = new PrintContent();
                model.Index = (byte)(i + 1);
                if ((bool)cellIsOpen.FormattedValue)
                    model.IsOpen =  1;
                else
                    model.IsOpen = 0;
                model.Content = cellContent.Value.ToString();
                model.Location = (byte)mPrintLocation.FindIndex(t => t == cellLocation.Value.ToString());
                printContents.Add(model);
            }
            WriteReceiptPrint_Parameter par = new WriteReceiptPrint_Parameter(printContents);
            WritePrintContent cmd = new WritePrintContent(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private void dgvPrintContent_CellMouseClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DataGridViewTextBoxColumn textbox = dgvPrintContent.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
                if (textbox != null) //如果该列是TextBox列
                {
                    dgvPrintContent.BeginEdit(true); //开始编辑状态
                    dgvPrintContent.ReadOnly = false;
                }

            }
            if (e.ColumnIndex == 3)
            {
                DataGridViewComboBoxColumn combobox = dgvPrintContent.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
                if (combobox != null) //如果该列是TextBox列
                {
                    dgvPrintContent.BeginEdit(true); //开始编辑状态
                    dgvPrintContent.ReadOnly = false;
                }
            }
            if (e.ColumnIndex == 1)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dgvPrintContent.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if ((bool)cell.FormattedValue)
                {
                    cell.Value = false;
                    cell.EditingCellFormattedValue = false;
                }
                else
                {
                    cell.Value = true;
                    cell.EditingCellFormattedValue = true;
                }
            }
        }

        private void butReadVoice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadVoice cmd = new ReadVoice(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadVoice_Result result = cmde.Command.getResult() as ReadVoice_Result;
                bool bIsOpen = result.IsOpen == 1;
                bool bCardMoney = result.CardMoney == 1;
                bool bPayMoney = result.PayMoney == 1;
                bool bPasswordTip = result.PasswordTip == 1;
                bool bBlackList = result.BlackList == 1;
                bool bErrorTip = result.ErrorTip == 1;
                Invoke(() =>
                {
                    cbVoiceIsOpen.Checked = bIsOpen;
                    cbVoiceMoney.Checked = bCardMoney;
                    cbVoicePayMoney.Checked = bPayMoney;
                    cbVoicePasswordTip.Checked = bPasswordTip;
                    cbVoiceBlacklist.Checked = bBlackList;
                    cbVoiceErrorTip.Checked = bErrorTip;
                });
                StringBuilder sb = new StringBuilder($"开机语音：{(bIsOpen ? "启用" : "禁用")}");
                sb.AppendLine($"播报开关：卡内余额:{(bCardMoney ? "ON" : "OFF")}，消费金额:{(bPayMoney ? "ON" : "OFF")}，黑名单:{(bBlackList ? "ON" : "OFF")}，错误提示：{(bErrorTip ? "ON" : "OFF")}，刷卡或密码提示：{(bPasswordTip ? "ON" : "OFF")}");
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };

            var cmdDtl2 = mMainForm.GetCommandDetail();
            if (cmdDtl2 == null) return;
            ReadVoiceStart cmd2 = new ReadVoiceStart(cmdDtl2);
            mMainForm.AddCommand(cmd2);

            //处理返回值
            cmdDtl2.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadVoiceStart_Result result = cmde.Command.getResult() as ReadVoiceStart_Result;
                bool bStart = result.Start == 1;
                bool bAdvertisement = result.Advertisement == 1;
              
                Invoke(() =>
                {
                    cbVoiceStart.Checked = bStart;
                    cbVoiceAdvertisement.Checked = bAdvertisement;
                    
                });
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"开机语音及广告语音开关：开机欢迎语:{(bStart ? "ON" : "OFF")}，供应商广告:{(bAdvertisement ? "ON" : "OFF")}");
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void butWriteVoice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte bIsOpen = (byte)(cbVoiceIsOpen.Checked ? 1 : 0);
            byte bPayMoney = (byte)(cbVoicePayMoney.Checked ? 1 : 0);
            byte bMoney = (byte)(cbVoiceMoney.Checked ? 1 : 0);
            byte bBlacklist = (byte)(cbVoiceBlacklist.Checked ? 1 : 0);
            byte bErrorTip = (byte)(cbVoiceErrorTip.Checked ? 1 : 0);
            byte bPasswordTip = (byte)(cbVoicePasswordTip.Checked ? 1 : 0);
            WriteVoice_Parameter par = new WriteVoice_Parameter(bIsOpen,bMoney,bPayMoney,bBlacklist,bErrorTip,bPasswordTip);
            WriteVoice cmd = new WriteVoice(cmdDtl, par);
            mMainForm.AddCommand(cmd);

           

            var cmdDtl2 = mMainForm.GetCommandDetail();
            if (cmdDtl2 == null) return;
            byte bVoiceStart = (byte)(cbVoiceStart.Checked ? 1 : 0);
            byte bAdvertisement = (byte)(cbVoiceAdvertisement.Checked ? 1 : 0);
            WriteVoiceStart_Parameter par2 = new WriteVoiceStart_Parameter(bVoiceStart, bAdvertisement);
            WriteVoiceStart cmd2 = new WriteVoiceStart(cmdDtl2, par2);
            mMainForm.AddCommand(cmd2);

            
        }

        private void butReadRelay_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadRelay cmd = new ReadRelay(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadRelay_Result result = cmde.Command.getResult() as ReadRelay_Result;
                bool bIsOpen = result.Use == 1;
                string tip = $"继电器类型：{mRelayMode[result.Mode]}，输出保持(s)：{result.OutputRetention}";
                if (!bIsOpen)
                {
                    tip = "继电器已禁用";
                }
                Invoke(() =>
                {
                    cbRelayIsOpen.Checked = bIsOpen;
                    txtOutputRetention.Value = decimal.Parse(result.OutputRetention.ToString());
                    cmbRelayMode.SelectedIndex = result.Mode - 1;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteRelay_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte bIsOpen = (byte)(cbRelayIsOpen.Checked ? 1 : 0);
            byte RelayMode = (byte)(cmbRelayMode.SelectedIndex + 1);
            byte OutputRetention = (byte)(txtOutputRetention.Value);
            WriteRelay_Parameter par = new WriteRelay_Parameter(bIsOpen, RelayMode, OutputRetention);
            WriteRelay cmd = new WriteRelay(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadBuzzer_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadBuzzer cmd = new ReadBuzzer(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadBuzzer_Result result = cmde.Command.getResult() as ReadBuzzer_Result;
                bool bIsOpen = result.Buzzer == 1;
                string tip = $"蜂鸣器{(bIsOpen ? "已启用": "已禁用")}";
                Invoke(() =>
                {
                    cbBuzzerUse.Checked = bIsOpen;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteBuzzer_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte bIsOpen = (byte)(cbBuzzerUse.Checked ? 1 : 0);
            WriteBuzzer_Parameter par = new WriteBuzzer_Parameter(bIsOpen);
            WriteBuzzer cmd = new WriteBuzzer(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadWIFIAccount_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadWIFIAccount cmd = new ReadWIFIAccount(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadWIFIAccount_Result result = cmde.Command.getResult() as ReadWIFIAccount_Result;
                
                string tip = $"名称：{result.Account}，密码：{result.Password}";
                Invoke(() =>
                {
                    txtAccount.Text = result.Account;
                    txtPassword.Text = result.Password;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteWIFIAccount_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            if (txtAccount.Text.Length > 0x20)
            {
                MessageBox.Show("wifi名称长度不能超过" + 0x20);
                return;
            }
            if (txtPassword.Text.Length > 0x20)
            {
                MessageBox.Show("wifi密码长度不能超过" + 0x20);
                return;
            }
            WriteWIFIAccount_Parameter par = new WriteWIFIAccount_Parameter(txtAccount.Text, txtPassword.Text);
            WriteWIFIAccount cmd = new WriteWIFIAccount(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void btnReadCacheContent_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadCache cmd = new ReadCache(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadCache_Result result = cmde.Command.getResult() as ReadCache_Result;

                string tip = "";
                Invoke(() =>
                {
                    txtCacheContent.Text = result.Content;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void btnWriteCacheContent_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteCache_Parameter par = new WriteCache_Parameter(txtCacheContent.Text);
            WriteCache cmd = new WriteCache(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private void butReadForbiddenMifareOne_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadForbiddenMifareOne cmd = new ReadForbiddenMifareOne(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadForbiddenMifareOne_Result result = cmde.Command.getResult() as ReadForbiddenMifareOne_Result;
                bool bIsOpen = result.Use == 1;
                string tip = $"{(bIsOpen ? "已禁用" : "已启用")}MifareOne卡";
                Invoke(() =>
                {
                    cbUseForbiddenMifareOne.Checked = bIsOpen;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteForbiddenMifareOne_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte bIsOpen = (byte)(cbUseForbiddenMifareOne.Checked ? 1 : 0);
            WriteForbiddenMifareOne_Parameter par = new WriteForbiddenMifareOne_Parameter(bIsOpen);
            WriteForbiddenMifareOne cmd = new WriteForbiddenMifareOne(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadStatisticsTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadConsumeLogStatisticsTime cmd = new ReadConsumeLogStatisticsTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadConsumeLogStatisticsTime_Result result = cmde.Command.getResult() as ReadConsumeLogStatisticsTime_Result;
                
                DateTime dt = DateTime.Now;
                string strTime = result.Time.ToString().PadLeft(4, '0');
                int hour = strTime.Substring(0, 2).ToInt32();
                int minute = strTime.Substring(2, 2).ToInt32();
                string tip = $"消费日志统计时间点：{hour}:{minute}";
                Invoke(() =>
                {
                    dtpStatisticsTime.Value = new DateTime(dt.Year, dt.Month, dt.Day,hour,minute,0);
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteStatisticsTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            int time = dtpStatisticsTime.Value.ToString("HHmm").ToInt32();
            WriteConsumeLogStatisticsTime_Parameter par = new WriteConsumeLogStatisticsTime_Parameter(time);
            WriteConsumeLogStatisticsTime cmd = new WriteConsumeLogStatisticsTime(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadLed_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadLed cmd = new ReadLed(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadLed_Result result = cmde.Command.getResult() as ReadLed_Result;

                string tip = $"背光灯的工作模式：{mLED[result.Mode - 1]}";
                Invoke(() =>
                {
                    cmbLED.SelectedIndex = result.Mode - 1;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteLed_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte mode = (byte)(cmbLED.SelectedIndex + 1);
            WriteLed_Parameter par = new WriteLed_Parameter(mode);
            WriteLed cmd = new WriteLed(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void butReadConsumerCards_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadConsumerCards cmd = new ReadConsumerCards(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadConsumerCards_Result result = cmde.Command.getResult() as ReadConsumerCards_Result;

                string tip = $"消费卡参数：";
                Invoke(() =>
                {
                    txtSectorNumber.Value = result.SectorNumber;
                    txtCardPassword.Text = result.Password;
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteConsumerCards_Click(object sender, EventArgs e)
        {

        }

        private void butReadControlCards_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadControlCards cmd = new ReadControlCards(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadControlCards_Result result = cmde.Command.getResult() as ReadControlCards_Result;

                string tip = $"控制卡参数：";
                Invoke(() =>
                {
                    
                });
                mMainForm.AddCmdLog(cmde, tip);
            };
        }

        private void butWriteControlCards_Click(object sender, EventArgs e)
        {

        }
    }
}
