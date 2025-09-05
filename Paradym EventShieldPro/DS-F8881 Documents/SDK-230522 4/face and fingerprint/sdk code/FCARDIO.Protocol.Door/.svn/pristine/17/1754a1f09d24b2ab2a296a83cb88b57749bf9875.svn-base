using System;
using System.Collections.Generic;
using System.Text;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 远程继电器门常开
    /// </summary>
    public class HoldRelay : OpenRelay
    {
        /// <summary>
        /// 远程继电器门常开
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">远程操作的继电器端口列表</param>
        public HoldRelay(INCommandDetail cd, RemoteRelay_Patameter par) : base(cd, par) { }

        /// <summary>
        /// 远程操作的命令参数
        /// </summary>
        /// <returns></returns>
        protected override byte RemoteCommandPar() => 2;
    }
}
