using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using System;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 更新记录起始号命令
    /// </summary>
    public class WriteTransactionDatabaseStartIndex : WriteTransactionDatabaseIndex
    {
       
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public WriteTransactionDatabaseStartIndex(DESDriveCommandDetail cd, WriteTransactionDatabaseIndex_Parameter parameter) : base(cd, parameter)
        {
            CmdPar = 0x01;
        }
    }
}
