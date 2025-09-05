using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System.Collections.Generic;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.TemplateMethod
{
    /// <summary>
    /// 读取所有元素命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TemplateReadData_Base<T> : Read_Command where T : TemplateData_Base, new()
    {
       

        /// <summary>
        /// 读取到的密码缓冲
        /// </summary>
        protected List<IByteBuffer> mReadBuffers;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        public TemplateReadData_Base(DESDriveCommandDetail cd) : base(cd)
        {
        }



        /// <summary>
        /// 检测下一包指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected abstract bool CheckResponseNext(DESPacket oPck);

        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected abstract bool CheckResponseCompleted(DESPacket oPck);



        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponseNext(oPck))
            {
                var buf = oPck.CommandPacket.CmdData;
                buf.Retain();
                mReadBuffers.Add(buf);
                CommandWaitResponse();
            }

            if (CheckResponseCompleted(oPck))
            {
                var buf = oPck.CommandPacket.CmdData;
                int iTotal = buf.ReadShort();
                _ProcessMax = iTotal;
                List<T> DataList = new List<T>(iTotal);
                foreach (IByteBuffer tmpbuf in mReadBuffers)
                {
                    int iCount = tmpbuf.ReadShort();
                    for (int i = 0; i < iCount; i++)
                    {
                        T dtl = new T();
                        dtl.SetBytes(tmpbuf);
                        DataList.Add(dtl);
                    }
                    _ProcessStep += iCount;
                    fireCommandProcessEvent();
                }

                TemplateResult_Base rst = CreateResult(DataList);
                _Result = rst;

                ClearBuf();
                CommandCompleted();
            }
        }



        /// <summary>
        /// 创建返回值
        /// </summary>
        /// <param name="passwordList"></param>
        protected abstract TemplateResult_Base CreateResult(List<T> passwordList);



        /// <summary>
        /// 清空缓冲区
        /// </summary>
        protected void ClearBuf()
        {
            foreach (IByteBuffer buf in mReadBuffers)
            {
                buf.Release();
            }
            mReadBuffers.Clear();
            mReadBuffers = null;
        }
    }
}
