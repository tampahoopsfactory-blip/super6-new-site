using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.WorkStatus
{
    /// <summary>
    /// 获取防盗主机布防状态_结果
    /// </summary>
    public class ReadTheftAlarmState_Result : INCommandResult
    {
        /// <summary>
        /// 布防状态<br/>
        /// <ul>
        /// <li>1  延时布防</li>
        /// <li>2  已布防</li>
        /// <li>3  延时撤防</li>
        /// <li>4  未布防</li>
        /// <li>5  报警延时，准备启用报警</li>
        /// <li>6  防盗报警已启动</li>
        /// </ul>
        /// </summary>
        public byte TheftState;

        /// <summary>
        /// 防盗主机报警状态（0未报警，1已报警）
        /// </summary>
        public byte TheftAlarm;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            return;
        }

        /// <summary>
        /// 对防盗主机布防状态参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            TheftState = databuf.ReadByte();
            TheftAlarm = databuf.ReadByte();
        }
    }
}