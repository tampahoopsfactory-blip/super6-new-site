using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.TimeGroup
{
    /// <summary>
    /// 添加开门时段
    /// </summary>
    public class AddTimeGroup : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 参数
        /// </summary>
        protected AddTimeGroup_Parameter mPar;
        /// <summary>
        /// 写入索引
        /// </summary>
        protected int writeIndex = 0;

        /// <summary>
        /// 总开门时段数
        /// </summary>
        protected int maxCount = 0;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">命令详情</param>
        /// <param name="par">命令逻辑所需要的命令参数 </param>
        public AddTimeGroup(INCommandDetail cd, AddTimeGroup_Parameter par) : base(cd, par) {
            mPar = par;
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            AddTimeGroup_Parameter model = value as AddTimeGroup_Parameter;
            if (model == null)
            {
                return false;
            }

            return model.checkedParameter();
        }
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            maxCount = mPar.ListWeekTimeGroup.Count;
            Packet(0x06, 0x03, 0x00, 225, GetBytes(GetNewCmdDataBuf(225)));
            writeIndex++;
            _ProcessMax = maxCount;

        }

        /// <summary>
        /// 将 参数 编码到字节流
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        protected IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(writeIndex + 1);

            mPar.ListWeekTimeGroup[writeIndex].GetBytes(databuf);
            return databuf;
        }

        /// <summary>
        /// 没有触发
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            //应答
            if (writeIndex < maxCount)
            {
                var buf = GetBytes(GetCmdBuf());
                DoorPacket.DataLen = buf.ReadableBytes;
                writeIndex++;
                _ProcessStep++;
                fireCommandProcessEvent();
                CommandReady();
            }
            else
            {
                CommandCompleted();
            }

        }


        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {
            if (CheckResponse_OK(oPck))
            {
                //继续发下一包
                CommandNext1(oPck);
            }

        }

    }
}
