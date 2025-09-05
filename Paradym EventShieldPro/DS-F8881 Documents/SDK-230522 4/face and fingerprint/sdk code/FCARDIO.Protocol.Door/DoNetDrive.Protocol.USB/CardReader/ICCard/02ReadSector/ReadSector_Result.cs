using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.CardReader.ICCard.Sector
{
    /// <summary>
    /// 读扇区内容 返回结果
    /// </summary>
    public class ReadSector_Result : INCommandResult
    {
        /// <summary>
        /// 寻卡是否成功
        /// </summary>
        public byte IsSuccess;

        /// <summary>
        /// 扇区号
        /// S50卡的取值范围是0-15
        /// S70卡的取值范围是0-39
        /// </summary>
        public int Number;

        /// <summary>
        /// 起始数据块
        /// S50卡每个扇区的块号都是0-3，其中块3是密码块
        /// S70卡0-31块扇区的块号是0-3，其中块3是密码块
        /// 32-39块扇区的块号是0-15，其中块15是密码块
        /// </summary>
        public int StartBlock;

        /// <summary>
        /// 读取字节数
        /// </summary>
        public int ReadCount;


        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] ByteContent;

        public void Dispose()
        {

        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="buf"></param>
        internal void SetBytes(IByteBuffer buf)
        {
            IsSuccess = buf.ReadByte();
            if (IsSuccess == 1)
            {
                Number = buf.ReadByte();
                StartBlock = buf.ReadByte();
                ReadCount = buf.ReadByte();

                ByteContent = new byte[ReadCount];
                buf.ReadBytes(ByteContent,0, ReadCount);
                //Content = StringUtil.ByteBufToHex(buf, ReadCount );

                //byte[] b = new byte[ReadCount];
                //buf.ReadBytes(b);
                ////Content = Encoding.GetEncoding("GB2312").GetString(b);
                //Content = (System.Text.Encoding.ASCII.GetString(b));
            }
        }
    }
}
