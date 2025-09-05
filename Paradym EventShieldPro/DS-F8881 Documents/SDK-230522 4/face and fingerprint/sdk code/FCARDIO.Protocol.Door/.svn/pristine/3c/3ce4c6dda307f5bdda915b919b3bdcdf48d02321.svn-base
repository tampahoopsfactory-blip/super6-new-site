using DoNetDrive.Core.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint
{
    /// <summary>
    /// 标记为子命令
    /// </summary>
    public interface ISubCommand
    {
        /// <summary>
        /// 进度
        /// </summary>
        int ProcessStep { get; set; }

        /// <summary>
        /// 进度最大值
        /// </summary>
        int ProcessMax { get; set; }

        /// <summary>
        /// 判断命令是否已结束
        /// </summary>
        /// <returns></returns>
        bool IsCommandOver();

        /// <summary>
        /// 释放
        /// </summary>
        void Release();

        /// <summary>
        /// 让子命令推进进度
        /// </summary>
        /// <param name="oPck"></param>
        void CommandNext(INPacket oPck);
    }
}
