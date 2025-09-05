using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.UnlockingTime
{
    /// <summary>
    /// 设置开锁时输出时长
    /// </summary>
    public class WriteUnlockingTime : Write_Command
    {
        /// <summary>
        /// 设置定时锁定门参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含定时锁定门参数</param>
        public WriteUnlockingTime(INCommandDetail cd, WriteUnlockingTime_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteUnlockingTime_Parameter model = value as WriteUnlockingTime_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteUnlockingTime_Parameter model = _Parameter as WriteUnlockingTime_Parameter;
            Packet(0x43, 0x06, 0x01, 0x82, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
