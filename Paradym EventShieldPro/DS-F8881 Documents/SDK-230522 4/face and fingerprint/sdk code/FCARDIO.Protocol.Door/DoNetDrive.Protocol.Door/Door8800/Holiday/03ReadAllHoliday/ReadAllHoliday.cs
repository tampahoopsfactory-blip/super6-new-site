using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Holiday
{
    /// <summary>
    /// 读取控制板中已存储的所有节假日<br/>
    /// 读取成功返回 ReadAllHoliday_Result
    /// </summary>
    public class ReadAllHoliday : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 读取到的节假日缓冲
        /// </summary>
        protected List<IByteBuffer> mReadBuffers;

        /// <summary>
        /// 构造命令，无需其他参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadAllHoliday(INCommandDetail cd) : base(cd, null)
        {
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x04, 3);
            mReadBuffers = new List<IByteBuffer>();
        }

        /// <summary>
        /// 检测下一包指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected virtual bool CheckResponseOK(OnlineAccessPacket oPck)
        {
            return (oPck.CmdType == 0x34 &&
                oPck.CmdIndex == 3 &&
                oPck.CmdPar == 0);
        }

        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected virtual bool CheckResponseCompleted(OnlineAccessPacket oPck)
        {
            return (oPck.CmdType == 0x34 &&
                oPck.CmdIndex == 3 &&
                oPck.CmdPar == 0xff && oPck.DataLen == 4);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            //应答：
            if (CheckResponseOK(oPck))
            {
                var buf = oPck.CmdData;
                buf.Retain();
                mReadBuffers.Add(buf);
                CommandWaitResponse();
            }
            //应答：传输结束
            if (CheckResponseCompleted(oPck))
            {
                var buf = oPck.CmdData;
                int iTotal = buf.ReadInt();
                _ProcessMax = iTotal;
                ReadAllHoliday_Result rst = new ReadAllHoliday_Result();
                _Result = rst;
                rst.SetBytes(iTotal,mReadBuffers);
                _ProcessStep = iTotal;
                ClearBuf();
                CommandCompleted();
            }

        }

        /// <summary>
        /// 命令重发时，对命令中一些缓冲做清空或参数重置<br/>
        /// 此命令一般情况下不需要实现！
        /// </summary>
        protected override void CommandReSend()
        {
            ClearBuf();
        }
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
