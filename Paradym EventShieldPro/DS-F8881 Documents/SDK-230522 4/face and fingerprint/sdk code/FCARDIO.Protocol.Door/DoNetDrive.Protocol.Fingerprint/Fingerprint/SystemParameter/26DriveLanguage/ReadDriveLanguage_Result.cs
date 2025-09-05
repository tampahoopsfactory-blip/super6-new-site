using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 表示设备语言的命令返回值
    /// </summary>
    public class ReadDriveLanguage_Result : WriteDriveLanguage_Parameter, INCommandResult
    {
    }
}
