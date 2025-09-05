using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.DoorWorkSetting
{
    /// <summary>
    /// 设置门工作方式
    /// </summary>
    public class WriteDoorWorkSetting : Protocol.Door.Door8800.Door.DoorWorkSetting.WriteDoorWorkSetting
    {
        /// <summary>
        /// 设置门工作方式
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteDoorWorkSetting(INCommandDetail cd, WriteDoorWorkSetting_Parameter par) : base(cd, par) {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteDoorWorkSetting_Parameter model = _Parameter as WriteDoorWorkSetting_Parameter;
            Packet(0x43, 0x04, 0x01, 0xE5, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
