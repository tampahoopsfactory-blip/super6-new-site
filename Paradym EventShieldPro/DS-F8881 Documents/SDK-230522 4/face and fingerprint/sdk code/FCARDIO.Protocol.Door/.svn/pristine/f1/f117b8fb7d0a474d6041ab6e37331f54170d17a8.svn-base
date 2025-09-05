using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.SendFireAlarm
{
    /// <summary>
    /// 通知设备触发消防报警
    /// </summary>
    public class WriteSendFireAlarm : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 通知设备触发消防报警 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public WriteSendFireAlarm(INCommandDetail cd) : base(cd,null) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x04, 0x01, 0x00);
        }

        protected override bool CheckCommandParameter(INCommandParameter value) => true;

    }
}
