using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.WriteTransactionDatabaseWriteIndex
{

    /// <summary>
    /// 修改指定记录数据库的写索引
    /// 记录尾地址
    /// </summary>
    public class WriteTransactionDatabaseWriteIndex : Write_Command
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public WriteTransactionDatabaseWriteIndex(INCommandDetail cd, WriteTransactionDatabaseWriteIndex_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteTransactionDatabaseWriteIndex_Parameter model = value as WriteTransactionDatabaseWriteIndex_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteTransactionDatabaseWriteIndex_Parameter model = _Parameter as WriteTransactionDatabaseWriteIndex_Parameter;
            Packet(0x04, 0x05, 0x05, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

    }
}
