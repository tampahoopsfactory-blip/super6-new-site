using System;
using System.Collections.Generic;
using System.Text;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 读取人员电梯扩展权限的命令返回值
    /// </summary>
    public class ReadPersonElevatorAccess_Result : WritePersonElevatorAccess_Parameter, INCommandResult
    {
    }
}
