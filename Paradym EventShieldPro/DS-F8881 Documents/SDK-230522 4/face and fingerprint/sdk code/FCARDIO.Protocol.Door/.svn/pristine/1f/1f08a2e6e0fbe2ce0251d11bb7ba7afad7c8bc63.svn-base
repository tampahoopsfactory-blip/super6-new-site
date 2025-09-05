using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.Initialize
{
    /// <summary>
    /// 初始化命令参数
    /// </summary>
    public class Initialize_Parameter : AbstractParameter
    {
        /// <summary>
        /// 初始化模式
        /// 1 --表示标准初始化
        /// 0x80 -- 表示全部初始化
        /// </summary>
        public byte Mode;


        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public Initialize_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Mode">初始化模式</param>
        public Initialize_Parameter(byte Mode)
        {
            this.Mode = Mode;
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
            if (Mode != 1 && Mode != 0x80)
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
            databuf.WriteByte(Mode);
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
            
        }
    }
}
