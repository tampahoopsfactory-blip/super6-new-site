using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.OutDoorSwitch
{
    /// <summary>
    /// 设置出门开关参数
    /// </summary>
    public class WriteOutDoorSwitch : Write_Command
    {
        /// <summary>
        /// 设置门工作方式
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteOutDoorSwitch(INCommandDetail cd, WriteOutDoorSwitch_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteOutDoorSwitch_Parameter model = value as WriteOutDoorSwitch_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteOutDoorSwitch_Parameter model = _Parameter as WriteOutDoorSwitch_Parameter;
            Packet(0x43, 0x07, 0x01, 0xE3, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
