

using DoNetDrive.Protocol.Door.Door8800.Data;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.POS.Data
{
    public class TransactionDatabaseDetail //:DoNetDrive.Protocol.Door.Door8800.Data.TransactionDatabaseDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public TransactionDetail[] ListTransaction { get; set; }

        /// <summary>
        /// 读卡相关记录
        /// </summary>
        public TransactionDetail CardTransactionDetail;
       
        /// <summary>
        /// 系统相关记录
        /// </summary>
        public TransactionDetail SystemTransactionDetail;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public TransactionDatabaseDetail()
        {
            CardTransactionDetail = new TransactionDetail();
            SystemTransactionDetail = new TransactionDetail();
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public int GetDataLen()
        {
            return 0x20;
        }

        /// <summary>
        /// 进行解码
        /// </summary>
        /// <param name="data"></param>
        public void SetBytes(IByteBuffer data)
        {
            ListTransaction = new TransactionDetail[] { CardTransactionDetail, SystemTransactionDetail };


            for (int i = 0; i < ListTransaction.Length; i++)
            {
                ListTransaction[i].SetBytes(data);

                if (ListTransaction[i].ReadIndex > ListTransaction[i].WriteIndex)
                {
                    ListTransaction[i].ReadIndex = 0;

                }
            }
            return;
        }
    }
}
