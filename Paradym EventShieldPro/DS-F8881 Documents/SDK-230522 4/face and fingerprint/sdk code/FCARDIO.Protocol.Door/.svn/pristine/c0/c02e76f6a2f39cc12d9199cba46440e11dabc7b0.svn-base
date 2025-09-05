using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.POS.SystemParameter.USBDisk
{
    /// <summary>
    /// 设置U盘命令参数
    /// </summary>
    public class WriteUSBDisk_Parameter : AbstractParameter
    {
        /// <summary>
        /// 卡片名单
        /// </summary>
        public byte UploadCardList;

        /// <summary>
        /// 菜单
        /// </summary>
        public byte UploadMenu;

        /// <summary>
        /// 消费时段
        /// </summary>
        public byte UploadTimeGroup;

        /// <summary>
        /// 卡类名单
        /// </summary>
        public byte UploadCardTypeList;

        /// <summary>
        /// 消费参数
        /// </summary>
        public byte UploadConsumeParameter;

        /// <summary>
        /// 程序固件升
        /// </summary>
        public byte UploadUpgrade;


        /// <summary>
        /// 下载卡片名单
        /// </summary>
        public byte DownloadCardList;

        /// <summary>
        /// 下载菜单
        /// </summary>
        public byte DownloadMenu;

        /// <summary>
        /// 下载消费时段
        /// </summary>
        public byte DownloadTimeGroup;

        /// <summary>
        /// 下载卡类名单
        /// </summary>
        public byte DownloadCardTypeList;

        /// <summary>
        /// 下载消费参数
        /// </summary>
        public byte DownloadConsumeParameter;

        /// <summary>
        /// 下载消费日志
        /// </summary>
        public byte DownloadTransaction;

        /// <summary>
        /// 下载系统日志
        /// </summary>
        public byte DownloadSystemTransaction;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteUSBDisk_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="CardList"></param>
        /// <param name="Menu"></param>
        /// <param name="TimeGroup"></param>
        /// <param name="CardTypeList"></param>
        /// <param name="ConsumeParameter"></param>
        /// <param name="Upgrade"></param>
        /// <param name="DownloadCardList"></param>
        /// <param name="DownloadMenu"></param>
        /// <param name="DownloadTimeGroup"></param>
        /// <param name="DownloadCardTypeList"></param>
        /// <param name="DownloadConsumeParameter"></param>
        /// <param name="DownloadTransaction"></param>
        /// <param name="DownloadSystemTransaction"></param>
        public WriteUSBDisk_Parameter(byte CardList, byte Menu, byte TimeGroup, byte CardTypeList, byte ConsumeParameter, byte Upgrade,
            byte DownloadCardList, byte DownloadMenu, byte DownloadTimeGroup, byte DownloadCardTypeList, byte DownloadConsumeParameter, byte DownloadTransaction, byte DownloadSystemTransaction)
        {
            this.UploadCardList = CardList;
            this.UploadMenu = Menu;
            this.UploadTimeGroup = TimeGroup;
            this.UploadCardTypeList = CardTypeList;
            this.UploadConsumeParameter = ConsumeParameter;
            this.UploadUpgrade = Upgrade;

            this.DownloadCardList = DownloadCardList;
            this.DownloadMenu = DownloadMenu;
            this.DownloadTimeGroup = DownloadTimeGroup;
            this.DownloadCardTypeList = DownloadCardTypeList;
            this.DownloadConsumeParameter = DownloadConsumeParameter;
            this.DownloadTransaction = DownloadTransaction;
            this.DownloadSystemTransaction = DownloadSystemTransaction;

            if (!checkedParameter())
            {
                throw new ArgumentException("Parameter Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (UploadCardList != 0 && UploadCardList != 1)
                throw new ArgumentException("UploadCardList Error!");
            if (UploadMenu != 0 && UploadMenu != 1)
                throw new ArgumentException("UploadMenu Error!");
            if (UploadTimeGroup != 0 && UploadTimeGroup != 1)
                throw new ArgumentException("UploadTimeGroup Error!");
            if (UploadCardTypeList != 0 && UploadCardTypeList != 1)
                throw new ArgumentException("UploadCardTypeList Error!");
            if (UploadConsumeParameter != 0 && UploadConsumeParameter != 1)
                throw new ArgumentException("UploadConsumeParameter Error!");
            if (UploadUpgrade != 0 && UploadUpgrade != 1)
                throw new ArgumentException("UploadUpgrade Error!");

            if (DownloadCardList != 0 && DownloadCardList != 1)
                throw new ArgumentException("DownloadCardList Error!");

            if (DownloadMenu != 0 && DownloadMenu != 1)
                throw new ArgumentException("DownloadMenu Error!");
            if (DownloadTimeGroup != 0 && DownloadTimeGroup != 1)
                throw new ArgumentException("DownloadTimeGroup Error!");
            if (DownloadCardTypeList != 0 && DownloadCardTypeList != 1)
                throw new ArgumentException("DownloadCardTypeList Error!");
            if (DownloadConsumeParameter != 0 && DownloadConsumeParameter != 1)
                throw new ArgumentException("DownloadConsumeParameter Error!");
            if (DownloadTransaction != 0 && DownloadTransaction != 1)
                throw new ArgumentException("DownloadTransaction Error!");
            if (DownloadSystemTransaction != 0 && DownloadSystemTransaction != 1)
                throw new ArgumentException("DownloadSystemTransaction Error!");
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
        /// 对有效期参数进行编码
        /// </summary>
        /// <param name="databuf">需要填充参数结构的字节缓冲区</param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf len error");
            }
            databuf.WriteByte(UploadCardList);
            databuf.WriteByte(UploadMenu);
            databuf.WriteByte(UploadTimeGroup);
            databuf.WriteByte(UploadCardTypeList);
            databuf.WriteByte(UploadConsumeParameter);
            databuf.WriteByte(UploadUpgrade);

            databuf.WriteByte(DownloadCardList);
            databuf.WriteByte(DownloadMenu);
            databuf.WriteByte(DownloadTimeGroup);
            databuf.WriteByte(DownloadCardTypeList);
            databuf.WriteByte(DownloadConsumeParameter);
            databuf.WriteByte(DownloadTransaction);
            databuf.WriteByte(DownloadSystemTransaction);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x0D;
        }

        /// <summary>
        /// 对有效期参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            UploadCardList = databuf.ReadByte();
            UploadMenu = databuf.ReadByte();
            UploadTimeGroup = databuf.ReadByte();
            UploadCardTypeList = databuf.ReadByte();
            UploadConsumeParameter = databuf.ReadByte();
            UploadUpgrade = databuf.ReadByte();

            DownloadCardList = databuf.ReadByte();
            DownloadMenu = databuf.ReadByte();
            DownloadTimeGroup = databuf.ReadByte();
            DownloadCardTypeList = databuf.ReadByte();
            DownloadConsumeParameter = databuf.ReadByte();
            DownloadTransaction = databuf.ReadByte();
            DownloadSystemTransaction = databuf.ReadByte();


        }
    }
}
