using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.Version
{
    /// <summary>
    /// 获取设备运行信息 返回结果
    /// </summary>
    public class ReadVersion_Result : INCommandResult
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string VerNum;

        /// <summary>
        /// 修正号
        /// </summary>
        public string Revise;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
           
        }


        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            uint data = databuf.ReadUnsignedInt();
            byte[] list = DoNetDrive.Common.NumUtil.Int32ToByte(data);
            var str = System.Text.Encoding.ASCII.GetString(list);
            VerNum = str.Substring(0, 2);
            Revise = str.Substring(2, 2);
            //Revise = databuf.ReadUnsignedShort();
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
