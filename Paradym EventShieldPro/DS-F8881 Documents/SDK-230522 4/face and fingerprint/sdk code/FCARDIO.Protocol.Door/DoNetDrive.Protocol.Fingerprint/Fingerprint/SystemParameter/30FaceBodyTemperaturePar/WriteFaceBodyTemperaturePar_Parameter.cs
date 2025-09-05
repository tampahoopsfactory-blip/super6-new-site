using System;
using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入体温检测开关及格式参数
    /// </summary>
    public class WriteFaceBodyTemperaturePar_Parameter : AbstractParameter
    {
        /// <summary>
        /// 体温检测开关及格式 0--禁止；1--摄氏度（默认值）；2--华氏度
        /// </summary>
        public int BodyTemperaturePar;

        /// <summary>
        /// 构建一个体温检测开关及格式参数的实例
        /// </summary>
        public WriteFaceBodyTemperaturePar_Parameter() { BodyTemperaturePar = 1; }

        /// <summary>
        /// 创建体温检测开关及格式的命令参数
        /// </summary>
        /// <param name="iBodyTemperaturePar">体温检测开关及格式  0--禁止；1--摄氏度（默认值）；2--华氏度</param>
        public WriteFaceBodyTemperaturePar_Parameter(int iBodyTemperaturePar)
        {
            BodyTemperaturePar = iBodyTemperaturePar;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (BodyTemperaturePar < 0 || BodyTemperaturePar > 2)
            {
                BodyTemperaturePar = 1;
            }

            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 2;
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
            if (BodyTemperaturePar == 0)
            {
                databuf.WriteByte(0);
                databuf.WriteByte(1);
            }
            else
            {
                databuf.WriteByte(1);
                databuf.WriteByte(BodyTemperaturePar);
            }
            return databuf;
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != 2)
            {
                throw new ArgumentException("databuf Error");
            }
            BodyTemperaturePar = databuf.ReadByte();
            if (BodyTemperaturePar != 0)
            {
                BodyTemperaturePar = databuf.ReadByte();
            }
        }
    }
}
