using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.LawfulCardReleaseAlarmSwitch
{
    /// <summary>
    /// 读取开门超时报警时，合法卡解除报警开关_结果
    /// </summary>
    public class ReadLawfulCardReleaseAlarmSwitch_Result : WriteLawfulCardReleaseAlarmSwitch_Parameter, INCommandResult
    {
    }
}