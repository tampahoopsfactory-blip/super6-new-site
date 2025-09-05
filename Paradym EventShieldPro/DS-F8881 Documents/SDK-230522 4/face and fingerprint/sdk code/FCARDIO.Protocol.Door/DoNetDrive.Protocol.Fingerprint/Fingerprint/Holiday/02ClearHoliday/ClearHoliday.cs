using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Holiday
{
    /// <summary>
    /// 清空人脸机中的所有节假日
    /// </summary>
    public class ClearHoliday : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 构造清空人脸机中的所有节假日命令，无需其他参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ClearHoliday(INCommandDetail cd) : base(cd, null)
        {
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x05, 2);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            return;
        }
    }
}
