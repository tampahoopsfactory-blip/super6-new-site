using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.SystemParameter.ConnectPassword
{
    /// <summary>
    /// 获取控制器通讯密码
    /// </summary>
    public class ReadConnectPassword : Read_Command
    {
        /// <summary>
        /// 命令数据部分
        /// </summary>
        private static readonly byte[] DataStrt = new byte[] { 0x64, 0xFE, 0xD8, 0xFB, 0xA3, 0x04, 0xDD, 0x72 };

        /// <summary>
        /// 获取控制器通讯密码 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadConnectPassword(DESDriveCommandDetail cd) : base(cd) {
        }

        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponse(oPck, 8))
            {
                var buf = oPck.CommandPacket.CmdData;
                Password_Result rst = new Password_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

        /// 拼装命令
        /// </summary>
        protected override void CreatePacket0()
        {
            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(8);
            buf.WriteBytes(DataStrt);

            Packet(0x01, 0x02, 0x01, 0x08, buf);
            //Packet(0x01, 0x04, 0x00, 0x07, buf);
        }
    }
}
