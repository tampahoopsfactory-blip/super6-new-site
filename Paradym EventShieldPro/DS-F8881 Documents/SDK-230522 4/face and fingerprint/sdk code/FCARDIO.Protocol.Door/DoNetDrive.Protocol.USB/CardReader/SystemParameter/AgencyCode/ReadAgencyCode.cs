using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.AgencyCode
{
    /// <summary>
    /// 读取经销商代码
    /// </summary>
    public class ReadAgencyCode : Read_Command
    {
        /// <summary>
        /// 获取控制器SN 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadAgencyCode(INCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(1, 0x0D);
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck, 1, 0x0D, 4))
            {
                var buf = oPck.CmdData;
                ReadAgencyCode_Result rst = new ReadAgencyCode_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
