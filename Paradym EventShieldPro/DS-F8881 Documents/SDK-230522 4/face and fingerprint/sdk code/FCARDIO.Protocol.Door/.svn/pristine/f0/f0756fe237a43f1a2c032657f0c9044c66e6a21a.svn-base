using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.BlacklistAlarm
{
    /// <summary>
    /// 黑名单报警 参数
    /// </summary>
    public class WriteBlacklistAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 是否启用黑名单报警功能，如果此功能关闭则表示遇到黑名单刷卡不报警，只记录
        /// </summary>
        public bool IsAlarm = true;

        /// <summary>
        /// 提供给 继承类 使用
        /// </summary>
        public WriteBlacklistAlarm_Parameter() { }

        /// <summary>
        /// 参数初始化实例
        /// </summary>
        /// <param name="isAlarm">是否启用黑名单报警功能</param>
        public WriteBlacklistAlarm_Parameter(bool isAlarm)
        {
            IsAlarm = isAlarm;
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
            databuf.WriteBoolean(IsAlarm);
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
            IsAlarm = databuf.ReadBoolean();
        }
    }
}
