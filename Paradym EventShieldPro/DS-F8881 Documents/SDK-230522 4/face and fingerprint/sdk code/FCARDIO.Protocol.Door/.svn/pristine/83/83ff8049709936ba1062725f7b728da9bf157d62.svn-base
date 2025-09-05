using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置 人脸、指纹比对阈值 参数
    /// </summary>
    public class WriteComparisonThreshold_Parameter : AbstractParameter
    {
        /// <summary>
        /// 人脸比对阈值（100-1）
        /// </summary>
        public byte FaceComparisonThreshold;

        /// <summary>
        /// 指纹比对阈值（100-1）
        /// </summary>
        public byte FingerprintComparisonThreshold;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteComparisonThreshold_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="faceComparisonThreshold">人脸比对阈值</param>
        /// <param name="fingerprintComparisonThreshold">指纹比对阈值</param>
        public WriteComparisonThreshold_Parameter(byte faceComparisonThreshold, byte fingerprintComparisonThreshold)
        {
            FaceComparisonThreshold = faceComparisonThreshold;
            FingerprintComparisonThreshold = fingerprintComparisonThreshold;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (FaceComparisonThreshold > 100 || FaceComparisonThreshold < 1)
            {
                return false;
            }
            if (FingerprintComparisonThreshold > 100 || FingerprintComparisonThreshold < 1)
            {
                return false;
            }

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
        /// 对记录存储方式参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(FingerprintComparisonThreshold).WriteByte(FaceComparisonThreshold);
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
        /// 对记录存储方式参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            FingerprintComparisonThreshold = databuf.ReadByte();
            FaceComparisonThreshold = databuf.ReadByte();
        }
    }
}
