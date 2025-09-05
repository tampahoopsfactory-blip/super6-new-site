using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 重新拉取云筑网人员命令
    /// </summary>
    public class SendReloadYZW_People : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建重新拉取云筑网人员命令的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public SendReloadYZW_People(INCommandDetail cd) : base(cd, null) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x3A, 0x02);
        }

        protected override bool CheckCommandParameter(INCommandParameter value) => true;
    }
}
