using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace FCARDIO.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置定时读卡播报语音消息_参数
    /// </summary>
    public class WriteCardPeriodSpeak_Parameter : Protocol.Door.FC8800.SystemParameter.FunctionParameter.WriteCardPeriodSpeak_Parameter
    {
        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteCardPeriodSpeak_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="isUse">是否启用</param>
        public WriteCardPeriodSpeak_Parameter(byte _Use)
        {
            Use = _Use;
        }

    }
}