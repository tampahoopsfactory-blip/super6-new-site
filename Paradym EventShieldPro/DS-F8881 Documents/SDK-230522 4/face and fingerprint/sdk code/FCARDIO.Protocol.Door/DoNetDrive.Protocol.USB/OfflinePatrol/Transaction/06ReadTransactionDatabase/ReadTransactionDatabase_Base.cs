using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USB.OfflinePatrol.Data.Transaction;
using DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.TransactionDatabaseDetail;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.USBDrive;
using System.Collections.Generic;
using System.Linq;
using DoNetDrive.Protocol.Packet;
using System;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ReadTransactionDatabase
{
    /// <summary>
    ///  读取新记录
    ///  读指定类型的记录数据库最新记录，并读取指定数量。
    ///  成功返回结果参考 link ReadTransactionDatabase_Result 
    /// </summary>
    public abstract class ReadTransactionDatabase_Base : Read_Command
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public ReadTransactionDatabase_Parameter mParameter;

        /// <summary>
        /// 每个步骤的处理返回值
        /// </summary>
        public IReadTransactionDatabase_CallBack readDataBaseDetailCallBack;

        /// <summary>
        /// 记录序号是否已读取的集合，
        /// </summary>
        public Dictionary<int, bool> mTransactionSerialNumberList;


        /// <summary>
        /// 指示当前命令进行的步骤
        /// </summary>
        public int mStep;
        /// <summary>
        /// 读取到的记录数据缓冲
        /// </summary>
        public Queue<IByteBuffer> mBufs;

        /// <summary>
        /// 本次读取的数量
        /// </summary>
        public int mReadQuantity;

        /// <summary>
        /// 可读取的新记录数量
        /// </summary>
        public int mReadable;

        /// <summary>
        /// 读取计数
        /// </summary>
        public int mReadTotal;


        /// <summary>
        /// 选择的记录模块
        /// </summary>
        public TransactionDetailBase transactionDetail;

        /// <summary>
        /// 事务类型
        /// </summary>
        public int mTransactionType;


        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadTransactionDatabase_Base(INCommandDetail cd, ReadTransactionDatabase_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadTransactionDatabase_Parameter model = value as ReadTransactionDatabase_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个指令
        /// </summary>
        protected override void CreatePacket0()
        {
            mStep = 1;
            ReadTransactionDatabase_Result result = new ReadTransactionDatabase_Result();
            readDataBaseDetailCallBack = new ReadDataBaseDetailCallBack();

            mParameter = (ReadTransactionDatabase_Parameter)_Parameter;
            result.DatabaseType = mParameter.DatabaseType;
            result.TransactionList = new List<AbstractTransaction>();
            mTransactionType = (int)mParameter.DatabaseType;
            _Result = result;
            Packet(0x04, 0x01);
        }


        /// <summary>
        /// 
        /// 处理接收返回值，避免父类直接完成命令，重写逻辑
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(USBDrivePacket oPck)
        {
            CommandNext1(oPck);
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (mStep != 4)
            {
                readDataBaseDetailCallBack.CommandCallBack(this, oPck);
            }
            else
            {
                if (CheckResponse_OK(oPck))
                {
                    _ProcessStep = _ProcessMax;
                    fireCommandProcessEvent();

                    ReadTransactionDatabase_Result result = (ReadTransactionDatabase_Result)_Result;
                    result.Quantity = mReadTotal;

                    //Console.WriteLine($"丢失总数：{mLoseCount}，重读次数：{mReReadCount}" );
                    CommandCompleted();
                }
            }
            /*
            switch (mStep)
            {
                case 1://读取记录数据库空间信息的返回值

                    //ReadDataBaseDetailCallBack(oPck);
                    break;
                case 2://读记录数据库的返回值
                    //ReadTransactionDatabaseByIndexCallBack(oPck);
                    break;

                case 3://重复读取遗漏的记录的返回值
                    ReReadDatabaseCallBack(oPck);
                    break;
                case 4://写记录上传断点
                    if (CheckResponse_OK(oPck))
                    {
                        _ProcessStep = _ProcessMax;
                        fireCommandProcessEvent();

                        ReadTransactionDatabase_Result result = (ReadTransactionDatabase_Result)_Result;
                        result.Quantity = mReadTotal;

                        //Console.WriteLine($"丢失总数：{mLoseCount}，重读次数：{mReReadCount}" );
                        CommandCompleted();
                    }
                    break;
                default:
                    break;
            }
            */
        }

        /// <summary>
        /// 读取记录数据库空间信息的返回值 mStep=1
        /// </summary>
        /// <param name="oPck"></param>
        public void ReadDataBaseDetailCallBack(USBDrivePacket oPck)
        {
            var buf = oPck.CmdData;
            ReadTransactionDatabaseDetail_Result rst = new ReadTransactionDatabaseDetail_Result();
            rst.SetBytes(buf);

            transactionDetail = rst.DatabaseDetail.ListTransaction[(int)mParameter.DatabaseType - 1];
            if (transactionDetail.readable() == 0)
            {
                CommandCompleted();
            }
            else
            {
                mStep = 2;
                mBufs = new Queue<IByteBuffer>();
                mTransactionSerialNumberList = new Dictionary<int, bool>();


                var dataBuf = GetNewCmdDataBuf(9);
                dataBuf.WriteByte((int)mParameter.DatabaseType);
                dataBuf.WriteInt(0);
                dataBuf.WriteInt(0);
                Packet(0x04, 0x06, 0x09, dataBuf);
                //计算最终需要读取的记录数
                mReadable = (int)transactionDetail.readable();
                if (mParameter.Quantity > 0)
                {
                    if (mParameter.Quantity < mReadable)
                    {
                        mReadable = mParameter.Quantity;

                    }
                }

                mReadQuantity = 0;
                mReadTotal = 0;
                _ProcessMax = mReadable;
                _ProcessStep = 0;

                if (transactionDetail.IsCircle)
                {
                    transactionDetail.ReadIndex = transactionDetail.WriteIndex;
                }
                ReadTransactionNext();
            }
        }

        /// <summary>
        /// 读记录数据库的返回值 mStep=2
        /// 读取下一包记录
        /// </summary>
        /// <param name="bPck"></param>
        //protected abstract void ReadTransactionDatabaseByIndexCallBack(USBDrivePacket bPck);

        /// <summary>
        /// 读取下一包记录
        /// </summary>
        public void ReadTransactionNext()
        {
            _ProcessStep = mReadTotal;
            mReadable -= mReadQuantity;
            if (mReadable <= 0)
            {
                //记录读取完毕，
                ReadIndexComplete();
                return;
            }

            //计算本次读取的数量
            mReadQuantity = mParameter.PacketSize;

            //如果发现读索引号定位在记录末尾，则强制转移到记录头
            if (transactionDetail.ReadIndex == transactionDetail.DataBaseMaxSize)
            {
                transactionDetail.ReadIndex = 0;
            }
            if (mReadQuantity > mReadable)
            {
                mReadQuantity = mReadable;
            }

            int iBeginIndex = (int)transactionDetail.ReadIndex + 1;
            int iEndIndex = iBeginIndex + mReadQuantity - 1;

            if (iEndIndex > transactionDetail.DataBaseMaxSize)
            {
                mReadQuantity = (int)(transactionDetail.DataBaseMaxSize - transactionDetail.ReadIndex);
                iEndIndex = iBeginIndex + mReadQuantity - 1;
            }
            AddDictSerialNumberRange((int)transactionDetail.ReadIndex, mReadQuantity);


            transactionDetail.ReadIndex = Convert.ToUInt32(iEndIndex);//更新记录尾号


            var cmdBuf = USBPacket.CmdData;
            cmdBuf.SetInt(1, iBeginIndex);
            cmdBuf.SetInt(5, mReadQuantity);

            CommandReady();
        }

        /// <summary>
        /// 读记录数据库的返回值 mStep=2
        /// 读取下一包记录
        /// </summary>
        /// <param name="oPck"></param>
        public void ReadTransactionDatabaseByIndexCallBack(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck,4,6))
            {
                var buf = oPck.CmdData;
                SaveTransactionToBuf(buf);
            }
            else if (CheckResponse(oPck, 0xF4 - 0x30, 0x06, 4))
            {
                ReadTransactionNext();
            }
        }


        /*
        private Random TestRnd = new Random();
        private int mLoseCount = 0, mReReadCount = 0;
        */

        /// <summary>
        /// 将返回的事务暂时保存在缓冲中
        /// </summary>
        /// <param name="bTransactionBuf"></param>
        public void SaveTransactionToBuf(IByteBuffer bTransactionBuf)
        {

            bTransactionBuf.MarkReaderIndex();
            int iSize = bTransactionBuf.ReadInt();//本数据包中包含的记录数
            int iBeginRecordNum = bTransactionBuf.ReadInt();//本数据包中第一个记录的序号（起始序号）
            bTransactionBuf.ResetReaderIndex();

            /*

            //随机不存，测试漏存现象
            int iValue = TestRnd.Next(1, 100);
            if (iValue > 45 && iValue < 60)
            {
                //mLoseCount += iSize;
                CommandWaitResponse();//不存 15%的概率
                return;
            }
            */

            //此序号未记录，返回，不保存
            if (!mTransactionSerialNumberList.ContainsKey(iBeginRecordNum))
            {
                //让命令持续等待下去
                CommandWaitResponse();
                return;
            }

            //检查是否重复读取
            int iRecordNum = 0;
            int iSaveCount = 0;
            for (int i = 0; i < iSize; i++)
            {
                iRecordNum = iBeginRecordNum + i;
                if (mTransactionSerialNumberList.ContainsKey(iRecordNum))
                {
                    if (!mTransactionSerialNumberList[iRecordNum])
                    {
                        mTransactionSerialNumberList[iRecordNum] = true;
                        iSaveCount++;
                    }
                }
            }
            if (iSaveCount > 0)
            {
                mReadTotal += iSaveCount;
                _ProcessStep = mReadTotal;
                fireCommandProcessEvent();

                bTransactionBuf.Retain();
                mBufs.Enqueue(bTransactionBuf);

            }
            //让命令持续等待下去
            CommandWaitResponse();
        }


        /// <summary>
        /// 检查修改记录读索引号的返回值  mStep=2
        /// </summary>
        private void ReadIndexComplete()
        {
            ReadTransactionDatabase_Result result = (ReadTransactionDatabase_Result)_Result;
            result.Quantity = mReadTotal;
            result.readable = (int)transactionDetail.readable();

            Analysis();

            CheckResultList();
        }

        /// <summary>
        /// 分析缓冲中的数据包
        /// </summary>
        private void Analysis()
        {
            ReadTransactionDatabase_Result result = (ReadTransactionDatabase_Result)_Result;
            if (result.TransactionList == null)
            {
                result.TransactionList = new List<AbstractTransaction>();
            }
            var lst = result.TransactionList;

            int iSize;
            //新建一个保存序列号字典，为了防止重复记录
            HashSet<int> hsSerialNumberList = new HashSet<int>();

            while (mBufs.Count > 0)
            {
                IByteBuffer buf = mBufs.Dequeue();
                iSize = buf.ReadInt();

                for (int i = 0; i < iSize; i++)
                {
                    int serialNumber = buf.ReadInt();
                    AbstractTransaction cd = GetNewTransaction();
                    cd.SetSerialNumber(serialNumber);
                    cd.SetBytes(buf);

                    if (!hsSerialNumberList.Contains(serialNumber))
                    {
                        lst.Add(cd);
                        hsSerialNumberList.Add(serialNumber);
                    }

                }
                buf.Release();//要释放
            }
            hsSerialNumberList.Clear();
            hsSerialNumberList = null;
        }


        /// <summary>
        /// 重复读取遗漏的记录 mStep=3
        /// </summary>
        /// <param name="oPck"></param>
        public void ReReadDatabaseCallBack(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck))
            {
                var buf = oPck.CmdData;
                SaveTransactionToBuf(buf);

            }
            else if (CheckResponse(oPck, 4, 6, 4))
            {
                //继续发送下一波
                CheckResultList();

            }
        }


        /// <summary>
        /// 检查是否有遗漏
        /// </summary>
        public void CheckResultList()
        {
            var tSerialNumber = mTransactionSerialNumberList.FirstOrDefault(t => t.Value == false);
            if (tSerialNumber.Key != 0)
            {
                //检查丢失的记录是否连续
                int iBeginNum = tSerialNumber.Key;
                int iEndNum = iBeginNum + 1;
                while (mTransactionSerialNumberList.ContainsKey(iEndNum) && mTransactionSerialNumberList[iEndNum] == false)
                {
                    iEndNum++;
                    if ((iEndNum - iBeginNum) > 150) break;
                }

                var buf = USBPacket.CmdData;

                buf.SetInt(1, iBeginNum);
                buf.SetInt(5, (iEndNum - iBeginNum));
                //mReReadCount++;
                mStep = 3;
                CommandReady();
            }
            else
            {
                Analysis();//分析并保存记录

                WriteTransactionReadIndex();
            }
        }
        /// <summary>
        /// 记录读取完毕，需要更新读索引（更新记录尾号）
        /// </summary>
        private void WriteTransactionReadIndex()
        {
            var buf = GetCmdBuf();
            buf.WriteByte((int)mParameter.DatabaseType);
            buf.WriteInt((int)transactionDetail.ReadIndex);
            buf.WriteBoolean(false);
            USBPacket.CmdIndex = 0x04;
            USBPacket.DataLen = buf.ReadableBytes;
            CommandReady();
            mStep = 4;
        }



        /// <summary>
        /// 命令释放时需要的函数
        /// </summary>
        protected override void Release1()
        {
            mTransactionSerialNumberList = null;
        }

        /// <summary>
        /// 提交序号到未读集合
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="len"></param>
        private void AddDictSerialNumberRange(int startIndex, int len)
        {
            for (int i = 1; i <= len; i++)
            {
                mTransactionSerialNumberList.Add(i + startIndex, false);
            }
        }

        /// <summary>
        /// 获取一个事务实体
        /// </summary>
        /// <returns></returns>
        protected abstract AbstractTransaction GetNewTransaction();
    }
}
