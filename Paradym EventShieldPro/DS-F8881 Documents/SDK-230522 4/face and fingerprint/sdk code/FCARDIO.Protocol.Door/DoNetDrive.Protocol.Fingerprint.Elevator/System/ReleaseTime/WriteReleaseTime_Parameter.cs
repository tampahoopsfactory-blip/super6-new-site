using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Core.Command;


namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 写入电梯继电器板的继电器开锁输出时长命令的参数
    /// </summary>
    public class WriteReleaseTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 继电器开锁输出时长列表，固定64个元素，每个元素代表一个继电器输出时长
        /// 输出时长取值范围： 0-65535 秒
        /// </summary>
        public List<int> ReleaseTimes;

        /// <summary>
        /// 创建写入电梯继电器板的继电器开锁输出时长命令的参数
        /// </summary>
        public WriteReleaseTime_Parameter()
        {
            ReleaseTimes = new List<int>(64);
        }

        /// <summary>
        /// 检查参数是否合法
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ReleaseTimes == null)
                return false;
            if (ReleaseTimes.Count != 64)
                return false;

            foreach (var time in ReleaseTimes)
            {
                if (time < 0 || time > 65535)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 返回实体二进制序列化后的长度
        /// </summary>
        /// <returns>长度</returns>
        public override int GetDataLen()
        {
            return 128;//64*2
        }

        /// <summary>
        /// 对定时常开参数命令的返回值进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes < GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            if (ReleaseTimes == null)
            {
                ReleaseTimes = new List<int>(64);
            }
            for (int i = 0; i < 64; i++)
            {
                ReleaseTimes.Add(databuf.ReadUnsignedShort());
            }
        }


        /// <summary>
        /// 对定时常开参数命令的返回值进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < GetDataLen())
            {
                throw new ArgumentException("databuf Error!");
            }
            for (int i = 0; i < 64; i++)
            {
                databuf.WriteUnsignedShort((ushort)ReleaseTimes[i]);
            }

            return databuf;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }
    }
}
