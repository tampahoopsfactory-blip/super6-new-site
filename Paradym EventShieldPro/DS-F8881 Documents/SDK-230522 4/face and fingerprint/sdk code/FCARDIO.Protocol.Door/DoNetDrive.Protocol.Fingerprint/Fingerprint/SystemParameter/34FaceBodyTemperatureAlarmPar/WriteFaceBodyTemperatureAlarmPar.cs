using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入体温报警阈值
    /// </summary>
    public class WriteFaceBodyTemperatureAlarmPar : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建写入体温报警阈值的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">体温报警阈值参数</param>
        public WriteFaceBodyTemperatureAlarmPar(INCommandDetail cd, WriteFaceBodyTemperatureAlarmPar_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteFaceBodyTemperatureAlarmPar_Parameter model = _Parameter as WriteFaceBodyTemperatureAlarmPar_Parameter;
            Packet(0x01, 0x2D, 0x00, 2, model.GetBytes(GetNewCmdDataBuf(2)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteFaceBodyTemperatureAlarmPar_Parameter model = value as WriteFaceBodyTemperatureAlarmPar_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }
    }
}
