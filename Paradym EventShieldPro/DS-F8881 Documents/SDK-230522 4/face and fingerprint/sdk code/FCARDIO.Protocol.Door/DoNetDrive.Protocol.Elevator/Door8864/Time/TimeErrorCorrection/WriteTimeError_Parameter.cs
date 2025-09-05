using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.Time.TimeErrorCorrection
{
    /// <summary>
    /// 设置误差自修正_参数
    /// </summary>
    public class WriteTimeError_Parameter : DoNetDrive.Protocol.Door.Door8800.Time.TimeErrorCorrection.WriteTimeError_Parameter
    {/// <summary>
     /// 构建一个空的实例
     /// </summary>
        public WriteTimeError_Parameter()
        {
        }

        /// <summary>
        /// 误差自修正参数初始化实例
        /// </summary>
        /// <param name="_TimeErrorCorrection">误差自修正参数</param>
        public WriteTimeError_Parameter(byte[] _TimeErrorCorrection)
        {
            TimeErrorCorrection = _TimeErrorCorrection;

        }
    }
}