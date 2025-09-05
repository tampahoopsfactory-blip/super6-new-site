using DoNetDrive.Protocol.POS.Data;
using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.ReservationRule
{
    public class WriteReservationRule_Parameter : AbstractParameter
    {
        /// <summary>
        /// 
        /// </summary>
        public WeekReservationRule WeekReservationRule;
        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteReservationRule_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Use">功能开关</param>
        /// <param name="DeductionCount">单次消费扣费次数</param>
        /// <param name="ResidueCount">计次卡消费后不扣除剩余次数</param>
        public WriteReservationRule_Parameter(WeekReservationRule WeekReservationRule)
        {
            this.WeekReservationRule = WeekReservationRule;
            if (!checkedParameter())
            {
                throw new ArgumentException("Parameter Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (WeekReservationRule == null)
            {
                return false;
            }
            for (int i = 0; i < 7; i++)
            {
                DayReservationRule dayReservationRule = WeekReservationRule.GetItem(i);
                for (int j = 0; j < 8; j++)
                {
                    ReservationRuleDetail item = dayReservationRule.GetItem(j);
                    if (item.Weekday < 1 || item.Weekday > 7)
                    {
                        return false;
                    }
                    if (item.MealTimeIndex < 1 || item.Weekday > 8)
                    {
                        return false;
                    }
                    if (item.SerialNumber < 1 || item.Weekday > 8)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 对有效期参数进行编码
        /// </summary>
        /// <param name="databuf">需要填充参数结构的字节缓冲区</param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf len error");
            }
            WeekReservationRule.GetBytes(databuf);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 392;
        }

        /// <summary>
        /// 对有效期参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
          
        }
    }
}
