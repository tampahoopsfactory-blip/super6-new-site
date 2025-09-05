using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN
{
    /// <summary>
    /// 广播写入控制器SN
    /// </summary>
    public class WriteSN_Broadcast : WriteSN
    {
        /// <summary>
        /// 广播写入控制器SN 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含SN数据</param>
        public WriteSN_Broadcast(INCommandDetail cd, SN_Parameter par) : base(cd, par) {
            DataStrt = new byte[] { 0xFC, 0x65, 0x65, 0x33, 0xFF };
            DataEnd = new byte[] { 0xCF, 0x35, 0x92 };
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0xC1, 0xD1, 0xF7, 0x18, GetCmdData());
            var par = _Parameter as SN_Parameter;

            if (par.UDPBroadcast)
            {
                DoorPacket.SetUDPBroadcastPacket();
            }
        }
    }
}