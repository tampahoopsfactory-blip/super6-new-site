using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 通知设备播报语音
    /// </summary>
    public class SendCMD_BroadcastVoice : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建通知设备播报语音的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public SendCMD_BroadcastVoice(INCommandDetail cd, SendCMD_BroadcastVoice_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as SendCMD_BroadcastVoice_Parameter;
            var iLen = model.GetDataLen();
            Packet(0x0D, 0x03, 0x00, (uint)iLen, model.GetBytes(GetNewCmdDataBuf(iLen)));
        }
        
    }
}
