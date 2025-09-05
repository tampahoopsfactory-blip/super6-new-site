using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.LEDOpenHoldTime
{
    /// <summary>
    /// 设置 LED开灯保持时间
    /// </summary>
    public class WriteLEDOpenHoldTime : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteLEDOpenHoldTime(INCommandDetail cd, WriteLEDOpenHoldTime_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteLEDOpenHoldTime_Parameter model = _Parameter as WriteLEDOpenHoldTime_Parameter;
            Packet(0x01, 0x0C, 1, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteLEDOpenHoldTime_Parameter model = value as WriteLEDOpenHoldTime_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        
    }
}