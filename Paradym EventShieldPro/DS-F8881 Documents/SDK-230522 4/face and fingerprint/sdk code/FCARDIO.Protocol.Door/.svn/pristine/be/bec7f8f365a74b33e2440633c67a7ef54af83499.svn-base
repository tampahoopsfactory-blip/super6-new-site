using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置人脸机活体检测阈值的命令参数
    /// </summary>
    public class WriteFaceBioassaySimilarity_Parameter : AbstractParameter
    {
        /// <summary>
        /// 活体检测阈值 1-99
        /// </summary>
        public int Similarity { get; set; }

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteFaceBioassaySimilarity_Parameter() { Similarity = 60; }

        /// <summary>
        /// 创建设置人脸机活体检测阈值的命令参数
        /// </summary>
        /// <param name="value">活体检测阈值 1-99</param>
        public WriteFaceBioassaySimilarity_Parameter(int value)
        {
            Similarity = value;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Similarity < 0 || Similarity > 100)
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
            databuf.WriteByte(Similarity);
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
            Similarity = databuf.ReadByte();
        }
    }
}
