using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置记录存储方式_参数
    /// </summary>
    public class WriteRecordMode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 记录存储方式（0 - 满循环、1 - 满不循环）
        /// </summary>
        public byte Mode;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteRecordMode_Parameter() { }

        /// <summary>
        /// 使用记录存储方式初始化实例
        /// </summary>
        /// <param name="_Mode">记录存储方式</param>
        public WriteRecordMode_Parameter(byte _Mode)
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
            if (Mode != 0 && Mode != 1)
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
        /// 对记录存储方式参数进行编码
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
        /// 对记录存储方式参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Mode = databuf.ReadByte();
        }
    }
}