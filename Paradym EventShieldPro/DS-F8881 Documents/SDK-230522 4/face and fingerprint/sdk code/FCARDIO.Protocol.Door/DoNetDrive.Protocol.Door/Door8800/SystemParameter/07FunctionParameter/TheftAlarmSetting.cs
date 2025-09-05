namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 防盗报警参数_模型
    /// </summary>
    public class TheftAlarmSetting
    {
        /// <summary>
        /// 是否启用（0 - 不启用、1 - 启用）
        /// </summary>
        public bool Use { get; set; }

        /// <summary>
        /// 进入延迟；单位：秒，取值：1-255
        /// </summary>
        public int InTime { get; set; }

        /// <summary>
        /// 退出延迟；单位：秒，取值：1-255
        /// </summary>
        public int OutTime { get; set; }

        /// <summary>
        /// 布防密码；8个数字。空补F。例如密码：23412；表达为：0xFFF23412
        /// </summary>
        public string BeginPassword { get; set; }

        /// <summary>
        /// 撤防密码；8个数字。空补F。例如密码：23412；表达为：0xFFF23412
        /// </summary>
        public string ClosePassword { get; set; }

        /// <summary>
        /// 报警时长；单位：秒 ,取值：0-65535
        /// </summary>
        public ushort AlarmTime { get; set; }
    }
}