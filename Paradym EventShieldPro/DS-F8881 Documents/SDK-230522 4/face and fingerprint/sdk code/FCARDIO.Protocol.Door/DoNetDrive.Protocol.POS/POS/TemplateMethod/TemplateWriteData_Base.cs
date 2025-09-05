using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.TemplateMethod
{
    /// <summary>
    /// 写入元素命令
    /// </summary>
    public abstract class TemplateWriteData_Base<T, D> : Write_Command where T : TemplateParameter_Base<D>, new() where D : TemplateData_Base, new()
    {



        /// <summary>
        /// 参数
        /// </summary>
        protected T mPar;


        /// <summary>
        /// 已上传数量
        /// </summary>
        protected int mIndex;

        /// <summary>
        /// 默认的缓冲区大小
        /// </summary>
        protected virtual int MaxBufSize
        {
            get
            {
                return GetMaxBufSize();
            }
        }

        /// <summary>
        /// 需要写入密码数
        /// </summary>
        protected int maxCount = 0;

        /// <summary>
        /// 保存写入失败的数据缓冲区
        /// </summary>
        protected Queue<IByteBuffer> mBufs = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public TemplateWriteData_Base(DESDriveCommandDetail cd, T parameter) : base(cd, parameter)
        {
            mPar = parameter;
            //T model = new T();
            //mParDataLen = model.GetDataLen();
            //mDeleteDataLen = model.GetDeleteDataLen();

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            maxCount = mPar.DataList.Count;
            CreateCommandPacket0();
            _ProcessMax = maxCount;
        }

        /// <summary>
        /// 继承类具体实现
        /// </summary>
        protected abstract void CreateCommandPacket0();

        /// <summary>
        /// 继承类具体实现
        /// </summary>
        protected abstract void CreateCommandNextPacket(IByteBuffer buf);

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            T model = value as T;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public IByteBuffer WriteDataToBuf(IByteBuffer databuf)
        {

            var lst = mPar.DataList;
            int iCount = lst.Count;//获取列表总长度
            iCount = iCount - mIndex;//计算未上传总数

            if (iCount > GetBatchCount())
            {
                iCount = GetBatchCount();
            }

            databuf.Clear();

            databuf.WriteByte(iCount);
            for (int i = 0; i < iCount; i++)
            {
                WriteDataBodyToBuf(databuf, lst[mIndex + i]);
            }

            mIndex += iCount;
            _ProcessStep += iCount;
            return databuf;
        }

        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="databuf"></param>
        /// <param name="data">要写入到缓冲区的元素</param>
        protected abstract void WriteDataBodyToBuf(IByteBuffer databuf, TemplateData_Base data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public virtual int GetBatchCount()
        {
            return 5;
        }

        /// <summary>
        /// 获取一次命令发送数据的最大长度
        /// </summary>
        /// <param name="count"></param>
        public virtual int GetMaxBufSize()
        {
            return (GetBatchCount() * mPar.GetDataLen()) + 1;
        }
        /**/
        /// <summary>
        /// 命令超时
        /// </summary>
        protected override void CommandReSend()
        {
            //mIndex -= mBatchCount;
            //var buf = GetCmdBuf();
            //WritePasswordToBuf(buf);
            //DoorPacket.DataLen = (UInt32)buf.ReadableBytes;
            //CommandReady();//设定命令当前状态为准备就绪，等待发送
        }

        protected virtual void SendCommandCompleted()
        {
            CommandCompleted();
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (IsWriteOver())
            {
                Create_Result();
                SendCommandCompleted();
            }
            else
            {

                //未发送完毕，继续发送
                /*
                var lst = mPar.DataList;
                int iCount = lst.Count;//获取列表总长度
                iCount = iCount - mIndex;//计算未上传总数

                if (iCount > GetBatchCount())
                {
                    iCount = GetBatchCount();
                }
                D par = new D();
                var bufSize = (iCount * par.GetDataLen()) + 1;
                var buf = GetNewCmdDataBuf(bufSize);
                WriteDataToBuf(buf);

                CreateCommandNextPacket(buf);
                */
                /**/
                var buf = GetCmdBuf();
                WriteDataToBuf(buf);
                FPacket.CommandPacket.DataLen = buf.ReadableBytes;
                //FPacket.DataLen = buf.ReadableBytes;
                //oPck.SetPacketCmdData(buf.ReadableBytes, buf);
                //SetPacketCmdData()
                CommandReady();//设定命令当前状态为准备就绪，等待发送
                
            }
        }

        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected abstract bool CheckResponseCompleted(DESPacket oPck);

        /// <summary>
        /// 重写父类对处理返回值的定义
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(DESPacket oPck)
        {
            if (CheckResponse_OK(oPck))
            {
                //继续发下一包
                CommandNext1(oPck);
            }
            else if (CheckResponseCompleted(oPck))
            {//检查是否不是错误返回值

                //缓存错误返回值
                if (mBufs == null)
                {
                    mBufs = new Queue<IByteBuffer>();
                }
                oPck.CommandPacket.CmdData.Retain();
                mBufs.Enqueue(oPck.CommandPacket.CmdData);

                //继续发下一包
                CommandNext1(oPck);
            }
        }

        /// <summary>
        /// 创建命令成功返回值
        /// </summary>
        protected virtual void Create_Result()
        {
            //无法写入的密码数量
            int FailTotal = 0;

            //无法写入的密码列表
            List<D> list = new List<D>();


            if (mBufs != null)
            {
                foreach (var buf in mBufs)
                {
                    int iCount = buf.ReadByte();
                    FailTotal += iCount;

                    for (int i = 0; i < iCount; i++)
                    {
                        ReadDataByFailBuf(list, buf);
                    }

                    buf.Release();
                }
                TemplateResult_Base result = CreateResult(list);
                _Result = result;
            }
        }

        /// <summary>
        /// 创建返回值
        /// </summary>
        /// <param name="passwordList">控制器返回的密码集合</param>
        protected abstract TemplateResult_Base CreateResult(List<D> list);

        /// <summary>
        /// 用来解析返回的错误元素数据
        /// </summary>
        D _Data;

        /// <summary>
        /// 从错误密码列表中读取一个错误密码，加入到passwordList中
        /// </summary>
        /// <param name="DataList">错误密码列表</param>
        /// <param name="buf"></param>
        private void ReadDataByFailBuf(List<D> DataList, IByteBuffer buf)
        {
            if (_Data == null) _Data = new D();
            _Data.SetBytes(buf);
            DataList.Add(_Data);
        }

        /// <summary>
        /// 检查是否已写完所有卡
        /// </summary>
        /// <returns></returns>
        protected bool IsWriteOver()
        {
            int iCount = mPar.DataList.Count;//获取列表总长度

            return (iCount - mIndex) == 0;
        }
    }
}
