using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.IllegalVerificationAlarm
{
    /// <summary>
    /// 读取 非法验证报警 返回结果
    /// </summary>
    public class ReadIllegalVerificationAlarm_Result : WriteIllegalVerificationAlarm_Parameter, INCommandResult
    {
    }
}
