using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.OpenDoorTimeoutAlarm
{
    /// <summary>
    /// 设置开门超时报警参数
    /// </summary>
    public class WriteOpenDoorTimeoutAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 允许开门的时间 1-65535秒，0--表示不启用
        /// </summary>
        public ushort AllowTime;

        /// <summary>
        /// 继电器输出
        /// false - 不输出继电器
        /// true - 输出继电器(匪警继电器)
        /// </summary>
        public bool RelayOutput;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteOpenDoorTimeoutAlarm_Parameter()
        {

        }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="isUse">是否开启</param>
        /// <param name="allowTime">允许开门的时间</param>
        /// <param name="relayOutput">继电器输出</param>
        public WriteOpenDoorTimeoutAlarm_Parameter(bool isUse, ushort allowTime, bool relayOutput)
        {
            IsUse = isUse;
            AllowTime = allowTime;
            RelayOutput = relayOutput;
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
            return;
        }

        /// <summary>
        /// 对误差自修正参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteBoolean(IsUse);
            databuf.WriteUnsignedShort(AllowTime);
            databuf.WriteBoolean(RelayOutput);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {

            return 0xC0;
        }

        /// <summary>
        /// 对误差自修正参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsUse = databuf.ReadBoolean();
            AllowTime = databuf.ReadUnsignedShort();
            RelayOutput = databuf.ReadBoolean();
        }
    }
}
