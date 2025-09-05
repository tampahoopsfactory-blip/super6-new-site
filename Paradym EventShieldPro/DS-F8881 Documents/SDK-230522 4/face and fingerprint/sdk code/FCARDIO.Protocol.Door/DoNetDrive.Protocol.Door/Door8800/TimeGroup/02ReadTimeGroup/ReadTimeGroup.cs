using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Door.Door8800.TimeGroup
{
    /// <summary>
    /// 读取所有开门时段
    /// </summary>
    public class ReadTimeGroup : Door8800Command_ReadParameter
    {

        /// <summary>
        /// 读取到的开门时段缓冲
        /// </summary>
        protected List<IByteBuffer> mReadBuffers;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        public ReadTimeGroup(INCommandDetail cd) : base(cd, null)
        {
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck))
            {
                var buf = oPck.CmdData;
                buf.Retain();
                mReadBuffers.Add(buf);
                _ProcessStep++;
                fireCommandProcessEvent();
                CommandWaitResponse();
            }

            if (CheckResponse(oPck, 0x06, 0x02, 0xff, 4))
            {
                var buf = oPck.CmdData;
                int iTotal = buf.ReadInt();

                ReadTimeGroup_Result rtgr = new ReadTimeGroup_Result();
                _Result = rtgr;

                SetBytes(rtgr, mReadBuffers);
                ClearBuf();
                CommandCompleted();
            }
        }

        /// <summary>
        ///  将 字节流  转换为 开门时段
        /// </summary>
        /// <param name="result">读取所有开门时段结果</param>
        /// <param name="databufs"></param>
        public void SetBytes(ReadTimeGroup_Result result, List<IByteBuffer> databufs)
        {
            result.ListWeekTimeGroup.Clear();
            //64个IByteBuffer，每个包含组 号2byte+224byte(7*8*4(时分-时分))
            foreach (IByteBuffer buf in databufs)
            {
                //StringUtility.WriteByteBuffer(buf);
                //continue;
                WeekTimeGroup wtg = new WeekTimeGroup(8);
                buf.ReadShort();
                wtg.SetBytes(buf);
                result.ListWeekTimeGroup.Add(wtg);
               
            }
            result.Count = result.ListWeekTimeGroup.Count;
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x06, 0x02);
            _ProcessMax = 64;
            mReadBuffers = new List<IByteBuffer>();
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
