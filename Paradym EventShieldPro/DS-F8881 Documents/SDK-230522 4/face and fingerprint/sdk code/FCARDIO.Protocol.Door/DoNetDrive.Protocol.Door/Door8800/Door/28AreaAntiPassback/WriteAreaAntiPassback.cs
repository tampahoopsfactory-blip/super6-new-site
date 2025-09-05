using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.AreaAntiPassback
{
    /// <summary>
    /// 设置区域防潜回功能
    /// </summary>
    public class WriteAreaAntiPassback : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value">包括门号和出门按钮功能</param>
        public WriteAreaAntiPassback(INCommandDetail cd, WriteAreaAntiPassback_Parameter value) : base(cd, value) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteAreaAntiPassback_Parameter model = value as WriteAreaAntiPassback_Parameter;
            if (model == null) return false;
            
            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteAreaAntiPassback_Parameter model = _Parameter as WriteAreaAntiPassback_Parameter;
            Packet(0x03, 0x19, 0x00, 25, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


    }
}
