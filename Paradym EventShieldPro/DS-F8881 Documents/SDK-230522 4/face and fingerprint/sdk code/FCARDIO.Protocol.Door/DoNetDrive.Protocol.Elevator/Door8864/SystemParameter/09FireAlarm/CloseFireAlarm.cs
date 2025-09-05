using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FireAlarm
{
    /// <summary>
    /// 解除消防报警
    /// </summary>
    public class CloseFireAlarm : Protocol.Door.Door8800.SystemParameter.FireAlarm.CloseFireAlarm
    {
        /// <summary>
        /// 解除消防报警 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public CloseFireAlarm(INCommandDetail cd) : base(cd) {
        }
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x41, 0x0C, 0x01);
        }
    }
}