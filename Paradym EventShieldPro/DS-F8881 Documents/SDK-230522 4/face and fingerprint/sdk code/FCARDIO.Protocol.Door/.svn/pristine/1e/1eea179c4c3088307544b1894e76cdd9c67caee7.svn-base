using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 清空记录序号
    /// 表示把记录起始号和记录尾号、上传断线都设置为0
    /// </summary>
    public class ClearTransactionDatabase_StartIndex : ClearTransactionDatabase_Base
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ClearTransactionDatabase_StartIndex(DESDriveCommandDetail cd, ClearTransactionDatabase_Parameter parameter) : base(cd, parameter)
        {
            CmdPar = 0x02;
        }
    }
}
