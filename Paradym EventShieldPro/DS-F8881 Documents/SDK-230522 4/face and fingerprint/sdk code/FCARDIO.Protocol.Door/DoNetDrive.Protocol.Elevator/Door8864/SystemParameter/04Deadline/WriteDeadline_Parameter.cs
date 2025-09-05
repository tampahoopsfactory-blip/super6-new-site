using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.Deadline
{
    /// <summary>
    /// 设置设备有效期_参数
    /// </summary>
    public class WriteDeadline_Parameter : Protocol.Door.Door8800.SystemParameter.Deadline.WriteDeadline_Parameter
    {
        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteDeadline_Parameter() { }

        /// <summary>
        /// 使用设备有效期初始化实例
        /// </summary>
        /// <param name="_Deadline">设备有效期</param>
        public WriteDeadline_Parameter(ushort _Deadline)
        {
            Deadline = _Deadline;
            if (!checkedParameter())
            {
                throw new ArgumentException("Deadline Error");
            }
        }
    }
}