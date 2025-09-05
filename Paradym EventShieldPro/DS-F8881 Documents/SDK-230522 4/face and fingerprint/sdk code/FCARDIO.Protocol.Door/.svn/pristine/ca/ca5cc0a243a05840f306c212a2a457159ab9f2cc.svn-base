using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 查询人员详情
    /// </summary>
    public class ReadPersonDetail_Parameter : AbstractParameter
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public uint UserCode;

        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="carddata">需要读取用户号</param>
        public ReadPersonDetail_Parameter(uint userCode)
        {
            UserCode = userCode;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (UserCode == 0)
            {
                throw new ArgumentException("UserCode Error!");
            }
            

            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 将结构编码为 字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < 4)
            {
                throw new ArgumentException("UserCode Error");
            }
            databuf.WriteInt((int)UserCode);
            return databuf;
        }

        /// <summary>
        /// 指定此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 4;
        }

        /// <summary>
        /// 未实现
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }
    }
}
