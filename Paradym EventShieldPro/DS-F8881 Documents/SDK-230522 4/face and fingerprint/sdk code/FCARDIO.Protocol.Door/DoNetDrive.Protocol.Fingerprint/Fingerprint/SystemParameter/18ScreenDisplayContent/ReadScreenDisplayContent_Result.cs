using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.ScreenDisplayContent
{
    /// <summary>
    /// 读取 屏幕显示内容 返回结果
    /// </summary>
    public class ReadScreenDisplayContent_Result : WriteScreenDisplayContent_Parameter, INCommandResult
    {
    }
}
