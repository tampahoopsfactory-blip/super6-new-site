using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 通知设备播报语音的参数
    /// </summary>
    public class SendCMD_BroadcastVoice_Parameter : AbstractParameter
    {
        /// <summary>
        /// 语音编号
        /// </summary>
        public byte VoiceNumber;



        /// <summary>
        /// 创建通知设备播报语音的参数
        /// </summary>
        /// <param name="num">语音编号</param>
        public SendCMD_BroadcastVoice_Parameter(byte num)
        {
            VoiceNumber = num;
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
            databuf.WriteByte(VoiceNumber);

            return databuf;
        }

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            VoiceNumber = databuf.ReadByte();
        }
    }
}
