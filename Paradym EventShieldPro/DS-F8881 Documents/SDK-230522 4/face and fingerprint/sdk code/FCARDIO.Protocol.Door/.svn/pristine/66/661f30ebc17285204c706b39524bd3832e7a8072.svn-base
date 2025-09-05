using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Transaction
{
    /// <summary>
    /// 读记录数据库
    /// 按指定索引号开始读指定类型的记录数据库，并读取指定数量。
    /// 成功返回结果参考 ReadTransactionDatabaseByIndex_Result 
    /// </summary>
    public class ReadTransactionDatabaseByIndex : ReadTransactionDatabaseByIndex_Base
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
            NewTransactionTable = new Func<AbstractTransaction>[7];
            NewTransactionTable[1]=()=> new CardTransaction();//读卡记录
            NewTransactionTable[2]=() => new ButtonTransaction();//出门开关记录
            NewTransactionTable[3]=() => new DoorSensorTransaction();//门磁记录
            NewTransactionTable[4]=() => new SoftwareTransaction();//软件操作记录
            NewTransactionTable[5]=() => new AlarmTransaction();//报警记录
            NewTransactionTable[6]=() => new SystemTransaction();//系统记录
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
