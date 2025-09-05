using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor
{
    /// <summary>
    /// 根据SN设置网络标识
    /// </summary>
    public class WriteControltorNetCode : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 根据SN设置网络标识
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含网络标识参数</param>
        public WriteControltorNetCode(INCommandDetail cd, SearchControltor_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            SearchControltor_Parameter model = value as SearchControltor_Parameter;
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
            SearchControltor_Parameter model = _Parameter as SearchControltor_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0xFE, 0x01, 2, model.GetBytes(buf));
            if (model.UDPBroadcast)
            {
                DoorPacket.SetUDPBroadcastPacket();
            }
        }
    }
}