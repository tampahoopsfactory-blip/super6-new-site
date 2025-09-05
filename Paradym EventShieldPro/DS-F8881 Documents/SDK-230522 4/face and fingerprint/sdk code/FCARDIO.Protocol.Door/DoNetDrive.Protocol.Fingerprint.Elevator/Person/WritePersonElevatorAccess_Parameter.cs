using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 写入人员电梯扩展权限的参数
    /// </summary>
    public class WritePersonElevatorAccess_Parameter : AbstractParameter
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public uint UserCode;

        /// <summary>
        /// 状态  1--表示成功；0--表示用户号未登记；2--表示不支持此功能
        /// </summary>
        public byte Status;
        /// <summary>
        /// 继电器权限列表，固定64个元素，每个元素代表一个继电器权限
        /// 权限说明：0表示无权限，1表示有权限
        /// </summary>
        public List<byte> RelayAccesss;

        /// <summary>
        /// 创建写入人员电梯扩展权限的参数
        /// </summary>
        public WritePersonElevatorAccess_Parameter()
        {
            RelayAccesss = new List<byte>(64);
        }

        /// <summary>
        /// 检查参数是否合法
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (RelayAccesss == null)
                return false;
            if (RelayAccesss.Count != 64)
                return false;

            foreach (var access in RelayAccesss)
            {
                if (access > 1)
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
            return 68;
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
            if (RelayAccesss == null)
            {
                RelayAccesss = new List<byte>(64);
            }
            UserCode = databuf.ReadUnsignedInt();
            Status = databuf.ReadByte();
            for (int i = 0; i < 64; i++)
            {
                RelayAccesss.Add(databuf.ReadByte());
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
            if (RelayAccesss == null)
            {
                RelayAccesss = new List<byte>(64);
            }
            databuf.WriteInt((int)UserCode);
            for (int i = 0; i < 64; i++)
            {
                databuf.WriteByte(RelayAccesss[i]);
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
