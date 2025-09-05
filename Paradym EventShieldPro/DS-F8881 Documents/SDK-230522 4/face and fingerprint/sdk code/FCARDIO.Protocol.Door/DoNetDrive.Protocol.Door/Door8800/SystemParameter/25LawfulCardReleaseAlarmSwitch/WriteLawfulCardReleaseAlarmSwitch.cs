using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.LawfulCardReleaseAlarmSwitch
{
    /// <summary>
    /// 设置开门超时报警时，合法卡解除报警开关
    /// </summary>
    public class WriteLawfulCardReleaseAlarmSwitch : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置开门超时报警时，合法卡解除报警开关
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含开门超时报警时，合法卡解除报警开关参数</param>
        public WriteLawfulCardReleaseAlarmSwitch(INCommandDetail cd, WriteLawfulCardReleaseAlarmSwitch_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteLawfulCardReleaseAlarmSwitch_Parameter model = value as WriteLawfulCardReleaseAlarmSwitch_Parameter;
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
            WriteLawfulCardReleaseAlarmSwitch_Parameter model = _Parameter as WriteLawfulCardReleaseAlarmSwitch_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x1A, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}