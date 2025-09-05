using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using System;
using DoNetDrive.Common.Extensions;

namespace DoNetDrive.Protocol.USB.CardReader.SystemParameter.AgencyCode
{
    /// <summary>
    /// 设置经销商代码
    /// </summary>
    public class WriteAgencyCode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 脱机门禁开门卡用的经销商代码
        /// 长度8
        /// </summary>
        public string Code;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteAgencyCode_Parameter()
        {

        }
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="code">脱机门禁开门卡用的经销商代码</param>
        public WriteAgencyCode_Parameter(string code)
        {
            Code = code;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Code != "" && Code.Length > 8)
                throw new ArgumentException("Code Error!");

            if (!Code.IsHex())
            {
                throw new ArgumentException("Code Error!");
            }

            return true;
        }



        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            Code = StringUtil.FillHexString(Code, 8, "F", true);
            StringUtil.HextoByteBuf(Code, databuf);
            return databuf;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 4;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Code = StringUtil.ByteBufToHex(databuf, 4);
        }
    }
}
