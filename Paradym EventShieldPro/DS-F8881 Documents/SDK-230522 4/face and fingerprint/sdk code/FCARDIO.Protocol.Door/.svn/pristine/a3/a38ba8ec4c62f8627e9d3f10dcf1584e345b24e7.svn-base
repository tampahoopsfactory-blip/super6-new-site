using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.Deadline
{
    /// <summary>
    /// 设置设备有效期_参数
    /// </summary>
    public class WriteDeadline_Parameter : AbstractParameter
    {
        /// <summary>
        /// 设备有效期
        /// </summary>
        public DateTime Deadline;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteDeadline_Parameter() { }

        /// <summary>
        /// 使用设备有效期初始化实例
        /// </summary>
        /// <param name="_Deadline">设备有效期</param>
        public WriteDeadline_Parameter(DateTime _Deadline)
        {
            Deadline = _Deadline;
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

        }

        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < GetDataLen())
            {
                throw new ArgumentException("databuf len error");
            }
            //return databuf.WriteUnsignedShort(Deadline);
            return null;
        }

        public override int GetDataLen()
        {
            return 8;
        }

        public override void SetBytes(IByteBuffer databuf)
        {
            //if (databuf.ReadableBytes != GetDataLen())
            //{
            //    throw new ArgumentException("databuf Error");
            //}
            int year = databuf.ReadByte();
            Deadline = DateTime.Now;
            //DateTime dt = new DateTime(year, databuf.ReadByte(), databuf.ReadByte(), databuf.ReadByte(), databuf.ReadByte(), databuf.ReadByte());
            //Deadline = databuf.ReadUnsignedShort();
        }
    }
}
