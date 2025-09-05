using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 获取烟雾报警参数_结果
    /// </summary>
    public class ReadSmogAlarmOption_Result : WriteSmogAlarmOption_Parameter, INCommandResult
    {
    }
}