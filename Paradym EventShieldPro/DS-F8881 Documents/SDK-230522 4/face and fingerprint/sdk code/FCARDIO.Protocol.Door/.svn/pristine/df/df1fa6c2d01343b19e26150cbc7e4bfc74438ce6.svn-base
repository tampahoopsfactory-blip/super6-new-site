using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Time
{
    /// <summary>
    /// 将电脑的最新时间写入到控制器中
    /// </summary>
    public class WriteTime : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 将电脑的最新时间写入到控制器中
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public WriteTime(INCommandDetail cd) : base(cd, null) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return true;
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(0x07);

            byte[] Datebuf = new byte[7];

            TimeUtil.DateToBCD_ssmmhhddMMwwyy(Datebuf, DateTime.Now);

            buf.WriteBytes(Datebuf);

            Packet(0x02, 0x02, 0x00, 0x07, buf);
        }
    }
}