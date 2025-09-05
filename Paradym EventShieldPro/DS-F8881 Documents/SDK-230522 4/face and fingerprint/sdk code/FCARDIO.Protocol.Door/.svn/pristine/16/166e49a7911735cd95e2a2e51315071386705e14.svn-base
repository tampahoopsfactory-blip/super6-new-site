using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.OperatedDevice.TriggerVibrate
{
    /// <summary>
    /// 触发震动原件
    /// </summary>
    public class TriggerVibrate : Write_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public TriggerVibrate(INCommandDetail cd, TriggerVibrate_Parameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            TriggerVibrate_Parameter model = _Parameter as TriggerVibrate_Parameter;
            Packet(0x05, 0x03, 2, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            TriggerVibrate_Parameter model = value as TriggerVibrate_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        
    }
}
