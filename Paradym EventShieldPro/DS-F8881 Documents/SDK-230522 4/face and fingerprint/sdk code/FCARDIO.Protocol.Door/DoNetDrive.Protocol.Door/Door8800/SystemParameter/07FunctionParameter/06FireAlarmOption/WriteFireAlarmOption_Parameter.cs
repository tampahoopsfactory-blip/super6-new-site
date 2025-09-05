using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置消防报警参数_参数
    /// </summary>
    public class WriteFireAlarmOption_Parameter : AbstractParameter
    {
        /// <summary>
        /// 消防报警参数（0 - 不启用、1 - 报警输出，并开所有门，只能软件解除、2 - 报警输出，不开所有门，只能软件解除、3 - 有信号报警并开门，无信号解除报警并关门、4 - 有报警信号时开一次门，就像按钮开门一样）
        /// </summary>
        public byte Option;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteFireAlarmOption_Parameter() { }

        /// <summary>
        /// 使用消防报警参数初始化实例
        /// </summary>
        /// <param name="_Option">消防报警参数</param>
        public WriteFireAlarmOption_Parameter(byte _Option)
        {
            Option = _Option;
            if (!checkedParameter())
            {
                throw new ArgumentException("Option Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Option != 0 && Option != 1 && Option != 2 && Option != 3 && Option != 4)
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
        /// 对消防报警参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(Option);
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
        /// 对消防报警参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Option = databuf.ReadByte();
        }
    }
}