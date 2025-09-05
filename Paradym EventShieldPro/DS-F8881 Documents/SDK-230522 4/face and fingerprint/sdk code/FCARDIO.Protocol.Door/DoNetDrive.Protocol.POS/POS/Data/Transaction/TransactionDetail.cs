using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Data
{
    public class TransactionDetail : DoNetDrive.Protocol.Door.Door8800.Data.TransactionDetail
    {

        /// <summary>
        /// 记录起始编号
        /// </summary>
        public long StartIndex;
        /// <summary>
        /// 记录末尾编号
        /// </summary>
        public long EndIndex;

        /// <summary>
        /// 可用的新记录数
        /// </summary>
        /// <returns>新记录数</returns>
        public override long readable()
        {
            //if (IsCircle)
            //{
            //    return DataBaseMaxSize;
            //}
            //if (ReadIndex > WriteIndex)
            //{
            //    ReadIndex = WriteIndex - DataBaseMaxSize;
            //    if (ReadIndex < 0) ReadIndex = 0;
            //}
            //if (ReadIndex == WriteIndex)
            //{
            //    return 0;
            //}

            //return (WriteIndex - ReadIndex);
            return EndIndex - WriteIndex;
        }

        /// <summary>
        /// 从字节缓冲区中生成一个对象
        /// </summary>
        /// <param name="data"></param>
        public new void SetBytes(IByteBuffer data)
        {
            DataBaseMaxSize = data.ReadUnsignedInt();
            StartIndex = data.ReadUnsignedInt();
            EndIndex = data.ReadUnsignedInt();
            WriteIndex = data.ReadUnsignedInt();
            return;
        }

    }
}
