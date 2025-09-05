using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data
{
    /// <summary>
    /// 记录数据库的详情
    /// 读卡记录, 出门开关, 门磁, 远程开门, 报警, 系统记录
    /// </summary>
    public class TransactionDatabaseDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public TransactionDetail[] ListTransaction { get; set; }
        /// <summary>
        /// 读卡相关记录
        /// </summary>
        public TransactionDetail CardTransactionDetail;
        /// <summary>
        /// 出门开关相关记录
        /// </summary>
        public TransactionDetail ButtonTransactionDetail;
        /// <summary>
        /// 门磁相关记录
        /// </summary>
        public TransactionDetail DoorSensorTransactionDetail;
        /// <summary>
        /// 远程操作相关记录
        /// </summary>
        public TransactionDetail SoftwareTransactionDetail;
        /// <summary>
        /// 报警相关记录
        /// </summary>
        public TransactionDetail AlarmTransactionDetail;
        /// <summary>
        /// 系统相关记录
        /// </summary>
        public TransactionDetail SystemTransactionDetail;

        /// <summary>
        /// 初始化参数
        /// </summary>
        public TransactionDatabaseDetail()
        {
            CardTransactionDetail = new TransactionDetail();
            ButtonTransactionDetail = new TransactionDetail();
            DoorSensorTransactionDetail = new TransactionDetail();
            SoftwareTransactionDetail = new TransactionDetail();
            AlarmTransactionDetail = new TransactionDetail();
            SystemTransactionDetail = new TransactionDetail();
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public virtual int GetDataLen()
        {
            return 0x0D * 6;
        }

        /// <summary>
        /// 进行解码
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetBytes(IByteBuffer data)
        {
            ListTransaction = new TransactionDetail[]{CardTransactionDetail, ButtonTransactionDetail, DoorSensorTransactionDetail,
            SoftwareTransactionDetail, AlarmTransactionDetail, SystemTransactionDetail};
            for (int i = 0; i < ListTransaction.Length; i++)
            {
                ListTransaction[i].SetBytes(data);
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IByteBuffer GetBytes()
        {
            return null;
        }
    }
}
