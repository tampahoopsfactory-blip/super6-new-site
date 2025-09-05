using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 添加人员并上传人员识别信息
    /// </summary>
    public class AddPeosonAndImage : BaseCombinedCommand
    {
        /// <summary>
        /// 添加人员的返回值
        /// </summary>
        AddPersonAndImage_Result mResult;

        /// <summary>
        /// 添加人员的参数
        /// </summary>
        AddPersonAndImage_Parameter mPar;

        /// <summary>
        /// 用户号
        /// </summary>
        private uint UserCode;


        /// <summary>
        /// 文件索引号
        /// </summary>
        private int _FileIndex = 0;

        /// <summary>
        /// 操作步骤
        /// </summary>
        private int _Step = 0;


        /// <summary>
        /// 创建添加人员及识别信息命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par">人员及识别信息</param>
        public AddPeosonAndImage(INCommandDetail cd, AddPersonAndImage_Parameter par) : base(cd, par) { mPar = par; }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            mResult = new AddPersonAndImage_Result(mPar.PersonDetail, mPar.IdentificationDatas);

            UserCode = mPar.PersonDetail.UserCode;
            _Result = mResult;
            DeletePerson(null);
        }
        /// <summary>
        /// 删除人员
        /// </summary>
        private void DeletePerson(OnlineAccessPacket oPck)
        {
            if (oPck == null)
            {
                var MaxBufSize = 4 + 1;
                var buf = GetNewCmdDataBuf(MaxBufSize);
                buf.WriteByte(1);
                buf.WriteInt((int)UserCode);
                Packet(0x07, 0x05, 0x00, (uint)buf.ReadableBytes, buf);
                _Step = 5;
            }
            else
            {
                    AddPerson(null);
            }
        }
        private void AddPerson(OnlineAccessPacket oPck)
        {
            if (oPck == null)
            {
                var dataBuf = GetNewCmdDataBuf(0xA2);
                dataBuf.WriteByte(1);
                mPar.PersonDetail.GetBytes(dataBuf);
                Packet(0x07, 0x04, 0x00, 0xA2, dataBuf);
                _Step = 0;
                CommandReady();
            }
            else
            {
                if (CheckResponse_OK(oPck))
                {
                    mResult.UserUploadStatus = true;
                    WriteImage();
                }
                else if (CheckResponse(oPck, 0x07, 0x04, 0xFF))
                {  //检查是否不是错误返回值
                    mResult.UserUploadStatus = false;
                    CommandCompleted();
                }
            }
        }
        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return (value as AddPersonAndImage_Parameter).checkedParameter();
        }


        /// <summary>
        /// 检查返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {
            switch (_Step)
            {
                case 0://添加人员
                    AddPerson(oPck);
                    break;
                case 1:
                    //开始写文件
                    _WriteSubCommand.CommandNext(oPck);
                    break;
                case 3://上传完毕
                    CheckWriteOver(oPck);
                    break;
                case 5:
                    DeletePerson(oPck);
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 封装的写文件命令
        /// </summary>
        private AdditionalData.WriteFileSubCommand _WriteSubCommand;

        /// <summary>
        /// 写文件
        /// </summary>
        private void WriteImage()
        {
            if ((_FileIndex + 1) > mPar.IdentificationDatas.Count)
            {
                //文件写完了
                CommandCompleted0();
                return;
            }
            if (_WriteSubCommand == null)
            {
                _WriteSubCommand = new AdditionalData.WriteFileSubCommand(this);
                _WriteSubCommand.WaitRepeatMessage = mPar.WaitRepeatMessage;
                _WriteSubCommand.WaitVerifyTime = mPar.WaitVerifyTime;
            }
            var id = mPar.IdentificationDatas[_FileIndex];

            _WriteSubCommand.BeginWrite((int)UserCode, id.DataType, id.DataNum, id.DataBuf);

            CommandReady();
            _Step = 1;
        }


        /// <summary>
        /// 检查文件写入是否完毕
        /// </summary>
        /// <param name="iStatus"></param>
        private void CheckWriteOver(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x0B, 3, 0, 1))
            {
                int iStatus = oPck.CmdData.ReadByte();
                mResult.IdDataUploadStatus[_FileIndex] = iStatus;

                if (iStatus == 4)
                {
                    if (!mPar.WaitRepeatMessage)
                    {
                        WriteNextImage();
                        return;
                    }
                }
                else
                {
                    WriteNextImage();
                    return;
                }
            }

            if (CheckResponse(oPck, 0x0B, 0x03, 1, 4))
            {
                uint code = oPck.CmdData.ReadUnsignedInt();
                mResult.IdDataRepeatUser[_FileIndex] = code;
                WriteNextImage();
            }
        }

        /// <summary>
        /// 命令执行完毕
        /// </summary>
        /// <param name="subCmd"></param>
        public override void SubCommandOver(ISubCommand subCmd)
        {
            var sc = subCmd as AdditionalData.WriteFileSubCommand;
            mResult.IdDataUploadStatus[_FileIndex] = sc.FileResult;
            if (sc.FileResult == 4)
            {
                mResult.IdDataRepeatUser[_FileIndex] = sc.RepeatUser;
            }
            WriteNextImage();
        }



        /// <summary>
        /// 开始写下一个文件
        /// </summary>
        private void WriteNextImage()
        {
            _FileIndex += 1;//开始写下一个
            if ((_FileIndex + 1) > mPar.IdentificationDatas.Count)
            {
                //文件写完了
                CommandCompleted0();
                return;
            }
            WriteImage();
        }

        protected void CommandCompleted0()
        {
            if (_WriteSubCommand != null)
            {
                _WriteSubCommand.Release();
                _WriteSubCommand = null;
            }
            CommandCompleted();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Release1()
        {
            base.Release1();
            mResult = null;
            mPar = null;
            if (_WriteSubCommand != null)
            {
                _WriteSubCommand.Release();
                _WriteSubCommand = null;
            }
        }

    }
}
