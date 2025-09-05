using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{

    /// <summary>
    /// 写入合法验证后显示的短消息
    /// </summary>
    public class WriteShortMessage : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建写入合法验证后显示的短消息的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">合法验证后显示的短消息参数</param>
        public WriteShortMessage(INCommandDetail cd, WriteShortMessage_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteShortMessage_Parameter model = _Parameter as WriteShortMessage_Parameter;
            Packet(0x01, 0x2F, 0x00, 60, model.GetBytes(GetNewCmdDataBuf(60)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteShortMessage_Parameter model = value as WriteShortMessage_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }
    }
}
