using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.OEM
{
    /// <summary>
    /// 读取OEM信息，包含 制造商、网址、出厂日期
    /// </summary>
    public class ReadOEM : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 读取OEM信息
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadOEM(INCommandDetail cd) : base(cd)
        {
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(1, 0x1E, 1);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 127))
            {
                var buf = oPck.CmdData;
                OEMDetail oem = new OEMDetail();
                oem.SetBytes(buf);

                OEM_Result rst = new OEM_Result(oem);
                _Result = rst;
                CommandCompleted();
            }
        }

    }
}



