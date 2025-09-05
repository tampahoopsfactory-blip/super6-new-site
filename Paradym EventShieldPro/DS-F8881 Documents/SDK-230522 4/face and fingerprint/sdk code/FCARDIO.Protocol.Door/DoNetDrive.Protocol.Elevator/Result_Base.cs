using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator
{
    /// <summary>
    /// 针对读命令返回结果进行抽象封装
    /// </summary>
    public abstract class Result_Base : INCommandResult
    {
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
