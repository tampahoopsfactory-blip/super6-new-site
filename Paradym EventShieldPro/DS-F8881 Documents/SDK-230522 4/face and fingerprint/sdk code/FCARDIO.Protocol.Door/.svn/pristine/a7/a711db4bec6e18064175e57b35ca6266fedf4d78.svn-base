using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Card;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.CardListBySort
{
    /// <summary>
    ///  将卡片列表写入到控制器排序区 
    /// </summary>
    public class WriteCardListBySort : WriteCardListBySortBase<Data.CardDetail>
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="perameter"></param>
        public WriteCardListBySort(INCommandDetail cd, WriteCardListBySort_Parameter perameter) : base(cd, perameter)
        {
            mPacketCardMax = 3;
            MaxBufSize = (mPacketCardMax * 0x65) + 8;
            CmdType = 0x47;
            CheckResponseCmdType = 0x27;
        }

       

        /// <summary>
        /// 从错误卡列表中读取一个错误卡号，加入到cardlist中
        /// </summary>
        /// <param name="CardList">错误卡列表</param>
        /// <param name="buf"></param>
        protected override void ReadCardByFailBuf(List<decimal> CardList, IByteBuffer buf)
        {
            CardList.Add((UInt64)buf.ReadInt());
        }

        /// <summary>
        /// 处理返回值 方便调试，可删除
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            base.CommandNext1(oPck);
        }
    }
}
