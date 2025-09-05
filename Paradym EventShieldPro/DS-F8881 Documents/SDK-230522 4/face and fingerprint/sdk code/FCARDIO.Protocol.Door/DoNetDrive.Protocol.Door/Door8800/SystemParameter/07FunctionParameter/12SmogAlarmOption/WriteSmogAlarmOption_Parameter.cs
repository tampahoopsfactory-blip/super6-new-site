using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置烟雾报警参数_参数
    /// </summary>
    public class WriteSmogAlarmOption_Parameter : AbstractParameter
    {
        /// <summary>
        /// 烟雾报警参数（0 - 关闭此功能（默认）、1 - 驱动 [烟雾报警继电器]，(信号有，就驱动的，信号无，就关闭)、2 - 驱动烟雾报警继电器并驱动所有门继电器，主板报警提示音响(开启后由软件关闭，或重启。)、3 - 驱动烟雾报警继电器并锁定所有门，主板报警提示音响(开启后由软件关闭，或重启。)）
        /// </summary>
        public byte Option;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteSmogAlarmOption_Parameter() { }

        /// <summary>
        /// 使用烟雾报警参数初始化实例
        /// </summary>
        /// <param name="_Option">烟雾报警参数</param>
        public WriteSmogAlarmOption_Parameter(byte _Option)
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
            if (Option != 0 && Option != 1 && Option != 2 && Option != 3)
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
        /// 对烟雾报警参数进行解码
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
        /// 对烟雾报警参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Option = databuf.ReadByte();
        }
    }
}