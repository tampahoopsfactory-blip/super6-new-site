using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.CardReader.ICCard.Sector
{
    /// <summary>
    /// 读扇区内容
    /// </summary>
    public class ReadSector : Read_Command
    {
        /// <summary>
        /// 获取控制器SN 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadSector(INCommandDetail cd, ReadSector_Parameter par) : base(cd, par)
        {
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            ReadSector_Parameter model = _Parameter as ReadSector_Parameter;
            Packet(2, 1,0x0A, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck, 2, 1))
            {
                var buf = oPck.CmdData;
                ReadSector_Result rst = new ReadSector_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
