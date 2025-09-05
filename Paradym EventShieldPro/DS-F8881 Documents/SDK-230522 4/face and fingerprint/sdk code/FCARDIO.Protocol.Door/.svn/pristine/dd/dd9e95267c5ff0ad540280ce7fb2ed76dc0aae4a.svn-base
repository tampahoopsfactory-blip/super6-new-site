using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;

namespace DoNetDrive.Protocol.USB.CardReader.Watch
{
    /// <summary>
    /// 读卡消息
    /// </summary>
    public class WatchReadCardTransaction : Transaction.AbstractTransaction
    {
        /// <summary>
        /// 卡类 <para/>
        /// 1--MF1 IC卡 S50 <para/>
        /// 2--NFC标签卡 <para/>
        /// 3--NFC手机 <para/>
        /// 4--身份证 <para/>
        /// 5--CPU IC卡 S50 <para/>
        /// 6--CPU卡 <para/>
        /// 7--MF1 IC卡 S70 <para/>
        /// 8--CPU IC卡 S70 <para/>
        /// </summary>
        public int CardType;

        /// <summary>
        /// 卡字节长度
        /// </summary>
        public int CardDataLen;


        /// <summary>
        /// 卡号
        /// </summary>
        public UInt64 Card;

        /// <summary>
        /// 创建一个读卡消息
        /// </summary>
        public WatchReadCardTransaction()
        {
            _TransactionType = 1;
            _TransactionCode = 1;
            _IsNull = false;
        }



        public override int GetDataLen()
        {
            throw new NotImplementedException();
        }

        public override void SetBytes(IByteBuffer databuf)
        {
            _TransactionDate = DateTime.Now;
            CardType = databuf.ReadByte();
            CardDataLen = databuf.ReadByte();

            switch (CardDataLen)
            {
                case 1:
                    Card = databuf.ReadByte();
                    break;
                case 2:
                    Card = databuf.ReadUnsignedShort();
                    break;
                case 3:
                    Card = (UInt64)databuf.ReadUnsignedMedium();
                    break;
                case 4:
                    Card = databuf.ReadUnsignedInt();
                    break;
                case 8:
                    Card = (UInt64)databuf.ReadLong();
                    break;
                default:
                    break;
            }
        }
    }
}
