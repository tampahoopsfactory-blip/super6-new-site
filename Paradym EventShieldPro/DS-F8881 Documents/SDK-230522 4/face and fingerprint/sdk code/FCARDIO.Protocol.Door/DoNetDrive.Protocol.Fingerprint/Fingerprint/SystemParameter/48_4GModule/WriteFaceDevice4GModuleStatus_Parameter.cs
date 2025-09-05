using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置人脸机设备4G模块的工作状态的参数
    /// </summary>
    public class WriteFaceDevice4GModuleStatus_Parameter : AbstractParameter
    {
        /// <summary>
        /// 人脸机设备4G模块的工作状态
        /// </summary>
        public bool IsOpen;


        /// <summary>
        /// 构建一个空设置人脸机设备4G模块的工作状态的参数的实例
        /// </summary>
        public WriteFaceDevice4GModuleStatus_Parameter() { IsOpen = false; }

        /// <summary>
        /// 创建设置人脸机设备4G模块的工作状态的参数
        /// </summary>
        /// <param name="open">云筑网推送功能</param>
        public WriteFaceDevice4GModuleStatus_Parameter(bool open)
        {
            IsOpen = open;
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
            return 1;
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

            return databuf;
        }

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsOpen = databuf.ReadBoolean();
        }
    }
}
