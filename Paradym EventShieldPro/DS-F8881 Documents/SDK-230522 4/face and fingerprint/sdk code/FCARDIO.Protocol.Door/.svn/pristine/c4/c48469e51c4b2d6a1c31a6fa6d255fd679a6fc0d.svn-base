using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Elevator.FC8864.Password
{
    /// <summary>
    /// 读取密码容量信息
    /// </summary>
    public class ReadPasswordDetail : Protocol.Door.Door8800.Password.ReadPasswordDetail
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        public ReadPasswordDetail(INCommandDetail cd) : base(cd)
        {
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x45, 1);
        }

        /// <summary>
        /// 检测下一包指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected override bool CheckResponseOK(OnlineAccessPacket oPck)
        {
            return (oPck.CmdType == 0x55 &&
                oPck.CmdIndex == 1 &&
                oPck.CmdPar == 0);
        }
    }
}
