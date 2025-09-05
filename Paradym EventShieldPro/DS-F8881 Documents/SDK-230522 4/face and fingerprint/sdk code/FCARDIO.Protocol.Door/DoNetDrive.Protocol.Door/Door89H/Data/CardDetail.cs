using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Core.Util;

namespace DoNetDrive.Protocol.Door.Door89H.Data
{
    /// <summary>
    /// Door89H 适用于此型号的卡详情
    /// </summary>
    public class CardDetail : CardDetailBase
    {
        /// <summary>
        /// 9字节大卡号
        /// </summary>
        public BigInt BigCard;

        /// <summary>
        /// 4字节卡号
        /// </summary>
        public override uint CardData { get => BigCard.UInt32Value; set => BigCard.BigValue = value; }

        /// <summary>
        /// 初始化
        /// </summary>
        public CardDetail() { BigCard = new BigInt(); }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="sur">授权卡信息 抽象类</param>
        public CardDetail(CardDetailBase sur) : base(sur) { }

        /// <summary>
        /// 获取一个卡详情实例，序列化到buf中的字节占比
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x25;//37字节
        }

        /// <summary>
        /// 将卡号序列化并写入buf中
        /// </summary>
        /// <param name="data"></param>
        public override void WriteCardData(IByteBuffer data)
        {
            BigCard.toBytes(data, 9);
        }

        /// <summary>
        /// 从buf中读取卡号
        /// </summary>
        /// <param name="data"></param>
        public override void ReadCardData(IByteBuffer data)
        {
            BigCard.SetBytes(data, 9);
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override int CompareTo(CardDetailBase o)
        {
            CardDetail other = o as CardDetail;
            if (other == null) return 1;
            return BigCard.BigValue.CompareTo(other.BigCard.BigValue);
        }
    }
}
