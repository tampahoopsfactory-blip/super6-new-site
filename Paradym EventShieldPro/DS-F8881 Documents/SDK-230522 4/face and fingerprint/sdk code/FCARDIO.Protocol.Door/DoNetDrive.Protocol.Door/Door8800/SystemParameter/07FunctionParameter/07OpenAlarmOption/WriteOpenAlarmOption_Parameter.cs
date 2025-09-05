using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置匪警报警参数_参数
    /// </summary>
    public class WriteOpenAlarmOption_Parameter : AbstractParameter
    {
        /// <summary>
        /// 匪警报警参数（0 - 关闭此功能、1 - 所有门锁定，报警输出，蜂鸣器不响。不开门，刷卡不能解除，软件解除，解除报警后门的锁定也解锁了、2 - 报警输出，不锁定，蜂鸣器响。不开门，刷卡可以解除，软件可以解除、3 - 按报警按钮就报警，门锁定，并输出，不按时就停止。不开门，按钮停止时就解除，软件或刷卡不能解除。按报警按钮的时候门是处于锁定状态的，不按时解除锁定状态）
        /// </summary>
        public byte Option;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteOpenAlarmOption_Parameter() { }

        /// <summary>
        /// 使用匪警报警参数初始化实例
        /// </summary>
        /// <param name="_Option">匪警报警参数</param>
        public WriteOpenAlarmOption_Parameter(byte _Option)
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
        /// 对匪警报警参数进行编码
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
        /// 对匪警报警参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Option = databuf.ReadByte();
        }
    }
}