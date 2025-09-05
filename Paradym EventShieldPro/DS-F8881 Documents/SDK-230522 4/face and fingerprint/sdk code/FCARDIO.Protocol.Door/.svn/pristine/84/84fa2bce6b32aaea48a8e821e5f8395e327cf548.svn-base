using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Data;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.OEM
{
    public class OEMDetail : INData
    {
        public static Encoding StringEncoding = Encoding.BigEndianUnicode;

        /// <summary>
        /// 制造商名称
        /// </summary>
        public string Manufacturer;
        /// <summary>
        /// 网址
        /// </summary>
        public string WebAddr;

        /// <summary>
        /// 出厂日期
        /// </summary>
        public DateTime DeliveryDate;

        /// <summary>
        /// 获取OEM数据结构的字节长度
        /// </summary>
        /// <returns></returns>
        public int GetDataLen()
        {
            return 127;
        }

        /// <summary>
        /// 获取一个 ByteBuf 此 缓冲中包含了此数据结构的所有数据
        /// </summary>
        /// <returns></returns>
        public IByteBuffer GetBytes()
        {
            return GetBytes(DotNetty.Buffers.UnpooledByteBufferAllocator.Default.Buffer(127));
        }

        /// <summary>
        ///  将数据序列化到指定的 ByteBuf 中
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < 127)
            {
                throw new ArgumentException("databuf Size < 127");
            }
            Util.StringUtil.WriteString(databuf, Manufacturer, 60, StringEncoding);
            Util.StringUtil.WriteString(databuf, WebAddr, 60, StringEncoding);
            DoNetDrive.Protocol.Util.StringUtil.HextoByteBuf(DeliveryDate.ToString("yyyyMMddhhmmss"), databuf);
            return databuf;
        }

        


        /// <summary>
        /// 将buf中数据转换为实体
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes < 127)
            {
                throw new ArgumentException("databuf Size < 127");
            }
            Manufacturer = Util.StringUtil.GetString(databuf, 60, StringEncoding);
            WebAddr = Util.StringUtil.GetString(databuf, 60, StringEncoding);
            DeliveryDate = Util.TimeUtil.BCDTimeToDate_yyyyMMddhhmmss(databuf);
        }


    }
}
