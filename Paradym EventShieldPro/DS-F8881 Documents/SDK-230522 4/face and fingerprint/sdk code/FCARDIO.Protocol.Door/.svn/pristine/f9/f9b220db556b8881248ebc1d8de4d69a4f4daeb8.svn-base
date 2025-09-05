using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data;
using DoNetDrive.Protocol.Door.Door8800.Transaction;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Transaction;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.Transaction.ReadTransactionDatabase
{
    /// <summary>
    ///  读取新记录
    ///  读指定类型的记录数据库最新记录，并读取指定数量。
    ///  成功返回结果参考 link ReadTransactionDatabase_Result 
    /// </summary>
    public class ReadTransactionDatabase : DoNetDrive.Protocol.Door.Door8800.Transaction.ReadTransactionDatabase
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
        public ReadTransactionDatabase(INCommandDetail cd, ReadTransactionDatabase_Parameter parameter) : base(cd, parameter)
        {
            CmdType = 0x48;
            CheckResponseCmdType = 0x28;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            base.CommandNext1(oPck);
        }

        /// <summary>
        /// 初始化实体创建工厂
        /// </summary>
        static ReadTransactionDatabase()
        {
            NewTransactionTable = new Func<AbstractTransaction>[7];
            NewTransactionTable[1] = () => new CardTransaction();//读卡记录
            NewTransactionTable[2] = () => new ButtonTransaction();//出门开关记录
            NewTransactionTable[3] = () => new DoorSensorTransaction();//门磁记录
            NewTransactionTable[4] = () => new SoftwareTransaction();//软件操作记录
            NewTransactionTable[5] = () => new AlarmTransaction();//报警记录
            NewTransactionTable[6] = () => new SystemTransaction();//系统记录
        }

        /// <summary>
        /// 获取一个事务实体
        /// </summary>
        /// <returns></returns>
        protected override AbstractTransaction GetNewTransaction()
        {
            return NewTransactionTable[mTransactionType ]();
        }
    }
}
