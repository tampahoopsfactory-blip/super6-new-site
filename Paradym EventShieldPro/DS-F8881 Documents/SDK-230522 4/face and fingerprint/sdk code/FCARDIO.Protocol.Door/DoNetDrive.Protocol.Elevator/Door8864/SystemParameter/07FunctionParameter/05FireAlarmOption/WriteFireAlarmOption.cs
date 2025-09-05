using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置消防报警参数
    /// </summary>
    public class WriteFireAlarmOption : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteFireAlarmOption
    {
        /// <summary>
        /// 设置消防报警参数 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含消防报警参数</param>
        public WriteFireAlarmOption(INCommandDetail cd, WriteFireAlarmOption_Parameter par) : base(cd, par) {

        }
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteFireAlarmOption_Parameter model = _Parameter as WriteFireAlarmOption_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x41, 0x0A, 0x04, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}