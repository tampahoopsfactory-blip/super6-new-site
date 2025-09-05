using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction
{
    /// <summary>
    /// 记录数据库类型
    /// </summary>
    public enum e_TransactionDatabaseType
    {
        /// <summary>
        /// 读卡相关记录
        /// </summary>
        OnCardTransaction = 1,

        /// <summary>
        /// 系统相关记录
        /// </summary>
        OnSystemTransaction = 2
    }
}
