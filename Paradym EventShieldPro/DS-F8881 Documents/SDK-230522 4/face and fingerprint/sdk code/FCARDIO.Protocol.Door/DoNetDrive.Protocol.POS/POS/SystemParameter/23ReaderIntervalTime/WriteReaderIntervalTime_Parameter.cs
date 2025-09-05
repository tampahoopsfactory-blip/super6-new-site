using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.ReaderIntervalTime
{
    /// <summary>
    /// 设置读卡间隔命令参数
    /// </summary>
    public class WriteReaderIntervalTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 间隔方式
        /// 0--不启用
        /// 1--使用ic卡中的最近消费日期做间隔判断
        /// 2--本机读卡间隔
        /// </summary>
        public byte Way;

        /// <summary>
        /// 读卡间隔时间，1-65535秒，
        /// </summary>
        public ushort IntervalTime;

        /// <summary>
        /// 间隔卡数
        /// </summary>
        public byte CardCount;


        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteReaderIntervalTime_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Way">间隔方式</param>
        /// <param name="IntervalTime">读卡间隔时间</param>
        /// <param name="CardCount">间隔卡数</param>
        public WriteReaderIntervalTime_Parameter(byte Way, ushort IntervalTime, byte CardCount)
        {
            this.Way = Way;
            this.IntervalTime = IntervalTime;
            this.CardCount = CardCount;
            if (!checkedParameter())
            {
                throw new ArgumentException("Parameter Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Way != 0 && Way != 1 && Way != 2)
            {
                return false;
            }
            if (Way == 2)
            {
                if (IntervalTime == 0)
                {
                    return false;
                }
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
        /// 对有效期参数进行编码
        /// </summary>
        /// <param name="databuf">需要填充参数结构的字节缓冲区</param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf len error");
            }
            databuf.WriteByte(Way);
            databuf.WriteUnsignedShort(IntervalTime);
            databuf.WriteByte(CardCount);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 4;
        }

        /// <summary>
        /// 对有效期参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            Way = databuf.ReadByte();
            IntervalTime = databuf.ReadUnsignedShort();
            CardCount = databuf.ReadByte();
        }
    }
}
