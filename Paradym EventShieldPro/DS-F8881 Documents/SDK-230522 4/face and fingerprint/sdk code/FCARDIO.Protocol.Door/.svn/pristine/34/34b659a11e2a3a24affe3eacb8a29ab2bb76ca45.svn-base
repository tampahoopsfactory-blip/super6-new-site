using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.Buzzer
{
    /// <summary>
    /// 控制蜂鸣器 参数
    /// </summary>
    public class WriteBuzzer_Parameter : AbstractParameter
    {
        /// <summary>
        /// 蜂鸣器鸣叫次数
        /// 1-10
        /// </summary>
        public int Times;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="times">蜂鸣器鸣叫次数</param>
        public WriteBuzzer_Parameter(int times)
        {
            Times = times;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Times < 1 || Times > 10)
                throw new ArgumentException("Times Error!");
            return true;
        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Times);
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
            Times = databuf.ReadByte();
        }
    }
}
