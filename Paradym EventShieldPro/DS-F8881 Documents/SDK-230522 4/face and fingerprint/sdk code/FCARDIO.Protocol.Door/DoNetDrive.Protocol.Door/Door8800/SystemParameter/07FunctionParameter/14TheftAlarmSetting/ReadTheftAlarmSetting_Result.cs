using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 获取智能防盗主机参数_结果
    /// </summary>
    public class ReadTheftAlarmSetting_Result : WriteTheftAlarmSetting_Parameter, INCommandResult
    {
    }
}