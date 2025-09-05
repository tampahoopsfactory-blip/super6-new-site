using DotNetty.Buffers;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Data.Transaction
{
    /// <summary>
    /// 记录数据库的详情
    /// 读卡记录, 系统记录
    /// </summary>
    public class TransactionDatabaseDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public TransactionDetailBase[] ListTransaction { get; set; }
        /// <summary>
        /// 读卡相关记录
        /// </summary>
        public CardTransactionDetail CardTransactionDetail;

        /// <summary>
        /// 系统相关记录
        /// </summary>
        public SystemTransactionDetail SystemTransactionDetail;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public TransactionDatabaseDetail()
        {
            CardTransactionDetail = new CardTransactionDetail();
           
            SystemTransactionDetail = new SystemTransactionDetail();
        }

        /// <summary>
        /// 进行解码
        /// </summary>
        /// <param name="data"></param>
        public void SetBytes(IByteBuffer data)
        {
            ListTransaction = new TransactionDetailBase[]{CardTransactionDetail, SystemTransactionDetail};
            for (int i = 0; i < ListTransaction.Length; i++)
            {
                ListTransaction[i].SetBytes(data);
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IByteBuffer GetBytes()
        {
            return null;
        }
    }
}
