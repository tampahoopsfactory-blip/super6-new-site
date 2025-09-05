using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.POS.SystemParameter.RecordStorageMode
{
    /// <summary>
    /// 读取记录存储方式 返回结果
    /// </summary>
    public class ReadRecordStorageMode_Result : WriteRecordStorageMode_Parameter, INCommandResult
    {
    }
}
