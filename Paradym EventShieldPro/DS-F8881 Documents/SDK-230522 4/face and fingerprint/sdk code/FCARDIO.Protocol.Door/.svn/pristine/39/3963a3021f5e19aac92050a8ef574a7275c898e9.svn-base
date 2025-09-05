using DotNetty.Buffers;
using FCARDIO.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCARDIO.Protocol.OnlineAccess;

namespace FCARDIO.Protocol.Elevator.FC8864.Card.CardDataBase
{
    /// <summary>
    /// 从控制器中读取卡片数据<br/>
    /// 成功返回结果参考 @link ReadCardDataBase_Base_Result
    /// </summary>
    public abstract class ReadCardDataBase_Base<T> : Read_Command
        where T : Data.CardDetailBase, new()
    {
        private int mStep;//指示当前命令进行的步骤
        /// <summary>
        /// 读取到的卡数据缓冲
        /// </summary>
        private Queue<IByteBuffer> mReadBuffers;

        /// <summary>
        /// 初始化命令结构 
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadCardDataBase_Base(INCommandDetail cd, ReadCardDataBase_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadCardDataBase_Parameter model = value as ReadCardDataBase_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x47, 0x01, 0x00);//首先读取卡片数据库详情，检查是否有卡片需要读取
            mStep = 1;
            _ProcessMax = 1;

        }

        /// <summary>
        /// 获取参数结构的字节编码
        /// </summary>
        /// <returns></returns>
        private IByteBuffer GetCmdData()
        {
            ReadCardDataBase_Parameter model = _Parameter as ReadCardDataBase_Parameter;
            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(model.GetDataLen());
            model.GetBytes(buf);
            return buf;
        }


        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            switch (mStep)
            {
                case 1://读取卡片数据库详情回调
                    if (CheckResponse(oPck))
                    {
                        ReadDetailCallBlack(oPck.CmdData);
                    }

                    break;
                case 2://读取卡片数据库内容
                    if (CheckResponse(oPck))//检查返回的是否为卡数据
                    {
                        ReadCardDatabaseCallBlack(oPck.CmdData);
                    }

                    if (CheckResponse(oPck, 0x07, 0x03, 0xFF)) //检查数据是否返回完毕
                    {
                        ReadCardDatabaseOverCallBlack(oPck.CmdData);
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 读取卡片详情的回调
        /// </summary>
        /// <param name="buf"></param>
        protected virtual void ReadDetailCallBlack(IByteBuffer buf)
        {
            //应答：卡片容量详情
            CardDatabaseDetail.ReadCardDatabaseDetail_Result detail_Result = new CardDatabaseDetail.ReadCardDatabaseDetail_Result();
            detail_Result.SetBytes(buf);

            ReadCardDataBase_Parameter model = _Parameter as ReadCardDataBase_Parameter;
            long iCard = 0;

            switch (model.CardType)
            {
                case 1://排序卡区域
                    iCard = detail_Result.SortCardSize;
                    break;
                case 2://非排序卡区域
                    iCard = detail_Result.SequenceCardSize;
                    break;
                default://所有区域
                    iCard = detail_Result.SequenceCardSize + detail_Result.SortCardSize;
                    break;

            }

            if (iCard > 0)
            {
                //继续读取卡数据
                _ProcessMax = (int)iCard;
                Packet(0x07, 0x03, 0x00, 0x01, GetCmdData());
                mStep = 2;//准备接受返回的卡数据
                mReadBuffers = new Queue<IByteBuffer>();
                CommandReady();

            }
            else
            {
                //命令直接完成
                ReadCardDataBase_Base_Result<T> result = CreateResult(null, 0, model.CardType);
                _Result = result;
                CommandCompleted();
            }
        }

        /// <summary>
        /// 读取卡片数据回调
        /// </summary>
        protected virtual void ReadCardDatabaseCallBlack(IByteBuffer buf)
        {
            int iCount = buf.GetInt(0);//获取本次传输的卡数量
            _ProcessStep += iCount;
            buf.Retain();
            mReadBuffers.Enqueue(buf);
            fireCommandProcessEvent();
            CommandWaitResponse();
        }

        /// <summary>
        /// 读取卡片数据完毕时的回调
        /// </summary>
        protected virtual void ReadCardDatabaseOverCallBlack(IByteBuffer buf)
        {
            ReadCardDataBase_Parameter model = _Parameter as ReadCardDataBase_Parameter;
            int iCount = buf.ReadInt();//获取本次总传输的卡数量
            if(iCount>0)
                _ProcessStep = iCount;
            fireCommandProcessEvent();
            //开始解析卡数据
            List<T> cardList = new List<T>();
            while (mReadBuffers.Count>0)
            {
                buf = mReadBuffers.Dequeue();
                iCount = buf.ReadInt();//返回缓冲区中包含的卡数量
                for (int i = 0; i < iCount; i++)
                {
                    T card = new T();
                    card.SetBytes(buf);
                    cardList.Add(card);
                }
                buf.Release();
            }
            ReadCardDataBase_Base_Result<T> result = CreateResult(cardList, cardList.Count, model.CardType);
            _Result = result;

            CommandCompleted();
            
        }

        /// <summary>
        /// 创建返回值
        /// </summary>
        /// <param name="cardList">授权卡集合</param>
        /// <param name="dataBaseSize">授权卡数量</param>
        /// <param name="cardType">授权卡类型</param>
        protected abstract ReadCardDataBase_Base_Result<T> CreateResult(List<T> cardList, int dataBaseSize, int cardType);

        /// <summary>
        /// 释放命令占用的内存<br/>
        /// 此命令一般情况下不需要实现！
        /// </summary>
        protected override void Release1()
        {

            ClearBuf();
            mReadBuffers = null;
        }

        /// <summary>
        /// 命令重发时，对命令中一些缓冲做清空或参数重置<br/>
        /// 此命令一般情况下不需要实现！
        /// </summary>
        protected override void CommandReSend()
        {
            ClearBuf();
        }

        /// <summary>
        /// 清空缓冲区
        /// </summary>
        protected void ClearBuf()
        {
            if (mReadBuffers != null)
            {
                foreach (IByteBuffer buf in mReadBuffers)
                {
                    buf.Release();
                }
                mReadBuffers.Clear();
            }

        }

    }
}
