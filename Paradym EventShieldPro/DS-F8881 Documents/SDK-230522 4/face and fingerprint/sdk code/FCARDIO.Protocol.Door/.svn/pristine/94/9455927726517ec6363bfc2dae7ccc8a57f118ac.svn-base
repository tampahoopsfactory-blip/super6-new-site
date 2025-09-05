using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Fingerprint.Data;
using DoNetDrive.Protocol.Fingerprint.Data.Transaction;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    ///  读取新记录
    ///  读指定类型的记录数据库最新记录，并读取指定数量。
    ///  成功返回结果参考 link ReadTransactionDatabase_Result 
    /// </summary>
    public class ReadTransactionDatabaseByAll : BaseCombinedCommand
    {
        int _Step;

        /// <summary>
        /// 读取记录事务的封装算法
        /// </summary>
        BaseReadTransactionDatabaseSubCommand _SubCommand;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadTransactionDatabaseByAll(INCommandDetail cd, ReadTransactionDatabase_Parameter parameter) : base(cd, parameter)
        { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            _Step = 1;
            Packet(0x08, 0x01);
        }



        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            var model = value as ReadTransactionDatabase_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {
            switch (_Step)
            {
                case 1://读取记录详情返回值
                    CheckDataDetail(oPck);
                    break;
                case 2://读取记录
                    _SubCommand?.CommandNext(oPck);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 检查记录详情返回值
        /// </summary>
        /// <param name="oPck"></param>
        private void CheckDataDetail(OnlineAccessPacket oPck)
        {
            int iType = 0;
            TransactionDetail transactionDetail;
            var model = _Parameter as ReadTransactionDatabase_Parameter;
            if (CheckResponse(oPck, 0x08, 0x01, 0x00))
            {
                var buf = oPck.CmdData;
                ReadTransactionDatabaseDetail_Result rst = new ReadTransactionDatabaseDetail_Result();
                rst.SetBytes(buf);

                iType = (int)model.DatabaseType;
                var details = rst.DatabaseDetail.ListTransaction;

                transactionDetail = details[iType - 1] as TransactionDetail;
                transactionDetail.ReadIndex = 0;
                transactionDetail.IsCircle = false;

                if (transactionDetail.WriteIndex > transactionDetail.DataBaseMaxSize)
                {
                    transactionDetail.ReadIndex = transactionDetail.WriteIndex - transactionDetail.DataBaseMaxSize;
                }

            }
            else
            {
                return;
            }

            if (transactionDetail.readable() == 0)
            {
                //没有记录
                CreateResultNULL(transactionDetail.ReadIndex);
                return;

            }

            switch (iType)
            {
                case 1://读卡记录
                    _SubCommand = new ReadTransactionDatabaseSubCommand<CardTransaction>(this);
                    break;
                case 2://门磁记录
                    _SubCommand = new ReadTransactionDatabaseSubCommand<DoorSensorTransaction>(this);
                    break;
                case 3://系统记录
                    _SubCommand = new ReadTransactionDatabaseSubCommand<SystemTransaction>(this);
                    break;
                case 4://体温记录
                    _SubCommand = new ReadTransactionDatabaseSubCommand<BodyTemperatureTransaction>(this);
                    break;
                default:
                    _SubCommand = new ReadTransactionDatabaseSubCommand<CardTransaction>(this);
                    break;
            }

            _SubCommand.PacketSize = model.PacketSize;
            _SubCommand.RollbackWriteReadIndex = model.RollbackWriteReadIndex;
            _SubCommand.BeginRead(iType, transactionDetail, model.Quantity);
            _Step = 2;
            CommandReady();
        }

        /// <summary>
        /// 命令执行完毕
        /// </summary>
        protected void CreateResultNULL(long ReadIndex)
        {
            var rst = new Protocol.Door.Door8800.Transaction.ReadTransactionDatabase_Result();
            _Result = rst;
            var model = _Parameter as ReadTransactionDatabase_Parameter;
            rst.DatabaseType = model.DatabaseType;
            rst.Quantity = 0;
            rst.TransactionList = new List<AbstractTransaction>();
            rst.readable = 0;
            rst.TransactionReadIndex = (int)ReadIndex;

            CommandCompleted();
        }




        /// <summary>
        /// 命令执行完毕
        /// </summary>
        /// <param name="subCmd"></param>
        public override void SubCommandOver(ISubCommand subCmd)
        {
            var rst = new Protocol.Door.Door8800.Transaction.ReadTransactionDatabase_Result();
            _Result = rst;
            var model = _Parameter as ReadTransactionDatabase_Parameter;
            rst.DatabaseType = model.DatabaseType;

            var sc = subCmd as BaseReadTransactionDatabaseSubCommand;
            var lst = sc.GetTransactions();
            rst.Quantity = lst.Count;
            rst.TransactionList = new List<AbstractTransaction>(lst.Values);
            rst.readable = (int)sc.TransactionDetail.readable();
            rst.TransactionReadIndex = (int)sc.TransactionDetail.ReadIndex;
            
            CommandCompleted();
  

            sc.Release();
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Release1()
        {
            base.Release1();
            if (_SubCommand != null)
            {
                _SubCommand.Release();
                _SubCommand = null;
            }
        }
    }
}
