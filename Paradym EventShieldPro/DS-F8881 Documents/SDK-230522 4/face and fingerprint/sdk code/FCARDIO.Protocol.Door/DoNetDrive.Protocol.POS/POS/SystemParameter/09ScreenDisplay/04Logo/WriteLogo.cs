using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay
{
    /// <summary>
    /// 设置开机供应商Logo命令
    /// </summary>
    public class WriteLogo : Write_Command
    {
        /// <summary>
        /// 设置开机供应商Logo 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含开机供应商Logo</param>
        public WriteLogo(Protocol.DESDriveCommandDetail cd, WriteLogo_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteLogo_Parameter model = value as WriteLogo_Parameter;
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
            WriteLogo_Parameter model = _Parameter as WriteLogo_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x09, 0x06, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}
