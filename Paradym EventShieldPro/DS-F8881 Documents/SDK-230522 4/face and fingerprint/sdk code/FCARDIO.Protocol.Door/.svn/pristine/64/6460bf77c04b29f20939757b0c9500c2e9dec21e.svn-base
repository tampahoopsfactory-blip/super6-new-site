using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Data
{
    /// <summary>
    /// 记录数据库的详情
    /// 读卡记录,  门磁,  系统记录  体温记录
    /// </summary>
    public class TransactionDatabaseDetail : DoNetDrive.Protocol.Door.Door8800.Data.TransactionDatabaseDetail
    {
        /// <summary>
        /// 表示体温记录详情
        /// </summary>
        public TransactionDetail BodyTemperatureTransactionDetail;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public TransactionDatabaseDetail()
        {
            CardTransactionDetail = new TransactionDetail();
            DoorSensorTransactionDetail = new TransactionDetail();
            SystemTransactionDetail = new TransactionDetail();
            BodyTemperatureTransactionDetail = new TransactionDetail();
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x0D * 4;
        }

        /// <summary>
        /// 进行解码
        /// </summary>
        /// <param name="data"></param>
        public override void SetBytes(IByteBuffer data)
        {
            if (data.ReadableBytes == 52)// 0x0D * 4;
            {
                ListTransaction = new DoNetDrive.Protocol.Door.Door8800.Data.TransactionDetail[] 
                { CardTransactionDetail, DoorSensorTransactionDetail, SystemTransactionDetail, BodyTemperatureTransactionDetail };
            }
            else// 0x0D * 3;
            {
                ListTransaction = new DoNetDrive.Protocol.Door.Door8800.Data.TransactionDetail[] 
                { CardTransactionDetail, DoorSensorTransactionDetail, SystemTransactionDetail };
            }

            for (int i = 0; i < ListTransaction.Length; i++)
            {
                ListTransaction[i].SetBytes(data);

                if(ListTransaction[i].ReadIndex> ListTransaction[i].WriteIndex)
                {
                    ListTransaction[i].ReadIndex = 0;

                }
            }
            return;
        }
    }
}
