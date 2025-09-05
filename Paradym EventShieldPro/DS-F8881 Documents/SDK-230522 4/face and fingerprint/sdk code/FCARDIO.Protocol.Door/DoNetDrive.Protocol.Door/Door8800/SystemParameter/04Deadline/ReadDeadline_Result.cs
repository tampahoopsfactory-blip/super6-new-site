using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.Deadline
{
    /// <summary>
    /// 获取设备有效期_结果
    /// </summary>
    public class ReadDeadline_Result : WriteDeadline_Parameter, INCommandResult
    {
    }
}