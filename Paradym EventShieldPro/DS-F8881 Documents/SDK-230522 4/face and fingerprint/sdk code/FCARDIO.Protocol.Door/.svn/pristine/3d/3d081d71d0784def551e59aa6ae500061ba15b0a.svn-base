using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay
{
    /// <summary>
    /// 设置短消息命令参数
    /// </summary>
    public class WriteMessage_Parameter : AbstractParameter
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteMessage_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Message">消息</param>
        public WriteMessage_Parameter(string Message)
        {
            this.Message = Message;
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
            if (string.IsNullOrWhiteSpace(Message))
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
            Util.StringUtil.WriteString(databuf, Message, 0x64, Encoding.GetEncoding("GB2312"));
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x64;
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
            Message = Util.StringUtil.GetString(databuf, 0x64, Encoding.GetEncoding("GB2312"));
        }
    }
}
