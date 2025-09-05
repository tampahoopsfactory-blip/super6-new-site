using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.GateMagneticAlarm
{
    /// <summary>
    /// 设置门磁报警参数
    /// </summary>
    public class WriteGateMagneticAlarm : Write_Command
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteGateMagneticAlarm(INCommandDetail cd, WriteGateMagneticAlarm_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteGateMagneticAlarm_Parameter model = _Parameter as WriteGateMagneticAlarm_Parameter;
            Packet(0x43, 0x09, 0x00, 0x08, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteGateMagneticAlarm_Parameter model = value as WriteGateMagneticAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }


        
    }
}
