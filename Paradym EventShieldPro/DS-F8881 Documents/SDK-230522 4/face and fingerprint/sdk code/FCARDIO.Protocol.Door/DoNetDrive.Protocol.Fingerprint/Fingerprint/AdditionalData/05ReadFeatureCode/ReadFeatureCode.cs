using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 读取指纹
    /// </summary>
    public class ReadFeatureCode : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 返回值
        /// </summary>
        ReadFeatureCode_Result mResult;

        /// <summary>
        /// 参数
        /// </summary>
        ReadFeatureCode_Parameter mPar;


        private byte[] _FileDatas;

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public ReadFeatureCode(INCommandDetail cd, ReadFeatureCode_Parameter par) : base(cd, par) { mPar = par; }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            mResult = new ReadFeatureCode_Result();
            _Result = mResult;
            ReadFeatureCode_Parameter model = _Parameter as ReadFeatureCode_Parameter;
            Packet(0x0B, 0x05, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadFeatureCode_Parameter model = value as ReadFeatureCode_Parameter;
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

            //读取文件句柄
            if (CheckResponse(oPck, 0x0B, 5, 1, 12))
            {
                var buf = oPck.CmdData;

                mResult.SetBytes(buf);
                if (mResult.UserCode != mPar.UserCode || mResult.FileType != mPar.Type) return;

                if (mResult.FileHandle == 0 || mResult.FileSize == 0)
                {
                    mResult.Result = false;
                    CommandCompleted();
                    return;
                }
                else
                {
                    if (mResult.FileHandle > 0 && mResult.FileSize > 0)
                    {
                        _FileDatas = new byte[mResult.FileSize];
                        _ProcessMax = mResult.FileSize;
                    }
                    else
                    {
                        CommandCompleted();
                        return;
                    }

                    CommandWaitResponse();
                    return;
                }
            }

            if (CheckResponse(oPck, 0x0B, 5, 2))
            {
                int FileHandle;
                int iDataIndex;
                int iSize = 0;
                iSize = oPck.DataLen - 7;
                var buf = oPck.CmdData;
                FileHandle = buf.ReadInt();
                iDataIndex = buf.ReadMedium();
                _ProcessStep = iDataIndex + iSize;
                if (_FileDatas == null || mResult.FileSize == 0)
                {
                    //_FileDatas = new byte[mResult.FileSize];
                    //可能有丢包，命令需要重发
                    //CommandCompleted();
                    CommandDetail.Timeout += 2000;
                    //延迟三秒后再试
                    _Connector.GetEventLoop().Schedule(() =>
                    {
                        CommandReady();
                    }, TimeSpan.FromSeconds(1));
                    return;
                }
                buf.ReadBytes(_FileDatas, iDataIndex, iSize);
                CommandWaitResponse();
                return;
            }
            if (CheckResponse(oPck, 0x0B, 5, 3, 4))
            {
                if (_FileDatas == null || mResult.FileSize == 0) return;

                var buf = oPck.CmdData;
                uint ReadCRC32 = buf.ReadUnsignedInt();
                var crc32 = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(_FileDatas, 0, (uint)_FileDatas.Length);
                mResult.FileCRC = ReadCRC32;
                _ProcessStep = _ProcessMax;

                if (crc32 == ReadCRC32)
                {
                    mResult.FileDatas = _FileDatas;
                    mResult.Result = true;
                }
                _FileDatas = null;
                CommandCompleted();
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Release1()
        {
            mResult = null;
            mPar = null;
            _FileDatas = null;

        }

    }
}
