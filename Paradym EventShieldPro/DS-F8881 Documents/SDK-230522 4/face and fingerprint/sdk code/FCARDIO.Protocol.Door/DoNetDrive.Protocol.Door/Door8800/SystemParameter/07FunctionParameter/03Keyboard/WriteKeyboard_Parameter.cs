using DotNetty.Buffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置读卡器密码键盘启用功能开关_参数
    /// </summary>
    public class WriteKeyboard_Parameter : AbstractParameter
    {
        /// <summary>
        /// 读卡器密码键盘启用功能开关（0 - 关闭、1 - 开启）（Bit0 - 1号读头、Bit1 - 2号读头、Bit2 - 3号读头、Bit3 - 4号读头、Bit4 - 5号读头、Bit5 - 6号读头、Bit6 - 7号读头、Bit7 - 8号读头）
        /// </summary>
        public BitArray Keyboard;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteKeyboard_Parameter() { }

        /// <summary>
        /// 使用读卡器密码键盘启用功能开关初始化实例
        /// </summary>
        /// <param name="_Keyboard">读卡器密码键盘启用功能开关</param>
        public WriteKeyboard_Parameter(BitArray _Keyboard)
        {
            Keyboard = _Keyboard;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Keyboard == null || Keyboard.Length != 8)
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
        /// 对读卡器密码键盘启用功能开关参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(BitToByte(Keyboard));
        }

        /// <summary>
        /// BitArray转byte
        /// </summary>
        /// <param name="Keyboard"></param>
        /// <returns></returns>
        public int BitToByte(BitArray Keyboard)
        {
            byte[] be = new byte[1];
            Keyboard.CopyTo(be, 0);
            return be[0];
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
        /// 对读卡器密码键盘启用功能开关参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            byte[] ByteSet = new byte[] { databuf.ReadByte() };
            Keyboard = new BitArray(ByteSet);
        }
    }
}