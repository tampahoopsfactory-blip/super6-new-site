using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Holiday
{
    /// <summary>
    /// ReadAllHoliday 命令的返回值<br/>
    /// 保存已读取到的所有节假日的集合
    /// </summary>
    public class ReadAllHoliday_Result: INCommandResult
    {
        /// <summary>
        /// 已读取到的节假日数量
        /// </summary>
        public int Count;

        /// <summary>
        /// 已读取到的节假日列表
        /// </summary>
        public readonly List<HolidayDetail> Holidays;

        /// <summary>
        /// 初始化，构造一个空的 HolidayDBDetail 详情实例
        /// </summary>
        public ReadAllHoliday_Result()
        {
            Holidays = new List<HolidayDetail>();
        }

        /// <summary>
        /// 将字节缓冲区反序列化到实例
        /// </summary>
        /// <param name="iTotal">预计从缓冲区中解码出的最大数量</param>
        /// <param name="databufs">字节缓冲区列表</param>
        public void SetBytes(int iTotal, List<IByteBuffer> databufs)
        {
            Holidays.Clear();
            Holidays.Capacity = iTotal + 10;
            foreach (IByteBuffer buf in databufs)
            {
                int iCount = buf.ReadInt();
                for (int i = 0; i < iCount; i++)
                {
                    HolidayDetail dtl = new HolidayDetail();
                    dtl.SetBytes(buf);
                    Holidays.Add(dtl);
                }
            }
            Count = Holidays.Count;
        }

        /// <summary>
        /// 释放使用的资源
        /// </summary>
        public void Dispose()
        {
            Holidays.Clear();
        }
    }
}
