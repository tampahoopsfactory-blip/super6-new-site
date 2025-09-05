using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.POS.Data;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadTransactionDatabaseByIndex_Result : INCommandResult
    {
        /// <summary>
        /// 记录数据库类型
        /// 读卡记录
        /// 系统记录
        /// </summary>
        public e_TransactionDatabaseType DatabaseType;

        /// <summary>
        /// 读取数量
        /// </summary>
        public int Quantity;

        /// <summary>
        /// 第一个记录的序号（起始序号）
        /// </summary>
        public int RecordNum;

        /// <summary>
        /// 记录列表
        /// </summary>
        public List<AbstractTransaction> TransactionList;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public ReadTransactionDatabaseByIndex_Result() {
            NewTransactionTable = new Func<AbstractTransaction>[3];
            NewTransactionTable[1] = () => new CardTransaction();//读卡记录
            NewTransactionTable[2] = () => new SystemTransaction();//系统记录
        }

        Func<AbstractTransaction>[] NewTransactionTable;
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
        public void SetBytes(IByteBuffer buf,int transactionType)
        {
            Quantity = buf.ReadByte();//本数据包中包含的记录数
            RecordNum = buf.ReadInt();//本数据包中第一个记录的序号（起始序号）
            for (int i = 0; i < Quantity; i++)
            {
                AbstractTransaction cd = null; GetNewTransaction(transactionType);
                cd.SetSerialNumber(RecordNum + i);
                cd.SetBytes(buf);
                TransactionList.Add(cd);
            }
        }

        AbstractTransaction GetNewTransaction(int transactionType)
        {
            return NewTransactionTable[transactionType]();
        }
    }
}
