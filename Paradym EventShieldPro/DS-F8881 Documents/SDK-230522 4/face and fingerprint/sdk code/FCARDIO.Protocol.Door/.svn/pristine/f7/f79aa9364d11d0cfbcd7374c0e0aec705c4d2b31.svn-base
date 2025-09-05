using DotNetty.Buffers;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.SystemStatus
{
    /// <summary>
    /// 获取设备运行信息_结果
    /// </summary>
    public class ReadSystemRunStatus_Result : INCommandResult
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
        /// 上电时间
        /// </summary>
        public string StartTime;

        /// <summary>
        /// 返回值内容
        /// </summary>
        public byte[] ResultContent;

        /// <summary>
        /// 对设备运行信息进行解码
        /// </summary>
        /// <param name="databuf">包含设备运行信息结构的缓冲区</param>
        public void SetBytes(IByteBuffer databuf)
        {
            int iReadIndex = databuf.ReaderIndex;
            ResultContent = new byte[databuf.ReadableBytes];
            databuf.ReadBytes(ResultContent);
            databuf.SetReaderIndex(iReadIndex);

            RunDay = databuf.ReadUnsignedShort();
            FormatCount = databuf.ReadUnsignedShort();
            RestartCount = databuf.ReadUnsignedShort();


            //DoNetDrive.Protocol.Util.TimeUtil.BCDTimeToDate_ssmmhhddMMyy();
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
            
            //年
            string year = "20" + databuf.ReadByte().ToString("X2");
            //拼接上电时间
            StartTime = $"{year}-{month}-{day} {hour}:{branch}:{second}"; ;

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
