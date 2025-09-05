using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.VoiceBroadcastSetting
{
    /// <summary>
    /// 语音播报功能
    /// </summary>
    public class WriteVoiceBroadcastSetting_Parameter : AbstractParameter
    {

        /// <summary>
        /// 是否启用语音播报功能
        /// </summary>
        public bool Use;

        /// <summary>
        /// 默认构造函数 给继承类使用
        /// </summary>
        public WriteVoiceBroadcastSetting_Parameter()
        {

        }
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="use">是否启用语音播报功能</param>
        public WriteVoiceBroadcastSetting_Parameter( bool use)
        {
            Use = use;
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
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteBoolean(Use);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Use = databuf.ReadBoolean();
        }
    }
}
