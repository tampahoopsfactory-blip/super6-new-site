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
    /// 读取开门超时报警功能<br/>
    /// 门磁打开超过一定时间后就会报警和发出提示语音和响声。
    /// 成功返回结果参考 ReadOvertimeAlarmSetting_Result 
    /// </summary>
    public class ReadOvertimeAlarmSetting
        : Door8800Command_Read_DoorParameter
    {
        /// <summary>
        /// 初始化命令 结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value">需要读取的门号结构</param>
        public ReadOvertimeAlarmSetting(INCommandDetail cd, DoorPort_Parameter value) : base(cd, value) { }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            DoorPort_Parameter model = _Parameter as DoorPort_Parameter;
            Packet(0x03, 0x0E, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x05))
            {
                var buf = oPck.CmdData;
                OvertimeAlarmSetting_Result rst = new OvertimeAlarmSetting_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

    }
}
