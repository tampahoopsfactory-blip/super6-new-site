using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.RecordStorageMode
{
    /// <summary>
    /// 写入记录存储方式
    /// </summary>
    public class WriteRecordStorageMode : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public WriteRecordStorageMode(INCommandDetail cd, WriteRecordStorageMode_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteRecordStorageMode_Parameter model = _Parameter as WriteRecordStorageMode_Parameter;
            Packet(0x01, 0x06, 1, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteRecordStorageMode_Parameter model = value as WriteRecordStorageMode_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        
    }
}
