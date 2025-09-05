using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Door.ReaderWorkSetting;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.ReaderWorkSetting
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadReaderWorkSetting_Base<T> : Door8800Command_Read_DoorParameter where T : DoorPort_Parameter
    {
        /// <summary>
        /// 设置门认证方式
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含门</param>
        public ReadReaderWorkSetting_Base(INCommandDetail cd, T par) : base(cd, par) {
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x119))
            {
                var buf = oPck.CmdData;
                ReaderWorkSetting_Result rst = new ReaderWorkSetting_Result();
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
            DoorPort_Parameter model = _Parameter as DoorPort_Parameter;
            Packet(0x03, 0x05, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
