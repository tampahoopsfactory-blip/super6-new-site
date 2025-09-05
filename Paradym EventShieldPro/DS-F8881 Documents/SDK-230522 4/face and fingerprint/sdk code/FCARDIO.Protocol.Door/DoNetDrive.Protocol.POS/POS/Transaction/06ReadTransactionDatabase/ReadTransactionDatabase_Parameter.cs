using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 读取新记录
    /// </summary>
    public class ReadTransactionDatabase_Parameter : AbstractParameter
    {
        /// <summary>
        /// 记录数据库类型
        /// 1  读卡记录 
        /// 2  系统记录  
        /// </summary>
        public e_TransactionDatabaseType DatabaseType;

        public int PacketSize;
        public int Quantity;
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="type">取值范围 1-4</param>
        /// <param name="_Quantity">读取数量</param>
        public ReadTransactionDatabase_Parameter(int type, int _Quantity)
        {
            DatabaseType = (e_TransactionDatabaseType)type;

            Quantity = _Quantity;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            int iType = (int)DatabaseType;
            if (iType < 1 || iType > 2)
                throw new ArgumentException("DatabaseType Error!");

            if (PacketSize < 1 || PacketSize > 200)
            {
                throw new ArgumentException("PacketSize Error!");
            }
            if (Quantity < 0 || Quantity > 1000)
            {
                throw new ArgumentException("Quantity Error!");
            }
            return true;
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            throw new NotImplementedException();
        }

        public override int GetDataLen()
        {
            throw new NotImplementedException();
        }

        public override void SetBytes(IByteBuffer databuf)
        {
            throw new NotImplementedException();
        }
    }
}
