using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.LED
{
    /// <summary>
    /// 控制LED灯
    /// </summary>
    public class WriteLED : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteLED(INCommandDetail cd, WriteLED_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteLED_Parameter model = _Parameter as WriteLED_Parameter;
            Packet(0x01, 0x0B, 1, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteLED_Parameter model = value as WriteLED_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }
    }
}
