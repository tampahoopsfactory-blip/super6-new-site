using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置离线卡刷卡开门功能的参数
    /// </summary>
    public class WriteOfflineCardOpenModel_Parameter : AbstractParameter
    {
        /// <summary>
        /// 离线卡刷卡开门功能开关
        /// </summary>
        public bool IsOpen;

        /// <summary>
        /// 离线设备地址
        /// </summary>
        public uint OfflineDeviceAddr;

        /// <summary>
        /// 离线组号
        /// </summary>
        public uint OfflineGroup;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteOfflineCardOpenModel_Parameter() { IsOpen = false; }

        /// <summary>
        /// 创建设置离线卡刷卡开门功能的参数
        /// </summary>
        /// <param name="open">离线卡刷卡开门功能</param>
        /// <param name="addr">离线设备地址</param>
        /// <param name="group">离线组号</param>
        public WriteOfflineCardOpenModel_Parameter(bool open,uint addr,uint group)
        {
            IsOpen = open;
            OfflineDeviceAddr = addr;
            OfflineGroup = group;
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
            return 9;
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
            databuf.WriteBoolean(IsOpen);
            databuf.WriteInt((int)OfflineDeviceAddr);
            databuf.WriteInt((int)OfflineGroup);

            return databuf;
        }

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsOpen = databuf.ReadBoolean();
            OfflineDeviceAddr = databuf.ReadUnsignedInt();
            OfflineGroup = databuf.ReadUnsignedInt();
        }
    }
}
