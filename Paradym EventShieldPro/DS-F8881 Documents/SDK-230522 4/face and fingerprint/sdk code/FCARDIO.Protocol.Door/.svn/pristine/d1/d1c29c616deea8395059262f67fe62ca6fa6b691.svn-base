using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    /// 清空指定类型的记录数据库
    /// </summary>
    public class ClearTransactionDatabase_Parameter:AbstractParameter
    {
        /// <summary>
        /// 记录数据库类型
        /// 1  读卡记录 
        /// 2  门磁记录  
        /// 3  系统记录 
        /// 4  体温记录
        /// </summary>
        public e_TransactionDatabaseType? DatabaseType;
        
        /// <summary>
        /// 清空指定类型的记录数据库
        /// </summary>
        /// <param name="type">取值范围 1-6</param>
        public ClearTransactionDatabase_Parameter(e_TransactionDatabaseType type)
        {
            DatabaseType = type;
        }

        /// <summary>
        /// 清空所有记录
        /// </summary>
        public ClearTransactionDatabase_Parameter()
        {

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {

            if (DatabaseType == null)
            {
                throw new ArgumentException("DatabaseType Error!");
            }

            int iType = (int)DatabaseType;
            if (iType < 1 || iType > 4)
                throw new ArgumentException("DatabaseType Error!");

            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (DatabaseType.HasValue)
            {
                databuf.WriteByte((int)DatabaseType.Value);
            }
            
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            
        }
    }
}
