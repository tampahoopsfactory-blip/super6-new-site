using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 表示人脸机活体检测的命令返回值
    /// </summary>
    public class ReadFaceBioassay_Result : WriteFaceBioassay_Parameter, INCommandResult
    {
    }
}
