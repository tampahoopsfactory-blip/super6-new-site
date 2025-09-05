using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.Version
{
    /// <summary>
    /// 获取设备运行信息 返回结果
    /// </summary>
    public class ReadVersion_Result : INCommandResult
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version;
        /// <summary>
        /// 修正号
        /// </summary>
        public string CorrectionNumber;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Version = null;
            CorrectionNumber = null;
        }


        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            Version = databuf.ReadString(2, System.Text.Encoding.ASCII);
            CorrectionNumber = databuf.ReadString(2, System.Text.Encoding.ASCII);
            //版本号与修正号拼接
            Version = $"{Version}.{CorrectionNumber}";
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf;
        }
    }
}
