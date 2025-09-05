using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.OperatedDevice.TriggerDoubleLamp
{
    /// <summary>
    /// 触发双色指示灯
    /// </summary>
    public class TriggerDoubleLamp : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public TriggerDoubleLamp(INCommandDetail cd, TriggerDoubleLamp_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            TriggerDoubleLamp_Parameter model = _Parameter as TriggerDoubleLamp_Parameter;
            Packet(0x05, 0x05, 1, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            TriggerDoubleLamp_Parameter model = value as TriggerDoubleLamp_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        
    }
}
