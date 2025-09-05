using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.SN
{
    /// <summary>
    /// 写入巡更棒SN_参数
    /// </summary>
    public class SN_Parameter : AbstractParameter
    {
        /// <summary>
        /// 机器号
        /// </summary>
        public int SN;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="sn"></param>
        public SN_Parameter(int sn)
        {
            SN = sn;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (SN <= 0)
                throw new ArgumentException("SN Error!");
            return true;
        }

        

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(SN);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            SN = databuf.ReadByte();
        }
    }
}
