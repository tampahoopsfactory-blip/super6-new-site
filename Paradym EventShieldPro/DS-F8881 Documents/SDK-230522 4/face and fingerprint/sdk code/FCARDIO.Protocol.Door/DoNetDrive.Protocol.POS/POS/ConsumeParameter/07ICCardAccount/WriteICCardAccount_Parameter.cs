using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.ICCardAccount
{
    /// <summary>
    /// 设置IC卡账户 命令参数
    /// </summary>
    public class WriteICCardAccount_Parameter : AbstractParameter
    {
        /// <summary>
        /// 现金账户开关
        /// </summary>
        public byte UseCashAccount;

        /// <summary>
        /// 补贴账户开关
        /// </summary>
        public byte UseSubsidyAccount;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteICCardAccount_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="UseCashAccount">现金账户开关</param>
        /// <param name="UseSubsidyAccount">补贴账户开关</param>
        public WriteICCardAccount_Parameter(byte UseCashAccount, byte UseSubsidyAccount)
        {
            this.UseCashAccount = UseCashAccount;
            this.UseSubsidyAccount = UseSubsidyAccount;
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
            if (UseCashAccount != 0 && UseCashAccount != 1)
            {
                return false;
            }
            if (UseSubsidyAccount != 0 && UseSubsidyAccount != 1)
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
            databuf.WriteByte(UseCashAccount);
            databuf.WriteByte(UseSubsidyAccount);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 2;
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
            UseCashAccount = databuf.ReadByte();
            UseSubsidyAccount = databuf.ReadByte();
        }
    }
}
