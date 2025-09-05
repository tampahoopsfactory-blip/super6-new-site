using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.Remote
{
    /// <summary>
    /// 设置门常开
    /// </summary>
    public class HoldDoor : OpenDoor
    {
        /// <summary>
        /// 设置门常开
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含门常开参数</param>
        public HoldDoor(INCommandDetail cd, Remote_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x03, 0x03, 0x02, 0x04, GetCmdData());
        }
    }
}
