using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Transaction
{
    /// <summary>
    /// 读记录数据库
    /// 按指定索引号开始读指定类型的记录数据库，并读取指定数量。
    /// 成功返回结果参考 ReadTransactionDatabaseByIndex_Result 
    /// </summary>
    public abstract class ReadTransactionDatabaseByIndex_Base : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 指令分类
        /// </summary>
        protected byte CmdType;

        /// <summary>
        /// 返回指令分类
        /// </summary>
        protected byte CheckResponseCmdType;

        /// <summary>
        /// ByteBuffer 队列
        /// </summary>
        protected Queue<IByteBuffer> mBufs;

        /// <summary>
        /// 事务类型
        /// </summary>
        protected int mTransactionType;
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadTransactionDatabaseByIndex_Base(INCommandDetail cd, ReadTransactionDatabaseByIndex_Parameter parameter) : base(cd, parameter)
        {
            CmdType = 0x08;
            CheckResponseCmdType = 0x08;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadTransactionDatabaseByIndex_Parameter model = value as ReadTransactionDatabaseByIndex_Parameter;
            if (model == null) return false;
            mTransactionType = model.TransactionType;

            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(CmdType, 0x04, 0x00, 0x09, GetCmdData());
            mBufs = new Queue<IByteBuffer>();

        }

        /// <summary>
        /// 获取参数结构的字节编码
        /// </summary>
        /// <returns></returns>
        protected virtual IByteBuffer GetCmdData()
        {
            ReadTransactionDatabaseByIndex_Parameter model = _Parameter as ReadTransactionDatabaseByIndex_Parameter;
            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(model.GetDataLen());
            model.GetBytes(buf);
            _ProcessMax = model.Quantity;
            return buf;
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, CheckResponseCmdType, 0x04,0))
            {
                var buf = oPck.CmdData;
                int iSize = buf.GetInt(0);
                _ProcessStep += iSize;
                buf.Retain();
                mBufs.Enqueue(buf);

                CommandWaitResponse();
            }
            if (CheckResponse(oPck, CheckResponseCmdType, 0x04, 0xff, 4))
            {
                var buf = oPck.CmdData;
                int iSize = buf.ReadInt();
                ReadTransactionDatabaseByIndex_Result result = new ReadTransactionDatabaseByIndex_Result();
                ReadTransactionDatabaseByIndex_Parameter par = _Parameter as ReadTransactionDatabaseByIndex_Parameter;
                result.TransactionType = (e_TransactionDatabaseType)mTransactionType;
                result.ReadIndex = par.ReadIndex;
                result.Quantity = iSize;
                result.TransactionList = new List<AbstractTransaction>();
                _Result = result;
                if (iSize > 0)
                {
                    Analysis();
                }
                _ProcessStep = _ProcessMax;
                fireCommandProcessEvent();

                CommandCompleted();

            }

        }

        /// <summary>
        /// 清空缓冲区
        /// </summary>
        protected virtual  void ClearBuf()
        {
            if (mBufs != null)
            {
                while (mBufs.Count > 0)
                {
                    var buf = mBufs.Dequeue();
                    buf.Release();
                    buf = null;
                }
            }
        }

        /// <summary>
        /// 命令重发时需要的函数
        /// </summary>
        protected override void CommandReSend()
        {
            ClearBuf();
            return;
        }
        /// <summary>
        /// 命令释放时需要的函数
        /// </summary>
        protected override void Release1()
        {
            ClearBuf();
            mBufs = null;
            return;
        }

        /// <summary>
        /// 获取一个事务实体
        /// </summary>
        /// <returns></returns>
        protected abstract AbstractTransaction GetNewTransaction();

        /// <summary>
        /// 分析缓冲中的数据包
        /// </summary>
        protected virtual void Analysis()
        {
            ReadTransactionDatabaseByIndex_Result result = (ReadTransactionDatabaseByIndex_Result)_Result;

            var lst = result.TransactionList;
            while (mBufs.Count > 0)
            {
                IByteBuffer buf = mBufs.Dequeue();
                int iSize = buf.ReadInt();

                for (int i = 0; i < iSize; i++)
                {
                    try
                    {
                        AbstractTransaction cd = GetNewTransaction();
                        cd.SetSerialNumber(buf.ReadInt());
                        cd.SetBytes(buf);
                        lst.Add(cd);
                    }
                    catch (Exception e)
                    {

                    }

                }
                buf.Release();//要释放
                buf = null;
            }
        }
    }
}
