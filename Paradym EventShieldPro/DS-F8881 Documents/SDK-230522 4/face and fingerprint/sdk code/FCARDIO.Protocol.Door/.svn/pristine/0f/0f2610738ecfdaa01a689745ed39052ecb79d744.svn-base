using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置读卡器数据校验_参数
    /// </summary>
    public class WriteReaderCheckMode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 读卡器数据校验（0不启用，1启用，2启用校验，但不提示非法数据或线路异常）
        /// </summary>
        public byte ReaderCheckMode;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteReaderCheckMode_Parameter() { }

        /// <summary>
        /// 使用读卡器数据校验初始化实例
        /// </summary>
        /// <param name="_ReaderCheckMode">读卡器数据校验</param>
        public WriteReaderCheckMode_Parameter(byte _ReaderCheckMode)
        {
            ReaderCheckMode = _ReaderCheckMode;
            if (!checkedParameter())
            {
                throw new ArgumentException("ReaderCheckMode Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ReaderCheckMode != 0 && ReaderCheckMode != 1 && ReaderCheckMode != 2)
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
            return;
        }

        /// <summary>
        /// 对读卡器数据校验参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(ReaderCheckMode);
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
        /// 对读卡器数据校验参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            ReaderCheckMode = databuf.ReadByte();
        }
    }
}