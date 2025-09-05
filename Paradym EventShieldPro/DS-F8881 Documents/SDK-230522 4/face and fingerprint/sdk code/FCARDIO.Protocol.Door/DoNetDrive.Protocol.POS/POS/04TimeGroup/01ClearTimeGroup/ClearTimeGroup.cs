using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.TimeGroup
{
    public class ClearTimeGroup : Read_Command
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        public ClearTimeGroup(DESDriveCommandDetail cd) : base(cd)
        {
        }

        protected override void CommandNext1(DESPacket oPck)
        {
            CommandCompleted();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x04, 0x01);
        }
    }
}
