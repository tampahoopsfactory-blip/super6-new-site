using DotNetty.Buffers;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.OperatedDevice.TriggerVibrate
{
    /// <summary>
    /// 触发震动原件 参数
    /// </summary>
    public class TriggerVibrate_Parameter : AbstractParameter
    {
        /// <summary>
        /// 震动时间
        /// </summary>
        public ushort Time;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="time">震动时间</param>
        public TriggerVibrate_Parameter(ushort time)
        {
            Time = time;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {

            return true;
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteUnsignedShort(Time);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 2;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

        }
    }
}
