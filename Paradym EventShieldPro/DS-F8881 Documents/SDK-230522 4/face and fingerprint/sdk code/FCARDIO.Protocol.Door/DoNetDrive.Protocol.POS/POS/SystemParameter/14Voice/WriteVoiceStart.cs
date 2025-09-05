using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.Voice
{
    public class WriteVoiceStart : Write_Command
    {
        /// <summary>
        /// 设置U盘 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteVoiceStart(Protocol.DESDriveCommandDetail cd, WriteVoiceStart_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteVoiceStart_Parameter model = value as WriteVoiceStart_Parameter;
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
            WriteVoiceStart_Parameter model = _Parameter as WriteVoiceStart_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x0E, 0x02, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}
