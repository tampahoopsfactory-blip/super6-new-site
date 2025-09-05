using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.AgencyCode
{
    /// <summary>
    /// 读取经销商代码 返回结果
    /// </summary>
    public class ReadAgencyCode_Result : WriteAgencyCode_Parameter, INCommandResult
    {
    }
}
