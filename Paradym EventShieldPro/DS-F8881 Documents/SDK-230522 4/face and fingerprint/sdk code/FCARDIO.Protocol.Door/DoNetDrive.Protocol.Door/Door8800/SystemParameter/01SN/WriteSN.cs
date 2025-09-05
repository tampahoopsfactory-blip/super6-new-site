using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN
{
    /// <summary>
    /// 写入控制器SN
    /// </summary>
    public class WriteSN : Door8800Command_WriteParameter
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
        public WriteSN(INCommandDetail cd, SN_Parameter par) : base(cd, par) {
            DataStrt = new byte[] { 0x03, 0xC5, 0x89, 0x12, 0x3E };
            DataEnd = new byte[] { 0x90, 0x7F, 0x78 };

        }

        /// <summary>
        /// 进行命令参数的检查
        /// </summary>
        /// <param name="value">命令包含的参数</param>
        /// <returns>true 表示检查通过，false 表示检查不通过</returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            SN_Parameter model = value as SN_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x01, 0x00, 0x18, GetCmdData());
        }

        /// <summary>
        /// 创建命令所需的命令数据<br/>
        /// 将命令打包到ByteBuffer中
        /// </summary>
        /// <returns>包含命令数据的ByteBuffer</returns>
        protected virtual IByteBuffer GetCmdData()
        {
            SN_Parameter model = _Parameter as SN_Parameter;

            var acl = _Connector.GetByteBufAllocator();
            string strDataStrt = System.Text.Encoding.ASCII.GetString(DataStrt);
            string strDataEnd = System.Text.Encoding.ASCII.GetString(DataEnd);
            var buf = acl.Buffer(0x18);
            buf.WriteBytes(DataStrt);
            model.GetBytes(buf);
            buf.WriteBytes(DataEnd);
            return buf;
        }

    }
}