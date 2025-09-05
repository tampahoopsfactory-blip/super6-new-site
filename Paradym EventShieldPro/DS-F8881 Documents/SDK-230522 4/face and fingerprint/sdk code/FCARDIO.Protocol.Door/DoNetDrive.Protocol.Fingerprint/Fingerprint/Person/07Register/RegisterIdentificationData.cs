using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 注册人员指纹特征码或注册人员头像
    /// </summary>
    public class RegisterIdentificationData : Door8800Command_ReadParameter
    {
        private int mStep;
        private uint UserCode;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="detail"></param>
        public RegisterIdentificationData(INCommandDetail detail, RegisterIdentificationData_Parameter par)
            : base(detail, par)
        {
            var res = new RegisterIdentificationData_Result(par.PersonDetail, par.DataType, par.DataNum);
            _Result = res;
            UserCode = par.PersonDetail.UserCode;

            _ProcessMax = 6;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return (value as RegisterIdentificationData_Parameter).checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            var buf = GetNewCmdDataBuf(162);
            buf.WriteByte(1);
            var par = _Parameter as RegisterIdentificationData_Parameter;
            var per = par.PersonDetail;
            per.GetBytes(buf);
            mStep = 1;
            Packet(0x07, 0x04, 0x00, (uint)buf.ReadableBytes, buf);
        }

        protected override void CommandNext1(OnlineAccessPacket oPck) { }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {
            var par = _Parameter as RegisterIdentificationData_Parameter;

            switch (mStep)
            {
                case 1://添加人员返回值处理
                    if (CheckResponse_OK(oPck))
                    {
                        if (par.DataType < 4)
                        {
                            //需要先删除
                            DeleteFile();
                        }
                        else
                        {
                            BeginReg();
                        }

                    }
                    if (CheckResponse(oPck, 0x37, 0x04, 0xFF))
                    {
                        var res = _Result as RegisterIdentificationData_Result;
                        res.Status = 5;
                        //添加人员失败
                        CommandCompleted();
                    }
                    break;
                case 2://开始注册
                    if (CheckResponse_OK(oPck))
                    {
                        BeginReg();
                    }
                    break;
                case 3://注册命令反馈
                    if (CheckResponse(oPck, 1))
                    {
                        var iStatus = oPck.CmdData.ReadByte();
                        var res = _Result as RegisterIdentificationData_Result;
                        res.Status = iStatus;
                        if (iStatus == 1)
                        {
                            _ProcessStep = 4;
                            fireCommandProcessEvent();
                            _Connector.fireCommandCompleteEventNotRemoveCommand(_EventArgs);
                            //修改超时等待时间
                            CommandDetail.Timeout = 15000;
                            mStep = 4;
                            CommandWaitResponse();
                        }
                        else
                        {
                            //注册失败
                            CommandCompleted();
                        }
                    }
                    break;
                case 4://等待注册结果返回
                    if (oPck.CmdType == 0x19 && oPck.CmdIndex == 0xF0 && oPck.CmdPar == 0)
                    {
                        var iStatus = oPck.CmdData.ReadByte();
                        var res = _Result as RegisterIdentificationData_Result;
                        res.Status = 100 + iStatus;
                        if (iStatus == 1)
                        {
                            _Connector.GetEventLoop().Schedule(() =>
                            {
                                ReadIdentificationData();
                            },  TimeSpan.FromMilliseconds(500));

                            
                        }
                        else
                        {
                            if (iStatus == 3)
                            {
                                mStep = 7;//等待返回重复用户号
                                CommandWaitResponse();
                                return;
                            }

                            //注册失败
                            CommandCompleted();
                        }
                    }
                    break;
                case 5://读取人员详情
                    ReadPersonDetail(oPck);
                    break;
                case 6://读取文件
                    ReadFile(oPck);
                    break;
                case 7://等待重复用户号
                    if (oPck.CmdType == 0x19 && oPck.CmdIndex == 0xF0 && oPck.CmdPar == 1)
                    {
                        uint iUser = oPck.CmdData.ReadUnsignedInt();
                        var res = _Result as RegisterIdentificationData_Result;
                        res.UserID = iUser;
                       
                        //注册失败
                        CommandCompleted();
                    }
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 删除文件
        /// </summary>
        protected void DeleteFile()
        {
            var par = _Parameter as RegisterIdentificationData_Parameter;

            var buf = GetNewCmdDataBuf(20);
            buf.WriteInt((int)UserCode);

            buf.WriteByte(par.DataType == 3 ? 1 : 0);
            buf.WriteInt(0);

            for (int i = 0; i < 10; i++)
            {
                int iValue = 0;
                if (par.DataType == 1)
                {
                    iValue = par.DataNum == i ? 1 : 0;
                }
                buf.WriteByte(0);
            }

            buf.WriteByte(par.DataType == 2 ? 1 : 0);

            mStep = 2;
            _ProcessStep = 2;

            Packet(0x0B, 0x06, 0x00, (uint)buf.ReadableBytes, buf);

            fireCommandProcessEvent();

            CommandReady();//设定命令当前状态为准备就绪，等待发送
        }


        /// <summary>
        /// 开始注册信息
        /// </summary>
        protected void BeginReg()
        {
            var par = _Parameter as RegisterIdentificationData_Parameter;

            var buf = GetNewCmdDataBuf(6);

            par.GetBytes(buf);
            mStep = 3;
            Packet(0x07, 0x20, 0x00, (uint)buf.ReadableBytes, buf);
            _ProcessStep = 3;
            fireCommandProcessEvent();
            CommandDetail.Timeout = 4000;
            CommandDetail.RestartCount = 0;
            CommandReady();//设定命令当前状态为准备就绪，等待发送
        }

        /// <summary>
        /// 读取人员识别信息
        /// </summary>
        protected void ReadIdentificationData()
        {
            var par = _Parameter as RegisterIdentificationData_Parameter;
            _ProcessStep = 5;
            fireCommandProcessEvent();



            //开始读取资料
            if (par.DataType == 4 || par.DataType == 5)
            {
                //读取人员详情
                CommandDetail.Timeout = 3000;
                CommandDetail.RestartCount = 5;
                mStep = 5;

                var buf = GetNewCmdDataBuf(4);
                buf.WriteInt((int)UserCode);
                Packet(0x07, 3, 1, 4, buf);
                CommandReady();//设定命令当前状态为准备就绪，等待发送

            }
            else
            {
                //读取文件
                CommandDetail.Timeout = 800;
                CommandDetail.RestartCount = 5;
                mStep = 6;
                mReadFileResult = new AdditionalData.ReadFile_Result();

                byte[] bDownloadTable = new byte[10];
                bDownloadTable[1] = 2;//指纹
                bDownloadTable[2] = 4;//红外人脸特征码
                bDownloadTable[3] = 1;//人员头像
                FileType = bDownloadTable[par.DataType];
                var buf = GetCmdBuf();
                buf.WriteByte(FileType);
                buf.WriteByte(par.DataNum);
                buf.WriteInt((int)UserCode);

                Packet(0x0B, 0x15, 0, 6, buf);
                CommandReady();//设定命令当前状态为准备就绪，等待发送
            }

        }

        /// <summary>
        /// 读取人员详情
        /// </summary>
        /// <param name="oPck"></param>
        protected void ReadPersonDetail(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0xA1))
            {

                var res = _Result as RegisterIdentificationData_Result;
                var par = res.PersonDetail;
                par.SetBytes(oPck.CmdData);

                CommandCompleted();
            }
        }




        #region 读取文件

        /// <summary>
        /// 读取到的文件块缓冲区
        /// </summary>
        private byte[] _FileDatas;

        /// <summary>
        /// 文件类型
        /// </summary>
        private int FileType;

        /// <summary>
        /// 执行步骤
        /// </summary>
        private int _FileStep = 0;

        /// <summary>
        /// 返回值
        /// </summary>
        AdditionalData.ReadFile_Result mReadFileResult;


        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="oPck"></param>
        protected void ReadFile(OnlineAccessPacket oPck)
        {
            switch (_FileStep)
            {
                case 0://读取文件句柄
                    CheckOpenFileResule(oPck);
                    break;
                case 1://读文件块
                    CheckReadFileBlockResule(oPck);
                    break;
                case 2://关闭文件
                    CheckReadFile(oPck);
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        /// 检查打开文件的返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected virtual void CheckOpenFileResule(OnlineAccessPacket oPck)
        {
            //读取文件句柄
            if (CheckResponse(oPck, 0x0B, 0x15, 1, 17))
            {
                var buf = oPck.CmdData;

                mReadFileResult.SetBytes(buf);
                if (mReadFileResult.UserCode != UserCode || mReadFileResult.FileType != FileType) return;

                if (mReadFileResult.FileHandle == 0 || mReadFileResult.FileSize == 0)
                {
                    CommandCompleted();
                    return;
                }
                else
                {
                    if (mReadFileResult.FileHandle > 0 && mReadFileResult.FileSize > 0)
                    {
                        _FileDatas = new byte[mReadFileResult.FileSize];
                        _ProcessMax = mReadFileResult.FileSize;
                    }
                    else
                    {
                        CommandCompleted();
                        return;
                    }
                    //开始读文件块
                    _FileStep = 1;

                    _ProcessStep = 0;
                    var iPackSize = 1024;
                    if (iPackSize > mReadFileResult.FileSize) iPackSize = mReadFileResult.FileSize;
                    _ProcessMax = mReadFileResult.FileSize;
                    var readBuf = GetNewCmdDataBuf(10);
                    readBuf.WriteInt(mReadFileResult.FileHandle);
                    readBuf.WriteInt(0);
                    readBuf.WriteShort(iPackSize);
                    fireCommandProcessEvent();
                    Packet(0x0B, 0x15, 2, (uint)readBuf.ReadableBytes, readBuf);

                    CommandReady();
                    return;
                }
            }
        }

        /// <summary>
        /// 接收读取到的文件块
        /// </summary>
        /// <param name="oPck"></param>
        protected virtual void CheckReadFileBlockResule(OnlineAccessPacket oPck)
        {
            //读取文件块返回值
            if (CheckResponse(oPck, 0x0B, 0x15, 2))
            {
                int FileHandle;
                int iDataIndex;
                int iSize = 0;


                var buf = oPck.CmdData;
                FileHandle = buf.ReadInt();
                iDataIndex = buf.ReadInt();
                iSize = buf.ReadUnsignedShort();
                uint crc = buf.ReadUnsignedInt();

                _ProcessStep = iDataIndex + iSize;
                fireCommandProcessEvent();

                buf.ReadBytes(_FileDatas, iDataIndex, iSize);

                var mycrc = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(_FileDatas, (uint)iDataIndex, (uint)iSize);

                if (crc == mycrc)
                {
                    //校验通过，读取下一包
                    var iPackSize = 1024;

                    iDataIndex += iPackSize;

                    var iDataLen = mReadFileResult.FileSize - iDataIndex;
                    buf = GetCmdBuf();
                    if (iDataLen > iPackSize) iDataLen = iPackSize;
                    buf.WriteInt(mReadFileResult.FileHandle);

                    if (iDataLen <= 0)
                    {   //全部文件读取完毕
                        _ProcessStep = _ProcessMax;
                        DoorPacket.CmdPar = 3;
                        DoorPacket.DataLen = 4;
                        _FileStep = 2;
                    }
                    else
                    {
                        buf.WriteInt(iDataIndex);
                        buf.WriteShort(iDataLen);
                    }


                }
                else
                {
                    //校验错误，重新读取

                }

                CommandReady();
                return;
            }
        }

        /// <summary>
        /// 读取文件完毕，检验CRC32
        /// </summary>
        /// <param name="oPck"></param>
        protected virtual void CheckReadFile(OnlineAccessPacket oPck)
        {
            //读取文件块返回值
            if (CheckResponse(oPck, 0x0B, 0x15, 3))
            {

                var crc32 = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(_FileDatas, 0, (uint)_FileDatas.Length);
                mReadFileResult.Result = (mReadFileResult.FileCRC == crc32);
                _ProcessStep = _ProcessMax;

                if (mReadFileResult.Result)
                {
                    var res = _Result as RegisterIdentificationData_Result;



                    int[] bTypeTable = new int[10];
                    bTypeTable[1] = 2;//指纹
                    bTypeTable[2] = 3;//红外人脸特征码
                    bTypeTable[3] = 1;//人员头像
                    int iType = bTypeTable[res.DataType];

                    res.ResultData = new IdentificationData(iType, res.DataNum, _FileDatas);
                }
                _FileDatas = null;
                CommandCompleted();
            }
        }

        #endregion

        /// <summary>
        /// 命令重发时需要处理的函数
        /// </summary>
        protected override void CommandReSend()
        {
            return;
        }

        /// <summary>
        /// 命令释放时需要处理的函数
        /// </summary>
        protected override void Release1()
        {
            mReadFileResult?.Dispose();
            mReadFileResult = null;
            _FileDatas = null;

            return;
        }
    }
}
