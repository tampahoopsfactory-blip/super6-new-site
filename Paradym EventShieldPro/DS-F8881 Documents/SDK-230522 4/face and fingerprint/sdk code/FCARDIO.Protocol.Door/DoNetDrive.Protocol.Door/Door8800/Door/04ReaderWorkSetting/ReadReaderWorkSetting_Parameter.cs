using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace FCARDIO.Protocol.Door.FC8800.Door.ReaderWorkSetting
{
    /// <summary>
    /// 门端口
    /// </summary>
    public class ReadReaderWorkSetting_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门号
        /// </summary>
        private byte Door;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public ReadReaderWorkSetting_Parameter() { }

        /// <summary>
        /// 门端口参数初始化实例
        /// </summary>
        /// <param name="door">门号</param>
        public ReadReaderWorkSetting_Parameter(byte door)
        {
            Door = door;
            checkedParameter();
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Door < 1 || Door > 4)
                throw new ArgumentException("Door Error!");
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
        /// 对门端口参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(Door);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x01;
        }

        /// <summary>
        /// 对门端口参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override void SetBytes(IByteBuffer databuf)
        {
            Door = databuf.ReadByte();
        }
    }
}
