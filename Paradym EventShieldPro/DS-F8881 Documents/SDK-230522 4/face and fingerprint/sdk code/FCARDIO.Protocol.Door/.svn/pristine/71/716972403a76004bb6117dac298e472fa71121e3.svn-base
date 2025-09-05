using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;


namespace DoNetDrive.Protocol.Fingerprint.Door
{
    /// <summary>
    /// 设置开门验证方式
    /// </summary>
    public class WriteDoorOpenCheckMode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 验证方式
        /// 1-人脸/密码/刷卡/指纹 （默认值） 
        /// 2-人脸/刷卡/指纹  +  密码（验证人脸后需要输入密码、指纹验证后需要输入密码，没有密码时直接开门）
        /// </summary>
        public byte CheckMode;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteDoorOpenCheckMode_Parameter() { CheckMode = 1; }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="iMode">验证方式</param>
        public WriteDoorOpenCheckMode_Parameter(byte iMode)
        {
            CheckMode = iMode;
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
        /// 对主板蜂鸣器参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(CheckMode);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x01;
        }

        /// <summary>
        /// 对主板蜂鸣器参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            CheckMode = databuf.ReadByte();
        }
    }
}
