using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 表示设备音量的命令参数
    /// </summary>
    public class WriteDriveVolume_Parameter : AbstractParameter
    {
        /// <summary>
        /// 设备音量：0-10；0--关闭声音；10--最大声音
        /// </summary>
        public int Volume;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteDriveVolume_Parameter() { Volume = 1; }

        /// <summary>
        /// 创建设备音量的命令参数
        /// </summary>
        /// <param name="iVolume">设备音量：0-10；0--关闭声音；10--最大声音</param>
        public WriteDriveVolume_Parameter(int iVolume)
        {
            Volume = iVolume;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Volume < 0 || Volume > 10)
            {
                Volume = 10;
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
            databuf.WriteByte(Volume);
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
            Volume = databuf.ReadByte();
        }
    }
}
