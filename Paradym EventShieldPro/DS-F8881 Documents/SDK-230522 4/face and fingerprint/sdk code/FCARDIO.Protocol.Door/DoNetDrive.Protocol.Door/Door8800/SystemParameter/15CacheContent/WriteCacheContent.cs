using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.CacheContent
{
    /// <summary>
    /// 设置缓存区内容
    /// </summary>
    public class WriteCacheContent : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置缓存区内容 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含缓存区内容</param>
        public WriteCacheContent(INCommandDetail cd, CacheContent_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            CacheContent_Parameter model = value as CacheContent_Parameter;
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
            CacheContent_Parameter model = _Parameter as CacheContent_Parameter;

            var acl = _Connector.GetByteBufAllocator();

            var buf = acl.Buffer(model.GetDataLen());

            Packet(0x01, 0xF0, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}
