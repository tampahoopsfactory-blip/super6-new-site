using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDatabase
{
    /// <summary>
    /// 读取所有巡更人员 返回结果
    /// </summary>
    public class ReadPatrolEmplDatabase_Result : INCommandResult
    {
        /// <summary>
        /// 总传输数量
        /// </summary>
        public ushort Quantity;

        /// <summary>
        /// 读取到的卡片列表
        /// </summary>
        public List<Data.PatrolEmpl> PatrolEmplList;

        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
    }
}
