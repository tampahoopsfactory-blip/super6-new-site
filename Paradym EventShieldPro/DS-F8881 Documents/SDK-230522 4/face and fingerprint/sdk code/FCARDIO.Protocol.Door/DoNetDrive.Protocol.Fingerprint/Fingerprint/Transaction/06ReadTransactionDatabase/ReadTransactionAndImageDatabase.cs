using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.Fingerprint.AdditionalData;
using DoNetDrive.Protocol.Fingerprint.Data;
using DoNetDrive.Protocol.Fingerprint.Data.Transaction;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Transaction;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    /// 读取认证记录及附加数据（体温，照片），仅适用于人脸机
    /// </summary>
    public class ReadTransactionAndImageDatabase : BaseCombinedCommand
    {
        /// <summary>
        /// 是否保存照片到文件
        /// </summary>
        private readonly bool mSaveImageToFile;
        /// <summary>
        /// 图片保存文件夹
        /// </summary>
        private readonly string mImageDir;

        /// <summary>
        /// 读取到的记录
        /// </summary>
        Dictionary<int, CardAndImageTransaction> _TransactionList;

        /// <summary>
        /// 记录详情
        /// </summary>
        DoNetDrive.Protocol.Door.Door8800.Data.TransactionDetail[] mTransactionDetailList;

        /// <summary>
        /// 读取记录开始时的上传断点
        /// </summary>
        private int mMarkReadIndex;

        /// <summary>
        /// 指示当前命令进行的步骤
        /// </summary>
        private int mStep;

        /// <summary>
        /// 指示当前下载照片的记录序号
        /// </summary>
        private int mSaveFileSerialNumber;

        /// <summary>
        /// 读取记录后的上传断点
        /// </summary>
        private int mCardRecord_ReadIndex;

        /// <summary>
        /// 读取计数
        /// </summary>
        protected int mReadTotal;

        /// <summary>
        /// 读取记录的子函数
        /// </summary>
        BaseReadTransactionDatabaseSubCommand _ReadTransactionCommand;
        /// <summary>
        /// 读取文件的子函数
        /// </summary>
        ReadFileSubCommand _ReadFileCommand;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadTransactionAndImageDatabase(INCommandDetail cd,
            ReadTransactionAndImageDatabase_Parameter parameter) : base(cd, parameter)
        {
            mSaveImageToFile = parameter.PhotoSaveToFile;
            mImageDir = parameter.SaveImageDirectory;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadTransactionAndImageDatabase_Parameter model = value as ReadTransactionAndImageDatabase_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个指令
        /// </summary>
        protected override void CreatePacket0()
        {
            mStep = 1;
            Packet(0x08, 0x01);
        }

        /// <summary>
        /// 
        /// 处理接收返回值，避免父类直接完成命令，重写逻辑
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {
            try
            {
                switch (mStep)
                {
                    case 1://读取记录详情返回值
                        CheckDataDetail(oPck);
                        break;
                    case 2://读取认证记录
                        _ReadTransactionCommand?.CommandNext(oPck);
                        break;
                    case 3://更新认证记录上传断点为备份断点
                        if (CheckResponse_OK(oPck)) BeginReadBodyTemperature();
                        break;
                    case 4://读取体温记录
                        _ReadTransactionCommand?.CommandNext(oPck);
                        break;
                    case 5://修改体温记录上传断点为备份断点
                        if (CheckResponse_OK(oPck)) BeginReadImageFile();
                        break;
                    case 6://开始读取记录照片
                        _ReadFileCommand?.CommandNext(oPck);
                        break;
                    case 7://记录和照片都读取完毕,更新认证记录上传断点完毕，开始更新体温上传断点
                        if (CheckResponse_OK(oPck)) WriteTransactionReadIndex_BodyTemperature();
                        break;
                    case 8://更新体温上传断点完毕，创建返回值
                        if (CheckResponse_OK(oPck)) CreateResult();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                Trace.WriteLine($"{_Connector.GetKey()} ReadTransactionAndImageDatabase_CommandNext0 {mStep}:{_ProcessStep} {ex.Message}");
            }
        }

        /// <summary>
        /// 检查记录详情返回值
        /// </summary>
        /// <param name="oPck"></param>
        private void CheckDataDetail(OnlineAccessPacket oPck)
        {
            TransactionDetail transactionDetail;
            var model = _Parameter as ReadTransactionAndImageDatabase_Parameter;
            if (CheckResponse(oPck, 0x08, 0x01, 0x00))
            {
                var buf = oPck.CmdData;
                var rst = new ReadTransactionDatabaseDetail_Result();
                rst.SetBytes(buf);
                mTransactionDetailList = rst.DatabaseDetail.ListTransaction;

                transactionDetail = mTransactionDetailList[0] as TransactionDetail;
            }
            else
            {
                return;
            }

            //读卡记录
            _ReadTransactionCommand = new ReadTransactionDatabaseSubCommand<CardTransaction>(this);

            mMarkReadIndex = (int)transactionDetail.ReadIndex;

            _ReadTransactionCommand.PacketSize = model.PacketSize;
            _ReadTransactionCommand.RollbackWriteReadIndex = model.RollbackWriteReadIndex;
            if (transactionDetail.readable() > 0)
            {
                _ReadTransactionCommand.BeginRead(1, transactionDetail, model.Quantity);
                mStep = 2;
                CommandReady();
            }
            else
            {
                CreateResult();
            }

        }

        /// <summary>
        /// 命令执行完毕
        /// </summary>
        /// <param name="subCmd"></param>
        public override void SubCommandOver(ISubCommand subCmd)
        {
            switch (mStep)
            {
                case 2://读取认证记录
                    ReadCardTransactionOver();
                    break;
                case 4://读取体温记录
                    ReadBodyTemperatureTransactionOver();
                    break;
                case 6://读取文件
                    ReadImageCallblack();
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 读取认证记录完毕，开始读取体温记录
        /// </summary>
        private void ReadCardTransactionOver()
        {
            var lst = _ReadTransactionCommand.GetTransactions();
            var RecordDTL = _ReadTransactionCommand.TransactionDetail;
            _TransactionList = new Dictionary<int, CardAndImageTransaction>(lst.Count);
            foreach (var kv in lst)
            {
                _TransactionList.Add(kv.Key, new CardAndImageTransaction((CardTransaction)kv.Value));
            }
            lst.Clear();
            mStep = 3;
            _ReadTransactionCommand.Release();
            _ReadTransactionCommand = null;

            if (RecordDTL != mTransactionDetailList[0])
                mTransactionDetailList[0].ReadIndex = RecordDTL.ReadIndex;
            mCardRecord_ReadIndex = (int)RecordDTL.ReadIndex;
            var model = _Parameter as ReadTransactionAndImageDatabase_Parameter;
            if(model.RollbackWriteReadIndex)
                WriteTransactionReadIndex(1, mMarkReadIndex);//还原上传断点为备份断点
            else
                BeginReadBodyTemperature();
        }

        #region 读取体温记录

        /// <summary>
        /// 开始读取体温记录
        /// </summary>
        private void BeginReadBodyTemperature()
        {
            if (mTransactionDetailList.Length == 3)
            {
                BeginReadImageFile();
                return;
            }
            mStep = 4;
            var tr = mTransactionDetailList[3];

            mMarkReadIndex = (int)tr.ReadIndex;

            if (tr.WriteIndex > 1)
            {
                if (tr.readable() > 0)
                {
                    var model = _Parameter as ReadTransactionAndImageDatabase_Parameter;
                    _ReadTransactionCommand = new ReadTransactionDatabaseSubCommand<BodyTemperatureTransaction>(this);
                    _ReadTransactionCommand.PacketSize = model.PacketSize;
                    _ReadTransactionCommand.RollbackWriteReadIndex = model.RollbackWriteReadIndex;
                    //开始读取体温记录
                    _ReadTransactionCommand.BeginRead(4,
                        tr as TransactionDetail, _TransactionList.Count);
                    CommandReady();
                }
                else
                {
                    BeginReadImageFile();
                }
            }
            else
            {
                BeginReadImageFile();
            }


        }

        /// <summary>
        /// 读取体温记录完毕
        /// </summary>
        private void ReadBodyTemperatureTransactionOver()
        {
            var lst = _ReadTransactionCommand.GetTransactions();
            var iBTMaxNum = 0;
            int trNum = 0;
            if (lst.Count == 0)
            {
                //所有记录已读取完毕，没有新记录
                ReadBodyTemperatureTransaction_UpdateReadIndexBlackup();
                return;
            }
            foreach (var k in lst.Keys)
            {
                var bt = lst[k] as BodyTemperatureTransaction;
                trNum = bt.SerialNumber;
                if (trNum > iBTMaxNum) iBTMaxNum = trNum;
                if (_TransactionList.ContainsKey(trNum))
                {
                    var ct = _TransactionList[trNum];
                    ct.BodyTemperature = bt.BodyTemperature;
                }
            }
            var trLst = lst.Values.ToArray();
            Array.Sort(trLst, (x, y) => (x.SerialNumber.CompareTo(y.SerialNumber)));

            var BT_Dtl = mTransactionDetailList[3] as TransactionDetail;

            if (iBTMaxNum > mCardRecord_ReadIndex)
            {
                //序号已经超过了认证记录，不能在读了，需要回滚
                var bt = trLst.First((x) => x.SerialNumber >= mCardRecord_ReadIndex) as BodyTemperatureTransaction;
                if (bt.SerialNumber > mCardRecord_ReadIndex)
                {

                    BT_Dtl.ReadIndex = bt.RecordSerialNumber - 1;
                }
                else
                {
                    BT_Dtl.ReadIndex = bt.RecordSerialNumber;
                }

                ReadBodyTemperatureTransaction_UpdateReadIndexBlackup();

            }
            else
            {
                if (iBTMaxNum != mCardRecord_ReadIndex)
                {
                    var model = _Parameter as ReadTransactionAndImageDatabase_Parameter;
                    _ReadTransactionCommand.RollbackWriteReadIndex = model.RollbackWriteReadIndex;
                    //体温记录中的序号，没有超过或等于记录序号，需要继续读取
                    _ReadTransactionCommand.BeginRead(4, BT_Dtl, _TransactionList.Count);
                    CommandReady();
                }
                else
                {
                    ReadBodyTemperatureTransaction_UpdateReadIndexBlackup();
                }
            }
        }

        /// <summary>
        /// 体温记录读取完毕，更新上传断点为备份断点
        /// </summary>
        private void ReadBodyTemperatureTransaction_UpdateReadIndexBlackup()
        {
            if (mTransactionDetailList[3] != _ReadTransactionCommand.TransactionDetail)
                mTransactionDetailList[3].ReadIndex = _ReadTransactionCommand.TransactionDetail.ReadIndex;
            _ReadTransactionCommand?.Release();
            _ReadTransactionCommand = null;

            mStep = 5;

            var model = _Parameter as ReadTransactionAndImageDatabase_Parameter;
            if (model.RollbackWriteReadIndex)
                WriteTransactionReadIndex(4, mMarkReadIndex);
            else
                BeginReadImageFile();
        }
        #endregion


        #region 修改上传断点
        private void WriteTransactionReadIndex_Card()
        {
            var oPar = _Parameter as ReadTransactionAndImageDatabase_Parameter;
            if (!oPar.AutoWriteReadIndex)
            {
                CreateResult();
                return;
            }
            mStep = 7;
            WriteTransactionReadIndex(1, (int)mTransactionDetailList[0].ReadIndex);
        }

        private void WriteTransactionReadIndex_BodyTemperature()
        {
            mStep = 8;
            WriteTransactionReadIndex(4, (int)mTransactionDetailList[3].ReadIndex);
        }

        /// <summary>
        /// 记录读取完毕，需要更新读索引（更新记录尾号）
        /// </summary>
        private void WriteTransactionReadIndex(int iType, int ReadIndex)
        {

            var buf = GetCmdBuf();
            if (buf.WritableBytes < 5)
                buf = GetNewCmdDataBuf(5);
            buf.WriteByte(iType);
            buf.WriteInt(ReadIndex);
            Packet(0x08, 0x03, 0x00, 5, buf);
            CommandReady();
        }
        #endregion

        #region 产生返回值
        /// <summary>
        /// 产生返回值，并使命令完结
        /// </summary>
        private void CreateResult()
        {
            var rst = new ReadTransactionAndImageDatabase_Result();
            _Result = rst;

            var tDtl = mTransactionDetailList[0];
            rst.readable = (int)tDtl.readable();
            if (_TransactionList != null)
            {
                rst.Quantity = _TransactionList.Count;

                rst.TransactionList = new List<CardAndImageTransaction>(_TransactionList.Values);
            }
            else
            {
                rst.Quantity = 0;

                rst.TransactionList = new List<CardAndImageTransaction>();
            }
            rst.CardTransactionReadIndex = (int)tDtl.ReadIndex;

            if (mTransactionDetailList.Length > 3)
            {
                rst.BodyTemperatureReadIndex = (int)mTransactionDetailList[3].ReadIndex;
            }
            else
            {
                rst.BodyTemperatureReadIndex = 0;
            }


            CommandCompleted();
        }
        #endregion


        #region 读取照片
        private void BeginReadImageFile()
        {
            var oPar = _Parameter as ReadTransactionAndImageDatabase_Parameter;
            if (!oPar.AutoDownloadImage)//不需要读照片
            {
                //读取完毕
                WriteTransactionReadIndex_Card();
                return;
            }

            mStep = 6;

            _ProcessMax = _TransactionList.Count;
            _ProcessStep = 0;
            fireCommandProcessEvent();

            mSaveFileSerialNumber = _TransactionList.Keys.Min();
            mSaveFileSerialNumber = GetDownloadImageIndex(mSaveFileSerialNumber);
            if (mSaveFileSerialNumber == -1)
            {
                //读取完毕
                WriteTransactionReadIndex_Card();
                return;
            }

            _ReadFileCommand = new ReadFileSubCommand(this);
            //Trace.WriteLine($"{_Connector.GetKey()} 开始下载记录照片:{mSaveFileSerialNumber}/{mMaxSerialNum} ");
            _ReadFileCommand.BeginRead(mSaveFileSerialNumber, 3, 1);
            CommandReady();
        }
        /// <summary>
        /// 读取照片完毕
        /// </summary>
        private void ReadImageCallblack()
        {
            if (_ReadFileCommand.IsCommandOver())
            {
                var CardTr = _TransactionList[mSaveFileSerialNumber] as CardAndImageTransaction;
                if (_ReadFileCommand.FileResult)
                {

                    CardTr.SetPhoto(1);
                    CardTr.PhotoSize = _ReadFileCommand.FileSize;

                    if (mSaveImageToFile)
                    {
                        try
                        {
                            string sFile = System.IO.Path.Combine(mImageDir, $"{mSaveFileSerialNumber}.jpg");
                            System.IO.File.WriteAllBytes(sFile, _ReadFileCommand.FileDatas);
                            CardTr.PhotoFile = sFile;
                        }
                        catch (Exception)
                        {
                            CardTr.PhotoFile = string.Empty;
                            CardTr.PhotoDataBuf = _ReadFileCommand.FileDatas;
                        }
                    }
                    else
                    {
                        CardTr.PhotoDataBuf = _ReadFileCommand.FileDatas;
                    }
                }
                else
                {
                    CardTr.SetPhoto(0);
                }

                DownloadImageNext();
            }
            else
            {
                DownloadImageNext();
            }
        }

        /// <summary>
        /// 获取需要下载的图片索引号
        /// </summary>
        /// <param name="iCurrentIndex"></param>
        /// <returns></returns>
        private int GetDownloadImageIndex(int iCurrentIndex)
        {
            var par = _Parameter as ReadTransactionAndImageDatabase_Parameter;

            do
            {
                if (_TransactionList[iCurrentIndex].Photo > 0)
                {
                    if (par.ImageDownloadCheckCallblack != null)
                    {
                        if (par.ImageDownloadCheckCallblack(iCurrentIndex, _TransactionList[iCurrentIndex]))
                        {
                            return iCurrentIndex;
                        }
                    }
                    else
                        return iCurrentIndex;
                }

                iCurrentIndex++;
                if (iCurrentIndex > mCardRecord_ReadIndex)
                {
                    return -1;
                }
            } while (true);


        }

        /// <summary>
        /// 准备下载下一张照片
        /// </summary>
        private void DownloadImageNext()
        {
            mSaveFileSerialNumber++;
            if (mSaveFileSerialNumber > mCardRecord_ReadIndex)
            {
                _ReadFileCommand.Release();
                //读取完毕
                WriteTransactionReadIndex_Card();
                return;
            }
            if (_TransactionList.ContainsKey(mSaveFileSerialNumber))
            {
                mSaveFileSerialNumber = GetDownloadImageIndex(mSaveFileSerialNumber);
                if (mSaveFileSerialNumber == -1)
                {
                    _ReadFileCommand.Release();
                    //读取完毕
                    WriteTransactionReadIndex_Card();
                    return;
                }
                _ProcessStep++;
                fireCommandProcessEvent();

                _ReadFileCommand.BeginRead(mSaveFileSerialNumber, 3, 1);
                CommandReady();
            }
            else
            {
                DownloadImageNext();
            }
        }

        #endregion

        /// <summary>
        /// 命令准备就绪
        /// </summary>
        /// <param name="subCmd"></param>
        public override void SubCommandReady(ISubCommand subCmd)
        {
            if (mStep != 6)
            {
                base.SubCommandReady(subCmd);
            }
            else
            {
                CommandReady();
            }

        }

    }
}
