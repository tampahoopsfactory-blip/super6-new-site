using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Util;
using System;

namespace DoNetDrive.Protocol.POS.SystemParameter.SystemStatus
{
    public class ReadSystemStatus_Result : INCommandResult
    {
        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage;

        /// <summary>
        /// 电量
        /// </summary>
        public byte Electricity;

        /// <summary>
        /// CPU温度
        /// </summary>
        public string Temperature;

        /// <summary>
        /// 供电方式
        /// 1 - 市电
        /// 2 - 电池供电
        /// </summary>
        public byte PowerSupplyMode;

        /// <summary>
        /// 监控状态
        /// </summary>
        public byte WatchState;

        /// <summary>
        /// 本次开机日期
        /// </summary>
        public DateTime StartDate;

        /// <summary>
        /// 本次开机时间
        /// </summary>
        public string StartTime;

        /// <summary>
        /// 今日营业金额
        /// </summary>
        public int Turnover;

        /// <summary>
        /// 今日消费次数
        /// </summary>
        public int ConsumeCount;

        /// <summary>
        /// 各定额段统计
        /// </summary>
        public byte[] FixedFee;

        /// <summary>
        /// 继电器状态
        /// 1--COM&NO；2--COM&NC；0--禁用
        /// </summary>
        public byte Relay;

        /// <summary>
        /// 当前语言环境
        /// 1 - 简
        /// 2 - 繁
        /// 3 - 英文
        /// </summary>
        public byte Language;

        /// <summary>
        /// 菜单锁定状态 0/1
        /// </summary>
        public byte MenuLock;

        /// <summary>
        /// 工作模式
        /// </summary>
        public byte WorkMode;

        /// <summary>
        /// 对设备运行信息进行解码
        /// </summary>
        /// <param name="databuf">包含设备运行信息结构的缓冲区</param>
        public void SetBytes(IByteBuffer databuf)
        {
            Voltage = string.Concat(databuf.ReadByte().ToString("X"), ".", databuf.ReadByte().ToString("X"));
            float ETemperature = databuf.ReadByte();
            Temperature = ETemperature.ToString() + "℃";
            Electricity = databuf.ReadByte();

            PowerSupplyMode = databuf.ReadByte();
            WatchState = databuf.ReadByte();
            StartDate = TimeUtil.BCDTimeToDate_yyMMddhhmmss(databuf);
            Turnover = databuf.ReadInt();
            ConsumeCount = databuf.ReadInt();

            //各定额段统计
            FixedFee = new byte[64];
            databuf.ReadBytes(FixedFee, 0,64);
            Relay = databuf.ReadByte();
            Language = databuf.ReadByte();
            MenuLock = databuf.ReadByte();
            WorkMode = databuf.ReadByte();

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            return;
        }
    }
}
