using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 补光灯模式参数
    /// </summary>
    public class WriteFaceLEDMode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 补光灯模式  1--一直亮；2--检测到人员时开；0--一直关
        /// </summary>
        public int LEDMode;

        /// <summary>
        /// 构建一个补光灯模式参数的实例
        /// </summary>
        public WriteFaceLEDMode_Parameter() { LEDMode = 2; }

        /// <summary>
        /// 创建补光灯模式的命令参数
        /// </summary>
        /// <param name="iLEDMode">补光灯模式  1--一直亮；2--检测到人员时开；0--一直关</param>
        public WriteFaceLEDMode_Parameter(int iLEDMode)
        {
            LEDMode = iLEDMode;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (LEDMode < 0 || LEDMode > 2)
            {
                LEDMode = 2;
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
            databuf.WriteByte(LEDMode);
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
            LEDMode = databuf.ReadByte();
        }
    }
}
