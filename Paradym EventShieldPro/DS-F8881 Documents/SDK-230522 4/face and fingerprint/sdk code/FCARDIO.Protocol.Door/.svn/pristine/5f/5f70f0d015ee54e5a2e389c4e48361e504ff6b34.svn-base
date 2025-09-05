using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.ManageMenuPassword
{
    /// <summary>
    /// 设置 管理菜单密码
    /// </summary>
    public class WriteManageMenuPassword : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteManageMenuPassword(INCommandDetail cd, WriteManageMenuPassword_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteManageMenuPassword_Parameter model = _Parameter as WriteManageMenuPassword_Parameter;
            Packet(0x01, 0x1d, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteManageMenuPassword_Parameter model = value as WriteManageMenuPassword_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }

       
    }
}
