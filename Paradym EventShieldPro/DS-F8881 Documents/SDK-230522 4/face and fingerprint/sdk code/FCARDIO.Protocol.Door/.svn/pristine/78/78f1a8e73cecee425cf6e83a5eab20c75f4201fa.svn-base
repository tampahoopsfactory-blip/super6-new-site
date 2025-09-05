using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetty.Buffers;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Core.Packet;
using DoNetDrive.Common.Extensions;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ButAnalyze_Click(object sender, EventArgs e)
        {
            string sHex = txtData.Text;
            if (string.IsNullOrEmpty(sHex))
            {
                return;
            }
            sHex = sHex.Replace(" ", string.Empty);
            sHex = sHex.Replace("\r\n", string.Empty);
            sHex = sHex.Replace("\t", string.Empty);
            if (string.IsNullOrEmpty(sHex))
                return;
            byte[] bHexData = sHex.HexToByte();

            var bufAlc = UnpooledByteBufferAllocator.Default;

            OnlineAccessDecompile oad = new OnlineAccessDecompile(bufAlc);
            var buf = bufAlc.Buffer(bHexData.Length);
            buf.WriteBytes(bHexData);
            List<INPacket> result = new List<INPacket>();
            bool bDecResult = oad.Decompile(buf, result);

            int cmdType = cmdCommandType.SelectedIndex;

            if (bDecResult)
            {
                StringBuilder sBuf = new StringBuilder();
                sBuf.AppendLine("命令解析成功！");

                foreach (var pck in result)
                {
                    OnlineAccessPacket packet = pck as OnlineAccessPacket;

                    sBuf.AppendLine("命令头：0x7E");
                    if(cmdType==0)
                    {
                        sBuf.Append("信息代码：0x").AppendLine(packet.Code.ToString("X8"));
                        sBuf.Append("SN：0x").AppendLine(packet.SN.ToHex());
                        sBuf.Append("密码：0x").AppendLine(packet.Password.ToHex());
                    }
                    else
                    {
                        buf = bufAlc.Buffer(24);
                        buf.WriteInt((int)packet.Code);
                        buf.WriteBytes(packet.SN);
                        buf.WriteBytes(packet.Password);

                        buf.ReadBytes(packet.SN,0,16);
                        buf.ReadBytes(packet.Password,0,4);
                        packet.Code = buf.ReadUnsignedInt();
                        buf.Release();

                        sBuf.Append("SN：0x").AppendLine(packet.SN.ToHex());
                        sBuf.Append("密码：0x").AppendLine(packet.Password.ToHex());
                        sBuf.Append("信息代码：0x").AppendLine(packet.Code.ToString("X8"));
                        
                    }
                    
                    sBuf.Append("命令--分类：0x").AppendLine(packet.CmdType.ToString("X2"));
                    sBuf.Append("命令--命令：0x").AppendLine(packet.CmdIndex.ToString("X2"));
                    sBuf.Append("命令--参数：0x").AppendLine(packet.CmdPar.ToString("X2"));
                    sBuf.Append("数据长度：0x").AppendLine(packet.DataLen.ToString("X8"));
                    if (packet.DataLen > 0)
                    {
                        sBuf.AppendLine("数据内容：0x").AppendLine(ByteBufferUtil.HexDump(packet.CmdData));
                    }
                    sBuf.Append("校验和：0x").AppendLine(packet.Check.ToString("X2"));
                    sBuf.AppendLine("命令尾：0x7E");
                    sBuf.AppendLine();
                    sBuf.AppendLine();

                    packet.Dispose();
                }

                txtLog.Text = sBuf.ToString();
            }
            else
            {
                txtLog.Text = "命令解析失败！";
            }
            oad.Dispose();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmdCommandType.Items.Clear();
            cmdCommandType.Items.Add("硬件发送的命令");
            cmdCommandType.Items.Add("软件发送的命令");
            cmdCommandType.SelectedIndex = 0;

        }
    }
}
