using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.RelayReleaseTime
{
    /// <summary>
    /// 开锁时输出时长_结果
    /// </summary>
    public class RelayReleaseTime_Result : WriteRelayReleaseTime_Parameter, INCommandResult
    {
    }
}
