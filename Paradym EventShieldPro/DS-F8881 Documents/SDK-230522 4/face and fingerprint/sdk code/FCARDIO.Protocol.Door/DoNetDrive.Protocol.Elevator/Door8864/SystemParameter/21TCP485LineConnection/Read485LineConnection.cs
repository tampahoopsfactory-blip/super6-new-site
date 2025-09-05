using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.TCP485LineConnection
{
    /// <summary>
    /// 读取 TCP、485线路桥接
    /// </summary>
    public class Read485LineConnection : Protocol.Door.Door8800.SystemParameter.Check485Line.ReadCheck485Line
    {

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        public Read485LineConnection(INCommandDetail cd) : base(cd)
        {
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x41, 0x80, 0x01);
        }

        /// <summary>
        /// 检查指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="dl">参数长度</param>
        /// <returns></returns>
        protected override bool CheckResponse(OnlineAccessPacket oPck, int dl)
        {
            return (oPck.DataLen == dl);

        }
    }
}
