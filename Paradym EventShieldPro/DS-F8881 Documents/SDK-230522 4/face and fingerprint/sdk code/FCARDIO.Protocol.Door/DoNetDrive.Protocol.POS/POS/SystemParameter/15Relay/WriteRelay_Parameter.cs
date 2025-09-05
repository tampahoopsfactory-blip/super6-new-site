using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.Relay
{
    /// <summary>
    /// 设置继电器命令参数
    /// </summary>
    public class WriteRelay_Parameter : AbstractParameter
    {
        /// <summary>
        /// 功能启用
        /// </summary>
        public byte Use;
        /// <summary>
        /// 继电器类型
        /// 1 - 不输出（默认）  COM 和 NC
        /// 2 - 输出            COM 和 NO
        /// 3 - 读卡切换输出状态（当读到合法卡后自动自动切换到当前相反的状态。）例如卷闸门
        /// </summary>
        public byte Mode;

        /// <summary>
        /// 输出保持
        /// </summary>
        public ushort OutputRetention;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteRelay_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Use">功能启用</param>
        /// <param name="Relay">继电器类型</param>
        /// <param name="OutputRetention">输出保持</param>
        public WriteRelay_Parameter(byte Use,byte Mode, ushort OutputRetention)
        {
            this.Use = Use;
            this.Mode = Mode;
            this.OutputRetention = OutputRetention;
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
            if (Mode < 1 || Mode > 3)
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
            databuf.WriteByte(Mode);
            databuf.WriteUnsignedShort(OutputRetention);
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
            Use = databuf.ReadByte();
            Mode = databuf.ReadByte();
            OutputRetention = databuf.ReadUnsignedShort();
        }
    }
}
