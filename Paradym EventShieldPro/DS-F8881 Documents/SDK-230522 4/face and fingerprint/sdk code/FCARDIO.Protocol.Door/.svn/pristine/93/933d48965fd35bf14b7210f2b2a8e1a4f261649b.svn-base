using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword
{
    /// <summary>
    /// 重置控制器通讯密码
    /// </summary>
    public class ResetConnectPassword : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 命令数据部分
        /// </summary>
        private static readonly byte[] DataStrt = new byte[] { 0x46, 0x43, 0x61, 0x72, 0x64, 0x59, 0x7A };

        /// <summary>
        /// 重置控制器通讯密码 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ResetConnectPassword(INCommandDetail cd) : base(cd, null) {

        }

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
        /// 拼装命令
        /// </summary>
        protected override void CreatePacket0()
        {
            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(7);
            buf.WriteBytes(DataStrt);

            Packet(0x01, 0x05, 0x00, 0x07, buf);
        }


    }
}