using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.SystemParameter.ConsumeLogStatisticsTime
{
    public class WriteConsumeLogStatisticsTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 统计时间点HHMM 范围 0000 - 2359
        /// </summary>
        public int Time;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteConsumeLogStatisticsTime_Parameter()
        {

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="mode">记录存储方式 0表示记录满循环，1表示记录满不循环</param>
        public WriteConsumeLogStatisticsTime_Parameter(int Time)
        {
            this.Time = Time;
        }

        /// <summary>
        /// 检查参数 0或1
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Time > 2359 || Time < 0)
                throw new ArgumentException("Time Error!");
            return true;
        }

        public override void Dispose()
        {

        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteShort(Time);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 2;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Time = databuf.ReadShort();
        }
    }
}
