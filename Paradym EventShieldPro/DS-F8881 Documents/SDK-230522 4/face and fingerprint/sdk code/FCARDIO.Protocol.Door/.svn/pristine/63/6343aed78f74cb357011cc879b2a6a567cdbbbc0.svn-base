using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Reservation
{
    /// <summary>
    /// 控制器中的订餐数据库信息
    /// </summary>
    public class ReadDatabaseDetail_Result : INCommandResult
    {
        /// <summary>
        /// 最大容量
        /// </summary>
        public ushort SortSize;

        /// <summary>
        /// 最大容量
        /// </summary>
        public ushort UseSize;


        /// <summary>
        /// 创建结构
        /// </summary>
        public ReadDatabaseDetail_Result()
        {
        }

        public void SetBytes(IByteBuffer buf)
        {
            SortSize = buf.ReadUnsignedShort();
            UseSize = buf.ReadUnsignedShort();

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            return;
        }
    }
}
