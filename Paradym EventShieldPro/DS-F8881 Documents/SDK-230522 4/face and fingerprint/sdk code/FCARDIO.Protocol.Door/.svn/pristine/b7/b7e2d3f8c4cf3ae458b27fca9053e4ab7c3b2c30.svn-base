using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.AdditionalCharges
{
    /// <summary>
    /// 设置附加费用命令
    /// </summary>
    public class WriteAdditionalCharges_Parameter : AbstractParameter
    {
        /// <summary>
        /// 附加费开关
        /// </summary>
        public byte Use;

        /// <summary>
        /// 时段免附加费消费次数
        /// </summary>
        public byte FreeTimes;

        /// <summary>
        /// 保留
        /// </summary>
        public byte Standby;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteAdditionalCharges_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Use">附加费开关</param>
        /// <param name="FreeTimes">时段免附加费消费次数</param>
        /// <param name="Standby">保留</param>
        public WriteAdditionalCharges_Parameter(byte Use, byte FreeTimes, byte Standby)
        {
            this.Use = Use;
            this.FreeTimes = FreeTimes;
            this.Standby = Standby;
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
            if (Use != 0 && Use != 1)
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
            databuf.WriteByte(Use);
            databuf.WriteByte(FreeTimes);
            databuf.WriteByte(Standby);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 3;
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
            Use = databuf.ReadByte();
            FreeTimes = databuf.ReadByte();
            Standby = databuf.ReadByte();
        }
    }
}
