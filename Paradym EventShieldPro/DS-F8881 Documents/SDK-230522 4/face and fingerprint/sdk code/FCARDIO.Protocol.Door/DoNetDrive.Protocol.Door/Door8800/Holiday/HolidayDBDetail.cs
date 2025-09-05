using DotNetty.Buffers;
using DoNetDrive.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Holiday
{
    /// <summary>
    /// 控制器中存储的节假日详情
    /// </summary>
    public class HolidayDBDetail 
    {
        /// <summary>
        /// 控制器中可以存储的最大容量
        /// </summary>
        public int Capacity;

        /// <summary>
        /// 控制器中已存储的数量
        /// </summary>
        public int Count;

        /// <summary>
        /// 将字节缓冲区反序列化到实例
        /// </summary>
        /// <param name="databuf">字节缓冲区</param>
        public void SetBytes(IByteBuffer databuf)
        {
            Capacity = databuf.ReadUnsignedShort();
            Count = databuf.ReadUnsignedShort();
        }
    }
}
