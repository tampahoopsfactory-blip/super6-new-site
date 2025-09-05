using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.LED
{
    /// <summary>
    /// 控制LED灯
    /// </summary>
    public class WriteLED_Parameter : AbstractParameter
    {
        /// <summary>
        /// 1表示红灯亮
        /// 2表示绿灯亮
        /// </summary>
        public int Code;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="code"></param>
        public WriteLED_Parameter(byte code)
        {
            Code = code;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Code < 1 || Code > 2)
                throw new ArgumentException("Code Error!");
            return true;
        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Code);
            return databuf;
        }

        /// <summary>
        /// 获取长度
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
            Code = databuf.ReadByte();
        }
    }
}