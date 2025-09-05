using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入客户端模式通讯方式 
    ///0--禁用;
    ///1--UDP;
    ///2--TCP Client;
    ///3--TCP Client + TLS ;
    ///4--MQTT（TCP Client）;
    ///5--MQTT（TCP Client） + TLS ;
    /// </summary>
    public class WriteClientWorkMode : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建写入客户端模式通讯方式的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteClientWorkMode(INCommandDetail cd, WriteClientWorkMode_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteClientWorkMode_Parameter;
            Packet(0x01, 0x30, 0x02, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }
    }
}
