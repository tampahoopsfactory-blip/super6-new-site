using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardControl
{
    /// <summary>
    /// 读取扇区验证
    /// </summary>
    public class ReadICCardControl : Read_Command
    {
        /// <summary>
        /// 获取控制器SN 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadICCardControl(INCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(1, 0x87);
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck, 1, 0x87, 0x1D))
            {
                var buf = oPck.CmdData;
                ReadICCardControl_Result rst = new ReadICCardControl_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }


    }
}
