using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Door.Remote
{
    /// <summary>
    /// 远程关门
    /// </summary>
    public class CloseDoor : OpenDoor
    {
        /// <summary>
        /// 远程关门
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public CloseDoor(INCommandDetail cd) : base(cd) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x03, 0x03, 0x01);
        }
    }
}
