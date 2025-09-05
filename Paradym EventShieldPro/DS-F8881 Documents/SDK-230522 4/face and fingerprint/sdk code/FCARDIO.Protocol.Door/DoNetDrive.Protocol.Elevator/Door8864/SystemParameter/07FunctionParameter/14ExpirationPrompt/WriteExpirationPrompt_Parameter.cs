using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter.ExpirationPrompt
{
    /// <summary>
    /// 卡片到期提示参数
    /// </summary>
    public class WriteExpirationPrompt_Parameter : Protocol.Door.Door8800.SystemParameter.FunctionParameter.WriteCardPeriodSpeak_Parameter
    {
        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteExpirationPrompt_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="isUse">是否启用</param>
        public WriteExpirationPrompt_Parameter(byte _Use)
        {
            Use = _Use;
        }

    }
}
