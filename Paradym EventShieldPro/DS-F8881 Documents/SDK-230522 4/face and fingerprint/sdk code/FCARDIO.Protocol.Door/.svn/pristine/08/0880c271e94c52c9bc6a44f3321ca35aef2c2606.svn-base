using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.Voice
{
    public class WriteVoiceStart_Parameter : AbstractParameter
    {
        /// <summary>
        /// 开机
        /// </summary>
        public byte Start;

        /// <summary>
        /// 广告
        /// </summary>
        public byte Advertisement;


        public WriteVoiceStart_Parameter()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="Advertisement"></param>
        public WriteVoiceStart_Parameter(byte Start, byte Advertisement)
        {
            this.Start = Start;
            this.Advertisement = Advertisement;
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
            if (Start != 0 && Start != 1)
                throw new ArgumentException("Start Error!");
            if (Advertisement != 0 && Advertisement != 1)
                throw new ArgumentException("Advertisement Error!");
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
            databuf.WriteByte(Start);
            databuf.WriteByte(Advertisement);
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
            Start = databuf.ReadByte();
            Advertisement = databuf.ReadByte();
        }
    }
}
