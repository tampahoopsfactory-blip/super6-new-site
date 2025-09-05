using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.POS.Subsidy
{
    /// <summary>
    /// 读取补贴容量信息结果
    /// </summary>
    public class ReadSubsidyDataBase_Result : INCommandResult
    {
        /// <summary>
        /// 补贴名单容量
        /// </summary>
        public short SortSize;

        /// <summary>
        /// 已存数量
        /// </summary>
        public short UseSize;

        /// <summary>
        /// 初始化，构造一个空的 HolidayDBDetail 详情实例
        /// </summary>
        public ReadSubsidyDataBase_Result()
        {

        }

        /// <summary>
        /// 将字节缓冲区反序列化到实例
        /// </summary>
        /// <param name="buf"></param>
        public void SetBytes(IByteBuffer buf)
        {
            SortSize = buf.ReadShort();
            UseSize = buf.ReadShort();
        }

        /// <summary>
        /// 释放使用的资源
        /// </summary>
        public void Dispose()
        {
            return;
        }
    }
}
