using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 远程操作时的继电器列表参数
    /// </summary>
    public class RemoteRelay_Patameter : AbstractParameter
    {
        /// <summary>
        /// 继电器列表，固定64个元素，每个元素代表一个继电器类型
        /// 输出类型：
        /// 0、不操作此继电器
        /// 1、对继电器执行操作
        /// </summary>
        public List<byte> Relays;


        /// <summary>
        /// 创建远程操作时的继电器列表参数
        /// </summary>
        public RemoteRelay_Patameter()
        {
            Relays = new List<byte>(64);
        }

        /// <summary>
        /// 检查参数是否合法
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Relays == null)
                return false;
            if (Relays.Count != 64)
                return false;

            foreach (var relay in Relays)
            {
                if (relay > 1)
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
            return 64;
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
            if (Relays == null)
            {
                Relays = new List<byte>(64);
            }

            for (int i = 0; i < 64; i++)
            {
                Relays.Add(databuf.ReadByte());
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
            if (Relays == null)
            {
                Relays = new List<byte>(64);
            }
            for (int i = 0; i < 64; i++)
            {
                databuf.WriteByte(Relays[i]);
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
