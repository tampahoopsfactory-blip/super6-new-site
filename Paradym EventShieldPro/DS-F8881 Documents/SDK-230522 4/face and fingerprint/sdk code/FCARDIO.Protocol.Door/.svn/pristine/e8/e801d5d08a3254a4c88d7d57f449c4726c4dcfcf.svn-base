using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置消防报警参数_参数
    /// </summary>
    public class WriteFireAlarmOption_Parameter : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteFireAlarmOption_Parameter
    {
        /// <summary>
        /// 
        /// </summary>
        public WriteFireAlarmOption_Parameter()
        {

        }
        /// <summary>
        /// 消防报警参数（0 - 不启用、1 - 报警输出，并开所有门，只能软件解除、2 - 报警输出，不开所有门，只能软件解除）
        /// </summary>
        //public byte Option;

        public WriteFireAlarmOption_Parameter(byte _Option)
        {
            Option = _Option;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Option != 0 && Option != 1 && Option != 2)
            {
                return false;
            }

            return true;
        }

    }
}