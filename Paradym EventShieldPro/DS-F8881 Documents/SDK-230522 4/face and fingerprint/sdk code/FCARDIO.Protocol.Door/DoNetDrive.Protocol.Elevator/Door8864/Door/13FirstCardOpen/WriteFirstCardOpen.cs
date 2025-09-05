using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.FirstCardOpen
{
    /// <summary>
    /// 设置 首卡开门参数
    /// </summary>
    public class WriteFirstCardOpen : Write_Command
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteFirstCardOpen(INCommandDetail cd, WriteFirstCardOpen_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteFirstCardOpen_Parameter model = _Parameter as WriteFirstCardOpen_Parameter;
            Packet(0x43, 0x08, 0x00, 0x03, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteFirstCardOpen_Parameter model = value as WriteFirstCardOpen_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }


        
    }
}
