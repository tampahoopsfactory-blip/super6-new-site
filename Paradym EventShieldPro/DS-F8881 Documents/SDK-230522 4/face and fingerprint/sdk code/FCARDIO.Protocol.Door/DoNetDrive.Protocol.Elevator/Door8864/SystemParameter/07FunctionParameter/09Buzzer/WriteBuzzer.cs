using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置主板蜂鸣器
    /// </summary>
    public class WriteBuzzer : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteBuzzer
    {
        /// <summary>
        /// 设置主板蜂鸣器 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteBuzzer(INCommandDetail cd, WriteBuzzer_Parameter par) : base(cd, par) {

        }
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteBuzzer_Parameter model = _Parameter as WriteBuzzer_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x41, 0x0A, 0x08, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}