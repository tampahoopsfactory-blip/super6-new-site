using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Card
{
    /// <summary>
    /// Door88\Door58 将卡片列表从到控制器中删除
    /// </summary>
    public class DeleteCard : DeleteCardBase<Data.CardDetail>
    {


        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public DeleteCard(INCommandDetail cd, DeleteCard_Parameter parameter) : base(cd, parameter)
        {
            mPacketCardMax = 20;
            MaxBufSize = (mPacketCardMax * 4) + 4;
        }



        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="card">卡详情</param>
        /// <param name="buf">缓冲区</param>
        protected override void WriteCardBodyToBuf0(Data.CardDetail card, IByteBuffer buf)
        {
            buf.WriteByte(0);
            buf.WriteInt((int)card.CardData);
        }


    }
}
