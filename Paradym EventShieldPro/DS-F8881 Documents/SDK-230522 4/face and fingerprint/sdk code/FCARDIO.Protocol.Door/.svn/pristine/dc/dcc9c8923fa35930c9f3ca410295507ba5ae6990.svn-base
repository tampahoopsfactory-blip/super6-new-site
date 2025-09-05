using DotNetty.Buffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置读卡器密码键盘启用功能开关_参数
    /// </summary>
    public class WriteKeyboard_Parameter : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteKeyboard_Parameter
    {
        /// <summary>
        /// 
        /// </summary>
        public WriteKeyboard_Parameter()
        {

        }
        /// <summary>
        /// 读卡器密码键盘启用功能开关（0 - 关闭、1 - 开启）（Bit0 - 1号读头、Bit1 - 2号读头）
        /// </summary>
        //public BitArray Keyboard;
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
            if (Keyboard == null || Keyboard.Length != 2)
            {
                return false;
            }
            return true;
        }

    }
}