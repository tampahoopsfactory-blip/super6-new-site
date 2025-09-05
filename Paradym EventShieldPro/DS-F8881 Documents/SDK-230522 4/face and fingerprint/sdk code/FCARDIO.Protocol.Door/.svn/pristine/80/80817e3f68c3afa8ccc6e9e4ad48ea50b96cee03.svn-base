using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.Transaction
{
    /// <summary>
    /// 更新指针命令参数
    /// </summary>
    public class WriteTransactionDatabaseIndex_Parameter : AbstractParameter
    {

        /// <summary>
        ///  记录数据库类型
        ///  1  读卡记录
        ///  2  系统记录
        /// </summary>
        public e_TransactionDatabaseType DatabaseType;
        /// <summary>
        /// 数据库中的引号
        /// </summary>
        public int Index;

        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="_DatabaseType">记录数据库类型</param>
        /// <param name="_ReadIndex">数据库中的读索引号</param>
        /// <param name="_IsCircle">循环标记</param>
        public WriteTransactionDatabaseIndex_Parameter(e_TransactionDatabaseType _DatabaseType, int _Index)
        {
            DatabaseType = _DatabaseType;
            Index = _Index;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {

            if (Index < 0)
            {
                throw new ArgumentException("ReadIndex Error!");
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
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != 5)
            {
                throw new ArgumentException("Error");
            }
            databuf.WriteByte((byte)DatabaseType);
            databuf.WriteInt(Index);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 5;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Index = databuf.ReadByte();
        }
    }
}