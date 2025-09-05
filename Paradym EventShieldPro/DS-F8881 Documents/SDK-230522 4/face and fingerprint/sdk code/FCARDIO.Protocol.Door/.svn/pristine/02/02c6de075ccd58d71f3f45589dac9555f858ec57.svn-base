using System;
using System.Collections.Generic;
using System.Text;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 远程开门
    /// </summary>
    public class OpenRelay : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 远程开门
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">远程操作的继电器端口列表</param>
        public OpenRelay(INCommandDetail cd, RemoteRelay_Patameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            var model = value as RemoteRelay_Patameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }

        /// <summary>
        /// 远程操作的命令代码
        /// </summary>
        /// <returns></returns>
        protected virtual byte RemoteCommandCode() => 0x23;

        /// <summary>
        /// 远程操作的命令参数
        /// </summary>
        /// <returns></returns>
        protected virtual byte RemoteCommandPar() => 0;

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as RemoteRelay_Patameter;

            Packet(3, RemoteCommandCode(), RemoteCommandPar(), (uint)model.GetDataLen(), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
