using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 表示设备语言的命令参数
    /// </summary>
    public class WriteDriveLanguage_Parameter : AbstractParameter
    {
        /// <summary>
        /// 设备语言：1--中文；2--英文；3--繁体
        /// </summary>
        public int Language;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteDriveLanguage_Parameter() { Language = 1; }

        /// <summary>
        /// 创建设备语言的命令参数
        /// </summary>
        /// <param name="iLanguage">设备语言：1--中文；2--英文；3--繁体</param>
        public WriteDriveLanguage_Parameter(int iLanguage)
        {
            Language = iLanguage;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Language < 1 || Language > 120)
            {
                Language = 1;
            }

            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 编码参数
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Language);
            return databuf;
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != 1)
            {
                throw new ArgumentException("databuf Error");
            }
            Language = databuf.ReadByte();
        }
    }
}
