using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.RelayOption
{
    /// <summary>
    /// 设置继电器参数
    /// </summary>
    public class WriteRelayOption : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 继电器参数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteRelayOption(INCommandDetail cd, RelayOption_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            RelayOption_Parameter model = value as RelayOption_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x03, 0x02, 0x01, 0x01, GetCmdData());
        }

        /// <summary>
        /// 创建命令所需的命令数据<br/>
        /// 将命令打包到ByteBuffer中
        /// </summary>
        /// <returns>包含命令数据的ByteBuffer</returns>
        protected IByteBuffer GetCmdData()
        {
            RelayOption_Parameter model = _Parameter as RelayOption_Parameter;
            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(model.GetDataLen());
            model.GetBytes(buf);
            return buf;
        }
    }
}
