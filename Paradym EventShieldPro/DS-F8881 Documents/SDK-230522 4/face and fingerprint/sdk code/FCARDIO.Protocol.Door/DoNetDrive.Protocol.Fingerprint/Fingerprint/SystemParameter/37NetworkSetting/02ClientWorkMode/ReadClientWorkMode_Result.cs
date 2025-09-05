using DoNetDrive.Core.Command;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter 
{
    /// <summary>
    /// 读取客户端模式通讯方式 返回值
    /// </summary>
    public class ReadClientWorkMode_Result: WriteClientWorkMode_Parameter, INCommandResult
    {
    }
}
