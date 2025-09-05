using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.OnlineAccess;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 读取文件状态，封装读取文件的步骤
    /// </summary>
    internal class ReadFileSubCommand : BaseSubCommand
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public int UserCode;

        /// <summary>
        /// 文件类型
        /// 1 - 人员头像
        /// 2 - 指纹
        /// 3 - 记录照片
        /// 4 - 红外人脸特征码
        /// 5 - 动态人脸特征码
        /// </summary>
        public int FileType;

        /// <summary>
        /// 文件序号；只有类型为指纹时，取值范围0-9，其他类型时固定为1
        /// </summary>
        public int FileNum;

        /// <summary>
        /// 文件句柄
        /// </summary>
        public int FileHandle;

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize;

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] FileDatas;

        /// <summary>
        /// CRC32校验数据
        /// </summary>
        public uint FileCRC;

        /// <summary>
        /// 指示命令执行结果
        /// </summary>
        public bool FileResult;


        /// <summary>
        /// 当前读取文件状态的步骤
        /// </summary>
        private int _Step;


        /// <summary>
        /// 创建一个读取文件子命令
        /// </summary>
        /// <param name="mainCmd"></param>
        public ReadFileSubCommand(ICombinedCommand mainCmd) : base(mainCmd)
        {
        }

        /// <summary>
        /// 释放
        /// </summary>
        public override void Release()
        {
            base.Release();
            FileResult = false;
            FileDatas = null;

        }

        /// <summary>
        /// 开始读取文件
        /// </summary>
        public void BeginRead(int userCode, int iType, int iNum)
        {

            UserCode = userCode;
            FileType = iType;

            if (iType != 2) iNum = 1;
            FileNum = iNum;

            BeginRead();
        }

        /// <summary>
        /// 开始读
        /// </summary>
        private void BeginRead()
        {
            FileHandle = 0;
            FileSize = 0;
            FileCRC = 0;
            FileResult = false;
            _IsOver = false;

            _Step = 0;
            ProcessMax = 3;
            ProcessStep = 1;
            var buf = GetNewCmdDataBuf(6);
            buf.WriteByte(FileType);
            buf.WriteByte(FileNum);
            buf.WriteInt(UserCode);
            Packet(0x0B, 0x15, 0x00, 6, buf);
        }



        /// <summary>
        ///  命令进行到下一个步骤
        /// </summary>
        /// <param name="oPck"></param>
        public override void CommandNext(INPacket accessPacket)
        {
            OnlineAccessPacket oPck = accessPacket as OnlineAccessPacket;
            if (oPck == null) return;
            try
            {

                switch (_Step)
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
            catch (Exception ex)
            {

                Trace.WriteLine($"{mCommand.GetConnector().GetKey()} ReadFileSubCommand_CommandNext {UserCode} -- {_Step}:{FileHandle}:{ProcessStep}/{ProcessMax} {ex.Message}");
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
                //解析返回值
                int iFileType = buf.ReadByte();
                int iFileUserCode = buf.ReadInt();
                uint iFileHandle = buf.ReadUnsignedInt();
                int iFileSize = buf.ReadInt();
                if (iFileSize < 0)
                {
                    iFileHandle = 0;
                    iFileSize = 0;
                }
                uint iFileCRC = buf.ReadUnsignedInt();


                if (iFileUserCode != UserCode || FileType != iFileType) return;

                if (iFileHandle == 0)
                {
                    //Trace.WriteLine($"{mCommand.GetConnector().GetKey()} 读文件返回尺寸为长度0 {UserCode},{FileType},{iFileHandle} , {iFileSize}");

                    FileResult = false;
                    CommandOver();
                    return;
                }
                if (iFileSize == 0)
                {
                    FileResult = false;
                    CommandOver();
                    return;
                }
                if (iFileHandle == uint.MaxValue)
                {
                    mCommand.GetCommandDetail().Timeout = 150000;
                    //延迟三秒后再试
                    mCommand.GetConnector().GetEventLoop().Schedule(() =>
                    {
                        BeginRead();
                        CommandReady();
                    }, TimeSpan.FromSeconds(2));
                    return;
                }
                else
                {
                    ProcessMax = iFileSize;

                    FileSize = iFileSize;
                    FileHandle = (int)iFileHandle;
                    FileCRC = iFileCRC;


                    FileDatas = new byte[FileSize];

                    //开始读文件块
                    _Step = 1;

                    var iPackSize = 1024;
                    if (iPackSize > FileSize) iPackSize = FileSize;

                    var readBuf = GetNewCmdDataBuf(10);
                    readBuf.WriteInt(FileHandle);
                    readBuf.WriteInt(0);
                    readBuf.WriteShort(iPackSize);

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

                buf.ReadBytes(FileDatas, iDataIndex, iSize);

                var mycrc = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(FileDatas, (uint)iDataIndex, (uint)iSize);

                if (crc == mycrc)
                {
                    //校验通过，读取下一包
                    var iPackSize = 1024;

                    iDataIndex += iPackSize;
                    ProcessStep = iDataIndex;

                    var iDataLen = FileSize - iDataIndex;
                    buf = GetCmdBuf();
                    if (iDataLen > iPackSize) iDataLen = iPackSize;
                    buf.WriteInt(FileHandle);

                    if (iDataLen <= 0)
                    {
                        ProcessStep = ProcessMax;

                        //全部文件读取完毕
                        OnlineAccessPacket DoorPacket = GetPacket();
                        DoorPacket.CmdPar = 3;
                        DoorPacket.DataLen = 4;
                        _Step = 2;
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

                var crc32 = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(FileDatas, 0, (uint)FileDatas.Length);
                FileResult = (FileCRC == crc32);

                if (!FileResult)
                {
                    FileDatas = null;

                    //校验不通过，重新读取
                    BeginRead(UserCode, FileType, FileNum);
                    CommandReady();
                    return;
                }

                CommandOver();
            }
        }


    }
}
