using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace FCARDIO.Protocol.Door.FC8800.SystemParameter.CardDeadlineTipDay
{
    /// <summary>
    /// 设置有效期即将过期提醒时间_参数
    /// </summary>
    public class WriteCardDeadlineTipDay_Parameter : AbstractParameter
    {
        /// <summary>
        /// 有效期即将过期提醒时间（取值范围: 1-255。0--表示关闭;默认值是30天）
        /// </summary>
        public byte Day;

        public WriteCardDeadlineTipDay_Parameter(byte _Day)
        {
            Day = _Day;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Day < 1 || Day > 255)
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
        /// 编码参数
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(Day);
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
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Day = databuf.ReadByte();
        }
    }
}