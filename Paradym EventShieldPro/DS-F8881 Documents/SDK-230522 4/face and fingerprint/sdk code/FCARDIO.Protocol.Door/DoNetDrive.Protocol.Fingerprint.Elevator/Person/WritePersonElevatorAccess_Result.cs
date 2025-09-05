using System;
using System.Collections.Generic;
using System.Text;
using DoNetDrive.Core.Command;
using DotNetty.Buffers;


namespace DoNetDrive.Protocol.Fingerprint
{
    /// <summary>
    /// 写入人员电梯扩展权限的返回值
    /// </summary>
    public class WritePersonElevatorAccess_Result :  INCommandResult
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
        /// 创建写入人员电梯扩展权限的返回值
        /// </summary>
        public WritePersonElevatorAccess_Result()
        {}

       
        /// <summary>
        /// 返回实体二进制序列化后的长度
        /// </summary>
        /// <returns>长度</returns>
        public int GetDataLen()
        {
            return 5;
        }

        /// <summary>
        /// 对定时常开参数命令的返回值进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes < GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }

            UserCode = databuf.ReadUnsignedInt();
            Status = databuf.ReadByte();

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
