using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Alarm
{
    /// <summary>
    /// 解除报警 参数
    /// </summary>
    public class CloseAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 报警类型
        /// bit0 -- 非法卡报警
        /// bit1 -- 门磁报警
        /// bit2 -- 胁迫报警
        /// bit3 -- 开门超时报警
        /// bit4 -- 黑名单报警
        /// bit5 -- 防拆报警
        /// bit6 -- 消防报警
        public byte[] BitList;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="bitList">读卡类型</param>
        public CloseAlarm_Parameter(byte[] bitList)
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
            for (int i = 0; i < BitList.Length; i++)
            {
                list[i] = BitList[i];
            }
            list[7] = 0;
            byte type = Common.NumUtil.BitToByte(list);
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
