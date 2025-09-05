using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.Version
{
    /// <summary>
    /// 获取设备版本号
    /// </summary>
    public class ReadVersion : Read_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadVersion(INCommandDetail cd) : base(cd, null)
        {

        }
        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck,1,3, 4))
            {
                var buf = oPck.CmdData;
                ReadVersion_Result rst = new ReadVersion_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(1, 3);
        }
    }
}
