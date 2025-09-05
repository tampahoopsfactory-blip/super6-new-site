using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置感光模式的参数
    /// </summary>
    public class WriteLightPattern_Parameter : AbstractParameter
    {
        /// <summary>
        /// 感光模式；1、标准  默认值；2、增亮;3、减暗；
        /// </summary>
        public int LightPattern;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteLightPattern_Parameter() { LightPattern = 1; }

        /// <summary>
        /// 创建设置感光模式的参数
        /// </summary>
        /// <param name="iMode">感光模式 1--标准;2--增亮 ;3--减暗</param>
        public WriteLightPattern_Parameter(int iMode)
        {
            LightPattern = iMode;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (LightPattern <= 0 || LightPattern >= 4)
            {
                return false;
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
            databuf.WriteByte(LightPattern);

            return databuf;
        }

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            LightPattern = databuf.ReadByte();
        }
    }
}
