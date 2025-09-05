using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.RelayReleaseTime
{
    /// <summary>
    /// 开锁时输出时长_参数
    /// </summary>
    public class WriteRelayReleaseTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门
        /// </summary>
        public byte Door;

        /// <summary>
        /// 开锁时输出时长（输出时长2字节，最大65535秒。0表示0.5秒）
        /// </summary>
        public ushort ReleaseTime;

        /// <summary>
        /// 提供给 RelayReleaseTime_Result 使用
        /// </summary>
        public WriteRelayReleaseTime_Parameter() { }

        /// <summary>
        /// 开锁时输出时长参数初始化实例
        /// </summary>
        /// <param name="door">门端口</param>
        /// <param name="releaseTime">开锁时输出时长</param>
        public WriteRelayReleaseTime_Parameter(byte door, ushort releaseTime)
        {
            Door = door;
            ReleaseTime = releaseTime;
            checkedParameter();
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Door < 1 || Door > 4)
                throw new ArgumentException("Door Error!");

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
        /// 对开锁时输出时长参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteByte(Door);
            databuf.WriteUnsignedShort(ReleaseTime);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x03;
        }

        /// <summary>
        /// 对开锁时输出时长参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            Door = databuf.ReadByte();
            ReleaseTime = databuf.ReadUnsignedShort();
        }
    }
}
