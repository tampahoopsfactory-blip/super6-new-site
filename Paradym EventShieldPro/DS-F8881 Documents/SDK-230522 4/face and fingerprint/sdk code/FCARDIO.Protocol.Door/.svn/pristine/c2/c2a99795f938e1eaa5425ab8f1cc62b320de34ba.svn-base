using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.OperatedDevice.LCDFlash
{
    /// <summary>
    /// LCD屏幕刷屏
    /// </summary>
    public class LCDFlash : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public LCDFlash(INCommandDetail cd, LCDFlash_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            LCDFlash_Parameter model = _Parameter as LCDFlash_Parameter;
            Packet(0x05, 0x04, 1, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            LCDFlash_Parameter model = value as LCDFlash_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

       
    }
}
