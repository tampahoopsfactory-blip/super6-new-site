using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Time.TimeErrorCorrection;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Elevator.FC8864.Time.TimeErrorCorrection
{
    /// <summary>
    /// 读取误差自修正参数
    /// </summary>
    public class ReadTimeError : Protocol.Door.Door8800.Time.TimeErrorCorrection.ReadTimeError
    {
        /// <summary>
        /// 读取误差自修正参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadTimeError(INCommandDetail cd) : base(cd) {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x42, 0x03);
        }

        /// <summary>
        /// 检查指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="dl">参数长度</param>
        /// <returns></returns>
        protected override bool CheckResponse(OnlineAccessPacket oPck, int dl)
        {
            return (oPck.DataLen == dl);

        }
    }
}