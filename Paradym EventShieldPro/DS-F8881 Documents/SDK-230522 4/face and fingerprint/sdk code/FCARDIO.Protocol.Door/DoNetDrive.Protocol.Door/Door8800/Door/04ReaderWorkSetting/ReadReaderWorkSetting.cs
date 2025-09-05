using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.ReaderWorkSetting
{
    /// <summary>
    /// 读取门认证方式
    /// </summary>
    public class ReadReaderWorkSetting : ReadReaderWorkSetting_Base<DoorPort_Parameter>
    {
        /// <summary>
        /// 读取门认证方式
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含门</param>
        public ReadReaderWorkSetting(INCommandDetail cd, DoorPort_Parameter par) : base(cd, par) { }

    }

}
