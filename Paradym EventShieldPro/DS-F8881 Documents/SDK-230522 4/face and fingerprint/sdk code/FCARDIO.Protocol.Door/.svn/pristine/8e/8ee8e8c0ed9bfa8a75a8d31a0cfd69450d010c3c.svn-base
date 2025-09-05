using DoNetDrive.Protocol.Transaction;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Data.Transaction
{
    /// <summary>
    /// 表示一个体温记录
    /// </summary>
    public class BodyTemperatureTransaction : AbstractTransaction
    {
        /// <summary>
        /// 记录逻辑存储位置序号
        /// </summary>
        public int RecordSerialNumber { get; protected set; }

        /// <summary>
        /// 体温
        /// </summary>
        protected int Temperature;
        /// <summary>
        /// 体温，整数，需要除10
        /// </summary>
        public int BodyTemperature => Temperature;

        /// <summary>
        /// 创建一个体温记录
        /// </summary>
        public BodyTemperatureTransaction()
        {
            _TransactionType = 4;
            _IsNull = false;
        }


        /// <summary>
        /// 指示一个事务记录所占用的缓冲区长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 6;
        }

        /// <summary>
        /// 使用缓冲区构造一个事务实例
        /// </summary>
        /// <param name="data">缓冲区</param>
        public override void SetBytes(IByteBuffer data)
        {
            try
            {
                _IsNull = CheckNull(data, 2);

                if (_IsNull)
                {
                    ReadNullRecord(data);
                    return;
                }
                RecordSerialNumber = _SerialNumber;
                _SerialNumber = data.ReadInt();
                _TransactionDate = DateTime.MinValue;
                _TransactionCode = 1;
                Temperature = data.ReadUnsignedShort();
            }
            catch (Exception e)
            {
            }

            return;
        }
    }
}
