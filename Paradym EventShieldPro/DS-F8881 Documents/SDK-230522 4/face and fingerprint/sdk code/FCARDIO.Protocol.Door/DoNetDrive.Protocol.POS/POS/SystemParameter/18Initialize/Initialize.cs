using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.Initialize
{
    /// <summary>
    /// 初始化设备命令
    /// </summary>
    public class Initialize : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public Initialize(Protocol.DESDriveCommandDetail cd, Initialize_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            Initialize_Parameter model = value as Initialize_Parameter;
            if (model == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Initialize_Parameter model = _Parameter as Initialize_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x20, 0, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}
