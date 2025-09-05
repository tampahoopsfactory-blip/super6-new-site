using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Door.Remote
{
    /// <summary>
    /// 解除锁定门
    /// </summary>
    public class UnlockDoor : OpenDoor
    {
        /// <summary>
        /// 解除锁定门
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public UnlockDoor(INCommandDetail cd) : base(cd) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x03, 0x04, 0x01);
        }
    }
}
