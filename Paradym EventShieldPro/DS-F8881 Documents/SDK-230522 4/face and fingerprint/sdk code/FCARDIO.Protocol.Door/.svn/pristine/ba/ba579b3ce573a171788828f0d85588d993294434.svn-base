using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Data.Transaction
{
    /// <summary>
    /// 记录指针信息 基类
    /// </summary>
    public abstract class TransactionDetailBase
    {
        /// <summary>
        /// 数据库容量
        /// </summary>
        public uint DataBaseMaxSize;
        /// <summary>
        /// 写索引号(记录尾号)
        /// </summary>
        public uint WriteIndex;
        /// <summary>
        /// 读索引号(上传断点)
        /// </summary>
        public uint ReadIndex;
        /// <summary>
        /// 循环标记
        /// </summary>
        public bool IsCircle;

        /// <summary>
        /// 可用的新记录数
        /// </summary>
        /// <returns>新记录数</returns>
        public long readable()
        {
            if (IsCircle)
            {
                return DataBaseMaxSize;
            }
            if (WriteIndex > DataBaseMaxSize)
            {
                WriteIndex = 0;
            }
            if (ReadIndex > DataBaseMaxSize)
            {
                ReadIndex = 0;
            }
            if (ReadIndex == WriteIndex)
            {
                return 0;
            }
            //记录尾号大于上传断点，那么表示新记录只有上传断点至记录尾号之间这段。
            if (WriteIndex > ReadIndex)
            {
                return (WriteIndex - ReadIndex);
            }
            //记录尾号小于上传断点，那么表示新记录有两段，一段是上传断点至记录末，一处是记录头至记录尾号
            if (WriteIndex < ReadIndex)
            {
                return WriteIndex + (DataBaseMaxSize - ReadIndex);
            }
            return 0;
        }

        /// <summary>
        /// 可用的新记录数
        /// </summary>
        /// <returns>新记录数</returns>
        public long NewSzie()
        {
            return readable();
        }

        /// <summary>
        /// 从字节缓冲区中生成一个对象
        /// </summary>
        /// <param name="data"></param>
        public abstract void SetBytes(IByteBuffer data);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IByteBuffer GetBytes()
        {
            return null;
        }
    }
}
