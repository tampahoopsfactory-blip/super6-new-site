using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.AreaAntiPassback
{
    /// <summary>
    /// 设置区域防潜回
    /// </summary>
    public class WriteAreaAntiPassback_Parameter : AbstractParameter
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
        /// 主机SN  (16)
        /// </summary>
        public string SN;

        /// <summary>
        /// 主机IP地址 (4)
        /// </summary>
        public byte[] IP;

        /// <summary>
        /// 主机端口 (2)
        /// </summary>
        public ushort Port;

        /// <summary>
        /// 提供给 AreaAntiPassback_Result 使用
        /// </summary>
        public WriteAreaAntiPassback_Parameter()
        {
            IP = new byte[] { (byte)255, (byte)255, (byte)255, (byte)255 };
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="door">门号</param>
        /// <param name="use">功能开启</param>
        /// <param name="type">从属类别</param>
        /// <param name="sn">主机SN</param>
        /// <param name="ip">主机IP地址</param>
        /// <param name="port">主机端口</param>
        public WriteAreaAntiPassback_Parameter(byte door, bool use, bool type, string sn, byte[] ip, ushort port)
        {
            DoorNum = door;
            Use = use;
            Type = type;
            SN = sn;
            IP = ip;
            Port = port;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DoorNum < 1 || DoorNum > 4)
                throw new ArgumentException("Door Error!");
            if (IP == null || IP.Length != 4)
            {
                throw new ArgumentException("IP Is Null!");
            }
            if (SN != null && SN.Length > 16)
            {
                throw new ArgumentException("SN Length more then 16!");
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
            SN = SN ?? "";
            SN = SN.PadLeft(16, '0');
            byte[] array = new byte[16];
            array = System.Text.Encoding.ASCII.GetBytes(SN);
            databuf.WriteBytes(array);

            databuf.WriteBytes(IP);
            databuf.WriteShort(Port);
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
            byte[] b = new byte[16];
            databuf.ReadBytes(b);
            SN = Convert.ToString(System.Text.Encoding.ASCII.GetString(b));
            //SN = StringUtil.ByteBufToHex(databuf, 16);
            databuf.ReadBytes(IP, 0, 4);
            Port = databuf.ReadUnsignedShort();
        }
    }
}
