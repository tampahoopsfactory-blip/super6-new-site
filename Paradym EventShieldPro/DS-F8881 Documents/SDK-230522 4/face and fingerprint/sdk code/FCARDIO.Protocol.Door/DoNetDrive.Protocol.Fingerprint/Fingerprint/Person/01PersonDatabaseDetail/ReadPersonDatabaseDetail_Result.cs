using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 控制器中的人员数据库信息
    /// </summary>
    public class ReadPersonDatabaseDetail_Result : INCommandResult
    {
        /// <summary>
        /// 人员档案最大容量
        /// </summary>
        public long SortDataBaseSize;

        /// <summary>
        /// 已存储人员数量
        /// </summary>
        public long SortPersonSize;


        /// <summary>
        /// 指纹特征码最大容量
        /// </summary>
        public long SortFingerprintDataBaseSize;

        /// <summary>
        /// 已存储指纹数量
        /// </summary>
        public long SortFingerprintSize;

        /// <summary>
        /// 人脸特征码最大容量
        /// </summary>
        public long SortFaceDataBaseSize;

        /// <summary>
        /// 已存储人脸数量
        /// </summary>
        public long SortFaceSize;

        /// <summary>
        /// 创建结构
        /// </summary>
        public ReadPersonDatabaseDetail_Result()
        {
        }

        /// <summary>
        /// 读取ByteBuffer内容
        /// </summary>
        /// <param name="buf"></param>
        public void SetBytes(IByteBuffer buf)
        {
            SortDataBaseSize = buf.ReadUnsignedInt();
            SortPersonSize = buf.ReadUnsignedInt();
            SortFingerprintDataBaseSize = buf.ReadUnsignedInt();
            SortFingerprintSize = buf.ReadUnsignedInt();
            SortFaceDataBaseSize = buf.ReadUnsignedInt();
            SortFaceSize = buf.ReadUnsignedInt();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            return;
        }
    }
}
