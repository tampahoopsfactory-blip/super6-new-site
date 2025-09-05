using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Password
{
    /// <summary>
    /// 读取密码容量信息
    /// </summary>
    public class ReadPasswordDetail : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        public ReadPasswordDetail(INCommandDetail cd) : base(cd, null)
        {
        }

        /// <summary>
        /// 检测下一包指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected virtual bool CheckResponseOK(OnlineAccessPacket oPck)
        {
            return (oPck.CmdType == 0x35 &&
                oPck.CmdIndex == 1 &&
                oPck.CmdPar == 0);
        }

        /// <summary>
        /// 处理返回通知
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponseOK(oPck))
            {
                var buf = oPck.CmdData;
                ReadPasswordDetail_Result rst = new ReadPasswordDetail_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x05, 1);
        }
    }
}
