using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ItemDetectionFunction
{
    /// <summary>
    /// 读取 物品检测功能 返回结果
    /// </summary>
    public class ReadItemDetectionFunction_Result : WriteItemDetectionFunction_Parameter, INCommandResult
    {
    }
}
