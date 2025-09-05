using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 读取人员电梯扩展权限的参数
    /// </summary>
    public class ReadPersonElevatorAccess_Parameter : AbstractParameter
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public uint UserCode;


        /// <summary>
        /// 创建读取人员电梯扩展权限的参数
        /// </summary>
        public ReadPersonElevatorAccess_Parameter(uint UserCode)
        {
            this.UserCode = UserCode;
        }

        /// <summary>
        /// 检查参数是否合法
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            return true;
        }

        /// <summary>
        /// 返回实体二进制序列化后的长度
        /// </summary>
        /// <returns>长度</returns>
        public override int GetDataLen()
        {
            return 4;
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
            UserCode = databuf.ReadUnsignedInt();
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

            databuf.WriteInt((int)UserCode);

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
