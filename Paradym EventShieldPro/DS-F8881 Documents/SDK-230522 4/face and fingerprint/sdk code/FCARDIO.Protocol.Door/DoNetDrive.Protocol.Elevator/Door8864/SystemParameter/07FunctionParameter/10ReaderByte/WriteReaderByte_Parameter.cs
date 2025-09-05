using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter.ReaderByte
{
    /// <summary>
    /// 设置 读卡器字节数 参数
    /// </summary>
    public class WriteReaderByte_Parameter : AbstractParameter
    {
        /// <summary>
        /// 读卡字节数
        /// 1 - 韦根26(三字节)
        /// 2 - 韦根34(四字节)
        /// 3 - 韦根26(二字节)
        /// 4 - 禁用
        /// </summary>
        public byte Type;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteReaderByte_Parameter() { }

        /// <summary>
        /// 使用通讯密码初始化实例
        /// </summary>
        /// <param name="type">读卡字节数</param>
        public WriteReaderByte_Parameter(byte type)
        {
            Type = type;

        }
     

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Type > 5 || Type < 1)
            {
                return false;
            }
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

            return databuf.WriteByte(Type);
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

            Type = databuf.ReadByte() ;
        }
    }
}
