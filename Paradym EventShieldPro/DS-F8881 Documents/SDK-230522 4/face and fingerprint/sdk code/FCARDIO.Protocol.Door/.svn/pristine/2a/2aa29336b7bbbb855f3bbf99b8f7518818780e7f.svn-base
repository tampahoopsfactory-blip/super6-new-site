using System;
using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;


namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入体温报警阈值参数
    /// </summary>
    public class WriteFaceBodyTemperatureAlarmPar_Parameter : AbstractParameter
    {
        /// <summary>
        /// 体温报警阈值  350-500  稳定用整形存储，显示时除10，例如37.5，存储为375
        /// </summary>
        public int AlarmPar;

        /// <summary>
        /// 构建一个体温报警阈值参数的实例
        /// </summary>
        public WriteFaceBodyTemperatureAlarmPar_Parameter() { AlarmPar = 375; }

        /// <summary>
        /// 创建体温报警阈值的命令参数
        /// </summary>
        /// <param name="iAlarmPar">体温报警阈值  350-500  稳定用整形存储，显示时除10，例如37.5，存储为375</param>
        public WriteFaceBodyTemperatureAlarmPar_Parameter(int iAlarmPar)
        {
            AlarmPar = iAlarmPar;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (AlarmPar < 350 || AlarmPar > 500)
            {
                AlarmPar = 375;
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

            databuf.WriteShort(AlarmPar);
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
            AlarmPar = databuf.ReadShort();
        }
    }
}
