using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.OpenDoorTimeoutAlarm
{
    /// <summary>
    /// 设置开门超时报警参数
    /// </summary>
    public class WriteOpenDoorTimeoutAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 开门超时报警参数 集合
        /// </summary>
        public Data.OpenDoorTimeoutAlarm[] OpenDoorTimeoutAlarmList;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteOpenDoorTimeoutAlarm_Parameter()
        {

        }
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="list">开门超时报警参数</param>
        public WriteOpenDoorTimeoutAlarm_Parameter(Data.OpenDoorTimeoutAlarm[] list)
        {
            OpenDoorTimeoutAlarmList = list;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (OpenDoorTimeoutAlarmList == null || OpenDoorTimeoutAlarmList.Length != 64)
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

            foreach (var item in OpenDoorTimeoutAlarmList)
            {
                databuf.WriteUnsignedShort(item.AllowTime);
                databuf.WriteBoolean(item.IsUse);
            }

            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {

            return 0xC0;
        }

        /// <summary>
        /// 对误差自修正参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            OpenDoorTimeoutAlarmList = new  Data.OpenDoorTimeoutAlarm[64];
            for (int i = 0; i < 64; i++)
            {
                Data.OpenDoorTimeoutAlarm model = new Data.OpenDoorTimeoutAlarm();
                model.AllowTime = databuf.ReadUnsignedShort();
                model.IsUse = databuf.ReadBoolean();
                OpenDoorTimeoutAlarmList[i] = model;
            }

        }
    }
}
