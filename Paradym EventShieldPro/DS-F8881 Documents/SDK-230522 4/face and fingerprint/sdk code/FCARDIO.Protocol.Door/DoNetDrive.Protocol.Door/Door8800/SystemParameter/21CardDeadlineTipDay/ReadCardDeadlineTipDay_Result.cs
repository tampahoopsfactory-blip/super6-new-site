using FCARDIO.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCARDIO.Protocol.Door.FC8800.SystemParameter.CardDeadlineTipDay
{
    /// <summary>
    /// 获取有效期即将过期提醒时间_结果
    /// </summary>
    public class ReadCardDeadlineTipDay_Result : INCommandResult
    {
        public byte Day;

        public ReadCardDeadlineTipDay_Result(byte _Day)
        {
            Day = _Day;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            return;
        }
    }
}