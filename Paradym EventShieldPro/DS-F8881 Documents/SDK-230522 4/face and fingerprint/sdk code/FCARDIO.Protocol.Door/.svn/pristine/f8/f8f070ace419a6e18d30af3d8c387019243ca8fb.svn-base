using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.Integral
{
    /// <summary>
    /// 设置积分参数
    /// </summary>
    public class WriteIntegral_Parameter : AbstractParameter
    {
        /// <summary>
        /// 积分功能开关
        /// </summary>
        public byte Use;

        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal Money;

        /// <summary>
        /// 积分值
        /// </summary>
        public int Integral;

        /// <summary>
        /// 单次消费最高积分
        /// </summary>
        public int MaxIntegral;

        /// <summary>
        /// 单次最大累计次数
        /// </summary>
        public int MaxCount;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteIntegral_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="UseICCardDiscount">IC卡折扣开关</param>
        /// <param name="UseCardTypeDiscount">卡类折扣开关</param>
        /// <param name="UsePOSDiscount">机器折扣开关</param>
        /// <param name="POSDiscount">机器折扣</param>
        /// <param name="DoubleDiscount">折上折开关</param>
        public WriteIntegral_Parameter(byte Use, int Money, int Integral, int MaxIntegral, int MaxCount)
        {
            this.Use = Use;
            this.Money = Money;
            this.Integral = Integral;
            this.MaxIntegral = MaxIntegral;
            this.MaxCount = MaxCount;
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
            if (Money < 0 || Money > 21474836)
            {
                return false;
            }
            if (Integral < 0)
            {
                return false;
            }
            if (MaxIntegral < 0)
            {
                return false;
            }
            if (MaxCount < 0)
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
            databuf.WriteInt(Convert.ToInt32(Money * 100));
            databuf.WriteInt(Integral);
            databuf.WriteInt(MaxIntegral);
            databuf.WriteInt(MaxCount);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 17;
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
            Money = (decimal)databuf.ReadInt() / (decimal)100;
            Integral = databuf.ReadInt();
            MaxIntegral = databuf.ReadInt();
            MaxCount = databuf.ReadInt();
        }
    }
}
