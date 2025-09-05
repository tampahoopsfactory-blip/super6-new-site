using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;

namespace DoNetDrive.Protocol.POS.TimeGroup
{
    /// <summary>
    /// 读取所有时段
    /// </summary>
    public class ReadTimeGroup : Read_Command
    {
        /// <summary>
        /// 读取到的开门时段缓冲
        /// </summary>
        protected List<IByteBuffer> mReadBuffers;
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        public ReadTimeGroup(DESDriveCommandDetail cd, ReadTimeGroup_Parameter par) : base(cd,par)
        {
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            ReadTimeGroup_Parameter model = _Parameter as ReadTimeGroup_Parameter;
           
            var buf = GetNewCmdDataBuf(1);
            Packet(0x04, 0x02, 0x00, 1, buf.WriteByte(model.Index));
            if (model.Index ==  0)
            {
                _ProcessMax = 64;
            }
          
            mReadBuffers = new List<IByteBuffer>();
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(DESPacket oPck)
        {

            if (CheckResponse(oPck,0x71))
            {
                var buf = oPck.CommandPacket.CmdData;
                buf.Retain();
                mReadBuffers.Add(buf);
                _ProcessStep++;
                fireCommandProcessEvent();
                //CommandWaitResponse();
            }

            if (CheckResponse(oPck, 0x04, 0x02, 0xff, 1))
            {
                var buf = oPck.CommandPacket.CmdData;
                int iTotal = buf.ReadByte();

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
            //64个IByteBuffer，每个包含组 号2byte+224byte(7*4*4(时分-时分))
            foreach (IByteBuffer buf in databufs)
            {
                //StringUtility.WriteByteBuffer(buf);
                //continue;
                WeekTimeGroup wtg = new WeekTimeGroup(4);
                //buf.ReadShort();
                result.Index = buf.ReadByte();
                wtg.SetBytes(buf);
                result.ListWeekTimeGroup.Add(wtg);

            }
            result.Count = result.ListWeekTimeGroup.Count;
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
