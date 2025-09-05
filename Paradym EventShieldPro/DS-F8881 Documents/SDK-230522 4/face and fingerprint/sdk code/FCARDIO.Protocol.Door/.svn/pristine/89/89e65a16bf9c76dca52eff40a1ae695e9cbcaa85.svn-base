using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using System;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 更新记录指针命令
    /// </summary>
    public class WriteTransactionDatabaseIndex : Write_Command
    {
        /// <summary>
        /// 控制码参数
        /// </summary>
        protected byte CmdPar = 0x00;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public WriteTransactionDatabaseIndex(DESDriveCommandDetail cd, WriteTransactionDatabaseIndex_Parameter parameter) : base(cd, parameter)
        {
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteTransactionDatabaseIndex_Parameter model = value as WriteTransactionDatabaseIndex_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteTransactionDatabaseIndex_Parameter model = _Parameter as WriteTransactionDatabaseIndex_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x09, 0x03, CmdPar, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}
