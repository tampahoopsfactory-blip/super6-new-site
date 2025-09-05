using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.Transaction;
using System;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    ///  读取新记录
    ///  读指定类型的记录数据库最新记录，并读取指定数量。
    ///  成功返回结果参考 link ReadTransactionDatabase_Result 
    /// </summary>
    public class ReadTransactionDatabase : ReadTransactionDatabase_Base
    {
        /// <summary>
        /// 新记录实体创建工厂
        /// </summary>
        public static readonly Func<AbstractTransaction>[] NewTransactionTable;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadTransactionDatabase(DESDriveCommandDetail cd, ReadTransactionDatabase_Parameter parameter) : base(cd, parameter)
        { }

        /// <summary>
        /// 初始化实体创建工厂
        /// </summary>
        static ReadTransactionDatabase()
        {
            NewTransactionTable = new Func<AbstractTransaction>[3];
            NewTransactionTable[1] = () => new CardTransaction();//读卡记录
            NewTransactionTable[2] = () => new SystemTransaction();//系统记录
        }

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
