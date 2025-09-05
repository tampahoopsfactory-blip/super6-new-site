using DotNetty.Buffers;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Data.Transaction
{
    /// <summary>
    /// 系统记录
    /// </summary>
    public class SystemTransaction : AbstractTransaction
    {
        public SystemTransaction()
        {
            _TransactionType = 2;
        }
        /// <summary>
        /// 获取读卡记录格式长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 8;
        }

        /// <summary>
        /// 从buf中读取记录数据
        /// </summary>
        /// <param name="dtBuf"></param>
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

                _TransactionCode = data.ReadByte();
                _TransactionDate = TimeUtil.BCDTimeToDate_yyMMddhhmmss(data);
                data.ReadByte();
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            {
            }

            return;
        }
    }
}
