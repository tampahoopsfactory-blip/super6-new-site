using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.BalcklistAlarmOption
{
    /// <summary>
    /// 设置黑名单报警功能开关
    /// </summary>
    public class WriteBalcklistAlarmOption : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置黑名单报警功能开关
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含黑名单报警功能开关参数</param>
        public WriteBalcklistAlarmOption(INCommandDetail cd, WriteBalcklistAlarmOption_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteBalcklistAlarmOption_Parameter model = value as WriteBalcklistAlarmOption_Parameter;
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
            WriteBalcklistAlarmOption_Parameter model = _Parameter as WriteBalcklistAlarmOption_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x12, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}