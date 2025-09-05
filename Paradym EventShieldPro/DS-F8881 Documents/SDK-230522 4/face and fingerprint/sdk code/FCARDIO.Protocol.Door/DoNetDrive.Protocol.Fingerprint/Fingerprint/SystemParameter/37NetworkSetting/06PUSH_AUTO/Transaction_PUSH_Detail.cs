using DotNetty.Buffers;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{

    /// <summary>
    /// 记录上传详情，包含目前记录上传断点、已上传记录数量统计
    /// </summary>
    public class Transaction_PUSH_Detail
    {
        /// <summary>
        /// 上传断点
        /// </summary>
        public uint ReadIndex;

        /// <summary>
        /// 待上传的数量
        /// </summary>
        public uint Readable;

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            ReadIndex = databuf.ReadUnsignedInt();
            Readable = databuf.ReadUnsignedInt();
        }
    }
}
