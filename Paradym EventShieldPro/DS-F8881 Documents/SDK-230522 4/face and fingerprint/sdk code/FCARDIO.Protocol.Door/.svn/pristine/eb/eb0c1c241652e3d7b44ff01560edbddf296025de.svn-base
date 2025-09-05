using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.ReadCardType
{
    /// <summary>
    /// 写入记录存储方式 参数
    /// </summary>
    public class WriteReadCardType_Parameter : AbstractParameter
    {
        /// <summary>
        /// 读卡类型
        /// <para>bit0 -- MF1 IC卡</para>
        /// <para>bit1 -- NFC标签卡</para>
        /// <para>bit2 -- NFC手机</para>
        /// <para>bit3 -- 身份证</para>
        /// <para>bit4 -- CPU IC卡</para>
        /// <para>bit5 -- CPU卡</para>
        /// <para>bit6 -- ID卡</para>
        public byte[] BitList;


        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteReadCardType_Parameter()
        {

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="mode">读卡类型</param>
        public WriteReadCardType_Parameter(byte[] bitList)
        {
            BitList = bitList;
        }

        /// <summary>
        /// 检查参数 0或1
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (BitList == null || BitList.Length != 7)
                throw new ArgumentException("BitList Error!");
            foreach (var item in BitList)
            {
                if (item != 0 && item != 1)
                    throw new ArgumentException("BitList Error!");
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
            byte[] list = new byte[8];
            for (int i = 0; i < BitList.Length; i++)
            {
                list[i] = BitList[i];
            }
            list[7] = 0;
            byte type = DoNetDrive.Common.NumUtil.BitToByte(list);
            databuf.WriteUnsignedShort(type);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 2;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            ushort type = databuf.ReadUnsignedShort();
            BitList = DoNetDrive.Common.NumUtil.ByteToBit((byte)type);

        }

    }
}
