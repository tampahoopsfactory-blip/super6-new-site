using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.OffLineSubsidyLimit
{
    /// <summary>
    /// 设置离线补贴充值，卡内补贴余额上限命令参数
    /// </summary>
    public class WriteOffLineSubsidyLimit_Parameter : AbstractParameter
    {
        /// <summary>
        /// 金额
        /// </summary>
        public int Quota;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteOffLineSubsidyLimit_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Quota">金额</param>
        public WriteOffLineSubsidyLimit_Parameter(int Quota)
        {
            this.Quota = Quota;
            if (!checkedParameter())
            {
                throw new ArgumentException("Parameter Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Quota < 0)
            {
                return false;
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
        /// 对有效期参数进行编码
        /// </summary>
        /// <param name="databuf">需要填充参数结构的字节缓冲区</param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf len error");
            }
            databuf.WriteMedium(Quota);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 3;
        }

        /// <summary>
        /// 对有效期参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            Quota = databuf.ReadMedium();
        }
    }
}
