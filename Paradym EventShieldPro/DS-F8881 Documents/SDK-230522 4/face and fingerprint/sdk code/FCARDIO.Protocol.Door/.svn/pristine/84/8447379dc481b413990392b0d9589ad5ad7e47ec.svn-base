using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.WritePatrolEmpl
{
    /// <summary>
    /// 添加巡更人员失败结果
    /// </summary>
    public class WritePatrolEmpl_Result : INCommandResult
    {
        /// <summary>
        /// 读取到的工号列表
        /// </summary>
        public List<ushort> PCodeList;

        /// <summary>
        /// 创建结构
        /// </summary>
        public WritePatrolEmpl_Result(List<ushort> list)
        {
            PCodeList = list;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            PCodeList?.Clear();
            PCodeList = null;
        }



    }
}
