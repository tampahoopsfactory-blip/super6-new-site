using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup
{
    /// <summary>
    /// 表示一天的时段 ,一天可以包含多个时段
    /// </summary>
    public class DayTimeGroup
    {
        /// <summary>
        /// 表示一个时段，开始时间和结束时间 集合
        /// </summary>
        protected TimeSegment[] mSegment;


        /// <summary>
        /// 初始化天时段
        /// </summary>
        /// <param name="SegmentCount">一天中的时段数量</param>
        public DayTimeGroup(int SegmentCount)
        {
            SetSegmentCount(SegmentCount);
        }

        /// <summary>
        /// 设置一天中包含的时段数量
        /// </summary>
        /// <param name="SegmentCount">时段数量</param>
        public virtual void SetSegmentCount(int SegmentCount)
        {
            mSegment = new TimeSegment[SegmentCount];
            for (int i = 0; i < SegmentCount; i++)
            {
                mSegment[i] = new TimeSegment();
            }
        }

        /// <summary>
        /// 获取一天中包含的时段数量
        /// </summary>
        /// <returns>时段数量</returns>
        public int GetSegmentCount()
        {
            if (mSegment == null)
            {
                return 0;
            }
            return mSegment.Length;
        }

        /// <summary>
        /// 获取一个时段，进行操作
        /// </summary>
        /// <param name="iIndex">此时段在这一天当中的索引号，索引从0开始</param>
        /// <returns></returns>
        public TimeSegment GetItem(int iIndex)
        {
            if (iIndex < 0 || iIndex >= GetSegmentCount())
            {
                throw new ArgumentException("iIndex<0 || iIndex >= " + GetSegmentCount().ToString());
            }
            return mSegment[iIndex];
        }

        /// <summary>
        /// 将对象写入到字节缓冲区
        /// </summary>
        /// <param name="bBuf"></param>
        public void GetBytes(IByteBuffer bBuf)
        {
            int iCount = GetSegmentCount();
            for (int i = 0; i < iCount; i++)
            {
                mSegment[i].GetBytes(bBuf);
            }

        }

        /// <summary>
        /// 从字节缓冲区中生成一个对象
        /// </summary>
        /// <param name="bBuf"></param>
        public void SetBytes(IByteBuffer bBuf)
        {
            int iCount = GetSegmentCount();

            int index = 2;
            for (int i = 0; i < iCount; i++)
            {
                mSegment[i].SetBytes(bBuf);
                index += 4;
                if (bBuf.ReadableBytes == 0 )
                {
                    return;
                }
            }
        }
    }
}
