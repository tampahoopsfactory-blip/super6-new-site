using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace FCARDIO.Protocol.Door.FC8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置记录存储方式_参数
    /// </summary>
    public class WriteRecordMode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 记录存储方式（0 - 满循环、1 - 满不循环）
        /// </summary>
        public ushort Mode;

        public WriteRecordMode_Parameter(ushort _Mode)
        {
            Mode = _Mode;
        }

        public override bool checkedParameter()
        {
            if (Mode != 0 && Mode != 1)
            {
                return false;
            }

            return true;
        }

        public override void Dispose()
        {
            return;
        }

        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteUnsignedShort(Mode);
        }

        public override int GetDataLen()
        {
            return 0x01;
        }

        public override void SetBytes(IByteBuffer databuf)
        {
            Mode = databuf.ReadUnsignedShort();
        }
    }
}