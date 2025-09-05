using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置识别结果查询二维码生成开关
    /// </summary>
    public class WriteRecordQRCode : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设置识别结果查询二维码生成开关的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteRecordQRCode(INCommandDetail cd, WriteRecordQRCode_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteRecordQRCode_Parameter;
            Packet(0x01, 0x35, 0x00, 121, model.GetBytes(GetNewCmdDataBuf(121)));
        }
    }
}
