using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.TimeGroup
{
    public class AddTimeGroup_Parameter : AbstractParameter
    {
        /// <summary>
        /// 写入索引
        /// </summary>
        private int writeIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        public readonly List<WeekTimeGroup> ListWeekTimeGroup;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list">开门时段集合</param>
        public AddTimeGroup_Parameter(List<WeekTimeGroup> list)
        {
            ListWeekTimeGroup = list;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ListWeekTimeGroup == null || ListWeekTimeGroup.Count != 64)
            {
                throw new ArgumentException("ListWeekTimeGroup.Count Error!");
            }

            return true;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void Dispose()
        {
            ListWeekTimeGroup.Clear();
        }

        /// <summary>
        /// 设置写入索引
        /// </summary>
        /// <param name="index">写入索引号</param>
        public void SetWriteIndex(int index)
        {
            if (index < ListWeekTimeGroup.Count)
            {
                writeIndex = index;
            }
        }

        /// <summary>
        /// 将 参数 编码到字节流
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x71;
        }

        /// <summary>
        /// 没有实现
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {


        }
    }
}
