using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.KeepAliveInterval
{
    /// <summary>
    /// 设置控制器作为客户端时，和服务器的保活间隔时间
    /// </summary>
    public class WriteKeepAliveInterval : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置保活间隔时间 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含保活间隔时间</param>
        public WriteKeepAliveInterval(INCommandDetail cd, WriteKeepAliveInterval_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteKeepAliveInterval_Parameter model = value as WriteKeepAliveInterval_Parameter;
            if (model == null)
            {
                return false;
            }

            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteKeepAliveInterval_Parameter model = _Parameter as WriteKeepAliveInterval_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0xF0, 0x02, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}