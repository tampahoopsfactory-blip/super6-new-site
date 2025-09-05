using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.SystemStatus
{
    /// <summary>
    /// 获取设备运行信息 返回结果
    /// </summary>
    public class ReadSystemStatus_Result : INCommandResult
    {
        /// <summary>
        /// 出厂年月日
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// 格式化次数
        /// </summary>
        public ushort FormatCount;

        /// <summary>
        /// 开机次数
        /// </summary>
        public ushort StartCount;

        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage;

        /// <summary>
        /// 电量
        /// </summary>
        public byte Electricity;

        /// <summary>
        /// 当前已选巡更人员
        /// </summary>
        public ushort PatrolEmpl;

        /// <summary>
        /// 巡更员数量
        /// </summary>
        public ushort PatrolEmplCount;

        /// <summary>
        /// 读卡记录新记录数量
        /// </summary>
        public uint CardRecordCount;

        /// <summary>
        /// 系统记录新记录数量
        /// </summary>
        public uint SystemRecordCount;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }


        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            Time = DoNetDrive.Protocol.Util.TimeUtil.BCDTimeToDate_yyMMdd(databuf);
            FormatCount = databuf.ReadUnsignedShort();
            StartCount = databuf.ReadUnsignedShort();
            Voltage = string.Concat(databuf.ReadByte().ToString("X"),".", databuf.ReadByte().ToString("X"));
            Electricity = databuf.ReadByte();
            PatrolEmpl = databuf.ReadUnsignedShort();
            PatrolEmplCount = databuf.ReadUnsignedShort();
            CardRecordCount = databuf.ReadUnsignedInt();
            SystemRecordCount = databuf.ReadUnsignedInt();
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public IByteBuffer GetBytes(IByteBuffer databuf)
        {
            
            return databuf;
        }
    }
}
