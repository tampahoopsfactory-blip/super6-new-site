using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ItemDetectionFunction
{
    /// <summary>
    /// 设置 物品检测功能
    /// </summary>
    public class WriteItemDetectionFunction : Write_Command
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public WriteItemDetectionFunction(INCommandDetail cd, WriteItemDetectionFunction_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteItemDetectionFunction_Parameter model = _Parameter as WriteItemDetectionFunction_Parameter;
            Packet(0x41, 0x81, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteItemDetectionFunction_Parameter model = value as WriteItemDetectionFunction_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }
    }
}
