using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.InputTerminalFunction
{
    /// <summary>
    /// 读取输入端子功能定义
    /// </summary>
    public class ReadInputTerminalFunction : Read_Command
    {

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        public ReadInputTerminalFunction(INCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x41, 0x12, 0x01);
        }


        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x01))
            {
                var buf = oPck.CmdData;
                ReadInputTerminalFunction_Result rst = new ReadInputTerminalFunction_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
