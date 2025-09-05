using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ReadTransactionDatabaseByIndex
{
    /// <summary>
    /// 读取控制器中的卡片数据库信息返回值
    /// </summary>
    public class ReadTransactionDatabaseByIndex_Result : INCommandResult
    {
        /// <summary>
        ///  记录数据库类型
        ///  1 读卡记录
        ///  2 系统记录
        /// </summary>
        public e_TransactionDatabaseType TransactionType;

        /// <summary>
        /// 读索引号
        /// </summary>
        public int ReadIndex;

        /// <summary>
        /// 读取数量
        /// </summary>
        public int Quantity;

        /// <summary>
        /// 记录列表
        /// </summary>
        public List<AbstractTransaction> TransactionList;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public ReadTransactionDatabaseByIndex_Result() { }

        /// <summary>
        ///创建结构
        /// </summary>
        /// <param name="_DatabaseType">记录数据库类型 取值1-6</param>
        /// <param name="_ReadIndex">读索引号</param>
        /// <param name="_Quantity">读取数量</param>
        /// <param name="_TransactionList">记录列表</param>
        public ReadTransactionDatabaseByIndex_Result(e_TransactionDatabaseType _DatabaseType, int _ReadIndex, int _Quantity, List<AbstractTransaction> _TransactionList)
        {
            TransactionType = _DatabaseType;
            ReadIndex = _ReadIndex;
            Quantity = _Quantity;
            TransactionList = _TransactionList;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }


    }
}
