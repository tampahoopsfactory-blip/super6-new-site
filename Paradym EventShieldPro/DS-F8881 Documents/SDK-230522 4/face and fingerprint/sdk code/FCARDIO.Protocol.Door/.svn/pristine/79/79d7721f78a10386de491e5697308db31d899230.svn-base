using DoNetDrive.Common.Extensions;
using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.SN
{
    /// <summary>
    /// 写入控制器SN_参数
    /// </summary>
    public class SN_Parameter : AbstractParameter

    {
        /// <summary>
        /// SN的字节数组
        /// </summary>
        public byte[] SNBuf;

        /// <summary>
        /// 命令是否用UDP广播发送？
        /// </summary>
        public bool UDPBroadcast;


        /// <summary>
        /// 使用默认构造函数初始化类
        /// </summary>
        public SN_Parameter() { }

        /// <summary>
        /// 使用一个包含SN的数组初始化类
        /// </summary>
        /// <param name="_SN">SN的字节数组</param>
        public SN_Parameter(byte[] _SN)
        {
            SNBuf = _SN;
            if (!checkedParameter())
            {
                throw new ArgumentException("SN Error");
            }
        }
        /// <summary>
        /// 使用SN字符串初始化类
        /// </summary>
        /// <param name="_SN">SN字符串</param>
        public SN_Parameter(string _SN)
            : this(_SN.GetBytes())
        {
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns>true 表示参数符合规则，false 表示参数不符合规则</returns>
        public override bool checkedParameter()
        {
            if (SNBuf == null)
            {
                return false;
            }
            if (SNBuf.Length != 16)
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
            SNBuf = null;
            return;
        }

        /// <summary>
        /// 对SN参数进行编码
        /// </summary>
        /// <param name="databuf">需要填充参数结构的字节缓冲区</param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < GetDataLen())
            {
                throw new ArgumentException("databuf len error");
            }
            databuf.WriteBytes(SNBuf);
            return databuf;
        }

        /// <summary>
        /// 获取参数结构长度
        /// </summary>
        /// <returns>结构长度</returns>
        public override int GetDataLen()
        {
            return 16;
        }

        /// <summary>
        /// 对SN参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (SNBuf == null)
            {
                SNBuf = new byte[16];
            }
            if (databuf.ReadableBytes != 16)
            {
                throw new ArgumentException("databuf Error");
            }
            databuf.ReadBytes(SNBuf);
        }
    }
}
