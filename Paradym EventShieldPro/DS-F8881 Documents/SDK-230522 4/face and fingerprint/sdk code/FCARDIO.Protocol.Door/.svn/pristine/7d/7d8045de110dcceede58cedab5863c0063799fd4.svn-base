using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.ExpirationPrompt
{
    /// <summary>
    /// 权限到期提示参数
    /// </summary>
    public class WriteExpirationPrompt_Parameter : AbstractParameter
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 有效期阀值
        /// 1-255
        /// </summary>
        public byte Time;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteExpirationPrompt_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="isUse">是否启用</param>
        /// <param name="time">有效期阀值</param>
        public WriteExpirationPrompt_Parameter(bool isUse,byte time)
        {
            IsUse = isUse;
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
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 对主板蜂鸣器参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteBoolean(IsUse).WriteByte(Time);
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
        /// 对主板蜂鸣器参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsUse = databuf.ReadBoolean();
            Time = databuf.ReadByte();
        }
    }
}
