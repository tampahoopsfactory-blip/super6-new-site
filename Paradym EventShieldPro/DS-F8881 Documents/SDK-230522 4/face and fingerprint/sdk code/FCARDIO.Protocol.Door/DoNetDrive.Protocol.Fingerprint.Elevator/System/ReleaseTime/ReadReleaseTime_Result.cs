using System;
using System.Collections.Generic;
using System.Text;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 读取电梯继电器板的继电器开锁输出时长命令的返回值
    /// </summary>
    public class ReadReleaseTime_Result : WriteReleaseTime_Parameter, INCommandResult
    {
    }
}
