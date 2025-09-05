using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;

namespace DoNetDrive.Protocol.Door.Door8800.Door.PushButtonSetting
{
    /// <summary>
    /// 出门开关 参数
    /// </summary>
    public class WritePushButtonSetting_Parameter
        : AbstractParameter
    {
        /// <summary>
        /// 门号
        /// 门端口在控制版中的索引号，取值：1-4
        /// </summary>
        public int DoorNum;

        /// <summary>
        /// 是否启用出门按钮功能
        /// </summary>
        public bool Use;

        /// <summary>
        /// 是否启用出门按钮常开功能
        /// 出门按钮按下5秒后门进入常开状态，再次按5秒退出常开
        /// </summary>
        public bool NormallyOpen;

        /// <summary>
        /// 门工作方式时段
        /// </summary>
        public WeekTimeGroup weekTimeGroup;

        /// <summary>
        /// 提供给PushButtonSetting_Result使用
        /// </summary>
        public WritePushButtonSetting_Parameter()
        {
            weekTimeGroup = new WeekTimeGroup(8);
        }

        /// <summary>
        /// 创建结构，并传入门号和是否开启此功能
        /// </summary>
        /// <param name="door">门号</param>
        /// <param name="use">是否开启此功能</param>
        /// <param name="normallyOpen">是否启用出门按钮常开功能</param>
        /// <param name="tg">开门时段</param>
        public WritePushButtonSetting_Parameter(byte door, bool use, bool normallyOpen, WeekTimeGroup tg)
        {
            DoorNum = door;
            Use = use;
            NormallyOpen = normallyOpen;
            //时间
            weekTimeGroup = tg;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DoorNum < 1 || DoorNum > 4)
                throw new ArgumentException("Door Error!");
            if (weekTimeGroup == null)
            {
                throw new ArgumentException("weekTimeGroup Error!");
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
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != 3)
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteByte(DoorNum);
            databuf.WriteBoolean(Use);
            databuf.WriteBoolean(NormallyOpen);
            //时间？？？
            weekTimeGroup.GetBytes(databuf);

            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 3;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (weekTimeGroup == null)
            {
                weekTimeGroup = new WeekTimeGroup(8);
            }
            if (databuf.ReadableBytes != 227)
            {
                throw new ArgumentException("databuf Error");
            }
            DoorNum = databuf.ReadByte();
            Use = databuf.ReadBoolean();
            NormallyOpen = databuf.ReadBoolean();
            weekTimeGroup.SetBytes(databuf);
           
        }
    }
}
