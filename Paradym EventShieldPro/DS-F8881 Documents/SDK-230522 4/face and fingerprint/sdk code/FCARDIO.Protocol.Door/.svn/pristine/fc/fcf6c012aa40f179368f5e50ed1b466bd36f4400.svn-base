using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.POS.Card
{
    public class ReadCardDataBase_Result : INCommandResult
    {
        /// <summary>
        /// 名单容量
        /// </summary>
        public ushort SortSize;

        /// <summary>
        /// 已存数量
        /// </summary>
        public ushort UseSize;


        /// <summary>
        /// 创建结构
        /// </summary>
        public ReadCardDataBase_Result()
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
