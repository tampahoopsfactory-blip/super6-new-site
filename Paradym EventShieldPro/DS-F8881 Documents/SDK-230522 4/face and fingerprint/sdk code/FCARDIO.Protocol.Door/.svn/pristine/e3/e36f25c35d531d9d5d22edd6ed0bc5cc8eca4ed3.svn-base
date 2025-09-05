using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    public abstract class WritePersonBase : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 当前命令进度
        /// </summary>
        protected int mStep = 0;
        /// <summary>
        /// 指示当前命令进行的步骤
        /// </summary>
        protected int mWriteIndex = 0;

        /// <summary>
        /// 保存写入失败的数据缓冲区
        /// </summary>
        protected Queue<IByteBuffer> mBufs = null;

        /// <summary>
        /// 指令分类
        /// </summary>
        protected byte CmdPar;

        /// <summary>
        /// 每一包中最大的卡数量
        /// </summary>
        protected int mPacketMax;
        /// <summary>
        /// 默认的缓冲区大小
        /// </summary>
        protected int MaxBufSize = 350;

        /// <summary>
        /// 保存待上传卡列表的参数
        /// </summary>
        protected WritePerson_ParameterBase _mPar = null;

        public WritePersonBase(INCommandDetail cd, WritePerson_ParameterBase parameter) : base(cd, parameter)
        {
            _mPar = parameter;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            _mPar = value as WritePerson_ParameterBase;
            if (_mPar == null) return false;
            return _mPar.checkedParameter();
        }

        /// <summary>
        /// 将人员详情写入到ByteBuf中
        /// </summary>
        protected virtual void WritePersonToBuf(IByteBuffer buf)
        {
            var lst = _mPar.PersonList;
            int iCount = lst.Count;//获取列表总长度
            iCount = iCount - mWriteIndex;//计算未上传总数

            int iLen = iCount;
            if (iLen > mPacketMax)
            {
                iLen = mPacketMax;
            }
            buf.Clear();

            buf.WriteByte(iLen);

            for (int i = 0; i < iLen; i++)
            {
                var person = lst[mWriteIndex + i];
                WritePersonToBuf(person, buf);
            }

            mWriteIndex += iLen;
            _ProcessStep += iLen;
        }

        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="card">卡详情</param>
        /// <param name="buf">缓冲区</param>
        protected abstract void WritePersonToBuf(Data.Person person, IByteBuffer buf);


        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (IsWriteOver())
            {
                Create_Result();
                CommandCompleted();
            }
            else
            {
                //未发送完毕，继续发送
                var buf = GetCmdBuf();
                WritePersonToBuf(buf);
                DoorPacket.DataLen = buf.ReadableBytes;
                CommandReady();//设定命令当前状态为准备就绪，等待发送
            }
        }

        /// <summary>
        /// 创建命令成功返回值
        /// </summary>
        protected virtual void Create_Result()
        {
            //无法写入的人员数量
            int FailTotal = 0;

            //无法写入的人员列表
            List<uint> list = new List<uint>();

            if (mBufs != null)
            {
                foreach (var buf in mBufs)
                {
                    int iCount = buf.ReadInt();
                    FailTotal += iCount;

                    for (int i = 0; i < iCount; i++)
                    {
                        ReadPasswordByFailBuf(list, buf);
                    }

                    buf.Release();
                }
               
            }
             _Result = new WritePerson_Result(list);
        }


        /// <summary>
        /// 从错误密码列表中读取一个错误密码，加入到passwordList中
        /// </summary>
        /// <param name="personList">错误人员列表</param>
        /// <param name="buf"></param>
        private void ReadPasswordByFailBuf(List<uint> personList, IByteBuffer buf)
        {
            personList.Add(buf.ReadUnsignedInt());
        }

        /// <summary>
        /// 重写父类对处理返回值的定义
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {
            if (CheckResponse_OK(oPck))
            {

                //继续发下一包
                CommandNext1(oPck);
            }
            else if (CheckResponse(oPck, 0x07, 0x04, 0xFF))
            {//检查是否不是错误返回值

                //缓存错误返回值
                if (mBufs == null)
                {
                    mBufs = new Queue<IByteBuffer>();
                }
                oPck.CmdData.Retain();
                mBufs.Enqueue(oPck.CmdData);

                //继续发下一包
                CommandNext1(oPck);
            }
        }

        /// <summary>
        /// 检查是否已写完所有卡
        /// </summary>
        /// <returns></returns>
        protected bool IsWriteOver()
        {
            int iCount = _mPar.PersonList.Count;//获取列表总长度

            return (iCount - mWriteIndex) == 0;
        }
    }
}