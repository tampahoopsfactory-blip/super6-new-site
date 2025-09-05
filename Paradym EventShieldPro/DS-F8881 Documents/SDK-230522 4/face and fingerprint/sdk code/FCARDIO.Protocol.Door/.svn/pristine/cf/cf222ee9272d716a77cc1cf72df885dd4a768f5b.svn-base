using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    /// 读取控制器中的卡片数据库信息返回值
    /// </summary>
    public class ReadTransactionDatabaseByIndex_Parameter
        : Protocol.Door.Door8800.Transaction.ReadTransactionDatabaseByIndex_Parameter
    {

        /// <summary>
        ///创建结构
        /// </summary>
        /// <param name="_DatabaseType">记录数据库类型 取值1-4</param>
        /// <param name="_ReadIndex">读索引号</param>
        /// <param name="_Quantity">读取数量</param>
        public ReadTransactionDatabaseByIndex_Parameter(int _DatabaseType, int _ReadIndex, int _Quantity)
            :base(_DatabaseType, _ReadIndex, _Quantity)
        {
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (TransactionType < 1 || TransactionType > 4)
                throw new ArgumentException("DatabaseType Error!");
            if (Quantity <= 0 || Quantity > 60)
            {
                throw new ArgumentException("Quantity Error!");
            }
            if (ReadIndex <= 0 )
            {
                throw new ArgumentException("ReadIndex Error!");
            }
            return true;
        }
    }
}
