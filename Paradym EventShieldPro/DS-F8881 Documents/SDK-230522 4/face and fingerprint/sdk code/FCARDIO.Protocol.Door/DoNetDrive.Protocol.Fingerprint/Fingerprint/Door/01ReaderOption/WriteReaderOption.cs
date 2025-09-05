using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.ReaderOption
{
    /// <summary>
    /// 设置读卡器字节数
    /// </summary>
    public class WriteReaderOption : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteReaderOption(INCommandDetail cd, ReaderOption_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReaderOption_Parameter model = value as ReaderOption_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            ReaderOption_Parameter model = _Parameter as ReaderOption_Parameter;

            Packet(0x03, 0x01, 0x01, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

    }
}
