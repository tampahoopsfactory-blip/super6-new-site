using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置离线记录推送开关
    /// </summary>
    public class WriteOfflineRecordPush : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设置离线记录推送开关的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteOfflineRecordPush(INCommandDetail cd, WriteOfflineRecordPush_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 创建设置离线记录推送开关的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="open">开关</param>
        public WriteOfflineRecordPush(INCommandDetail cd, bool open) : base(cd, new WriteOfflineRecordPush_Parameter(open)) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteOfflineRecordPush_Parameter;
            Packet(0x01, 0x37, 0x00, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }
    }
}
