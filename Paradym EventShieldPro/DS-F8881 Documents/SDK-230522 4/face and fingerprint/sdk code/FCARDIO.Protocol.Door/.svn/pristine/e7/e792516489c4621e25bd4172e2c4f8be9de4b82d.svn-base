using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 准备写文件返回结果
    /// </summary>
    public class WriteFeatureCode_Result : INCommandResult
    {
        /// <summary>
        /// 文件句柄
        /// </summary>
        public int FileHandle;

        /// <summary>
        /// 写入结果
        /// 1--校验成功
        /// 0--校验失败
        /// 2--特征码无法识别
        /// 3--人员照片不可识别
        /// 4--人员重复
        /// -1 -- 拒绝写入
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
