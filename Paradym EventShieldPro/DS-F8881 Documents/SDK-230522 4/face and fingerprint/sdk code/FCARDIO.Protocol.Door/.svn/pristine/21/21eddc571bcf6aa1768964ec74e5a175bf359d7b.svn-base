using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Card;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.DeleteCard
{
    /// <summary>
    /// 将卡片列表从到控制器中删除
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
            MaxBufSize = (mPacketCardMax * 5) + 4;
            CmdType = 0x47;
            CheckResponseCmdType = 0x27;
        }

        /// <summary>
        /// 处理返回值 方便调试，可删除
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            base.CommandNext1(oPck);
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
