using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入口罩识别开关
    /// </summary>
    public class WriteFaceMouthmufflePar : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建写入口罩识别开关的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">口罩识别开关参数</param>
        public WriteFaceMouthmufflePar(INCommandDetail cd, WriteFaceMouthmufflePar_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteFaceMouthmufflePar_Parameter model = _Parameter as WriteFaceMouthmufflePar_Parameter;
            Packet(0x01, 0x28, 0x00, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteFaceMouthmufflePar_Parameter model = value as WriteFaceMouthmufflePar_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }
    }
}
