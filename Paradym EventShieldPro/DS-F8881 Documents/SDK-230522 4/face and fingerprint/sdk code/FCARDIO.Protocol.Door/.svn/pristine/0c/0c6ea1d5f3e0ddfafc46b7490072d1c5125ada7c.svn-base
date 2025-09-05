using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;


namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 读取定时常开参数命令的参数
    /// </summary>
    public class ReadTimingOpen_Parameter : AbstractParameter
    {
        /// <summary>
        /// 端口号
        /// </summary>
        public byte Port;

        /// <summary>
        /// 创建读取定时常开参数命令的参数
        /// </summary>
        /// <param name="Port">端口号</param>
        public ReadTimingOpen_Parameter(byte Port)
        {
            this.Port = Port;
            checkedParameter();
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Port < 1 || Port > 64)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// 对电梯工作模式进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(Port);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }

        /// <summary>
        /// 对电梯工作模式进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

            Port = databuf.ReadByte();
        }
    }
}
