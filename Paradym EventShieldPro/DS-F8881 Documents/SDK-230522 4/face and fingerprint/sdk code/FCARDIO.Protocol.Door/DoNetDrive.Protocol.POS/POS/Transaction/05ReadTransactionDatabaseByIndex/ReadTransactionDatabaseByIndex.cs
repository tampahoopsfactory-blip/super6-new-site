using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.Transaction;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 读取记录
    /// </summary>
    public class ReadTransactionDatabaseByIndex : Read_Command
    {
        /// <summary>
        /// 读取到的密码缓冲
        /// </summary>
        protected List<IByteBuffer> mReadBuffers;

        ReadTransactionDatabaseByIndex_Parameter mPar;

        public static Func<AbstractTransaction>[] NewTransactionTable;
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="detail"></param>
        public ReadTransactionDatabaseByIndex(DESDriveCommandDetail detail, ReadTransactionDatabaseByIndex_Parameter par) : base(detail, par) {
            mPar = par;
            NewTransactionTable = new Func<AbstractTransaction>[3];
            NewTransactionTable[1] = () => new CardTransaction();//读卡记录
            NewTransactionTable[2] = () => new SystemTransaction();//系统记录
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            ReadTransactionDatabaseByIndex_Parameter model = _Parameter as ReadTransactionDatabaseByIndex_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x09, 0x04, 0x00, 0x07, model.GetBytes(buf));
            mReadBuffers = new List<IByteBuffer>();
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
            return model.checkedParameter();
        }


        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponse(oPck, 0x09, 4, 0))
            {
                var buf = oPck.CommandPacket.CmdData;
                buf.Retain();
                mReadBuffers.Add(buf);

            }
            else if (CheckResponse(oPck, 0x09, 4, 0xFF, 2))
            {
                var buf = oPck.CommandPacket.CmdData;
                int iTotal = buf.ReadShort();
                _ProcessMax = iTotal;
                ReadTransactionDatabaseByIndex_Result rst = new ReadTransactionDatabaseByIndex_Result();
                rst.TransactionList = new List<DoNetDrive.Protocol.Transaction.AbstractTransaction>(iTotal);
                foreach (IByteBuffer tmpbuf in mReadBuffers)
                {
                    int iSize = tmpbuf.ReadByte();
                    int iSerialNumber = tmpbuf.ReadInt();
                    for (int i = 0; i < iSize; i++)
                    {
                        AbstractTransaction cd = GetNewTransaction((int)mPar.DatabaseType);
                        cd.SetSerialNumber(iSerialNumber + i);
                        cd.SetBytes(tmpbuf);
                        rst.TransactionList.Add(cd);
                    }
                    _ProcessStep += iSize;
                    fireCommandProcessEvent();
                    tmpbuf.Release();
                }

                
                _Result = rst;

                mReadBuffers.Clear();
                CommandCompleted();

            }

        }

        AbstractTransaction GetNewTransaction(int transactionType)
        {
            return NewTransactionTable[transactionType]();
        }
    }
}
