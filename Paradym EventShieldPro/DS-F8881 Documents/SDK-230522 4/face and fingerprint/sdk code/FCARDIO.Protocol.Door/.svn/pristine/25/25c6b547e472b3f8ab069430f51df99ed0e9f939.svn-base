using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door
{
    /// <summary>
    /// 写入 开门验证方式
    /// </summary>
    public class WriteDoorOpenCheckMode : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建 设置开门验证方式 命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteDoorOpenCheckMode(INCommandDetail cd, WriteDoorOpenCheckMode_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteDoorOpenCheckMode_Parameter model = _Parameter as WriteDoorOpenCheckMode_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(1);

            Packet(0x03, 0x16, 0x00, 1, model.GetBytes(buf));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteDoorOpenCheckMode_Parameter model = value as WriteDoorOpenCheckMode_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }
    }
}
