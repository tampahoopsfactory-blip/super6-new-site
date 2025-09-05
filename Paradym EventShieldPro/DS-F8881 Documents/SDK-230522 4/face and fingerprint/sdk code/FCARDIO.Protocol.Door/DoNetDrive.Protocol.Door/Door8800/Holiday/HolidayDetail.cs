using DotNetty.Buffers;
using DoNetDrive.Core.Data;
using DoNetDrive.Protocol.Util;
using System;

namespace DoNetDrive.Protocol.Door.Door8800.Holiday
{
    /// <summary>
    /// 表示一个节假日
    /// </summary>
    public class HolidayDetail : AbstractData
    {
        /// <summary>
        /// 节假日的索引号
        /// </summary>
        public byte Index;

        /// <summary>
        /// 节假日日期
        /// </summary>
        public DateTime Holiday;

        /// <summary>
        /// 节假日类型：<br/>
        /// 1、上午 (00:00:00  -  11:59:59)<br/>
        /// 2、下午 (12:00:00  -  23:59:59)<br/>
        /// 3、全天 (00:00:00  -  23:59:59)
        /// </summary>
        public byte HolidayType;

        /// <summary>
        /// 表示，是否每年循环
        /// </summary>
        public bool YearLoop;


        /// <summary>
        /// 将实例序列化到字节缓冲区中
        /// </summary>
        /// <param name="databuf">字节缓冲区，需要预先分配好空间</param>
        /// <returns>保存有实例数据的缓冲区</returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Index);
            if(YearLoop)
            {
                databuf.WriteByte(0);
            }
            else
            {
                databuf.WriteByte(ByteUtil.ByteToBCD((byte)(Holiday.Year - 2000)));
            }
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)(Holiday.Month)));
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)(Holiday.Day)));
            databuf.WriteByte(HolidayType);
            //databuf.WriteByte(Year);

            return databuf;
        }


        /// <summary>
        /// 将实例序列化到字节缓冲区中的长度
        /// </summary>
        /// <returns>字节缓冲区中的长度</returns>
        public override int GetDataLen()
        {
            return 5;
        }

        /// <summary>
        /// 将字节缓冲区反序列化到实例
        /// </summary>
        /// <param name="databuf">字节缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Index = databuf.ReadByte();
            int iYear, iMonth, iDay;
            iYear = databuf.ReadByte();
            if(iYear ==0)
            {
                YearLoop = true;
                iYear = DateTime.Now.Year;
            }
            else
            {
                YearLoop = false;
                iYear = 2000 + ByteUtil.BCDToByte((byte)iYear);
            }
            
            iMonth = ByteUtil.BCDToByte(databuf.ReadByte());
            iDay = ByteUtil.BCDToByte(databuf.ReadByte());
            Holiday = new DateTime(iYear, iMonth, iDay);
            HolidayType = databuf.ReadByte();
            //Year = databuf.ReadByte();
        }
    }
}
