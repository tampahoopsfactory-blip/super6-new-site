using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.WriteTransactionDatabaseReadIndex
{
    /// <summary>
    /// 更新记录指针
    /// </summary>
    public class WriteTransactionDatabaseReadIndex : Write_Command
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public WriteTransactionDatabaseReadIndex(INCommandDetail cd, WriteTransactionDatabaseReadIndex_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteTransactionDatabaseReadIndex_Parameter model = value as WriteTransactionDatabaseReadIndex_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteTransactionDatabaseReadIndex_Parameter model = _Parameter as WriteTransactionDatabaseReadIndex_Parameter;
            Packet(0x04, 0x04, 0x06, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

    }
}
