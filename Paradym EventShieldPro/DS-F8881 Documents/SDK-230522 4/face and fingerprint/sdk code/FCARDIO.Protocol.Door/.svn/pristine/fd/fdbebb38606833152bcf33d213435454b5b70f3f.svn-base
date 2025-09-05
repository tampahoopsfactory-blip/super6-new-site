using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 使设备立刻进入点名模式的参数
    /// </summary>
    public class SendCMD_BeginAttendance_Parameter : AbstractParameter
    {
        /// <summary>
        /// 点名时长
        /// </summary>
        public ushort AttendanceTime;



        /// <summary>
        /// 创建使设备立刻进入点名模式的参数
        /// </summary>
        /// <param name="time">点名时长</param>
        public SendCMD_BeginAttendance_Parameter(ushort time)
        {
            AttendanceTime = time;
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
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 2;
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
            databuf.WriteShort((short)AttendanceTime);

            return databuf;
        }

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            AttendanceTime = databuf.ReadUnsignedShort();
        }
    }
}
