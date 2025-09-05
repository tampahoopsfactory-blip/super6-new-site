using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.CardDetail
{
    /// <summary>
    /// FC88/MC58  读取单个卡片在控制器中的信息
    /// </summary>
    public class ReadCardDetail_Parameter
         : AbstractParameter
    {
        /// <summary>
        /// 要读取详情的授权卡卡号
        /// 取值：1-0xFFFFFFFF
        /// </summary>
        public UInt64 CardData;

        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="carddata">需要读取卡片详情的卡号</param>
        public ReadCardDetail_Parameter(UInt64 carddata)
        {
            CardData = carddata;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if(CardData==0)
            {
                throw new ArgumentException("CardData Error!");
            }
            if (CardData > (UInt64)UInt32.MaxValue)
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
            if (databuf.WritableBytes < 5)
            {
                throw new ArgumentException("Crad Error");
            }
            databuf.WriteByte(0);
            databuf.WriteInt((int)CardData);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 5;
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
