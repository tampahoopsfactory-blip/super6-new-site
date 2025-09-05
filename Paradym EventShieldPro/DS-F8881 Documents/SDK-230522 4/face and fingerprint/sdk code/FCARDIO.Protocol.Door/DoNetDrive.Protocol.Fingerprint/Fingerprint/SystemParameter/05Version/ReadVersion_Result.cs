using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.Version
{
    /// <summary>
    /// 获取设备版本号_结果
    /// </summary>
    public class ReadVersion_Result : INCommandResult
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version;

        /// <summary>
        /// 指纹版本号
        /// </summary>
        public string FingerprintVersion;

        /// <summary>
        /// 人脸版本号
        /// </summary>
        public string FaceVersion;
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Version = null;
            return;
        }

        /// <summary>
        /// 对设备版本号参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public void SetBytes(IByteBuffer databuf)
        {
            Version = databuf.ReadString(4, System.Text.Encoding.ASCII);
            FingerprintVersion = databuf.ReadString(4, System.Text.Encoding.ASCII);
            FaceVersion = databuf.ReadString(3, System.Text.Encoding.ASCII);
            //版本号与修正号拼接
            Version = Version.Insert(2,".");
            FingerprintVersion = FingerprintVersion.Insert(2, ".");
            FaceVersion = FaceVersion.Insert(1, ".");
            //FingerprintVersion = databuf.ReadString(2, System.Text.Encoding.ASCII) + "." + databuf.ReadString(2, System.Text.Encoding.ASCII);
            //FaceVersion = databuf.ReadString(2, System.Text.Encoding.ASCII) + "." + databuf.ReadString(2, System.Text.Encoding.ASCII);
        }
    }
}
