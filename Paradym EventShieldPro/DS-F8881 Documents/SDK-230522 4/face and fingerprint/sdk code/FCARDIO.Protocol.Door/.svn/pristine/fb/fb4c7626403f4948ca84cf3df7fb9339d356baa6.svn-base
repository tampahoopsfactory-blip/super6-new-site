using System;
using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 写入人脸机体温数值显示开关参数
    /// </summary>
    public class WriteFaceBodyTemperatureShowPar_Parameter : AbstractParameter
    {
        /// <summary>
        /// 人脸机体温数值显示开关 0--禁止显示体温；1--显示体温信息
        /// </summary>
        public int IsShow;

        /// <summary>
        /// 构建一个人脸机体温数值显示开关参数的实例
        /// </summary>
        public WriteFaceBodyTemperatureShowPar_Parameter() { IsShow = 375; }

        /// <summary>
        /// 创建人脸机体温数值显示开关的命令参数
        /// </summary>
        /// <param name="iShow">人脸机体温数值显示开关  0--禁止显示体温；1--显示体温信息</param>
        public WriteFaceBodyTemperatureShowPar_Parameter(int iShow)
        {
            IsShow = iShow;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (IsShow < 0 || IsShow > 1)
            {
                IsShow = 1;
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

            databuf.WriteByte(IsShow);
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
            IsShow = databuf.ReadByte();
        }
    }
}
