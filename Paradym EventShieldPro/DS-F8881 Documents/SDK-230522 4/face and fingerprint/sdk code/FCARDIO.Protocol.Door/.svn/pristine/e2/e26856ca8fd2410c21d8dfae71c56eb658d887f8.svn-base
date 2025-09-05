using FCARDIO.Core.Command;
using FCARDIO.Protocol.OnlineAccess;

namespace FCARDIO.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 获取智能防盗主机参数
    /// </summary>
    public class ReadTheftAlarmSetting : Protocol.Door.FC8800.SystemParameter.FunctionParameter.ReadTheftAlarmSetting
    {
        /// <summary>
        /// 获取智能防盗主机参数 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadTheftAlarmSetting(INCommandDetail cd) : base(cd) {
            CmdType = 0x41;
            CmdIndex = 0x0A;
        }
    }
}