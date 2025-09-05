using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using FCARDIO.Core.Command;
using FCARDIO.Protocol.FC8800;
using FCARDIO.Protocol.OnlineAccess;

namespace FCARDIO.Protocol.Elevator.FC8864.Card.DeleteCard
{
    /// <summary>
    /// 将卡片列表从到控制器中删除
    /// </summary>
    public abstract class DeleteCardBase<T> : WriteCardListBase<T>
        where T : Data.CardDetailBase
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public DeleteCardBase(INCommandDetail cd, WriteCardList_Parameter_Base<T> parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {

            //创建一个通讯缓冲区
            var buf = GetNewCmdDataBuf(MaxBufSize);

            _ProcessMax = _CardPar.CardList.Count;
            WriteCardDetailToBuf(buf);

            Packet(0x07, 0x05, 0x00, (uint)buf.ReadableBytes, buf);
        }


        /// <summary>
        /// 重写父类对处理返回值的定义
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {
            if (CheckResponse_OK(oPck))
            {
                //继续发下一包
                CommandNext1(oPck);
            }
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (IsWriteOver())
            {
                //全部删除完毕
                CommandCompleted();
            }
            else
            {
                //未发送完毕，继续发送
                var buf = GetCmdBuf();
                WriteCardDetailToBuf(buf);
                FCPacket.DataLen = buf.ReadableBytes;
                CommandReady();//设定命令当前状态为准备就绪，等待发送
            }
        }

        /// <summary>
        /// 从错误卡列表中读取一个错误卡号，加入到cardlist中---本命令不实现此功能
        /// </summary>
        /// <param name="CardList">错误卡列表</param>
        /// <param name="buf"></param>
        protected override void ReadCardByFailBuf(List<UInt64> CardList, IByteBuffer buf)
        {
            return;
        }


        /// <summary>
        /// 写入数据头
        /// </summary>
        /// <param name="buf">数据缓冲区</param>
        /// <param name="iPacketCardCount">本次需要写入的卡号数量</param>
        protected override void WritePacketHeadToBuf(IByteBuffer buf, int iPacketCardCount)
        {
            buf.WriteInt(iPacketCardCount);
        }

        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="card">卡详情</param>
        /// <param name="buf">缓冲区</param>
        protected override void WriteCardBodyToBuf(T card, IByteBuffer buf)
        {
            WriteCardBodyToBuf0(card, buf);//数量
        }


        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="card">卡详情</param>
        /// <param name="buf">缓冲区</param>
        protected abstract void WriteCardBodyToBuf0(T card, IByteBuffer buf);


    }
}
