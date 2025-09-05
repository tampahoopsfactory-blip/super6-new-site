using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.POS.SystemParameter.ScreenDisplay
{
    /// <summary>
    /// 设置消费时显示内容命令参数
    /// </summary>
    public class WriteDisplayContent_Parameter : AbstractParameter
    {
        /// <summary>
        /// 人名
        /// </summary>
        public byte Name;

        /// <summary>
        /// 编号
        /// </summary>
        public byte PCode;

        /// <summary>
        /// 部门
        /// </summary>
        public byte Dept;

        /// <summary>
        /// 职务
        /// </summary>
        public byte Job;

        /// <summary>
        /// 余额
        /// </summary>
        public byte Balance;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteDisplayContent_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Name">人名</param>
        /// <param name="PCode">编号</param>
        /// <param name="Dept">部门</param>
        /// <param name="Job">职务</param>
        /// <param name="Balance">余额</param>
        public WriteDisplayContent_Parameter(byte Name, byte PCode, byte Dept, byte Job, byte Balance)
        {
            this.Name = Name;
            this.PCode = PCode;
            this.Dept = Dept;
            this.Job = Job;
            this.Balance = Balance;
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
            if (Name != 0 && Name != 1)
                throw new ArgumentException("Name Error!");
            if (PCode != 0 && PCode != 1)
                throw new ArgumentException("PCode Error!");
            if (Dept != 0 && Dept != 1)
                throw new ArgumentException("Dept Error!");
            if (Job != 0 && Job != 1)
                throw new ArgumentException("Job Error!");
            if (Balance != 0 && Balance != 1)
                throw new ArgumentException("Balance Error!");
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
            databuf.WriteByte(Name);
            databuf.WriteByte(PCode);
            databuf.WriteByte(Dept);
            databuf.WriteByte(Job);
            databuf.WriteByte(Balance);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x05;
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
            Name = databuf.ReadByte();
            PCode = databuf.ReadByte();
            Dept = databuf.ReadByte();
            Job = databuf.ReadByte();
            Balance = databuf.ReadByte();
        }
    }
}
