using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.Discount
{
    /// <summary>
    /// 设置折扣命令
    /// </summary>
    public class WriteDiscount_Parameter : AbstractParameter
    {
        /// <summary>
        /// IC卡折扣开关
        /// </summary>
        public byte UseICCardDiscount;

        /// <summary>
        /// 卡类折扣开关
        /// </summary>
        public byte UseCardTypeDiscount;

        /// <summary>
        /// 机器折扣开关
        /// </summary>
        public byte UsePOSDiscount;

        /// <summary>
        /// 本机折扣
        /// </summary>
        public byte POSDiscount;

        /// <summary>
        /// 折上折开关
        /// </summary>
        public byte UseDoubleDiscount;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteDiscount_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="UseICCardDiscount">IC卡折扣开关</param>
        /// <param name="UseCardTypeDiscount">卡类折扣开关</param>
        /// <param name="UsePOSDiscount">机器折扣开关</param>
        /// <param name="POSDiscount">机器折扣</param>
        /// <param name="DoubleDiscount">折上折开关</param>
        public WriteDiscount_Parameter(byte UseICCardDiscount, byte UseCardTypeDiscount, byte UsePOSDiscount, byte POSDiscount, byte DoubleDiscount)
        {
            this.UseICCardDiscount = UseICCardDiscount;
            this.UseCardTypeDiscount = UseCardTypeDiscount;
            this.UsePOSDiscount = UsePOSDiscount;
            this.POSDiscount = POSDiscount;
            this.UseDoubleDiscount = DoubleDiscount;
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
            if (UseICCardDiscount != 0 && UseICCardDiscount != 1)
            {
                return false;
            }
            if (UseCardTypeDiscount != 0 && UseCardTypeDiscount != 1)
            {
                return false;
            }
            if (UsePOSDiscount != 0 && UsePOSDiscount != 1)
            {
                return false;
            }
            if (POSDiscount > 100)
            {
                return false;
            }
            if (UseDoubleDiscount > 100)
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
            databuf.WriteByte(UseICCardDiscount);
            databuf.WriteByte(UseCardTypeDiscount);
            databuf.WriteByte(UsePOSDiscount);
            databuf.WriteByte(POSDiscount);
            databuf.WriteByte(UseDoubleDiscount);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 5;
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
            UseICCardDiscount = databuf.ReadByte();
            UseCardTypeDiscount = databuf.ReadByte();
            UsePOSDiscount = databuf.ReadByte();
            POSDiscount = databuf.ReadByte();
            UseDoubleDiscount = databuf.ReadByte();
        }
    }
}
