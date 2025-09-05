using DotNetty.Buffers;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Util;
using System;
using System.Text.RegularExpressions;

namespace DoNetDrive.Protocol.USB.CardReader.ICCard.Sector
{
    /// <summary>
    /// 写扇区内容
    /// </summary>
    public class WriteSector_Parameter : AbstractParameter
    {
        /// <summary>
        /// 卡片类型
        /// 1 - MF1 IC卡 S50
        /// 2 - NFC标签卡
        /// 3 - NFC手机
        /// 4 - 身份证
        /// 5 - CPU IC卡 S50
        /// 6 - CPU卡
        /// 7 - MF1 IC卡 S70
        /// 8 - CPU IC卡 S70
        /// 9 - ID卡
        /// </summary>
        public int Type;

        /// <summary>
        /// 扇区号
        /// S50卡的取值范围是0-15
        /// S70卡的取值范围是0-39
        /// </summary>
        public int SectorNumber;

        /// <summary>
        /// 起始数据块
        /// S50卡每个扇区的块号都是0-3，其中块3是密码块
        /// S70卡0-31块扇区的块号是0-3，其中块3是密码块
        /// 32-39块扇区的块号是0-15，其中块15是密码块
        /// </summary>
        public int StartBlock;


        /// <summary>
        /// 密钥验证类型
        /// 1--A密钥
        /// 2--B密钥
        /// </summary>
        public int VerifyMode;

        /// <summary>
        /// 扇区密码
        /// </summary>
        public string Password;

        /// <summary>
        /// 待写入数据内容
        /// </summary>
        public byte[] Content;

        /// <summary>
        /// 写入结果
        /// </summary>
        public byte Result;

        /// <summary>
        /// 写入块数
        /// </summary>
        public byte BlockCount;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="type">卡片类型</param>
        /// <param name="number">扇区号</param>
        /// <param name="startBlock">起始数据块</param>
        /// <param name="writeCount">写入块数</param>
        /// <param name="verifyMode">密钥验证类型</param>
        /// <param name="password">扇区密码</param>
        /// <param name="content">待写入数据内容</param>
        public WriteSector_Parameter(int type, int number, int startBlock, int verifyMode, string password,byte[] content)
        {
            Type = type;
            SectorNumber = number;
            StartBlock = startBlock;
            VerifyMode = verifyMode;
            Password = password;
            Content = content;
        }

        public WriteSector_Parameter()
        {

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {

            if (SectorNumber < 0)
            {
                throw new ArgumentException("SectorNumber Error!");
            }

            if(StartBlock < 0 )
            {
                throw new ArgumentException("StartBlock Error!");
            }


            if (Type == 1 || Type == 5)
            {
                if (SectorNumber > 15 )
                {
                    throw new ArgumentException("Number Error!");
                }
                if (StartBlock > 3)
                {
                    throw new ArgumentException("StartBlock Error!");
                }
            }
            else if (Type == 7 || Type == 8)
            {
                if (SectorNumber > 39 )
                {
                    throw new ArgumentException("Number Error!");
                }
                if (SectorNumber <= 31 && StartBlock > 3)
                {
                    throw new ArgumentException("StartBlock Error!");
                }
                if (SectorNumber >=32 &&  StartBlock > 15 )
                {
                    throw new ArgumentException("StartBlock Error!");
                }
            }
            else
            {
                throw new ArgumentException("Type Error!");
            }
            if (string.IsNullOrWhiteSpace( Password))
            {
                
                throw new ArgumentException("Password Error!");
            }
            if(Password.Length != 12) throw new ArgumentException("Password Error!");
            if (!Password.IsHex())
            {
                throw new ArgumentException("Password Error!");
            }

            if (VerifyMode != 1 && VerifyMode != 2)
            {
                throw new ArgumentException("VerifyMode Error!");
            }



            int iDataCount = GetWriteDataLen();

            if (Content == null)
            {
                throw new ArgumentException("Content Error!");
            }
            if (Content.Length % 16 !=0)
            {
                throw new ArgumentException("Content Error!");
            }
          
            return true;
        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            int iDataCount = GetWriteDataLen();
            int iWriteCount = Content.Length;
            if (iWriteCount > iDataCount) iWriteCount = iDataCount;

            databuf.WriteByte(SectorNumber);
            databuf.WriteByte(StartBlock);
            databuf.WriteByte(iWriteCount/16);
            databuf.WriteByte(VerifyMode);

            StringUtil.HextoByteBuf(Password, databuf);
            databuf.WriteBytes(Content, 0, iWriteCount);

            return databuf;
        }

        public int GetWriteDataLen()
        {
            int BlockCount = 4;
            if (SectorNumber >= 32) BlockCount = 16;
            return (BlockCount - StartBlock) * 16;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            int iDataCount = GetWriteDataLen();
            int iWriteCount = Content.Length;
            if (iWriteCount > iDataCount) iWriteCount = iDataCount;

            return 0x0A + iWriteCount;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer buf)
        {
            Result = buf.ReadByte();
            if (Result == 1)
            {
                SectorNumber = buf.ReadByte();
                StartBlock = buf.ReadByte();
                BlockCount = buf.ReadByte();

                //byte[] b = new byte[ReadCount];
                //buf.ReadBytes(b);
                ////Content = Encoding.GetEncoding("GB2312").GetString(b);
                //Content = (System.Text.Encoding.ASCII.GetString(b));
            }
        }
    }
}
