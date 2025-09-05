using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 查询人员附加数据详情 详情
    /// </summary>
    public class ReadPersonDetail_Result : INCommandResult
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public int UserCode;

        /// <summary>
        /// 人员头像列表
        /// </summary>
        public byte[] PhotoList;

        /// <summary>
        /// 指纹列表
        /// </summary>
        public byte[] FingerprintList;

        /// <summary>
        /// 人脸特征码
        /// </summary>
        public bool HasFace;

        public void Dispose()
        {

        }

        /// <summary>
        /// 读取ByteBuffer内容
        /// </summary>
        /// <param name="buf"></param>
        public void SetBytes(IByteBuffer buf)
        {
            UserCode = buf.ReadInt();
            PhotoList = new byte[5];
            buf.ReadBytes(PhotoList, 0, 5);

            FingerprintList = new byte[10];
            buf.ReadBytes(FingerprintList, 0, 10);

            HasFace = buf.ReadBoolean();
        }
    }
}
