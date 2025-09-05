using DotNetty.Buffers;
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
    public class TimeSegment_ReaderWork : TimeSegment
    {
        /// <summary>
        /// 认证方式
        /// </summary>
        protected byte CheckWay;

        /// <summary>
        /// 获取认证方式
        /// </summary>
        /// <returns></returns>
        public byte GetCheckWay()
        {
            return CheckWay;
        }

        /// <summary>
        /// 设置认证方式
        /// </summary>
        public void SetCheckWay(byte v)
        {
            CheckWay=v;
        }

        /// <summary>
        /// 将对象写入到字节缓冲区
        /// </summary>
        /// <param name="bBuf"></param>
        public override void GetBytes(IByteBuffer bBuf)
        {
            bBuf.WriteByte(ByteUtil.ByteToBCD((byte)mBeginTime.Hour));
            bBuf.WriteByte(ByteUtil.ByteToBCD((byte)mBeginTime.Minute));
            bBuf.WriteByte(ByteUtil.ByteToBCD((byte)mEndTime.Hour));
            bBuf.WriteByte(ByteUtil.ByteToBCD((byte)mEndTime.Minute));
            bBuf.WriteByte(CheckWay);
        }

        /// <summary>
        /// 从字节缓冲区中生成一个对象
        /// </summary>
        /// <param name="bBuf"></param>
        public override void SetBytes(IByteBuffer bBuf)
        {
            DateTime n = DateTime.Now;
            mBeginTime = new DateTime(n.Year, n.Month, n.Day, ByteUtil.BCDToByte(bBuf.ReadByte()), ByteUtil.BCDToByte(bBuf.ReadByte()), 0);
            mEndTime = new DateTime(n.Year, n.Month, n.Day, ByteUtil.BCDToByte(bBuf.ReadByte()), ByteUtil.BCDToByte(bBuf.ReadByte()), 0);
            CheckWay = bBuf.ReadByte();
        }
    }
}
