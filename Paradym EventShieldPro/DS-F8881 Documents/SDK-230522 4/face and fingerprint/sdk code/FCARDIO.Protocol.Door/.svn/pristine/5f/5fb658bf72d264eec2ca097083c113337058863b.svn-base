using DotNetty.Buffers;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoNetDrive.Protocol.Door.Door8800.Data
{
    /// <summary>
    /// Door88A、Door58 卡片权限详情
    /// </summary>
    public class CardDetail : CardDetailBase
    {

        /// <summary>
        /// 4字节卡号
        /// </summary>
        public override uint CardData { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public CardDetail() { }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="sur"></param>
        public CardDetail(CardDetailBase sur) : base(sur) { }

        /// <summary>
        /// 获取一个卡详情实例，序列化到buf中的字节占比
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x21;//33字节
        }

        /// <summary>
        /// 将卡号序列化并写入buf中
        /// </summary>
        /// <param name="data"></param>
        public override void WriteCardData(IByteBuffer data)
        {
            data.WriteByte(0);
            data.WriteInt((int)CardData);
        }

        /// <summary>
        /// 从buf中读取卡号
        /// </summary>
        /// <param name="data"></param>
        public override void ReadCardData(IByteBuffer data)
        {
            data.ReadByte();

            CardData = data.ReadUnsignedInt();
        }

    }
}
