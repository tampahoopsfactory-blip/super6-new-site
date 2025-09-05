using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置读卡间隔时间_参数
    /// </summary>
    public class WriteReaderIntervalTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 读卡间隔时间，最大65535秒，0表示无限制
        /// </summary>
        public ushort IntervalTime;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteReaderIntervalTime_Parameter() { }

        /// <summary>
        /// 使用读卡间隔时间参数初始化实例
        /// </summary>
        /// <param name="_IntervalTime">读卡间隔时间</param>
        public WriteReaderIntervalTime_Parameter(ushort _IntervalTime)
        {
            IntervalTime = _IntervalTime;
            if (!checkedParameter())
            {
                throw new ArgumentException("Option Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (IntervalTime < 0 || IntervalTime > 65535)
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
        /// 对读卡间隔时间参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteUnsignedShort(IntervalTime);
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
        /// 对读卡间隔时间参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IntervalTime = databuf.ReadUnsignedShort();
        }
    }
}