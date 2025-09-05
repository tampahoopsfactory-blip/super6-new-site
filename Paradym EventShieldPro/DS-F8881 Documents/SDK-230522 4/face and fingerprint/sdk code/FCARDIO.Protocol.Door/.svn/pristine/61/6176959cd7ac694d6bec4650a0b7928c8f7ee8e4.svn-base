using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup
{
    /// <summary>
    /// 表示一个完整时段，一个时段里包含7天
    /// </summary>
    public class WeekTimeGroup
    {
        /// <summary>
        /// 一周中的天段
        /// </summary>
        protected DayTimeGroup[] mDay;

        /// <summary>
        /// 
        /// </summary>
        private int iDaySegmentCount;

        /// <summary>
        /// 获取在周时段列表中的索引号
        /// </summary>
        protected int mIndex;

        /// <summary>
        /// 一天中的时段数量
        /// </summary>
        protected int DaySegmentCount;

        /// <summary>
        /// 是否有值
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// 初始化一个周时段
        /// </summary>
        /// <param name="iDaySegmentCount">一天中的时段数量</param>
        public WeekTimeGroup(int iDaySegmentCount)
        {

            DaySegmentCount = iDaySegmentCount;
            CreateDayTimeGroup();
        }

        /**
         * 创建一周中的天时段
         */
        protected virtual void CreateDayTimeGroup()
        {
            mDay = new DayTimeGroup[7];
            for (int i = 0; i < 7; i++)
            {
                mDay[i] = new DayTimeGroup(DaySegmentCount);
            }
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="iDaySegmentCount">一天中的时段数量</param>
        /// <param name="index">在周时段列表中的索引号</param>
        public WeekTimeGroup(int iDaySegmentCount, int index)
        {
            this.iDaySegmentCount = iDaySegmentCount;
            mIndex = index;
        }

        /**
         * 获取在周时段列表中的索引号
         *
         * @return 索引号 1-64
         */
        public int GetIndex()
        {
            return mIndex;
        }

        /// <summary>
        /// 设定在周时段列表中的索引号
        /// </summary>
        /// <param name="index">索引号</param>
        public void SetIndex(int index)
        {
            mIndex = index;
        }

        /// <summary>
        /// 获取一天的时段
        /// </summary>
        /// <param name="week">星期的枚举</param>
        /// <returns></returns>
        public DayTimeGroup GetItem(E_WeekDay week)
        {
            return mDay[(int)week];
        }

        /// <summary>
        /// 获取一天的时段
        /// </summary>
        /// <param name="index">索引号</param>
        /// <returns></returns>
        public DayTimeGroup GetItem(int index)
        {
            return mDay[index];
        }

        /// <summary>
        /// 获取完整的长度
        /// </summary>
        /// <returns></returns>
        public virtual int GetDataLen()
        {
            return 7 * DaySegmentCount * 4;
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
            int [] WeekList = new int[7];
            GetWeekList(FistWeek, WeekList);
            for (int i = 0; i < 7; i++)
            {
                mDay[WeekList[i]].SetBytes(data);
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
            int [] WeekList = new int[7];
            GetWeekList(FistWeek, WeekList);
            for (int i = 0; i < 7; i++)
            {
                mDay[WeekList[i]].GetBytes(data);
            }
        }

        /// <summary>
        /// 克隆一个周时段
        /// </summary>
        /// <returns></returns>
        public virtual WeekTimeGroup Clone()
        {
            WeekTimeGroup w = new WeekTimeGroup(DaySegmentCount);
            IByteBuffer bBuf = DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(DaySegmentCount * 4);
            for (int i = 0; i < 10; i++)
            {
                mDay[i].GetBytes(bBuf);   
                w.mDay[i].SetBytes(bBuf);
                bBuf.Clear();
            }
            return w;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetNowTime()
        {
            for (int y = 0; y < 7; y++)
            {
                DayTimeGroup dayTimeGroup = GetItem(y);
                //每天时段
                for (int i = 0; i < 8; i++)
                {
                    DateTime dt = DateTime.Now;
                    //dt = dt.AddMinutes(-1);
                    TimeSegment segment = dayTimeGroup.GetItem(i);
                    dt = dt.AddMinutes(i + 1);
                    segment.SetBeginTime(dt.Hour, dt.Minute);
                    dt = dt.AddMinutes(i + 1);
                    segment.SetEndTime(dt.Hour, dt.Minute);
                }
            }
            HasValue = true;
        }

        /// <summary>
        /// 初始化 一个完整时段
        /// </summary>
        public void InitTimeGroup()
        {
            for (int y = 0; y < 7; y++)
            {
                DayTimeGroup dayTimeGroup = GetItem(y);
                //每天时段
                for (int i = 0; i < 8; i++)
                {
                    TimeSegment segment = dayTimeGroup.GetItem(i);
                    segment.SetBeginTime(0, 0);
                    segment.SetEndTime(0, 0);
                }
            }
            HasValue = true;
        }
    }
}
