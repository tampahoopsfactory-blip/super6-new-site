using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.GateMagneticAlarm
{
    /// <summary>
    /// 设置门磁报警参数
    /// </summary>
    public class WriteGateMagneticAlarm : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteGateMagneticAlarm(INCommandDetail cd, WriteGateMagneticAlarm_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteGateMagneticAlarm_Parameter model = _Parameter as WriteGateMagneticAlarm_Parameter;
            Packet(0x04, 0x07, 0x01, 0xE1, model.GetBytes(GetNewCmdDataBuf(0xE1)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteGateMagneticAlarm_Parameter model = value as WriteGateMagneticAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

    }
}
