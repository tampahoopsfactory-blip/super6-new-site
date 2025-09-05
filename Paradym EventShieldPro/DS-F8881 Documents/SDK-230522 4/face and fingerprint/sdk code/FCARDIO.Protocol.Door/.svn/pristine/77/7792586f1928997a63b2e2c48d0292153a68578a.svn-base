using DoNetDrive.Protocol.POS.TemplateMethod;
using DoNetDrive.Protocol.Util;
using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.Data
{
    public class ReservationRuleDetail
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        public string ShowBeginTime
        {
            get
            {
                return BeginTime.ToString("HH:mm");
            }
           
        }

        public string ShowEndTime
        {
            get
            {
                return EndTime.ToString("HH:mm");
            }
        }

        /// <summary>
        /// 此时段订餐星期
        /// </summary>
        public int Weekday { get; set; }

        string[] WeekdayList = new string[] { "","星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };
        public string ShowWeekday
        {
            get
            {
                return WeekdayList[Weekday];
            }
        }
        /// <summary>
        /// 此时段订餐餐段
        /// </summary>
        public int MealTimeIndex { get; set; }

        public IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(SerialNumber);
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)BeginTime.Hour));
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)BeginTime.Minute));
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)EndTime.Hour));
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)EndTime.Minute));
            databuf.WriteByte(Weekday);
            databuf.WriteByte(MealTimeIndex);

            return databuf;
        }


        public void SetBytes(IByteBuffer databuf, int index)
        {
            databuf.ReadByte();
            SerialNumber = index;
            byte b1 = ByteUtil.BCDToByte(databuf.ReadByte());
            byte b2 = ByteUtil.BCDToByte(databuf.ReadByte());
            byte b3 = ByteUtil.BCDToByte(databuf.ReadByte());
            byte b4 = ByteUtil.BCDToByte(databuf.ReadByte());
            if (b1 > 59)
            {
                b1 = 0;
            }
            if (b2 > 59)
            {
                b2 = 0;
            }
            if (b3 > 59)
            {
                b3 = 0;
            }
            if (b4 > 59)
            {
                b4 = 0;
            }
            BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, b1, b2, 0);
            EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, b3, b4, 0);

            Weekday = databuf.ReadByte();
            MealTimeIndex = databuf.ReadByte();
        }

    }
}
