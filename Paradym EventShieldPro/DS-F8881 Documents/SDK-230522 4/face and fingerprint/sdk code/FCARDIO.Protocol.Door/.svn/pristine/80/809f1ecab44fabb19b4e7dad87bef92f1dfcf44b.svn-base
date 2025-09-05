using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.ConsumePassword
{
    /// <summary>
    /// 设置消费时密码确认命令参数
    /// </summary>
    public class WriteConsumePassword_Parameter : AbstractParameter
    {
        /// <summary>
        /// 功能开关
        /// </summary>
        public byte Use;

        /// <summary>
        /// 消费阀值
        /// </summary>
        public decimal LimitMoney;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteConsumePassword_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Use">功能开关</param>
        /// <param name="LimitMoney">消费阀值</param>
        public WriteConsumePassword_Parameter(byte Use, decimal LimitMoney)
        {
            this.Use = Use;
            this.LimitMoney = LimitMoney;
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
            if (LimitMoney < 0 || LimitMoney > 21474836)
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
            databuf.WriteInt(Convert.ToInt32(LimitMoney * 100));
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 5;
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
            LimitMoney = (decimal)databuf.ReadInt() / (decimal)100;
        }
    }
}
