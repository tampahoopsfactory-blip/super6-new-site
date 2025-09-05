using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;

namespace DoNetDrive.Protocol.Door.Door8800.Door.DoorWorkSetting
{
    /// <summary>
    /// 门工作方式_参数
    /// </summary>
    public class WriteDoorWorkSetting_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门
        /// </summary>
        public byte Door;

        /// <summary>
        /// 功能是否启用
        /// </summary>
        public byte Use;

        /// <summary>
        /// 开门方式
        /// 1 - 普通
        /// 2 - 多卡
        /// 3 - 首卡（时段）
        /// 4 - 常开（时段）
        /// </summary>
        public byte OpenDoorWay;

        /// <summary>
        /// 门常开触发模式
        /// 1 - 合法卡在时段内即可常开
        /// 2 - 授权中标记为常开卡的在指定时段内刷卡即可常开
        /// 3 - 自动开关，到时间自动开关门
        /// </summary>
        public byte DoorTriggerMode;

        /// <summary>
        /// 保留值
        /// </summary>
        public byte RetainValue;

        /// <summary>
        /// 门工作方式时段
        /// </summary>
        public WeekTimeGroup weekTimeGroup;

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
            if (Door < 1 || Door > 4)
                throw new ArgumentException("DoorNum Error");

            if (DoorTriggerMode < 1 || DoorTriggerMode > 3)
                throw new ArgumentException("DoorTriggerMode Error");

            if (OpenDoorWay < 1 || OpenDoorWay > 4)
                throw new ArgumentException("OpenDoorWay Error");


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
            databuf.WriteByte(Use);
            databuf.WriteByte(OpenDoorWay);
            databuf.WriteByte(DoorTriggerMode);
            databuf.WriteByte(RetainValue);
            weekTimeGroup.GetBytes(databuf);

            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0xE5;
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
            Use = databuf.ReadByte();
            OpenDoorWay = databuf.ReadByte();
            DoorTriggerMode = databuf.ReadByte();
            RetainValue = databuf.ReadByte();
            weekTimeGroup.SetBytes(databuf);
        }
    }
}
