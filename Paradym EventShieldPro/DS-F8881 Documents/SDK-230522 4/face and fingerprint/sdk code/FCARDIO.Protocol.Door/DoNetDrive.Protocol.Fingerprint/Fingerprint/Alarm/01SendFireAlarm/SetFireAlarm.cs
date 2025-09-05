using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.Alarm
{

    /// <summary>
    /// 设置消防报警功能开关命令
    /// </summary>
    public class SetFireAlarm : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置消防报警功能开关命令 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public SetFireAlarm(INCommandDetail cd, SetFireAlarm_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 设置消防报警功能开关命令 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="use">功能开关</param>
        public SetFireAlarm(INCommandDetail cd, bool use) : base(cd, new SetFireAlarm_Parameter(use)) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            SetFireAlarm_Parameter model = value as SetFireAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            SetFireAlarm_Parameter model = _Parameter as SetFireAlarm_Parameter;
            Packet(0x04, 0x01, 0x01, 0x01, model.GetBytes(GetNewCmdDataBuf(1)));
        }
    }
}
