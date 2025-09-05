using DotNetty.Buffers;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.Util;

namespace DoNetDrive.Protocol.Fingerprint.Data.Transaction
{
    public class CardTransaction : AbstractTransaction
    {
        /// <summary>
        /// 记录唯一序号
        /// </summary>
        public uint RecordSerialNumber { get; protected set; }
        /// <summary>
        /// 用户号
        /// </summary>
        public uint UserCode { get; protected set; }
        /// <summary>
        /// 是否包含照片
        /// </summary>
        public byte Photo { get; protected set; }

        /// <summary>
        /// 出入类型：1--表示进门；2--表示出门
        /// </summary>
        public byte Accesstype { get; protected set; }

        /// <summary>
        /// 初始化参数
        /// </summary>
        public CardTransaction()
        {
            _TransactionType = 1;
        }

        /// <summary>
        /// 获取读卡记录格式长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 13;
        }

        /// <summary>
        /// 从buf中读取记录数据
        /// </summary>
        /// <param name="dtBuf"></param>
        public override void SetBytes(IByteBuffer dtBuf)
        {
            
            RecordSerialNumber = dtBuf.ReadUnsignedInt();
            _SerialNumber = (int)RecordSerialNumber;
            UserCode = dtBuf.ReadUnsignedInt();
            byte[] time = new byte[6];
            dtBuf.ReadBytes(time, 0, 6);
            _TransactionDate = TimeUtil.BCDTimeToDate_ssmmhhddMMyy(time);
            Accesstype = dtBuf.ReadByte();
            _TransactionCode = dtBuf.ReadByte();
            Photo = dtBuf.ReadByte();
        }

        /// <summary>
        /// 设定是否具有照片
        /// </summary>
        /// <param name="iValue"></param>
        public void SetPhoto(byte iValue)
        {
            Photo = iValue;
        }
    }
}
