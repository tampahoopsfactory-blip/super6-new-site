using DotNetty.Buffers;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data.Transaction
{
    /// <summary>
    /// 保活包记录
    /// </summary>
    public class KeepaliveTransaction : AbstractTransaction
    {
        /// <summary>
        /// 创建一个保活包记录
        /// </summary>
        public KeepaliveTransaction()
        {
            _TransactionType = 0x22;//保活包记录
        }

        /// <summary>
        /// 指示一个事务记录所占用的缓冲区长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0;
        }

        /// <summary>
        /// 使用缓冲区构造一个事务实例
        /// </summary>
        /// <param name="data">缓冲区</param>
        public override void SetBytes(IByteBuffer data)
        {
            try
            {
                _IsNull = false;
                _TransactionCode = 0x22;
                _TransactionDate = DateTime.Now;

            }
            catch (Exception e)
            {
            }

            return;
        }
    }
}
