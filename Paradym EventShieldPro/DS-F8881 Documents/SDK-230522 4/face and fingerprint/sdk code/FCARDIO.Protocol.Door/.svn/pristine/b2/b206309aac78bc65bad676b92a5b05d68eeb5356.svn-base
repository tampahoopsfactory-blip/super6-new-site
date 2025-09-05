using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.SN
{
    /// <summary>
    /// 广播写入控制器SN
    /// </summary>
    public class WriteSN_Broadcast : Protocol.Door.Door8800.SystemParameter.SN.WriteSN_Broadcast
    {
        /// <summary>
        /// 广播写入控制器SN 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含SN数据</param>
        public WriteSN_Broadcast(INCommandDetail cd, SN_Parameter par) : base(cd, par)
        {
            DataStrt = new byte[] { 0x9F, 0x98, 0x79, 0xB8, 0x78 };
            DataEnd = new byte[] { 0xDB, 0xD7, 0x46 };

        }

    }
}
