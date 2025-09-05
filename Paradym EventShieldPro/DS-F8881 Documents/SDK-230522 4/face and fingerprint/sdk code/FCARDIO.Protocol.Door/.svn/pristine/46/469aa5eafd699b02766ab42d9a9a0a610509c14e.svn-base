using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.Menu
{
    /// <summary>
    /// 读取菜单信息命令
    /// </summary>
    public class ReadMenuDataBase : Read_Command
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="detail"></param>
        public ReadMenuDataBase(DESDriveCommandDetail detail) : base(detail, null) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return true;
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x06, 0x01, 0x00, 0x00, null);
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponse(oPck, 0x04))
            {
                var buf = oPck.CommandPacket.CmdData;
                ReadMenuDataBase_Result rst = new ReadMenuDataBase_Result();
                rst.SetBytes(buf);
                _Result = rst;

                CommandCompleted();
            }
        }

        /// <summary>
        /// 命令重发时需要处理的函数
        /// </summary>
        protected override void CommandReSend()
        {
            return;
        }

        /// <summary>
        /// 命令释放时需要处理的函数
        /// </summary>
        protected override void Release1()
        {
            return;
        }
    }
}
