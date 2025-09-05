using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.SystemStatus
{
    /// <summary>
    /// 读取 设备状态信息 返回结果
    /// </summary>
    public class ReadSystemStatus_Result : INCommandResult
    {
        /// <summary>
        /// 继电器物状态 
        /// 0 - COM和NC常闭
        /// 1 - COM和NO常闭
        /// </summary>
        public byte Relay;

        /// <summary>
        /// 运行状态
        /// 0 - 常闭
        /// 1 - 常开
        /// </summary>
        public byte RunState;

        /// <summary>
        /// 门磁开关
        /// 0 - 关
        /// 1 - 开
        /// </summary>
        public byte GateMagnetic;

        /// <summary>
        /// 门报警状态
        /// Bit0 - 非法认证报警
        /// Bit1 - 门磁报警
        /// Bit2 - 胁迫报警
        /// Bit3 - 开门超时报警
        /// Bit4 - 黑名单报警
        /// Bit5 - 防拆报警
        /// Bit6 - 消防报警
        /// </summary>
        public byte AlarmState;

        /// <summary>
        /// 锁定状态
        /// 0 - 未锁定
        /// 1 - 锁定
        /// </summary>
        public byte LockState;

        /// <summary>
        /// 监控状态
        /// 0 - 未开启监控
        /// 1 - 开启监控
        /// </summary>
        public byte WatchState;

        /// <summary>
        /// 返回值内容
        /// </summary>
        public byte[] ResultContent;

        /// <summary>
        /// 对设备状态信息进行解码
        /// </summary>
        /// <param name="databuf">包含设备运行信息结构的缓冲区</param>
        public void SetBytes(IByteBuffer databuf)
        {
            int iReadIndex = databuf.ReaderIndex;
            ResultContent = new byte[databuf.ReadableBytes];
            databuf.ReadBytes(ResultContent);
            databuf.SetReaderIndex(iReadIndex);

            Relay = databuf.ReadByte();
            RunState = databuf.ReadByte();
            GateMagnetic = databuf.ReadByte();
            AlarmState = databuf.ReadByte();
            LockState = databuf.ReadByte();
            WatchState = databuf.ReadByte();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
    }
}
