using System;
using System.Collections.Generic;
using System.Text;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 写入电梯继电器板的继电器开锁输出时长
    /// </summary>
    public class WriteReleaseTime : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建写入电梯继电器板的继电器开锁输出时长的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">开锁输出时长参数</param>
        public WriteReleaseTime(INCommandDetail cd, WriteReleaseTime_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            var model = value as WriteReleaseTime_Parameter;
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
            var model = _Parameter as WriteReleaseTime_Parameter;

            Packet(0x03, 0x28, 0x01, (uint)model.GetDataLen(), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
