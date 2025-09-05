using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup
{
    /// <summary>
    /// 读卡认证 表示一天的时段 ,一天可以包含多个时段
    /// </summary>
    public class DayTimeGroup_ReaderWork : DayTimeGroup
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="SegmentCount">一天的时段数量</param>
        public DayTimeGroup_ReaderWork(int SegmentCount) : base(SegmentCount)
        {
        }

        /// <summary>
        /// 设置一天可以包含多个时段
        /// </summary>
        /// <param name="SegmentCount">一天的时段数量</param>
        public override void SetSegmentCount(int SegmentCount)
        {
            mSegment = new TimeSegment_ReaderWork[SegmentCount];
            for (int i = 0; i < SegmentCount; i++)
            {
                mSegment[i] = new TimeSegment_ReaderWork();
            }
        }
    }
}
