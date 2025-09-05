using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.POS.File
{
    public class UploadFile_Result : INCommandResult
    {

        /// <summary>
        /// 写入结果
        /// 1--SD卡已满
        //  2--版本号错误
        /// </summary>
        public byte Success;

        public void Dispose()
        {

        }
    }
}
