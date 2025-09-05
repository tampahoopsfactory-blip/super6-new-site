using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.SN
{
    /// <summary>
    /// 写入机器号
    /// </summary>
    public class WriteSN : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteSN(INCommandDetail cd, SN_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            SN_Parameter model = _Parameter as SN_Parameter;
            Packet(0x01, 0xF1, 1, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            SN_Parameter model = value as SN_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        
    }
}
