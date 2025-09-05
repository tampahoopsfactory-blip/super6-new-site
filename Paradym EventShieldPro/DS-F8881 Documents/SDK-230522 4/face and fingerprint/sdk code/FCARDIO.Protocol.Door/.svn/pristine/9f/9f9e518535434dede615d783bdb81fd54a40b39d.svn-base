using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.TransactionDatabaseDetail
{
    /// <summary>
    /// 读取控制器中的卡片数据库信息
    /// </summary>
    public class ReadTransactionDatabaseDetail_Result : INCommandResult
    {
        /// <summary>
        /// 记录数据库的详情
        /// </summary>
        public Data.Transaction.TransactionDatabaseDetail DatabaseDetail;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public ReadTransactionDatabaseDetail_Result() {
            DatabaseDetail = new Data.Transaction.TransactionDatabaseDetail();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            DatabaseDetail = null;
        }

        /// <summary>
        /// 进行解码
        /// </summary>
        /// <param name="buf"></param>
        internal void SetBytes(IByteBuffer buf)
        {
            DatabaseDetail.SetBytes(buf);
        }
    }
}
