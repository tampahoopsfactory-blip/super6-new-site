using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.AutoLockedSetting
{
    /// <summary>
    /// 设置定时锁定门参数
    /// </summary>
    public class WriteAutoLockedSetting : Write_Command
    {
        /// <summary>
        /// 设置定时锁定门参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含定时锁定门参数</param>
        public WriteAutoLockedSetting(INCommandDetail cd, WriteAutoLockedSetting_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteAutoLockedSetting_Parameter model = value as WriteAutoLockedSetting_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteAutoLockedSetting_Parameter model = _Parameter as WriteAutoLockedSetting_Parameter;
            Packet(0x43, 0x05, 0x00, 0xE2, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
