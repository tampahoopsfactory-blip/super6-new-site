using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ManageCard
{
    /// <summary>
    /// 设置 管理卡功能
    /// </summary>
    public class WriteManageCard : Write_Command
    {
        /// <summary>
        /// 设置读卡间隔时间 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteManageCard(INCommandDetail cd, WriteManageCard_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteManageCard_Parameter model = _Parameter as WriteManageCard_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x41, 0x10, 0x00, 1, model.GetBytes(buf));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteManageCard_Parameter model = value as WriteManageCard_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }
    }
}
