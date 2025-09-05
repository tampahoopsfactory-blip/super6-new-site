using DoNetDrive.Protocol.Door.Door8800.Data;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Data
{
    public class WeekReservationRule
    {
        /// <summary>
        /// 一周中的7天订餐
        /// </summary>
        protected DayReservationRule[] mDayReservationRule;


        /// <summary>
        /// 是否有值
        /// </summary>
        public bool HasValue { get; private set; }

      
        /**
         * 创建一周中的天时段
         */
        protected virtual void CreateDayReservationRule()
        {
            mDayReservationRule = new DayReservationRule[7];
            for (int i = 0; i < 7; i++)
            {
                mDayReservationRule[i] = new DayReservationRule(8);
            }
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="index">在周时段列表中的索引号</param>
        public WeekReservationRule()
        {
            CreateDayReservationRule();
        }

        
        /// <summary>
        /// 获取一天的时段
        /// </summary>
        /// <param name="week">星期的枚举</param>
        /// <returns></returns>
        public DayReservationRule GetItem(E_WeekDay week)
        {
            return mDayReservationRule[(int)week];
        }

        /// <summary>
        /// 获取一天的时段
        /// </summary>
        /// <param name="index">索引号</param>
        /// <returns></returns>
        public DayReservationRule GetItem(int index)
        {
            return mDayReservationRule[index];
        }

        /// <summary>
        /// 获取完整的长度
        /// </summary>
        /// <returns></returns>
        public virtual int GetDataLen()
        {
            return 7 * 8 * 7;
        }

        /// <summary>
        /// 从字节缓冲区中生成一个对象
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetBytes(IByteBuffer data)
        {
            SetBytes(E_WeekDay.Monday, data);
        }

        /// <summary>
        /// 提供给 门工作方式 使用
        /// </summary>
        /// <param name="data"></param>
        public virtual void ReadDoorWorkSetBytes(IByteBuffer data)
        {
            SetBytes(E_WeekDay.Monday, data);
        }

        /// <summary>
        /// 从缓冲区中获取值并初始化周时段
        /// </summary>
        /// <param name="FistWeek">一周的第一天</param>
        /// <param name="data"></param>
        public virtual void SetBytes(E_WeekDay FistWeek, IByteBuffer data)
        {
            HasValue = true;
            int[] WeekList = new int[7];
            GetWeekList(FistWeek, WeekList);
            for (int i = 0; i < 7; i++)
            {
                mDayReservationRule[WeekList[i]].SetBytes(data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FistWeek">一周的第一天</param>
        /// <param name="WeekList">一周的天数集合</param>
        protected void GetWeekList(E_WeekDay FistWeek, int[] WeekList)
        {
            int lBeginIndex = (int)FistWeek;
            int iEndIndex = 6 - lBeginIndex;
            for (int i = 0; i <= iEndIndex; i++)
            {
                WeekList[i] = lBeginIndex + i;
            }
            if (lBeginIndex != 0)
            {
                iEndIndex += 1;
                lBeginIndex = 0;
                for (int i = iEndIndex; i <= 6; i++)
                {
                    WeekList[i] = lBeginIndex;
                    lBeginIndex += 1;
                }
            }
        }

        /// <summary>
        /// 使用从周一为一周的第一天进行排序的缓冲区获取时段信息
        /// </summary>
        /// <param name="data"></param>
        public virtual void GetBytes(IByteBuffer data)
        {
            GetBytes(E_WeekDay.Monday, data);
        }

        /// <summary>
        /// 使用从周一为一周的第一天进行排序的缓冲区获取时段信息
        /// </summary>
        /// <param name="FistWeek">一周的第一天</param>
        /// <param name="data"></param>
        public virtual void GetBytes(E_WeekDay FistWeek, IByteBuffer data)
        {
            int[] WeekList = new int[7];
            GetWeekList(FistWeek, WeekList);
            for (int i = 0; i < 7; i++)
            {
                mDayReservationRule[WeekList[i]].GetBytes(data);
            }
        }
    }
}
