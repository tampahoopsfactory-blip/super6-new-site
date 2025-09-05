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
    /// 表示人脸机活体检测的命令参数
    /// </summary>
    public class WriteFaceBioassay_Parameter : AbstractParameter
    {
        /// <summary>
        /// 活体检测类型：0--关闭；1--红外探测；2--红外+彩色；
        /// </summary>
        public int BioassayType;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteFaceBioassay_Parameter() { BioassayType = 1; }

        /// <summary>
        /// 创建人脸机活体检测的命令参数
        /// </summary>
        /// <param name="iType">活体检测类型：0--关闭；1--红外探测；2--红外+彩色</param>
        public WriteFaceBioassay_Parameter(int iType)
        {
            BioassayType = iType;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (BioassayType <0 || BioassayType > 2)
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
            databuf.WriteByte(BioassayType);
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
            BioassayType = databuf.ReadByte();
        }
    }
}
