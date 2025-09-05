using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 表示设置人脸机识别距离的命令
    /// </summary>
    public class WriteFaceIdentifyRange : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建人脸机识别距离的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteFaceIdentifyRange(INCommandDetail cd, WriteFaceIdentifyRange_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteFaceIdentifyRange_Parameter model = _Parameter as WriteFaceIdentifyRange_Parameter;
            Packet(0x01, 0x22, 0x00,1, model.GetBytes(GetNewCmdDataBuf(1)));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteFaceIdentifyRange_Parameter model = value as WriteFaceIdentifyRange_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }

    }
}
