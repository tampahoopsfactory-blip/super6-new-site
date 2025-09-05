using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.ExemptionVerificationOpen
{
    /// <summary>
    /// 设置 免验证开门 参数
    /// </summary>
    public class WriteExemptionVerificationOpen_Parameter : AbstractParameter
    {
        /// <summary>
        /// 启用免验证
        /// </summary>
        public bool IsUseExemptionVerification;

        /// <summary>
        /// 启用自动注册
        /// </summary>
        public bool IsUseAutomaticRegistration;

        /// <summary>
        /// 自动注册时段编号
        /// </summary>
        public byte PeriodNumber;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteExemptionVerificationOpen_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="isUseExemptionVerification">启用免验证</param>
        /// <param name="isUseAutomaticRegistration">启用自动注册</param>
        /// <param name="periodNumber">自动注册时段编号</param>
        public WriteExemptionVerificationOpen_Parameter(bool isUseExemptionVerification, bool isUseAutomaticRegistration, byte periodNumber)
        {
            IsUseExemptionVerification = isUseExemptionVerification;
            IsUseAutomaticRegistration = isUseAutomaticRegistration;
            PeriodNumber = periodNumber;
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
            databuf.WriteBoolean(IsUseExemptionVerification);

            databuf.WriteBoolean(IsUseAutomaticRegistration);

            databuf.WriteByte(PeriodNumber);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 3;
        }

        /// <summary>
        /// 对参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsUseExemptionVerification = databuf.ReadBoolean();
            IsUseAutomaticRegistration = databuf.ReadBoolean();

            PeriodNumber = databuf.ReadByte();
        }
    }
}
