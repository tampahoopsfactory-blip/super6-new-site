using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Time.TimeErrorCorrection
{
    /// <summary>
    /// 设置误差自修正_参数
    /// </summary>
    public class WriteTimeError_Parameter : AbstractParameter
    {
        /// <summary>
        /// 误差自修正（第一字节表示调快还是调慢，0表示调慢，1表示调快；第二字节表示具体秒数。）
        /// </summary>
        public byte[] TimeErrorCorrection;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteTimeError_Parameter()
        {
        }

        /// <summary>
        /// 误差自修正参数初始化实例
        /// </summary>
        /// <param name="_TimeErrorCorrection">误差自修正参数</param>
        public WriteTimeError_Parameter(byte[] _TimeErrorCorrection)
        {
            TimeErrorCorrection = _TimeErrorCorrection;
            if (!checkedParameter())
            {
                throw new ArgumentException("TimeErrorCorrection Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (TimeErrorCorrection == null)
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
        /// 对误差自修正参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteBytes(TimeErrorCorrection);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x02;
        }

        /// <summary>
        /// 对误差自修正参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (TimeErrorCorrection == null)
            {
                TimeErrorCorrection = new byte[2];
            }
            databuf.ReadBytes(TimeErrorCorrection);
        }
    }
}