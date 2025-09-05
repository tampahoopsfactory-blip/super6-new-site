using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay
{
    /// <summary>
    /// 设置标题命令参数
    /// </summary>
    public class WriteTitle_Parameter : AbstractParameter
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteTitle_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Title">标题</param>
        public WriteTitle_Parameter(string Title)
        {
            this.Title = Title;
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
            if (string.IsNullOrWhiteSpace(Title))
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
            Util.StringUtil.WriteString(databuf, Title, 0x1E, Encoding.GetEncoding("GB2312"));
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x1E;
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
            Title = Util.StringUtil.GetString(databuf, 0x1E, Encoding.GetEncoding("GB2312"));
        }
    }
}
