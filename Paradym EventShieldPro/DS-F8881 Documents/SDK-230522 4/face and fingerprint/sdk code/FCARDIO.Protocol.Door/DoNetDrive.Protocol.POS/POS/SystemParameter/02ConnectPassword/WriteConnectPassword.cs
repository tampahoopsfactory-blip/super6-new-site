using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.POS.SystemParameter.ConnectPassword
{
    /// <summary>
    /// 设置控制器通讯密码
    /// </summary>
    public class WriteConnectPassword : Write_Command
    {
        /// <summary>
        /// 设置控制器通讯密码 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含命令所需新的通讯密码</param>
        public WriteConnectPassword(Protocol.DESDriveCommandDetail cd, Password_Parameter par) : base(cd, par)
        {
        }
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Password_Parameter model = _Parameter as Password_Parameter;
            Packet(0x01, 0x02, 0x00, 8, model.GetBytes(_Connector.GetByteBufAllocator().Buffer(8)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            Password_Parameter model = value as Password_Parameter;
            if (model == null)
            {
                return false;
            }

            return model.checkedParameter();
        }

        protected override void CommandNext0(Protocol.DESPacket oPck)
        {
            if (CheckResponse_OK(oPck))
            {
                CommandCompleted();
            }

        }
    }
}
