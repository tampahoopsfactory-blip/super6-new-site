using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Protocol.Door.Door8800.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.TimeGroup
{
    /// <summary>
    /// 读取所有开门时段结果
    /// </summary>
    public class ReadTimeGroup_Result : INCommandResult
    {
        /// <summary>
        /// 返回的总数量
        /// </summary>
        public int Count;

        /// <summary>
        /// 开门时段集合
        /// </summary>
        public List<WeekTimeGroup> ListWeekTimeGroup;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public ReadTimeGroup_Result()
        {
            ListWeekTimeGroup = new List<WeekTimeGroup>();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            ListWeekTimeGroup.Clear();
            ListWeekTimeGroup = null;
        }

        
    }
}
