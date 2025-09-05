using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置烟雾报警参数
    /// </summary>
    public class WriteSmogAlarmOption : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteSmogAlarmOption
    {
        /// <summary>
        /// 设置烟雾报警参数 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含烟雾报警参数</param>
        public WriteSmogAlarmOption(INCommandDetail cd, WriteSmogAlarmOption_Parameter par) : base(cd, par) {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteSmogAlarmOption_Parameter model = _Parameter as WriteSmogAlarmOption_Parameter;

            Packet(0x41, 0x0A, 0x0B, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}