using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Card
{
    /// <summary>
    /// 将卡片列表写入到控制器排序区 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class WriteCardListBase<T> : Door8800Command_WriteParameter
        where T : Data.CardDetailBase
    {
        /// <summary>
        /// 当前命令进度
        /// </summary>
        protected int mStep = 0;
        /// <summary>
        /// 指示当前命令进行的步骤
        /// </summary>
        protected int mWriteCardIndex = 0;

        /// <summary>
        /// 保存写入失败的数据缓冲区
        /// </summary>
        protected Queue<IByteBuffer> mBufs = null;


        /// <summary>
        /// 每一包中最大的卡数量
        /// </summary>
        protected virtual int mPacketCardMax { get; set; }
        /// <summary>
        /// 默认的缓冲区大小
        /// </summary>
        protected int MaxBufSize = 350;

        /// <summary>
        /// 保存待上传卡列表的参数
        /// </summary>
        protected WriteCardList_Parameter_Base<T> _CardPar = null;


        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="perameter"></param>
        public WriteCardListBase(INCommandDetail cd, WriteCardList_Parameter_Base<T> perameter) : base(cd, perameter) { _CardPar = perameter; mPacketCardMax = 10; }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            _CardPar = value as WriteCardList_Parameter_Base<T>;
            if (_CardPar == null) return false;
            return _CardPar.checkedParameter();
        }




        /// <summary>
        /// 创建命令成功返回值
        /// </summary>
        protected virtual void Create_Result()
        {
            //无法写入的卡数量
            int FailTotal = 0;

            //无法写入的卡列表
            List<decimal> CardList = new List<decimal>();


            if (mBufs != null)
            {
                foreach (var buf in mBufs)
                {
                    int iCount = buf.ReadInt();
                    FailTotal += iCount;

                    for (int i = 0; i < iCount; i++)
                    {
                        ReadCardByFailBuf(CardList, buf);
                    }

                    buf.Release();
                }
            }


            WriteCardList_Result result = new WriteCardList_Result(FailTotal, CardList);
            _Result = result;
        }

        /// <summary>
        /// 从错误卡列表中读取一个错误卡号，加入到cardlist中
        /// </summary>
        /// <param name="CardList">错误卡列表</param>
        /// <param name="buf"></param>
        protected abstract void ReadCardByFailBuf(List<decimal> CardList, IByteBuffer buf);



        /// <summary>
        /// 将卡详情写入到ByteBuf中
        /// </summary>
        protected virtual void WriteCardDetailToBuf(IByteBuffer buf)
        {
            var lst = _CardPar.CardList;
            int iCount = lst.Count;//获取列表总长度
            iCount = iCount - mWriteCardIndex;//计算未上传总数

            int iLen = iCount;
            if (iLen > mPacketCardMax)
            {
                iLen = mPacketCardMax;
            }
            buf.Clear();

            WritePacketHeadToBuf(buf, iLen);

            for (int i = 0; i < iLen; i++)
            {


                var card = lst[mWriteCardIndex + i];
                WriteCardBodyToBuf(card, buf);
                
            }

            mWriteCardIndex += iLen;
            _ProcessStep += iLen;
        }



        /// <summary>
        /// 写入数据头
        /// </summary>
        /// <param name="buf">数据缓冲区</param>
        /// <param name="iPacketCardCount">本次需要写入的卡号数量</param>
        protected abstract void WritePacketHeadToBuf(IByteBuffer buf, int iPacketCardCount);

        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="card">卡详情</param>
        /// <param name="buf">缓冲区</param>
        protected virtual void WriteCardBodyToBuf(T card, IByteBuffer buf)
        {
            card.GetBytes(buf);
        }

        /// <summary>
        /// 检查是否已写完所有卡
        /// </summary>
        /// <returns></returns>
        protected bool IsWriteOver()
        {
            int iCount = _CardPar.CardList.Count;//获取列表总长度

            return (iCount - mWriteCardIndex) == 0;
        }
    }
}
