using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.IllegalVerificationAlarm
{
    /// <summary>
    /// 设置 非法验证报警 参数
    /// </summary>
    public class WriteIllegalVerificationAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 非法认证次数
        /// 0 - 读一次卡就报警
        /// </summary>
        public byte Times;

        /// <summary>
        /// 提供给 继承类 使用
        /// </summary>
        public WriteIllegalVerificationAlarm_Parameter() { }

        /// <summary>
        /// 参数初始化实例
        /// </summary>
        /// <param name="isUse">是否启用</param>
        /// <param name="times">非法认证次数</param>
        public WriteIllegalVerificationAlarm_Parameter(bool isUse, byte times)
        {
            IsUse = isUse;
            Times = times;
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
            databuf.WriteByte(Times);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x02;
        }

        /// <summary>
        /// 对参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsUse = databuf.ReadBoolean();
            Times = databuf.ReadByte();
        }
    }
}
