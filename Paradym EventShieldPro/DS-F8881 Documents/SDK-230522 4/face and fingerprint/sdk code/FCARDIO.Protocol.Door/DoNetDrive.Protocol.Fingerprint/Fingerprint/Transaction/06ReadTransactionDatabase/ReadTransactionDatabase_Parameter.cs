using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    /// 读取新记录
    /// </summary>
    public class ReadTransactionDatabase_Parameter
        : DoNetDrive.Protocol.Door.Door8800.Transaction.ReadTransactionDatabase_Parameter
    {
        /// <summary>
        /// 自动回滚读索引（适用于人脸机固件版本小于V4.41）
        /// </summary>
        public bool RollbackWriteReadIndex;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="type">取值范围 1-4</param>
        /// <param name="_Quantity">读取数量</param>
        public ReadTransactionDatabase_Parameter(int type, int _Quantity) :
            base((Protocol.Door.Door8800.Transaction.e_TransactionDatabaseType)type, _Quantity)
        {
            RollbackWriteReadIndex = false;
            PacketSize = 60;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            int iType = (int)DatabaseType;
            if (iType < 1 || iType > 4)
                throw new ArgumentException("DatabaseType Error!");

            if (PacketSize < 1 || PacketSize > 60)
            {
                PacketSize=60;
            }
            if (Quantity < 0 || Quantity > 1000000)
            {
                throw new ArgumentException("Quantity Error!");
            }
            return true;
        }
    }
}
