using FCARDIO.Core.Command;
using FCARDIO.Protocol.OnlineAccess;

namespace FCARDIO.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 获取定时读卡播报语音消息
    /// </summary>
    public class ReadCardPeriodSpeak : Protocol.Door.FC8800.SystemParameter.FunctionParameter.ReadCardPeriodSpeak
    {
        /// <summary>
        /// 获取定时读卡播报语音消息 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadCardPeriodSpeak(INCommandDetail cd) : base(cd) {
            CmdType = 0x41;
            CmdIndex = 0x0A;
            CmdPar = 0x8E;
        }

    }
}