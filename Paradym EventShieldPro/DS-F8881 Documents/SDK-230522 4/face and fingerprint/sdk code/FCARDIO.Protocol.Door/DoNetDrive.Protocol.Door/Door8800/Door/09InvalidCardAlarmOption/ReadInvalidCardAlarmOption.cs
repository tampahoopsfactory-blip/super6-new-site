using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.InvalidCardAlarmOption
{
    /// <summary>
    /// 读取非法读卡报警参数
    /// </summary>
    public class ReadInvalidCardAlarmOption : Door8800Command_Read_DoorParameter
    {
        /// <summary>
        /// 读取非法读卡报警参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含门号</param>
        public ReadInvalidCardAlarmOption(INCommandDetail cd, DoorPort_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            DoorPort_Parameter model = _Parameter as DoorPort_Parameter;
            Packet(0x03, 0x0A, 0x01, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x02))
            {
                var buf = oPck.CmdData;
                InvalidCardAlarmOption_Result rst = new InvalidCardAlarmOption_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
