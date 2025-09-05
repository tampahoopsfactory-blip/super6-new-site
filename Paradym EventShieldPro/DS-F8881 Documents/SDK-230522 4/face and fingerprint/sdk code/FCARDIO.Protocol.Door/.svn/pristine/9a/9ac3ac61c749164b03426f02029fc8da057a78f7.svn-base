using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData 
{
    /// <summary>
    /// TCP Client 模式下写文件的返回值
    /// </summary>
    public class WriteFileByTCP_Result : INCommandResult
    {

        /// <summary>
        /// 写入结果
        /// 0--非法参数;
        /// 1--文件存储完毕;  
        /// 2--CRC校验失败;
        /// 3--用户不存在;
        /// 4--存储已满;
        /// 5--文件不可识别（照片不可识别或特征码错误）;
        /// 6--照片、特征码重复;
        /// </summary>
        public int Result;

        /// <summary>
        /// 当结果为重复时，重复的用户编号
        /// </summary>
        public uint RepeatUser;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
    }
}
