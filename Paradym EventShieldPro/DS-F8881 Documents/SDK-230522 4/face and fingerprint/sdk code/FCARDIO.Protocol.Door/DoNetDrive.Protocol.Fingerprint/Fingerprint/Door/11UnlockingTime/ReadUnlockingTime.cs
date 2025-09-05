using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.Door.RelayReleaseTime
{
    /// <summary>
    /// 获取开锁时输出时长
    /// </summary>
    public class ReadUnlockingTime : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 读取门工作方式
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadUnlockingTime(INCommandDetail cd) : base(cd, null) { }

       

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x02))
            {
                var buf = oPck.CmdData;
                ReadUnlockingTime_Result rst = new ReadUnlockingTime_Result();
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
            Packet(0x03, 0x08, 0x00);
        }

       
    }
}
