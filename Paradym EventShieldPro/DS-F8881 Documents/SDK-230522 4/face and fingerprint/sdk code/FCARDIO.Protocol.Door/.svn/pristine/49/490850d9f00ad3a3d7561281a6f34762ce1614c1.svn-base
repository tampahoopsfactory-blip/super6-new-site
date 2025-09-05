using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.VoiceBroadcastSetting
{
    /// <summary>
    /// 设置语音播报功能
    /// </summary>
    public class WriteVoiceBroadcastSetting : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value">包括门号和出门按钮功能</param>
        public WriteVoiceBroadcastSetting(INCommandDetail cd, WriteVoiceBroadcastSetting_Parameter value) : base(cd, value) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteVoiceBroadcastSetting_Parameter model = value as WriteVoiceBroadcastSetting_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteVoiceBroadcastSetting_Parameter model = _Parameter as WriteVoiceBroadcastSetting_Parameter;
            Packet(0x03, 0x13, 0x00, 2, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

    }
}
