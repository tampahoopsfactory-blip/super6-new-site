using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 清空记录
    /// 表示把上传断点置为记录末尾编号相同，即无新记录
    /// </summary>
    public class ClearTransactionDatabase : ClearTransactionDatabase_Base
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ClearTransactionDatabase(DESDriveCommandDetail cd, ClearTransactionDatabase_Parameter parameter) : base(cd, parameter)
        {
            CmdPar = 0x01;
        }
    }
}
