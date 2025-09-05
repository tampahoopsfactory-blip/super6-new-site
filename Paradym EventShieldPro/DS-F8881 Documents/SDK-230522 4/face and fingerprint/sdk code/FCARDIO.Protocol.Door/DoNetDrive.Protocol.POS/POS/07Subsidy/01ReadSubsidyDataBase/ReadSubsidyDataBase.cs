using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.Subsidy
{
    /// <summary>
    /// 读取补贴容量信息
    /// </summary>
    public class ReadSubsidyDataBase : Read_Command
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        public ReadSubsidyDataBase(DESDriveCommandDetail cd) : base(cd, null)
        {
        }

        /// <summary>
        /// 处理返回通知
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponse(oPck, 0x04))
            {
                var buf = oPck.CommandPacket.CmdData;
                ReadSubsidyDataBase_Result rst = new ReadSubsidyDataBase_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x07, 1);
        }
    }
}
