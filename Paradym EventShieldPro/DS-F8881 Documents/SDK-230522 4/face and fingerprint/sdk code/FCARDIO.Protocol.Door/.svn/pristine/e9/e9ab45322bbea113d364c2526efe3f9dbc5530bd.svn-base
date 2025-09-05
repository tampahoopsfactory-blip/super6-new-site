using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Door.Door8800.Door.ReaderAlarm
{
    /// <summary>
    /// 写入 读卡器防拆报警
    /// </summary>
    public class WriteReaderAlarm : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value"></param>
        public WriteReaderAlarm(INCommandDetail cd, WriteReaderAlarm_Parameter value) : base(cd, value) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteReaderAlarm_Parameter model = value as WriteReaderAlarm_Parameter;
            if (model == null) return false;

            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteReaderAlarm_Parameter model = _Parameter as WriteReaderAlarm_Parameter;
            Packet(0x03, 0x1A, 0x00, 2, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
