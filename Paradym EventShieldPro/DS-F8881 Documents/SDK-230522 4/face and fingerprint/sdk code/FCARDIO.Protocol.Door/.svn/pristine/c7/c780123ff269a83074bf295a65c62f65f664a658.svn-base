using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.Alarm
{
    /// <summary>
    /// 解除报警_参数
    /// </summary>
    public class CloseAlarm_Parameter : AbstractParameter
    {
        /// <summary>
        /// 需要解除报警的门，取值范围：1-4
        /// </summary>
        public byte Door;

        /// <summary>
        /// 需要解除的报警类型（Bit0 - 非法卡报警、Bit1 - 门磁报警、Bit2 - 胁迫报警、Bit3 - 开门超时报警、Bit4 - 黑名单报警、Bit5 - 匪警报警、Bit6 - 防盗主机报警、Bit7 - 消防报警、Bit8 - 烟雾报警、Bit9 - 关闭电锁出错报警、Bit10 - 防拆报警、Bit11 - 强制关锁报警、Bit12 - 强制开锁报警）   注：9-12 为一体锁或一体机报警类型
        /// </summary>
        public ushort Alarm;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public CloseAlarm_Parameter() { }

        /// <summary>
        /// 使用解除报警参数初始化实例
        /// </summary>
        /// <param name="_Door">需要解除报警的门</param>
        /// <param name="_Alarm">需要解除的报警类型</param>
        public CloseAlarm_Parameter(byte _Door, ushort _Alarm)
        {
            Door = _Door;
            Alarm = _Alarm;
            if (!checkedParameter())
            {
                throw new ArgumentException("Alarm Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Door != 1 && Door != 2 && Door != 3 && Door != 4 && Door != 255)
            {
                throw new ArgumentException("Door Error");
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
        /// 对解除报警参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Door);
            databuf.WriteUnsignedShort(Alarm);

            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x03;
        }

        /// <summary>
        /// 对解除报警参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Door = databuf.ReadByte();
            Alarm = databuf.ReadUnsignedShort();
        }
    }
}