using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.CloseAlarm
{
    /// <summary>
    /// 解除报警 参数
    /// </summary>
    public class WriteCloseAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 报警类型
        /// <para>bit0 -- 非法卡报警</para>
        /// <para>bit1 -- 胁迫报警</para>
        /// <para>bit2 -- 黑名单报警</para>
        /// <para>bit3 -- 消防报警</para>
        public byte[] BitList;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="bitList">报警类型</param>
        public WriteCloseAlarm_Parameter(byte[] bitList)
        {
            BitList = bitList;
        }

        /// <summary>
        /// 检查参数 0或1
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (BitList == null || BitList.Length != 4)
                throw new ArgumentException("BitList Error!");
            foreach (var item in BitList)
            {
                if (item != 0 && item != 1)
                    throw new ArgumentException("BitList Error!");
            }
            return true;
        }

        public override void Dispose()
        {
        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            byte[] list = new byte[8];
            list[0] = BitList[0];
            list[1] = 0;
            list[2] = BitList[1];
            list[3] = 0;
            list[4] = BitList[2];
            list[5] = 0;
            list[6] = 0;
            list[7] = BitList[3];
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
           

        }

    }
}
