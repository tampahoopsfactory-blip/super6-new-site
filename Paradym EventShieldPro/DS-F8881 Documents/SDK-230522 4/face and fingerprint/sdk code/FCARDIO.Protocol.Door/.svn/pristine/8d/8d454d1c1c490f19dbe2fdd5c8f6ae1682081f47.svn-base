using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.OEM
{
    public class OEM_Parameter : AbstractParameter

    {
        /// <summary>
        /// OEM详情实例
        /// </summary>
        public OEMDetail Detail;

        /// <summary>
        /// 使用一个包含OEM详情的实例初始化类
        /// </summary>
        /// <param name="detail">OEM详情的实例</param>
        public OEM_Parameter(OEMDetail detail)
        {
            Detail = detail;
            if (!checkedParameter())
            {
                throw new ArgumentException("SN Error");
            }
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns>true 表示参数符合规则，false 表示参数不符合规则</returns>
        public override bool checkedParameter()
        {
            if (Detail == null)
            {
                return false;
            }
            return true;
        }




        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            Detail = null;
            return;
        }

        /// <summary>
        /// 对SN参数进行编码
        /// </summary>
        /// <param name="databuf">需要填充参数结构的字节缓冲区</param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            Detail.GetBytes(databuf);
            return databuf;
        }

        /// <summary>
        /// 获取参数结构长度
        /// </summary>
        /// <returns>结构长度</returns>
        public override int GetDataLen()
        {
            return 127;
        }

        /// <summary>
        /// 对SN参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Detail.SetBytes(databuf);
        }
    }
}
