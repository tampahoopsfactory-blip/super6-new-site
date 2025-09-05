using FCARDIO.Core.Command;

namespace FCARDIO.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 获取定时读卡播报语音消息_结果
    /// </summary>
    public class ReadCardPeriodSpeak_Result : WriteCardPeriodSpeak_Parameter, INCommandResult
    {
    }
}