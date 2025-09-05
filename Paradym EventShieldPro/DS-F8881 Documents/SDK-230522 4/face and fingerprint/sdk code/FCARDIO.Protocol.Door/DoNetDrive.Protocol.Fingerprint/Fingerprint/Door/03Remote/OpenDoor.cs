using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.Remote
{
    /// <summary>
    /// 远程开门
    /// </summary>
    public class OpenDoor : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 远程开门
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public OpenDoor(INCommandDetail cd) : base(cd,null) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return true;
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {

            Packet(0x03, 0x03, 0x00);
        }

        
    }
}
