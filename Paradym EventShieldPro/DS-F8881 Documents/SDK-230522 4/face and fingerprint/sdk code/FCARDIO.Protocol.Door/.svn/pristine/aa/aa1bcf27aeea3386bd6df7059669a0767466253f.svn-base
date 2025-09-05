using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.ExemptionVerificationOpen
{
    /// <summary>
    /// 设置 免验证开门
    /// </summary>
    public class WriteExemptionVerificationOpen : Door8800Command_WriteParameter
    {
        /// <summary>
        ///  初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteExemptionVerificationOpen(INCommandDetail cd, WriteExemptionVerificationOpen_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteExemptionVerificationOpen_Parameter model = value as WriteExemptionVerificationOpen_Parameter;
            if (model == null)
            {
                return false;
            }

            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteExemptionVerificationOpen_Parameter model = _Parameter as WriteExemptionVerificationOpen_Parameter;
            Packet(0x03, 0x11, 0x00, 3, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
