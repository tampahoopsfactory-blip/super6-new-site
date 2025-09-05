using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.KeyboardCardIssuingManage
{
    /// <summary>
    /// 读取键盘发卡管理功能
    /// </summary>
    public class ReadKeyboardCardIssuingManage : Read_Command
    {

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        public ReadKeyboardCardIssuingManage(INCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x41, 0x11, 0x01);
        }


        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x05))
            {
                var buf = oPck.CmdData;
                ReadKeyboardCardIssuingManage_Result rst = new ReadKeyboardCardIssuingManage_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
