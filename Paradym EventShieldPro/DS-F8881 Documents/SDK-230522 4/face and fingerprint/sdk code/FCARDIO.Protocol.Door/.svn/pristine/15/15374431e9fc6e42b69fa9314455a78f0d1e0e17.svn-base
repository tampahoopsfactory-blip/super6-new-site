using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter._37NetworkSetting._06PUSH_AUTO
{
    /// <summary>
    /// 获取自动推送服务器详情
    /// </summary>
    public class Read_PUSH_AUTO_Service : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 创建获取自动推送服务器详情的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public Read_PUSH_AUTO_Service(INCommandDetail cd) : base(cd) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x30, 0x08);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 46))
            {
                var buf = oPck.CmdData;
                var rst = new Read_PUSH_AUTO_Service_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

    }
}
