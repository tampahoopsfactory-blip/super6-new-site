using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置人脸机设备4G模块的工作状态
    /// </summary>
    public class WriteFaceDevice4GModuleStatus : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设置人脸机设备4G模块的工作状态的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteFaceDevice4GModuleStatus(INCommandDetail cd, WriteFaceDevice4GModuleStatus_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 创建设置人脸机设备4G模块的工作状态的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="open">开关</param>
        public WriteFaceDevice4GModuleStatus(INCommandDetail cd, bool open) : base(cd, new WriteFaceDevice4GModuleStatus_Parameter(open)) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteFaceDevice4GModuleStatus_Parameter;
            Packet(0x01, 0x3B, 0x00, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }
    }
}
