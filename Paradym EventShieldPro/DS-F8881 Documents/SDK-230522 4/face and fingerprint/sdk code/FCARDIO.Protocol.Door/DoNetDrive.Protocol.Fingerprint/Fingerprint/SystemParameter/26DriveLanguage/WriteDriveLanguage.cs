using System;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入设备语言
    /// </summary>
    public class WriteDriveLanguage : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设备语言的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteDriveLanguage(INCommandDetail cd, WriteDriveLanguage_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteDriveLanguage_Parameter model = _Parameter as WriteDriveLanguage_Parameter;
            Packet(0x01, 0x25, 0x00, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteDriveLanguage_Parameter model = value as WriteDriveLanguage_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }
    }
}
