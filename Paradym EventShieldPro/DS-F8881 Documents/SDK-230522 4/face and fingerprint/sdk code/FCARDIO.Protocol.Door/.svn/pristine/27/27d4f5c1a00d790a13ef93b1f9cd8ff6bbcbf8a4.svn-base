using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.SystemStatus
{
    /// <summary>
    /// 获取设备运行信息_结果
    /// </summary>
    public class ReadSystemStatus_Result : INCommandResult
    {
        /// <summary>
        /// 系统运行天数
        /// </summary>
        public int RunDay;

        /// <summary>
        /// 格式化次数
        /// </summary>
        public int FormatCount;

        /// <summary>
        /// 看门狗复位次数
        /// </summary>
        public int RestartCount;

        /// <summary>
        /// UPS供电状态（0 - 表示电源取电；1 - 表示UPS供电）
        /// </summary>
        public int UPS;

        /// <summary>
        /// 系统温度
        /// </summary>
        public string Temperature;

        /// <summary>
        /// 上电时间
        /// </summary>
        public string StartTime;

        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage;

        /// <summary>
        /// 对设备运行信息进行解码
        /// </summary>
        /// <param name="databuf">包含设备运行信息结构的缓冲区</param>
        public void SetBytes(IByteBuffer databuf)
        {
            RunDay = databuf.ReadUnsignedShort();
            FormatCount = databuf.ReadUnsignedShort();
            RestartCount = databuf.ReadUnsignedShort();
            UPS = databuf.ReadByte();
            //设备温度正负状态（第一字节是正或负，0负，1正）
            int ZFState = databuf.ReadByte();
            //设备温度（第二字节的值）
            float ETemperature = databuf.ReadByte();
            Temperature = ETemperature.ToString() + "℃";
            if (ZFState == 0)
            {
                Temperature = "-" + Temperature;
            }

            //秒
            string second = databuf.ReadByte().ToString("X2");
            //分
            string branch = databuf.ReadByte().ToString("X2");
            //时
            string hour = databuf.ReadByte().ToString("X2");
            //日
            string day = databuf.ReadByte().ToString("X2");
            //月
            string month = databuf.ReadByte().ToString("X2");
            //周
            int week = databuf.ReadByte();
            string weekStr = string.Empty;
            if (week == 1)
            {
                weekStr = "星期一";
            }
            else if (week == 2)
            {
                weekStr = "星期二";
            }
            else if (week == 3)
            {
                weekStr = "星期三";
            }
            else if (week == 4)
            {
                weekStr = "星期四";
            }
            else if (week == 5)
            {
                weekStr = "星期五";
            }
            else if (week == 6)
            {
                weekStr = "星期六";
            }
            else if (week == 7)
            {
                weekStr = "星期日";
            }
            //年
            string year = "20" + databuf.ReadByte().ToString("X2");
            //拼接上电时间
            StartTime = $"{year}-{month}-{day} {hour}:{branch}:{second} ({weekStr})"; ;

            //电压第一节小数点前的值
            int SVoltage = databuf.ReadByte();
            //电压第二节小数点后的值
            int EVoltage = databuf.ReadByte();
            //拼接电压
            Voltage = $"{SVoltage}.{EVoltage}V";
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