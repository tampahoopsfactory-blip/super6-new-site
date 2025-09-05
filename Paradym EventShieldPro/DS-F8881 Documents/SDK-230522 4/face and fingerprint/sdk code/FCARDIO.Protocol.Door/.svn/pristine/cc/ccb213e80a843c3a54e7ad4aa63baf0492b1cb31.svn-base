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
    /// 表示人脸机识别距离的命令参数
    /// </summary>
    public class WriteFaceIdentifyRange_Parameter : AbstractParameter
    {
        /// <summary>
        /// 识别距离类型：1--近距离（0.2-0.5米）；2--中距离（0.2-1.5米）；3--远距离（0.2-1.5米以上）
        /// </summary>
        public int IdentifyRange;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteFaceIdentifyRange_Parameter() { IdentifyRange = 1; }

        /// <summary>
        /// 创建人脸机识别距离的命令参数
        /// </summary>
        /// <param name="iRange">识别距离：1--近距离（0.2-0.5米）；2--中距离（0.2-1.5米）；3--远距离（0.2-1.5米以上）</param>
        public WriteFaceIdentifyRange_Parameter(int iRange)
        {
            IdentifyRange = iRange;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (IdentifyRange <1 || IdentifyRange > 3)
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
            databuf.WriteByte(IdentifyRange);
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
            IdentifyRange = databuf.ReadByte();
        }
    }
}
