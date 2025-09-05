using System;
using System.Collections.Generic;
using System.Text;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 设置定时常开参数的命令
    /// </summary>
    public class WriteTimingOpen : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设置定时常开参数的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">电梯工作模式命令的参数</param>
        public WriteTimingOpen(INCommandDetail cd, WriteTimingOpen_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            var model = value as WriteTimingOpen_Parameter;
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
            var model = _Parameter as WriteTimingOpen_Parameter;

            Packet(0x03, 0x26, 0x01, (uint)model.GetDataLen(), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
