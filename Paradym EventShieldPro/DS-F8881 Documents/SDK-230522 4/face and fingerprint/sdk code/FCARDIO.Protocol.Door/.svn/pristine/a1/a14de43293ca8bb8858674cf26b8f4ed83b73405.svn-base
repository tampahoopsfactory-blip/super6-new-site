using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.FunctionParameter.ReaderWorkSetting
{
    /// <summary>
    /// 读卡认证方式
    /// </summary>
    public class WriteReaderWorkSetting_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门
        /// </summary>
        public byte Door;

        /// <summary>
        /// 门认证方式时段
        /// </summary>
        public WeekTimeGroup_ReaderWork weekTimeGroup_ReaderWork;

        /// <summary>
        /// 提供给 ReaderWorkSetting_Result 使用
        /// </summary>
        public WriteReaderWorkSetting_Parameter() {
            weekTimeGroup_ReaderWork = new WeekTimeGroup_ReaderWork(8);
        }

        /// <summary>
        /// 门读卡认证方式参数初始化实例
        /// </summary>
        /// <param name="iDoor">门</param>
        /// <param name="tg">门认证方式时段</param>
        public WriteReaderWorkSetting_Parameter(WeekTimeGroup_ReaderWork tg)
        {
            weekTimeGroup_ReaderWork = tg;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {

            if (weekTimeGroup_ReaderWork == null)
                throw new ArgumentException("WeekTimeGroup_ReaderWork Is Null!");
            
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            weekTimeGroup_ReaderWork = null;
        }

        /// <summary>
        /// 对门认证方式参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteByte(Door);
            weekTimeGroup_ReaderWork.GetBytes(databuf);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x118;
        }

        /// <summary>
        /// 对门认证方式参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (weekTimeGroup_ReaderWork == null)
            {
                weekTimeGroup_ReaderWork = new WeekTimeGroup_ReaderWork(8);
            }
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            weekTimeGroup_ReaderWork.SetBytes(databuf);
        }
    }
}
