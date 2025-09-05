using DoNetDrive.Protocol.Util;
using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.Time
{
    /// <summary>
    /// 设置控制器的日期时间
    /// </summary>
    public class WriteCustomTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 设备有效期
        /// </summary>
        public DateTime ControllerDate;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteCustomTime_Parameter() { }

        /// <summary>
        /// 使用设备有效期初始化实例
        /// </summary>
        /// <param name="ControllerDate">电脑时间</param>
        public WriteCustomTime_Parameter(DateTime ControllerDate)
        {
            this.ControllerDate = ControllerDate;
            if (!checkedParameter())
            {
                throw new ArgumentException("Deadline Error");
            }
        }

        public override bool checkedParameter()
        {
            //if (Deadline < 0 || Deadline > 65535)
            //{
            //    throw new ArgumentException("Deadline Error");
            //}

            return true;
        }

        public override void Dispose()
        {

        }/// <summary>
         /// 对控制器的日期时间参数进行编码
         /// </summary>
         /// <param name="databuf"></param>
         /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            byte[] Datebuf = new byte[7];
            DateToBCD_ssmmhhddMMwwyy(Datebuf, ControllerDate);
            databuf.WriteBytes(Datebuf);
            return databuf;
        }

        /// <summary>
        /// 日期类型转换为BCD格式日期字节数组
        /// </summary>
        /// <param name="btData"></param>
        /// <param name="date"></param>
        public static void DateToBCD_ssmmhhddMMwwyy(byte[] btData, DateTime date)
        {
            if (date == null)
            {
                for (int i = 0; i < 7; i++)
                {
                    btData[i] = 0;
                }
            }
            else
            {
                btData[0] = (byte)(date.Year - 2000);
                
                btData[1] = (byte)date.Month;
                btData[2] = (byte)date.Day;
                btData[3] = (byte)date.Hour;
                btData[4] = (byte)date.Minute;
                btData[5] = (byte)date.Second;
                btData[6] = (byte)GetWeekNum();
                btData = ByteUtil.ByteToBCD(btData);
            }
        }

        /// <summary>
        /// 定义星期代表数值（星期表示：1表示星期一；2表示星期二。。。。6表示星期六；7表示星期日；）
        /// </summary>
        /// <returns></returns>
        public static int GetWeekNum()
        {
            int weekNum = 1;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    weekNum = 1;
                    break;
                case DayOfWeek.Tuesday:
                    weekNum = 2;
                    break;
                case DayOfWeek.Wednesday:
                    weekNum = 3;
                    break;
                case DayOfWeek.Thursday:
                    weekNum = 4;
                    break;
                case DayOfWeek.Friday:
                    weekNum = 5;
                    break;
                case DayOfWeek.Saturday:
                    weekNum = 6;
                    break;
                case DayOfWeek.Sunday:
                    weekNum = 7;
                    break;
            }
            return weekNum;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x07;
        }

        /// <summary>
        /// 对控制器的日期时间参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            //控制器的日期时间
            ControllerDate = BCDTimeToDate_yyMMddhhmmss(databuf);
        }

        /// <summary>
        /// BCD格式日期字节数组转换为日期类型
        /// </summary>
        /// <param name="btTime"></param>
        /// <returns></returns>
        public static DateTime BCDTimeToDate_yyMMddhhmmss(IByteBuffer buf)
        {
            buf = ByteUtil.BCDToByte(buf, buf.ReaderIndex, 7);

            int year  = buf.ReadByte();
            int month  = buf.ReadByte();
            int day  = buf.ReadByte();
            int hour = buf.ReadByte();
            int minute = buf.ReadByte();
          
            int sec = buf.ReadByte();
            int week = buf.ReadByte();
            if (year > 99)
            {
                return DateTime.Now;
            }
            if (month == 0 || month > 12)
            {
                return DateTime.Now;
            }
            if (day == 0 || day > 31)
            {
                return DateTime.Now;
            }
            if (hour > 23)
            {
                return DateTime.Now;
            }

            if (minute > 59)
            {
                return DateTime.Now;
            }
            if (sec > 59)
            {
                return DateTime.Now;
            }

            DateTime dTime = new DateTime(2000 + year, month, day, hour, minute, sec);
            return dTime;
        }
    }
}

