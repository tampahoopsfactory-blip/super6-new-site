using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.TTLOutput
{
    /// <summary>
    /// 读取TTL输出参数
    /// </summary>
    public class ReadTTLOutput : Read_Command
    {
        /// <summary>
        /// 获取控制器SN 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadTTLOutput(INCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(1, 0x89);
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck, 1, 0x89, 0x05))
            {
                var buf = oPck.CmdData;
                ReadTTLOutput_Result rst = new ReadTTLOutput_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }


    }
}