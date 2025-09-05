using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.GateMagneticAlarm
{
    /// <summary>
    /// 设置门磁报警参数
    /// </summary>
    public class WriteGateMagneticAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 不启用门磁报警检查的时间段
        /// </summary>
        public WeekTimeGroup WeekTimeGroup;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteGateMagneticAlarm_Parameter()
        {
            WeekTimeGroup = new WeekTimeGroup(8);
        }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="doorNumList"></param>
        public WriteGateMagneticAlarm_Parameter(bool isUse, WeekTimeGroup weekTimeGroup)
        {
            IsUse = isUse;

            WeekTimeGroup = weekTimeGroup;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (WeekTimeGroup == null)
            {
                return false;
            }
            for (int i = 0; i < 7; i++)
            {
                var dayTimeGroup = WeekTimeGroup.GetItem(i);
                int count = dayTimeGroup.GetSegmentCount();
                for (int j = 0; j < count; j++)
                {
                    var ts = dayTimeGroup.GetItem(j);
                    if (ts.GetBeginTime() > ts.GetEndTime())
                    {
                        return false;
                    }
                }
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
            databuf.WriteBoolean(IsUse);
            WeekTimeGroup.GetBytes(databuf);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {

            return 0xE1;
        }

        /// <summary>
        /// 对误差自修正参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsUse = databuf.ReadBoolean();
            WeekTimeGroup.SetBytes(databuf);
        }
    }
}
