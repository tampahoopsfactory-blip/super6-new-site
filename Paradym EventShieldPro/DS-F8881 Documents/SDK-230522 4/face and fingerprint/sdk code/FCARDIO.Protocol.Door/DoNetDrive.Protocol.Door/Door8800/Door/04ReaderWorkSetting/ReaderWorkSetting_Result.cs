using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Core.Data;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Door.ReaderWorkSetting
{
    /// <summary>
    /// 门认证方式_结果
    /// </summary>
    public class ReaderWorkSetting_Result : WriteReaderWorkSetting_Parameter, INCommandResult
    {
    }
}
