using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.ReadCardType
{
    /// <summary>
    /// 写入记录存储方式
    /// </summary>
    public class WriteReadCardType : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteReadCardType(INCommandDetail cd, WriteReadCardType_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteReadCardType_Parameter model = _Parameter as WriteReadCardType_Parameter;
            Packet(0x01, 0x05, 2, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteReadCardType_Parameter model = value as WriteReadCardType_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        
    }
}
