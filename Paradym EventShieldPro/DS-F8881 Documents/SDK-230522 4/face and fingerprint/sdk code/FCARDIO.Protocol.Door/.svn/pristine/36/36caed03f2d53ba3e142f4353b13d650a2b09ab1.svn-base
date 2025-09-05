using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Door.InvalidCardAlarmOption
{
    /// <summary>
    /// 非法读卡报警_参数
    /// </summary>
    public class WriteInvalidCardAlarmOption_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门号
        /// 门端口在控制板中的索引号，取值：1-4
        /// </summary>
        public int DoorNum;

        /// <summary>
        /// 是否报警功能
        /// </summary>
        public bool Use;

        /// <summary>
        /// 当读未注册卡到达一定次数后报警
        /// 0-255,  0--读一次卡就报警
        /// </summary>
        public byte ReadInvalidCardTime;

        /// <summary>
        /// 提供给 InvalidCardAlarmOption_Result 使用
        /// </summary>
        public WriteInvalidCardAlarmOption_Parameter() { }

        /// <summary>
        /// 非法读卡报警参数初始化实例
        /// </summary>
        /// <param name="door">门号</param>
        /// <param name="use">是否开启此功能</param>
        /// <param name="time">次数</param>
        public WriteInvalidCardAlarmOption_Parameter(byte door, bool use,byte time)
        {
            DoorNum = door;
            Use = use;
            ReadInvalidCardTime = time;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        public override bool checkedParameter()
        {
            if (DoorNum < 1 || DoorNum > 4 )
                throw new ArgumentException("door Error!");
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
        /// 对非法读卡报警参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteByte(DoorNum);
            databuf.WriteBoolean(Use);
            return databuf;
        }

        /// <summary>
        /// 指示此类结构编码为字节缓冲后的长度
        /// </summary>
        public override int GetDataLen()
        {
            return 0x02;
        }

        /// <summary>
        /// 对非法读卡报警参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            DoorNum = databuf.ReadByte();
            Use = databuf.ReadBoolean();
        }

        /// <summary>
        /// 对非法读卡报警参数进行解码
        /// <param name="databuf"></param>
        /// </summary>
        public void ReadInvalidCardTime_SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            DoorNum = databuf.ReadByte();
            ReadInvalidCardTime = databuf.ReadByte();
        }

        /// <summary>
        /// 对非法读卡报警参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public IByteBuffer ReadInvalidCardTime_GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteByte(DoorNum);
            databuf.WriteByte(ReadInvalidCardTime);
            return databuf;
        }
    }
}
