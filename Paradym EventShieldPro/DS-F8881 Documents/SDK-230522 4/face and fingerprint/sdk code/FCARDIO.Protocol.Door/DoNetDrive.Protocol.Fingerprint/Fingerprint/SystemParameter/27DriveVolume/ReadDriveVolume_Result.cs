using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 表示设备音量的命令返回值
    /// </summary>
    public class ReadDriveVolume_Result : WriteDriveVolume_Parameter, INCommandResult
    {
    }
}
