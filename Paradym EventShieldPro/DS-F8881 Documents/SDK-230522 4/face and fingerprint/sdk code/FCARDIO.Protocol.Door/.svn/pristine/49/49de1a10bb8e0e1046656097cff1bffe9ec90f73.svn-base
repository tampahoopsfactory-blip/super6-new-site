using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.AutoTest
{
    /// <summary>
    /// 自动测试命令的执行的结果
    /// </summary>
    public class AutoTestCommand_Result : INCommandResult
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public int Result;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            return;
        }

        /// <summary>
        /// 自动测试命令的执行的结果进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public void SetBytes(IByteBuffer databuf)
        {
            Result = databuf.ReadByte();
        }
    }
}
