using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Door.ReadCardAndTakePictures
{
    /// <summary>
    /// 写入 读卡拍照联动消息
    /// </summary>
    public class WriteReadCardAndTakePictures : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        public WriteReadCardAndTakePictures(INCommandDetail cd, WriteReadCardAndTakePictures_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteReadCardAndTakePictures_Parameter model = value as WriteReadCardAndTakePictures_Parameter;
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
            WriteReadCardAndTakePictures_Parameter model = _Parameter as WriteReadCardAndTakePictures_Parameter;
            Packet(0x03, 0x1B, 0x00, 271, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}

