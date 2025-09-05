using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.InputTerminalFunction
{
    /// <summary>
    /// 设置 输入端子功能定义
    /// </summary>
    public class WriteInputTerminalFunction : Write_Command
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public WriteInputTerminalFunction(INCommandDetail cd, WriteInputTerminalFunction_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteInputTerminalFunction_Parameter model = _Parameter as WriteInputTerminalFunction_Parameter;
            Packet(0x41, 0x12, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteInputTerminalFunction_Parameter model = value as WriteInputTerminalFunction_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }
    }
}
