using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Door.OvertimeAlarmSetting
{
    /// <summary>
    /// 设置开门超时提示参数
    /// </summary>
    public class WriteOvertimeAlarmSetting_Parameter
        : AbstractParameter
    {
        /// <summary>
        /// 门号
        /// 门端口在控制板中的索引号，取值：1-4
        /// </summary>
        public int DoorNum;

        /// <summary>
        /// 是否启用功能
        /// 0表示关闭，1表示开启
        /// </summary>
        public bool Use;

        /// <summary>
        /// 超时时间，指门磁一直打开的时间。
        /// 取值范围：0-65535,0表示关闭；单位秒；
        /// </summary>
        public int Overtime;

        /// <summary>
        /// 继电器输出
        /// 0--不输出继电器；1--输出继电器(匪警继电器)。
        /// </summary>
        public bool Alarm;

        /// <summary>
        /// 提供给 OvertimeAlarmSetting_Result 使用
        /// </summary>
        public WriteOvertimeAlarmSetting_Parameter()
        {
        }
        
        /// <summary>
        /// 创建结构,并传入门号和是否开启此功能
        /// </summary>
        /// <param name="door">门号</param>
        /// <param name="use">是否启动此功能</param>
        /// <param name="overtime">超出时间</param>
        /// <param name="alarm">超出后,是否开启此功能</param>
        public WriteOvertimeAlarmSetting_Parameter(byte door, bool use, int overtime, bool alarm)
        {
            DoorNum = door;
            Use = use;
            Overtime = overtime;
            Alarm = alarm;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DoorNum < 1 || DoorNum > 4)
                throw new ArithmeticException("DoorNum Is Error");
            if (Overtime < 0 || Overtime > 65535)
            {
                throw new ArithmeticException("Overtime Is Error");
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
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != 5)
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteByte(DoorNum);
            databuf.WriteBoolean(Use);
            databuf.WriteShort(Overtime);
            databuf.WriteBoolean(Alarm);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 5;
        }
    
        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            DoorNum = databuf.ReadByte();
            Use = databuf.ReadBoolean();
            Overtime = databuf.ReadUnsignedShort();
            Alarm = databuf.ReadBoolean();
        }
    }
}
