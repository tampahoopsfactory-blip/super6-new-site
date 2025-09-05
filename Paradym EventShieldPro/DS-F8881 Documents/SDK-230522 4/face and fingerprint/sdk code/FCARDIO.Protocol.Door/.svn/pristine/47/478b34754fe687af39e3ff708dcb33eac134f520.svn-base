using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPClient
{
    /// <summary>
    /// 读取TCP客户端列表
    /// </summary>
    public class ReadTCPClientList : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 读取TCP客户端列表
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadTCPClientList(INCommandDetail cd) : base(cd) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x14, 0x00);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            var buf = oPck.CmdData;
            TCPClient_Result rst = new TCPClient_Result();
            _Result = rst;
            rst.SetBytes(buf);
            CommandCompleted();
        }
    }
}
