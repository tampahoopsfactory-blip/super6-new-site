using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup
{
    /// <summary>
    /// 表示一个时段，开始时间和结束时间
    /// </summary>
    public class TimeSegment
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        protected DateTime mBeginTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        protected DateTime mEndTime;


        /// <summary>
        /// 设置开始时间
        /// </summary>
        /// <param name="Hour">开始时间的小时部分 取值范围 0-23</param>
        /// <param name="Minute">开始时间的分钟部分 取值范围 0-59</param>
        public void SetBeginTime(int Hour, int Minute)
        {
            CheckTime(Hour, Minute);
            DateTime n = DateTime.Now;
            mBeginTime = new DateTime(n.Year, n.Month, n.Day, Hour, Minute, 0);
        }

        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetBeginTime()
        {
            return mBeginTime;
        }


        /// <summary>
        /// 设置结束时间
        /// </summary>
        /// <param name="Hour">开始时间的小时部分 取值范围 0-23</param>
        /// <param name="Minute">开始时间的分钟部分 取值范围 0-59</param>
        public void SetEndTime(int Hour, int Minute)
        {
            CheckTime(Hour, Minute);
            DateTime n = DateTime.Now;
            mEndTime = new DateTime(n.Year, n.Month, n.Day, Hour, Minute, 0);
        }

        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetEndTime()
        {
            return mEndTime;
        }

        /// <summary>
        /// 返回时间段 字符串格式 HH:mm - HH:mm
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            StringBuilder buf = new StringBuilder(15);
            buf.Append(mBeginTime.ToString("HH:mm"));
            buf.Append(" - ");
            buf.Append(mEndTime.ToString("HH:mm"));
            return buf.ToString();
        }

        /// <summary>
        /// 检查时分参数
        /// </summary>
        /// <param name="Hour">小时</param>
        /// <param name="Minute">分钟</param>
        private void CheckTime(int Hour, int Minute)
        {
            if (Hour < 0 || Hour > 23)
            {
                throw new ArgumentException("Hour -- 0-23");
            }

            if (Minute < 0 || Minute > 59)
            {
                throw new ArgumentException("Minute -- 0-59");
            }
        }

        /// <summary>
        /// 将对象写入到字节缓冲区
        /// </summary>
        /// <param name="bBuf"></param>
        public virtual void GetBytes(IByteBuffer bBuf)
        {

            bBuf.WriteByte(ByteUtil.ByteToBCD((byte)mBeginTime.Hour));
            bBuf.WriteByte(ByteUtil.ByteToBCD((byte)mBeginTime.Minute));
            bBuf.WriteByte(ByteUtil.ByteToBCD((byte)mEndTime.Hour));
            bBuf.WriteByte(ByteUtil.ByteToBCD((byte)mEndTime.Minute));
        }

        /// <summary>
        /// 从字节缓冲区中生成一个对象
        /// </summary>
        /// <param name="bBuf"></param>
        public virtual void SetBytes(IByteBuffer bBuf)
        {

            DateTime n = DateTime.Now;
            //mBeginTime = new DateTime(n.Year, n.Month, n.Day, ByteUtil.BCDToByte(bBuf.ReadByte()), ByteUtil.BCDToByte(bBuf.ReadByte()), 0);
            //mEndTime = new DateTime(n.Year, n.Month, n.Day, ByteUtil.BCDToByte(bBuf.ReadByte()), ByteUtil.BCDToByte(bBuf.ReadByte()), 0);
            byte b1 = ByteUtil.BCDToByte(bBuf.ReadByte());
            byte b2 = ByteUtil.BCDToByte(bBuf.ReadByte());
            byte b3 = ByteUtil.BCDToByte(bBuf.ReadByte());
            byte b4 = ByteUtil.BCDToByte(bBuf.ReadByte());
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
            mBeginTime = new DateTime(n.Year, n.Month, n.Day, (b1), (b2), 0);
            mEndTime = new DateTime(n.Year, n.Month, n.Day, (b3), (b4), 0);
        }

        
    }
}
