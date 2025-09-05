using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.KeepAliveInterval
{
    /// <summary>
    /// 设置控制器作为客户端时，和服务器的保活间隔时间_参数
    /// </summary>
    public class WriteKeepAliveInterval_Parameter : AbstractParameter
    {
        /// <summary>
        /// 保活间隔时间（取值0-65535，单位秒。0表示不使用心跳保活间隔参数。）
        /// </summary>
        public ushort IntervalTime;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteKeepAliveInterval_Parameter() { }

        /// <summary>
        /// 使用保活间隔时间参数初始化实例
        /// </summary>
        /// <param name="_IntervalTime">保活间隔时间</param>
        public WriteKeepAliveInterval_Parameter(ushort _IntervalTime)
        {
            IntervalTime = _IntervalTime;
            if (!checkedParameter())
            {
                throw new ArgumentException("IntervalTime Error");
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
        /// 对保活间隔时间参数进行编码
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
        /// 对保活间隔时间参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IntervalTime = databuf.ReadUnsignedShort();
        }
    }
}