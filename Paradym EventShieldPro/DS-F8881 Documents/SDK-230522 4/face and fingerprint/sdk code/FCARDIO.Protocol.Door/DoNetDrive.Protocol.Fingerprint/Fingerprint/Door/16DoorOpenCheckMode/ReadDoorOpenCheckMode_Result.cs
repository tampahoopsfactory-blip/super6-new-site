using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Door
{
    /// <summary>
    /// 读取 开门验证方式 返回结果
    /// </summary>
    public class ReadDoorOpenCheckMode_Result : WriteDoorOpenCheckMode_Parameter, INCommandResult
    {
    }
}
