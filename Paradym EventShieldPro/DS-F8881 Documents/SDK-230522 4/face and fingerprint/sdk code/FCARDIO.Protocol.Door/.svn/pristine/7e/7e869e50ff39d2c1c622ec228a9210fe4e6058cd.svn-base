using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using System;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 清空指定类型的记录数据库
    /// </summary>
    public class ClearTransactionDatabase_Base : Write_Command
    {
        /// <summary>
        /// 控制码参数
        /// </summary>
        protected byte CmdPar = 0x01;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ClearTransactionDatabase_Base(DESDriveCommandDetail cd, ClearTransactionDatabase_Parameter parameter) : base(cd, parameter) {
        }

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

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x09, 0x02, CmdPar, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}
