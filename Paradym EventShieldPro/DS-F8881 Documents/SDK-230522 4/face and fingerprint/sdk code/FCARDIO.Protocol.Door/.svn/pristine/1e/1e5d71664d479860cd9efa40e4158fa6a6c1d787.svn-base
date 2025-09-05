using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.OutputFormat
{
    /// <summary>
    /// 设置输出格式
    /// </summary>
    public class WriteOutputFormat : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteOutputFormat(INCommandDetail cd, WriteOutputFormat_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteOutputFormat_Parameter model = _Parameter as WriteOutputFormat_Parameter;
            Packet(0x01, 0x06, 2, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteOutputFormat_Parameter model = value as WriteOutputFormat_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }


    }
}
