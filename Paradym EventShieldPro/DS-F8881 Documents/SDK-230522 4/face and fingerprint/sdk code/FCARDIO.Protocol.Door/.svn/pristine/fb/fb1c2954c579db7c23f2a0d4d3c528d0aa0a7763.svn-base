using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.TheftFortify
{
    /// <summary>
    /// 防盗报警布防
    /// </summary>
    public class SetTheftFortify : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 防盗报警布防
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public SetTheftFortify(INCommandDetail cd) : base(cd) { }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x11, 0x01);
        }

        /// <summary>
        /// 处理返回通知
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            return;
        }
    }
}