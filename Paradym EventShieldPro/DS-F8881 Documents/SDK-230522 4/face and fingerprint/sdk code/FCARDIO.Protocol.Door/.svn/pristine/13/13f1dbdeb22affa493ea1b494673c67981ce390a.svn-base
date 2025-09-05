using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup
{
    /// <summary>
    /// 读卡认证方式 的开门时段
    /// </summary>
    public class WeekTimeGroup_ReaderWork : WeekTimeGroup
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="iDaySegmentCount">一周的时段数量</param>
        public WeekTimeGroup_ReaderWork(int iDaySegmentCount) : base(iDaySegmentCount)
        {
        }

        /// <summary>
        /// 初始化一个周时段
        /// </summary>
        protected override void CreateDayTimeGroup()
        {
            mDay = new DayTimeGroup_ReaderWork[7];
            for (int i = 0; i < 7; i++)
            {
                mDay[i] = new DayTimeGroup_ReaderWork(DaySegmentCount);
            }
        }

        /// <summary>
        /// 获取一个周时段长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 7 * DaySegmentCount * 5;
        }

        /// <summary>
        /// 从缓冲区中获取值并初始化周时段
        /// </summary>
        /// <param name="FistWeek">一周的第一天</param>
        /// <param name="data"></param>
        public override void SetBytes(E_WeekDay FistWeek, IByteBuffer data)
        {
            int[] WeekList = new int[7];
            GetWeekList(FistWeek, WeekList);
            for (int i = 0; i < 7; i++)
            {
                mDay[WeekList[i]].SetBytes(data);
            }
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="data"></param>
        public override void SetBytes(IByteBuffer data)
        {
            SetBytes(E_WeekDay.Monday, data);
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="data"></param>
        public override void GetBytes(IByteBuffer data)
        {
            GetBytes(E_WeekDay.Monday, data);
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="FistWeek">一周的第一天</param>
        /// <param name="data"></param>
        public override void GetBytes(E_WeekDay FistWeek, IByteBuffer data)
        {
            int[] WeekList = new int[7];
            GetWeekList(FistWeek, WeekList);
            for (int i = 0; i < 7; i++)
            {
                mDay[WeekList[i]].GetBytes(data);
            }
        }
    }
}
