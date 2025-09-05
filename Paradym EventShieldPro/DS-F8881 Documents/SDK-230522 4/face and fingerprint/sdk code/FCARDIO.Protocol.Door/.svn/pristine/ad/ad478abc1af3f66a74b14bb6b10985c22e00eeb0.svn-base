using FCARDIO.Core.Command;
using System;

namespace FCARDIO.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置定时读卡播报语音消息
    /// </summary>
    public class WriteCardPeriodSpeak : Protocol.Door.FC8800.SystemParameter.FunctionParameter.WriteCardPeriodSpeak
    {
        /// <summary>
        /// 设置定时读卡播报语音消息 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteCardPeriodSpeak(INCommandDetail cd, WriteCardPeriodSpeak_Parameter par) : base(cd, par) {
            CmdType = 0x41;
            CmdIndex = 0x0A;
            CmdPar = 0x0E;
        }

    }
}