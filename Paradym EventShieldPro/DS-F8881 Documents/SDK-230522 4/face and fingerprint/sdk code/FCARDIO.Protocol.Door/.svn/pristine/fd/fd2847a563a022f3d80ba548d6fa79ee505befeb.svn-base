using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.Time
{
    /// <summary>
    /// 将自定义时间写入到控制器中
    /// </summary>
    public class WriteCustomTime : Protocol.Door.Door8800.Time.WriteCustomTime
    {
        /// <summary>
        /// 将自定义时间写入到控制器中
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含自定义时间</param>
        public WriteCustomTime(INCommandDetail cd, WriteCustomTime_Parameter par) : base(cd, par) {
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteCustomTime_Parameter model = _Parameter as WriteCustomTime_Parameter;
            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(model.GetDataLen());
            Packet(0x42, 0x02, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}