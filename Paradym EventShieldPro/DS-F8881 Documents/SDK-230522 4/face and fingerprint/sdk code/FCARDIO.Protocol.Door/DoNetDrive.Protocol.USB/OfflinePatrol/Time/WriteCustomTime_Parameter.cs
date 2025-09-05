using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Time
{
    /// <summary>
    /// 设置设备的日期时间
    /// </summary>
    public class WriteCustomTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 控制器的日期时间
        /// </summary>
        public DateTime ControllerDate;

        /// <summary>
        /// 提供给 ReadTime_Result 使用
        /// </summary>
        public WriteCustomTime_Parameter()
        {

        }
        /// <summary>
        /// 控制器的日期时间参数初始化实例
        /// </summary>
        /// <param name="_ControllerDate">控制器的日期时间参数</param>
        public WriteCustomTime_Parameter(DateTime _ControllerDate)
        {
            ControllerDate = _ControllerDate;
            if (!checkedParameter())
            {
                throw new ArgumentException("ControllerDate Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ControllerDate < new DateTime(2000,1,1) || ControllerDate > new DateTime(2099,12,31))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 对控制器的日期时间参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            byte[] Datebuf = new byte[6];
            TimeUtil.DateToBCD_ssmmhhddMMyy(Datebuf, ControllerDate);
            databuf.WriteBytes(Datebuf);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x06;
        }

        /// <summary>
        /// 对控制器的日期时间参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            //控制器的日期时间
            ControllerDate = BCDTimeToDate_ssmmHHddMMyy(databuf);
        }

        public DateTime BCDTimeToDate_ssmmHHddMMyy(IByteBuffer buf)
        {
            buf = ByteUtil.BCDToByte(buf, buf.ReaderIndex, 6);

            int sec = buf.ReadByte();
            int minute = buf.ReadByte();
            int hour = buf.ReadByte();
            int day = buf.ReadByte();
            int month = buf.ReadByte();
            int year = buf.ReadByte();

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