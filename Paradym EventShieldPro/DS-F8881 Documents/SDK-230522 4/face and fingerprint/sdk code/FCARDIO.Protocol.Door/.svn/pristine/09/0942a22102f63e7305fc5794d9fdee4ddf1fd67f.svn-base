using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter.ReaderWorkSetting
{
    /// <summary>
    /// 读卡认证方式
    /// </summary>
    public class ReadReaderWorkSetting : Read_Command
    {
        /// <summary>
        /// 读取认证方式
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadReaderWorkSetting(INCommandDetail cd) : base(cd, null) { }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x118))
            {
                var buf = oPck.CmdData;
                ReaderWorkSetting_Result rst = new ReaderWorkSetting_Result();
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
            Packet(0x41, 0x0A, 0x8A);
        }
    }
}
