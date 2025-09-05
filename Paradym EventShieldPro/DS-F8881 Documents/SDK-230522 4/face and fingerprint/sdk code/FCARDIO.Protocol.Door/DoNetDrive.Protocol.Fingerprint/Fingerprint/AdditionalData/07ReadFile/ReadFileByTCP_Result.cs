using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// TCP Client 模式下读文件 返回值
    /// </summary>
    public class ReadFileByTCP_Result : INCommandResult
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public int UserCode;

        /// <summary>
        /// 文件类型
        /// 1 - 人员头像
        /// 2 - 指纹
        /// 3 - 记录照片
        /// 4 - 红外人脸特征码
        /// 5 - 动态人脸特征码
        /// </summary>
        public int FileType;

        /// <summary>
        /// 文件序号
        /// </summary>
        public int FileSerialNumber;

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize;

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] FileDatas;

        /// <summary>
        /// CRC32校验数据
        /// </summary>
        public uint FileCRC;

        /// <summary>
        /// 指示命令执行结果
        /// </summary>
        public int ResultCode;


        public void Dispose()
        {

        }

        /// <summary>
        /// 读取ByteBuffer内容
        /// </summary>
        /// <param name="buf"></param>
        public virtual void SetBytes(IByteBuffer buf)
        {
            ResultCode = buf.ReadByte();
            FileSize = buf.ReadInt();
            FileCRC = buf.ReadUnsignedInt();
            FileDatas = new byte[FileSize];
            buf.ReadBytes(FileDatas, 0, FileSize);
        }
    }
}
