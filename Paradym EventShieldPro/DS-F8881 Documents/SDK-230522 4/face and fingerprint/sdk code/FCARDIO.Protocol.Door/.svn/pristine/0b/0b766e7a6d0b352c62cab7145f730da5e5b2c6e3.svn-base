using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.SystemParameter.ConsumeLogStatisticsTime
{
    /// <summary>
    /// 读取消费日志统计时间点
    /// </summary>
    public class ReadConsumeLogStatisticsTime : Read_Command
    {
        /// <summary>
        /// 读取记录存储方式 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadConsumeLogStatisticsTime(DESDriveCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponse(oPck, 2))
            {
                var buf = oPck.CommandPacket.CmdData;
                ReadConsumeLogStatisticsTime_Result rst = new ReadConsumeLogStatisticsTime_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x0B, 0x01);
        }
    }
}
