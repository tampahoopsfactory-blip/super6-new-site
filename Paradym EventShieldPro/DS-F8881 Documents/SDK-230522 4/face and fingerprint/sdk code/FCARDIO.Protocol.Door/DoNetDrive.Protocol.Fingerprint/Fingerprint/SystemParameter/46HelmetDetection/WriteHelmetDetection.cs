using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置安全帽检测功能
    /// </summary>
    public class WriteHelmetDetection : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设置安全帽检测功能的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteHelmetDetection(INCommandDetail cd, WriteHelmetDetection_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 创建设置安全帽检测功能的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="open">开关</param>
        public WriteHelmetDetection(INCommandDetail cd, bool open) : base(cd, new WriteHelmetDetection_Parameter(open)) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteHelmetDetection_Parameter;
            Packet(0x01, 0x39, 0x00, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }
    }
}
