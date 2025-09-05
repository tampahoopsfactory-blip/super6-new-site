using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.GateMagneticAlarm
{
    /// <summary>
    /// 读取门磁报警参数
    /// </summary>
    public class ReadGateMagneticAlarm_Result : WriteGateMagneticAlarm_Parameter , INCommandResult
    {
    }
}
