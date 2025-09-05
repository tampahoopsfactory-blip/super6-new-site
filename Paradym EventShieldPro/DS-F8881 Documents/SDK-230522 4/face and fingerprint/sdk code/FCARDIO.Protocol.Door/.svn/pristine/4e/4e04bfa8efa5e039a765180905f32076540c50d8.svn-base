using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.CardType
{
    /// <summary>
    /// 读取单个菜单命令
    /// </summary>
    public class ReadCardTypeDetail_Parameter : AbstractParameter
    {
        public int CardType;

        public ReadCardTypeDetail_Parameter(int CardType)
        {
            this.CardType = CardType;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (CardType < 0)
            {
                throw new ArgumentException("CardType Error!");
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
        /// 将结构编码为 字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != 8)
            {
                throw new ArgumentException("CardType Error");
            }
            databuf.WriteByte(CardType);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 8;
        }

        /// <summary>
        /// 未实现
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }
    }
}
