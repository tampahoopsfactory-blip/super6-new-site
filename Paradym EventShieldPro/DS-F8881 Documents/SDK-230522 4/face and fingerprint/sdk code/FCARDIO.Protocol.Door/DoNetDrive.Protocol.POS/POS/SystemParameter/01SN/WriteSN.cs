using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS.SystemParameter.SN
{
    /// <summary>
    /// 写入控制器SN
    /// </summary>
    public class WriteSN : Write_Command
    {
        /// <summary>
        /// 命令数据中的前缀
        /// </summary>
        protected byte[] DataStrt;
        /// <summary>
        /// 命令数据中的后缀
        /// </summary>
        protected byte[] DataEnd;

        /// <summary>
        /// 写入控制器SN 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含SN数据</param>
        public WriteSN(DESDriveCommandDetail cd, SN_Parameter par) : base(cd, par)
        {
            DataStrt = new byte[] { 0x87, 0x97, 0x4f, 0x45, 0x77 };
            DataEnd = new byte[] { 0x49, 0xA7, 0x7D };
        }

        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return true;
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x01, 0xF0, 0x18, GetCmdData());
        }

        /// <summary>
        /// 创建命令所需的命令数据<br/>
        /// 将命令打包到ByteBuffer中
        /// </summary>
        /// <returns>包含命令数据的ByteBuffer</returns>
        protected IByteBuffer GetCmdData()
        {
            SN_Parameter model = _Parameter as SN_Parameter;

            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(0x18);
            buf.WriteBytes(DataStrt);
            buf.WriteBytes(DataEnd);
            model.GetBytes(buf);
            return buf;
        }
    }
}
