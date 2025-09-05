using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.Door.Door8800;

namespace FCARDIO.Protocol.Door.FC8800.Door.ReaderInterval
{
    public class ReadReaderInterval_Parameter
         : AbstractParameter
    {
        private const int _DataLength = 0x01;
        private byte[] _ReaderInterval = null;

        public ReadReaderInterval_Parameter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerInterval"></param>
        public ReadReaderInterval_Parameter(byte[] readerInterval)
        {
            _ReaderInterval = readerInterval;
            checkedParameter();
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (_ReaderInterval == null)
                throw new ArgumentException("readerInterval Is Null!");
            if (_ReaderInterval.Length != _DataLength)
                throw new ArgumentException("readerInterval Length Error!");
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            _ReaderInterval = null;
        }

        /// <summary>
        /// 对SN参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != _DataLength)
            {
                throw new ArgumentException("databuf Error!");
            }
            return databuf.WriteBytes(_ReaderInterval);
        }

        public override int GetDataLen()
        {
            return _DataLength;
        }

        /// <summary>
        /// 对SN参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (_ReaderInterval == null)
            {
                _ReaderInterval = new byte[_DataLength];
            }
            if (databuf.ReadableBytes != _DataLength)
            {
                throw new ArgumentException("databuf Error");
            }
            databuf.ReadBytes(_ReaderInterval);
        }
    }
}
