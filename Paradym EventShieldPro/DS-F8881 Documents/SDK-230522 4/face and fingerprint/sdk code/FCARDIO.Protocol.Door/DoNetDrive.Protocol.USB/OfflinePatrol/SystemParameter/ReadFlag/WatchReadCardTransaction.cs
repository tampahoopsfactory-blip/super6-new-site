using DotNetty.Buffers;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ReadFlag
{
    /// <summary>
    /// 读卡消息
    /// </summary>
    public class WatchReadCardTransaction : AbstractTransaction
    {
        public int Card;
        /// 创建一个读卡消息
        /// </summary>
        public WatchReadCardTransaction()
        {
            _TransactionType = 1;
            _TransactionCode = 1;
            _IsNull = false;
        }


        public override int GetDataLen()
        {
            throw new NotImplementedException();
        }

        public override void SetBytes(IByteBuffer databuf)
        {
            _TransactionDate = DateTime.Now;
            Card = databuf.ReadUnsignedMedium();
        }
    }
}
