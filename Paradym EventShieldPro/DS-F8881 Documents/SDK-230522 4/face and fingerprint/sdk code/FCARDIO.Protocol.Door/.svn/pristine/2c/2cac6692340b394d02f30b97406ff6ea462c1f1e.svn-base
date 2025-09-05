using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.Holiday
{
    /// <summary>
    /// 清空控制器中的所有节假日
    /// </summary>
    public class ClearHoliday : Protocol.Door.Door8800.Holiday.ClearHoliday
    {
        /// <summary>
        /// 构造命令，无需其他参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ClearHoliday(INCommandDetail cd) : base(cd)
        {
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x44, 2);
        }
    }
}
