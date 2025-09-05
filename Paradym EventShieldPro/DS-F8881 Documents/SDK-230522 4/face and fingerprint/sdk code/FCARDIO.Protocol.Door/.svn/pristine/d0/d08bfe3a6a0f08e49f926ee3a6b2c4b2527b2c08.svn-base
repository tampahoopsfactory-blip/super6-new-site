using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.DoorWorkSetting
{
    /// <summary>
    /// 门工作方式_参数
    /// </summary>
    public class WriteDoorWorkSetting_Parameter : Protocol.Door.Door8800.Door.DoorWorkSetting.WriteDoorWorkSetting_Parameter
    {
        /// <summary>
        /// 门 1 - 65
        /// </summary>
        //public byte Door;

        

        /// <summary>
        /// 提供给 DoorWorkSetting_Result 使用
        /// </summary>
        public WriteDoorWorkSetting_Parameter() {
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
        public WriteDoorWorkSetting_Parameter(byte door, byte use, byte openDoorWay, byte doorTriggerMode, byte retainValue, WeekTimeGroup tg)
        {
            Door = door;
            Use = use;
            OpenDoorWay = openDoorWay;
            DoorTriggerMode = doorTriggerMode;
            RetainValue = retainValue;
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

            if (DoorTriggerMode < 1 || DoorTriggerMode > 3)
                throw new ArgumentException("DoorTriggerMode Error");

            if (OpenDoorWay != 1 && OpenDoorWay != 3 && OpenDoorWay != 4)
                throw new ArgumentException("OpenDoorWay Error");


            if (weekTimeGroup == null)
                throw new ArgumentException("WeekTimeGroup Is Null!");
            return true;
        }

        
    }
}
