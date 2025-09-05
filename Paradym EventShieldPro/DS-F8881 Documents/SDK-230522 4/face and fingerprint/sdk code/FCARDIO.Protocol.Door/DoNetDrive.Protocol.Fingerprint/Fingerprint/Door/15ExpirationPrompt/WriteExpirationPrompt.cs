using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.ExpirationPrompt
{
    /// <summary>
    /// 设置 权限到期提示参数
    /// </summary>
    public class WriteExpirationPrompt : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置读卡间隔时间 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteExpirationPrompt(INCommandDetail cd, WriteExpirationPrompt_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteExpirationPrompt_Parameter model = _Parameter as WriteExpirationPrompt_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(2);

            Packet(0x03, 0x15, 0x00, 2, model.GetBytes(buf));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteExpirationPrompt_Parameter model = value as WriteExpirationPrompt_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }
    }
}
