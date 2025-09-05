using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置离线卡刷卡开门功能
    /// </summary>
    public class WriteOfflineCardOpenModel : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设置离线卡刷卡开门功能的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteOfflineCardOpenModel(INCommandDetail cd, WriteOfflineCardOpenModel_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteOfflineCardOpenModel_Parameter;
            Packet(0x01, 0x38, 0x00, (uint)model.GetDataLen(), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
