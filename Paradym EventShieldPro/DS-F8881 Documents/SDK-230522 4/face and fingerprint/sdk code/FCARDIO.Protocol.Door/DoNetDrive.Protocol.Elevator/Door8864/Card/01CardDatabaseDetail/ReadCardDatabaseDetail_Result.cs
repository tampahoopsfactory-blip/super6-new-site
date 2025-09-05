using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.CardDatabaseDetail
{
    /// <summary>
    /// 控制器中的卡片数据库信息
    /// </summary>
    public class ReadCardDatabaseDetail_Result : INCommandResult
    {
        /// <summary>
        /// 排序数据区容量上限
        /// </summary>
        public long SortDataBaseSize;

        /// <summary>
        /// 排序数据区已使用数量
        /// </summary>
        public long SortCardSize;


        /// <summary>
        /// 顺序存储区容量上限
        /// </summary>
        public long SequenceDataBaseSize;

        /// <summary>
        /// 顺序存储区已使用数量
        /// </summary>
        public long SequenceCardSize;

        /// <summary>
        /// 创建结构
        /// </summary>
        public ReadCardDatabaseDetail_Result()
        {
        }

        internal void SetBytes(IByteBuffer buf)
        {
            SortDataBaseSize = buf.ReadUnsignedInt();
            SortCardSize = buf.ReadUnsignedInt();
            SequenceDataBaseSize = buf.ReadUnsignedInt();
            SequenceCardSize = buf.ReadUnsignedInt();

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
