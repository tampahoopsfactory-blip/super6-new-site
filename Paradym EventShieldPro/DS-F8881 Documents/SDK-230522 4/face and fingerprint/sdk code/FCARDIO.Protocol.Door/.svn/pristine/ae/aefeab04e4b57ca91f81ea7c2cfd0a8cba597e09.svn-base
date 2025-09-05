using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Card
{
    /// <summary>
    /// 将卡片列表写入到控制器非排序区 
    /// </summary>
    public abstract class WriteCardListBySequenceBase<T> : WriteCardListBase<T>
        where T : Data.CardDetailBase, new()
    {

        /// <summary>
        /// 
        /// </summary>
        public byte CmdType;

        /// <summary>
        /// 
        /// </summary>
        public byte CheckResponseCmdType;

        /// <summary>
        /// 初始化命令结构 
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="perameter">包含需要上传的卡列表参数</param>
        public WriteCardListBySequenceBase(INCommandDetail cd, WriteCardList_Parameter_Base<T> perameter) : base(cd, perameter)
        {
            CmdType = 0x07;
            CheckResponseCmdType = 0x07;
        }

        /// <summary>
        /// 写入数据头
        /// </summary>
        /// <param name="buf">数据缓冲区</param>
        /// <param name="iPacketCardCount">本次需要写入的卡号数量</param>
        protected override void WritePacketHeadToBuf(IByteBuffer buf, int iPacketCardCount)
        {
            buf.WriteInt(iPacketCardCount);//指示此包包含的卡数量
        }


        /// <summary>
        /// 创建一个通讯指令
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
            else if (CheckResponse(oPck, CheckResponseCmdType, 0x04, 0xFF, oPck.DataLen))
            {//检查是否不是错误返回值

                //缓存错误返回值
                if (mBufs == null)
                {
                    mBufs = new Queue<IByteBuffer>();
                }
                oPck.CmdData.Retain();
                mBufs.Enqueue(oPck.CmdData);

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
                Create_Result();
                //全部上传完毕
                CommandCompleted();
            }
            else
            {
                //未发送完毕，继续发送
                var buf = GetCmdBuf();
                WriteCardDetailToBuf(buf);
                DoorPacket.DataLen = buf.ReadableBytes;
                CommandReady();//设定命令当前状态为准备就绪，等待发送
            }
        }


        /// <summary>
        /// 用来解析返回的错误卡数据
        /// </summary>
        protected T _CardDetail;


    }


}
