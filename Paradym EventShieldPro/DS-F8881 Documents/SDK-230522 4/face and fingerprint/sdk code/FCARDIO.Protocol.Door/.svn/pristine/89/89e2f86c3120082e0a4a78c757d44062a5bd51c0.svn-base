using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 获取所有系统参数_结果
    /// </summary>
    public class ReadAllSystemSetting_Result : INCommandResult
    {
        /// <summary>
        /// 记录存储方式（0是满循环，1表示满不循环）
        /// </summary>
        public short RecordMode;

        /// <summary>
        /// 读卡器密码键盘启用功能开关（Bit0 - 1号读头、Bit1 - 2号读头、Bit2 - 3号读头、Bit3 - 4号读头、Bit4 - 5号读头、Bit5 - 6号读头、Bit6 - 7号读头、Bit7 - 8号读头）
        /// </summary>
        public short Keyboard;

        /// <summary>
        /// 互锁功能开关，4个门的互锁状态，各门端口的取值（0 - 不启用互锁功能、1 - 已启用互锁功能）
        /// </summary>
        public DoorPortDetail LockInteraction;

        /// <summary>
        /// 消防报警功能参数（0 - 不启用、1 - 报警输出，并开所有门，只能软件解除、2 - 报警输出，不开所有门，只能软件解除、3 - 有信号报警并开门，无信号解除报警并关门、4 - 有报警信号时开一次门，就像按钮开门一样）
        /// </summary>
        public short FireAlarmOption;

        /// <summary>
        /// 匪警报警功能参数（0 - 关闭此功能、1 - 所有门锁定，报警输出，蜂鸣器不响。不开门，刷卡不能解除，软件解除，解除报警后门的锁定也解锁了、2 - 报警输出，不锁定，蜂鸣器响。不开门，刷卡可以解除，软件可以解除、3 - 按报警按钮就报警，门锁定，并输出，不按时就停止。不开门，按钮停止时就解除，软件或刷卡不能解除。按报警按钮的时候门是处于锁定状态的，不按时解除锁定状态）
        /// </summary>
        public short OpenAlarmOption;

        /// <summary>
        /// 读卡间隔时间（0表示无限制，最大65535秒）
        /// </summary>
        public int ReaderIntervalTime;

        /// <summary>
        /// 语音播报开关（语音段对照可参考《Door8800语音表》 每个开关true 表示启用，false 表示禁用）
        /// </summary>
        public BroadcastDetail SpeakOpen;

        /// <summary>
        /// 读卡器校验（0不启用，1启用，2启用校验，但不提示非法数据或线路异常）
        /// </summary>
        public short ReaderCheckMode;

        /// <summary>
        /// 主板蜂鸣器（0不启用，1启用）
        /// </summary>
        public short BuzzerMode;

        /// <summary>
        /// 烟雾报警功能参数（0 - 关闭此功能（默认）、1 - 驱动 [烟雾报警继电器]，(信号有，就驱动的，信号无，就关闭)、2 - 驱动烟雾报警继电器并驱动所有门继电器，主板报警提示音响(开启后由软件关闭，或重启。)、3 - 驱动烟雾报警继电器并锁定所有门，主板报警提示音响(开启后由软件关闭，或重启。)）
        /// </summary>
        public short SmogAlarmOption;

        /// <summary>
        /// 门内人数限制，上限值：0--表示不受限制，全局上限优先级最高，全局上限如果大于 0 则设备使用全局上限   例如：全局上限为100,1门上限为50,2门上限为100,。。。。4门上限为1000，设备将使用全局上限100，即整个主板上进入数不能超过100，此数据重启后清空
        /// </summary>
        public DoorLimit EnterDoorLimit;

        /// <summary>
        /// 防盗主机
        /// </summary>
        public TheftAlarmSetting TheftAlarmPar;

        /// <summary>
        /// 防潜回功能参数（01--单独每个门检测防潜回；02--整个控制器统一防潜回）
        /// </summary>
        public short CheckInOut;

        /// <summary>
        /// 卡片到期提示（0不启用，1启用）
        /// </summary>
        public short CardPeriodSpeak;

        /// <summary>
        /// 定时播报
        /// </summary>
        public ReadCardSpeak ReadCardSpeak;

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
        /// <param name="_SmogAlarmOption">烟雾报警功能参数</param>
        /// <param name="_EnterDoorLimit">门内人数限制</param>
        /// <param name="_TheftAlarmPar">防盗主机</param>
        /// <param name="_CheckInOut">防潜回功能参数</param>
        /// <param name="_CardPeriodSpeak">卡片到期提示</param>
        /// <param name="_ReadCardSpeak">定时播报</param>
        public ReadAllSystemSetting_Result(short _RecordMode, short _Keyboard, DoorPortDetail _LockInteraction, short _FireAlarmOption, short _OpenAlarmOption, int _ReaderIntervalTime, BroadcastDetail _SpeakOpen, short _ReaderCheckMode, short _BuzzerMode, short _SmogAlarmOption, DoorLimit _EnterDoorLimit, TheftAlarmSetting _TheftAlarmPar, short _CheckInOut, short _CardPeriodSpeak, ReadCardSpeak _ReadCardSpeak)
        {
            RecordMode = _RecordMode;
            Keyboard = _Keyboard;
            LockInteraction = _LockInteraction;
            FireAlarmOption = _FireAlarmOption;
            OpenAlarmOption = _OpenAlarmOption;
            ReaderIntervalTime = _ReaderIntervalTime;
            SpeakOpen = _SpeakOpen;
            ReaderCheckMode = _ReaderCheckMode;
            BuzzerMode = _BuzzerMode;
            SmogAlarmOption = _SmogAlarmOption;
            EnterDoorLimit = _EnterDoorLimit;
            TheftAlarmPar = _TheftAlarmPar;
            CheckInOut = _CheckInOut;
            CardPeriodSpeak = _CardPeriodSpeak;
            ReadCardSpeak = _ReadCardSpeak;
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