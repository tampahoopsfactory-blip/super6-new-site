using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Door.ReaderOption
{
    /// <summary>
    /// 读卡器字节数
    /// </summary>
    public class ReaderOption_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门字节数组
        /// 1 - 韦根26(三字节)
        /// 2 - 韦根34(四字节)
        /// 3 - 韦根26(二字节)
        /// 4 - 禁用
        /// 5 - 8字节
        /// </summary>
        public byte ReaderOption;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public ReaderOption_Parameter() { }

        /// <summary>
        /// 控制器4个门的读卡器字节数初始化实例
        /// </summary>
        /// <param name="readerOption">门字节数组</param>
        public ReaderOption_Parameter(byte readerOption)
        {
            ReaderOption = readerOption;
            checkedParameter();
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ReaderOption < 1 || ReaderOption > 5)
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
        }

        /// <summary>
        /// 对控制器4个门的读卡器字节数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(ReaderOption);
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
        /// 对控制器4个门的读卡器字节数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

            ReaderOption = databuf.ReadByte();
        }
    }
}
