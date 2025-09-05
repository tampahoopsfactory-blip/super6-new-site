using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.ConsumptionLimits
{
    /// <summary>
    /// 读取消费机消费限额返回结果
    /// </summary>
    public class ReadConsumptionLimits_Result : WriteConsumptionLimits_Parameter, INCommandResult
    {
    }
}
