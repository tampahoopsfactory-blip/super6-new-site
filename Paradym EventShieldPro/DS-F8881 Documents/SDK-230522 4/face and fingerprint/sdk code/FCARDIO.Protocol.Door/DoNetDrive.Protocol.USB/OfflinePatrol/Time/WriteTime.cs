using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Time
{
    /// <summary>
    /// 将电脑的最新时间写入到设备中
    /// </summary>
    public class WriteTime : Write_Command
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

            var buf = acl.Buffer(0x06);

            byte[] Datebuf = new byte[6];

            TimeUtil.DateToBCD_ssmmhhddMMyy(Datebuf, DateTime.Now);

            buf.WriteBytes(Datebuf);

            Packet(0x02, 0x02, 0x06, buf);
        }
    }
}