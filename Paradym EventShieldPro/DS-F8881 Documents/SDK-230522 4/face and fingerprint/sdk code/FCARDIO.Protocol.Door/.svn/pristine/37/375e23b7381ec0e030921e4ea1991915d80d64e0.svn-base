using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.AntiPassback
{
    /// <summary>
    /// 写入防潜返
    /// 刷卡进门后，必须刷卡出门才能再次刷卡进门。
    /// </summary>
    public class WriteAntiPassback
        : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter">包含门号和防潜返功能</param>
        public WriteAntiPassback(INCommandDetail cd, WriteAntiPassback_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteAntiPassback_Parameter model = value as WriteAntiPassback_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteAntiPassback_Parameter model = _Parameter as WriteAntiPassback_Parameter;
            Packet(0x03, 0x0C, 0x01, 0x02, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


    }
}
