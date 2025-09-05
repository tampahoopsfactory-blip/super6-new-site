using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.TemplateMethod
{
    /// <summary>
    /// 写入元素命令
    /// </summary>
    public abstract class TemplateWriteData_Base<T> : Door8800Command_WriteParameter where T : TemplateData_Base,new ()
    {
       

        /// <summary>
        /// 1个写入参数长度
        /// </summary>
        protected int mParDataLen;

        /// <summary>
        /// 1个删除参数长度
        /// </summary>
        protected int mDeleteDataLen;
        /// <summary>
        /// 参数
        /// </summary>
        TemplateParameter_Base mPar;

        /// <summary>
        /// 每次上传数量
        /// </summary>
        protected int mBatchCount = 5;

        /// <summary>
        /// 已上传数量
        /// </summary>
        protected int mIndex;

        /// <summary>
        /// 默认的缓冲区大小
        /// </summary>
        protected int MaxBufSize = 350;

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
        public TemplateWriteData_Base(INCommandDetail cd, TemplateParameter_Base parameter) : base(cd, parameter)
        {
            mPar = parameter;
            T model = new T();
            mParDataLen = model.GetDataLen();
            mDeleteDataLen = model.GetDeleteDataLen();
         
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
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            TemplateParameter_Base model = value as TemplateParameter_Base;
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

            int iLen = iCount;
            if (iLen > mBatchCount)
            {
                iLen = mBatchCount;
            }

            databuf.Clear();

            databuf.WriteInt(iLen);
            for (int i = 0; i < iLen; i++)
            {
                WriteDataBodyToBuf(databuf, lst[mIndex + i]);
            }

            mIndex += iLen;
            _ProcessStep += iLen;
            return databuf;
        }

        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="databuf"></param>
        /// <param name="data">要写入到缓冲区的元素</param>
        protected abstract void WriteDataBodyToBuf(IByteBuffer databuf, TemplateData_Base data);

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
                WriteDataToBuf(buf);
                DoorPacket.DataLen = buf.ReadableBytes;
                CommandReady();//设定命令当前状态为准备就绪，等待发送
            }
        }

        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected abstract bool CheckResponseCompleted(OnlineAccessPacket oPck);

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
            else if (CheckResponseCompleted(oPck))
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
        /// 创建命令成功返回值
        /// </summary>
        protected virtual void Create_Result()
        {
            //无法写入的密码数量
            int FailTotal = 0;

            //无法写入的密码列表
            List<TemplateData_Base> PasswordList = new List<TemplateData_Base>();


            if (mBufs != null)
            {
                foreach (var buf in mBufs)
                {
                    int iCount = buf.ReadInt();
                    FailTotal += iCount;

                    for (int i = 0; i < iCount; i++)
                    {
                        ReadDataByFailBuf(PasswordList, buf);
                    }

                    buf.Release();
                }
                TemplateResult_Base result = CreateResult(PasswordList);
                _Result = result;
            }
        }

        /// <summary>
        /// 创建返回值
        /// </summary>
        /// <param name="passwordList">控制器返回的密码集合</param>
        protected abstract TemplateResult_Base CreateResult(List<TemplateData_Base> passwordList);

        /// <summary>
        /// 用来解析返回的错误元素数据
        /// </summary>
        T _Data;

        /// <summary>
        /// 从错误密码列表中读取一个错误密码，加入到passwordList中
        /// </summary>
        /// <param name="DataList">错误密码列表</param>
        /// <param name="buf"></param>
        private void ReadDataByFailBuf(List<TemplateData_Base> DataList, IByteBuffer buf)
        {
            if (_Data == null) _Data = new T();
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
