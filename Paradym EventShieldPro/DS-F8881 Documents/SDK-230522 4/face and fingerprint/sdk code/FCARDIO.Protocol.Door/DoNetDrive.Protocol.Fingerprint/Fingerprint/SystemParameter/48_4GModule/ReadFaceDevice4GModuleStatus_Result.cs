using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 读取人脸机设备4G模块的工作状态的返回值
    /// </summary>
    public class ReadFaceDevice4GModuleStatus_Result : WriteFaceDevice4GModuleStatus_Parameter, INCommandResult
    {
    }
}
