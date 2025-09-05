using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 使设备立刻进入休眠模式
    /// </summary>
    public class SendCMD_EnterSleep : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建使设备立刻进入休眠模式的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public SendCMD_EnterSleep(INCommandDetail cd) : base(cd, null) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x0D, 0x02, 0x00);
        }

        protected override bool CheckCommandParameter(INCommandParameter value) => true;
    }
}
