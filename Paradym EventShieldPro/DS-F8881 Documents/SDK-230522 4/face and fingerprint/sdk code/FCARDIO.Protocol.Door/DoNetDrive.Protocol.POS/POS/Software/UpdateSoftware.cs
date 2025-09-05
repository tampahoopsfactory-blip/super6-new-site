using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Core.Extension;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using System.Diagnostics;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.Software
{
    /// <summary>
    /// 固件升级
    /// </summary>
    public class UpdateSoftware : Write_Command
    {
        /// <summary>
        /// 上传固件的返回结果
        /// </summary>
        UpdateSoftware_Result mResult;
        UpdateSoftware_Parameter mPar;

        /// <summary>
        /// 写索引
        /// </summary>
        private int _WriteIndex = 0;

        /// <summary>
        /// 操作步骤
        /// </summary>
        private int _Step = 0;


        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public UpdateSoftware(DESDriveCommandDetail cd, UpdateSoftware_Parameter par) : base(cd, par)
        {
            mPar = par;
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var dataBuf = GetNewCmdDataBuf(4);
            dataBuf.WriteInt(mPar.GetDataLen());
            Packet(0x0A, 0x11, 0x00, 4, dataBuf);
            _Step = 0;

            mResult = new UpdateSoftware_Result();
            _Result = mResult;
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            UpdateSoftware_Parameter p = value as UpdateSoftware_Parameter;
            if (p == null)
            {
                return false;
            }
            return p.checkedParameter();
        }


        /// <summary>
        /// 检查返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(DESPacket oPck)
        {
            switch (_Step)
            {
                case 0://准备写固件的返回值
                    //应答OK
                    CheckOKResult(oPck);
                    break;
                case 1:
                    CheckWriteFileResult(oPck);
                    break;
                case 2://上传完毕
                    if (CheckResponse(oPck, 0x0A, 0x13, 0, 1))
                    {
                        mResult.Success = oPck.CmdData.ReadByte();
                        Trace.WriteLine($"{DateTime.Now:mm:ss.fff} 升级包完毕，返回值：{mResult.Success}！");

                        
                        CommandCompleted();
                    }
                    break;
                default:
                    break;
            }




        }
        /// <summary>
        /// 检查打开文件返回值
        /// </summary>
        private void CheckOKResult(DESPacket oPck)
        {
            if (CheckResponse_OK(oPck))
            {
                var data = mPar.Datas;
                var iPackSize = 1024;
                if (iPackSize > data.Length) iPackSize = data.Length;
                _ProcessMax = data.Length;
                _ProcessStep = 0;
                int iBufSize = 4 + iPackSize;
                var writeBuf = GetNewCmdDataBuf(iBufSize);
                _WriteIndex = 0;
                writeBuf.WriteInt(_WriteIndex);
                writeBuf.WriteBytes(data, 0, iPackSize);

                Packet(0x0A, 0x12, 0, (uint)writeBuf.ReadableBytes, writeBuf);
                _Step = 1;
                CommandReady();
            }
        }

        /// <summary>
        /// 检查写文件返回值
        /// </summary>
        private void CheckWriteFileResult(DESPacket oPck)
        {
            if (CheckResponse(oPck, 0x0A, 0x12, 0))
            {
                var data = mPar.Datas;
                var iPackSize = 1024;

                _WriteIndex += iPackSize;
                _ProcessStep += iPackSize;

                var iFileLen = data.Length;
                var iDataLen = iFileLen - _WriteIndex;
                var buf = GetCmdBuf();
                if (iDataLen > iPackSize) iDataLen = iPackSize;
                if (iDataLen <= 0)
                {
                    
                    _ProcessStep = _ProcessMax;
                    var crc32 = mPar.SoftwareCRC32;

                    buf.WriteInt((int)crc32);
                    FPacket.CommandPacket.CmdIndex = 0x13;
                    FPacket.CommandPacket.DataLen = 4;
                    _Step = 2;
                }
                else
                {
                    //Trace.WriteLine($"{DateTime.Now:mm:ss.fff} 发送包索引：{_WriteIndex}");

                    buf.WriteInt(_WriteIndex);
                    buf.WriteBytes(data, _WriteIndex, iDataLen);
                    FPacket.CommandPacket.DataLen = buf.ReadableBytes;
                }
                CommandReady();
            }
            else if (CheckResponse(oPck, 0x0A, 0x12, 2))
            {
                mResult.Success = 255;
                CommandCompleted();
            }
        }
    }
}
