using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.DeletePatrolEmpl
{
    /// <summary>
    /// 删除巡更人员
    /// </summary>
    public class DeletePatrolEmpl : Write_Command
    {
        /// <summary>
        /// 默认的缓冲区大小
        /// </summary>
        protected int MaxBufSize = 120;

        /// <summary>
        /// 每次上传数量
        /// </summary>
        protected const int mBatchCount = 10;

        /// <summary>
        /// 删除巡更人员参数
        /// </summary>
        DeletePatrolEmpl_Parameter mPar;

        /// <summary>
        /// 已上传数量
        /// </summary>
        protected int mIndex;

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public DeletePatrolEmpl(INCommandDetail cd, DeletePatrolEmpl_Parameter par) : base(cd, par)
        {
            mPar = par;
            MaxBufSize = (mBatchCount * 2) + 2;
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var buf = GetNewCmdDataBuf(MaxBufSize);
            WritePatrolEmplToBuf(buf);
            Packet(0x3, 0x6, (uint)buf.ReadableBytes, buf);
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            DeletePatrolEmpl_Parameter model = value as DeletePatrolEmpl_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 写入 删除巡更人员 字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public IByteBuffer WritePatrolEmplToBuf(IByteBuffer databuf)
        {

            var lst = mPar.PCodeList;
            int iCount = lst.Count;//获取列表总长度
            iCount = iCount - mIndex;//计算未上传总数

            int iLen = iCount;
            if (iLen > mBatchCount)
            {
                iLen = mBatchCount;
            }

            databuf.Clear();

            databuf.WriteUnsignedShort((ushort)iLen);
            for (int i = 0; i < iLen; i++)
            {
                databuf.WriteUnsignedShort(lst[mIndex + i]);
            }

            mIndex += iLen;
            _ProcessStep += iLen;
            return databuf;
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (IsWriteOver())
            {
                CommandCompleted();
            }
            else
            {
                //未发送完毕，继续发送
                var buf = GetCmdBuf();
                WritePatrolEmplToBuf(buf);
                USBPacket.DataLen = buf.ReadableBytes;
                CommandReady();//设定命令当前状态为准备就绪，等待发送
            }
        }

        /// <summary>
        /// 检查是否已写完
        /// </summary>
        /// <returns></returns>
        private bool IsWriteOver()
        {
            return (mPar.PCodeList.Count - mIndex) == 0;
        }

        /// <summary>
        /// 重写父类对处理返回值的定义
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(USBDrivePacket oPck)
        {
            if (CheckResponse_OK(oPck))
            {

                //继续发下一包
                CommandNext1(oPck);
            }
           
        }
    }
}
