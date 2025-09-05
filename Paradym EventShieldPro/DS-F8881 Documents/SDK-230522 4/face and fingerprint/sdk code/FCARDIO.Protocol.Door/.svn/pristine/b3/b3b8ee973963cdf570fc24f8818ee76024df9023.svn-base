using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.AlarmPassword
{
    /// <summary>
    /// 写入胁迫报警功能
    /// 功能开启后，在密码键盘读卡器上输入特定密码后就会报警；
    /// </summary>
    public class WriteAlarmPassword
        : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter">包含门号和胁迫报警开关的结构</param>
        public WriteAlarmPassword(INCommandDetail cd, WriteAlarmPassword_parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteAlarmPassword_parameter model = value as WriteAlarmPassword_parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteAlarmPassword_parameter model = _Parameter as WriteAlarmPassword_parameter;
            Packet(0x03, 0x0B, 0x00, 0x07, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

    }
}
