using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 读取记录 参数
    /// </summary>
    public class ReadTransactionDatabaseByIndex_Parameter : AbstractParameter
    {
        /// <summary>
        /// 记录数据库类型
        /// 1  读卡记录 
        /// 2  系统记录  
        /// </summary>
        public e_TransactionDatabaseType DatabaseType;

        public int Quantity;

        public int Index;
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="type">取值范围 1-4</param>
        /// <param name="_Index">序号</param>
        /// <param name="_Quantity">读取数量</param>
        public ReadTransactionDatabaseByIndex_Parameter(int type, int _Index, int _Quantity)
        {
            DatabaseType = (e_TransactionDatabaseType)type;
            Index = _Index;
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
            if (Index < 0)
            {
                throw new ArgumentException("Index Error!");
            }
            //if (iType == 1 && (Index + Quantity) > 50000)
            //{
            //    throw new ArgumentException("Index Error!");
            //}
            //if (iType == 2 && (Index + Quantity) > 40000)
            //{
            //    throw new ArgumentException("Index Error!");
            //}

            if (Quantity <= 0 || Quantity > 1000)
            {
                throw new ArgumentException("Quantity Error!");
            }
            return true;
        }

        public override void Dispose()
        {

        }

        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != 7)
            {
                throw new ArgumentException("Error");
            }
            databuf.WriteByte((byte)DatabaseType);
            databuf.WriteInt(Index);
            databuf.WriteShort(Quantity);
            return databuf;
        }

        public override int GetDataLen()
        {
            return 7;
        }

        public override void SetBytes(IByteBuffer databuf)
        {

        }
    }
}