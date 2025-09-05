using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.RecordMode
{
    /// <summary>
    /// 设置记录存储方式_参数
    /// </summary>
    public class WriteRecordMode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 记录存储方式（0 - 满循环、1 - 满不循环）
        /// </summary>
        public int Mode;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteRecordMode_Parameter() { }

        /// <summary>
        /// 使用记录存储方式初始化实例
        /// </summary>
        /// <param name="_Mode">记录存储方式</param>
        public WriteRecordMode_Parameter(int _Mode)
        {
            Mode = _Mode;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Mode > 1 || Mode < 0)
            {
                return false;
            }

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
        /// 对记录存储方式参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(Mode);
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
        /// 对记录存储方式参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Mode = databuf.ReadByte();
        }
    }
}
