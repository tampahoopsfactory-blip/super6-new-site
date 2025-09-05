using System;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 修改网络服务器参数
    /// </summary>
    public class WriteNetworkServerDetail : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建修改网络服务器参数的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteNetworkServerDetail(INCommandDetail cd, WriteNetworkServerDetail_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteNetworkServerDetail_Parameter;
            Packet(0x01, 0x30, 0x00, 105, model.GetBytes(GetNewCmdDataBuf(105)));
        }
    }
}
