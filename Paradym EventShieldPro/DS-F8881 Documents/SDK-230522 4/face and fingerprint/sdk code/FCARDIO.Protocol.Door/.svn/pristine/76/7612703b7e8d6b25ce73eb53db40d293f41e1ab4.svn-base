using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardCustomNum
{
    /// <summary>
    /// 设置卡号参数
    /// </summary>
    public class WriteICCardCustomNum : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteICCardCustomNum(INCommandDetail cd, WriteICCardCustomNum_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteICCardCustomNum_Parameter model = _Parameter as WriteICCardCustomNum_Parameter;
            Packet(0x01, 0x08, 0x0C, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteICCardCustomNum_Parameter model = value as WriteICCardCustomNum_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }


    }
}
