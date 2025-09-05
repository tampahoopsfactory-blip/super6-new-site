using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入补光灯模式
    /// </summary>
    public class WriteFaceLEDMode : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建写入补光灯模式的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">补光灯模式参数</param>
        public WriteFaceLEDMode(INCommandDetail cd, WriteFaceLEDMode_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteFaceLEDMode_Parameter model = _Parameter as WriteFaceLEDMode_Parameter;
            Packet(0x01, 0x27, 0x00, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteFaceLEDMode_Parameter model = value as WriteFaceLEDMode_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }
    }
}
