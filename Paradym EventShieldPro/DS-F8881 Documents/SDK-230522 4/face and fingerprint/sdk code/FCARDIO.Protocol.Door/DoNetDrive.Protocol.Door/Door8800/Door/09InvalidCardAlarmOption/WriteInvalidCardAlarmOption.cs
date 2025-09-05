using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Door.Door8800.Door.InvalidCardAlarmOption
{
    /// <summary>
    /// 设置非法读卡报警参数
    /// </summary>
    public class WriteInvalidCardAlarmOption : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置非法读卡报警参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含非法读卡报警参数</param>
        public WriteInvalidCardAlarmOption(INCommandDetail cd, WriteInvalidCardAlarmOption_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteInvalidCardAlarmOption_Parameter model = value as WriteInvalidCardAlarmOption_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteInvalidCardAlarmOption_Parameter model = _Parameter as WriteInvalidCardAlarmOption_Parameter;
            Packet(0x03, 0x0A, 0x00, 0x02, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

    }
}
