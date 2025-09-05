using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.InOutSideReadOpenSetting
{
    /// <summary>
    /// 设置门内外同时读卡开门
    /// </summary>
    public class WriteInOutSideReadOpenSetting : Door8800CommandEx
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value">包括门号和出门按钮功能</param>
        public WriteInOutSideReadOpenSetting(INCommandDetail cd, InOutSideReadOpenSetting_Parameter value) : base(cd, value) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            InOutSideReadOpenSetting_Parameter model = value as InOutSideReadOpenSetting_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            InOutSideReadOpenSetting_Parameter model = _Parameter as InOutSideReadOpenSetting_Parameter;
            Packet(0x03, 0x14, 0x00, 2, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            return;
        }

    }
}
