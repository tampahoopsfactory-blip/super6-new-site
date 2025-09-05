using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.OEM
{
    /// <summary>
    /// 设置OEM信息
    /// </summary>
    public class WriteOEM : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 写入控制器SN 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含SN数据</param>
        public WriteOEM(INCommandDetail cd, OEM_Parameter par) : base(cd, par)
        {
        }

        /// <summary>
        /// 进行命令参数的检查
        /// </summary>
        /// <param name="value">命令包含的参数</param>
        /// <returns>true 表示检查通过，false 表示检查不通过</returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            OEM_Parameter model = value as OEM_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            OEM_Parameter model = _Parameter as OEM_Parameter;
            var buf = GetNewCmdDataBuf(127);
            Packet(0x01, 0x1E, 0x00, (uint)model.GetDataLen(), model.GetBytes(buf));
        }
    }
}
