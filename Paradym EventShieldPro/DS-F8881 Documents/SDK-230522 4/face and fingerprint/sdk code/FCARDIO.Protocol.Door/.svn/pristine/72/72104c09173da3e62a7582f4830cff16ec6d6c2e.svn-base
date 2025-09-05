using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.RecordStorageMode
{
    /// <summary>
    /// 写入记录存储方式 参数
    /// </summary>
    public class WriteRecordStorageMode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 记录存储方式
        /// 0表示记录满循环，1表示记录满不循环
        /// </summary>
        public int Mode;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteRecordStorageMode_Parameter()
        {

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="mode">记录存储方式 0表示记录满循环，1表示记录满不循环</param>
        public WriteRecordStorageMode_Parameter(int mode)
        {
            Mode = mode;
        }

        /// <summary>
        /// 检查参数 0或1
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Mode != 0 && Mode != 1)
                throw new ArgumentException("Mode Error!");
            return true;
        }

        public override void Dispose()
        {
           
        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Mode);
            return databuf;
        }

        /// <summary>
        /// 获取长度
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
            Mode = databuf.ReadByte();
        }
    }
}
