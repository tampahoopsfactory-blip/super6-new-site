using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置防潜回模式_参数
    /// </summary>
    public class WriteCheckInOut_Parameter : AbstractParameter
    {
        /// <summary>
        /// 防潜回模式（01--单独每个门检测防潜回；02--整个控制器统一防潜回）
        /// </summary>
        public byte Mode;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteCheckInOut_Parameter() { }

        /// <summary>
        /// 使用防潜回模式初始化实例
        /// </summary>
        /// <param name="_Mode">防潜回模式</param>
        public WriteCheckInOut_Parameter(byte _Mode)
        {
            Mode = _Mode;
            if (!checkedParameter())
            {
                throw new ArgumentException("Mode Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Mode != 1 && Mode != 2)
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
        /// 对防潜回模式参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(Mode);
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
        /// 对防潜回模式参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Mode = databuf.ReadByte();
        }
    }
}