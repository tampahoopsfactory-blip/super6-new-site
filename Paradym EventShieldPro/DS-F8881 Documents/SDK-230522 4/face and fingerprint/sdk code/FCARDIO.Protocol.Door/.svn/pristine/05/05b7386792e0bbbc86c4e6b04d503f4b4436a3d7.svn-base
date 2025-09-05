using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.Menu
{
    /// <summary>
    /// 读取单个菜单命令
    /// </summary>
    public class ReadMenuDetail_Parameter : AbstractParameter
    {
        public int MenuNum;

        public ReadMenuDetail_Parameter(int MenuNum)
        {
            this.MenuNum = MenuNum;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (MenuNum <= 0)
            {
                throw new ArgumentException("MenuNum Error!");
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
        /// 将结构编码为 字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != 4)
            {
                throw new ArgumentException("Card Error");
            }
            databuf.WriteInt(MenuNum);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 4;
        }

        /// <summary>
        /// 未实现
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }
    }
}
