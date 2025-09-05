using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置第三方平台推送功能
    /// </summary>
    public class WriteThirdpartyAPI : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设置第三方平台推送功能的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteThirdpartyAPI(INCommandDetail cd, WriteThirdpartyAPI_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteThirdpartyAPI_Parameter;
            int len = model.GetDataLen();
            Packet(0x01, 0x3C, 0x00, (uint)len, model.GetBytes(GetNewCmdDataBuf(len)));
        }
    }
}
