using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Fingerprint.Data;
namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    /// 读取控制器中的卡片数据库信息
    /// </summary>
    public class ReadTransactionDatabaseDetail_Result : INCommandResult
    {
        /// <summary>
        /// 记录数据库的详情
        /// </summary>
        public TransactionDatabaseDetail DatabaseDetail;

        /// <summary>
        /// 记录数据库详情缓冲区
        /// </summary>
        public byte[] ResultContent;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public ReadTransactionDatabaseDetail_Result()
        {
            DatabaseDetail = new TransactionDatabaseDetail();
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
        public void SetBytes(IByteBuffer buf)
        {
            ResultContent = new byte[buf.ReadableBytes];
            int iMark = buf.ReaderIndex;
            buf.ReadBytes(ResultContent);
            buf.SetReaderIndex(iMark);

            DatabaseDetail.SetBytes(buf);
        }
    }
}
