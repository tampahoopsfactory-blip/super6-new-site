using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.LegalVerificationCloseAlarm
{
    /// <summary>
    /// 设置 合法验证解除报警开关
    /// </summary>
    public class WriteLegalVerificationCloseAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 提供给 RelayReleaseTime_Result 使用
        /// </summary>
        public WriteLegalVerificationCloseAlarm_Parameter() { }

        /// <summary>
        /// 参数初始化实例
        /// </summary>
        /// <param name="isUse">是否启用</param>
        public WriteLegalVerificationCloseAlarm_Parameter(bool isUse)
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
