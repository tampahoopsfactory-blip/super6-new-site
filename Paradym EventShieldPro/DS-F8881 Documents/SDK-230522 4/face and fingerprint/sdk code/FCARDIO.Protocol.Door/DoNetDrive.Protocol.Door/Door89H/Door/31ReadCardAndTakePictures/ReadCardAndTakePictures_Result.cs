using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Door.ReadCardAndTakePictures
{
    /// <summary>
    /// 读卡拍照联动消息 返回结果
    /// </summary>
    public class ReadCardAndTakePictures_Result : WriteReadCardAndTakePictures_Parameter, INCommandResult
    {
    }
}
