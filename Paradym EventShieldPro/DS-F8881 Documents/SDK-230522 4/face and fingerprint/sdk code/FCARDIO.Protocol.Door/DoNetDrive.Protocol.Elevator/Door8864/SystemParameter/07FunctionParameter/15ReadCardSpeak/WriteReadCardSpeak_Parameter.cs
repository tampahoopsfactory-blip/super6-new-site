using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置定时读卡播报语音消息参数_参数
    /// </summary>
    public class WriteReadCardSpeak_Parameter : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteReadCardSpeak_Parameter
    {
        public WriteReadCardSpeak_Parameter()
        {

        }
        public WriteReadCardSpeak_Parameter(ReadCardSpeak _SpeakSetting)
        {
            SpeakSetting = _SpeakSetting;
        }
    }
}