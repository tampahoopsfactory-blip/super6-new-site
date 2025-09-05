using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 获取定时读卡播报语音消息参数
    /// </summary>
    public class ReadReadCardSpeak : Protocol.Door.Door8800.SystemParameter.FunctionParameter.ReadReadCardSpeak
    {
        /// <summary>
        /// 获取定时读卡播报语音消息参数 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadReadCardSpeak(INCommandDetail cd) : base(cd) {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x41, 0x0A, 0x8E);
        }

        /// <summary>
        /// 检查指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="dl">参数长度</param>
        /// <returns></returns>
        protected override bool CheckResponse(OnlineAccessPacket oPck, int dl)
        {
            return (oPck.DataLen == dl);

        }
    }
}