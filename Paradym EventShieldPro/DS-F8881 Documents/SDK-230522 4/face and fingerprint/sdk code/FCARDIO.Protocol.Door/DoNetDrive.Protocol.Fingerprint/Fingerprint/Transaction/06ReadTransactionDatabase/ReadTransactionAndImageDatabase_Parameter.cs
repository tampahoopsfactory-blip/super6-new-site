using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.Fingerprint.Data.Transaction;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Transaction
{
    /// <summary>
    /// 读取记录、体温、照片的参数
    /// </summary>
    public class ReadTransactionAndImageDatabase_Parameter : AbstractParameter
    {
        /// <summary>
        /// 自动回滚读索引（适用于人脸机固件版本小于V4.41）
        /// </summary>
        public bool RollbackWriteReadIndex;

        /// <summary>
        /// 自动更新读索引（上传断点）
        /// </summary>
        public bool AutoWriteReadIndex;

        /// <summary>
        /// 自动读取照片
        /// </summary>
        public bool AutoDownloadImage;

        /// <summary>
        /// 图片下载检测器，用来判断是否需要下载此图片
        /// </summary>
        /// <returns>true--需要下载照片；false--不需要下载照片</returns>
        public Func<int, CardAndImageTransaction, bool> ImageDownloadCheckCallblack;

        /// <summary>
        /// 读取数量 1-500
        /// </summary>
        public int Quantity;

        /// <summary>
        ///  每次读取数量 1-60
        /// </summary>
        public int PacketSize = 60;

        /// <summary>
        /// 照片文件保存的文件夹
        /// </summary>
        public string SaveImageDirectory { get; set; }

        /// <summary>
        /// 将照片保存到文件
        /// </summary>
        public bool PhotoSaveToFile;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="type">取值范围 1-6</param>
        /// <param name="_Quantity">读取数量 1 - 50</param>
        public ReadTransactionAndImageDatabase_Parameter(int _Quantity, bool savetoFile, string _SaveImageDirectory)
        {
            RollbackWriteReadIndex = false;
            AutoWriteReadIndex = true;
            AutoDownloadImage = true;
            PacketSize = 60;
            PhotoSaveToFile = savetoFile;
            SaveImageDirectory = _SaveImageDirectory;
            Quantity = _Quantity;
            checkedParameter();
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (PhotoSaveToFile)
            {
                if (string.IsNullOrEmpty(SaveImageDirectory))
                {
                    throw new ArgumentException("SaveImagePath Error!");
                }
                if (!Directory.Exists(SaveImageDirectory))
                {
                    throw new ArgumentException("SaveImagePath Error!");
                }
            }

            if (PacketSize < 1 || PacketSize > 60)
            {
                PacketSize = 60;
            }
            if (Quantity < 1 || Quantity > 500)
            {
                throw new ArgumentException("Quantity value error (1-500)!");
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
