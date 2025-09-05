using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.POS.Software
{
    /// <summary>
    /// 上传固件的结果反馈
    /// </summary>
    public class UpdateSoftware_Result : INCommandResult
    {

        /// <summary>
        /// 写入结果
        /// 1--校验成功
        //  0--校验失败
        /// </summary>
        public byte Success;

        public void Dispose()
        {

        }
    }
}
