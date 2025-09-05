using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.IllegalVerificationAlarm
{
    /// <summary>
    /// 设置 非法验证报警
    /// </summary>
    public class WriteIllegalVerificationAlarm : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteIllegalVerificationAlarm(INCommandDetail cd, WriteIllegalVerificationAlarm_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteIllegalVerificationAlarm_Parameter model = _Parameter as WriteIllegalVerificationAlarm_Parameter;
            Packet(0x04, 0x04, 0x00, 0x02, model.GetBytes(GetNewCmdDataBuf(2)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteIllegalVerificationAlarm_Parameter model = value as WriteIllegalVerificationAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }
    }
}
