using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace FCARDIO.Protocol.Door.FC8800.Door.RelayReleaseTime
{
    public class ReadRelayReleaseTime_Parameter
        : AbstractParameter
    {
        private const int _DataLength = 0x01;
        private byte[] _ReleaseTime = null;

        public ReadRelayReleaseTime_Parameter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="releaseTime"></param>
        public ReadRelayReleaseTime_Parameter(byte[] releaseTime)
        {
            _ReleaseTime = releaseTime;
            checkedParameter();
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (_ReleaseTime == null)
                throw new ArgumentException("releaseTime Is Null!");
            if (_ReleaseTime.Length != _DataLength)
                throw new ArgumentException("releaseTime Length Error!");
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            _ReleaseTime = null;
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
            return databuf.WriteBytes(_ReleaseTime);
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
            if (_ReleaseTime == null)
            {
                _ReleaseTime = new byte[_DataLength];
            }
            if (databuf.ReadableBytes != _DataLength)
            {
                throw new ArgumentException("databuf Error");
            }
            databuf.ReadBytes(_ReleaseTime);
        }
    }
}
