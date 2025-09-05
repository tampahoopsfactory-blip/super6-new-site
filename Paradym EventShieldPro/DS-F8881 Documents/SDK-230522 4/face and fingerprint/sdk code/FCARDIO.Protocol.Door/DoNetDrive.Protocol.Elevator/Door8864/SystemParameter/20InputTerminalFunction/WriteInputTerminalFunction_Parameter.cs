using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.InputTerminalFunction
{
    /// <summary>
    /// 输入端子功能定义 参数
    /// </summary>
    public class WriteInputTerminalFunction_Parameter : AbstractParameter
    {
        /// <summary>
        /// 功能定义：
        /// 01--开锁按钮（默认
        /// 02--门磁检查
        /// </summary>
        public int Function;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteInputTerminalFunction_Parameter() { }

        /// <summary>
        /// 使用主板蜂鸣器初始化实例
        /// </summary>
        /// <param name="function">功能定义</param>
        public WriteInputTerminalFunction_Parameter(int function)
        {
            Function = function;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Function < 1 || Function > 2)
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
            return;
        }

        /// <summary>
        /// 对主板蜂鸣器参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(Function);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x01;
        }

        /// <summary>
        /// 对主板蜂鸣器参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Function = databuf.ReadByte();
        }
    }
}
