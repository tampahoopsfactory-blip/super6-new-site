using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入自动推送服务开关参数
    /// </summary>
    public class Write_PUSH_AUTO_Service_Parameter : AbstractParameter
    {
        /// <summary>
        /// 启动标志
        /// 1--启用；0--禁用
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 创建写入自动推送服务开关参数
        /// </summary>
        /// <param name="bUse">开关</param>
        public Write_PUSH_AUTO_Service_Parameter(bool bUse)
        {
            IsUse = bUse;
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
            databuf.WriteBoolean(IsUse);

            return databuf;
        }

        /// <summary>
        /// 废弃
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }
    }
}
