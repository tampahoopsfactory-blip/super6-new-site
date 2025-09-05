using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.OutputFormat
{
    /// <summary>
    /// 设置输出格式参数
    /// </summary>
    public class WriteOutputFormat_Parameter : AbstractParameter
    {
        /// <summary>
        /// 输出协议格式
        /// 0 - 禁止输出
        /// 1 - WG26（三字节）+WG34（4字节需短路key1按键）
        /// 2 - WG26（三字节）+WG66（八字节需短路key1按键）
        /// </summary>
        public int Format;

        /// <summary>
        /// 卡号字节顺序
        /// 1 - 高位在前，低位在后
        /// 2 - 低位在前，高位在后
        /// </summary>
        public int Sort;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteOutputFormat_Parameter()
        {

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="type">输出协议格式</param>
        /// <param name="sort"></param>
        public WriteOutputFormat_Parameter(int format, int sort)
        {
            Format = format;
            Sort = sort;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Format > 2 || Format < 0)
                throw new ArgumentException("Type Error!");
            if (Sort < 1 || Sort > 2)
                throw new ArgumentException("Type Error!");
            return true;
        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Format);
            databuf.WriteByte(Sort);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 2;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Format = databuf.ReadByte();
            Sort = databuf.ReadByte();
        }
    }
}
