using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.CancelDoorAlarm
{
    /// <summary>
    /// 解除端口报警 参数
    /// </summary>
    public class WriteCancelDoorAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 端口 1-64
        /// </summary>
        public int Door;

        /// <summary>
        /// 报警代码 1--解除门磁报警；2--解除开门超时报警。
        /// </summary>
        public int AlarmCode;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteCancelDoorAlarm_Parameter()
        {

        }
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="door">端口</param>
        /// <param name="alarmCode">报警代码</param>
        public WriteCancelDoorAlarm_Parameter(int door, int alarmCode)
        {
            Door = door;
            AlarmCode = alarmCode;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Door < 1 || Door > 64)
            {
                return false;
            }
            if (Door < 1 || Door > 2)
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
        /// 对误差自修正参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Door);
            databuf.WriteByte(AlarmCode);

            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x02;
        }

        /// <summary>
        /// 对误差自修正参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Door = databuf.ReadByte();
            AlarmCode = databuf.ReadByte();

        }
    }
}
