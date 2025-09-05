using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 读取大型文件的返回值
    /// </summary>
    public class ReadFile_Result: ReadFeatureCode_Result
    {
        /// <summary>
        /// 读取ByteBuffer内容
        /// </summary>
        /// <param name="buf"></param>
        public override void SetBytes(IByteBuffer buf)
        {
            FileType = buf.ReadByte();
            UserCode = buf.ReadInt();
            FileHandle = buf.ReadInt();
            FileSize = buf.ReadInt();
            if (FileSize < 0)
            {
                FileHandle = 0;
                FileSize = 0;
            }
            FileCRC = buf.ReadUnsignedInt();

        }
    }


}
