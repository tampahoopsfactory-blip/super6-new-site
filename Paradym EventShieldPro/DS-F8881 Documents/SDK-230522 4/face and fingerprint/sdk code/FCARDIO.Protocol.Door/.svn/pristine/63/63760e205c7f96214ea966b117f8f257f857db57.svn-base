using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.DataEncryptionSwitch
{
    /// <summary>
    /// 设置 数据包加密开关
    /// </summary>
    public class WriteDataEncryptionSwitch : Door8800Command_WriteParameter
    {
        /// <summary>
        ///  初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteDataEncryptionSwitch(INCommandDetail cd, WriteDataEncryptionSwitch_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteDataEncryptionSwitch_Parameter model = value as WriteDataEncryptionSwitch_Parameter;
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
            WriteDataEncryptionSwitch_Parameter model = _Parameter as WriteDataEncryptionSwitch_Parameter;
            Packet(0x01, 0x17, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
