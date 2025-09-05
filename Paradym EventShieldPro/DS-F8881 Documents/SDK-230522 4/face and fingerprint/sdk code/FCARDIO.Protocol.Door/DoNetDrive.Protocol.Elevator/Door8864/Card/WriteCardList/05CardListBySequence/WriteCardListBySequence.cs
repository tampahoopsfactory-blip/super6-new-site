using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Card;
using DoNetDrive.Protocol.OnlineAccess;
using DotNetty.Buffers;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.CardListBySequence
{
    /// <summary>
    ///  将卡片列表写入到控制器非排序区 
    /// </summary>
    public class WriteCardListBySequence 
        : WriteCardListBySequenceBase<FC8864.Data.CardDetail>
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="perameter"></param>
        public WriteCardListBySequence(INCommandDetail cd, WriteCardListBySequence_Parameter perameter) : base(cd, perameter)
        {
            mPacketCardMax = 3;
            MaxBufSize = (mPacketCardMax * 0x65) + 4;

            CmdType = 0x47;
            CheckResponseCmdType = 0x27;
        }

        /// <summary>
        /// 创建一个通讯指令 方便调试，可删除
        /// </summary>
        protected override void CreatePacket0()
        {
            //创建一个通讯缓冲区
            var buf = GetNewCmdDataBuf(MaxBufSize);

            _ProcessMax = _CardPar.CardList.Count;
            WriteCardDetailToBuf(buf);

            Packet(CmdType, 0x04, 0x00, (uint)buf.ReadableBytes, buf);
        }

        /// <summary>
        /// 处理返回值 方便调试，可删除
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            base.CommandNext1(oPck);
        }

        protected override void ReadCardByFailBuf(List<decimal> CardList, IByteBuffer buf)
        {

        }
    }
}
