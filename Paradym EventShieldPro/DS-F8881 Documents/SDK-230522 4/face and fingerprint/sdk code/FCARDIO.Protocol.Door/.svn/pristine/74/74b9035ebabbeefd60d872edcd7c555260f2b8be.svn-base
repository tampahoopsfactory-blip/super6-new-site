using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDatabase
{
    /// <summary>
    /// 读取所有巡更人员
    /// </summary>
    public class ReadPatrolEmplDatabase : Read_Command
    {
        // 队列
        Queue<IByteBuffer> mBufs;

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadPatrolEmplDatabase(INCommandDetail cd) : base(cd, null)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(3, 3);
            mBufs = new Queue<IByteBuffer>();
            _ProcessMax = 1;
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            //读取下一包
            if (CheckResponse(oPck,0x3,3))
            {
                var buf = oPck.CmdData;
                _ProcessStep++;
                buf.Retain();
                mBufs.Enqueue(buf);
                CommandWaitResponse();
            }
            //全部读取完成
            if (CheckResponse(oPck, 0xF3 - 0x30, 0x3, 0x2))
            //else
            {
                //业务代码
                while (mBufs.Count > 0)
                {
                    var buf = oPck.CmdData;
                    ushort iTotal = buf.ReadUnsignedShort();
                    _ProcessMax = iTotal;
                    //读取buf内容
                    ReadPatrolEmplDatabase_Result result = new ReadPatrolEmplDatabase_Result();
                    _Result = result;
                    Analysis(iTotal);
                    //读取buf内容
                    buf.Release();//要释放
                }
                //
                _ProcessStep = _ProcessMax;
                fireCommandProcessEvent();

                CommandCompleted();
            }
        }

        /// <summary>
        /// 分析缓冲中的数据包
        /// </summary>
        /// <param name="iSize">总数</param>
        private void Analysis(ushort iSize)
        {
            ReadPatrolEmplDatabase_Result result = (ReadPatrolEmplDatabase_Result)_Result;
           
            result.Quantity = iSize;
            result.PatrolEmplList = new List<Data.PatrolEmpl>();
            while (mBufs.Count > 0)
            {
                IByteBuffer buf = mBufs.Dequeue();
                iSize = buf.ReadUnsignedShort();

                for (int i = 0; i < iSize; i++)
                {
                    Data.PatrolEmpl patrolEmpl = new Data.PatrolEmpl();
                    patrolEmpl.SetBytes(buf);
                    result.PatrolEmplList.Add(patrolEmpl);
                }
                buf.Release();//要释放
            }
        }
    }
}
