using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.ReaderIntervalTime
{
    /// <summary>
    /// 设置重复验证权限间隔
    /// </summary>
    public class WriteReaderIntervalTime : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置读卡间隔时间 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteReaderIntervalTime(INCommandDetail cd, WriteReaderIntervalTime_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteReaderIntervalTime_Parameter model = _Parameter as WriteReaderIntervalTime_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(4);

            Packet(0x03, 0x14, 0x00, 4, model.GetBytes(buf));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteReaderIntervalTime_Parameter model = value as WriteReaderIntervalTime_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }

        
    }
}