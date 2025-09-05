using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 使设备立刻进入点名模式
    /// </summary>
    public class SendCMD_BeginAttendance : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建使设备立刻进入点名模式的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public SendCMD_BeginAttendance(INCommandDetail cd, SendCMD_BeginAttendance_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as SendCMD_BeginAttendance_Parameter;
            var iLen =model.GetDataLen();
            Packet(0x0D, 0x01, 0x02,(uint)iLen, model.GetBytes(GetNewCmdDataBuf(iLen)));
        }

    }
}
