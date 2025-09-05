using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 写入电梯继电器板的继电器输出类型的参数
    /// </summary>
    public class WriteRelayType_Parameter : AbstractParameter
    {
        /// <summary>
        /// 继电器类型列表，固定64个元素，每个元素代表一个继电器输出类型
        /// 输出类型：
        /// 1、COM_NC常闭（默认值）
        /// 2、COM_NO常闭
        /// </summary>
        public List<byte> RelayTypes;


        /// <summary>
        /// 创建写入电梯继电器板的继电器输出类型的参数
        /// </summary>
        public WriteRelayType_Parameter()
        {
            RelayTypes = new List<byte>(64);
        }

        /// <summary>
        /// 检查参数是否合法
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (RelayTypes == null)
                return false;
            if (RelayTypes.Count != 64)
                return false;

            foreach (var relay in RelayTypes)
            {
                if (relay < 1 || relay > 2)
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
            if (RelayTypes == null)
            {
                RelayTypes = new List<byte>(64);
            }

            for (int i = 0; i < 64; i++)
            {
                RelayTypes.Add(databuf.ReadByte());
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
            if (RelayTypes == null)
            {
                RelayTypes = new List<byte>(64);
            }
            for (int i = 0; i < 64; i++)
            {
                databuf.WriteByte(RelayTypes[i]);
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
