using System;
using System.Text;
using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入合法验证后显示的短消息参数
    /// </summary>
    public class WriteShortMessage_Parameter : AbstractParameter
    {
        public static Encoding StringEncoding = Encoding.BigEndianUnicode;

        /// <summary>
        /// 合法验证后显示的短消息 30个字符
        /// </summary>
        public string Message;

        /// <summary>
        /// 构建一个合法验证后显示的短消息参数的实例
        /// </summary>
        public WriteShortMessage_Parameter() { Message = string.Empty; }

        /// <summary>
        /// 创建合法验证后显示的短消息的命令参数
        /// </summary>
        /// <param name="sMsg">合法验证后显示的短消息  30个字符</param>
        public WriteShortMessage_Parameter(string sMsg)
        {
            Message = sMsg;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                if (Message.Length > 30) Message = Message.Substring(0, 30);
            }
            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 60;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 编码参数
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {


            Util.StringUtil.WriteString(databuf, Message, 60, StringEncoding);

            return databuf;
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != 60)
            {
                throw new ArgumentException("databuf Error");
            }
            int isNull = databuf.ReadShort();
            if (isNull == 0)
            {
                Message = string.Empty;
            }
            else
            {
                databuf.SetReaderIndex(0);
                Message = Util.StringUtil.GetString(databuf,60, StringEncoding); 
            }
            
        }
    }
}
