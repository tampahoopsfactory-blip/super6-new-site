using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Protocol.Door.Door8800.TimeGroup;
using DoNetDrive.Protocol.OnlineAccess;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.TimeGroup
{
    /// <summary>
    /// 读取所有开门时段
    /// </summary>
    public class ReadTimeGroup : Protocol.Door.Door8800.TimeGroup.ReadTimeGroup
    {

        
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        public ReadTimeGroup(INCommandDetail cd) : base(cd)
        {
        }
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x46, 2);
            _ProcessMax = 64;
            mReadBuffers = new List<IByteBuffer>();
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

            if (CheckResponse(oPck, 0x56, 0x02, 0xff, 4)) //
            {
                var buf = oPck.CmdData;
                int iTotal = buf.ReadInt();

                ReadTimeGroup_Result rtgr = new ReadTimeGroup_Result();
                _Result = rtgr;
                if (mReadBuffers.Count > 64)
                {
                    mReadBuffers.RemoveAt(64);
                }
                SetBytes(rtgr, mReadBuffers);
                ClearBuf();
                CommandCompleted();
            }
        }

        /// <summary>
        /// 检查指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="dl">参数长度</param>
        /// <returns></returns>
        protected override bool CheckResponse(OnlineAccessPacket oPck, int dl)
        {
            return (oPck.DataLen == dl);

        }

        /// <summary>
        /// 检查指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="CmdType">命令类型</param>
        /// <param name="CmdIndex">命令索引</param>
        /// <param name="CmdPar">命令参数</param>
        /// <param name="dl">参数长度</param>
        /// <returns></returns>
        protected override bool CheckResponse(OnlineAccessPacket oPck, byte CmdType, byte CmdIndex, byte CmdPar, int dl)
        {
            return (oPck.CmdType == CmdType &&
                oPck.CmdIndex == CmdIndex &&
                oPck.CmdPar == CmdPar &&
                oPck.DataLen == dl);

        }
    }
}
