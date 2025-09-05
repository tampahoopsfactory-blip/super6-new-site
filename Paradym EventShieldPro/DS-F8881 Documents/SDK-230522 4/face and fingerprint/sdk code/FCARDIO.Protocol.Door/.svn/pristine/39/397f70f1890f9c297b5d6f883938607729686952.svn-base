using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.SN
{

    /// <summary>
    /// 写入控制器SN
    /// </summary>
    public class WriteSN : DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.WriteSN
    {
        /// <summary>
        /// 写入控制器SN 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含SN数据</param>
        public WriteSN(INCommandDetail cd, SN_Parameter par) : base(cd, par)
        {
            DataStrt = new byte[] { 0xD7, 0x6D, 0x82, 0x12, 0xC4 };
            DataEnd = new byte[] { 0x15, 0x47, 0x42 };

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x41, 0x01, 0xF0, 0x18, GetCmdData());
        }
    }
}
