using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.CacheContent
{
    /// <summary>
    /// 缓存区内容操作
    /// </summary>
    public class CacheContent_Parameter : AbstractParameter
    {
        /// <summary>
        /// 缓存区内容
        /// </summary>
        public string CacheContent;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public CacheContent_Parameter() { }

        /// <summary>
        /// 使用缓存区内容初始化实例
        /// </summary>
        /// <param name="sCacheContent">缓存区内容</param>
        public CacheContent_Parameter(string sCacheContent)
        {
            CacheContent = sCacheContent;
            if (!checkedParameter())
            {
                throw new ArgumentException("CacheContent Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (CacheContent == null)
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
            return 0x1E;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 对缓存区内容进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            var bbuf = System.Text.ASCIIEncoding.ASCII.GetBytes(CacheContent);
            int iLen = 0x1E;
            if (bbuf.Length < iLen) iLen = bbuf.Length;
            databuf.WriteBytes(bbuf, 0, iLen);

            iLen = 0x1e - iLen;
            for (int i = 0; i < iLen; i++)
            {
                databuf.WriteByte(0);
            }

            return databuf;
        }

        /// <summary>
        /// 对缓存区内容进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            int iLen = 0x1E;
            for (int i = 0; i < 0x1E; i++)
            {
                if (databuf.GetByte(i) == 0)
                {
                    iLen = i;
                    break;
                }
            }

            if (iLen > 0)
            {
                CacheContent = databuf.ReadString(iLen, System.Text.Encoding.ASCII);
            }
            else
            {
                CacheContent = string.Empty;
            }

        }
    }
}
