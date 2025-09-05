using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;


namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 设置定时开门参数
    /// </summary>
    public class WriteTimingOpen_Parameter : AbstractParameter
    {
        /// <summary>
        /// 端口号
        /// </summary>
        public byte Port;

        /// <summary>
        /// 是否启用
        /// 1--启用;0--禁用
        /// </summary>
        public byte Use;

        /// <summary>
        /// 常开工作模式
        /// 1--合法认证通过后在指定时段内即可常开
        /// 2---授权中标记为常开特权的在指定时段内认证通过即可常开
        /// 3--自动开关，到时间自动开关门。
        /// </summary>
        public byte WorkType;

        /// <summary>
        /// 定时常开时段
        /// </summary>
        public WeekTimeGroup WeekTimeGroup;

        /// <summary>
        /// 创建设置定时常开参数命令的返回值
        /// </summary>
        public WriteTimingOpen_Parameter()
        {
            WeekTimeGroup = new WeekTimeGroup(8);
        }

        /// <summary>
        /// 检查参数是否合法
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Port < 1 || Port > 64)
                return false;
            if (Use < 0 || Use > 1)
                return false;

            if (WorkType < 1 || WorkType > 3)
                return false;

            if (WeekTimeGroup == null)
                return false;
            return true;
        }

        /// <summary>
        /// 返回实体二进制序列化后的长度
        /// </summary>
        /// <returns>长度</returns>
        public override int GetDataLen()
        {
            return 0xE3;
        }

        /// <summary>
        /// 对定时常开参数命令的返回值进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes < GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            if (WeekTimeGroup == null)
            {
                WeekTimeGroup = new WeekTimeGroup(8);
            }

            Port = databuf.ReadByte();
            Use = databuf.ReadByte();
            WorkType = databuf.ReadByte();
            WeekTimeGroup.SetBytes(databuf);
        }


        /// <summary>
        /// 对定时常开参数命令的返回值进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < GetDataLen())
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteByte(Port);
            databuf.WriteByte(Use);
            databuf.WriteByte(WorkType);
            WeekTimeGroup.GetBytes(databuf);

            return databuf;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }
    }
}
