using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.Card
{
    /// <summary>
    /// 读取单个名单命令
    /// </summary>
    public class ReadCardDetail_Parameter : AbstractParameter
    {
        public int CardData;

        public ReadCardDetail_Parameter(int CardData)
        {
            this.CardData = CardData;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (CardData <= 0)
            {
                throw new ArgumentException("CardData Error!");
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
            if (databuf.WritableBytes != 4)
            {
                throw new ArgumentException("Card Error");
            }
            databuf.WriteInt(CardData);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 4;
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
