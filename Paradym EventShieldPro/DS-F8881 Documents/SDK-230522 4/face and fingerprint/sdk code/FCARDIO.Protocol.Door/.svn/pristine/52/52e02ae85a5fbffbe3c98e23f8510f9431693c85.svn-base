using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.ReaderWorkSetting
{
    /// <summary>
    /// 设置门认证方式
    /// </summary>
    public class WriteReaderWorkSetting : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置门认证方式
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含门</param>
        public WriteReaderWorkSetting(INCommandDetail cd, WriteReaderWorkSetting_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteReaderWorkSetting_Parameter model = value as WriteReaderWorkSetting_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteReaderWorkSetting_Parameter model = _Parameter as WriteReaderWorkSetting_Parameter;
            Packet(0x03, 0x05, 0x01, 0x119, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}
