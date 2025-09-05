using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadTransactionDatabase_Result : INCommandResult
    {
        /// <summary>
        /// 记录数据库类型
        /// 读卡记录
        /// 出门开关记录
        /// 门磁记录
        /// 软件操作记录
        /// 报警记录
        /// 系统记录
        /// </summary>
        public e_TransactionDatabaseType DatabaseType;

        /// <summary>
        /// 读取数量
        /// </summary>
        public int Quantity;

        /// <summary>
        /// 剩余新记录数量
        /// </summary>
        public int readable;

        /// <summary>
        /// 记录列表
        /// </summary>
        public List<AbstractTransaction> TransactionList;

        /// <summary>
        /// 读取记录后，记录的读取索引（上传断点）
        /// </summary>
        public int TransactionReadIndex;


        /// <summary>
        /// 初始化参数
        /// </summary>
        public ReadTransactionDatabase_Result() { }

      
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            TransactionList?.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        internal void SetBytes(IByteBuffer buf)
        {
            
        }
    }
}
