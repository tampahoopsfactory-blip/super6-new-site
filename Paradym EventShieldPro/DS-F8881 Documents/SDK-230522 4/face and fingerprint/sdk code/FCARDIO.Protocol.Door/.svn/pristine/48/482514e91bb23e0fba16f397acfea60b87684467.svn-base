using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Door.ReadCardAndTakePictures
{
    /// <summary>
    /// 写入 读卡拍照联动消息
    /// </summary>
    public class WriteReadCardAndTakePictures_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门号
        /// 门端口在控制板中的索引号，取值：1-4
        /// </summary>
        public int DoorNum;

        /// <summary>
        /// 进门读卡联动开关 (1)
        /// </summary>
        public bool InDoorUse;

        /// <summary>
        /// 出门读卡联动开关 (1)
        /// </summary>
        public bool OutDoorUse;

        /// <summary>
        /// 进门联动IP
        /// </summary>
        public byte[] InDoorIP;

        /// <summary>
        /// 进门联动端口
        /// </summary>
        public ushort InDoorPort;

        /// <summary>
        /// 进门联动IP
        /// </summary>
        public string InDoorProtocol;

        /// <summary>
        /// 出门联动IP
        /// </summary>
        public byte[] OutDoorIP;

        /// <summary>
        /// 出门联动端口
        /// </summary>
        public ushort OutDoorPort;

        /// <summary>
        /// 出门联动IP
        /// </summary>
        public string OutDoorProtocol;

        /// <summary>
        /// 提供给继承类使用
        /// </summary>
        public WriteReadCardAndTakePictures_Parameter()
        {
            InDoorIP = new byte[] { (byte)0, (byte)0, (byte)0, (byte)0 };
            OutDoorIP = new byte[] { (byte)0, (byte)0, (byte)0, (byte)0 };
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="doorNum"></param>
        /// <param name="inDoorUse"></param>
        /// <param name="inDoorIP"></param>
        /// <param name="inDoorPort"></param>
        /// <param name="inDoorProtocol"></param>
        /// <param name="outDoorUse"></param>
        /// <param name="outDoorIP"></param>
        /// <param name="outDoorPort"></param>
        /// <param name="outDoorProtocol"></param>
        public WriteReadCardAndTakePictures_Parameter(byte doorNum, bool inDoorUse, byte[] inDoorIP,ushort inDoorPort,string inDoorProtocol,
            bool outDoorUse, byte[] outDoorIP, ushort outDoorPort, string outDoorProtocol)
        {
            DoorNum = doorNum;
            InDoorUse = inDoorUse;
            InDoorIP = inDoorIP;
            InDoorPort = inDoorPort;
            InDoorProtocol = inDoorProtocol;

            OutDoorUse = outDoorUse;
            OutDoorIP = outDoorIP;
            OutDoorPort = outDoorPort;
            OutDoorProtocol = outDoorProtocol;
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DoorNum < 1 || DoorNum > 4)
                throw new ArgumentException("Door Error!");
            if (InDoorIP == null)
            {
                throw new ArgumentException("InDoorIP is Null!");

            }
            if (OutDoorIP == null)
            {
                throw new ArgumentException("OutDoorIP is Null!");

            }
            string strInDoorIP = string.Join(".", InDoorIP.Select(t => t.ToString()));
            string pattern = @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$";
            bool isHexNum = Regex.IsMatch(strInDoorIP, pattern);
            if (!isHexNum)
            {
                throw new ArgumentException("InDoorIP Error!");
            }
            string strOutDoorIP = string.Join(".", OutDoorIP.Select(t => t.ToString()));
            isHexNum = Regex.IsMatch(strOutDoorIP, pattern);
            if (!isHexNum)
            {
                throw new ArgumentException("OutDoorIP Error!");
            }

            if (InDoorProtocol != null && InDoorProtocol.Length > 128)
            {
                throw new ArgumentException("InDoorProtocol  Length more then 128!");
            }


            if (OutDoorProtocol != null && OutDoorProtocol.Length > 128)
            {
                throw new ArgumentException("OutDoorProtocol Length more then 128!");
            }

            pattern = @"^([0-9a-fA-F]+)$";
            isHexNum = Regex.IsMatch(InDoorProtocol, pattern);
            if (!isHexNum)
            {
                throw new ArgumentException("InDoorProtocol Error!");
            }

            isHexNum = Regex.IsMatch(OutDoorProtocol, pattern);
            if (!isHexNum)
            {
                throw new ArgumentException("OutDoorProtocol Error!");
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
            if (databuf.WritableBytes != 271)
            {
                throw new ArgumentException("door Error!");
            }
            databuf.WriteByte(DoorNum);
            databuf.WriteBoolean(InDoorUse);
            databuf.WriteBoolean(OutDoorUse);

            databuf.WriteBytes(InDoorIP);
            databuf.WriteUnsignedShort(InDoorPort);

            databuf.WriteBytes(OutDoorIP);
            databuf.WriteUnsignedShort(OutDoorPort);

            InDoorProtocol = InDoorProtocol ?? "";
            OutDoorProtocol = OutDoorProtocol ?? "";

            int length = InDoorProtocol.Length % 2;

            databuf.WriteByte(length == 0 ? InDoorProtocol.Length / 2 : InDoorProtocol.Length / 2 + 1);
            InDoorProtocol = StringUtil.FillHexString(InDoorProtocol, 254, "0", true);
            StringUtil.HextoByteBuf(InDoorProtocol, databuf);

            length = OutDoorProtocol.Length % 2;
            databuf.WriteByte(length == 0 ? OutDoorProtocol.Length / 2 : OutDoorProtocol.Length / 2 + 1);
            OutDoorProtocol = StringUtil.FillHexString(OutDoorProtocol, 254, "0", true);
            StringUtil.HextoByteBuf(OutDoorProtocol, databuf);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 271;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            DoorNum = databuf.ReadByte();
            InDoorUse = databuf.ReadBoolean();
            OutDoorUse = databuf.ReadBoolean();

            databuf.ReadBytes(InDoorIP,0,4);
            InDoorPort = databuf.ReadUnsignedShort();

            databuf.ReadBytes(OutDoorIP, 0, 4);
            OutDoorPort = databuf.ReadUnsignedShort();

            byte blength = databuf.ReadByte();
            if (blength > 0)
            {
                InDoorProtocol = StringUtil.ByteBufToHex(databuf, 127).Substring(0,blength * 2).ToUpper();
               
            }
            else
            {
                InDoorProtocol = string.Empty;
            }
            

            blength = databuf.ReadByte();
            if (blength > 0)
            {
                OutDoorProtocol = StringUtil.ByteBufToHex(databuf, 127).Substring(0, blength * 2).ToUpper();
            }
            else
            {
                OutDoorProtocol = string.Empty;
            }
                
        }
    }
}
