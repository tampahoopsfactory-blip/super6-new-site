using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.KeyboardCardIssuingManage
{
    /// <summary>
    /// 设置 键盘发卡管理功能
    /// </summary>
    public class WriteKeyboardCardIssuingManage : Write_Command
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public WriteKeyboardCardIssuingManage(INCommandDetail cd, WriteKeyboardCardIssuingManage_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteKeyboardCardIssuingManage_Parameter model = _Parameter as WriteKeyboardCardIssuingManage_Parameter;
            Packet(0x41, 0x11, 0x00, 0x05, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteKeyboardCardIssuingManage_Parameter model = value as WriteKeyboardCardIssuingManage_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

    }
}
