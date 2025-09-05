using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置感光模式
    /// </summary>
    public class WriteLightPattern : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设置感光模式的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteLightPattern(INCommandDetail cd, WriteLightPattern_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteLightPattern_Parameter;
            Packet(0x01, 0x36, 0x00, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }
    }
}
