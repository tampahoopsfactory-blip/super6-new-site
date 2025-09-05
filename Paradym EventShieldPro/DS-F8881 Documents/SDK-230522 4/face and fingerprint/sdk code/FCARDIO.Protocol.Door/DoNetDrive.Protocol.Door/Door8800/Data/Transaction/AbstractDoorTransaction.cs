using DotNetty.Buffers;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data
{
    /// <summary>
    /// 关于门的事件抽象类
    /// </summary>
    public abstract class AbstractDoorTransaction : SystemTransaction
    {
        /// <summary>
        /// 门号
        /// </summary>
        public short Door;
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="type">记录模块代码</param>
        public AbstractDoorTransaction(int type)
        {
            _TransactionType = (short)type;
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

                if(_IsNull)
                {
                    ReadNullRecord(data);
                    return;
                }

                Door = data.ReadByte();
                _TransactionDate = TimeUtil.BCDTimeToDate_yyMMddhhmmss(data);
                _TransactionCode = data.ReadByte();
            }
            catch (Exception e)
            {
            }

            return;
        }
    }
}
