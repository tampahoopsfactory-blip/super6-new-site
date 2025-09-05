using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置定时读卡播报语音消息参数_参数
    /// </summary>
    public class WriteReadCardSpeak_Parameter : AbstractParameter
    {
        /// <summary>
        /// 定时读卡播报语音消息参数
        /// </summary>
        public ReadCardSpeak SpeakSetting;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteReadCardSpeak_Parameter() { }

        /// <summary>
        /// 使用定时读卡播报语音消息参数初始化实例
        /// </summary>
        /// <param name="_SpeakSetting">定时读卡播报语音消息参数</param>
        public WriteReadCardSpeak_Parameter(ReadCardSpeak _SpeakSetting)
        {
            SpeakSetting = _SpeakSetting;
            if (!checkedParameter())
            {
                throw new ArgumentException("SpeakSetting Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (SpeakSetting == null)
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
            SpeakSetting = null;
            return;
        }

        /// <summary>
        /// 对定时读卡播报语音消息参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteBoolean(SpeakSetting.Use);
            databuf.WriteByte(SpeakSetting.MsgIndex);
            byte[] btData = new byte[4];

            Utility.StringUtility.DateToBCD_yyMMddhh(btData, SpeakSetting.BeginDate);
            databuf.WriteBytes(btData);

            Utility.StringUtility.DateToBCD_yyMMddhh(btData, SpeakSetting.EndDate);
            databuf.WriteBytes(btData);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x0A;
        }

        /// <summary>
        /// 对定时读卡播报语音消息参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (SpeakSetting == null)
            {
                SpeakSetting = new ReadCardSpeak();
            }
            SpeakSetting.Use = databuf.ReadBoolean();
            SpeakSetting.MsgIndex = databuf.ReadByte();

            byte[] btData = new byte[4];
            databuf.ReadBytes(btData, 0, 4);
            SpeakSetting.BeginDate = Utility.StringUtility.BCDTimeToDate_yyMMddhh(btData);

            databuf.ReadBytes(btData, 0, 4);
            SpeakSetting.EndDate = Utility.StringUtility.BCDTimeToDate_yyMMddhh(btData);
            return;
        }

    }
}