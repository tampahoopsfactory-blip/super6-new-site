using DoNetDrive.Core.Command;
using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.USBDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ReadFlag
{
    /// <summary>
    /// 发卡标记打开
    /// </summary>
    public class OpenReadFlag : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public OpenReadFlag(INCommandDetail cd) : base(cd, null)
        {
            _IsWaitResponse = false;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return true;
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x0E, 0, null);
        }

        
    }
}