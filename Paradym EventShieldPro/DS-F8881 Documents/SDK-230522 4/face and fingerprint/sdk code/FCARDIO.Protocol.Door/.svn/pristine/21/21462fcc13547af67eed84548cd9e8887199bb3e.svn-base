using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.BalcklistAlarmOption
{
    /// <summary>
    /// 设置黑名单报警功能开关_参数
    /// </summary>
    public class WriteBalcklistAlarmOption_Parameter : AbstractParameter
    {
        /// <summary>
        /// 黑名单报警功能开关（0 - 关、1 - 开）
        /// </summary>
        public byte Use;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteBalcklistAlarmOption_Parameter() { }

        /// <summary>
        /// 使用黑名单报警功能开关参数初始化实例
        /// </summary>
        /// <param name="_Use">黑名单报警功能开关参数</param>
        public WriteBalcklistAlarmOption_Parameter(byte _Use)
        {
            Use = _Use;
            if (!checkedParameter())
            {
                throw new ArgumentException("Use Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Use != 0 && Use != 1)
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
        /// 对黑名单报警功能开关参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(Use);
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
        /// 对黑名单报警功能开关参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Use = databuf.ReadByte();
        }
    }
}