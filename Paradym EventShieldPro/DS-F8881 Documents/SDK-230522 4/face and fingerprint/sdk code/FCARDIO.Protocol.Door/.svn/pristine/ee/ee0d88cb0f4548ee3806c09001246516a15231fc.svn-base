using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.ReaderByte
{
    /// <summary>
    /// 设置读卡器字节命令参数
    /// </summary>
    public class WriteReaderByte_Parameter : AbstractParameter
    {
        /// <summary>
        /// 卡号字节长度
        /// 2、3、4 三种长度
        /// </summary>
        public byte ByteLength;

        /// <summary>
        /// 卡号取值规则
        /// 1 - 低位在前 
        /// 2 - 高位在前
        /// </summary>
        public byte Rule;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteReaderByte_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="ByteLength">卡号字节长度</param>
        /// <param name="Rule">卡号取值规则</param>
        public WriteReaderByte_Parameter(byte ByteLength, byte Rule)
        {
            this.ByteLength = ByteLength;
            this.Rule = Rule;
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
            if (ByteLength < 2 || ByteLength > 4)
            {
                return false;
            }
            if (Rule < 1 || Rule > 2)
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
            databuf.WriteByte(ByteLength);
            databuf.WriteByte(Rule);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 2;
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
            ByteLength = databuf.ReadByte();
            Rule = databuf.ReadByte();
        }
    }
}
