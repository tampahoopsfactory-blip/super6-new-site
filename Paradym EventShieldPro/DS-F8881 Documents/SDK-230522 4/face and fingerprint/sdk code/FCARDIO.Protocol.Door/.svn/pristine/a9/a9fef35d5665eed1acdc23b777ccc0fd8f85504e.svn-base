using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Door
{
    /// <summary>
    /// 门号参数，取值范围 1-4
    /// </summary>
    public class DoorPort_Parameter : AbstractParameter
    {
        /// <summary>
        ///  门索引号
        ///  取值范围 1-4
        /// </summary>
        public int Door;

        /// <summary>
        /// 门号参数初始化实例
        /// </summary>
        /// <param name="iDoor"></param>
        public DoorPort_Parameter(int iDoor)
        {
            Door = iDoor;
            checkedParameter();
        }


        /// <summary>
        /// 检查参数的统一接口
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Door < 1 || Door > 4)
                throw new ArgumentException("Door Error!");
            return true;
        }


        /// <summary>
        /// 释放资源时由上层调用
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
            if(databuf.WritableBytes != 1)
            {
                return null;
            }
            databuf.WriteByte(Door);
            return databuf;
        }

        /// <summary>
        /// 指示此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Door=databuf.ReadByte();
        }
    }
}
