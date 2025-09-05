using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.BlacklistAlarm
{
    /// <summary>
    /// 设置 黑名单报警
    /// </summary>
    public class WriteBlacklistAlarm : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteBlacklistAlarm(INCommandDetail cd, WriteBlacklistAlarm_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteBlacklistAlarm_Parameter model = value as WriteBlacklistAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteBlacklistAlarm_Parameter model = _Parameter as WriteBlacklistAlarm_Parameter;
            Packet(0x04, 0x02, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(1)));
        }

    }
}
