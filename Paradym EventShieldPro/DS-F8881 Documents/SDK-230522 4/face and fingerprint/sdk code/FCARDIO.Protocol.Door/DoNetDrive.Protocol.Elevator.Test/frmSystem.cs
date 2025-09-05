using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.SN;
using System;
using System.Windows.Forms;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ConnectPassword;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.Deadline;
using System.Text.RegularExpressions;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.Version;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.SystemStatus;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.TCPSetting;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter;
using System.Collections;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FireAlarm;
using System.Text;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter.ReaderByte;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter.InvalidCardAlarmOption;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.AlarmPassword;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter.ExpirationPrompt;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.CloseAlarm;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ManageCard;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.KeyboardCardIssuingManage;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.InputTerminalFunction;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.TCP485LineConnection;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ItemDetectionFunction;
using DoNetDrive.Protocol.Door.Door8800.Door;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter.ReaderWorkSetting;
using DoNetDrive.Protocol.Elevator.Test.Model;
using System.Collections.Generic;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using System.ComponentModel;
using System.Linq;
using DoNetDrive.Common.Extensions;

namespace DoNetDrive.Protocol.Elevator.Test
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
                        frmMain.AddNodeForms(onlyObj);
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

        string[] ReaderByteTypeList = new string[] { "三字节", "四字节", "二字节", "禁用" };
        string[] InputTerminalFunctionList = new string[] { "开锁按钮", "门磁检查" };
        string[] AlarmOptionList = new string[] { "不开门，报警输出", "开门，报警输出", "锁定门，报警，只能软件解锁" };
        List<WeekTimeGroupDto> ListWeekTimeGroupDto = new List<WeekTimeGroupDto>();
        private void FrmSystem_Load(object sender, EventArgs e)
        {
            cmbReaderByteType.Items.AddRange(ReaderByteTypeList);
            cmbReaderByteType.SelectedIndex = 0;

            cmbAlarmOption.Items.Clear();
            cmbAlarmOption.Items.AddRange(AlarmOptionList);
            cmbAlarmOption.SelectedIndex = 0;

            cmbInputTerminalFunction.Items.Clear();
            cmbInputTerminalFunction.Items.AddRange(InputTerminalFunctionList);
            cmbInputTerminalFunction.SelectedIndex = 0;
        }

        private void FrmSystem_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }

        private void ButReadSN_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadSN cmd = new ReadSN(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Door.Door8800.SystemParameter.SN.SN_Result;
                string sn = result.SNBuf.GetString();
                Invoke(() =>
                {
                    txtSN.Text = sn;
                });
                mMainForm.AddCmdLog(cmde, sn);
            };
        }

        private void ButReadConnectPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadConnectPassword cmd = new ReadConnectPassword(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Door.Door8800.SystemParameter.ConnectPassword.Password_Result;
                string pwd = result.Password;
                Invoke(() =>
                {
                    txtConnectPassword.Text = pwd;
                });
                mMainForm.AddCmdLog(cmde, pwd);
            };
        }

        private void ButWriteConnectPassword_Click(object sender, EventArgs e)
        {
            string pwd = txtConnectPassword.Text;
            if (pwd.Length != 8)
            {
                MsgErr("通讯密码 是8位十六进制字符，请重新输入！");
                return;
            }
            if (!pwd.IsHex())
            {
                MsgErr("通讯密码 是8位十六进制字符，请重新输入！");
                return;
            }


            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteConnectPassword cmd = new WriteConnectPassword(cmdDtl, new Password_Parameter(pwd));
            mMainForm.AddCommand(cmd);
        }

        private void ButResetConnectPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadConnectPassword cmd = new ReadConnectPassword(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Door.Door8800.SystemParameter.ConnectPassword.Password_Result;
                string pwd = result.Password;

                mMainForm.AddCmdLog(cmde, pwd);
            };
        }

        private void BtnReadDeadline_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadDeadline cmd = new ReadDeadline(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Door.Door8800.SystemParameter.Deadline.ReadDeadline_Result;

                ushort Deadline = result.Deadline; //有效期
                string DeadlineInfo = string.Empty;
                if (Deadline == 0)
                {
                    DeadlineInfo = "失效";
                }
                else if (Deadline == 65535)
                {
                    DeadlineInfo = "无限制(65535)";
                }
                else
                {
                    DeadlineInfo = Deadline.ToString() + "天";
                }

                Invoke(() =>
                {
                    cbxDeadline.Text = DeadlineInfo.Replace("天", "");
                });
                string DeadlineDay = "设备剩余有效天数:" + DeadlineInfo;
                mMainForm.AddCmdLog(cmde, DeadlineDay);
            };
        }

        private void BtnWriteDeadline_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxDeadline.Text.Trim(), reg))
            {
                if (cbxDeadline.Text != "无限制(65535)" && cbxDeadline.Text != "失效")
                {
                    MsgErr("请输入正确有效天数！");
                    return;
                }
            }
            if (Regex.IsMatch(cbxDeadline.Text.Trim(), reg))
            {
                if (Convert.ToUInt32(cbxDeadline.Text) < 0 || Convert.ToUInt32(cbxDeadline.Text) > 65535)
                {
                    MsgErr("请输入正确有效天数！");
                    return;
                }
            }

            ushort deadlineDay = 0;
            string deadlineInfo = cbxDeadline.Text;
            if (deadlineInfo == "无限制(65535)")
            {
                deadlineDay = 65535;
            }
            else if (deadlineInfo == "失效")
            {
                deadlineDay = 0;
            }
            else
            {
                deadlineDay = Convert.ToUInt16(cbxDeadline.Text);
            }

            //WriteDeadline_Parameter wp = new WriteDeadline_Parameter();
            //wp.Deadline = deadlineDay;
            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x02);
            //wp.GetBytes(buf);

            //WriteDeadline_Parameter wp2 = new WriteDeadline_Parameter();
            //wp2.SetBytes(buf);

            //string DeadlineDay = "设备剩余有效期天数:" + deadlineDay;
            //mMainForm.AddCmdLog(null, DeadlineDay);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteDeadline cmd = new WriteDeadline(cmdDtl, new WriteDeadline_Parameter(deadlineDay));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadVersion_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadVersion cmd = new ReadVersion(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.SystemParameter.Version.ReadVersion_Result;
                string version = result.Version.ToString();
                Invoke(() =>
                {
                    txtVersion.Text = "Ver " + version;
                });
                version = "版本号：" + version;
                mMainForm.AddCmdLog(cmde, version);
            };
        }

        private void ButRendTCPSetting_Click(object sender, EventArgs e)
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
                    txtIPMask.Text = result.TCP.mIPMask;
                    txtIPGateway.Text = result.TCP.mIPGateway;
                    txtDNS.Text = result.TCP.mDNS;
                    txtDNSBackup.Text = result.TCP.mDNSBackup;
                    txtTCPPort.Text = result.TCP.mTCPPort.ToString();
                    txtUDPPort.Text = result.TCP.mUDPPort.ToString();
                    txtServerIP.Text = result.TCP.mServerIP;
                    txtServerAddr.Text = result.TCP.mServerAddr;
                    txtServerPort.Text = result.TCP.mServerPort.ToString();

                    cbxProtocolType.SelectedIndex = result.TCP.mProtocolType;
                    cbxAutoIP.SelectedIndex = result.TCP.mAutoIP == true ? 1 : 0;
                });
                string TCPInfo = DebugTCPDetail(result.TCP);
                mMainForm.AddCmdLog(cmde, TCPInfo);
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

        private void ButWriteTCPSetting_Click(object sender, EventArgs e)
        {
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
            if (!Regex.IsMatch(txtServerAddr.Text.Trim(), reg4))
            {
                MsgErr("请输入正确服务器域名！");
                return;
            }
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

            //var buf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(0x89);
            //tcp.GetBytes(buf);

            //TCPDetail newtcp = new TCPDetail();
            //newtcp.SetBytes(buf);

            //string TCPInfo = "MAC地址:" + newtcp.mMAC +
            //                    "  IP：" + newtcp.mIP +
            //                    "  子网掩码：" + newtcp.mIPMask +
            //                    "  网关地址：" + newtcp.mIPGateway +
            //                    "  DNS：" + newtcp.mDNS +
            //                    "  备用DNS：" + newtcp.mDNSBackup +
            //                    "  本地TCP端口：" + newtcp.mTCPPort +
            //                    "  本地UDP端口：" + newtcp.mUDPPort +
            //                    "  服务器IP：" + newtcp.mServerIP +
            //                    "  服务器域名：" + newtcp.mServerAddr +
            //                    "  TCP工作模式：" + newtcp.mProtocolType +
            //                    "  自动获得IP：" + newtcp.mAutoIP +
            //                    "  服务器端口：" + newtcp.mServerPort;
            //mMainForm.AddCmdLog(null, TCPInfo);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteTCPSetting cmd = new WriteTCPSetting(cmdDtl, new WriteTCPSetting_Parameter(tcp));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadSystemStatus_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadSystemStatus cmd = new ReadSystemStatus(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.SystemParameter.SystemStatus.ReadSystemStatus_Result;
                string RunDay = result.RunDay.ToString() + "天"; //设备已运行天数
                string FormatCount = result.FormatCount.ToString() + "次"; //格式化次数
                string RestartCount = result.RestartCount.ToString() + "次"; //看门狗复位次数
                string UPS = result.UPS == 0 ? "电源取电" : "UPS供电"; //UPS工作状态
                string Temperature = result.Temperature; //设备温度
                string Voltage = result.Voltage; //接入电压
                string StartTime = result.StartTime; //上电时间

                Invoke(() =>
                {
                    txtRunDay.Text = RunDay;
                    txtFormatCount.Text = FormatCount;
                    txtRestartCount.Text = RestartCount;
                    txtUPS.Text = UPS;
                    txtTemperature.Text = Temperature;
                    txtVoltage.Text = Voltage;
                    txtStartTime.Text = StartTime;

                });
                string TCPInfo = "设备已运行天数:" + RunDay +
                                 "  格式化次数：" + FormatCount +
                                 "  看门狗复位次数：" + RestartCount +
                                 "  UPS工作状态：" + UPS +
                                 "  设备温度：" + Temperature +
                                 "  接入电压：" + Voltage +
                                 "  上电时间：" + StartTime;
                mMainForm.AddCmdLog(cmde, TCPInfo);
            };
        }

        private void BtnReadRecordMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadRecordMode cmd = new ReadRecordMode(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.SystemParameter.FunctionParameter.ReadRecordMode_Result;
                string ModeStr = result.Mode == 0 ? "【0、记录存满后，循环覆盖存储】" : "【1、满后报警，不再保存新纪录】"; //记录存储方式
                Invoke(() =>
                {
                    if (result.Mode == 0)
                    {
                        rBtnCover.Checked = true;
                    }
                    else
                    {
                        rBtnNoCover.Checked = true;
                    }
                });
                ModeStr = "记录存储方式：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteRecordMode_Click(object sender, EventArgs e)
        {
            byte mode = 0;
            if (rBtnNoCover.Checked == true)
            {
                mode = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteRecordMode cmd = new WriteRecordMode(cmdDtl, new WriteRecordMode_Parameter(mode));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadKeyboard_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadKeyboard cmd = new ReadKeyboard(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.SystemParameter.FunctionParameter.ReadKeyboard_Result;
                string KeyboardInfo = string.Empty;
                Invoke(() =>
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (result.Keyboard[i] == true)
                        {
                            if (i == 0)
                            {
                                cBox1.Checked = true;
                            }
                            else if (i == 1)
                            {
                                cBox2.Checked = true;
                            }
                            KeyboardInfo = KeyboardInfo + "  读卡器：" + (i + 1) + "，键盘开关：【1、接收键盘信号】";
                        }
                        else
                        {
                            KeyboardInfo = KeyboardInfo + "  读卡器：" + (i + 1) + "，键盘开关：【0、不接收键盘信号】";
                        }
                    }
                });
                mMainForm.AddCmdLog(cmde, KeyboardInfo);
            };
        }

        private void BtnWriteKeyboard_Click(object sender, EventArgs e)
        {
            BitArray bitSet = new BitArray(2);
            bitSet[0] = cBox1.Checked;
            bitSet[1] = cBox2.Checked;
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteKeyboard cmd = new WriteKeyboard(cmdDtl, new WriteKeyboard_Parameter(bitSet));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadFireAlarmOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadFireAlarmOption cmd = new ReadFireAlarmOption(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.SystemParameter.FunctionParameter.ReadFireAlarmOption_Result;
                int OptionType = result.Option; //消防报警参数
                string OptionTypeStr = string.Empty;
                if (OptionType == 0)
                {
                    OptionTypeStr = "【0、不启用】";
                }
                else if (OptionType == 1)
                {
                    OptionTypeStr = "【1、报警输出，并开所有门，只能软件解除】";
                }
                else if (OptionType == 2)
                {
                    OptionTypeStr = "【2、报警输出，不开所有门，只能软件解除】";
                }
                Invoke(() =>
                {
                    cbxOption.SelectedIndex = OptionType;
                });
                string Info = "消防报警参数：" + OptionTypeStr;
                mMainForm.AddCmdLog(cmde, Info);
            };
        }

        private void BtnWriteFireAlarmOption_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(cbxOption.SelectedIndex) == -1)
            {
                MsgErr("请选择消防报警模式！");
                return;
            }
            byte Option = Convert.ToByte(cbxOption.SelectedIndex);


            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteFireAlarmOption cmd = new WriteFireAlarmOption(cmdDtl, new WriteFireAlarmOption_Parameter(Option));
            mMainForm.AddCommand(cmd);
        }

        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            SendFireAlarm cmd = new SendFireAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "开启消防报警");
            };
        }

        private void BtnCloseAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            CloseFireAlarm cmd = new CloseFireAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, "解除消防报警");
            };
        }

        private void BtnAlarmState_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadFireAlarmState cmd = new ReadFireAlarmState(cmdDtl);
            mMainForm.AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                string ModeStr = cmd.FireAlarmState == 0 ? "【0、未开启报警】" : "【1、已开启报警】"; //消防报警状态
                Invoke(() =>
                {
                    mMainForm.AddCmdLog(cmde, ModeStr);
                });
            };
        }

        private void BtnReadBroadcast_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadBroadcast cmd = new ReadBroadcast(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.SystemParameter.FunctionParameter.ReadBroadcast_Result;

                Invoke(() =>
                {
                    byte[] broadcast = new byte[4];
                    Array.Copy(result.Broadcast.Broadcast, 6, broadcast, 0, 4);
                    StringBuilder BroadcastInfo = new StringBuilder(64); //语音段开关

                    Array.Reverse(broadcast);
                    BitArray bit = new BitArray(broadcast);
                    for (int i = 30; i >= 0; i--)
                    {
                        BroadcastInfo.Append(bit[i] ? 1 : 0);
                    }

                    txtBroadcast.Text = BroadcastInfo.ToString();
                    string IntervalTimeStr = "语音段开关：" + txtBroadcast.Text + "  顺序 31←1";
                    mMainForm.AddCmdLog(cmde, IntervalTimeStr);
                });

            };
        }

        private void BtnWriteBroadcast_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-1]*$";
            if (!Regex.IsMatch(txtBroadcast.Text.Trim(), reg) || txtBroadcast.Text.Trim().Length != 31)
            {
                MsgErr("请输入正确格式语音开关段设置！");
                return;
            }
            byte[] bData = new byte[10];

            string strBit = txtBroadcast.Text.Trim();
            strBit = "0" + strBit;

            byte[] tmpData = new byte[4];
            BitArray bit = new BitArray(tmpData);
            int strIndex = 0;
            for (int i = 30; i >= 0; i--)
            {
                bit[i] = (strBit.Substring(++strIndex, 1) == "1");
            }

            bit.CopyTo(tmpData, 0);
            Array.Reverse(tmpData);
            Array.Copy(tmpData, 0, bData, 6, 4);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteBroadcast cmd = new WriteBroadcast(cmdDtl, new WriteBroadcast_Parameter(bData));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadIntervalTime_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReaderIntervalTime cmd = new ReadReaderIntervalTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as ReadReaderIntervalTime_Result;

                ushort IntervalTime = result.IntervalTime; //读卡间隔时间
                string IntervalTimeInfo = string.Empty;
                if (IntervalTime == 0)
                {
                    IntervalTimeInfo = "无限制";
                }
                else
                {
                    IntervalTimeInfo = IntervalTime.ToString() + "秒";
                }

                Invoke(() =>
                {
                    cbxIntervalTime.Text = IntervalTimeInfo.Replace("秒", "");
                    cbIsUseInterval.Checked = result.IsUse;
                    cmbIntervalMode.SelectedIndex = result.Mode - 1;
                });
                string IntervalTimeStr = "读卡间隔参数：" + (result.IsUse ? "启用" : "不启用") + "，\r\n读卡间隔时间：" + IntervalTimeInfo;
                IntervalTimeStr += "，\r\n间隔时检测模式：【" + result.Mode.ToString() + "】";
                mMainForm.AddCmdLog(cmde, IntervalTimeStr);
            };
        }

        private void BtnWriteIntervalTime_Click(object sender, EventArgs e)
        {
            string reg = @"^\+?[0-9]*$";
            if (!Regex.IsMatch(cbxIntervalTime.Text.Trim(), reg))
            {
                if (cbxIntervalTime.Text != "无限制")
                {
                    MsgErr("请输入正确读卡间隔时间！");
                    return;
                }
            }
            if (Regex.IsMatch(cbxIntervalTime.Text.Trim(), reg))
            {
                if (Convert.ToUInt32(cbxIntervalTime.Text) < 0 || Convert.ToUInt32(cbxIntervalTime.Text) > 65535)
                {
                    MsgErr("请输入正确读卡间隔时间！");
                    return;
                }
            }

            ushort IntervalTime = 0;
            string deadlineInfo = cbxIntervalTime.Text;
            if (deadlineInfo == "无限制")
            {
                IntervalTime = 0;
            }
            else
            {
                IntervalTime = Convert.ToUInt16(cbxIntervalTime.Text);
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteReaderIntervalTime cmd = new WriteReaderIntervalTime(cmdDtl, new WriteReaderIntervalTime_Parameter(cbIsUseInterval.Checked, IntervalTime, (byte)(cmbIntervalMode.SelectedIndex + 1)));
            mMainForm.AddCommand(cmd);
        }

        private void CbIsUseInterval_CheckedChanged(object sender, EventArgs e)
        {
            plInterval.Visible = cbIsUseInterval.Checked;
        }

        private void BtnReadReaderCheckMode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReaderCheckMode cmd = new ReadReaderCheckMode(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.SystemParameter.FunctionParameter.ReadReaderCheckMode_Result;
                string ModeStr = string.Empty; //读卡器数据校验
                Invoke(() =>
                {
                    if (result.ReaderCheckMode == 0)
                    {
                        rBtnNoEnable.Checked = true;
                        ModeStr = "0、不启用";
                    }
                    else if (result.ReaderCheckMode == 1)
                    {
                        rBtnEnable.Checked = true;
                        ModeStr = "1、启用";
                    }
                    else
                    {
                        rBtnEnableValidation.Checked = true;
                        ModeStr = "2、启用校验";
                    }
                });
                ModeStr = "读卡器校验：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteReaderCheckMode_Click(object sender, EventArgs e)
        {
            byte mode = 0;
            if (rBtnEnable.Checked == true)
            {
                mode = 1;
            }
            else if (rBtnEnableValidation.Checked == true)
            {
                mode = 2;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteReaderCheckMode cmd = new WriteReaderCheckMode(cmdDtl, new WriteReaderCheckMode_Parameter(mode));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadBuzzer_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadBuzzer cmd = new ReadBuzzer(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Protocol.Door.Door8800.SystemParameter.FunctionParameter.ReadBuzzer_Result;
                string ModeStr = result.Buzzer == 0 ? "【0、不启用】" : "【1、启用】"; //记录存储方式
                Invoke(() =>
                {
                    if (result.Buzzer == 0)
                    {
                        rBtnNoBuzzer.Checked = true;
                    }
                    else
                    {
                        rBtnBuzzer.Checked = true;
                    }
                });
                ModeStr = "主板蜂鸣器：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteBuzzer_Click(object sender, EventArgs e)
        {
            byte buzzer = 0;
            if (rBtnBuzzer.Checked == true)
            {
                buzzer = 1;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteBuzzer cmd = new WriteBuzzer(cmdDtl, new WriteBuzzer_Parameter(buzzer));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadReaderByte_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReaderByte cmd = new ReadReaderByte(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadReaderByte_Result result = cmde.Command.getResult() as ReadReaderByte_Result;
                Invoke(() =>
                {
                    cmbReaderByteType.SelectedIndex = result.Type - 1;
                });
                string ModeStr = "读卡器字节数：【" + result.Type.ToString() + "、" + ReaderByteTypeList[result.Type - 1] + "】";
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteReaderByte_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteReaderByte cmd = new WriteReaderByte(cmdDtl, new WriteReaderByte_Parameter((byte)(cmbReaderByteType.SelectedIndex + 1)));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadInvalidCardAlarmOption_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadInvalidCardAlarmOption cmd = new ReadInvalidCardAlarmOption(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadInvalidCardAlarmOption_Result result = cmde.Command.getResult() as ReadInvalidCardAlarmOption_Result;
                string ModeStr = result.Use == 0 ? "【0、不启用】" : "【1、启用】"; //记录存储方式
                Invoke(() =>
                {
                    if (result.Use == 1)
                    {
                        rbInvalidCardAlarmOption1.Checked = true;
                    }
                    else
                    {
                        rbInvalidCardAlarmOption0.Checked = true;
                    }
                });
                ModeStr = "非法读卡报警：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteInvalidCardAlarmOption_Click(object sender, EventArgs e)
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte use = (byte)(rbInvalidCardAlarmOption1.Checked ? 1 : 0);
            WriteInvalidCardAlarmOption cmd = new WriteInvalidCardAlarmOption(cmdDtl, new WriteInvalidCardAlarmOption_Parameter(use));
            mMainForm.AddCommand(cmd);
        }

        private void ButReadAlarmPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var cmd = new ReadAlarmPassword(cmdDtl);
            mMainForm.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as AlarmPassword_Result;
                Invoke(() =>
                {
                    cbAlarmPasswordUse.Checked = result.Use;
                    cmbAlarmOption.SelectedIndex = (result.AlarmOption - 1);
                    Password.Text = result.Password;

                });

                mMainForm.AddCmdLog(cmde, $"命令成功：功能开关:{(result.Use ? "启用" : "不启用")},报警密码：{result.Password},报警选项：{AlarmOptionList[result.AlarmOption - 1]}");
            };
        }

        private void ButWriteAlarmPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            String pwd = Password.Text.ToString();
            int alarmOption = cmbAlarmOption.SelectedIndex + 1;

            var par = new WriteAlarmPassword_Parameter(cbAlarmPasswordUse.Checked, pwd, alarmOption);
            var cmd = new WriteAlarmPassword(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private void CbAlarmPasswordUse_CheckedChanged(object sender, EventArgs e)
        {
            plAlarmPassword.Visible = cbAlarmPasswordUse.Checked;
        }

        private void BtnReadExpirationPrompt_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadExpirationPrompt cmd = new ReadExpirationPrompt(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadExpirationPrompt_Result result = cmde.Command.getResult() as ReadExpirationPrompt_Result;
                string ModeStr = result.Use == 0 ? "【0、不启用】" : "【1、启用】"; //记录存储方式
                Invoke(() =>
                {
                    if (result.Use == 1)
                    {
                        rbExpirationPrompt1.Checked = true;
                    }
                    else
                    {
                        rbExpirationPrompt0.Checked = true;
                    }
                });
                ModeStr = "卡片到期提示参数：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteExpirationPrompt_Click(object sender, EventArgs e)
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte use = (byte)(rbExpirationPrompt1.Checked ? 1 : 0);
            WriteExpirationPrompt cmd = new WriteExpirationPrompt(cmdDtl, new WriteExpirationPrompt_Parameter(use));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadReadCardSpeak_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReadCardSpeak cmd = new ReadReadCardSpeak(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Door.Door8800.SystemParameter.FunctionParameter.ReadReadCardSpeak_Result;
                string UseStr = result.SpeakSetting.Use ? "【1、启用】" : "【0、不启用】"; //定时读卡播报语音消息功能是否启用
                string MsgIndexStr = result.SpeakSetting.MsgIndex == 1 ? "【1、交房租】" : "【2、交管理费】"; //消息编号类型
                string STime = result.SpeakSetting.BeginDate.ToString("yyyy-MM-dd HH时");
                string ETime = result.SpeakSetting.EndDate.ToString("yyyy-MM-dd HH时");
                Invoke(() =>
                {
                    if (result.SpeakSetting.Use)
                    {
                        rBtnEnableReadCardSpeak.Checked = true;
                    }
                    else
                    {
                        rBtnNoEnableReadCardSpeak.Checked = true;
                    }
                    if (result.SpeakSetting.MsgIndex == 1)
                    {
                        rBtnPayRent.Checked = true;
                    }
                    else
                    {
                        rBtnPayManagementFee.Checked = true;
                    }
                    txtSTime.Text = STime;
                    txtETime.Text = ETime;
                });
                UseStr = "功能开关：" + UseStr +
                         "  消息编号类型：" + MsgIndexStr +
                         "  起始时段：" + STime +
                         "  功能开关：" + ETime;
                mMainForm.AddCmdLog(cmde, UseStr);
            };
        }

        private void BtnWriteReadCardSpeak_Click(object sender, EventArgs e)
        {
            string reg = @"^\d{4}-\d{2}-\d{2} \d{2}时";
            if (!Regex.IsMatch(txtSTime.Text.Trim(), reg) || txtSTime.Text == "")
            {
                MsgErr("请输入正确起始时段！");
                return;
            }
            if (Convert.ToInt16(txtSTime.Text.Substring(0, 4)) > 2099
                || Convert.ToInt16(txtSTime.Text.Substring(5, 2)) > 12
                || Convert.ToInt16(txtSTime.Text.Substring(8, 2)) > 31
                || Convert.ToInt16(txtSTime.Text.Substring(11, 2)) > 23)
            {
                MsgErr("请输入正确起始时段！");
                return;
            }
            if (!Regex.IsMatch(txtETime.Text.Trim(), reg) || txtETime.Text == "")
            {
                MsgErr("请输入正确结束时段！");
                return;
            }
            if (Convert.ToInt16(txtETime.Text.Substring(0, 4)) > 2099
                || Convert.ToInt16(txtETime.Text.Substring(5, 2)) > 12
                || Convert.ToInt16(txtETime.Text.Substring(8, 2)) > 31
                || Convert.ToInt16(txtETime.Text.Substring(11, 2)) > 23)
            {
                MsgErr("请输入正确结束时段！");
                return;
            }
            if (Convert.ToInt16(txtSTime.Text.Substring(0, 4)) > Convert.ToInt16(txtETime.Text.Substring(0, 4)))
            {
                MsgErr("请输入正确时段范围！");
                return;
            }
            else if (Convert.ToInt16(txtSTime.Text.Substring(0, 4)) == Convert.ToInt16(txtETime.Text.Substring(0, 4)))
            {
                if (Convert.ToInt16(txtSTime.Text.Substring(5, 2)) > Convert.ToInt16(txtETime.Text.Substring(5, 2)))
                {
                    MsgErr("请输入正确时段范围！");
                    return;
                }
                else if (Convert.ToInt16(txtSTime.Text.Substring(5, 2)) == Convert.ToInt16(txtETime.Text.Substring(5, 2)))
                {
                    if (Convert.ToInt16(txtSTime.Text.Substring(8, 2)) > Convert.ToInt16(txtETime.Text.Substring(8, 2)))
                    {
                        MsgErr("请输入正确时段范围！");
                        return;
                    }
                    else if (Convert.ToInt16(txtSTime.Text.Substring(8, 2)) == Convert.ToInt16(txtETime.Text.Substring(8, 2)))
                    {
                        if (Convert.ToInt16(txtSTime.Text.Substring(11, 2)) > Convert.ToInt16(txtETime.Text.Substring(11, 2)))
                        {
                            MsgErr("请输入正确时段范围！");
                            return;
                        }
                    }
                }
            }

            int MsgIndex = 1;
            if (rBtnPayManagementFee.Checked == true)
            {
                MsgIndex = 2;
            }

            ReadCardSpeak rcs = new ReadCardSpeak();
            rcs.Use = rBtnEnableReadCardSpeak.Checked;
            rcs.MsgIndex = MsgIndex;
            rcs.BeginDate = Convert.ToDateTime(txtSTime.Text.Replace("时", "") + ":00:00");
            rcs.EndDate = Convert.ToDateTime(txtETime.Text.Replace("时", "") + ":00:00");

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteReadCardSpeak cmd = new WriteReadCardSpeak(cmdDtl, new WriteReadCardSpeak_Parameter(rcs));
            mMainForm.AddCommand(cmd);
        }

        private void CbCheckedAll_CheckedChanged(object sender, EventArgs e)
        {
            cbAlarm0.Checked = cbAlarm2.Checked = cbAlarm4.Checked = cbAlarm7.Checked = cbCheckedAll.Checked;
        }

        private void BtnCloseAllAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[4];
            list[0] = Convert.ToByte(cbAlarm0.Checked);
            list[1] = Convert.ToByte(cbAlarm2.Checked);
            list[2] = Convert.ToByte(cbAlarm4.Checked);
            list[3] = Convert.ToByte(cbAlarm7.Checked);
            WriteCloseAlarm_Parameter par = new WriteCloseAlarm_Parameter(list);
            WriteCloseAlarm cmd = new WriteCloseAlarm(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                mMainForm.AddCmdLog(cmde, $"命令成功");
            };
        }

        private void BtnReadManageCard_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadManageCard cmd = new ReadManageCard(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadManageCard_Result result = cmde.Command.getResult() as ReadManageCard_Result;
                string ModeStr = result.Use == 0 ? "【0、不启用】" : "【1、启用】"; //记录存储方式
                Invoke(() =>
                {
                    if (result.Use == 1)
                    {
                        rbManageCardIsUse1.Checked = true;
                    }
                    else
                    {
                        rbManageCardIsUse0.Checked = true;
                    }
                });
                ModeStr = "管理卡功能：" + ModeStr;
                mMainForm.AddCmdLog(cmde, ModeStr);
            };
        }

        private void BtnWriteManageCard_Click(object sender, EventArgs e)
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte use = (byte)(rbManageCardIsUse1.Checked ? 1 : 0);
            WriteManageCard cmd = new WriteManageCard(cmdDtl, new WriteManageCard_Parameter(use));
            mMainForm.AddCommand(cmd);
        }

        private void BtnReadManageKeyboardSetting_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadKeyboardCardIssuingManage cmd = new ReadKeyboardCardIssuingManage(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadKeyboardCardIssuingManage_Result result = cmde.Command.getResult() as ReadKeyboardCardIssuingManage_Result;

                Invoke(() =>
                {
                    cbManageKeyboardSettingUse.Checked = result.Use;
                    txtPassword.Text = result.Password;
                });
                string use = result.Use ? "启用" : "不启用";
                string str = $"键盘发卡功能：【{use}】，密码:{ result.Password}";
                mMainForm.AddCmdLog(cmde, str);
            };
        }

        private void BtnWriteManageKeyboardSetting_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim().Length < 4)
            {
                MessageBox.Show("密码不能少于4位");
                return;
            }
            string pattern = @"\b(0[xX])?[A-Fa-f0-9]+\b";
            bool isHexNum = Regex.IsMatch(txtPassword.Text.Trim(), pattern);
            if (!isHexNum)
            {
                MessageBox.Show("密码格式不正确");
                return;
            }
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteKeyboardCardIssuingManage_Parameter par = new WriteKeyboardCardIssuingManage_Parameter(cbManageKeyboardSettingUse.Checked, txtPassword.Text.Trim());
            WriteKeyboardCardIssuingManage cmd = new WriteKeyboardCardIssuingManage(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void BtnReadInputTerminalFunction_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadInputTerminalFunction cmd = new ReadInputTerminalFunction(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadInputTerminalFunction_Result result = cmde.Command.getResult() as ReadInputTerminalFunction_Result;
                int Function = result.Function; //消防报警参数

                Invoke(() =>
                {
                    cmbInputTerminalFunction.SelectedIndex = Function - 1;
                });
                string Info = $"输入端子功能定义：【{Function}、】" + InputTerminalFunctionList[Function - 1];
                mMainForm.AddCmdLog(cmde, Info);
            };
        }

        private void BtnWriteInputTerminalFunction_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteInputTerminalFunction_Parameter par = new WriteInputTerminalFunction_Parameter(cmbInputTerminalFunction.SelectedIndex + 1);
            WriteInputTerminalFunction cmd = new WriteInputTerminalFunction(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void Read485LineConnection_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            Read485LineConnection cmd = new Read485LineConnection(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Door.Door8800.SystemParameter.Check485Line.ReadCheck485Line_Result;

                Invoke(() =>
                {
                    cb485IsUse.Checked = result.Use == 1;
                });
                string use = result.Use == 1 ? "开启线路桥接" : "关闭线路桥接";
                string str = $"TCP、485线路桥接：【{use}】";
                mMainForm.AddCmdLog(cmde, str);
            };
        }

        private void BtnWrite485LineConnection_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            byte use = (byte)(cb485IsUse.Checked ? 1 : 0);
            Write485LineConnection_Parameter par = new Write485LineConnection_Parameter(use);
            Write485LineConnection cmd = new Write485LineConnection(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }

        private void BtnReadDoorWorkSetting_Click(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadReaderWorkSetting cmd = new ReadReaderWorkSetting(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                byte[] ByteDoorAlarmStateSet = null;
                BitArray bitSet = null;
                ReaderWorkSetting_Result result = cmde.Command.getResult() as ReaderWorkSetting_Result;
                ListWeekTimeGroupDto.Clear();
                for (int i = 0; i < 7; i++)
                {
                    WeekTimeGroupDto dto = new WeekTimeGroupDto();
                    dto.WeekDay = GetWeekStr(i);
                    dto.Ex = "-";
                    dto.IsEx = "true";
                    ListWeekTimeGroupDto.Add(dto);

                    for (int j = 0; j < 8; j++)
                    {
                        var tz = result.weekTimeGroup_ReaderWork.GetItem(i).GetItem(j) as TimeSegment_ReaderWork;
                        ByteDoorAlarmStateSet = new byte[] { tz.GetCheckWay() };
                        bitSet = new BitArray(ByteDoorAlarmStateSet);

                        string strCheckWay = Convert.ToString(tz.GetCheckWay(), 2).PadLeft(4, '0');

                        dto = new WeekTimeGroupDto();
                        dto.WeekDay = (j + 1).ToString();
                        dto.WeekDayIndex = i;
                        dto.StartTime = result.weekTimeGroup_ReaderWork.GetItem(i).GetItem(j).GetBeginTime().ToString("HH:mm");
                        dto.EndTime = result.weekTimeGroup_ReaderWork.GetItem(i).GetItem(j).GetEndTime().ToString("HH:mm");
                        dto.id0 = strCheckWay[3] == '1';// bitSet[0];
                        dto.id1 = strCheckWay[2] == '1';// bitSet[1];
                        dto.id2 = strCheckWay[1] == '1';// bitSet[2];
                        dto.id3 = strCheckWay[0] == '1';// bitSet[3];
                        ListWeekTimeGroupDto.Add(dto);
                    }
                }
                Invoke(() =>
                {
                    dataGridView1.DataSource = new BindingList<WeekTimeGroupDto>(ListWeekTimeGroupDto);
                });

            };
        }

        /// <summary>
        /// 获得数值代表的星期
        /// </summary>
        /// <param name="index">数值（0-6，0代表星期一...6代表星期日）</param>
        /// <returns></returns>
        private string GetWeekStr(int index)
        {
            string weekStr = string.Empty;
            if (index == 0)
            {
                return "星期一";
            }
            else if (index == 1)
            {
                return "星期二";
            }
            else if (index == 2)
            {
                return "星期三";
            }
            else if (index == 3)
            {
                return "星期四";
            }
            else if (index == 4)
            {
                return "星期五";
            }
            else if (index == 5)
            {
                return "星期六";
            }
            else if (index == 6)
            {
                return "星期日";
            }
            return weekStr;
        }

        private void BtnWriteDoorWorkSetting_Click(object sender, EventArgs e)
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WeekTimeGroup_ReaderWork tg = new WeekTimeGroup_ReaderWork(8);
            ConvertDtoToModel(tg);

            WriteReaderWorkSetting_Parameter par = new WriteReaderWorkSetting_Parameter(tg);
            WriteReaderWorkSetting write = new WriteReaderWorkSetting(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        /// <summary>
        /// GridView数据 转换成 WeekTimeGroup_ReaderWork
        /// </summary>
        /// <param name="tg"></param>
        private void ConvertDtoToModel(WeekTimeGroup_ReaderWork tg)
        {
            for (int i = 0; i < 7; i++)
            {
                var day = tg.GetItem(i);
                for (int j = 0; j < 8; j++)
                {
                    var dto = ListWeekTimeGroupDto.FirstOrDefault(t => t.WeekDay == (j + 1).ToString() && t.WeekDayIndex == i);
                    //DateTime nw = DateTime.Now;
                    var tz = day.GetItem(j) as TimeSegment_ReaderWork;
                    string[] tsStart = dto.StartTime.Split(':');
                    string[] tsEnd = dto.EndTime.Split(':');
                    tz.SetBeginTime(int.Parse(tsStart[0]), int.Parse(tsStart[1]));
                    tz.SetEndTime(int.Parse(tsEnd[0]), int.Parse(tsEnd[1]));
                    string strDoor1 = (dto.id3 ? "1" : "0") + (dto.id2 ? "1" : "0") + (dto.id1 ? "1" : "0") + (dto.id0 ? "1" : "0");
                    byte checkWay = Convert.ToByte(strDoor1, 2);

                    tz.SetCheckWay(checkWay);
                }
            }

        }

        private void BtnWriteSn_Click(object sender, EventArgs e)
        {
            if (txtSN.Text.Length != 16)
            {
                MessageBox.Show("sn错误，请输入16位数字字符");
                return;
            }
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteSN cmd = new WriteSN(cmdDtl, new Door.Door8800.SystemParameter.SN.SN_Parameter(txtSN.Text.Trim()));
            mMainForm.AddCommand(cmd);
        }
    }
}
