using DotNetty.Buffers;
using FCARDIO.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCARDIO.Core.Util;

namespace FCARDIO.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置智能防盗主机参数_参数
    /// </summary>
    public class WriteTheftAlarmSetting_Parameter : Protocol.Door.FC8800.SystemParameter.FunctionParameter.WriteTheftAlarmSetting_Parameter
    {
        

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteTheftAlarmSetting_Parameter() { }

        /// <summary>
        /// 使用防盗报警参数信息初始化实例
        /// </summary>
        /// <param name="_Setting">防盗报警参数信息</param>
        public WriteTheftAlarmSetting_Parameter(TheftAlarmSetting _Setting)
        {
            Setting = _Setting;
            if (!checkedParameter())
            {
                throw new ArgumentException("Setting Error");
            }
        }

    }
}