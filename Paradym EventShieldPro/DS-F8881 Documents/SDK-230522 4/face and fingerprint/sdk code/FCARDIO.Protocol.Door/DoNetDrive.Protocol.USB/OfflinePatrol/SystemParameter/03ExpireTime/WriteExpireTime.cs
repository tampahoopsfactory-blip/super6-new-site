using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ExpireTime
{
    /// <summary>
    /// 写入设备有效期
    /// </summary>
    public class WriteExpireTime : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteExpireTime(INCommandDetail cd, ExpireTime_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            ExpireTime_Parameter model = _Parameter as ExpireTime_Parameter;
            Packet(0x01, 0x02, 3, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ExpireTime_Parameter model = value as ExpireTime_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

       
    }
}
