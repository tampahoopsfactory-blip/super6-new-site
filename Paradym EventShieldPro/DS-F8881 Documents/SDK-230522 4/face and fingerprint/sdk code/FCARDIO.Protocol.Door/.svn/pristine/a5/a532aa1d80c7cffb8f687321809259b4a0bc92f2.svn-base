using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.TimeGroup
{
    /// <summary>
    /// 添加开门时段
    /// </summary>
    public class AddTimeGroup : Protocol.Door.Door8800.TimeGroup.AddTimeGroup
    {
       

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">命令详情</param>
        /// <param name="par">命令逻辑所需要的命令参数 </param>
        public AddTimeGroup(INCommandDetail cd, AddTimeGroup_Parameter par) : base(cd, par) {
            mPar = par;
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            maxCount = mPar.ListWeekTimeGroup.Count;
            Packet(0x46, 0x3, 0x00, 225, GetBytes(GetNewCmdDataBuf(225)));
            writeIndex++;
            _ProcessMax = maxCount;
        }
    }
}
