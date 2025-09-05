using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.AlarmPassword
{
    /// <summary>
    /// 设置胁迫报警密码
    /// </summary>
    public class WriteAlarmPassword : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public WriteAlarmPassword(INCommandDetail cd, WriteAlarmPassword_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteAlarmPassword_Parameter model = _Parameter as WriteAlarmPassword_Parameter;
            Packet(0x04, 0x05, 0x00, 0x06, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteAlarmPassword_Parameter model = value as WriteAlarmPassword_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }
    }
}
