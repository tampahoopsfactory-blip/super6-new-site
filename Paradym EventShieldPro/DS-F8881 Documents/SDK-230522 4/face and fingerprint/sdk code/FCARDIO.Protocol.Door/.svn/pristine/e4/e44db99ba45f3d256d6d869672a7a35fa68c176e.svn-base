using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.ConsumeLogStatisticsTime
{
    /// <summary>
    /// 设置消费日志统计时间点
    /// </summary>
    public class WriteConsumeLogStatisticsTime : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteConsumeLogStatisticsTime(Protocol.DESDriveCommandDetail cd, WriteConsumeLogStatisticsTime_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteConsumeLogStatisticsTime_Parameter model = _Parameter as WriteConsumeLogStatisticsTime_Parameter;
            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x0B, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteConsumeLogStatisticsTime_Parameter model = value as WriteConsumeLogStatisticsTime_Parameter;
            if (model == null) return false;
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
