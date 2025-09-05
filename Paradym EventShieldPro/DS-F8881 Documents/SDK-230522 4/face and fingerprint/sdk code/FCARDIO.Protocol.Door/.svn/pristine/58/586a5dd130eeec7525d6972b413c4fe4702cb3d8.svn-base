using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.WiegandOutput
{
    /// <summary>
    /// 设置 韦根输出
    /// </summary>
    public class WriteWiegandOutput : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteWiegandOutput(INCommandDetail cd, WriteWiegandOutput_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteWiegandOutput_Parameter model = value as WriteWiegandOutput_Parameter;
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
            WriteWiegandOutput_Parameter model = _Parameter as WriteWiegandOutput_Parameter;
            Packet(0x01, 0x1c, 0x02, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
