using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.ExploreLockMode
{
    /// <summary>
    /// 设置防探测功能开关
    /// </summary>
    public class WriteExploreLockMode : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置防探测功能开关
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含防探测功能开关参数</param>
        public WriteExploreLockMode(INCommandDetail cd, WriteExploreLockMode_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteExploreLockMode_Parameter model = value as WriteExploreLockMode_Parameter;
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
            WriteExploreLockMode_Parameter model = _Parameter as WriteExploreLockMode_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0x12, 0x02, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}