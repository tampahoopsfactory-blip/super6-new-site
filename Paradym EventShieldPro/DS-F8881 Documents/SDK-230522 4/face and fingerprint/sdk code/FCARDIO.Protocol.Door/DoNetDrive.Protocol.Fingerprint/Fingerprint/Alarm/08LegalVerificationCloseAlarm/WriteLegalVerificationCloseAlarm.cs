using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.LegalVerificationCloseAlarm
{
    /// <summary>
    /// 设置 合法验证解除报警开关
    /// </summary>
    public class WriteLegalVerificationCloseAlarm : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteLegalVerificationCloseAlarm(INCommandDetail cd, WriteLegalVerificationCloseAlarm_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteLegalVerificationCloseAlarm_Parameter model = _Parameter as WriteLegalVerificationCloseAlarm_Parameter;
            Packet(0x04, 0x08, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(1)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteLegalVerificationCloseAlarm_Parameter model = value as WriteLegalVerificationCloseAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }
    }
}
