using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.LEDOpenHoldTime
{
    /// <summary>
    /// 读取 开机保持时间 返回结果
    /// </summary>
    public class ReadLEDOpenHoldTime_Result : WriteLEDOpenHoldTime_Parameter, INCommandResult
    {
    }
}
