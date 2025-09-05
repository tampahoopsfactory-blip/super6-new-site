using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.AntiDisassemblyAlarm
{
    /// <summary>
    /// 设置 防拆报警功能
    /// </summary>
    public class WriteAntiDisassemblyAlarm : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteAntiDisassemblyAlarm(INCommandDetail cd, WriteAntiDisassemblyAlarm_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteAntiDisassemblyAlarm_Parameter model = _Parameter as WriteAntiDisassemblyAlarm_Parameter;
            Packet(0x04, 0x03, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(1)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteAntiDisassemblyAlarm_Parameter model = value as WriteAntiDisassemblyAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

    }
}
