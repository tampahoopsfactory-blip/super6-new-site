using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.POSWorkMode
{
    /// <summary>
    /// 设置消费机工作模式命令参数
    /// </summary>
    public class WritePOSWorkMode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 消费模式
        /// 1 - 标准收费
        /// 2 - 定额收费
        /// 3 - 菜单收费
        /// 4 - 订餐机
        /// 5 - 补贴机
        /// 6 - 子账收费
        /// 7 - 子账补贴
        /// </summary>
        public byte Mode;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WritePOSWorkMode_Parameter() { }

        /// <summary>
        /// 使用消费模式初始化实例
        /// </summary>
        /// <param name="Mode">消费模式</param>
        public WritePOSWorkMode_Parameter(byte Mode)
        {
            this.Mode = Mode;
            if (!checkedParameter())
            {
                throw new ArgumentException("Mode Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Mode > 7 || Mode < 1)
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

            return databuf.WriteByte(Mode);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x01;
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
            Mode = databuf.ReadByte();
        }
    }
}
