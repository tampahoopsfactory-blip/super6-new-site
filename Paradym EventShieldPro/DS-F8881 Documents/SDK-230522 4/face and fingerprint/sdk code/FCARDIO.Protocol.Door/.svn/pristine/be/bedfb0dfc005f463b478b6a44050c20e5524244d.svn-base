using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.SystemParameter.Cache
{
    /// <summary>
    /// 设置缓存区命令参数
    /// </summary>
    public class WriteCache_Parameter : AbstractParameter
    {
        /// <summary>
        /// 缓存区内容
        /// </summary>
        public string Content;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteCache_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Content">缓存区内容</param>
        public WriteCache_Parameter(string Content)
        {
            this.Content = Content;
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
            if (string.IsNullOrEmpty(Content))
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
            Util.StringUtil.WriteString(databuf, Content, 0x1E, Encoding.BigEndianUnicode);
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
            Content = Util.StringUtil.GetString(databuf, 0x1E, Encoding.BigEndianUnicode);
        }
    }
}
