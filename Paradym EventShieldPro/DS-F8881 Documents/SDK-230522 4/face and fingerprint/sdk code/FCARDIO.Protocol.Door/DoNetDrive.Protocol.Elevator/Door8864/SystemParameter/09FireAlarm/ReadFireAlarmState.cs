using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FireAlarm
{
    /// <summary>
    /// 读取消防报警状态
    /// </summary>
    public class ReadFireAlarmState : Protocol.Door.Door8800.SystemParameter.FireAlarm.ReadFireAlarmState
    {
       
        /// <summary>
        /// 读取消防报警状态 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadFireAlarmState(INCommandDetail cd) : base(cd) {
        }
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x41, 0x0C, 0x02);
        }
    }
}