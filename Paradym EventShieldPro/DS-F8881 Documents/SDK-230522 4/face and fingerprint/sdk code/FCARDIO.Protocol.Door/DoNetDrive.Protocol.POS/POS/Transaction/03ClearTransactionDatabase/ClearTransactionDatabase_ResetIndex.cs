using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 重定位起始序号
    /// 表示把记录起始号、上传断点都设置为记录尾号的值
    /// </summary>
    public class ClearTransactionDatabase_ResetIndex : ClearTransactionDatabase_Base
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ClearTransactionDatabase_ResetIndex(DESDriveCommandDetail cd, ClearTransactionDatabase_Parameter parameter) : base(cd, parameter)
        {
            CmdPar = 0x03;
        }
    }
}
