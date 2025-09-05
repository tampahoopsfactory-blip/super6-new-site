using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.TTLOutput
{
    /// <summary>
    /// 设置TTL输出参数
    /// </summary>
    public class WriteTTLOutput : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteTTLOutput(INCommandDetail cd, WriteTTLOutput_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteTTLOutput_Parameter model = _Parameter as WriteTTLOutput_Parameter;
            Packet(0x01, 0x09, 0x05, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteTTLOutput_Parameter model = value as WriteTTLOutput_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }


    }
}
