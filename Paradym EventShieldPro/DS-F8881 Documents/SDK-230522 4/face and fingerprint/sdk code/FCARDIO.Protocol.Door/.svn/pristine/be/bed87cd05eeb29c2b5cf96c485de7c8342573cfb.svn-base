using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 远程锁定继电器
    /// </summary>
    public class LockRelay : OpenRelay
    {
        /// <summary>
        /// 远程锁定继电器
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">远程操作的继电器端口列表</param>
        public LockRelay(INCommandDetail cd, RemoteRelay_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 远程操作的命令代码
        /// </summary>
        /// <returns></returns>
        protected override byte RemoteCommandCode() => 0x24;
    }
}
