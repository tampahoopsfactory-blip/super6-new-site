using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置认证记录保存现场照片开关的参数
    /// </summary>
    public class WriteSaveRecordImage_Parameter : AbstractParameter
    {
        /// <summary>
        /// 保存现场照片开关
        /// </summary>
        public bool SaveImageSwitch;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteSaveRecordImage_Parameter() { SaveImageSwitch = true; }

        /// <summary>
        /// 创建设置认证记录保存现场照片开关的参数
        /// </summary>
        /// <param name="bSwitch">保存现场照片开关</param>
        public WriteSaveRecordImage_Parameter(bool bSwitch)
        {
            SaveImageSwitch = bSwitch;
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
            databuf.WriteBoolean(SaveImageSwitch);

            return databuf;
        }

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            SaveImageSwitch = databuf.ReadBoolean();
        }
    }
}
