using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Transaction;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    /// 抽象的读取记录逻辑
    /// </summary>
    public abstract class BaseReadTransactionDatabaseSubCommand : BaseSubCommand
    {
        /// <summary>
        /// 本次读取的数量
        /// </summary>
        protected int mReadQuantity;

        /// <summary>
        /// 可读取的新记录数量
        /// </summary>
        protected int mReadable;

        /// <summary>
        /// 读取计数
        /// </summary>
        protected int mReadTotal;

        /// <summary>
        /// 事务类型
        /// </summary>
        protected int mTransactionType;

        /// <summary>
        /// 事务类型
        /// </summary>
        protected Dictionary<int, AbstractTransaction> _TransactionList;

        /// <summary>
        /// 事务序号集合，用于检查遗漏
        /// </summary>
        protected Dictionary<int, bool> _SerialNumberList;

        /// <summary>
        /// 读取数量 0-160000,0表示都取所有新记录
        /// </summary>
        public int Quantity;

        /// <summary>
        /// 每次读取数量 1-150
        /// </summary>
        public int PacketSize;

        /// <summary>
        /// 需要读取的记录详情
        /// </summary>
        public Data.TransactionDetail TransactionDetail;

        /// <summary>
        /// 获取读取完毕的记录列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, AbstractTransaction> GetTransactions() => _TransactionList;

        /// <summary>
        /// 进行的步骤
        /// </summary>
        private int mStep;
        /// <summary>
        /// 任务开始时的读索引(上传断点)
        /// </summary>
        private long mOldReadIndex;

        /// <summary>
        /// 检查记录遗漏次数
        /// </summary>
        private int mCheckRecordCount;
        /// <summary>
        /// 回滚读索引（上传断点）
        /// </summary>
        public bool RollbackWriteReadIndex;

        /// <summary>
        /// 创建一个读取事务的子命令
        /// </summary>
        /// <param name="mainCmd"></param>
        public BaseReadTransactionDatabaseSubCommand(ICombinedCommand mainCmd) : base(mainCmd)
        {
            PacketSize = 50;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public override void Release()
        {
            base.Release();
            _TransactionList = null;
            _SerialNumberList?.Clear();
            _SerialNumberList = null;
            TransactionDetail = null;

        }

        /// <summary>
        /// 开始读取事务
        /// </summary>
        public void BeginRead(int iTransactionType, Data.TransactionDetail dtl, int iReadQuantity)
        {
            //_TransactionList.Clear();
            mStep = 1;
            mReadTotal = 0;
            mReadable = 0;
            mReadQuantity = 0;
            mCheckRecordCount = 0;
            _IsOver = false;
            Quantity = iReadQuantity;
            mTransactionType = iTransactionType;
            TransactionDetail = dtl;
            _TransactionList = new Dictionary<int, AbstractTransaction>();
            _SerialNumberList = new Dictionary<int, bool>();
            CheckReadIndex();
            mOldReadIndex = TransactionDetail.ReadIndex;

            var dataBuf = GetNewCmdDataBuf(9);
            dataBuf.WriteByte(mTransactionType);
            dataBuf.WriteInt(0);
            dataBuf.WriteInt(0);
            Packet(0x08, 0x04, 0x00, 0x09, dataBuf);

            //计算最终需要读取的记录数
            mReadable = (int)TransactionDetail.readable();
            if (Quantity > 0)
            {
                if (Quantity < mReadable)
                {
                    mReadable = Quantity;
                }
            }
            ProcessMax = mReadable;
            ProcessStep = 0;

            ReadTransactionNext();
        }

        /// <summary>
        /// 检查上传断点是否异常
        /// </summary>
        protected virtual void CheckReadIndex()
        {
            if (TransactionDetail.ReadIndex > TransactionDetail.WriteIndex)
            {
                TransactionDetail.ReadIndex = TransactionDetail.WriteIndex;
            }
        }

        /// <summary>
        /// 读取下一包记录
        /// </summary>
        private bool ReadTransactionNext()
        {
            ProcessStep = mReadTotal;
            mReadable -= mReadQuantity;
            if (mReadable <= 0)
            {
                if(RollbackWriteReadIndex)
                {
                    //开始回滚上传断点
                    BeginRollbackWriteReadIndex();
                    return true;
                }
                //记录读取完毕，
                return CheckResultList();
            }

            //计算本次读取的数量
            mReadQuantity = PacketSize;
            if (mReadQuantity > mReadable)
            {
                mReadQuantity = mReadable;
            }

            GetReadIndexRange(out int iBeginIndex, out int iEndIndex);

            AddDictSerialNumberRange((int)TransactionDetail.ReadIndex, mReadQuantity);

            TransactionDetail.ReadIndex = iEndIndex;//更新记录尾号

            //Console.WriteLine($"读取记录，起始序号：{iBeginIndex}，读取数量：{mReadQuantity}");
            var cmdBuf = GetPacket().CmdData;
            cmdBuf.SetInt(1, iBeginIndex);
            cmdBuf.SetInt(5, mReadQuantity);
            return true;
        }


        /// <summary>
        ///  命令进行到下一个步骤
        /// </summary>
        /// <param name="oPck"></param>
        public override void CommandNext(INPacket accessPacket)
        {
            if (_IsOver) return;

            OnlineAccessPacket oPck = accessPacket as OnlineAccessPacket;
            if (oPck == null) return;

            switch (mStep)
            {
                case 1://读记录
                case 3://读取遗漏记录
                    if (CheckResponse(oPck, 0x08, 4, 0))
                    {
                        var buf = oPck.CmdData;
                        SaveTransaction(buf);
                    }
                    else if (CheckResponse(oPck, 0x08, 0x04, 0xff, 4))
                    {
                        if (ReadTransactionNext())
                            CommandReady();
                    }
                    break;
                case 2://回滚上传断点
                    if (CheckResponseOK(oPck))
                    {
                        var buf = GetPacket().CmdData;
                        Packet(0x08, 0x04, 0x00, 0x09, buf);
                        if (CheckResultList())
                        {
                            CommandReady();
                        }
                    }
                    break;
                default:
                    break;
            }

            
        }

        //Random ran = new Random();
        
        /// <summary>
        /// 保存记录
        /// </summary>
        /// <param name="buf"></param>
        private void SaveTransaction(IByteBuffer buf)
        {
            
            int iRecordCount = buf.ReadInt();
            /*int RandKey = ran.Next(1, 100);
            if (RandKey > 80)
            {
                int iSerialNumber = buf.ReadInt();
                Console.WriteLine($"跳过记录，起始序号：{iSerialNumber},数量： {iRecordCount}");
                return;
            }*/



            for (int i = 0; i < iRecordCount; i++)
            {
                int iSerialNumber = buf.ReadInt();
                AbstractTransaction Tr = GetNewTransaction();
                Tr.SetSerialNumber(iSerialNumber);
                Tr.SetBytes(buf);

                if (_SerialNumberList.ContainsKey(iSerialNumber))
                {
                    if (_SerialNumberList[iSerialNumber] == false)
                    {
                        _SerialNumberList[iSerialNumber] = true;
                        _TransactionList.Add(iSerialNumber, Tr);
                        mReadTotal++;
                    }
                }

            }
        }
        /// <summary>
        /// 获取一个新的事务实例
        /// </summary>
        /// <returns></returns>
        protected abstract AbstractTransaction GetNewTransaction();

        /// <summary>
        /// 提交序号到未读集合
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="len"></param>
        private void AddDictSerialNumberRange(int startIndex, int len)
        {
            for (int i = 1; i <= len; i++)
            {
                _SerialNumberList.Add(i + startIndex, false);
            }
        }

        /// <summary>
        /// 检查上传断点是否异常
        /// </summary>
        protected void GetReadIndexRange(out int iBeginIndex, out int iEndIndex)
        {

            iBeginIndex = (int)TransactionDetail.ReadIndex + 1;
            iEndIndex = iBeginIndex + mReadQuantity - 1;

            if (iEndIndex > TransactionDetail.WriteIndex)
            {
                iEndIndex = (int)TransactionDetail.WriteIndex;
                mReadQuantity = (int)(iEndIndex - iBeginIndex);
            }
        }




        /// <summary>
        /// 检查是否有遗漏
        /// </summary>
        private bool CheckResultList()
        {
            mStep = 3;

            if (Quantity == 0)
            {
                if (mCheckRecordCount > 1000)
                {
                    CommandOver();
                    return false;
                }
            }
            else
            {
                if (mCheckRecordCount > (Quantity * 10))
                {
                    CommandOver();
                    return false;
                }
            }

            var tSerialNumber = _SerialNumberList.FirstOrDefault(t => t.Value == false);
            int iReadQuantity = PacketSize;
            if (tSerialNumber.Key != 0)
            {
                //检查丢失的记录是否连续
                int iBeginNum = tSerialNumber.Key;
                int iEndNum = iBeginNum + 1;
                while (_SerialNumberList.ContainsKey(iEndNum) && _SerialNumberList[iEndNum] == false)
                {
                    iEndNum++;
                    if ((iEndNum - iBeginNum) >= iReadQuantity) break;
                }
                //Console.WriteLine($"记录丢失，重读，起始序号：{iBeginNum}，读取数量：{mReadQuantity}");
                var buf = GetPacket().CmdData;
                mReadQuantity = (iEndNum - iBeginNum);
                mReadable = mReadQuantity;
                buf.SetInt(1, iBeginNum);
                buf.SetInt(5, mReadQuantity);
                mCheckRecordCount += 1;
                return true;
            }
            else
            {
                CommandOver();
                return false;
            }
        }

        /// <summary>
        /// 回滚上传断点
        /// </summary>
        private void BeginRollbackWriteReadIndex()
        {
            //Console.WriteLine($"回滚上传断点，断点号：{mOldReadIndex}");
            mStep = 2;
            var buf = GetPacket().CmdData;
            buf.SetInt(1, (int)mOldReadIndex);
            buf.SetInt(5, 0);
            Packet(0x08, 0x03, 0x00, 0x05, buf);
        }

    }
}
