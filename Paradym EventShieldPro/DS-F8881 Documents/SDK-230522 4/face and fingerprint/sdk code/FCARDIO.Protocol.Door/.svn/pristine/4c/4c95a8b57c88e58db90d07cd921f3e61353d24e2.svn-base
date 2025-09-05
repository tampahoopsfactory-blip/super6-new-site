using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.IllegalVerificationAlarm
{
    /// <summary>
    /// 读取 非法验证报警
    /// </summary>
    public class ReadIllegalVerificationAlarm : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadIllegalVerificationAlarm(INCommandDetail cd) : base(cd, null) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x04, 0x04, 0x01);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x04, 0x04, 0x01))
            {
                var buf = oPck.CmdData;
                ReadIllegalVerificationAlarm_Result rst = new ReadIllegalVerificationAlarm_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
