using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.OutDoorSwitch
{
    /// <summary>
    /// 出门开关参数_参数
    /// </summary>
    public class WriteOutDoorSwitch_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门 1 - 65
        /// </summary>
        public byte Door;

        /// <summary>
        /// 功能是否启用
        /// </summary>
        public bool Use;

        /// <summary>
        /// 门工作方式时段
        /// </summary>
        public WeekTimeGroup weekTimeGroup;

        /// <summary>
        /// 提供给 DoorWorkSetting_Result 使用
        /// </summary>
        public WriteOutDoorSwitch_Parameter() {
            weekTimeGroup = new WeekTimeGroup(8);
        }

        /// <summary>
        /// 门工作方式参数初始化实例
        /// </summary>
        /// <param name="door">门</param>
        /// <param name="use">功能是否启用</param>
        /// <param name="openDoorWay">开门方式</param>
        /// <param name="doorTriggerMode">门常开触发模式</param>
        /// <param name="retainValue">保留值</param>
        /// <param name="tg">门工作方式时段</param>
        public WriteOutDoorSwitch_Parameter(byte door, bool use, WeekTimeGroup tg)
        {
            Door = door;
            Use = use;
            weekTimeGroup = tg;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Door < 1 || Door > 65)
                throw new ArgumentException("DoorNum Error");

            if (weekTimeGroup == null)
                throw new ArgumentException("WeekTimeGroup Is Null!");
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            weekTimeGroup = null;
        }

        /// <summary>
        /// 对门工作方式参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteByte(Door);
            databuf.WriteBoolean(Use);
            databuf.WriteByte(0);
            weekTimeGroup.GetBytes(databuf);

            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0xE3;
        }

        /// <summary>
        /// 对门工作方式参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (weekTimeGroup == null)
            {
                weekTimeGroup = new WeekTimeGroup(8);
            }
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            Door = databuf.ReadByte();
            Use = databuf.ReadBoolean();
            databuf.ReadByte();
            weekTimeGroup.ReadDoorWorkSetBytes(databuf);
        }
    }
}
