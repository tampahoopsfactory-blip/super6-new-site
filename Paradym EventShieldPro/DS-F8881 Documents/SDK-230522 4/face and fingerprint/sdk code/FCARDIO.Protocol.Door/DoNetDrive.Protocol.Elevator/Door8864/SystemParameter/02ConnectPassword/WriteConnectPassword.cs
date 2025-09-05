using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ConnectPassword
{
    /// <summary>
    /// 设置控制器通讯密码
    /// </summary>
    public class WriteConnectPassword : Write_Command
    {
        /// <summary>
        /// 设置控制器通讯密码 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含命令所需新的通讯密码</param>
        public WriteConnectPassword(INCommandDetail cd, Password_Parameter par) : base(cd, par) {

        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            Password_Parameter model = value as Password_Parameter;
            if (model == null)
            {
                return false;
            }

            return model.checkedParameter();
        }

        /// <summary>
        /// 拼装命令
        /// </summary>
        protected override void CreatePacket0()
        {
            Password_Parameter model = _Parameter as Password_Parameter;
            Packet(0x41, 0x03, 0x00, 4, model.GetBytes(_Connector.GetByteBufAllocator().Buffer(4)));
        }
    }
}