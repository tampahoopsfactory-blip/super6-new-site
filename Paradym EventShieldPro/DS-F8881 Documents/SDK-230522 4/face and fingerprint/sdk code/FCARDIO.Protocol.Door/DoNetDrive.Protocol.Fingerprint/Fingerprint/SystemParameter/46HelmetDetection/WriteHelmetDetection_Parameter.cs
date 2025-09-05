using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置安全帽检测功能的参数
    /// </summary>
    public class WriteHelmetDetection_Parameter : AbstractParameter
    {
        /// <summary>
        /// 安全帽检测功能开关
        /// </summary>
        public bool IsOpen;


        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteHelmetDetection_Parameter() { IsOpen = false; }

        /// <summary>
        /// 创建设置安全帽检测功能的参数
        /// </summary>
        /// <param name="open">安全帽检测功能</param>
        public WriteHelmetDetection_Parameter(bool open)
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
