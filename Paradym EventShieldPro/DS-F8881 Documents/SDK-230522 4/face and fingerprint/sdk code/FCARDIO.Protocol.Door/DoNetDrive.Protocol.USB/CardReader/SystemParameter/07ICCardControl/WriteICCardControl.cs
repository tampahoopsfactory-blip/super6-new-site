using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardControl
{
    /// <summary>
    /// 设置扇区验证
    /// </summary>
    public class WriteICCardControl : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteICCardControl(INCommandDetail cd, WriteICCardControl_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteICCardControl_Parameter model = _Parameter as WriteICCardControl_Parameter;
            Packet(0x01, 0x07, 0x1D, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteICCardControl_Parameter model = value as WriteICCardControl_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }


    }
}
