using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.ReaderIntervalTime
{
    /// <summary>
    /// 设置重复验证权限间隔_参数
    /// </summary>
    public class WriteReaderIntervalTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 读卡间隔时间,单位秒，最大65535秒，0表示无限制
        /// </summary>
        public ushort IntervalTime;

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 检测模式
        /// 1 - 记录读卡，不开门，有提示
        /// 2 - 不记录读卡，不开门，有提示
        /// 3 - 不做响应，无提示
        /// </summary>
        public byte Mode;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteReaderIntervalTime_Parameter() { }

        /// <summary>
        /// 使用读卡间隔时间参数初始化实例
        /// </summary>
        /// <param name="isUse">是否有效</param>
        /// <param name="_IntervalTime">读卡间隔时间,单位秒，最大65535秒，0表示无限制</param>
        /// <param name="mode">检测模式:1 - 记录读卡，不开门，有提示;2 - 不记录读卡，不开门，有提示;3 - 不做响应，无提示</param>
        public WriteReaderIntervalTime_Parameter(bool isUse,ushort _IntervalTime, byte mode)
        {
            IsUse = isUse;
            Mode = mode;
            IntervalTime = _IntervalTime;
           
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Mode < 1 || Mode > 3)
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
        /// 对参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteBoolean(IsUse);
            databuf.WriteByte(Mode);
            databuf.WriteUnsignedShort(IntervalTime);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x04;
        }

        /// <summary>
        /// 对参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsUse = databuf.ReadBoolean();
            Mode = databuf.ReadByte();
            IntervalTime = databuf.ReadUnsignedShort();
        }
    }
}