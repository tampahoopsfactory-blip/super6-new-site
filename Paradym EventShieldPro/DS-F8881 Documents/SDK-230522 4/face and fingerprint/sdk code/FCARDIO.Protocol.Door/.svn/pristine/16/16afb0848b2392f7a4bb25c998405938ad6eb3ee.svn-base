using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.ICCardAccount
{
    /// <summary>
    /// 设置IC卡账户命令
    /// </summary>
    public class WriteICCardAccount : Write_Command
    {
        /// <summary>
        /// 设置设备有效期 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含设备有效期</param>
        public WriteICCardAccount(Protocol.DESDriveCommandDetail cd, WriteICCardAccount_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteICCardAccount_Parameter model = value as WriteICCardAccount_Parameter;
            if (model == null)
            {
                return false;
            }

            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteICCardAccount_Parameter model = _Parameter as WriteICCardAccount_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x03, 0x07, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
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
