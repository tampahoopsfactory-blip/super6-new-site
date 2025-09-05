using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 远程解除继电器锁定
    /// </summary>
    public class UnlockRelay : OpenRelay
    {
        /// <summary>
        /// 远程解除继电器锁定
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">远程操作的继电器端口列表</param>
        public UnlockRelay(INCommandDetail cd, RemoteRelay_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 远程操作的命令代码
        /// </summary>
        /// <returns></returns>
        protected override byte RemoteCommandCode() => 0x24;

        /// <summary>
        /// 远程操作的命令参数
        /// </summary>
        /// <returns></returns>
        protected override byte RemoteCommandPar() => 1;
    }
}
