using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置语音播报语音段开关
    /// </summary>
    public class WriteBroadcast : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteBroadcast
    {
        /// <summary>
        /// 设置语音播报语音段开关 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteBroadcast(INCommandDetail cd, WriteBroadcast_Parameter par) : base(cd, par) {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteBroadcast_Parameter model = _Parameter as WriteBroadcast_Parameter;

            Packet(0x41, 0x0A, 0x05, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}