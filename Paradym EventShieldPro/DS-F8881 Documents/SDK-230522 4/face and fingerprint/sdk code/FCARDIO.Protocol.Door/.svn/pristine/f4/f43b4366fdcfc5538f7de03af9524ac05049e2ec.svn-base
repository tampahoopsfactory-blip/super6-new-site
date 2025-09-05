using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Door.RelayOption
{
    /// <summary>
    /// 继电器参数
    /// </summary>
    public class RelayOption_Parameter : AbstractParameter
    {

        /// <summary>
        /// 继电器否支持双稳态
        /// </summary>
        public bool IsSupport ;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public RelayOption_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="isRelay">继电器是否支持双稳态</param>
        public RelayOption_Parameter(bool isSupport)
        {
            IsSupport = isSupport;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// 对继电器参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteBoolean(IsSupport);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }

        /// <summary>
        /// 对继电器参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsSupport = databuf.ReadBoolean();
        }
    }
}
