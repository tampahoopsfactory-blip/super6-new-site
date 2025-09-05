using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.CreateTime
{
    /// <summary>
    /// 设置生产日期
    /// </summary>
    public class WriteCreateTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteCreateTime_Parameter()
        {

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="time">生产日期</param>
        public WriteCreateTime_Parameter(DateTime time)
        {
            Time = time;
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Time < new DateTime(2000, 1, 1) || Time > new DateTime(2099, 12, 31))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            DoNetDrive.Protocol.Util.TimeUtil.DateToBCD_yyMMdd(databuf, Time);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 3;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Time = DoNetDrive.Protocol.Util.TimeUtil.BCDTimeToDate_yyMMdd(databuf);
        }
    }
}