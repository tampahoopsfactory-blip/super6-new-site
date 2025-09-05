using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.StartupHoldTime
{
    /// <summary>
    /// 设置 开机保持时间
    /// </summary>
    public class WriteStartupHoldTime : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteStartupHoldTime(INCommandDetail cd, WriteStartupHoldTime_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteStartupHoldTime_Parameter model = _Parameter as WriteStartupHoldTime_Parameter;
            Packet(0x01, 0x0A, 1, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteStartupHoldTime_Parameter model = value as WriteStartupHoldTime_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

       
    }
}