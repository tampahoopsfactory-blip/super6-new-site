using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Data
{
    /// <summary>
    /// 适用于人脸机和指纹机的详情
    /// </summary>
    public class TransactionDetail: DoNetDrive.Protocol.Door.Door8800.Data.TransactionDetail
    {

        /// <summary>
        /// 可用的新记录数
        /// </summary>
        /// <returns>新记录数</returns>
        public override long readable()
        {
            if (WriteIndex < ReadIndex)
            {
                return 0;
            }

            if (ReadIndex == WriteIndex)
            {
                return 0;
            }

            return (WriteIndex - ReadIndex);
        }
    }
}
