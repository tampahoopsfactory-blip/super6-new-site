using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Data.Transaction
{
    /// <summary>
    /// 事件日志详情，包含数据库容量，记录索引，已读取索引，循环标志4个部分
    /// </summary>
    public class CardTransactionDetail : TransactionDetailBase
    {
       

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public int GetDataLen()
        {
            return 13;
        }

        /// <summary>
        /// 从字节缓冲区中生成一个对象
        /// </summary>
        /// <param name="data"></param>
        public override void SetBytes(IByteBuffer data)
        {
            DataBaseMaxSize = data.ReadUnsignedInt();
            WriteIndex = data.ReadUnsignedInt();
            ReadIndex = data.ReadUnsignedInt();
            IsCircle = data.ReadBoolean();
            return;
        }

     
       
    }
}
