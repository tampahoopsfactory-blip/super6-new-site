using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.CloseAlarm
{
    /// <summary>
    /// 解除报警
    /// </summary>
    public class WriteCloseAlarm : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteCloseAlarm(INCommandDetail cd, WriteCloseAlarm_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteCloseAlarm_Parameter model = _Parameter as WriteCloseAlarm_Parameter;
            Packet(0x41, 0x0D, 0, 2, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteCloseAlarm_Parameter model = value as WriteCloseAlarm_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

    }
}