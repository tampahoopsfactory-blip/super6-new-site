using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 清空所有类型的记录数据库
    /// </summary>
    public class TransactionDatabaseEmpty : Write_Command
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        public TransactionDatabaseEmpty(DESDriveCommandDetail cd) : base(cd, null) { }

        /// <summary>
        /// 检查 参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return true;
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x09, 0x02, 0x00);
        }
    }
}
