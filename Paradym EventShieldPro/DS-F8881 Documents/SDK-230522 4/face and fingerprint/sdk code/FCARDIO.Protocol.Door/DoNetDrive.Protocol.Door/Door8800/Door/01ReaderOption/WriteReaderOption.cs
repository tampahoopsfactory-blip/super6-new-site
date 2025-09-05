using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.ReaderOption
{
    /// <summary>
    /// 设置控制器4个门的读卡器字节数
    /// </summary>
    public class WriteReaderOption<T> : Door8800Command_WriteParameter where T : ReaderOption_Parameter,new ()
    {
        /// <summary>
        /// 设置控制器4个门的读卡器字节数
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含控制器4个门的读卡器字节数</param>
        public WriteReaderOption(INCommandDetail cd, T par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            T model = value as T;
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
            T model = _Parameter as T;

            Packet(0x03, 0x01, 0x01, 0x04, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

    }
}
