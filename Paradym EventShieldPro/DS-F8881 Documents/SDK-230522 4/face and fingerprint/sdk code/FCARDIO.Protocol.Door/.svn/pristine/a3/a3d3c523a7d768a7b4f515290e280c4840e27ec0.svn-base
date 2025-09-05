using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.SystemParameter.ConnectPassword
{
    /// <summary>
    /// 重置控制器通讯密码
    /// </summary>
    public class ResetConnectPassword : Read_Command
    {

        /// <summary>
        /// 命令数据部分
        /// </summary>
        private static readonly byte[] DataStrt = new byte[] { 0x4C, 0xD5, 0x4E, 0xF6, 0xDC, 0x53, 0xF8, 0x97 };

        /// <summary>
        /// 重置控制器通讯密码 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ResetConnectPassword(DESDriveCommandDetail cd) : base(cd)
        {
        }

        protected override void CommandNext1(DESPacket oPck)
        {

        }

        /// <summary>
        /// 拼装命令
        /// </summary>
        protected override void CreatePacket0()
        {
            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(8);
            buf.WriteBytes(DataStrt);

            Packet(0x01, 0x02, 0x02, 0x08, buf);
        }

    }
}