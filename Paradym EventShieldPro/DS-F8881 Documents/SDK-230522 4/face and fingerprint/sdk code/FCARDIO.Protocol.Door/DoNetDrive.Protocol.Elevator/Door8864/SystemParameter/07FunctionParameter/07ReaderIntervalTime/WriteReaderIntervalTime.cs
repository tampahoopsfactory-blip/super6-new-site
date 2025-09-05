using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置读卡间隔时间
    /// </summary>
    public class WriteReaderIntervalTime : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteReaderIntervalTime
    {
        /// <summary>
        /// 设置读卡间隔时间 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteReaderIntervalTime(INCommandDetail cd, WriteReaderIntervalTime_Parameter par) : base(cd, par) {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteReaderIntervalTime_Parameter model = _Parameter as WriteReaderIntervalTime_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x41, 0x0A, 0x06, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}