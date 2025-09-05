using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace FCARDIO.Protocol.Door.FC8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置读卡器密码键盘启用功能开关_参数
    /// </summary>
    public class WriteKeyboard_Parameter : AbstractParameter
    {
        /// <summary>
        /// 读卡器密码键盘启用功能开关（0 - 关闭、1 - 开启）（Bit0 - 1号读头、Bit1 - 2号读头、Bit2 - 3号读头、Bit3 - 4号读头、Bit4 - 5号读头、Bit5 - 6号读头、Bit6 - 7号读头、Bit7 - 8号读头）
        /// </summary>
        public ushort Keyboard;

        public WriteKeyboard_Parameter(ushort _Keyboard)
        {
            Keyboard = _Keyboard;
        }

        public override bool checkedParameter()
        {
            if (Keyboard != 0 && Keyboard != 1)
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
            return databuf.WriteUnsignedShort(Keyboard);
        }

        public override int GetDataLen()
        {
            return 0x01;
        }

        public override void SetBytes(IByteBuffer databuf)
        {
            Keyboard = databuf.ReadUnsignedShort();
        }
    }
}