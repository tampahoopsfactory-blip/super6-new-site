using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.Fingerprint.Data.Transaction;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Transaction;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    /// 读取记录算法，封装读取记录的步骤
    /// </summary>
    public class ReadTransactionDatabaseSubCommand<T> : BaseReadTransactionDatabaseSubCommand
        where T : AbstractTransaction, new()
    {
        /// <summary>
        /// 创建一个读取事务的子命令
        /// </summary>
        /// <param name="mainCmd"></param>
        public ReadTransactionDatabaseSubCommand(ICombinedCommand mainCmd) : base(mainCmd)
        {
        }

        /// <summary>
        /// 获取一个新的事务实例
        /// </summary>
        /// <returns></returns>
        protected override AbstractTransaction GetNewTransaction()
        {
            return new T();
        }
    }
}
