using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Data
{
    public class CardTransactionDetail : TransactionDetail
    {
        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public new int GetDataLen()
        {
            return 0x20;
        }

        /// <summary>
        /// 从字节缓冲区中生成一个对象
        /// </summary>
        /// <param name="data"></param>
        public void SetBytes(IByteBuffer data)
        {
            DataBaseMaxSize = data.ReadUnsignedInt();
            StartIndex = data.ReadUnsignedInt();
            WriteIndex = data.ReadUnsignedInt();
            ReadIndex = data.ReadUnsignedInt();
            return;
        }
    }
}
