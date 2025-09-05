using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.InterLockSetting
{
    /// <summary>
    /// 写入区域互锁
    /// </summary>
    public class WriteInterLockSetting_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门号
        /// 门端口在控制板中的索引号，取值：1-4
        /// </summary>
        public int DoorNum;

        /// <summary>
        /// 功能开启 (1)
        /// </summary>
        public bool Use;

        /// <summary>
        /// 从属类别 (1)
        /// </summary>
        public bool Type;
        /// <summary>
        /// 区域代码  (16)
        /// </summary>
        public int AreaCode;

        /// <summary>
        /// 从机序号
        /// </summary>
        public byte Num;
        /// <summary>
        /// 主机IP地址 (4)
        /// </summary>
        public byte[] IP;

        /// <summary>
        /// 主机IP端口 (2)
        /// </summary>
        public ushort Port;

        /// <summary>
        /// 提供给 InterLockSetting_Result 使用
        /// </summary>
        public WriteInterLockSetting_Parameter()
        {
            IP = new byte[] { (byte)255, (byte)255, (byte)255, (byte)255 };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="door">门号</param>
        /// <param name="use">功能开启</param>
        /// <param name="type">从属类别</param>
        /// <param name="areacode">区域代码</param>
        /// <param name="num">从机序号</param>
        /// <param name="ip">主机IP地址</param>
        /// <param name="port">主机IP端口</param>
        public WriteInterLockSetting_Parameter(byte door, bool use, bool type, int areacode,byte num, byte[] ip, ushort port)
        {
            DoorNum = door;
            Use = use;
            Type = type;
            AreaCode = areacode;
            Num = num;
            IP = ip;
            Port = port;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Num < 1 || Num > 63)
            {
                throw new ArgumentException("Num Error!");
            }
            if (AreaCode < 0)
            {
                throw new ArgumentException("AreaCode Error!");
            }
            if (DoorNum < 1 || DoorNum > 4)
                throw new ArgumentException("Door Error!");
            string ip = string.Join(".", IP.Select(t => t.ToString()));
            string pattern = @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$";
            bool isHexNum = Regex.IsMatch(ip, pattern);
            if (!isHexNum)
            {
                throw new ArgumentException("IP Error!");
            }
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
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != 25)
            {
                throw new ArgumentException("door Error!");
            }
            databuf.WriteByte(DoorNum);
            databuf.WriteBoolean(Use);
            databuf.WriteBoolean(Type);

            databuf.WriteInt(AreaCode);
            databuf.WriteByte(Num);
            databuf.WriteBytes(IP);
            databuf.WriteShort(Port);
            for (int i = 0; i < 11; i++)
            {
                databuf.WriteByte(0);
            }
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 25;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            DoorNum = databuf.ReadByte();
            Use = databuf.ReadBoolean();
            Type = databuf.ReadBoolean();
            AreaCode = databuf.ReadInt();
            Num = databuf.ReadByte();
            databuf.ReadBytes(IP, 0, 4);
            Port = databuf.ReadUnsignedShort();
        }
    }
}
