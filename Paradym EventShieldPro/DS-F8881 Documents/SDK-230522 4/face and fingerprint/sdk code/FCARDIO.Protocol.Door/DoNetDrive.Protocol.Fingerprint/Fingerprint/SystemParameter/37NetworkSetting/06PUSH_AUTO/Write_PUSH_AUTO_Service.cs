using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入自动推送服务参数
    /// </summary>
    public class Write_PUSH_AUTO_Service : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建写入自动推送服务参数的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public Write_PUSH_AUTO_Service(INCommandDetail cd, Write_PUSH_AUTO_Service_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as Write_PUSH_AUTO_Service_Parameter;
            Packet(0x01, 0x30, 0x07, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }
    }
}
