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
    /// 写扇区内容
    /// </summary>
    public class WriteSector : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteSector(INCommandDetail cd, WriteSector_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteSector_Parameter model = _Parameter as WriteSector_Parameter;
            Packet(0x02, 0x02, (uint)model.GetDataLen(), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteSector_Parameter model = value as WriteSector_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext0(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck, 2, 2))
            {
                var buf = oPck.CmdData;
                WriteSector_Result rst = new WriteSector_Result();
                rst.SetBytes(buf);

                _Result = rst;
                CommandCompleted();
            }
        }
    }
}
