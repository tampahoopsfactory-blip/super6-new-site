using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.HTTPPageLandingSwitch
{
    /// <summary>
    /// 设置HTTP网页登陆开关
    /// </summary>
    public class WriteHTTPPageLandingSwitch : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置HTTP网页登陆开关
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含HTTP网页登陆开关参数</param>
        public WriteHTTPPageLandingSwitch(INCommandDetail cd, WriteHTTPPageLandingSwitch_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteHTTPPageLandingSwitch_Parameter model = value as WriteHTTPPageLandingSwitch_Parameter;
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
            WriteHTTPPageLandingSwitch_Parameter model = _Parameter as WriteHTTPPageLandingSwitch_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x19, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}