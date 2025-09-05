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
    /// 控制器各端口工作状态信息_结果
    /// </summary>
    public class ReadWorkStatus_Result : INCommandResult
    {
        /// <summary>
        /// 门继电器物理状态（0 - 表示COM和NC常闭、1 - 表示COM和NO常闭、2 - 表示继电器无法检测，继电器异常）
        /// </summary>
        public DoorPortDetail RelayState;

        /// <summary>
        /// 运行状态（常开还是常闭，0表示常闭，1常开）
        /// </summary>
        public DoorPortDetail DoorLongOpenState;

        /// <summary>
        /// 门磁开关（0表关，1表示开）
        /// </summary>
        public DoorPortDetail DoorState;

        /// <summary>
        /// 门报警状态（0 - 非法刷卡报警、1 - 门磁报警、2 - 胁迫报警、3 - 开门超时报警、4 - 黑名单报警）
        /// </summary>
        public DoorPortDetail DoorAlarmState;

        /// <summary>
        /// 设备报警状态（0 - 匪警报警、1 - 防盗报警、2 - 消防报警、3 - 烟雾报警、4 - 消防报警(命令通知)）
        /// </summary>
        public byte AlarmState;

        /// <summary>
        /// 继电器逻辑状态.
        /// 继电器的逻辑开关状态
        /// 
        /// 0--继电器关；
        /// 1--继电器开；
        /// 2--双稳态；  
        /// 
        /// 门序号值说明
        /// 1-4是表示门的继电器，这个继电器状态需要根据门的继电器类型判断真实物理状况或者根据第一组状态值【门继电器物理状态】判断。<br/>
        /// 5-8是报警继电器，目前定义只有0或1两个状态。
        /// 状态0表示：COM和NC导通
        /// 状态1表示：COM和NO导通
        /// 
        /// 1-4  4个门的继电器
        /// 5   消防继电器
        /// 6   匪警继电器
        /// 7   烟雾报警继电器
        /// 8   防盗主机报警继电器
        /// 
        /// </summary>
        public DoorPortDetail LockState;

        /// <summary>
        /// 锁定状态.<br/>
        /// 4个门，0--未锁定，1--锁定
        /// </summary>
        public DoorPortDetail PortLockState;

        /// <summary>
        /// 监控状态.<br/>
        /// 0--未开启监控；1--开启监控
        /// </summary>
        public byte WatchState;

        /// <summary>
        /// 门内人数
        /// </summary>
        public DoorLimit EnterTotal;

        /// <summary>
        /// 防盗主机布防状态<br/>
        /// 
        /// 1   延时布防
        /// 2   已布防
        /// 3   延时撤防
        /// 4   未布防
        /// 5   报警延时，准备启用报警
        /// 6   防盗报警已启动
        /// 
        /// </summary>
        public byte TheftState;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            RelayState = null;
            DoorLongOpenState = null;
            DoorState = null;
            DoorAlarmState = null;
            LockState = null;
            PortLockState = null;
            EnterTotal = null;

            return;
        }

        /// <summary>
        /// 对控制器各端口工作状态信息进行解码
        /// </summary>
        /// <param name="databuf">包含控制器各端口工作状态信息结构的缓冲区</param>
        public void SetBytes(IByteBuffer databuf)
        {
            //最大门数
            ushort MaxDoorNum = 4;
            //门继电器物理状态
            if (RelayState == null)
            {
                RelayState = new DoorPortDetail(MaxDoorNum);
            }
            for (int i = 0; i < RelayState.DoorMax; i++)
            {
                RelayState.DoorPort[i] = databuf.ReadByte();
            }
            //运行状态
            if (DoorLongOpenState == null)
            {
                DoorLongOpenState = new DoorPortDetail(MaxDoorNum);
            }
            for (int i = 0; i < DoorLongOpenState.DoorMax; i++)
            {
                DoorLongOpenState.DoorPort[i] = databuf.ReadByte();
            }
            //门磁开关
            if (DoorState == null)
            {
                DoorState = new DoorPortDetail(MaxDoorNum);
            }
            for (int i = 0; i < DoorState.DoorMax; i++)
            {
                DoorState.DoorPort[i] = databuf.ReadByte();
            }
            //门报警状态
            if (DoorAlarmState == null)
            {
                DoorAlarmState = new DoorPortDetail(MaxDoorNum);
            }
            for (int i = 0; i < DoorAlarmState.DoorMax; i++)
            {
                DoorAlarmState.DoorPort[i] = databuf.ReadByte();
            }
            //设备报警状态
            AlarmState = databuf.ReadByte();
            //继电器逻辑状态
            if (LockState == null)
            {
                LockState = new DoorPortDetail(8);
            }
            for (int i = 0; i < LockState.DoorMax; i++)
            {
                LockState.DoorPort[i] = databuf.ReadByte();
            }
            //锁定状态
            if (PortLockState == null)
            {
                PortLockState = new DoorPortDetail(MaxDoorNum);
            }
            for (int i = 0; i < PortLockState.DoorMax; i++)
            {
                PortLockState.DoorPort[i] = databuf.ReadByte();
            }
            //监控状态
            WatchState = databuf.ReadByte();
            //门内人数
            if (EnterTotal == null)
            {
                EnterTotal = new DoorLimit();
            }
            EnterTotal.GlobalEnter = databuf.ReadUnsignedInt(); //全局人数
            for (int j = 0; j < MaxDoorNum; j++) //1-4门人数
            {
                EnterTotal.DoorEnterArray[j] = databuf.ReadUnsignedInt();
            }
            //防盗主机布防状态
            TheftState = databuf.ReadByte();
        }
    }
}