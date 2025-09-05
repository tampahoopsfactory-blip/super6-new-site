using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 表示人脸机活体检测阈值的命令返回值
    /// </summary>
    public class ReadFaceBioassaySimilarity_Result : WriteFaceBioassaySimilarity_Parameter, INCommandResult
    {
    }
}
