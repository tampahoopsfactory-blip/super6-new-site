using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using DoNetDrive.Common.Extensions;
using System;
using System.Text.RegularExpressions;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardCustomNum
{
    /// <summary>
    /// IC卡自定义卡号 参数
    /// </summary>
    public class WriteICCardCustomNum_Parameter : AbstractParameter
    {
        /// <summary>
        /// 开关
        /// </summary>
        public bool IsOpen;

        /// <summary>
        /// 扇区号 
        /// 1-15
        /// </summary>
        public int Num;

        /// <summary>
        /// 扇区密码 长度6
        /// </summary>
        public string Password;

        /// <summary>
        /// 密码验证方式
        /// 1 - A密钥
        /// 2 - B密钥
        /// </summary>
        public int VerifyMode;

        /// <summary>
        /// 密码验证方式
        /// 1 - 直接验证
        /// 2 - 使用动态加密（RC4加密）
        /// </summary>
        public int ComputingMode;

        /// <summary>
        /// 卡号长度
        /// 2 - 8
        /// </summary>
        public int CardLength;

        /// <summary>
        /// 卡号数据起始位
        /// 0 - 0-15
        /// 1 - 16-31
        /// 2 - 32-47
        /// </summary>
        public int CardDataStartIndex;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteICCardCustomNum_Parameter()
        {

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="isOpen">开关</param>
        /// <param name="num">扇区号</param>
        /// <param name="password">扇区密码</param>
        /// <param name="verifyMode">密码验证方式</param>
        /// <param name="computingMode">密码计算方式</param>
        /// <param name="cardLength">卡号长度</param>
        /// <param name="cardDataStartIndex">卡号数据起始位</param>
        public WriteICCardCustomNum_Parameter(bool isOpen, int num, string password, int verifyMode, int computingMode, int cardLength
            , int cardDataStartIndex)
        {
            IsOpen = isOpen;
            Num = num;
            Password = password;
            VerifyMode = verifyMode;
            ComputingMode = computingMode;
            CardLength = cardLength;
            CardDataStartIndex = cardDataStartIndex;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Num > 15 || Num < 1)
                throw new ArgumentException("Num Error!");
            if (Password == null || Password.Length > 12)
                throw new ArgumentException("Password Error!");

            if (VerifyMode > 2 || VerifyMode < 1)
                throw new ArgumentException("VerifyMode Error!");
            if (ComputingMode > 2 || ComputingMode < 1)
                throw new ArgumentException("ComputingMode Error!");
            if (CardLength < 2 || CardLength > 8)
                throw new ArgumentException("CardLength Error!");
            if (CardDataStartIndex > 2 || CardDataStartIndex < 0)
                throw new ArgumentException("CardDataStartIndex Error!");

           
            if (!Password.IsHex())
            {
                throw new ArgumentException("Password Error!");
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
            databuf.WriteBoolean(IsOpen);
            databuf.WriteByte(Num);

            Password = StringUtil.FillHexString(Password, 12, "F", true);
            StringUtil.HextoByteBuf(Password, databuf);

            databuf.WriteByte(VerifyMode);
            databuf.WriteByte(ComputingMode);

            databuf.WriteByte(CardLength);
            databuf.WriteByte(CardDataStartIndex);

            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x0C;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsOpen = databuf.ReadBoolean();
            Num = databuf.ReadByte();
            Password = StringUtil.ByteBufToHex(databuf, 6);
            VerifyMode = databuf.ReadByte();
            ComputingMode = databuf.ReadByte();
            CardLength = databuf.ReadByte();
            CardDataStartIndex = databuf.ReadByte();
        }
    }
}
