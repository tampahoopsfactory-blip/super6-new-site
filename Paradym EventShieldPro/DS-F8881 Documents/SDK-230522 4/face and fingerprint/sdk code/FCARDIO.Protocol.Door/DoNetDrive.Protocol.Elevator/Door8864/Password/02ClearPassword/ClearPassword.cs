using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Elevator.FC8864.Password
{
    /// <summary>
    /// 清空所有密码
    /// </summary>
    public class ClearPassword : Protocol.Door.Door8800.Password.ClearPassword
    {
        /// <summary>
        /// 构造命令，无需其他参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ClearPassword(INCommandDetail cd) : base(cd)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x45, 2);
        }
    }
}
