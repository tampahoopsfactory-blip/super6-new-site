using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Door.Door8800.Holiday
{
    /// <summary>
    /// ReadHolidayDetail 指令的结果返回，保存控制器中存储的节假日详情
    /// </summary>
    public class ReadHolidayDetail_Result : INCommandResult
    {
        /// <summary>
        /// 控制器中存储的节假日详情
        /// </summary>
        public HolidayDBDetail Detail;

        /// <summary>
        /// 初始化，构造一个空的 HolidayDBDetail 详情实例
        /// </summary>
        public ReadHolidayDetail_Result()
        {
            Detail = new HolidayDBDetail();
        }

        /// <summary>
        /// 将字节缓冲区反序列化到实例
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            Detail.SetBytes(databuf);
        }

        /// <summary>
        /// 释放使用的资源
        /// </summary>
        public void Dispose()
        {
            Detail = null;
        }
    }
}
