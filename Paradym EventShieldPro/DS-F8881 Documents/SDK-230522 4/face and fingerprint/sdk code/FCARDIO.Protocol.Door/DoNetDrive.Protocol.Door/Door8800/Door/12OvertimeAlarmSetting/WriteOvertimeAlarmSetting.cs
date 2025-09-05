using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.OvertimeAlarmSetting
{
    /// <summary>
    /// 写入开门超时报警功能<br/>
    /// 门磁打开超过一定时间后就会报警和发出提示语音和响声。
    /// 成功返回结果参考 ReadOvertimeAlarmSetting_Result 
    /// </summary>
    public class WriteOvertimeAlarmSetting
        : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter">包含门号和防潜返功能</param>
        public WriteOvertimeAlarmSetting(INCommandDetail cd, WriteOvertimeAlarmSetting_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteOvertimeAlarmSetting_Parameter model = value as WriteOvertimeAlarmSetting_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteOvertimeAlarmSetting_Parameter model = _Parameter as WriteOvertimeAlarmSetting_Parameter;
            Packet(0x03, 0x0E, 0x01, 0x05, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

    }
}
