using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Data
{
    /// <summary>
    /// 巡更人员信息
    /// </summary>
    public class PatrolEmpl
    {
        /// <summary>
        /// 工号
        /// </summary>
        public ushort PCode;

        /// <summary>
        /// 卡号
        /// 1 - 16777215
        /// </summary>
        public uint CardData;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name;

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="buf"></param>
        public void SetBytes(IByteBuffer buf)
        {
            PCode = buf.ReadUnsignedShort();
            if (PCode == ushort.MaxValue)
            {
                PCode = 0;
                return;
            }
            CardData = (UInt32)buf.ReadUnsignedMedium();

            byte[] bName = new byte[10];
            buf.ReadBytes(bName);
            Name = Encoding.GetEncoding("GBK").GetString(bName);

            //Name = StringUtil.ByteBufToHex(buf, 10);
        }

        /// <summary>
        /// 写入添加的巡更人员 到字节缓冲
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public IByteBuffer GetBytes(IByteBuffer buf)
        {

            buf.WriteUnsignedShort(PCode);

            buf.WriteMedium((int)CardData);
            //byte[] b = DoNetDrive.Common.NumUtil.Int24ToByte((int)CardData);
            //buf.WriteBytes(b);

            Util.StringUtil.WriteString(buf, Name, 10, Encoding.GetEncoding("GBK"));
            //byte[] bName = new byte[10];
            //bName = Encoding.GetEncoding("GBK").GetBytes(Name.PadRight(10, '0'));//Name
            //buf.WriteBytes(bName);
            return buf;
        }

    }
}
