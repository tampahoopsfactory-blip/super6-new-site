using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.FirstCardOpen
{
    /// <summary>
    /// 设置首卡开门参数
    /// </summary>
    public class WriteFirstCardOpen_Parameter : AbstractParameter
    {

        /// <summary>
        ///  门索引号
        ///  取值范围 1-65
        /// </summary>
        public int Door;

        /// <summary>
        /// 刷卡或卡加密码：0表示非首卡时段不允许刷卡或卡加密码通行，1表示允许
        /// </summary>
        public bool IsCardPassword;
        /// <summary>
        /// 密码：0表示非首卡时段不允许输入密码通行，1表示允许
        /// </summary>
        public bool IsPassword;


        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteFirstCardOpen_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="door">门索引号</param>
        /// <param name="isCardPassword">刷卡或卡加密码</param>
        /// <param name="isPassword">密码</param>
        public WriteFirstCardOpen_Parameter(int door,bool isCardPassword, bool isPassword)
        {
            Door = door;
            IsCardPassword = isCardPassword;
            IsPassword = isPassword;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Door < 1 || Door > 65)
                throw new ArgumentException("Door Error!");
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
        /// 对参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Door);
            databuf.WriteBoolean(IsCardPassword);
            databuf.WriteBoolean(IsPassword);
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
        /// 对参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Door = databuf.ReadByte();
            IsCardPassword = databuf.ReadBoolean();
            IsPassword = databuf.ReadBoolean();
        }
    }

}