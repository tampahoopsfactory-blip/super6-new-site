using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.ForbiddenMifareOne
{
    /// <summary>
    /// 设置禁用Mifare One 卡命令参数
    /// </summary>
    public class WriteForbiddenMifareOne_Parameter : AbstractParameter
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public byte Use;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteForbiddenMifareOne_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Use">是否启用</param>
        public WriteForbiddenMifareOne_Parameter(byte Use)
        {
            this.Use = Use;
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
            if (Use != 1 && Use != 0)
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
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
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
        }
    }
}
