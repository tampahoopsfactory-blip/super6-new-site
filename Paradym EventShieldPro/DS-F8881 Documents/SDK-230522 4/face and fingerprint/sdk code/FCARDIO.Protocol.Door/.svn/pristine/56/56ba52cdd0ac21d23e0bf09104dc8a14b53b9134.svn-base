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
    /// 读卡记录
    /// </summary>
    public class CardTransaction : AbstractTransaction
    {
        /// <summary>
        /// 工号
        /// </summary>
        public ushort PCode;

        /// <summary>
        /// 卡号
        /// </summary>
        public uint CardData;

        /// <summary>
        /// 状态
        /// 0  普通卡
        /// 1  巡更人员卡
        /// </summary>
        public int State;

        /// <summary>
        /// 获取读卡记录格式长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 12;
        }

        public CardTransaction()
        {
            _TransactionType = 1;
        }
        /// <summary>
        /// 从buf中读取记录数据
        /// </summary>
        /// <param name="dtBuf"></param>
        public override void SetBytes(IByteBuffer dtBuf)
        {
            try
            {
                PCode = dtBuf.ReadUnsignedShort();
                if (PCode == ushort.MaxValue)
                {
                    _IsNull = true;
                    PCode = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        dtBuf.ReadByte();
                    }
                    
                    return;
                }

                int year = dtBuf.ReadByte() + 2000;

                try
                {
                    _TransactionDate = new DateTime(year, dtBuf.ReadByte(), dtBuf.ReadByte(), dtBuf.ReadByte(), dtBuf.ReadByte(), dtBuf.ReadByte());
                }
                catch (Exception)
                {

                    _TransactionDate = DateTime.MinValue;
                }
                

                State = dtBuf.ReadByte();


                CardData = (uint)dtBuf.ReadUnsignedMedium();
               
            }
            catch (Exception e)
            {
            }

            return;

        }
    }
}
