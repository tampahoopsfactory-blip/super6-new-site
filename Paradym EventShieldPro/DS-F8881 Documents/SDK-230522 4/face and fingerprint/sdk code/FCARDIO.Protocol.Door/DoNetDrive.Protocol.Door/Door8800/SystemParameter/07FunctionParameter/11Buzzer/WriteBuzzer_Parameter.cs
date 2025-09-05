using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置主板蜂鸣器_参数
    /// </summary>
    public class WriteBuzzer_Parameter : AbstractParameter
    {
        /// <summary>
        /// 主板蜂鸣器（0不启用，1启用）
        /// </summary>
        public byte Buzzer;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteBuzzer_Parameter() { }

        /// <summary>
        /// 使用主板蜂鸣器初始化实例
        /// </summary>
        /// <param name="_Buzzer">主板蜂鸣器</param>
        public WriteBuzzer_Parameter(byte _Buzzer)
        {
            Buzzer = _Buzzer;
            if (!checkedParameter())
            {
                throw new ArgumentException("Buzzer Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Buzzer != 0 && Buzzer != 1)
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
            return databuf.WriteByte(Buzzer);
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
            Buzzer = databuf.ReadByte();
        }
    }
}