using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.AntiDisassemblyAlarm
{
    /// <summary>
    /// 设置 防拆报警功能 参数
    /// </summary>
    public class WriteAntiDisassemblyAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 是否启用,防拆开关断开后会记录控制板防拆报警，获取状态时会显示这个状态，当防拆开关短路后自动关闭报警，（除此之外不可关闭，除非关闭防拆功能。） 
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 提供给 RelayReleaseTime_Result 使用
        /// </summary>
        public WriteAntiDisassemblyAlarm_Parameter() { }

        /// <summary>
        /// 参数初始化实例
        /// </summary>
        /// <param name="isUse">是否启用</param>
        public WriteAntiDisassemblyAlarm_Parameter(bool isUse)
        {
            IsUse = isUse;
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
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 对参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteBoolean(IsUse);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x01;
        }

        /// <summary>
        /// 对参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsUse = databuf.ReadBoolean();
        }
    }
}
