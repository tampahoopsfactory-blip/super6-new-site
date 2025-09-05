using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Card
{
    /// <summary>
    ///  Door89H 将卡片列表从到控制器中删除
    /// </summary>
    public class DeleteCard : Door8800.Card.DeleteCardBase<Data.CardDetail>
    {


        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public DeleteCard(INCommandDetail cd, DeleteCard_Parameter parameter) : base(cd, parameter)
        {
            mPacketCardMax = 20;
            MaxBufSize = (mPacketCardMax * 9) + 4;
        }



        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="card">卡详情</param>
        /// <param name="buf">缓冲区</param>
        protected override void WriteCardBodyToBuf0(Data.CardDetail card, IByteBuffer buf)
        {
            card.BigCard.toBytes(buf, 9);
        }


    }
}
