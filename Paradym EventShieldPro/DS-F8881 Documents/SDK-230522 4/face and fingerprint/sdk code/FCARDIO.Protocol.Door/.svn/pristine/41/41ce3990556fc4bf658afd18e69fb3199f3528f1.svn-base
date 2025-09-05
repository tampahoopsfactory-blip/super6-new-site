using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Card
{
    /// <summary>
    /// Door89H 读取单个卡片在控制器中的信息
    /// </summary>
    public class ReadCardDetail_Parameter : Door8800.Card.ReadCardDetail_Parameter
    {
        /// <summary>
        /// 9字节卡号
        /// </summary>
        public Core.Util.BigInt BigCard;

        /// <summary>
        /// 要读取详情的授权卡8字节卡号
        /// 取值：1-0xFFFFFFFF
        /// </summary>
        public override uint CardData { get => BigCard.UInt32Value; set => BigCard.BigValue = value; }

        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="bCard">需要读取卡片详情的卡号</param>
        public ReadCardDetail_Parameter(decimal bCard) : base(0)
        {
            BigCard.BigValue = bCard;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (CardData == 0)
            {
                throw new ArgumentException("CardData Error!");
            }
            return true;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 9;
        }

        /// <summary>
        /// 将结构编码为 字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < 9)
            {
                throw new ArgumentException("Crad Error");
            }
            BigCard.toBytes(databuf, 9);
            return databuf;
        }
    }
}
