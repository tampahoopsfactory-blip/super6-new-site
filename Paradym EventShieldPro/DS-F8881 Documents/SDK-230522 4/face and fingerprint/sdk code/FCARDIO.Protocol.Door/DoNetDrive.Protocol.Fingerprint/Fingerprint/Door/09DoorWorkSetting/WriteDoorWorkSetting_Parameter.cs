using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Door.DoorWorkSetting
{
    /// <summary>
    /// 定时常开_参数
    /// </summary>
    public class WriteDoorWorkSetting_Parameter : AbstractParameter
    {

        /// <summary>
        /// 功能是否启用
        /// </summary>
        public bool Use;


        /// <summary>
        /// 门常开触发模式
        /// 1 - 合法卡在时段内即可常开
        /// 2 - 授权中标记为常开卡的在指定时段内刷卡即可常开
        /// 3 - 自动开关，到时间自动开关门
        /// </summary>
        public byte DoorTriggerMode;


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
        /// 门初始化实例
        /// </summary>
        /// <param name="use">功能是否启用</param>
        /// <param name="doorTriggerMode">门常开触发模式</param>
        /// <param name="tg">门工作方式时段</param>
        public WriteDoorWorkSetting_Parameter(bool use, byte doorTriggerMode, WeekTimeGroup tg)
        {
            Use = use;
            DoorTriggerMode = doorTriggerMode;
            weekTimeGroup = tg;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            

            if (DoorTriggerMode < 1 || DoorTriggerMode > 3)
                throw new ArgumentException("DoorTriggerMode Error");

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
        /// 对参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteBoolean(Use);
            databuf.WriteByte(DoorTriggerMode);
            weekTimeGroup.GetBytes(databuf);

            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0xE2;
        }

        /// <summary>
        /// 对参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override void SetBytes(IByteBuffer databuf)
        {
            
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            if (weekTimeGroup == null)
            {
                weekTimeGroup = new WeekTimeGroup(8);
            }
            Use = databuf.ReadBoolean();
            DoorTriggerMode = databuf.ReadByte();
            weekTimeGroup.SetBytes(databuf);
        }
    }
}
