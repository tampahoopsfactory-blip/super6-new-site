using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay
{
    /// <summary>
    /// 设置开机供应商Logo命令参数
    /// </summary>
    public class WriteLogo_Parameter : AbstractParameter
    {
        /// <summary>
        /// 开机供应商Logo
        /// </summary>
        public string Logo;

        /// <summary>
        /// 开机供应商Logo
        /// </summary>
        public string Phone;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteLogo_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Logo">开机供应商Logo</param>
        /// <param name="Phone">开机供应商电话</param>
        public WriteLogo_Parameter(string Logo, string Phone)
        {
            this.Logo = Logo;
            this.Phone = Phone;
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
            if (string.IsNullOrWhiteSpace(Logo))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Phone))
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
            Util.StringUtil.WriteString(databuf, Logo, 0x1e, Encoding.GetEncoding("GB2312"));
            Util.StringUtil.WriteString(databuf, Phone, 0x1e, Encoding.GetEncoding("GB2312"));
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x3C;
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
            Logo = Util.StringUtil.GetString(databuf, 0x1e, Encoding.GetEncoding("GB2312"));
            Phone = Util.StringUtil.GetString(databuf, 0x1e, Encoding.GetEncoding("GB2312"));
        }
    }
}
