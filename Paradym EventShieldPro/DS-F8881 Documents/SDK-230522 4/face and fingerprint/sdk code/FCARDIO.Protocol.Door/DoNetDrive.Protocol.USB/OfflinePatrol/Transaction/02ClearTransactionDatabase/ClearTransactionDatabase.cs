using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ClearTransactionDatabase
{
    /// <summary>
    /// 清空指定类型的记录数据库
    /// </summary>
    public class ClearTransactionDatabase 
        : Write_Command
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ClearTransactionDatabase(INCommandDetail cd, ClearTransactionDatabase_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ClearTransactionDatabase_Parameter model = value as ClearTransactionDatabase_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            ClearTransactionDatabase_Parameter model = _Parameter as ClearTransactionDatabase_Parameter;
            Packet(0x04, 0x03, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


    }
}
