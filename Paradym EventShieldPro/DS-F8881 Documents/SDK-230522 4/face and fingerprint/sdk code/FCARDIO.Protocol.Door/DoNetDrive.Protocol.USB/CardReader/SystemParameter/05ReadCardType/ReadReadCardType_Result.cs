using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.ReadCardType
{
    /// <summary>
    /// 读取记录存储方式 返回结果
    /// </summary>
    public class ReadReadCardType_Result : WriteReadCardType_Parameter, INCommandResult
    {
    }
}
