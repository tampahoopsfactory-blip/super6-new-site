using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Time;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.Time
{
    /// <summary>
    /// 从控制器中读取控制器时间
    /// </summary>
    public class ReadTime : Read_Command
    {
        /// <summary>
        /// 获取设备运行信息 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadTime(DESDriveCommandDetail cd) : base(cd)
        {
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x02, 0x01);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponse(oPck, 0x07))
            {
                var buf = oPck.CommandPacket.CmdData;
                ReadTime_Result rst = new ReadTime_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
