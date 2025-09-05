using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN
{
    /// <summary>
    /// 获取控制器SN
    /// </summary>
    public class ReadSN : Read_Command
    {
        /// <summary>
        /// 获取控制器SN 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadSN(INCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(1, 1);
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck, 1, 1, 1))
            {
                var buf = oPck.CmdData;
                SN_Result rst = new SN_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }


    }
}
