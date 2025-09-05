using FCARDIO.Core.Command;
using System;

namespace FCARDIO.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置智能防盗主机参数
    /// </summary>
    public class WriteTheftAlarmSetting : Protocol.Door.FC8800.SystemParameter.FunctionParameter.WriteTheftAlarmSetting
    {
        /// <summary>
        /// 设置智能防盗主机参数 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含智能防盗主机参数</param>
        public WriteTheftAlarmSetting(INCommandDetail cd, WriteTheftAlarmSetting_Parameter par) : base(cd, par) {
            CmdType = 0x41;
            CmdIndex = 0x0A;
        }

    }
}