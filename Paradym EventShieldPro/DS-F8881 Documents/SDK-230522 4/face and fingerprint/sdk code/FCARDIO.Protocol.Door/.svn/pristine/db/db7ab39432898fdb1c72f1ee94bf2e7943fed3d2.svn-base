using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting
{
    /// <summary>
    /// 设置TCP参数
    /// </summary>
    public class WriteTCPSetting : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置控制器TCP参数 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含TCP参数信息</param>
        public WriteTCPSetting(INCommandDetail cd, WriteTCPSetting_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteTCPSetting_Parameter model = value as WriteTCPSetting_Parameter;
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
            WriteTCPSetting_Parameter model = _Parameter as WriteTCPSetting_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x06, 0x01, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
            var par = _Parameter as WriteTCPSetting_Parameter;

            if (par.UDPBroadcast)
            {
                DoorPacket.SetUDPBroadcastPacket();
            }
        }
    }
}