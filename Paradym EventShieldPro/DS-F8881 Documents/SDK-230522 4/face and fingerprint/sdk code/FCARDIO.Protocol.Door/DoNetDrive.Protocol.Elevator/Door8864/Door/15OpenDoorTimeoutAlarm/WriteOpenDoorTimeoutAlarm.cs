using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.OpenDoorTimeoutAlarm
{
    /// <summary>
    /// 设置 开门超时报警参数
    /// </summary>
    public class WriteOpenDoorTimeoutAlarm : Write_Command
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteOpenDoorTimeoutAlarm(INCommandDetail cd, WriteOpenDoorTimeoutAlarm_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteOpenDoorTimeoutAlarm_Parameter model = _Parameter as WriteOpenDoorTimeoutAlarm_Parameter;
            Packet(0x43, 0x0A, 0x00, 0xC0, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteOpenDoorTimeoutAlarm_Parameter model = value as WriteOpenDoorTimeoutAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

    }
}
