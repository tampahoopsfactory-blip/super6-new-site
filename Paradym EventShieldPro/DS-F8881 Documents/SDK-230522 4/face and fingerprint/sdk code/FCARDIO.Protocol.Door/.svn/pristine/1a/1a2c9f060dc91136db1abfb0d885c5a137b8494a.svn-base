using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.AgencyCode
{
    /// <summary>
    /// 设置经销商代码
    /// </summary>
    public class WriteAgencyCode : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteAgencyCode(INCommandDetail cd, WriteAgencyCode_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteAgencyCode_Parameter model = _Parameter as WriteAgencyCode_Parameter;
            Packet(0x01, 0x0C, 4, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteAgencyCode_Parameter model = value as WriteAgencyCode_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }
    }
}
