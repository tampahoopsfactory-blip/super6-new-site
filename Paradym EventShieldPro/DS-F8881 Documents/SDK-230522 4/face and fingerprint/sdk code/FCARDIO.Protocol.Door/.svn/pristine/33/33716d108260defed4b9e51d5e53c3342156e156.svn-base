using DotNetty.Buffers;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.Util;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Data.Transaction
{
    public class CardAndImageTransaction : CardTransaction
    {
        /// <summary>
        /// 体温，整数，需要除10
        /// </summary>
        public int BodyTemperature;

        

        /// <summary>
        /// 照片路径
        /// </summary>
        public string PhotoFile;

        /// <summary>
        /// 照片尺寸
        /// </summary>
        public int PhotoSize;

        /// <summary>
        /// 照片缓冲区
        /// </summary>
        public byte[] PhotoDataBuf;

        /// <summary>
        /// 创建一个认证记录
        /// </summary>
        /// <param name="ct"></param>
        public CardAndImageTransaction(CardTransaction ct)
        {
            RecordSerialNumber = ct.RecordSerialNumber;
            _SerialNumber = ct.SerialNumber;
            UserCode = ct.UserCode;
            _TransactionDate =ct.TransactionDate;
            Accesstype = ct.Accesstype;
            _TransactionCode =ct.TransactionCode;
            Photo = ct.Photo;
        }
    }
}
