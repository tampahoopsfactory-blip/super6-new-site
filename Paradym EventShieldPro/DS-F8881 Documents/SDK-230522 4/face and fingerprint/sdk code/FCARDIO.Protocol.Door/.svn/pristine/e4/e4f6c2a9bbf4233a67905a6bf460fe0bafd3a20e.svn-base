using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Fingerprint.Data.Transaction;
using DoNetDrive.Protocol.Transaction;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    /// 读记录数据库
    /// 按指定索引号开始读指定类型的记录数据库，并读取指定数量。
    /// 成功返回结果参考 ReadTransactionDatabaseByIndex_Result 
    /// </summary>
    public class ReadTransactionDatabaseByIndex : Protocol.Door.Door8800.Transaction.ReadTransactionDatabaseByIndex_Base
    {
        /// <summary>
        /// 新记录实体创建工厂
        /// </summary>
        public static readonly Func<AbstractTransaction>[] NewTransactionTable;
        /// <summary>
        /// 初始化实体创建工厂
        /// </summary>
        static ReadTransactionDatabaseByIndex()
        {
            NewTransactionTable = new Func<AbstractTransaction>[5];
            NewTransactionTable[1] = () => new CardTransaction();//读卡记录
            NewTransactionTable[2] = () => new DoorSensorTransaction();//门磁记录
            NewTransactionTable[3] = () => new SystemTransaction();//系统记录
            NewTransactionTable[4] = () => new BodyTemperatureTransaction();//系统记录
        }


        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadTransactionDatabaseByIndex(INCommandDetail cd, ReadTransactionDatabaseByIndex_Parameter parameter):base(cd,parameter)
        {        }

        /// <summary>
        /// 获取一个事务实体
        /// </summary>
        /// <returns></returns>
        protected override AbstractTransaction GetNewTransaction()
        {
            return NewTransactionTable[mTransactionType]();
        }
    }
}
