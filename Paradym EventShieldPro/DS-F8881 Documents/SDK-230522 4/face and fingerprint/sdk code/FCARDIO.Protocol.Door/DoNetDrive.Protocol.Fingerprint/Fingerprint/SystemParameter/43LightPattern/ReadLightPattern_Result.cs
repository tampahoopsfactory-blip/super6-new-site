using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 读取感光模式的返回值
    /// </summary>
    public class ReadLightPattern_Result : WriteLightPattern_Parameter, INCommandResult
    {
    }
}
