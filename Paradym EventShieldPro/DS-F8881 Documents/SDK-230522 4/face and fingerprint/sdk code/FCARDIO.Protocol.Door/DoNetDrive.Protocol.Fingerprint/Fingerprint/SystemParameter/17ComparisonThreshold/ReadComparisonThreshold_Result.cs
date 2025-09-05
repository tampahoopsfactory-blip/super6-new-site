using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 读取 脸、指纹比对阈值 返回结果
    /// </summary>
    public class ReadComparisonThreshold_Result : WriteComparisonThreshold_Parameter, INCommandResult
    {
    }
}
