using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.SystemParameter.SN;

namespace DoNetDrive.Protocol.POS.SystemParameter.SN
{
    /// <summary>
    /// 获取控制器SN
    /// </summary>
    public class ReadSN : Read_Command
    {
        /// <summary>
        /// 获取控制器SN 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadSN(DESDriveCommandDetail cd):base(cd)
        {
        }
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(1, 1);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponse(oPck, 16))
            {
                var buf = oPck.CommandPacket.CmdData;
                SN_Result rst = new SN_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

    }
}
