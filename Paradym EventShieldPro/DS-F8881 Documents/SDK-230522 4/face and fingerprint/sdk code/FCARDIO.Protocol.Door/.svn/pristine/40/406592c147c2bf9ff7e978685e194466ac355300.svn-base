using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm
{
    /// <summary>
    /// 解除报警
    /// </summary>
    public class CloseAlarm : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public CloseAlarm(INCommandDetail cd, CloseAlarm_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            CloseAlarm_Parameter model = _Parameter as CloseAlarm_Parameter;
            Packet(0x04, 0x09, 0, 2, model.GetBytes(GetNewCmdDataBuf(2)));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            CloseAlarm_Parameter model = value as CloseAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

    }
}