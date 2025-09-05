using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.TCP485LineConnection
{
    /// <summary>
    /// 设置 TCP、485线路桥接
    /// </summary>
    public class Write485LineConnection : Protocol.Door.Door8800.SystemParameter.Check485Line.WriteCheck485Line
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public Write485LineConnection(INCommandDetail cd, Write485LineConnection_Parameter parameter) : base(cd, parameter) {

        }
        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Write485LineConnection_Parameter model = _Parameter as Write485LineConnection_Parameter;
            Packet(0x41, 0x80, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
