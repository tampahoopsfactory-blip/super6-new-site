using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 读取客户端模式通讯方式 
    ///0--禁用;
    ///1--UDP;
    ///2--TCP Client;
    ///3--TCP Client + TLS ;
    ///4--MQTT（TCP Client）;
    ///5--MQTT（TCP Client） + TLS ;
    /// </summary>
    public class ReadClientWorkMode : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 创建读取客户端模式通讯方式的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadClientWorkMode(INCommandDetail cd) : base(cd) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x30, 0x03);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 1))
            {
                var buf = oPck.CmdData;
                var rst = new ReadClientWorkMode_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
