using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter.ReadAllSystemSetting
{
    /// <summary>
    /// 读取所有功能参数
    /// </summary>
    public class ReadAllSystemSetting_Result : INCommandResult
    {
        /// <summary>
        /// 记录存储方式（0是满循环，1表示满不循环）
        /// </summary>
        public byte RecordMode;

        /// <summary>
        /// 读卡器密码键盘启用功能开关（Bit0 - 1号读头、Bit1 - 2号读头、Bit2 - 3号读头、Bit3 - 4号读头、Bit4 - 5号读头、Bit5 - 6号读头、Bit6 - 7号读头、Bit7 - 8号读头）
        /// </summary>
        public byte Keyboard;

        /// <summary>
        /// 键盘发卡功能开关
        /// </summary>
        public bool KeyboardCard;

        /// <summary>
        /// 主板管理密码
        /// </summary>
        public string Password;


        
        /// <summary>
        /// 读卡间隔时间（0表示无限制，最大65535秒）
        /// </summary>
        public int ReaderIntervalTime;

        /// <summary>
        /// 语音播报开关（语音段对照可参考《Door8800语音表》 每个开关true 表示启用，false 表示禁用）
        /// </summary>
        public BroadcastDetail Broadcast;

        /// <summary>
        /// 读卡间隔是否启用（0不启用，1启用，2启用校验，但不提示非法数据或线路异常）
        /// </summary>
        public bool ReaderIntervalTimeUse;

        /// <summary>
        /// 读卡间隔时间，最大65535秒，0表示无限制
        /// </summary>
        public ushort IntervalTime;

        /// <summary>
        /// 检测模式
        /// 1 - 记录读卡，不开门，有提示
        /// 2 - 不记录读卡，不开门，有提示
        /// 3 - 不做响应，无提示
        /// </summary>
        public byte ReaderMode;

        /// <summary>
        /// 读卡器数据校验是否启用（0不启用，1启用，2启用校验，但不提示非法数据或线路异常）
        /// </summary>
        public bool ReaderCheckMode;

        /// <summary>
        /// 主板蜂鸣器（0不启用，1启用）
        /// </summary>
        public bool BuzzerMode;

        /// <summary>
        /// 读卡器参数 （1 - 韦根26(三字节)，2 - 韦根34(四字节)，3 - 韦根26(二字节)，4 - 禁用）
        /// </summary>
        public byte ReaderByte;

        /// <summary>
        /// 读卡认证方式
        /// </summary>
        public WeekTimeGroup_ReaderWork ReaderWorkSetting;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool InvalidCardAlarm;

        /// <summary>
        /// 胁迫报警是否启用
        /// </summary>
        public bool AlarmPasswordIsUse;

        /// <summary>
        /// 胁迫报警胁迫密码
        /// </summary>
        public string AlarmPassword;

        /// <summary>
        /// 报警选项
        /// 1   不开门，报警输出
        /// 2   开门，报警输出
        /// 3   锁定门，报警，只能软件解锁
        /// </summary>
        public byte AlarmOption;

        /// <summary>
        /// 卡片到期提示
        /// </summary>
        public bool ExpirationPromptIsUse;

        /// <summary>
        /// 定时读卡播报语音消息
        /// </summary>
        public ReadCardSpeak ReadCardSpeak;

        public ReadAllSystemSetting_Result()
        {

        }

        public void SetBytes(IByteBuffer buf)
        {
            RecordMode = buf.ReadByte();
            Keyboard = buf.ReadByte();
            KeyboardCard = buf.ReadBoolean();
            Password = StringUtil.ByteBufToHex(buf, 4);
            if (Broadcast == null)
            {
                Broadcast = new BroadcastDetail();
            }
            for (int i = 0; i < 10; i++)
            {
                Broadcast.Broadcast[i] = buf.ReadByte();
            }
            ReaderIntervalTimeUse = buf.ReadBoolean();
            IntervalTime = buf.ReadUnsignedShort();
        }


        /*
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="_RecordMode">记录存储方式</param>
        /// <param name="_Keyboard">读卡器密码键盘启用功能开关</param>
        /// <param name="_LockInteraction">互锁功能开关</param>
        /// <param name="_FireAlarmOption">消防报警功能参数</param>
        /// <param name="_OpenAlarmOption">匪警报警功能参数</param>
        /// <param name="_ReaderIntervalTime">读卡间隔时间</param>
        /// <param name="_SpeakOpen">语音播报开关</param>
        /// <param name="_ReaderCheckMode">读卡器校验</param>
        /// <param name="_BuzzerMode">主板蜂鸣器</param>
        /// <param name="_TheftAlarmPar">防盗主机</param>
        /// <param name="_CheckInOut">防潜回功能参数</param>
        /// <param name="_CardPeriodSpeak">卡片到期提示</param>
        /// <param name="_ReadCardSpeak">定时播报</param>
        public ReadAllSystemSetting_Result(byte _RecordMode, byte _Keyboard, byte _FireAlarmOption, byte _OpenAlarmOption, int _ReaderIntervalTime, BroadcastDetail _SpeakOpen, bool _ReaderCheckMode, bool _BuzzerMode, TheftAlarmSetting _TheftAlarmPar, short _CheckInOut, short _CardPeriodSpeak, ReadCardSpeak _ReadCardSpeak)
        {
            RecordMode = _RecordMode;
            Keyboard = _Keyboard;
            FireAlarmOption = _FireAlarmOption;
            OpenAlarmOption = _OpenAlarmOption;
            ReaderIntervalTime = _ReaderIntervalTime;
            SpeakOpen = _SpeakOpen;
            ReaderCheckMode = _ReaderCheckMode;
            BuzzerMode = _BuzzerMode;
            TheftAlarmPar = _TheftAlarmPar;
            CheckInOut = _CheckInOut;
            CardPeriodSpeak = _CardPeriodSpeak;
            ReadCardSpeak = _ReadCardSpeak;
        }
        */
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            return;
        }
    }
}