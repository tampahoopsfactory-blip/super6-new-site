using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using System;
using System.Text.RegularExpressions;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.ICCardControl
{
    /// <summary>
    /// 设置扇区验证 参数
    /// </summary>
    public class WriteICCardControl_Parameter : AbstractParameter
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
        /// 验证扇区内容开关
        /// </summary>
        public bool IsOpenVerifySector;

        /// <summary>
        /// 扇区内容长度
        /// 0 - 16
        /// </summary>
        public int SectorContentLength;

        /// <summary>
        /// 扇区内的需要验证的数据块起始位
        /// 0 - 0-15
        /// 1 - 16-31
        /// 2 - 32-47
        /// </summary>
        public int VerifyDataStartIndex;

        /// <summary>
        /// 验证内容 长度16
        /// </summary>
        public string VerifyContent;
        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteICCardControl_Parameter()
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
        /// <param name="isOpenVerifySector">验证扇区内容开关</param>
        /// <param name="sectorContentLength">扇区内容长度</param>
        /// <param name="verifyDataStartIndex">验证数据起始位</param>
        /// <param name="verifyContent">验证内容</param>
        public WriteICCardControl_Parameter (bool isOpen, int num, string password, int verifyMode, int computingMode, bool isOpenVerifySector, int sectorContentLength
            , int verifyDataStartIndex, string verifyContent)
        {
            IsOpen = isOpen;
            Num = num;
            Password = password;
            VerifyMode = verifyMode;
            ComputingMode = computingMode;
            IsOpenVerifySector = isOpenVerifySector;
            SectorContentLength = sectorContentLength;
            VerifyDataStartIndex = verifyDataStartIndex;
            VerifyContent = verifyContent;
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
            if (SectorContentLength > 16 || SectorContentLength < 0)
                throw new ArgumentException("SectorContentLength Error!");
            if (VerifyDataStartIndex > 2 || VerifyDataStartIndex < 0)
                throw new ArgumentException("VerifyDataStartIndex Error!");
            if (VerifyContent == null || VerifyContent.Length > 32)
                throw new ArgumentException("VerifyContent Error!");

            string pattern = @"^([0-9a-fA-F]+)$";
            bool isHexNum = Regex.IsMatch(Password, pattern);
            if (!isHexNum)
            {
                throw new ArgumentException("Password Error!");
            }

            isHexNum = Regex.IsMatch(VerifyContent, pattern);
            if (!isHexNum)
            {
                throw new ArgumentException("VerifyContent Error!");
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

            databuf.WriteBoolean(IsOpenVerifySector);
            databuf.WriteByte(SectorContentLength);
            databuf.WriteByte(VerifyDataStartIndex);

            //byte[] b = new byte[16];
            //b = System.Text.Encoding.ASCII.GetBytes(VerifyContent);
            //databuf.WriteBytes(b);

            VerifyContent = StringUtil.FillHexString(VerifyContent, 32, "0", true);
            StringUtil.HextoByteBuf(VerifyContent, databuf);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x1D;
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
            IsOpenVerifySector = databuf.ReadBoolean();
            SectorContentLength = databuf.ReadByte();
            VerifyDataStartIndex = databuf.ReadByte();
            VerifyContent = StringUtil.ByteBufToHex(databuf, 16);
        }
    }
}
