using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置读卡器数据校验
    /// </summary>
    public class WriteReaderCheckMode : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteReaderCheckMode
    {
        /// <summary>
        /// 设置读卡器数据校验 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteReaderCheckMode(INCommandDetail cd, WriteReaderCheckMode_Parameter par) : base(cd, par) {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteReaderCheckMode_Parameter model = _Parameter as WriteReaderCheckMode_Parameter;
            Packet(0x41, 0x0A, 0x07, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}