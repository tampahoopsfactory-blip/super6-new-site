using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.OffLineSubsidyLimit
{
    /// <summary>
    /// 设置离线补贴充值，卡内补贴余额上限命令
    /// </summary>
    public class WriteOffLineSubsidyLimit : Write_Command
    {
        /// <summary>
        /// 设置设备有效期 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含设备有效期</param>
        public WriteOffLineSubsidyLimit(Protocol.DESDriveCommandDetail cd, WriteOffLineSubsidyLimit_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteOffLineSubsidyLimit_Parameter model = value as WriteOffLineSubsidyLimit_Parameter;
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
            WriteOffLineSubsidyLimit_Parameter model = _Parameter as WriteOffLineSubsidyLimit_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x17, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext0(Protocol.DESPacket oPck)
        {
            if (CheckResponse_OK(oPck))
            {
                CommandCompleted();
            }

        }
    }
}
