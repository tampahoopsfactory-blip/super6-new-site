using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.TTLOutput
{
    /// <summary>
    /// 设置TTL输出参数 参数
    /// </summary>
    public class WriteTTLOutput_Parameter : AbstractParameter
    {
        /// <summary>
        /// 开关
        /// </summary>
        public bool IsOpen;

        /// <summary>
        /// 波特率 
        /// 0 - 1200
        /// 1 - 2400
        /// 2 - 4800
        /// 3 - 9600
        /// 4 - 11400
        /// 5 - 19200
        /// 6 - 38400
        /// 7 - 43000
        /// 8 - 56000
        /// 9 - 57600
        /// 10 - 115200
        /// </summary>
        public int BaudRate;

        /// <summary>
        /// 奇偶校验
        /// 0 - N(无)
        /// 1 - E(偶数)
        /// 2 - O(奇数)
        /// </summary>
        public int Parity;

        /// <summary>
        /// 数据位数
        /// 4-8
        /// </summary>
        public int DataBits;

        /// <summary>
        /// 停止位数
        /// 0 - 1
        /// 1 - 1.5
        /// 2 - 2
        /// </summary>
        public int StopBits;
        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteTTLOutput_Parameter()
        {

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="isOpen">开关</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">奇偶校验</param>
        /// <param name="dataBits">数据位数</param>
        /// <param name="stopBits">停止位数</param>
        public WriteTTLOutput_Parameter(bool isOpen, int baudRate, int parity, int dataBits, int stopBits)
        {
            IsOpen = isOpen;
            BaudRate = baudRate;
            Parity = parity;
            DataBits = dataBits;
            StopBits = stopBits;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (BaudRate > 10 || BaudRate < 0)
                throw new ArgumentException("BaudRate Error!");
            if (Parity > 2 || Parity < 0)
                throw new ArgumentException("Parity > Error!");
            if (DataBits > 8 || DataBits < 4)
                throw new ArgumentException("DataBits Error!");
            if (StopBits > 2 || StopBits < 0)
                throw new ArgumentException("StopBits Error!");
            return true;
        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteBoolean(IsOpen);
            databuf.WriteByte(BaudRate);
            databuf.WriteByte(Parity);
            databuf.WriteByte(DataBits);
            databuf.WriteByte(StopBits);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x05;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsOpen = databuf.ReadBoolean();
            BaudRate = databuf.ReadByte();
            Parity = databuf.ReadByte();
            DataBits = databuf.ReadByte();
            StopBits = databuf.ReadByte();
        }
    }
}
