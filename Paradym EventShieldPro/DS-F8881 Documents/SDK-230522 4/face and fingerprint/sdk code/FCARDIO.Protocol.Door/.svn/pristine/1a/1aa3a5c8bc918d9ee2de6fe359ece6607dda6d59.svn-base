using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Door.RelayReleaseTime
{
    /// <summary>
    /// 开锁时输出时长_参数
    /// </summary>
    public class WriteUnlockingTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 开锁时输出时长（输出时长2字节，最大65535秒。0表示0.5秒）
        /// </summary>
        public ushort ReleaseTime;

        /// <summary>
        /// 提供给 RelayReleaseTime_Result 使用
        /// </summary>
        public WriteUnlockingTime_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="releaseTime">开锁时输出时长,单位秒，最大65535秒。0表示0.5秒</param>
        public WriteUnlockingTime_Parameter(ushort releaseTime)
        {
            ReleaseTime = releaseTime;
            checkedParameter();
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ReleaseTime < 0 || ReleaseTime > 65535)
                throw new ArgumentException("releaseTime Error!");
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 对参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteUnsignedShort(ReleaseTime);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x02;
        }

        /// <summary>
        /// 对参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            ReleaseTime = databuf.ReadUnsignedShort();
        }
    }
}
