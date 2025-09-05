using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ExpireTime
{
    /// <summary>
    /// 读取设备有效期 返回结果
    /// </summary>
    public class ExpireTime_Result : ExpireTime_Parameter, INCommandResult
    {
    }
}
