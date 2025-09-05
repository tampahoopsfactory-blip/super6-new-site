using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 写文件子命令
    /// </summary>
    public class WriteFileSubCommand : BaseSubCommand
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public int UserCode;

        /// <summary>
        /// 文件类型
        /// 1 - 人员头像
        /// 2 - 指纹
        /// 3 - 红外人脸特征码
        /// 4 - 动态人脸特征码
        /// 10 - 开机图片
        /// 11 - 待机图片
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
        public byte[] _FileDatas;

        /// <summary>
        /// CRC32校验数据
        /// </summary>
        public uint FileCRC;

        /// <summary>
        /// 写入结果
        /// 1--校验成功
        /// 0--校验失败
        /// 2--特征码无法识别
        /// 3--人员照片不可识别
        /// 4--人员重复
        /// -1 -- 拒绝写入
        /// </summary>
        public int FileResult;

        /// <summary>
        /// 重复的用户编号
        /// </summary>
        public uint RepeatUser;

        /// <summary>
        /// 如果发生照片重复消息时，是否等待重复详情，适用于人脸机固件版本4.24以上版本
        /// </summary>
        public bool WaitRepeatMessage;
        /// <summary>
        /// 等待校验的时间，单位毫秒
        /// </summary>
        public int WaitVerifyTime;

        /// <summary>
        /// 写索引
        /// </summary>
        private int _WriteIndex = 0;



        /// <summary>
        /// 操作步骤
        /// </summary>
        private int _Step = 0;


        /// <summary>
        /// 创建一个写文件子命令
        /// </summary>
        /// <param name="mainCmd"></param>
        public WriteFileSubCommand(ICombinedCommand mainCmd) : base(mainCmd)
        {
            WaitVerifyTime = 6000;
            WaitRepeatMessage = false;
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        public void BeginWrite(int userCode, int iType, int iNum, byte[] bDatas)
        {
            RepeatUser = 0;
            FileResult = 0;
            FileHandle = 0;
            FileSize = 0;
            FileCRC = 0;
            _IsOver = false;
            _FileDatas = bDatas;

            UserCode = userCode;
            FileType = iType;

            if (FileType == 1 || FileType ==3 || FileType==4) iNum = 1;

            FileNum = iNum;
            ProcessMax = 3;
            ProcessStep = 1;

            BeginWrite();

        }

        private void BeginWrite()
        {
            _Step = 0;
            var dataBuf = mCommand.GetNewSubCmdDataBuf(6);
            dataBuf.WriteInt(UserCode);
            dataBuf.WriteByte(FileType);
            dataBuf.WriteByte(FileNum);
            mCommand.ChangePacket(0x0B, 0x01, 0x00, 6, dataBuf);
        }


        /// <summary>
        ///  命令进行到下一个步骤
        /// </summary>
        /// <param name="accessPacket"></param>
        public override void CommandNext(INPacket accessPacket)
        {
            OnlineAccessPacket oPck = accessPacket as OnlineAccessPacket;
            if (oPck == null) return;
            switch (_Step)
            {
                case 0:
                    //返回文件句柄
                    CheckOpenFileResult(oPck);
                    break;
                case 1:
                    CheckWriteFileResult(oPck);
                    break;
                case 2://上传完毕
                    if (CheckResponse(oPck, 0x0B, 3, 0, 1))
                    {
                        FileResult = oPck.CmdData.ReadByte();
                        if (FileResult == 4)
                        {
                            if (!WaitRepeatMessage)
                            {
                                CommandOver();
                                return;
                            }
                        }
                        else
                        {
                            CommandOver();
                            return;
                        }

                    }
                    //等待接收重复的用户号
                    if (CheckResponse(oPck, 0x0B, 0x03, 1, 4))
                    {
                        uint code = oPck.CmdData.ReadUnsignedInt();
                        RepeatUser = code;
                        CommandOver();
                    }

                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 检查打开文件返回值
        /// </summary>
        private void CheckOpenFileResult(OnlineAccessPacket oPck)
        {
            /// <summary>
            /// 文件句柄
            /// </summary>
            uint _FileHandle = 0;

            if (CheckResponse(oPck, 4))
            {
                var buf = oPck.CmdData;
                _FileHandle = buf.ReadUnsignedInt();
                if (_FileHandle == 0)
                {
                    FileResult = -1;
                    CommandOver();
                    return;
                }
                if (_FileHandle == UInt32.MaxValue)
                {
                    mCommand.GetCommandDetail().Timeout = 150000;
                    //延迟三秒后再试
                    mCommand.GetConnector().GetEventLoop().Schedule(() =>
                    {
                        BeginWrite();
                        CommandReady();
                    },  TimeSpan.FromSeconds(3));
                    return;
                }
                else
                {
                    FileHandle = (int)_FileHandle;
                    var data = _FileDatas;
                    var iPackSize = 1024;
                    if (iPackSize > data.Length) iPackSize = data.Length;
                    ProcessMax = data.Length;
                    ProcessStep = 0;
                    int iBufSize = 7 + iPackSize;
                    var writeBuf = GetNewCmdDataBuf(iBufSize);
                    writeBuf.WriteInt(FileHandle);
                    _WriteIndex = 0;
                    writeBuf.WriteMedium(_WriteIndex);
                    writeBuf.WriteBytes(data, 0, iPackSize);

                    mCommand.ChangePacket(0x0B, 2, 0, (uint)writeBuf.ReadableBytes, writeBuf);
                    _Step = 1;
                    CommandReady();

                }
            }
        }

        /// <summary>
        /// 检查写文件返回值
        /// </summary>
        private void CheckWriteFileResult(OnlineAccessPacket oPck)
        {
            OnlineAccessPacket DoorPacket = GetPacket();
            if (CheckResponse(oPck, 0x0B, 2, 0))
            {
                var data = _FileDatas;
                var iPackSize = 1024;

                _WriteIndex += iPackSize;
                ProcessStep += iPackSize;

                var iFileLen = data.Length;
                var iDataLen = iFileLen - _WriteIndex;
                var buf = GetCmdBuf();
                if (iDataLen > iPackSize) iDataLen = iPackSize;
                if (iDataLen <= 0)
                {
                    ProcessStep = ProcessMax;
                    mCommand.GetCommandDetail().Timeout = WaitVerifyTime;

                    var crc32 = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(data, 0, (uint)data.Length);

                    buf.WriteInt((int)crc32);
                    DoorPacket.CmdIndex = 0x03;
                    DoorPacket.DataLen = 4;
                    _Step = 2;
                }
                else
                {
                    buf.WriteInt(FileHandle);
                    buf.WriteMedium(_WriteIndex);
                    buf.WriteBytes(data, _WriteIndex, iDataLen);
                    DoorPacket.DataLen = buf.ReadableBytes;
                }
                CommandReady();
            }
            else if (CheckResponse(oPck, 0x0B, 2, 2))
            {

                BeginWrite();
                CommandReady();

            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Release()
        {

            base.Release();
            _FileDatas = null;
        }
    }
}
