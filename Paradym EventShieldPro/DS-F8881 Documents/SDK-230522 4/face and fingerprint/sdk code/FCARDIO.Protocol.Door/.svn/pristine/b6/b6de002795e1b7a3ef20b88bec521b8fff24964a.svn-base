using System;
using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入口罩识别开关参数
    /// </summary>
    public class WriteFaceMouthmufflePar_Parameter : AbstractParameter
    {
        /// <summary>
        /// 口罩识别开关 0--禁止；1--启用
        /// </summary>
        public int Mouthmuffle;

        /// <summary>
        /// 构建一个口罩识别开关参数的实例
        /// </summary>
        public WriteFaceMouthmufflePar_Parameter() { Mouthmuffle = 2; }

        /// <summary>
        /// 创建口罩识别开关的命令参数
        /// </summary>
        /// <param name="iMouthmuffle">口罩识别开关  0--禁止；1--启用</param>
        public WriteFaceMouthmufflePar_Parameter(int iMouthmuffle)
        {
            Mouthmuffle = iMouthmuffle;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Mouthmuffle < 0 || Mouthmuffle > 1)
            {
                Mouthmuffle = 1;
            }

            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 编码参数
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(Mouthmuffle);
            return databuf;
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != 1)
            {
                throw new ArgumentException("databuf Error");
            }
            Mouthmuffle = databuf.ReadByte();
        }
    }
}
