using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Software
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

        /// <summary>
        /// 跳过的数据包数量
        /// </summary>
        public int SkipPacketCount { get; internal set; }

        public void Dispose()
        {

        }
    }
}
