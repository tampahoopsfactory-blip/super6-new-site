using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Transaction
{
    /// <summary>
    /// 读取新记录
    /// </summary>
    public class ReadTransactionDatabase_Parameter
        : AbstractParameter
    {
        /// <summary>
        ///  记录数据库类型
        ///  1 读卡记录
        ///  2 出门开关记录
        ///  3 门磁记录
        ///  4 软件操作记录
        ///  5 报警记录
        ///  6 系统记录
        /// </summary>
        public e_TransactionDatabaseType DatabaseType;

        /// <summary>
        /// 读取数量 0-160000,0表示都取所有新记录
        /// </summary>
        public int Quantity;

        /// <summary>
        ///  每次读取数量 1-150
        /// </summary>
        public int PacketSize = 200;

        /// <summary>
        /// 自动更新读索引（上传断点）
        /// </summary>
        public bool AutoWriteReadIndex;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="type">取值范围 1-6</param>
        /// <param name="_Quantity">读取数量</param>
        public ReadTransactionDatabase_Parameter(e_TransactionDatabaseType type, int _Quantity)
        {
            AutoWriteReadIndex = true;
            DatabaseType = type;
            Quantity = _Quantity;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (PacketSize < 1 || PacketSize > 200)
            {
                throw new ArgumentException("PacketSize Error!");
            }
            if (Quantity < 0 || Quantity > 300000)
            {
                throw new ArgumentException("Quantity Error!");
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
            if (databuf.WritableBytes != 0)
            {
                throw new ArgumentException("Crad Error");
            }
            databuf.WriteByte(PacketSize);
            databuf.WriteByte(Quantity);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            PacketSize = databuf.ReadByte();
            Quantity = databuf.ReadByte();
        }
    }
}
