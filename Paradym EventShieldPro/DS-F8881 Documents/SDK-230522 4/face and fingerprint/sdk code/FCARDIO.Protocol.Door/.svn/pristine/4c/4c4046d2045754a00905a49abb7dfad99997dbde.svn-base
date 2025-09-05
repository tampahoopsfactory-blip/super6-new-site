using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入人脸机体温数值显示开关
    /// </summary>
    public class WriteFaceBodyTemperatureShowPar : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建写入人脸机体温数值显示开关的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">人脸机体温数值显示开关参数</param>
        public WriteFaceBodyTemperatureShowPar(INCommandDetail cd, WriteFaceBodyTemperatureShowPar_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteFaceBodyTemperatureShowPar_Parameter model = _Parameter as WriteFaceBodyTemperatureShowPar_Parameter;
            Packet(0x01, 0x2E, 0x00, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteFaceBodyTemperatureShowPar_Parameter model = value as WriteFaceBodyTemperatureShowPar_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }
    }
}
